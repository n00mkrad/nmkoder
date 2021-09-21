using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
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
    partial class QuickConvertUi : QuickConvert
    {
        private static MainForm form;
        public static bool scaleLink = true;


        public static void Init()
        {
            form = Program.mainForm;

            foreach (Codecs.VideoCodec c in Enum.GetValues(typeof(Codecs.VideoCodec)))  // Load video codecs
                form.encVidCodecsBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encVidCodecsBox);

            foreach (Codecs.AudioCodec c in Enum.GetValues(typeof(Codecs.AudioCodec)))  // Load audio codecs
                form.encAudEnc.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encAudEnc);

            foreach (Codecs.SubtitleCodec c in Enum.GetValues(typeof(Codecs.SubtitleCodec)))  // Load audio codecs
                form.encSubEnc.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encSubEnc);

            foreach (string c in Enum.GetNames(typeof(Containers.Container)))   // Load containers
                form.containerBox.Items.Add(c.ToUpper());

            ConfigParser.LoadComboxIndex(form.containerBox);
        }

        public static void InitFile()
        {
            try
            {
                if (!RunTask.runningBatch) // Don't load new values into UI in batch mode since we apply the same for all files
                {
                    Program.mainForm.encScaleBoxW.Text = Program.mainForm.encScaleBoxH.Text = "";
                    InitAudioChannels(MediaInfo.current.AudioStreams.FirstOrDefault()?.Channels);
                    InitBurnCombox();
                    LoadMetadataGrid();
                }

                ValidateContainer();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialized media file: {e.Message}\n{e.StackTrace}");
            }
        }

        public static void VidEncoderSelected(int index)
        {
            Codecs.VideoCodec c = (Codecs.VideoCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);

            bool enc = !(c == Codecs.VideoCodec.Copy || c == Codecs.VideoCodec.StripVideo);
            Program.mainForm.encVidQualityBox.Enabled = enc;
            Program.mainForm.encVidPresetBox.Enabled = enc;
            Program.mainForm.encVidColorsBox.Enabled = enc;
            Program.mainForm.encVidFpsBox.Enabled = enc;
            Program.mainForm.encScaleBoxW.Enabled = Program.mainForm.encScaleBoxH.Enabled = enc;
            Program.mainForm.encCropModeBox.Enabled = enc;

            Program.mainForm.qInfoLabel.Text = info.QInfo;
            LoadQualityLevel(info);
            LoadPresets(info);
            LoadColorFormats(info);
            ValidateContainer();
        }

        public static void AudEncoderSelected(int index)
        {
            Codecs.AudioCodec c = (Codecs.AudioCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);

            Program.mainForm.encAudCh.Enabled = !(c == Codecs.AudioCodec.Copy || c == Codecs.AudioCodec.StripAudio);
            LoadAudBitrate(info);
            ValidateContainer();
        }

        #region Load Video Options

        static void LoadQualityLevel(CodecInfo info)
        {
            if (info.QMax > 0)
                form.encVidQualityBox.Maximum = info.QMax;
            else
                form.encVidQualityBox.Maximum = 100;

            form.encVidQualityBox.Minimum = info.QMin;

            if (info.QDefault >= 0)
                form.encVidQualityBox.Text = info.QDefault.ToString();
            else
                form.encVidQualityBox.Text = "";
        }

        static void LoadPresets(CodecInfo info)
        {
            form.encVidPresetBox.Items.Clear();

            if (info.Presets != null)
                foreach (string p in info.Presets)
                    form.encVidPresetBox.Items.Add(p.ToTitleCase()); // Add every preset to the dropdown

            if (form.encVidPresetBox.Items.Count > 0)
                form.encVidPresetBox.SelectedIndex = info.PresetDef; // Select default preset
        }

        static void LoadColorFormats(CodecInfo info)
        {
            form.encVidColorsBox.Items.Clear();

            if (info.ColorFormats != null)
                foreach (string p in info.ColorFormats)
                    form.encVidColorsBox.Items.Add(p.ToUpper()); // Add every pix_fmt to the dropdown

            if (form.encVidColorsBox.Items.Count > 0)
                form.encVidColorsBox.SelectedIndex = info.ColorFormatDef; // Select default pix_fmt
        }

        #endregion

        #region Load Audio Options

        static void LoadAudBitrate(CodecInfo info)
        {
            if (info.QDefault >= 0)
                form.encAudBr.Text = info.QDefault.ToString();
            else
                form.encAudBr.Text = "";
        }

        #endregion

        public static void ValidateContainer()
        {
            if (form.containerBox.SelectedIndex < 0)
                return;

            Codecs.VideoCodec vCodec = (Codecs.VideoCodec)form.encVidCodecsBox.SelectedIndex;
            Codecs.AudioCodec aCodec = (Codecs.AudioCodec)form.encAudEnc.SelectedIndex;
            Codecs.SubtitleCodec sCodec = (Codecs.SubtitleCodec)form.encSubEnc.SelectedIndex;

            Containers.Container c = (Containers.Container)form.containerBox.SelectedIndex;

            if (!(Containers.ContainerSupports(c, vCodec) && Containers.ContainerSupports(c, aCodec) && Containers.ContainerSupports(c, sCodec)))
            {
                Containers.Container supported = Containers.GetSupportedContainer(vCodec, aCodec, sCodec);

                Logger.Log($"{c.ToString().ToUpper()} doesn't support one of the selected codecs - Auto-selected {supported.ToString().ToUpper()} instead.");

                for (int i = 0; i < form.containerBox.Items.Count; i++)
                    if (form.containerBox.Items[i].ToString().ToUpper() == supported.ToString().ToUpper())
                        form.containerBox.SelectedIndex = i;
            }

            Containers.Container current = (Containers.Container)form.containerBox.SelectedIndex;
            string path = Path.ChangeExtension(form.outputBox.Text.Trim(), current.ToString().ToLower());
            Program.mainForm.outputBox.Text = path;
            ValidatePath();
        }

        public static void ValidatePath()
        {
            if (MediaInfo.current == null)
                return;

            string ext = Program.mainForm.containerBox.Text.ToLower();
            Program.mainForm.outputBox.Text = IoUtils.GetAvailableFilename(Path.ChangeExtension(MediaInfo.current.Path, ext));
        }

        #region Get Current Codec

        public static Codecs.VideoCodec GetCurrentCodecV()
        {
            return (Codecs.VideoCodec)form.encVidCodecsBox.SelectedIndex;
        }

        public static Codecs.AudioCodec GetCurrentCodecA()
        {
            return (Codecs.AudioCodec)form.encAudEnc.SelectedIndex;
        }

        public static Codecs.SubtitleCodec GetCurrentCodecS()
        {
            return (Codecs.SubtitleCodec)form.encSubEnc.SelectedIndex;
        }

        #endregion

        public static Dictionary<string, string> GetVideoArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("q", form.encVidQualityBox.Value.ToString());
            dict.Add("preset", form.encVidPresetBox.Text.ToLower());
            dict.Add("pixFmt", form.encVidColorsBox.Text.ToLower());
            return dict;
        }

        public static Dictionary<string, string> GetAudioArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("bitrate", form.encAudBr.Text.ToLower());
            dict.Add("ac", form.encAudCh.Text.Split(' ')[0].Trim());
            return dict;
        }

        #region Load Media Info Into UI Where Needed

        public static void InitAudioChannels(int? ch)
        {
            if (ch == null || ch < 1)
            {
                Logger.Log($"SetAudioChannelsCombox: ch is null or < 1 - returning", true);
                form.encAudCh.SelectedIndex = 1;
                return;
            }

            for (int i = 0; i < form.encAudCh.Items.Count; i++)
            {
                if (form.encAudCh.Items[i].ToString().Split(' ').First().GetInt() == ch)
                    form.encAudCh.SelectedIndex = i;
            }
        }

        public static void InitBurnCombox()
        {
            ComboBox burnBox = Program.mainForm.encSubBurnBox;

            burnBox.Items.Clear();
            burnBox.Items.Add("Disabled");

            for (int i = 0; i < MediaInfo.current.SubtitleStreams.Count; i++)
            {
                string lang = MediaInfo.current.SubtitleStreams[i].Language.Trim();
                burnBox.Items.Add($"Subtitle Track {i + 1}{(lang.Length > 1 ? $" ({lang})" : "")}");
            }

            burnBox.SelectedIndex = 0;
        }

        #endregion

        #region Metadata Tab 

        public static string lastMap = "";

        public static void LoadMetadataGrid()
        {
            if (MediaInfo.current == null)
                return;

            string currMap = MediaInfo.GetMapArgs();

            if (currMap == lastMap)
                return;

            lastMap = currMap;

            DataGridView grid = Program.mainForm.metaGrid;
            MediaFile c = MediaInfo.current;

            if (grid.Columns.Count != 3)
            {
                grid.Columns.Clear();
                grid.Columns.Add("1", "Track");
                grid.Columns.Add("2", "Title");
                grid.Columns.Add("3", "Lang");
            }

            grid.Rows.Clear();

            grid.Rows.Add($"File", MediaInfo.current.Title, MediaInfo.current.Language);

            List<VideoStream> vStreams = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(e => e.Stream.Type == Stream.StreamType.Video).Select(s => (VideoStream)s.Stream).ToList();
            List<AudioStream> aStreams = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(e => e.Stream.Type == Stream.StreamType.Audio).Select(s => (AudioStream)s.Stream).ToList();
            List<SubtitleStream> sStreams = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(e => e.Stream.Type == Stream.StreamType.Subtitle).Select(s => (SubtitleStream)s.Stream).ToList();

            for (int i = 0; i < vStreams.Count; i++)
                if (Program.mainForm.streamListBox.GetItemChecked(vStreams[i].Index))
                    grid.Rows.Add($"Video Track {i + 1}", vStreams[i].Title, vStreams[i].Language);

            for (int i = 0; i < aStreams.Count; i++)
                if (Program.mainForm.streamListBox.GetItemChecked(aStreams[i].Index))
                    grid.Rows.Add($"Audio Track {i + 1}", aStreams[i].Title, aStreams[i].Language);

            for (int i = 0; i < sStreams.Count; i++)
                if (Program.mainForm.streamListBox.GetItemChecked(sStreams[i].Index))
                    grid.Rows.Add($"Subtitle Track {i + 1}", sStreams[i].Title, sStreams[i].Language);

            grid.Columns[0].ReadOnly = true;
            grid.Columns[0].AutoSizeMode = grid.Columns[1].AutoSizeMode = grid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns[0].FillWeight = 15;
            grid.Columns[1].FillWeight = 75;
            grid.Columns[2].FillWeight = 10;
        }

        public static string GetMetadataArgs()
        {
            bool attachments = MediaInfo.current.AttachmentStreams.Count > 0;
            string stripStr = attachments ? ":s:t 0:s:t" : " -1"; // If there are attachments, only copy the attachment metadata, otherwise none
            int cfg = Config.GetInt(Config.Key.metaMode);

            if (cfg == 2) // 2 = Strip All
                return $"-map_metadata{stripStr}"; 

            bool map = cfg == 0;  // 0 = Copy + Apply Editor Tags - 1 = Strip Others + Apply Editor Tags
            DataGridView grid = Program.mainForm.metaGrid;
            List<string> args = new List<string>();

            foreach (DataGridViewRow row in grid.Rows)
            {
                string track = row.Cells[0].Value?.ToString();
                string title = row.Cells[1].Value?.ToString().Trim();
                string lang = row.Cells[2].Value?.ToString().Trim();

                int idx = track.GetInt() - 1;

                if (!map && string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(lang))
                    continue;

                if (idx < 0)
                {
                    args.Add($"-metadata title=\"{title}\"");
                }
                else
                {
                    if (track.ToLower().Contains("video"))
                        args.Add($"-metadata:s:v:{idx} title=\"{title}\" -metadata:s:s:{idx} language=\"{lang}\"");

                    if (track.ToLower().Contains("audio"))
                        args.Add($"-metadata:s:a:{idx} title=\"{title}\" -metadata:s:s:{idx} language=\"{lang}\"");

                    if (track.ToLower().Contains("subtitle"))
                        args.Add($"-metadata:s:s:{idx} title=\"{title}\" -metadata:s:s:{idx} language=\"{lang}\"");
                }
            }

            return $"-map_metadata{(map ? " 0" : stripStr)} {string.Join(" ", args)}";
        }

        #endregion

        #region Get Args

        public static string GetMuxingArgsFromUi()
        {
            Containers.Container c = (Containers.Container)Program.mainForm.containerBox.SelectedIndex;
            return Containers.GetMuxingArgs(c);
        }

        public static async Task<string> GetVideoFilterArgs(Codecs.VideoCodec vCodec, CodecArgs codecArgs = null)
        {
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (MediaInfo.current.VideoStreams.Count < 1 || (vCodec == Codecs.VideoCodec.Copy || vCodec == Codecs.VideoCodec.StripVideo))
                return "";

            VideoStream vs = MediaInfo.current.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if (Program.mainForm.encSubBurnBox.SelectedIndex > 0) // Check Filter: Subtitle Burn-In
            {
                string filename = MediaInfo.current.Path.Replace(@"\", @"\\\\").Replace(@":\\\\", @"\\:\\\\"); // https://trac.ffmpeg.org/ticket/3334
                filters.Add($"subtitles={filename.Wrap()}:si={Program.mainForm.encSubBurnBox.Text.GetInt() - 1}");
            }

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = Program.mainForm.encScaleBoxW.Text.Trim().ToLower();
            string scaleH = Program.mainForm.encScaleBoxH.Text.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(GetScaleFilter(scaleW, scaleH));

            if (Program.mainForm.encCropModeBox.SelectedIndex > 0) // Check Filter: Crop/Cropdetect
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(false));

            if (filters.Count > 0)
                return $"-vf {string.Join(",", filters.Where(x => x.Trim().Length > 2))}";
            else
                return "";
        }

        private static string GetScaleFilter(string w, string h, bool logWarnings = true)
        {
            string argW = w.Replace("w", "iw").Replace("h", "ih");
            string argH = h.Replace("w", "iw").Replace("h", "ih");

            if (w.EndsWith("%"))
                argW = $"iw*{((float)w.GetInt() / 100).ToStringDot()}";
            else if (string.IsNullOrWhiteSpace(w))
                argW = "-2";

            if (h.EndsWith("%"))
                argH = $"ih*{((float)h.GetInt() / 100).ToStringDot()}";
            else if (string.IsNullOrWhiteSpace(h))
                argH = "-2";

            string forceDiv = (argW.Contains("*") || argH.Contains("*")) ? ":force_original_aspect_ratio=increase:force_divisible_by=2" : "";

            if (logWarnings && forceDiv.Length > 0 && (argW.Contains("*") && argH.Contains("*")))
                Logger.Log($"Info: Scaling using percentages enforces the original aspect ratio. You cannot use different percentages for width and height.");

            return $"scale={argW}:{argH}{forceDiv},setsar=1:1";
        }

        #endregion

        public static Fraction GetUiFps()
        {
            TextBox fpsBox = Program.mainForm.encVidFpsBox;

            if (Program.mainForm.encVidFpsBox.Text.Contains("/"))   // Parse fraction
            {
                string[] split = fpsBox.Text.Split('/');
                Fraction frac = new Fraction(split[0].GetInt(), split[1].GetInt());

                if (!fpsBox.ReadOnly)
                    return frac;
            }
            else    // Parse float
            {
                fpsBox.Text = fpsBox.Text.TrimNumbers(true);

                if (!fpsBox.ReadOnly)
                    return new Fraction(fpsBox.GetFloat());
            }

            return new Fraction();
        }
    }
}
