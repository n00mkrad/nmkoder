using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
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
        public int Subsample { get; set; } = 1;
        public int AlignMode { get; set; } = 0;
        public int VmafModel { get; set; } = 0;
        public string VideoLq { get; set; }
        public string VideoHq { get; set; }

        private bool closeRightAway = false;

        ListBox fileList = Program.mainForm.fileListBox;

        public UtilsMetricsForm(bool close = false)
        {
            InitializeComponent();

            closeRightAway = close;

            if (closeRightAway)
                Opacity = 0;

            vmaf.Checked = UtilGetMetrics.runVmaf;
            ssim.Checked = UtilGetMetrics.runSsim;
            psnr.Checked = UtilGetMetrics.runPsnr;
            align.SelectedIndex = UtilGetMetrics.alignMode;
            subsample.SelectedIndex = (UtilGetMetrics.subsample - 1).Clamp(0, 64);
            vmafMdl.SelectedIndex = UtilGetMetrics.vmafModel;
            AcceptButton = confirmBtn;
        }

        private void LoadVideoBox (ComboBox box, string videoPath)
        {
            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if((((FileListEntry)fileList.Items[i])).File.TruePath == videoPath)
                {
                    box.SelectedIndex = i;
                    return;
                }
            }
        }

        private void UtilsMetricsForm_Load(object sender, EventArgs e)
        {
            
        }

        private void UtilsMetricsForm_Shown(object sender, EventArgs e)
        {
            ListBox fileList = Program.mainForm.fileListBox;

            foreach(FileListEntry entry in fileList.Items)
            {
                encodedVideo.Items.Add(entry.File);
                referenceVideo.Items.Add(entry.File);
            }

            LoadVideoBox(encodedVideo, UtilGetMetrics.vidLq);
            LoadVideoBox(referenceVideo, UtilGetMetrics.vidHq);

            if (encodedVideo.SelectedIndex < 0 || referenceVideo.SelectedIndex < 0)
            {
                encodedVideo.SelectedItem = encodedVideo.Items.OfType<MediaFile>().OrderByDescending(x => x.Size).Last();
                referenceVideo.SelectedItem = referenceVideo.Items.OfType<MediaFile>().OrderByDescending(x => x.Size).First();
            }

            if (closeRightAway)
                Close();
        }

        bool pressedOk = false;

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            Subsample = subsample.SelectedIndex + 1;
            AlignMode = align.SelectedIndex;
            VmafModel = vmafMdl.SelectedIndex;
            CheckedBoxes = new bool[] { vmaf.Checked, ssim.Checked, psnr.Checked };
            VideoLq = ((MediaFile)encodedVideo.SelectedItem).TruePath;
            VideoHq = ((MediaFile)referenceVideo.SelectedItem).TruePath;
            DialogResult = DialogResult.OK;
            pressedOk = true;
            Close();
            Program.mainForm.BringToFront();
        }

        private void UtilsMetricsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!pressedOk)
                confirmBtn_Click(null, null);
        }
    }
}
