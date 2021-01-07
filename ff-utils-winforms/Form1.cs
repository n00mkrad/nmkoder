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

namespace ff_utils_winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.logTbox = logTbox;
            CheckForIllegalCrossThreadCalls = false;
            InitCombox(createMp4Enc, 0);
            InitCombox(createMp4Crf, 1);
            InitCombox(createMp4Fps, 2);
            InitCombox(loopTimesLossless, 0);
            InitCombox(loopEncTimes, 0);
            InitCombox(loopEnc, 0);
            InitCombox(loopCrf, 1);
            InitCombox(encContainer, 0);
            InitCombox(encVidCodec, 1);
            InitCombox(encVidCrf, 1);
            InitCombox(encAudCodec, 1);
            InitCombox(encAudBitrate, 4);
            InitCombox(encAudioCh, 0);
            InitCombox(changeSpeedCombox, 0);
            InitCombox(comparisonEnc, 0);
            InitCombox(comparisonCrf, 1);
            InitCombox(comp2enc, 0);
            InitCombox(comp2crf, 1);
            InitCombox(delayTrackCombox, 0);
        }

        void InitCombox(ComboBox cbox, int index)
        {
            cbox.SelectedIndex = index;
            cbox.Text = cbox.Items[index].ToString();
        }

        private void DragEnterHandler(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void DragDropHandler(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (mainTabControl.SelectedTab == extractFramesPage) ExtractFrames(files);
            if (mainTabControl.SelectedTab == framesToVideoPage) FramesToVideo(files);
            if (mainTabControl.SelectedTab == loopPage) Loop(files);
            if (mainTabControl.SelectedTab == speedPage) ChangeSpeed(files);
            if (mainTabControl.SelectedTab == comparisonPage) CreateComparison(files);
            if (mainTabControl.SelectedTab == encPage) Encode(files);
            if (mainTabControl.SelectedTab == delayPage) Delay(files);
        }

        void ExtractFrames(string[] files)
        {
            if (extractFramesTabcontrol.SelectedIndex == 0)
            {
                foreach (string file in files)
                    FFmpegCommands.VideoToFrames(file, tonemapHdrCbox2.Checked, extractAllDelSrcCbox.Checked);
            }
            if (extractFramesTabcontrol.SelectedIndex == 1)
            {
                int frameNum = frameNumTbox.GetInt();
                foreach (string file in files)
                    FFmpegCommands.ExtractSingleFrame(file, frameNum, tonemapHdrCbox2.Checked, extractSingleDelSrcCbox.Checked);
            }
        }

        void FramesToVideo(string[] dirs)
        {
            foreach (string dir in dirs)
            {
                string concatFile = Path.Combine(IOUtils.GetTempPath(), "concat-temp.ini");
                string[] paths = IOUtils.GetFilesSorted(dir);
                string concatFileContent = "";
                foreach (string path in paths)
                    concatFileContent += $"file '{path}'\n";
                File.WriteAllText(concatFile, concatFileContent);

                if (createVidTabControl.SelectedIndex == 0) // Create MP4
                {
                    bool h265 = createMp4Enc.SelectedIndex == 1;
                    int crf = createMp4Crf.GetInt();
                    float fps = createMp4Fps.GetFloat();
                    FFmpegCommands.FramesToMp4Concat(concatFile, dir + ".mp4", h265, crf, fps);
                }
                if (createVidTabControl.SelectedIndex == 1) // Create APNG
                {
                    bool optimize = createApngOpti.Checked;
                    float fps = createApngFps.GetFloat();
                    FFmpegCommands.FramesToApng(dir, optimize, fps, createApngPrefix.Text.Trim(), createApngDelSrc.Checked);
                }
                if (createVidTabControl.SelectedIndex == 2) // Create GIF
                {
                    bool optimize = createGifOpti.Checked;
                    float fps = createGifFps.GetFloat();
                    FFmpegCommands.FramesToGif(dir, optimize, fps, createGifPrefix.Text.Trim(), createGifDelSrc.Checked);
                }

                //await Task.Delay(10);
            }
        }

        void Loop(string[] files)
        {
            if (loopTabControl.SelectedIndex == 0) // Lossless
            {
                int times = loopTimesLossless.GetInt();
                foreach (string file in files)
                    FFmpegCommands.LoopVideo(file, times, loopEncDelSrc.Checked);
            }
            if (loopTabControl.SelectedIndex == 1) // With Re-Encoding
            {
                int times = int.Parse(loopEncTimes.Text.Trim());
                bool h265 = loopEnc.SelectedIndex == 1;
                int crf = int.Parse(loopCrf.Text.Trim());
                foreach (string file in files)
                    FFmpegCommands.LoopVideoEnc(file, times, h265, crf, loopEncDelSrc.Checked);
            }
        }

        void Encode(string[] files)
        {
            foreach (string file in files)
                EncodeTabHelper.Run(file, encContainer, encVidCodec, encFpsBox, encAudCodec, encAudioCh, encVidCrf, encAudBitrate, encDelSrc);
        }


        void ChangeSpeed(string[] files)
        {
            if (speedTabControl.SelectedIndex == 0) // Lossless
            {
                int times = changeSpeedCombox.GetInt();
                foreach (string file in files)
                    FFmpegCommands.ChangeSpeed(file, times, loopEncDelSrc.Checked);
            }
        }

        void CreateComparison(string[] files)
        {
            string vcodec = "libx264";

            if (compTabControl.SelectedIndex == 0) // Side By Side
            {
                if (comparisonEnc.SelectedIndex == 1)
                    vcodec = "libx265";
                FFmpegCommands.CreateComparison(files[0], files[1], false, vcodec, comparisonCrf.GetInt(), comparisonDelSrc.Checked);
            }

            if (compTabControl.SelectedIndex == 1) // Over-Under
            {
                if (comp2enc.SelectedIndex == 1)
                    vcodec = "libx265";
                FFmpegCommands.CreateComparison(files[0], files[1], true, vcodec, comp2crf.GetInt(), comp2delSrc.Checked);
            }
        }

        void Delay(string[] files)
        {
            FFmpegCommands.Track track = (delayTrackCombox.SelectedIndex == 0) ? FFmpegCommands.Track.Audio : FFmpegCommands.Track.Video;
            foreach (string file in files)
                FFmpegCommands.Delay(file, track, delayAmount.Text.GetFloat(), delayDelSrc.Checked);
        }
    }
}
