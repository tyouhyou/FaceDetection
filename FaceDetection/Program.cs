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

            Ctrl ctl = new Ctrl();
            FrmTest frm = new FrmTest(ctl);

            frm.FormClosing += (o, e) =>
            {
                ctl.Stop();
            };

            Application.Run(frm);
        }
    }
}
