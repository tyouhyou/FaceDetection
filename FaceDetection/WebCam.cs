using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace FaceDetection
{
    public class WebCam
    {
        public void StartCapture(int camNum = 0)
        {
            VideoCapture videoCapture = OpenCvSharp.VideoCapture.FromCamera(camNum);
            Mat frame = new Mat();
            bool success = true;
            try
            {
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

                    Cv2.ImShow("Test", frame);
                }
            }
            finally
            {
                videoCapture.Release();
                Cv2.DestroyAllWindows();
            }
        }
    }
}
