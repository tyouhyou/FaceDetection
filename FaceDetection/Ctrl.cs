using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                try
                {
                    var rst = await face.DetectImage(e.Frame);
                    if (null != rst && 0 < rst.Count)
                    {
                        Logger.Log("Get Face Result and push to queue");
                        cam.PushFaceInQue(rst);
                    }
                }
                catch(Exception ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(e.ToString());
                }
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

        public void ArbitrarilyTest()
        {
            var regtext = new List<string>()
            {
                //@"[]", @"[ WARN:0] terminating async callback",
                //@"{ ""error"": { ""code"": ""RateLimitExceeded"", ""message"": ""Rate limit is exceeded. Try again later."" } }",
                //@"[{""faceId"":""8269e849-0e85-403a-a924-19ffda6f2b16"",""faceRectangle"":{""top"":188,""left"":383,""width"":160,""height"":160},""faceAttributes"":{""emotion"":{""anger"":0.0,""contempt"":0.011,""disgust"":0.0,""fear"":0.0,""happiness"":0.012,""neutral"":0.909,""sadness"":0.067,""surprise"":0.0}}}]",
                //@"[{""faceId"":""d60d3e72-586d-477d-a2e5-aa28c6b4021a"",""faceRectangle"":{""top"":196,""left"":310,""width"":133,""height"":133},""faceAttributes"":{""emotion"":{""anger"":0.0,""contempt"":0.129,""disgust"":0.0,""fear"":0.0,""happiness"":0.006,""neutral"":0.858,""sadness"":0.006,""surprise"":0.0}}},{""faceId"":""5edde257-ae82-454b-ac91-478cd8cbf209"",""faceRectangle"":{""top"":207,""left"":484,""width"":96,""height"":96},""faceAttributes"":{""emotion"":{""anger"":0.0,""contempt"":0.0,""disgust"":0.0,""fear"":0.0,""happiness"":0.0,""neutral"":0.951,""sadness"":0.049,""surprise"":0.0}}}]",
                @"[{""faceRectangle"":{""top"":205,""left"":374,""width"":176,""height"":176},""faceAttributes"":{""emotion"":{""anger"":0.0,""contempt"":0.001,""disgust"":0.0,""fear"":0.0,""happiness"":0.0,""neutral"":0.877,""sadness"":0.122,""surprise"":0.0}}}]"
            };

            foreach (var t in regtext)
            {
                // break point here
                var rst = FaceReponseParser.ParseViaRE(t);
                if (rst != null)
                    foreach(var r in rst)
                        Logger.Log("id : " + r.Id);
            }
        }

    }
}
