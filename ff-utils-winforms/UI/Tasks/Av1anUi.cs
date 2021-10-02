using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    class Av1anUi
    {
        private static MainForm form;

        public static void Init()
        {
            form = Program.mainForm;

            foreach (Codecs.Av1anCodec c in Enum.GetValues(typeof(Codecs.Av1anCodec)))  // Load video codecs
                form.av1anCodecBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.av1anCodecBox);


            foreach (Av1an.QualityMode qm in Enum.GetValues(typeof(Av1an.QualityMode)))  // Load quality modes
                form.av1anQualModeBox.Items.Add(qm.ToString().Replace("Crf", "CRF").Replace("TargetVmaf", "Target VMAF"));

            form.av1anQualModeBox.SelectedIndex = 0;


            form.av1anOptsSplitModeBox.SelectedIndex = 1;


            foreach (Av1an.ChunkMethod cm in Enum.GetValues(typeof(Av1an.ChunkMethod)))  // Load chunk modes
                form.av1anOptsChunkModeBox.Items.Add(cm);


            foreach (Codecs.AudioCodec c in Enum.GetValues(typeof(Codecs.AudioCodec)))  // Load audio codecs
                form.av1anAudCodecBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);
            
            ConfigParser.LoadComboxIndex(form.av1anAudCodecBox);


            form.av1anAudChannelsBox.SelectedIndex = 1;


            form.av1anContainerBox.Items.Add(Containers.Container.Mkv.ToString().ToUpper());
            form.av1anContainerBox.Items.Add(Containers.Container.Webm.ToString().ToUpper());
        }

        public static void InitFile(string path)
        {
            try
            {
                form.av1anOutputPathBox.Text = path;

                if (!RunTask.runningBatch) // Don't load new values into UI in batch mode since we apply the same for all files
                {
                    form.av1anScaleBoxW.Text = form.av1anScaleBoxH.Text = "";
                    InitAudioChannels(TrackList.current.AudioStreams.FirstOrDefault()?.Channels);
                }

                ValidateContainer();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialize media file: {e.Message}\n{e.StackTrace}");
            }
        }

        public static void InitAudioChannels(int? ch)
        {
            if (ch == null || ch < 1)
            {
                form.av1anAudChannelsBox.SelectedIndex = 1;
                return;
            }

            for (int i = 0; i < form.av1anAudChannelsBox.Items.Count; i++)
            {
                if (form.av1anAudChannelsBox.Items[i].ToString().Split(' ').First().GetInt() == ch)
                    form.av1anAudChannelsBox.SelectedIndex = i;
            }
        }

        public static void VidEncoderSelected(int index)
        {
            Codecs.Av1anCodec c = (Codecs.Av1anCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);
            form.qInfoLabel.Text = info.QInfo;
            form.presetInfoLabel.Text = info.PInfo;
            form.av1anGrainSynthStrengthUpDown.Enabled = form.av1anGrainSynthDenoiseBox.Enabled = c != Codecs.Av1anCodec.VpxVp9; // Disable grain synth for VP9 since only AV1 has it
            form.av1anGrainSynthDenoiseBox.Enabled = c == Codecs.Av1anCodec.AomAv1; // Only AOM has an option to enable or disable denoising...
            form.av1anGrainSynthDenoiseBox.Checked = c == Codecs.Av1anCodec.SvtAv1; // ...SVT always has it enabled
            LoadQualityLevel(info);
            LoadPresets(info);
            LoadColorFormats(info);
        }

        public static void AudEncoderSelected(int index)
        {
            Codecs.AudioCodec c = (Codecs.AudioCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);

            form.av1anAudChannelsBox.Enabled = !(c == Codecs.AudioCodec.Copy || c == Codecs.AudioCodec.StripAudio);
            form.av1anAudQualUpDown.Enabled = info.QDefault >= 0;
            LoadAudBitrate(info);
            ValidateContainer();
        }

        static void LoadAudBitrate(CodecInfo info)
        {
            int channels = form.av1anAudChannelsBox.Text.Split(' ')[0].GetInt();

            if (info.QDefault >= 0)
            {
                form.av1anAudQualUpDown.Value = (info.QDefault * MiscUtils.GetAudioBitrateMultiplier(channels)).RoundToInt();
                form.av1anAudQualUpDown.Text = form.av1anAudQualUpDown.Value.ToString();
            }
            else
            {
                form.av1anAudQualUpDown.Value = 0;
                form.av1anAudQualUpDown.Text = "";
            }
        }

        #region Load Info After Selecting Encoder

        static void LoadQualityLevel(CodecInfo info)
        {
            if (IsUsingVmaf())
                return;

            if (info.QMax > 0)
                form.av1anQualityUpDown.Maximum = info.QMax;
            else
                form.av1anQualityUpDown.Maximum = 100;

            form.av1anQualityUpDown.Minimum = info.QMin;

            if (info.QDefault >= 0)
                form.av1anQualityUpDown.Value = info.QDefault;
            else
                form.av1anQualityUpDown.Text = "";
        }

        static void LoadPresets(CodecInfo info)
        {
            form.av1anPresetBox.Items.Clear();

            if (info.Presets != null)
                foreach (string p in info.Presets)
                    form.av1anPresetBox.Items.Add(p.ToTitleCase()); // Add every preset to the dropdown

            if (form.av1anPresetBox.Items.Count > 0)
                form.av1anPresetBox.SelectedIndex = info.PresetDef; // Select default preset
        }

        static void LoadColorFormats(CodecInfo info)
        {
            form.av1anColorsBox.Items.Clear();

            if (info.ColorFormats != null)
                foreach (string p in info.ColorFormats)
                    form.av1anColorsBox.Items.Add(p.ToUpper()); // Add every pix_fmt to the dropdown

            if (form.av1anColorsBox.Items.Count > 0)
                form.av1anColorsBox.SelectedIndex = info.ColorFormatDef; // Select default pix_fmt
        }

        #endregion

        #region Get Current Codec

        public static Codecs.Av1anCodec GetCurrentCodecV()
        {
            return (Codecs.Av1anCodec)form.av1anCodecBox.SelectedIndex;
        }

        public static Codecs.AudioCodec GetCurrentCodecA()
        {
            return (Codecs.AudioCodec)form.av1anAudCodecBox.SelectedIndex;
        }

        #endregion

        public static Dictionary<string, string> GetVideoArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("q", form.av1anQualityUpDown.Value.ToString());
            dict.Add("preset", form.av1anPresetBox.Text.ToLower());
            dict.Add("pixFmt", form.av1anColorsBox.Text.ToLower());
            dict.Add("grainSynthStrength", form.av1anGrainSynthStrengthUpDown.Value.ToString());
            dict.Add("grainSynthDenoise", form.av1anGrainSynthDenoiseBox.Checked.ToString());
            return dict;
        }

        public static Dictionary<string, string> GetAudioArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("bitrate", form.av1anAudQualUpDown.Text.ToLower());
            dict.Add("ac", form.av1anAudChannelsBox.Text.Split(' ')[0].Trim());
            return dict;
        }

        #region Get Args

        public static async Task<string> GetVideoFilterArgs(CodecArgs codecArgs = null)
        {
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (TrackList.current.VideoStreams.Count < 1)
                return "";

            VideoStream vs = TrackList.current.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = form.av1anScaleBoxW.Text.Trim().ToLower();
            string scaleH = form.av1anScaleBoxH.Text.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(MiscUtils.GetScaleFilter(scaleW, scaleH));

            if (form.av1anCropBox.SelectedIndex > 0) // Check Filter: Crop/Cropdetect
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(false));

            filters = filters.Where(x => x.Trim().Length > 2).ToList(); // Strip empty filters

            if (filters.Count > 0)
                return $"-vf {string.Join(",", filters)}";
            else
                return "";
        }

        public static string GetSplittingMethod()
        {
            if (form.av1anOptsSplitModeBox.SelectedIndex == 0)
                return $"none";
            else
                return $"av-scenechange";
        }

        public static string GetChunkGenMethod()
        {
            switch (form.av1anOptsChunkModeBox.SelectedIndex)
            {
                case 0: return "hybrid";
                case 1: return "lsmash";
                case 3: return "ffms2";
                case 4: return "segment";
                case 5: return "select";
            }

            return "";
        }

        public static string GetOutPath()
        {
            return form.av1anOutputPathBox.Text.Trim();
        }

        #endregion

        public static void ValidateContainer()
        {
            if (form.av1anContainerBox.SelectedIndex < 0)
                return;

            Codecs.AudioCodec aCodec = (Codecs.AudioCodec)form.av1anAudCodecBox.SelectedIndex;

            Containers.Container c = (Containers.Container)form.av1anContainerBox.SelectedIndex;

            if (!Containers.ContainerSupports(c, aCodec))
            {
                Containers.Container supported = Containers.Container.Mkv;

                for (int i = 0; i < form.av1anContainerBox.Items.Count; i++)
                    if (form.av1anContainerBox.Items[i].ToString().ToUpper() == supported.ToString().ToUpper())
                        form.av1anContainerBox.SelectedIndex = i;
            }

            Containers.Container current = MiscUtils.ParseEnum<Containers.Container>(form.av1anContainerBox.Text);
            string path = Path.ChangeExtension(form.av1anOutputPathBox.Text.Trim(), current.ToString().ToLower());
            form.av1anOutputPathBox.Text = path;
            ValidatePath();
        }

        public static void ValidatePath()
        {
            if (TrackList.current == null)
                return;

            if (File.Exists(form.av1anOutputPathBox.Text))
                form.av1anOutputPathBox.Text = IoUtils.GetAvailableFilename(form.av1anOutputPathBox.Text);
        }

        public static Fraction GetUiFps()
        {
            TextBox fpsBox = form.av1anFpsBox;
            return MiscUtils.GetFpsFromString(fpsBox.Text);
        }

        public static void AskDeleteTempFolder (string dir)
        {
            DialogResult dialog = MessageBox.Show($"Do you want to keep the temporary folder of this video for resuming?", "Keep temp folder?", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.No)
                IoUtils.TryDeleteIfExists(dir);
        }

        public static bool IsUsingVmaf ()
        {
            return form.av1anQualModeBox.SelectedIndex == 1;
        }
    }
}
