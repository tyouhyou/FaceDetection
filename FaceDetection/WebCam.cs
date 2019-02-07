using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;

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

        public void StartCapture(int camNum = 0)
        {
            Cv2.NamedWindow(WindowName, WindowMode.Normal);
            Cv2.ResizeWindow(WindowName, Width, Height);

            VideoCapture videoCapture = OpenCvSharp.VideoCapture.FromCamera(camNum);
            videoCapture.Set(CaptureProperty.FrameWidth, Width);
            videoCapture.Set(CaptureProperty.FrameHeight, Height);

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

                    Cv2.ImShow(WindowName, frame);

                    OnVideoCaptured(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame));
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
