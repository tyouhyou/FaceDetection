using System;
using System.Windows.Forms;

namespace FaceDetection
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                UnhandledException(s, (Exception)e.ExceptionObject);
            };

            Application.ThreadException += (s, e) =>
            {
                UnhandledException(s, e.Exception);
            };

            StartApp();
        }

        private static void UnhandledException(object sender, Exception e)
        {
            MessageBox.Show("Error Occurred! " + Environment.NewLine + e.ToString());
        }

        static void StartApp()
        {
            Ctrl ctl = new Ctrl();
            FrmTest frm = new FrmTest(ctl);

            frm.FormClosing += (o, e) =>
            {
                // TODO: anything needed
            };

            Application.Run(frm);
        }
    }
}
