using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FaceDetection
{
    class Facial
    {
        private static readonly FaceAttributeType[] FaceAttributes =
        {
            FaceAttributeType.Emotion,
        };

        private FaceClient FacialClient
        {
            get;
            set;
        }

        public Facial()
        {
            FacialClient = new FaceClient(new ApiKeyServiceClientCredentials(PrivateDefines.FaceSubscriptionKey),
                                    new DelegatingHandler[] { })
            {
                Endpoint = PrivateDefines.FaceHost
            };
        }

        public async Task<List<Face>> DetectImage(Image img)
        {
            List<Face> rst = null;

            var imageConverter = new ImageConverter();
            var imageData = (byte[])imageConverter.ConvertTo(img, typeof(byte[]));

            using (var httpClient = new HttpClient())
            using (var content = new ByteArrayContent(imageData))
            {
                content.Headers.Add("Ocp-Apim-Subscription-Key", PrivateDefines.FaceSubscriptionKey);
                content.Headers.Add("Content-Type", "application/octet-stream");

                var query = new QueryBuilder();
                query.Add("returnFaceAttributes", "emotion");
                query.Add("returnFaceId", "false");
                query.Add("returnFaceLandmarks", "false");

                var uri = PrivateDefines.FaceEndPoint + query.Get();;

                var response = await httpClient.PostAsync(uri, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Logger.Log(responseContent);

                rst = FaceReponseParser.ParseViaRE(responseContent);
            }

            return rst;
        }

        public async Task DetectLocalImage(string path)
        {
            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    IList<DetectedFace> faces = await FacialClient.Face.DetectWithStreamAsync(stream, true, false, FaceAttributes);
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
                IList<DetectedFace>  faces = await FacialClient.Face.DetectWithUrlAsync(url, true, false, FaceAttributes);
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
