using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace FaceDetection
{
    /// <summary>
    /// A simple log writer
    /// </summary>
    public class Logger
    {
        private static string LogFile
        {
            set;
            get;
        }

        static Logger()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            LogFile = Path.Combine(dir, "log.txt");
        }

        public static void Log(string msg)
        {
            using (StreamWriter sw = new StreamWriter(LogFile, true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " -> " + msg);
            }
        }
    }
}
