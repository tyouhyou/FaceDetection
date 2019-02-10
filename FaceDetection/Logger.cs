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
    static class Logger
    {
        private static string LogFile
        {
            set;
            get;
        }

        static Logger()
        {
            var dir = Directory.GetCurrentDirectory();
            LogFile = Path.Combine(dir, "log.txt");
        }

        public static void Log(string msg)
        {
            using (FileStream fs = new FileStream(LogFile, FileMode.OpenOrCreate | FileMode.Append))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " -> " + msg);
            }
        }
    }
}
