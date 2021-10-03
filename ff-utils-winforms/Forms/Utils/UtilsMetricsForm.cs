using Nmkoder.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms.Utils
{
    public partial class UtilsMetricsForm : Form
    {
        public bool[] CheckedBoxes { get; set; }
        public string VideoLq { get; set; }
        public string VideoHq { get; set; }

        public UtilsMetricsForm(bool vmafChecked, bool ssimChecked, bool psnrChecked)
        {
            InitializeComponent();
            vmaf.Checked = vmafChecked;
            ssim.Checked = ssimChecked;
            psnr.Checked = psnrChecked;
            AcceptButton = confirmBtn;
        }

        private void UtilsMetricsForm_Load(object sender, EventArgs e)
        {

        }

        private void UtilsMetricsForm_Shown(object sender, EventArgs e)
        {
            ListBox fileList = Program.mainForm.fileListBox;

            foreach(MediaFile mf in fileList.Items)
            {
                encodedVideo.Items.Add(mf);
                referenceVideo.Items.Add(mf);
            }

            encodedVideo.SelectedIndex = 0;
            referenceVideo.SelectedIndex = 1;
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            CheckedBoxes = new bool[] { vmaf.Checked, ssim.Checked, psnr.Checked };
            VideoLq = ((MediaFile)encodedVideo.SelectedItem).TruePath;
            VideoHq = ((MediaFile)referenceVideo.SelectedItem).TruePath;
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }
    }
}
