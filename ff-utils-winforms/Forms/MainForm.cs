using Nmkoder.Extensions;
using Nmkoder.GuiHelpers;
using Nmkoder.IO;
using Nmkoder.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using ImageMagick;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Utils;

namespace Nmkoder.Forms
{
    public partial class MainForm : Form
    {
        bool initialized = false;

        public Cyotek.Windows.Forms.TabList mainTabList;

        // General
        public ListBox fileListBox;
        public CheckedListBox streamListBox;
        public PictureBox thumbnailBox;
        public Label formatInfoLabel;
        public Label thumbLabel;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.mainForm = this;
            Logger.textbox = logTbox;

            mainTabList = tabList;

            InitQuickConvert();
            InitAv1an();

            metaGrid = metadataGrid;
            ffmpegOutputBox = outputPath;

            fileListBox = fileList;
            streamListBox = streamList;
            thumbnailBox = thumbnail;
            formatInfoLabel = formatInfo;
            thumbLabel = thumbInfo;

            CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            QuickConvert.Init();
            Av1an.Init();
            LoadUiConfig();
            encVidCodec_SelectedIndexChanged(null, null);
            encAudioCodec_SelectedIndexChanged(null, null);
            encSubCodec_SelectedIndexChanged(null, null);
            av1anAudCodec_SelectedIndexChanged(null, null);
            initialized = true;
        }

        void LoadUiConfig()
        {
            ConfigParser.LoadComboxIndex(fileListMode);
            ConfigParser.LoadComboxIndex(taskMode);
            // Quick Convert
            ConfigParser.LoadComboxIndex(containers);
            ConfigParser.LoadComboxIndex(encVidCodec);
            ConfigParser.LoadComboxIndex(encAudCodec);
            ConfigParser.LoadComboxIndex(encSubCodec);
            ConfigParser.LoadComboxIndex(metaMode);

            LoadConfigAv1an();
        }

        void SaveUiConfig(object sender = null, EventArgs e = null)
        {
            if (!initialized)
                return;

            ConfigParser.SaveComboxIndex(fileListMode);
            ConfigParser.SaveComboxIndex(taskMode);
            // Quick Convert
            ConfigParser.SaveComboxIndex(containers);
            ConfigParser.SaveComboxIndex(encVidCodec);
            ConfigParser.SaveComboxIndex(encAudCodec);
            ConfigParser.SaveComboxIndex(encSubCodec);
            ConfigParser.SaveComboxIndex(metaMode);
        }

        public bool IsInFocus() { return (ActiveForm == this); }

        public void SetProgress(int percent, bool ignoreIfNotBusy = true)
        {
            if (ignoreIfNotBusy && !Program.busy)
                return;

            percent = percent.Clamp(0, 100);
            TaskbarManager.Instance.SetProgressValue(percent, 100);
            progBar.Value = percent;
            progBar.Refresh();
        }

        public void SetStatus(string str)
        {
            // TODO: IMPLEMENT
            Logger.Log(str, true);
            //statusLabel.Text = str;
        }

        public void SetWorking(bool state, bool allowCancel = true)
        {
            Logger.Log($"SetWorking({state})", true);
            SetProgress(-1);
            Control[] controlsToDisable = new Control[] { runBtn /* runStepBtn, stepSelector, settingsBtn */ };
            Control[] controlsToHide = new Control[] { runBtn /* runBtn, runStepBtn, stepSelector */ };
            progressCircle.Visible = state;
            busyControlsPanel.Visible = state;

            foreach (Control c in controlsToDisable)
                c.Enabled = !state;

            foreach (Control c in controlsToHide)
                c.Visible = !state;

            //busyControlsPanel.Enabled = allowCancel;
            Program.busy = state;
        }

        public RunTask.TaskType GetCurrentTaskType()
        {
            if(tabList.SelectedPage == quickConvertPage)
                return RunTask.TaskType.Convert;

            if (tabList.SelectedPage == av1anPage)
                return RunTask.TaskType.Av1an;

            if (tabList.SelectedPage == utilsPage)
                return GetUtilsTaskType();

            return RunTask.TaskType.None;
        }

        private void DragEnterHandler(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void inputPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void inputPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            bool anyFilesLoaded = fileList.Items.Count > 0;

            if(files.Length > 1 || anyFilesLoaded)
            {
                FileImportForm form = new FileImportForm(files, anyFilesLoaded);
                form.ShowDialog();

                if (form.ImportFiles.Count > 0)
                    TrackList.HandleFiles(form.ImportFiles.ToArray(), form.Clear);
            }
            else
            {
                TrackList.HandleFiles(files, false);
            }
        }

        public void runBtn_Click(object sender = null, EventArgs e = null)
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.MultiFileInput)
                RunTask.Start();
            else
                RunTask.StartBatch();
        }

        private void encAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUiConfig();
            QuickConvertUi.AudEncoderSelected(encAudCodecBox.SelectedIndex);
        }

        private void encSubCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUiConfig();
        }

        private void thumbnail_Click(object sender, EventArgs e)
        {
            ThumbnailView.ThumbnailClick();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            RunTask.Cancel("Canceled manually.", true);
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            Logger.Log($"Not implemented yet.");
        }

        private void metaMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUiConfig();
        }

        private void tabList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = tabList.SelectedPage;
            runBtn.Enabled = sel == quickConvertPage || sel == utilsPage || sel == av1anPage;

            if (sel == fileListPage)
                RefreshFileListUi();

            if (sel == streamListPage)
                RefreshStreamListUi();

            if (sel == settingsPage)
                LoadConfig();
            else
                SaveConfig();
        }
    }
}
