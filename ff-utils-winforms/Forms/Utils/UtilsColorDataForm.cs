using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.UI;
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

        private bool batchMode = false;
        private bool closeRightAway = false;
        ListBox fileList = Program.mainForm.fileListBox;

        public UtilsColorDataForm(bool close = false)
        {
            InitializeComponent();
            batchMode = RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess;
            closeRightAway = close || batchMode;

            if (closeRightAway)
                Opacity = 0;

            if (batchMode)
            {
                if(!close)
                    Logger.Log($"In batch processing mode, this util can only be used to read the metadata! Use the Multi File Mode for transferring.");

                if (TrackList.current == null)
                {
                    pressedOk = true;
                    Close();
                }
            }

            copyColorSpace.Checked = UtilColorData.copyColorSpace;
            copyHdrData.Checked = UtilColorData.copyHdrData;
            AcceptButton = confirmBtn;
        }

        private void LoadVideoBox (ComboBox box, string videoPath)
        {
            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if((((FileListEntry)fileList.Items[i])).File.ImportPath == videoPath)
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

            if (batchMode)
            {
                if(TrackList.current != null)
                    sourceVideo.Items.Add(TrackList.current.File);
            }
            else
            {
                foreach (FileListEntry entry in fileList.Items)
                {
                    sourceVideo.Items.Add(entry.File);
                    targetVideo.Items.Add(entry.File);
                }
            }
            
            LoadVideoBox(sourceVideo, UtilColorData.vidSrc);

            if (!batchMode && fileList.Items.Count > 1)
                LoadVideoBox(targetVideo, UtilColorData.vidTarget);

            if (sourceVideo.SelectedIndex < 0 || targetVideo.SelectedIndex < 0)
            {
                sourceVideo.SelectedItem = sourceVideo.Items.OfType<MediaFile>().OrderByDescending(x => x.Size).First();

                if (!batchMode && fileList.Items.Count > 1)
                    targetVideo.SelectedItem = targetVideo.Items.OfType<MediaFile>().OrderByDescending(x => x.Size).Last();
            }

            if (closeRightAway)
                Close();
        }

        bool pressedOk = false;

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            TransferColorSpace = copyColorSpace.Checked;
            TransferHdrData = copyHdrData.Checked;

            if(sourceVideo.SelectedItem != null)
                VideoSrc = ((MediaFile)sourceVideo.SelectedItem).ImportPath;

            if (targetVideo.SelectedItem != null)
                VideoTarget = ((MediaFile)targetVideo.SelectedItem).ImportPath;

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
