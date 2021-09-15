﻿using Nmkoder.Extensions;
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

namespace Nmkoder
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
        // Quick Convert - Video
        public ComboBox containerBox;
        public ComboBox encVidCodecsBox;
        public NumericUpDown encVidQualityBox;
        public ComboBox encVidPresetBox;
        public ComboBox encVidColorsBox;
        public TextBox encVidFpsBox;
        // Quick Convert - Audio
        public ComboBox encAudEnc;
        public NumericUpDown encAudBr;
        public ComboBox encAudCh;
        // Quick Convert - Subs
        public ComboBox encSubEnc;
        public ComboBox encSubBurnBox;
        // Quick Convert - Other
        public DataGridView metaGrid;
        public TextBox outputBox;
        public TextBox customArgsBox;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.mainForm = this;
            Logger.textbox = logTbox;

            mainTabList = tabList;

            containerBox = containers;
            encVidCodecsBox = encVidCodec;
            encVidQualityBox = encVidQuality;
            encVidPresetBox = encVidPreset;
            encVidColorsBox = encVidColors;
            encVidFpsBox = encVidFps;

            encAudEnc = encAudCodec;
            encAudBr = encAudBitrate;
            encAudCh = encAudChannels;

            encSubEnc = encSubCodec;
            encSubBurnBox = encSubBurn;

            metaGrid = metadataGrid;
            outputBox = outputPath;
            customArgsBox = encCustomArgs;

            fileListBox = fileList;
            streamListBox = streamList;
            thumbnailBox = thumbnail;
            formatInfoLabel = formatInfo;
            thumbLabel = thumbInfo;

            encAudChannels.SelectedIndex = 1;

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

        private void Form1_Shown(object sender, EventArgs e)
        {
            QuickConvert.Init();
            LoadConfig();
            encVidCodec_SelectedIndexChanged(null, null);
            encAudioCodec_SelectedIndexChanged(null, null);
            encSubCodec_SelectedIndexChanged(null, null);
            initialized = true;
        }

        void LoadConfig()
        {
            ConfigParser.LoadComboxIndex(taskMode);
            ConfigParser.LoadComboxIndex(containers);
            ConfigParser.LoadComboxIndex(encVidCodec);
            ConfigParser.LoadComboxIndex(encAudCodec);
            ConfigParser.LoadComboxIndex(encSubCodec);
            ConfigParser.LoadComboxIndex(metaMode);
        }

        void SaveConfig(object sender = null, EventArgs e = null)
        {
            if (!initialized)
                return;

            ConfigParser.SaveComboxIndex(taskMode);
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

        public Main.RunTask.TaskType GetCurrentTaskType()
        {
            if(tabList.SelectedPage == quickConvertPage)
                return RunTask.TaskType.Convert;

            return RunTask.TaskType.None;
        }

        private void DragEnterHandler(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private async void DragDropHandler(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            await Task.Delay(100);
            //if (mainTabControl.SelectedTab == extractFramesPage) await ExtractFrames(files);
            //if (mainTabControl.SelectedTab == framesToVideoPage) await FramesToVideo(files);
            //if (mainTabControl.SelectedTab == loopPage) await Loop(files);
            //if (mainTabControl.SelectedTab == speedPage) await ChangeSpeed(files);
            //if (mainTabControl.SelectedTab == comparisonPage) await CreateComparison(files);
            //if (mainTabControl.SelectedTab == encPage) await Encode(files);
            //if (mainTabControl.SelectedTab == delayPage) await Delay(files);
        }

        private void inputPanel_DragEnter(object sender, DragEventArgs e) { e.Effect = DragDropEffects.Copy; }

        private void inputPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            MediaInfo.HandleFiles(files);
        }

        public void ClearCurrentFile ()
        {
            streamList.Items.Clear();
            streamDetails.Text = "";
            ThumbnailView.ClearUi();
        }

        private void streamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(streamList.SelectedItem == null)
                return;

            MediaStreamListEntry entry = (MediaStreamListEntry)streamList.SelectedItem;
            streamDetails.Text = MediaInfo.GetStreamDetails(entry.Stream);
        }

        private void encVidCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
            QuickConvertUi.VidEncoderSelected(encVidCodec.SelectedIndex);
        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
            QuickConvertUi.ValidateContainer();
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show($"You have loaded multiple files. Do you want to run batch processing and apply this action to all of them?" +
                $"\n\nClick \"No\" if you only want to run the first (currently loaded) file.", "Batch Processing", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
                RunTask.StartBatch();
            else
                RunTask.Start();
        }

        private void encAudioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
            QuickConvertUi.AudEncoderSelected(encAudEnc.SelectedIndex);
        }

        private void encSubCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
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

        private void streamList_Leave(object sender, EventArgs e)
        {
            QuickConvertUi.LoadMetadataGrid();
        }

        private void metaMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void addTracksFromFileBtn_Click(object sender, EventArgs e)
        {
            MediaInfo.AddStreamsToList((MediaFile)fileList.SelectedItem, false);
        }

        private void streamList_MouseDown(object sender, MouseEventArgs e)
        {
            if (streamList.IndexFromPoint(new Point(e.X, e.Y)) <= -1) // if no item was clicked
                streamList.SelectedItems.Clear();
        }


        //async Task ExtractFrames(string[] files)
        //{
        //    if (extractFramesTabcontrol.SelectedIndex == 0)
        //    {
        //        foreach (string file in files)
        //            await FFmpegCommands.VideoToFrames(file, tonemapHdrCbox2.Checked, extractAllDelSrcCbox.Checked);
        //    }
        //    if (extractFramesTabcontrol.SelectedIndex == 1)
        //    {
        //        int frameNum = frameNumTbox.GetInt();
        //        foreach (string file in files)
        //            await FFmpegCommands.ExtractSingleFrame(file, frameNum, tonemapHdrCbox2.Checked, extractSingleDelSrcCbox.Checked);
        //    }
        //}
        //
        //async Task FramesToVideo(string[] dirs)
        //{
        //    foreach (string dir in dirs)
        //    {
        //        if (!IOUtils.IsPathDirectory(dir))
        //        {
        //            Program.Print("Please drop a folder containing frames, not single files!");
        //            continue;
        //        }
        //
        //        string concatFile = Path.Combine(IOUtils.GetTempPath(), "concat-temp.ini");
        //        string[] paths = IOUtils.GetFilesSorted(dir);
        //        string concatFileContent = "";
        //        foreach (string path in paths)
        //            concatFileContent += $"file '{path}'\n";
        //        File.WriteAllText(concatFile, concatFileContent);
        //
        //        if (createVidTabControl.SelectedTab == framesToVidTab) // Create MP4
        //        {
        //            bool h265 = createMp4Enc.SelectedIndex == 1;
        //            int crf = createMp4Crf.GetInt();
        //            float fps = createMp4Fps.GetFloat();
        //            await FFmpegCommands.FramesToMp4Concat(concatFile, dir + ".mp4", h265, crf, fps);
        //        }
        //        if (createVidTabControl.SelectedTab == framesToGifTab) // Create GIF
        //        {
        //            bool optimize = createGifOpti.Checked;
        //            float fps = createGifFps.GetFloat();
        //            await FFmpegCommands.FramesToGifConcat(concatFile, dir + ".gif", optimize, fps);
        //        }
        //        if (createVidTabControl.SelectedTab == framesToApngTab) // Create APNG
        //        {
        //            bool optimize = createApngOpti.Checked;
        //            float fps = createApngFps.GetFloat();
        //            await FFmpegCommands.FramesToApngConcat(concatFile, dir + ".png", optimize, fps);
        //        }
        //
        //        //await Task.Delay(10);
        //    }
        //}
        //
        //async Task Loop(string[] files)
        //{
        //    if (loopTabControl.SelectedIndex == 0) // Lossless
        //    {
        //        int times = loopTimesLossless.GetInt();
        //        foreach (string file in files)
        //            await FFmpegCommands.LoopVideo(file, times, false);
        //    }
        //}
        //
        //async Task Encode(string[] files)
        //{
        //    foreach (string file in files)
        //    {
        //        if(encodeTabControl.SelectedTab == encVidTab)
        //            EncodeTabHelper.Encode(file, encContainer, encVidCodec, encFpsBox, encAudCodec, encAudioCh, encVidCrf, encAudBitrate, encDelSrc);
        //        if (encodeTabControl.SelectedTab == encGifTab)
        //            EncodeTabHelper.VideoToGif(file, vidToGifOptimize, vidToGifFps);
        //        if (encodeTabControl.SelectedTab == encApngTab)
        //            EncodeTabHelper.VideoToApng(file, vidToApngOptimize, vidToApngFps);
        //    }
        //
        //}
        //
        //
        //async Task ChangeSpeed(string[] files)
        //{
        //    if (speedTabControl.SelectedIndex == 0) // Lossless
        //    {
        //        int times = changeSpeedCombox.GetInt();
        //        foreach (string file in files)
        //            await FFmpegCommands.ChangeSpeed(file, times, changeSpeedAudio.Checked);
        //    }
        //}
        //
        //async Task CreateComparison(string[] files)
        //{
        //    if (files.Length < 2)
        //    {
        //        Program.Print("Please drop two video files!");
        //        return;
        //    }
        //
        //    await ComparisonHelper.CreateComparison(files, comparisonLayout.SelectedIndex == 1, comparisonType.SelectedIndex == 1, comparisonCrf.GetInt());
        //}
        //
        //async Task Delay(string[] files)
        //{
        //    FFmpegCommands.Track track = (delayTrackCombox.SelectedIndex == 0) ? FFmpegCommands.Track.Audio : FFmpegCommands.Track.Video;
        //    foreach (string file in files)
        //        await FFmpegCommands.Delay(file, track, delayAmount.Text.GetFloat(), false);
        //}
    }
}