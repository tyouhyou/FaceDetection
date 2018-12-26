using System;
using System.IO;
using System.Windows.Forms;

namespace FaceDetection
{
    public partial class Frm : Form
    {
        private Ctrl control;

        public Frm(Ctrl control)
        {
            InitializeComponent();

            this.control = control;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                string file;
                if (!string.IsNullOrWhiteSpace(txtLocalImg.Text) && File.Exists(file = txtLocalImg.Text.Trim()))
                {
                    dialog.FileName = file;
                }
                dialog.Multiselect = false;

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    txtLocalImg.Text = dialog.FileName;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            control.DetectFace(txtRemoteImg.Text, txtLocalImg.Text);
        }
    }
}
