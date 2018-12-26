using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetection
{
    class Facial
    {
        private static readonly FaceAttributeType[] FaceAttributes =
        {
            FaceAttributeType.Emotion,
        };

        private FaceClient Client
        {
            get;
            set;
        }

        public Facial()
        {
            Client = new FaceClient(new ApiKeyServiceClientCredentials(PrivateDefines.FaceSubscriptionKey),
                                    new DelegatingHandler[] { })
            {
                Endpoint = PrivateDefines.FaceEndPoint
            };
        }

        public async Task DetectLocalImage(string path)
        {
            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    IList<DetectedFace> faces = await Client.Face.DetectWithStreamAsync(stream, true, false, FaceAttributes);
                    ShowFaceResult(faces);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public async Task DetectRemoteImage(string url)
        {
            try
            {
                IList<DetectedFace>  faces = await Client.Face.DetectWithUrlAsync(url, true, false, FaceAttributes);
                ShowFaceResult(faces);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowFaceResult(IList<DetectedFace> faces)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var face in faces)
            {
                sb.AppendLine(string.Format("face id:  {0}", face.FaceId));
                sb.AppendLine();
                sb.AppendLine("emotion -->");
                var properties = face.FaceAttributes.Emotion.GetType().GetProperties();
                foreach (var property in properties)
                {
                    sb.AppendLine(string.Format("{0}: {1}", property.Name, (double)property.GetValue(face.FaceAttributes.Emotion)));
                }
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
