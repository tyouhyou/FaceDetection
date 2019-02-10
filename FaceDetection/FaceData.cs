using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using OpenCvSharp;

namespace FaceDetection
{
    /// <summary>
    /// Would regex be faster and more flexible than json.net ?
    /// </summary>
    public class FaceReponseParser
    {
        private static readonly string RE_RESP_1 = @"(\[|)\{(?<recd>.+)\},\{";
        private static readonly string RE_RESP_2 = @"(\[|)\{(?<recd>.+)\}\]";
        private static readonly string RE_FACE_ID = @"""faceId"":""(?<id>.+)""(,|$)";
        private static readonly string RE_FACE_RECT = @"""faceRectangle"":\{(?<rect>.+)\}+";
        private static readonly string RE_FACE_EMOT = @"""emotion"":\{(?<emot>.+)\}";

        private static readonly Regex re_resp_1 = new Regex(RE_RESP_1, RegexOptions.Compiled);
        private static readonly Regex re_resp_2 = new Regex(RE_RESP_2, RegexOptions.Compiled);
        private static readonly Regex re_id = new Regex(RE_FACE_ID, RegexOptions.Compiled);
        private static readonly Regex re_emot = new Regex(RE_FACE_EMOT, RegexOptions.Compiled);
        private static readonly Regex re_rect = new Regex(RE_FACE_RECT, RegexOptions.Compiled);

        public static List<Face> ParseViaRE(string json)
        {
            List<Face> rst = new List<Face>();
            
            int idx = 0;
            Match mf, mid, memot, mrect;
            while((mf = re_resp_1.Match(json, idx)).Success ||
                  (mf = re_resp_2.Match(json, idx)).Success)
            {
                var recd = mf.Groups["recd"].Value;
                if ((memot = re_emot.Match(recd)).Success &&
                    (mrect = re_rect.Match(recd)).Success)
                {
                    var id = string.Empty;
                    if ((mid = re_id.Match(recd)).Success)
                    {
                        id = mid.Groups["id"].Value;
                    }
                    var emotion = new FaceEmotion(memot.Groups["emot"].Value);
                    var rect = new FaceRectangle(mrect.Groups["rect"].Value);

                    rst.Add(new Face
                    {
                        Id = id
                        ,
                        Frame = rect
                        ,
                        Attributes = new FaceAttributes
                        {
                            Emotion = emotion
                        }
                    });
                }

                idx += mf.Value.Length;
                if (json.Length > idx)
                {
                    idx -= 1;
                }
            }
            
            return rst;
        }

        public static List<Face> ParseViaJS(string json)
        {
            List<Face> rst = null;
            try
            {
                rst = JsonConvert.DeserializeObject<List<Face>>(json);
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
            return rst;
        }
    }

    public class Face
    {
        [JsonProperty("faceId")]
        public string Id { set; get; }

        [JsonProperty("faceRectangle")]
        public FaceRectangle Frame { set; get; }

        [JsonProperty("faceAttributes")]
        public FaceAttributes Attributes { set; get; }
    }

    public class FaceRectangle
    {
        [JsonProperty("top")]
        public int Top { set; get; }

        [JsonProperty("left")]
        public int Left { set; get; }

        [JsonProperty("width")]
        public int Width { set; get; }

        [JsonProperty("height")]
        public int Height { set; get; }

        public Rect Rectangle { set; get; }

        public Point TopLeft { set; get; }
        
        private static readonly string RE_KV = @"""(?<key>\w+)"":(?<value>\d+)(,|\}|$)";
        private static readonly Regex re_kv = new Regex(RE_KV, RegexOptions.Compiled);

        /** would reflction be much slower than regex? */

        public FaceRectangle() { /* for json.net */ }

        public FaceRectangle(string values)
        {
            string key;
            int value;
            foreach (Match match in re_kv.Matches(values))
            {
                key = match.Groups["key"].Value;
                value = int.Parse(match.Groups["value"].Value);

                switch (key)
                {
                    case "top":
                        Top = value;
                        break;
                    case "left":
                        Left = value;
                        break;
                    case "width":
                        Width = value;
                        break;
                    case "height":
                        Height = value;
                        break;
                    default:
                        break;
                }
            }
            Rectangle = new Rect(Left, Top, Width, Height);
            TopLeft = new Point(Left, Top);
        }
    }

    public class FaceAttributes
    {
        [JsonProperty("emotion")]
        public FaceEmotion Emotion { set; get; }
    }

    public class FaceEmotion
    {
        [JsonProperty("anger")]
        public float Anger { set; get; }

        [JsonProperty("contemp")]
        public float Contempt { set; get; }

        [JsonProperty("disgust")]
        public float Disgust { set; get; }

        [JsonProperty("fear")]
        public float Fear { set; get; }

        [JsonProperty("happiness")]
        public float Happiness { set; get; }

        [JsonProperty("neural")]
        public float Neutral { set; get; }

        [JsonProperty("sadness")]
        public float Sadness { set; get; }

        [JsonProperty("surprise")]
        public float Surprise { set; get; }
        
        private static readonly string RE_KV = @"""(?<key>\w+)"":(?<value>[\d|\.]+)(,|\}|$)";
        private static readonly Regex re_kv = new Regex(RE_KV, RegexOptions.Compiled);

        public FaceEmotion() { /* for json.net */ }

        public FaceEmotion(string values)
        {
            /** would reflction be much slower than regex? */

            string key;
            float value;
            foreach (Match match in re_kv.Matches(values))
            {
                key = match.Groups["key"].Value;
                value = float.Parse(match.Groups["value"].Value);

                switch (key)
                {
                    case "anger":
                        Anger = value;
                        break;
                    case "contempt":
                        Contempt = value;
                        break;
                    case "disgust":
                        Disgust = value;
                        break;
                    case "fear":
                        Fear = value;
                        break;
                    case "happiness":
                        Happiness = value;
                        break;
                    case "neutral":
                        Neutral = value;
                        break;
                    case "sadness":
                        Sadness = value;
                        break;
                    case "surprise":
                        Surprise = value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
