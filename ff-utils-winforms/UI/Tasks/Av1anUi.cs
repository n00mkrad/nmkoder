using Newtonsoft.Json;
using Nmkoder.Data;
using Nmkoder.Data.Codecs;
using Nmkoder.Data.Colors;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    class Av1anUi
    {
        private static MainForm Form;
        public static CropConfig CurrentCrop;

        public static void Init()
        {
            Form = Program.mainForm;

            Form.av1anCodecBox.Items.AddRange(Enum.GetValues(typeof(CodecUtils.Av1anCodec)).Cast<CodecUtils.Av1anCodec>().Select(c => CodecUtils.GetCodec(c).FriendlyName).ToArray()); // Load video codecs

            ConfigParser.LoadComboxIndex(Form.av1anCodecBox);

            foreach (Av1an.QualityMode qm in Enum.GetValues(typeof(Av1an.QualityMode)))  // Load quality modes
                Form.av1anQualModeBox.Items.Add(qm.ToString().Replace("Crf", "CRF").Replace("TargetVmaf", "Target VMAF"));

            Form.av1anQualModeBox.SelectedIndex = 0;

            Form.av1anOptsSplitModeBox.SelectedIndex = 1;

            foreach (Av1an.ChunkMethod cm in Enum.GetValues(typeof(Av1an.ChunkMethod)))  // Load chunk modes
                Form.av1anOptsChunkModeBox.Items.Add(cm);

            Form.av1anAudCodecBox.Items.AddRange(Enum.GetValues(typeof(CodecUtils.AudioCodec)).Cast<CodecUtils.AudioCodec>().Select(c => CodecUtils.GetCodec(c).FriendlyName).ToArray()); // Load audio codecs

            ConfigParser.LoadComboxIndex(Form.av1anAudCodecBox);

            Form.av1anContainerBox.Items.Add(Containers.Container.Mkv.ToString().ToUpper());
            Form.av1anContainerBox.Items.Add(Containers.Container.Webm.ToString().ToUpper());
        }

        public static void InitFile(string path)
        {
            try
            {
                Form.av1anOutputPathBox.Text = path;

                if (!RunTask.runningBatch) // Don't load new values into UI in batch mode since we apply the same for all files
                {
                    InitAudioChannels(TrackList.current.File.AudioStreams.FirstOrDefault()?.Channels);
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
                Form.av1anAudChannelsBox.SelectedIndex = 1;
                return;
            }

            for (int i = 0; i < Form.av1anAudChannelsBox.Items.Count; i++)
            {
                if (Form.av1anAudChannelsBox.Items[i].ToString().Split(' ').First().GetInt() == ch)
                    Form.av1anAudChannelsBox.SelectedIndex = i;
            }
        }

        public static void VidEncoderSelected(int index)
        {
            CodecUtils.Av1anCodec c = (CodecUtils.Av1anCodec)index;
            IEncoder enc = CodecUtils.GetCodec(c);
            Form.qInfoLabel.Text = enc.QInfo;
            Form.presetInfoLabel.Text = enc.PresetInfo;
            bool av1 = c == CodecUtils.Av1anCodec.AomAv1 || c == CodecUtils.Av1anCodec.SvtAv1;
            Form.av1anGrainSynthStrengthUpDown.Enabled = Form.av1anGrainSynthDenoiseBox.Enabled = Form.av1anGrainSynthDenoiseBox.Enabled = av1; // Only AV1 has grain synth
            LoadQualityLevel(enc);
            LoadPresets(enc);
            LoadColorFormats(enc);
            LoadAdvancedArgsGrid(enc);
        }

        public static void AudEncoderSelected(int index)
        {
            CodecUtils.AudioCodec c = (CodecUtils.AudioCodec)index;
            IEncoder enc = CodecUtils.GetCodec(c);

            Form.av1anAudChannelsBox.Enabled = !(c == CodecUtils.AudioCodec.CopyAudio || c == CodecUtils.AudioCodec.StripAudio);
            Form.av1anAudQualUpDown.Enabled = enc.QDefault >= 0 && Math.Abs(enc.QMin - enc.QMax) > 0;
            LoadAudBitrate(enc);
            ValidateContainer();
        }

        static void LoadAudBitrate(IEncoder enc)
        {
            int channels = Form.av1anAudChannelsBox.Text.Split(' ')[0].GetInt();

            if (enc.QDefault >= 0)
            {
                Form.av1anAudQualUpDown.Value = (enc.QDefault * MiscUtils.GetAudioBitrateMultiplier(channels)).RoundToInt();
                Form.av1anAudQualUpDown.Text = Form.av1anAudQualUpDown.Value.ToString();
            }
            else
            {
                Form.av1anAudQualUpDown.Value = 0;
                Form.av1anAudQualUpDown.Text = "";
            }
        }

        #region Load Info After Selecting Encoder

        static void LoadQualityLevel(IEncoder enc)
        {
            if (IsUsingVmaf())
                return;

            if (enc.QMax > 0)
                Form.av1anQualityUpDown.Maximum = enc.QMax;
            else
                Form.av1anQualityUpDown.Maximum = 100;

            Form.av1anQualityUpDown.Minimum = enc.QMin;

            if (enc.QDefault >= 0)
                Form.av1anQualityUpDown.Value = enc.QDefault;
            else
                Form.av1anQualityUpDown.Text = "";
        }

        static void LoadPresets(IEncoder enc)
        {
            Form.av1anPresetBox.Items.Clear();

            if (enc.Presets != null)
                foreach (string p in enc.Presets)
                    Form.av1anPresetBox.Items.Add(p.ToTitleCase()); // Add every preset to the dropdown

            if (Form.av1anPresetBox.Items.Count > 0)
                Form.av1anPresetBox.SelectedIndex = enc.PresetDefault; // Select default preset
        }

        static void LoadColorFormats(IEncoder enc)
        {
            Form.av1anColorsBox.Items.Clear();

            if (enc.ColorFormats != null)
                foreach (PixelFormats p in enc.ColorFormats)
                    Form.av1anColorsBox.Items.Add(PixFmtUtils.GetFormat(p).FriendlyName); // Add every pix_fmt to the dropdown

            if (Form.av1anColorsBox.Items.Count > 0)
                Form.av1anColorsBox.SelectedIndex = enc.ColorFormatDefault; // Select default pix_fmt
        }

        public static void LoadAdvancedArgsGrid(IEncoder enc)
        {
            string jsonPath = Path.Combine(Paths.GetBinPath(), "av1an", "encoderArgs", enc.Name + ".json");

            DataGridView grid = Program.mainForm.Av1anAdvancedArgsGrid;
            grid.Rows.Clear();
            grid.Columns.Clear();

            if (!File.Exists(jsonPath))
                return;

            grid.Columns.Clear();
            grid.Columns.Add("0", "Argument");
            grid.Columns.Add("1", "Value");
            grid.Columns.Add("2", "Description, Possible Values");

            List<string[]> args = new List<string[]>();
            try
            {
                args = JsonConvert.DeserializeObject<List<string[]>>(File.ReadAllText(jsonPath));
            }
            catch (Exception e)
            {
                Logger.Log($"Error loading advanced arg JSON: {e.Message}");
                args = new List<string[]>();
            }

            foreach (string[] arg in args)
                grid.Rows.Add(arg[0], arg[1], arg[2]);

            grid.RowHeadersVisible = false;
            grid.Columns[0].ReadOnly = true;
            grid.Columns[2].ReadOnly = true;
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
            grid.Columns[0].FillWeight = 25;
            grid.Columns[1].FillWeight = 20;
            grid.Columns[2].FillWeight = 55;
        }

        #endregion

        #region Get Current Codec

        public static CodecUtils.Av1anCodec GetCurrentCodecV()
        {
            return (CodecUtils.Av1anCodec)Form.av1anCodecBox.SelectedIndex;
        }

        public static CodecUtils.AudioCodec GetCurrentCodecA()
        {
            return (CodecUtils.AudioCodec)Form.av1anAudCodecBox.SelectedIndex;
        }

        #endregion

        #region Get Args From UI

        public static Dictionary<string, string> GetVideoArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("qMode", Form.av1anQualModeBox.SelectedIndex.ToString());
            dict.Add("q", Form.av1anQualityUpDown.Value.ToString());
            dict.Add("preset", Form.av1anPresetBox.Text.ToLower());
            dict.Add("pixFmt", PixFmtUtils.GetFormat(CodecUtils.GetCodec((CodecUtils.Av1anCodec)Program.mainForm.av1anCodecBox.SelectedIndex).ColorFormats[Program.mainForm.av1anColorsBox.SelectedIndex]).Name);
            dict.Add("grainSynthStrength", Form.av1anGrainSynthStrengthUpDown.Value.ToString());
            dict.Add("grainSynthDenoise", Form.av1anGrainSynthDenoiseBox.Checked.ToString());
            dict.Add("threads", Form.av1anThreadsUpDown.Value.ToString());
            dict.Add("custom", Form.av1anCustomEncArgsBox.Text);
            dict.Add("advanced", string.Join(" ", Form.Av1anAdvancedArgsGrid.Rows.Cast<DataGridViewRow>().Select(x => $"--{x.Cells[0].Value}={x.Cells[1].Value}")));
            return dict;
        }

        public static Dictionary<string, string> GetAudioArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("bitrate", Form.av1anAudQualUpDown.Text.ToLower());
            dict.Add("ac", Form.av1anAudChannelsBox.Text.Split(' ')[0].Trim());
            return dict;
        }

        #endregion

        #region Get Args

        public static async Task<string> GetVideoFilterArgs(CodecArgs codecArgs = null)
        {
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (TrackList.current.File.VideoStreams.Count < 1)
                return "";

            VideoStream vs = TrackList.current.File.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = Form.av1anScaleBoxW.Text.Trim().ToLower();
            string scaleH = Form.av1anScaleBoxH.Text.Trim().ToLower();

            if (Program.mainForm.av1anCropBox.Text.ToLower().Contains("manual") && CurrentCrop != null) // Check Filter: Manual Crop
                filters.Add($"crop={CurrentCrop.GetFilterArgs(vs.Resolution)}");

            if (Program.mainForm.av1anCropBox.Text.ToLower().Contains("auto")) // Check Filter: Autocrop
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(TrackList.current.File.ImportPath, false));

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(MiscUtils.GetScaleFilter(scaleW, scaleH));

            filters.AddRange(GetCustomFilters());

            filters = filters.Where(x => x.Trim().Length > 2).ToList(); // Strip empty filters

            if (filters.Count > 0)
                return $"-vf {string.Join(",", filters)}";
            else
                return "";
        }

        private static List<string> GetCustomFilters ()
        {
            DataGridView grid = Program.mainForm.Av1anAdvancedFiltersGrid;
            return grid.Rows.Cast<DataGridViewRow>().ToList().Select(x => (string)x.Cells[0].Value).Where(x => x != null).ToList();
        }

        public static string GetSplittingMethodArgs()
        {
            return $"--split-method {(Form.av1anOptsSplitModeBox.SelectedIndex == 0 ? "none" : "av-scenechange")}";
        }

        public static string GetChunkGenMethod()
        {
            return $"-m {Form.av1anOptsChunkModeBox.Text.ToLower().Trim()}";
        }

        public static string GetConcatMethodArgs()
        {
            return $"-c {Form.av1anOptsConcatModeBox.Text.ToLower().Trim()}";
        }

        public static string GetChunkOrderArgs()
        {
            return $"--chunk-order {Form.av1anOptsChunkOrderBox.Text.Split('(')[0].ToLower().Trim()}";
        }

        public static string GetThreadAffArgs()
        {
            return $"--set-thread-affinity {Form.av1anThreadsUpDown.Value}";
        }

        public static string GetOutPath()
        {
            return Form.av1anOutputPathBox.Text.Trim();
        }

        #endregion


        public static void ValidateContainer()
        {
            if (Form.av1anContainerBox.SelectedIndex < 0)
                return;

            IEncoder aCodec = CodecUtils.GetCodec((CodecUtils.AudioCodec)Form.av1anAudCodecBox.SelectedIndex);
            Containers.Container c = (Containers.Container)Enum.Parse(typeof(Containers.Container), Form.av1anContainerBox.Text, true);

            if (!Containers.ContainerSupports(c, aCodec))
            {
                Containers.Container supported = Containers.Container.Mkv;

                for (int i = 0; i < Form.av1anContainerBox.Items.Count; i++)
                    if (Form.av1anContainerBox.Items[i].ToString().ToUpper() == supported.ToString().ToUpper())
                        Form.av1anContainerBox.SelectedIndex = i;

                Logger.Log($"{c.ToString().ToUpper()} does not support audio option '{aCodec.FriendlyName}' - Using {supported.ToString().ToUpper()} instead.");
            }

            Containers.Container current = MiscUtils.ParseEnum<Containers.Container>(Form.av1anContainerBox.Text);
            string path = Path.ChangeExtension(Form.av1anOutputPathBox.Text.Trim(), current.ToString().ToLower());
            Form.av1anOutputPathBox.Text = path;
            ValidatePath();
        }

        public static void InitAdvFilterGrid()
        {
            DataGridView grid = Program.mainForm.Av1anAdvancedFiltersGrid;
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("0", "Filter");
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
        }

        public static void ValidatePath()
        {
            if (TrackList.current == null)
                return;

            if (File.Exists(Form.av1anOutputPathBox.Text))
                Form.av1anOutputPathBox.Text = IoUtils.GetAvailableFilename(Form.av1anOutputPathBox.Text, ".av1an");
        }

        public static Fraction GetUiFps()
        {
            TextBox fpsBox = Form.av1anFpsBox;
            return MiscUtils.GetFpsFromString(fpsBox.Text);
        }

        public static void AskDeleteTempFolder(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
                return;

            int minKbytes = 4; // If the temp folder is smaller than this, delete it without asking
            var dirSize = IoUtils.GetDirSize(dir, true);

            if (RunTask.currentFileListMode == RunTask.FileListMode.Batch || RunTask.runningBatch || !Directory.Exists(Path.Combine(dir, "split")) || !File.Exists(Path.Combine(dir, "scenes.json")) || dirSize < minKbytes * 1024)
            {
                Logger.Log($"Temp folder has no scene detection data or is <{minKbytes}kb, deleting without asking", true);
                IoUtils.TryDeleteIfExists(dir);
                IoUtils.DeleteIfExists(dir + ".json");
                return;
            }

            string size = FormatUtils.Bytes(dirSize);
            string chunks = $"{IoUtils.GetFileInfosSorted(Path.Combine(dir, "encode"), false, "*.*").Where(x => x.Length >= 1024).Count()} encoded video chunks";
            string msg = $"Av1an has finished.\nDo you want to delete the temporary folder of this encode? It's {size} and contains {chunks}.";
            DialogResult dialog = MessageBox.Show(msg, "Delete av1an temp folder?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            Program.mainForm.Activate();

            if (dialog == DialogResult.Yes)
            {
                IoUtils.TryDeleteIfExists(dir);
                IoUtils.DeleteIfExists(dir + ".json");
            }
        }

        public static bool IsUsingVmaf()
        {
            return Form.av1anQualModeBox.SelectedIndex == 1;
        }
    }
}
