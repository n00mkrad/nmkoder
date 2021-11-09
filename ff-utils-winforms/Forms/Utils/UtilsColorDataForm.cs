using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI.Tasks;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms.Utils
{
    public partial class UtilsColorDataForm : Form
    {
        public bool TransferColorSpace { get; set; }
        public bool TransferHdrData { get; set; }
        public string VideoSrc { get; set; }
        public string VideoTarget { get; set; }

        ListBox fileList = Program.mainForm.fileListBox;

        public UtilsColorDataForm()
        {
            InitializeComponent();
            
            copyColorSpace.Checked = UtilColorData.copyColorSpace;
            copyHdrData.Checked = UtilColorData.copyHdrData;
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

        private void UtilsColorDataForm_Load(object sender, EventArgs e)
        {
            
        }

        private void UtilsColorDataForm_Shown(object sender, EventArgs e)
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
        }

        bool pressedOk = false;

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            TransferColorSpace = copyColorSpace.Checked;
            TransferHdrData = copyHdrData.Checked;
            VideoSrc = ((MediaFile)encodedVideo.SelectedItem).TruePath;
            VideoTarget = ((MediaFile)referenceVideo.SelectedItem).TruePath;
            DialogResult = DialogResult.OK;
            pressedOk = true;
            Close();
            Program.mainForm.BringToFront();
        }

        private void UtilsColorDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!pressedOk)
                confirmBtn_Click(null, null);
        }
    }
}
