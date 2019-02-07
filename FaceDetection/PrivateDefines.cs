using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FaceDetection
{
    static class PrivateDefines
    {
        #region FACE API ENDPOINT

        private static readonly string DefineFile = @".\PrivateDefines.xml";

        private static readonly string XPath_SubscriptionKey = "/private/azureface/subscriptionkey";
        private static readonly string XPath_FaceEndPoint = "/private/azureface/endpoint";
        private static readonly string XPath_FaceHost = "/private/azureface/facehost";

        public static string FaceSubscriptionKey
        {
            get;
            private set;
        }

        public static string FaceEndPoint
        {
            get;
            private set;
        }

        public static string FaceHost
        {
            get;
            private set;
        }

        #endregion

        static PrivateDefines()
        {
            XDocument doc = XDocument.Load(DefineFile);
            FaceSubscriptionKey = doc.XPathSelectElement(XPath_SubscriptionKey).Value;
            FaceEndPoint = doc.XPathSelectElement(XPath_FaceEndPoint).Value;
            FaceHost = doc.XPathSelectElement(XPath_FaceHost).Value;
        }
    }
}
