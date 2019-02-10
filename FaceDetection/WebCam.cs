using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;

namespace FaceDetection
{
    public class WebCam
    {
        private int Width
        {
            get;set;
        }

        private int Height
        {
            get;set;
        }

        private string WindowName
        {
            get;
            set;
        }

        private ConcurrentQueue<List<Face>> FaceQueue = new ConcurrentQueue<List<Face>>();
        
        public event EventHandler<VideoCaptureEventArgs> VideoCaptured;

        public WebCam(string windowName="WebCam", int width=800, int height=600)
        {
            WindowName = windowName;
            Width = width;
            Height = height;
        }

        public void ReSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void PushFaceInQue(List<Face> faces)
        {
            FaceQueue.Enqueue(faces);
        }

        public void StartCapture(int camNum = 0)
        {
            List<Face> faces = null;
            Scalar scRect = new Scalar(0, 0, 255);
            Scalar scText = new Scalar(255, 255, 255);

            Cv2.NamedWindow(WindowName, WindowMode.Normal);
            Cv2.ResizeWindow(WindowName, Width, Height);
            Cv2.StartWindowThread();

            VideoCapture videoCapture = OpenCvSharp.VideoCapture.FromCamera(camNum);
            videoCapture.Set(CaptureProperty.FrameWidth, Width);
            videoCapture.Set(CaptureProperty.FrameHeight, Height);

            var tm = DateTime.Now;
            DateTime tmP = new DateTime(1971,1,1);

            try
            {
                Mat frame = new Mat();
                
                bool success = true;
                while (success)
                {
                    var key = Cv2.WaitKey(50) & 0xFF;
                    if ((int)'q' == key || 27 == key)
                    {
                        break;
                    }

                    success = videoCapture.Read(frame);
                    if (!success)
                    {
                        break;
                    }

                    List<Face> fs = null;
                    if (FaceQueue.TryDequeue(out fs))
                    {
                        Logger.Log("Detection returns faces.");
                        faces = fs;
                    }

                    if (null != faces)
                    { 
                        foreach (var face in faces)
                        {
                            Cv2.Rectangle(frame, face.Frame.Rectangle, scRect);
                            Cv2.PutText(frame, "Happiness: " + face.Attributes.Emotion.Happiness, face.Frame.TopLeft, HersheyFonts.HersheyPlain, 1, scText);
                        }
                    }

                    Cv2.ImShow(WindowName, frame);

                    // To avoid the limit rate of Azure on Free Account, detect every 3 seconds. TODO: lift it
                    if (((tm = DateTime.Now) - tmP).TotalSeconds > 3 )
                    {
                        tmP = tm;
                        Logger.Log("On event video capturred");
                        OnVideoCaptured(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame));
                    }
                }
            }
            finally
            {
                videoCapture.Release();
                Cv2.DestroyAllWindows();
            }
        }

        protected void OnVideoCaptured(Image frame)
        {
            if (null != VideoCaptured)
            {
                VideoCaptured(this, new VideoCaptureEventArgs
                {
                    Frame = frame
                });
            }
        }
    }

    public class VideoCaptureEventArgs : EventArgs
    {
        public Image Frame { set; get; } 
    }
}
