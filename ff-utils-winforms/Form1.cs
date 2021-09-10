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

namespace Nmkoder
{
    public partial class Form1 : Form
    {
        public CheckedListBox streamListBox;
        public PictureBox thumbnailBox;
        public Label formatInfoLabel;

        public ComboBox containerBox;
        public ComboBox encEncoderBox;
        public ComboBox encQualityBox;
        public ComboBox encPresetBox;
        public ComboBox encColorsBox;

        public ComboBox encAudioEnc;
        public ComboBox encAudioBr;

        public TextBox outputBox;
        public TextBox customArgsBox;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.mainForm = this;
            Logger.textbox = logTbox;

            containerBox = containers;
            encEncoderBox = encEncoder;
            encQualityBox = encQuality;
            encPresetBox = encPreset;
            encColorsBox = encColors;

            encAudioEnc = encAudCodec;
            encAudioBr = encAudBitrate;

            outputBox = outputPath;
            customArgsBox = customArgs;

            streamListBox = streamList;
            thumbnailBox = thumbnail;
            formatInfoLabel = formatInfo;

            
            CheckForIllegalCrossThreadCalls = false;

            InitCombox(createMp4Enc, 0);
            InitCombox(createMp4Crf, 1);
            InitCombox(createMp4Fps, 2);
            InitCombox(loopTimesLossless, 0);
            InitCombox(encContainer, 0);
            InitCombox(encVidCodec, 1);
            InitCombox(encVidCrf, 1);
            InitCombox(encAudCodec, 1);
            InitCombox(encAudBitrate, 4);
            InitCombox(encAudioCh, 0);
            InitCombox(changeSpeedCombox, 0);
            InitCombox(comparisonLayout, 0);
            InitCombox(comparisonType, 0);
            InitCombox(comparisonCrf, 1);
            InitCombox(delayTrackCombox, 0);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            QuickConvert.Init();
            LoadConfig();
        }

        void LoadConfig()
        {
            ConfigParser.LoadComboxIndex(taskMode);
            ConfigParser.LoadComboxIndex(containers);
            ConfigParser.LoadComboxIndex(encEncoder);
        }

        void SaveConfig(object sender = null, EventArgs e = null)
        {
            ConfigParser.SaveComboxIndex(taskMode);
            ConfigParser.SaveComboxIndex(containers);
            ConfigParser.SaveComboxIndex(encEncoder);
        }

        void InitCombox(ComboBox cbox, int index)
        {
            cbox.SelectedIndex = index;
            cbox.Text = cbox.Items[index].ToString();
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
            Control[] controlsToDisable = new Control[] { /* runBtn, runStepBtn, stepSelector, settingsBtn */ };
            Control[] controlsToHide = new Control[] { /* runBtn, runStepBtn, stepSelector */ };
            //progressCircle.Visible = state;
            //busyControlsPanel.Visible = state;

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
                return Main.RunTask.TaskType.Convert;

            return Main.RunTask.TaskType.None;
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

        private void streamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            streamDetails.Text = MediaInfo.GetStreamDetails(streamList.SelectedIndex);
        }

        private void encEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfig();
            QuickConvertUi.VidEncoderSelected(encEncoder.SelectedIndex);
        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuickConvertUi.CheckContainer();
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            RunTask.Start();
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
