﻿using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data.Ui;
using Paths = Nmkoder.Data.Paths;
using Nmkoder.OS;
using System.Windows.Input;
using System.Collections.Generic;
using Nmkoder.Media;
using System.Diagnostics;

namespace Nmkoder.Forms
{
    public partial class MainForm : Form
    {
        bool initialized = false;

        public Cyotek.Windows.Forms.TabList MainTabList { get { return tabList; } }
        public PictureBox ThumbnailBox { get { return thumbnail; } }
        public Label FormatInfoLabel { get { return formatInfo; } }
        public Label ThumbLabel { get { return thumbInfo; } }
        public Button PauseBtn { get { return pauseBtn; } }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.mainForm = this;
            Logger.textbox = logTbox;

            InitQuickConvert();
            InitAv1an();

            CheckForIllegalCrossThreadCalls = false;
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            QuickConvert.Init();
            Av1an.Init();
            LoadUiConfig();
            encVidCodec_SelectedIndexChanged(null, null);
            encAudioCodec_SelectedIndexChanged(null, null);
            av1anAudCodec_SelectedIndexChanged(null, null);
            await RefreshFileListUi();

            if (Program.args.Where(x => x.StartsWith("package=")).Count() == 1)
                await PackageBuild.Run(Program.args.Where(x => x.StartsWith("package=")).First().Split('=')[1]);

            if (Paths.GetExe().Length > 150)
                Logger.Log($"Warning: Nmkoder's installation path is very long ({Paths.GetExe().Length} characters) - This can lead to problems. It is recommended to move it to a higher directory to reduce the path length.");

            QuickConvertUi.InitAdvFilterGrid();
            Av1anUi.InitAdvFilterGrid();
            UpdateResetSettingsText();
            QuickConvertUi.InitFile();

            initialized = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUiConfig();
            SaveConfigAv1an();

            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Shift)
            {
                string msg = "The Shift key was held when trying to close Nmkoder. This gives you the option to leave all subprocesses (e.g. av1an) running even after closing the GUI. " +
                    "Are you sure you want to close Nmkoder without stopping its subprocesses?";
                DialogResult dialog = MessageBox.Show(msg, "Keep subprocesses running?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Program.mainForm.Activate();

                if (dialog == DialogResult.No)
                    ProcessManager.KillAll();
            }
            else
            {
                ProcessManager.KillAll();
            }

            Program.Cleanup();
        }

        void LoadUiConfig()
        {
            ConfigParser.LoadComboxIndex(fileListMode);
            ConfigParser.LoadComboxIndex(taskMode);
            // Quick Convert
            ConfigParser.LoadGuiElement(containers);
            ConfigParser.LoadComboxIndex(encVidCodec);
            ConfigParser.LoadComboxIndex(encAudCodec);
            ConfigParser.LoadComboxIndex(encSubCodec);
            ConfigParser.LoadComboxIndex(encMetaCopySource);
            // Settings to reset

            LoadConfigAv1an();
            ResetSettingsOnNewFile.Load(settingsToResetLabel);
        }

        void SaveUiConfig(object sender = null, EventArgs e = null)
        {
            if (!initialized)
                return;

            ConfigParser.SaveComboxIndex(fileListMode);
            ConfigParser.SaveComboxIndex(taskMode);
            // Quick Convert
            ConfigParser.SaveGuiElement(containers);
            ConfigParser.SaveComboxIndex(encVidCodec);
            ConfigParser.SaveComboxIndex(encAudCodec);
            ConfigParser.SaveComboxIndex(encSubCodec);
            ConfigParser.SaveComboxIndex(encMetaCopySource);
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

            List<Control> controlsToDisable = new List<Control> { runBtn /* runStepBtn, stepSelector, settingsBtn */ };
            List<Control> controlsToHide = new List<Control> { runBtn /* runBtn, runStepBtn, stepSelector */ };

            if (RunTask.currentFileListMode == RunTask.FileListMode.Batch)
            {
                controlsToDisable.AddRange(new Control[] { trackListDefaultAudio, trackListDefaultSubs });
                controlsToHide.AddRange(new Control[] { trackListCheckTracksBtn, trackListSortTracksBtn, trackListMoveUpBtn, trackListMoveDownBtn, fileListMoveUpBtn, fileListMoveDownBtn });

                streamListBlockPanel.Visible = state;
            }

            progressCircle.Visible = state;
            busyControlsPanel.Visible = state;

            foreach (Control c in controlsToDisable)
                c.Enabled = !state;

            foreach (Control c in controlsToHide)
                c.Visible = !state;

            Program.busy = state;
        }

        public RunTask.TaskType RunningTask = RunTask.TaskType.None;
        public RunTask.TaskType SelectedTask
        {
            get
            {
                if (tabList.SelectedPage == quickConvertPage)
                    return RunTask.TaskType.Convert;

                if (tabList.SelectedPage == av1anPage)
                    return RunTask.TaskType.Av1an;

                if (tabList.SelectedPage == utilsPage)
                    return GetUtilsTaskType();

                return RunTask.TaskType.None;
            }
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
            if (Program.busy)
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> invalidFiles = files.Where(x => x.Length > 256).ToList();

            if (invalidFiles.Any())
            {
                string list = string.Join("\n", invalidFiles.Select(x => $"{x} ({x.Length} Chars)"));
                UiUtils.ShowMessageBox($"The following files can not be imported because their path is too long:\n\n{list}\n\nPlease ensure their path is less than 256 characters long.", UiUtils.MessageType.Warning);
            }

            files = files.Where(x => x.Length <= 256).ToArray();

            if (files.Length < 1)
                return;

            bool anyFilesLoaded = fileList.Items.Count > 0;

            if (anyFilesLoaded)
            {
                FileImportForm form = new FileImportForm(files, anyFilesLoaded);
                form.ShowDialog();

                if (form.ImportFiles.Count > 0)
                    FileList.HandleFiles(form.ImportFiles.ToArray(), form.Clear);
            }
            else
            {
                FileList.HandleFiles(files, false);
            }
        }

        public void runBtn_Click(object sender = null, EventArgs e = null)
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.Mux)
                RunTask.Start();
            else
                RunTask.StartBatch();
        }

        private void encAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuickConvertUi.AudEncoderSelected(encAudCodecBox.SelectedIndex);
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
            SuspendResume.SuspendProcs(!SuspendResume.frozen);
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

            if (sel == quickConvertPage)
                QuickConvertUi.ValidatePath();

            if (sel == av1anPage)
                Av1anUi.ValidatePath();
        }

        public void SetButtonActive(Control c, bool state)
        {
            c.Enabled = state;
            c.ForeColor = state ? Color.White : Color.FromArgb(48, 48, 48);
        }

        #region FileList

        public async Task RefreshFileListUi()
        {
            bool anySelected = fileList.SelectedItems.Count > 0;
            bool oneSelected = fileList.SelectedItems.Count == 1;

            SetButtonActive(fileListMoveUpBtn, oneSelected);
            SetButtonActive(fileListMoveDownBtn, oneSelected);
            SetButtonActive(fileListRemoveBtn, anySelected);
            SetButtonActive(addTracksFromFileBtn, RunTask.currentFileListMode == RunTask.FileListMode.Mux && anySelected);

            int count = Program.mainForm.fileListBox.Items.Count;

            fileCountLabel.Text = $"{count} file{(count != 1 ? "s" : "")} loaded. " +
                $"{(count > 1 && RunTask.currentFileListMode == RunTask.FileListMode.Mux ? "Double click any of them or use the Load Tracks button to load their tracks." : "")}";

            if (TrackList.current != null && !fileList.Items.Cast<ListViewItem>().Select(x => ((FileListEntry)x.Tag)).Any(x => x.File.Equals(TrackList.current.File)))
            {
                TrackList.ClearCurrentFile();

                if (RunTask.currentFileListMode == RunTask.FileListMode.Mux)
                    await TrackList.SetAsMainFile(fileList.Items[0], false);
            }

            QuickConvertUi.RefreshFileListRelatedOptions();
        }

        private async void fileListMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunTask.FileListMode oldMode = RunTask.currentFileListMode;
            RunTask.FileListMode newMode = (RunTask.FileListMode)fileListMode.SelectedIndex;

            if (oldMode == RunTask.FileListMode.Mux && newMode == RunTask.FileListMode.Batch)
                TrackList.ClearCurrentFile(true);

            RunTask.currentFileListMode = newMode;

            Text = $"NMKODER [{(RunTask.currentFileListMode == RunTask.FileListMode.Mux ? "Mux" : "Batch")}]";

            SaveUiConfig();
            await RefreshFileListUi();

            if (oldMode == RunTask.FileListMode.Batch && newMode == RunTask.FileListMode.Mux)
            {
                if (fileList.Items.Count == 1 && !AreAnyTracksLoaded())
                {
                    await TrackList.SetAsMainFile(fileList.Items[0]);
                    await TrackList.AddStreamsToList(((FileListEntry)fileListBox.Items[0].Tag).File, fileListBox.Items[0].BackColor, true);
                }
            }
        }

        private async void addTracksFromFileBtn_Click(object sender, EventArgs e)
        {
            addTracksFromFileBtn.Enabled = false;

            foreach (ListViewItem item in fileList.SelectedItems.Cast<ListViewItem>())
            {
                await TrackList.AddStreamsToList(((FileListEntry)item.Tag).File, item.BackColor, true);

                if (TrackList.current == null)
                    await TrackList.SetAsMainFile(item);
            }

            QuickConvertUi.LoadMetadataGrid();
            addTracksFromFileBtn.Enabled = true;
        }

        private void fileList_SelectedIndexChanged(object sender = null, EventArgs e = null)
        {
            RefreshFileListUi();
        }

        private void fileListCleanBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fileList.SelectedItems)
                fileList.Items.Remove(item);

            TrackList.Refresh();
        }

        private void fileListMoveUpBtn_Click(object sender, EventArgs e)
        {
            UiUtils.MoveListViewItem(fileList, UiUtils.MoveDirection.Up);
        }

        private void fileListMoveDownBtn_Click(object sender, EventArgs e)
        {
            UiUtils.MoveListViewItem(fileList, UiUtils.MoveDirection.Down);
        }

        private bool AreAnyTracksLoaded()
        {
            return streamList.Items.Count > 0;
        }

        private void fileListSortBtn_Click(object sender, EventArgs e)
        {
            sortFileListContextMenu.Show(System.Windows.Forms.Cursor.Position);
        }

        private void fileList_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ListViewItem item = fileList.HitTest(e.X, e.Y).Item;

            if (item != null && RunTask.currentFileListMode == RunTask.FileListMode.Mux)
                addTracksFromFileBtn_Click(null, null);
        }

        private void SetFileListItems(ListViewItem[] items)
        {
            fileList.Items.Clear();

            foreach (ListViewItem item in items)
                fileList.Items.Add(item);
        }

        private void sortMenuAbcDesc_Click(object sender, EventArgs e)
        {
            SetFileListItems(fileList.Items.Cast<ListViewItem>().OrderBy(x => ((FileListEntry)x.Tag).File.SourcePath).ToArray());
        }

        private void sortMenuAbcAsc_Click(object sender, EventArgs e)
        {
            SetFileListItems(fileList.Items.Cast<ListViewItem>().OrderByDescending(x => ((FileListEntry)x.Tag).File.SourcePath).ToArray());
        }

        private void sortMenuSizeDesc_Click(object sender, EventArgs e)
        {
            SetFileListItems(fileList.Items.Cast<ListViewItem>().OrderByDescending(x => ((FileListEntry)x.Tag).File.FileInfo.Length).ToArray());
        }

        private void sortMenuSizeAsc_Click(object sender, EventArgs e)
        {
            SetFileListItems(fileList.Items.Cast<ListViewItem>().OrderBy(x => ((FileListEntry)x.Tag).File.FileInfo.Length).ToArray());
        }

        private void sortMenuRecentDesc_Click(object sender, EventArgs e)
        {
            SetFileListItems(fileList.Items.Cast<ListViewItem>().OrderByDescending(x => ((FileListEntry)x.Tag).File.FileInfo.LastWriteTime).ToArray());
        }

        private void sortMenuRecentAsc_Click(object sender, EventArgs e)
        {
            SetFileListItems(fileList.Items.Cast<ListViewItem>().OrderBy(x => ((FileListEntry)x.Tag).File.FileInfo.LastWriteTime).ToArray());
        }


        #endregion

        #region Quick Convert

        private void encCropMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            encCropConfBtn.Visible = encCropMode.Text.ToLower().Contains("manual");
        }

        private void encCropConfBtn_Click(object sender, EventArgs e)
        {
            Size res = new Size();

            if (TrackList.current != null && TrackList.current.File.VideoStreams.Count > 0)
                res = TrackList.current.File.VideoStreams[0].Resolution;

            CropForm form = new CropForm(res, QuickConvertUi.CurrentCrop);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
                QuickConvertUi.CurrentCrop = form.Crop;
        }

        #endregion

        #region AV1AN

        private void av1anCrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            av1anCropConfBtn.Visible = av1anCrop.Text.ToLower().Contains("manual");
        }

        private void av1anCropConfBtn_Click(object sender, EventArgs e)
        {
            Size res = new Size();

            if (TrackList.current != null && TrackList.current.File.VideoStreams.Count > 0)
                res = TrackList.current.File.VideoStreams[0].Resolution;

            CropForm form = new CropForm(res, Av1anUi.CurrentCrop);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
                Av1anUi.CurrentCrop = form.Crop;
        }

        #endregion

        private void trackListExtractTracksBtn_Click(object sender, EventArgs e)
        {
            TrackList.Extract(streamList.SelectedItems[0]);
        }

        private async void encTrimConfBtn_Click(object sender, EventArgs e)
        {
            TrimForm form = new TrimForm(TrackList.current == null ? 0 : TrackList.current.File.DurationMs, QuickConvertUi.CurrentTrim);
            form.ShowDialog();
            QuickConvertUi.CurrentTrim = form.NewTrimSettings;
            UpdateTrimBtnText();
        }

        public void UpdateTrimBtnText ()
        {
            encTrimConfBtn.Text = QuickConvertUi.CurrentTrim == null ? "Configure..." : QuickConvertUi.CurrentTrim.ToString();
        }

        private void resetSettingsConfBtn_Click(object sender, EventArgs e)
        {
            ResetSettingsForm form = new ResetSettingsForm();
            form.ShowDialog();
            UpdateResetSettingsText();
        }

        public void UpdateResetSettingsText()
        {
            settingsToResetLabel.Text = ResetSettingsOnNewFile.GetString();
        }


        private void resetSettingsResetAllBtn_Click(object sender, EventArgs e)
        {
            TrackList.ResetSettings(true, true);
        }

        private void discordBtn_Click(object sender, EventArgs e)
        {
            discordLinkMenu.Show(System.Windows.Forms.Cursor.Position);
        }

        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/nmkd/10");
        }

        private void patreonBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://patreon.com/n00mkrad");
        }

        private void joinNmkdSoftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/eJHD2NSJRe");
        }

        private void joinAv1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/JutnEFrDbM"); 
        }
    }
}
