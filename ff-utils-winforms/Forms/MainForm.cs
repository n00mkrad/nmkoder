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

            metaGrid = metadataGrid;
            ffmpegOutputBox = outputPath;

            fileListBox = fileList;
            streamListBox = streamList;
            thumbnailBox = thumbnail;
            formatInfoLabel = formatInfo;
            thumbLabel = thumbInfo;

            CheckForIllegalCrossThreadCalls = false;

            //InitCombox(createMp4Enc, 0);
            //InitCombox(createMp4Crf, 1);
            //InitCombox(createMp4Fps, 2);
            //InitCombox(loopTimesLossless, 0);
            //InitCombox(encContainer, 0);
            //InitCombox(encVidCodec, 1);
            //InitCombox(encVidCrf, 1);
            //InitCombox(encAudCodec, 1);
            //InitCombox(encAudBitrate, 4);
            //InitCombox(encAudioCh, 0);
            //InitCombox(changeSpeedCombox, 0);
            //InitCombox(comparisonLayout, 0);
            //InitCombox(comparisonType, 0);
            //InitCombox(comparisonCrf, 1);
            //InitCombox(delayTrackCombox, 0);
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

        public void SetProgress(int percent)
        {
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
            if(fileList.Items.Count < 1)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                dropPanel.Visible = true;
                dropLoadClearBg.BackColor = Color.FromArgb(48, 48, 48);
                dropLoadAddBg.BackColor = Color.FromArgb(48, 48, 48);
            }
        }

        private void inputPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            TrackList.HandleFiles(files, true);
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
            QuickConvertUi.AudEncoderSelected(encAudEnc.SelectedIndex);
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

        #region File Drop Panel

        private void dropLoadFilesClear_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            dropLoadClearBg.BackColor = Color.FromArgb(64, 64, 64);
            dropLoadAddBg.BackColor = Color.FromArgb(48, 48, 48);
        }

        private void dropLoadFilesAdd_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            dropLoadClearBg.BackColor = Color.FromArgb(48, 48, 48);
            dropLoadAddBg.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void dropLoadFilesClear_DragDrop(object sender, DragEventArgs e)
        {
            dropPanel.Visible = false;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            TrackList.HandleFiles(files, true);
        }

        private void dropLoadFilesAdd_DragDrop(object sender, DragEventArgs e)
        {
            dropPanel.Visible = false;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            TrackList.HandleFiles(files, false);
        }

        private void dropPanel_DragLeave(object sender, EventArgs e)
        {
            dropPanel.Visible = false;
        }

        #endregion
    }
}
