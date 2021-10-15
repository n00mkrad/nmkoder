using Nmkoder.Data;
using Nmkoder.UI.Tasks;
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
        public int AlignMode { get; set; } = 0;
        public int VmafModel { get; set; } = 0;
        public string VideoLq { get; set; }
        public string VideoHq { get; set; }

        ListBox fileList = Program.mainForm.fileListBox;

        public UtilsMetricsForm()
        {
            InitializeComponent();
            
            vmaf.Checked = UtilGetMetrics.runVmaf;
            ssim.Checked = UtilGetMetrics.runSsim;
            psnr.Checked = UtilGetMetrics.runPsnr;
            align.SelectedIndex = UtilGetMetrics.alignMode;
            vmafMdl.SelectedIndex = UtilGetMetrics.vmafModel;
            AcceptButton = confirmBtn;
        }

        private void LoadVideoBox (ComboBox box, string videoPath)
        {
            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if(((MediaFile)fileList.Items[i]).TruePath == videoPath)
                {
                    box.SelectedIndex = i;
                    return;
                }
            }
        }

        private void UtilsMetricsForm_Load(object sender, EventArgs e)
        {
            align.SelectedIndex = UtilGetMetrics.alignMode;
        }

        private void UtilsMetricsForm_Shown(object sender, EventArgs e)
        {
            ListBox fileList = Program.mainForm.fileListBox;

            foreach(MediaFile mf in fileList.Items)
            {
                encodedVideo.Items.Add(mf);
                referenceVideo.Items.Add(mf);
            }

            LoadVideoBox(encodedVideo, UtilGetMetrics.vidLq);
            LoadVideoBox(referenceVideo, UtilGetMetrics.vidHq);

            if (encodedVideo.SelectedIndex < 0)
                encodedVideo.SelectedIndex = 0;

            if (referenceVideo.SelectedIndex < 0)
                referenceVideo.SelectedIndex = 1;
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            AlignMode = align.SelectedIndex;
            VmafModel = vmafMdl.SelectedIndex;
            CheckedBoxes = new bool[] { vmaf.Checked, ssim.Checked, psnr.Checked };
            VideoLq = ((MediaFile)encodedVideo.SelectedItem).TruePath;
            VideoHq = ((MediaFile)referenceVideo.SelectedItem).TruePath;
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }
    }
}
