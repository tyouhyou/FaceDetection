using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Http;

namespace FaceDetection
{
    public class Ctrl
    {
        public void StartWebCam()
        {
            Facial face = new Facial();
            WebCam cam = new WebCam();
            cam.VideoCaptured += async (object sender, VideoCaptureEventArgs e) =>
            {
                await face.DetectImage(e.Frame);
            };
            cam.StartCapture();
        }

        public void DetectFace(string remoteImage, string localImage)
        {
            Facial face = new Facial();

            using (var dialog = new DialogInProcess())
            {
                dialog.Show();

                List<Task> Tasks = new List<Task>();
                Task tLocal;
                Task tRemote;

                if (!string.IsNullOrWhiteSpace(localImage))
                {
                    if (!File.Exists(localImage))
                    {
                        MessageBox.Show("The specified local image file does not exist.");
                    }
                    else
                    {
                        tLocal = face.DetectLocalImage(localImage);
                        Tasks.Add(tLocal);
                    }
                }

                if (!string.IsNullOrWhiteSpace(remoteImage))
                {
                    if (!Uri.IsWellFormedUriString(remoteImage, UriKind.Absolute))
                    {
                        MessageBox.Show("Invalid url.");
                    }
                    else
                    {
                        tRemote = face.DetectRemoteImage(remoteImage);
                        Tasks.Add(tRemote);
                    }
                }

                Task.WhenAll(Tasks.ToArray()).Wait(5000);
            }
        }

        public void Stop()
        {
            // TODO: if necessary
        }

    }
}
