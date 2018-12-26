using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace FaceDetection
{
    public class Ctrl
    {
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
