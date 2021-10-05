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
    partial class QuickConvertUi : QuickConvert
    {
        private static MainForm form;

        public static void Init()
        {
            form = Program.mainForm;

            foreach (Codecs.VideoCodec c in Enum.GetValues(typeof(Codecs.VideoCodec)))  // Load video codecs
                form.encVidCodecsBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encVidCodecsBox);

            foreach (QuickConvert.QualityMode qm in Enum.GetValues(typeof(QuickConvert.QualityMode)))  // Load quality modes
                form.encQualModeBox.Items.Add(qm.ToString().Replace("Crf", "CRF").Replace("TargetKbps", "Target Bitrate (Kbps)").Replace("TargetMbytes", "Target Filesize (MB)"));

            form.encQualModeBox.SelectedIndex = 0;

            foreach (Codecs.AudioCodec c in Enum.GetValues(typeof(Codecs.AudioCodec)))  // Load audio codecs
                form.encAudCodecBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encAudCodecBox);

            foreach (Codecs.SubtitleCodec c in Enum.GetValues(typeof(Codecs.SubtitleCodec)))  // Load audio codecs
                form.encSubCodecBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encSubCodecBox);

            foreach (string c in Enum.GetNames(typeof(Containers.Container)))   // Load containers
                form.ffmpegContainerBox.Items.Add(c.ToUpper());

            ConfigParser.LoadComboxIndex(form.ffmpegContainerBox);
        }

        public static void InitFile(string path)
        {
            try
            {
                Program.mainForm.ffmpegOutputBox.Text = path;

                if (!RunTask.runningBatch) // Don't load new values into UI in batch mode since we apply the same for all files
                {
                    Program.mainForm.encScaleBoxW.Text = Program.mainForm.encScaleBoxH.Text = "";
                    InitAudioChannels(TrackList.current.AudioStreams.FirstOrDefault()?.Channels);
                    InitBurnCombox();
                    LoadMetadataGrid();
                }

                ValidateContainer();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialize media file: {e.Message}\n{e.StackTrace}");
            }
        }

        public static void VidEncoderSelected(int index)
        {
            Codecs.VideoCodec c = (Codecs.VideoCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);
            Program.mainForm.ffmpegContainerBox.Visible = !Codecs.IsFixedFormat(c); // Disable container selection for fixed formats (GIF, PNG etc)
            bool enc = !(c == Codecs.VideoCodec.Copy || c == Codecs.VideoCodec.StripVideo);
            bool noRateControl = c == Codecs.VideoCodec.Gif || c == Codecs.VideoCodec.Png || c == Codecs.VideoCodec.Jpg;
            Program.mainForm.encVidQualityBox.Enabled = enc && info.QMin != info.QMax;
            Program.mainForm.encQualModeBox.Enabled = enc && !noRateControl;
            Program.mainForm.encVidPresetBox.Enabled = enc && info.Presets.Length > 0;
            Program.mainForm.encVidColorsBox.Enabled = enc && info.ColorFormats.Length > 0;
            Program.mainForm.encVidFpsBox.Enabled = enc;
            Program.mainForm.encScaleBoxW.Enabled = Program.mainForm.encScaleBoxH.Enabled = enc;
            Program.mainForm.encCropModeBox.Enabled = enc;
            Program.mainForm.qInfoLabel.Text = info.QInfo;
            Program.mainForm.presetInfoLabel.Text = info.PInfo;
            LoadQualityLevel(info);
            LoadPresets(info);
            LoadColorFormats(info);
            ValidateContainer();
        }

        public static void AudEncoderSelected(int index)
        {
            Codecs.AudioCodec c = (Codecs.AudioCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);

            Program.mainForm.encAudChannelsBox.Enabled = !(c == Codecs.AudioCodec.Copy || c == Codecs.AudioCodec.StripAudio);
            Program.mainForm.encAudQualUpDown.Enabled = info.QDefault >= 0;
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
                form.encVidQualityBox.Value = info.QDefault;
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
            int channels = form.encAudChannelsBox.Text.Split(' ')[0].GetInt();

            if (info.QDefault >= 0)
            {
                form.encAudQualUpDown.Value = (info.QDefault * MiscUtils.GetAudioBitrateMultiplier(channels)).RoundToInt();
                form.encAudQualUpDown.Text = form.encAudQualUpDown.Value.ToString();
            }
            else
            {
                form.encAudQualUpDown.Value = 0;
                form.encAudQualUpDown.Text = "";
            }
        }

        #endregion

        public static void ValidateContainer()
        {
            if (form.ffmpegContainerBox.SelectedIndex < 0)
                return;

            Codecs.VideoCodec vCodec = (Codecs.VideoCodec)form.encVidCodecsBox.SelectedIndex;
            Codecs.AudioCodec aCodec = (Codecs.AudioCodec)form.encAudCodecBox.SelectedIndex;
            Codecs.SubtitleCodec sCodec = (Codecs.SubtitleCodec)form.encSubCodecBox.SelectedIndex;

            Containers.Container c = (Containers.Container)form.ffmpegContainerBox.SelectedIndex;

            if (!(Containers.ContainerSupports(c, vCodec) && Containers.ContainerSupports(c, aCodec) && Containers.ContainerSupports(c, sCodec)))
            {
                Containers.Container supported = Containers.GetSupportedContainer(vCodec, aCodec, sCodec);

                //Logger.Log($"{c.ToString().ToUpper()} doesn't support one of the selected codecs - Auto-selected {supported.ToString().ToUpper()} instead.");

                for (int i = 0; i < form.ffmpegContainerBox.Items.Count; i++)
                    if (form.ffmpegContainerBox.Items[i].ToString().ToUpper() == supported.ToString().ToUpper())
                        form.ffmpegContainerBox.SelectedIndex = i;
            }

            Containers.Container current = MiscUtils.ParseEnum<Containers.Container>(form.ffmpegContainerBox.Text);
            bool noExt = Codecs.IsFixedFormat(vCodec);
            string path = Path.ChangeExtension(form.ffmpegOutputBox.Text.Trim(), noExt ? null : current.ToString().ToLower());
            Program.mainForm.ffmpegOutputBox.Text = path;
            ValidatePath();
        }

        public static void ValidatePath()
        {
            if (TrackList.current == null)
                return;

            //string ext = Program.mainForm.containerBox.Text.ToLower();

            if(File.Exists(Program.mainForm.ffmpegOutputBox.Text))
                Program.mainForm.ffmpegOutputBox.Text = IoUtils.GetAvailableFilename(Program.mainForm.ffmpegOutputBox.Text);
        }

        #region Get Current Codec

        public static Codecs.VideoCodec GetCurrentCodecV()
        {
            return (Codecs.VideoCodec)form.encVidCodecsBox.SelectedIndex;
        }

        public static Codecs.AudioCodec GetCurrentCodecA()
        {
            return (Codecs.AudioCodec)form.encAudCodecBox.SelectedIndex;
        }

        public static Codecs.SubtitleCodec GetCurrentCodecS()
        {
            return (Codecs.SubtitleCodec)form.encSubCodecBox.SelectedIndex;
        }

        #endregion

        public static Dictionary<string, string> GetVideoArgsFromUi(bool twoPass)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (twoPass)
                dict.Add("bitrate", GetBitrate());
            else
                dict.Add("q", form.encVidQualityBox.Value.ToString());
                
            dict.Add("preset", form.encVidPresetBox.Text.ToLower());
            dict.Add("pixFmt", form.encVidColorsBox.Text.ToLower());

            return dict;
        }

        private static string GetBitrate()
        {
            if((QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.TargetKbps)
            {
                string br = form.encVidQualityBox.Text.ToLower().Trim();
                br = br.EndsWith("k") || br.EndsWith("m") ? br : $"{br.GetInt()}k";
                return br;
            }

            if ((QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.TargetMbytes)
            {
                double durationSecs = TrackList.current.DurationMs / (double)1000;
                float targetMbytes = form.encVidQualityBox.Text.GetFloat();
                long targetBits = (long)Math.Round(targetMbytes * 8 * 1024 * 1024); 
                int targetBitrate = (int)Math.Floor(targetBits / durationSecs); // Round down since undershooting is better than overshooting here
                Logger.Log($"GetBitrate - TargetMbytes Mode - Distributing {targetMbytes} megabytes ({(float)targetBits / 1024} kbits) over {durationSecs} secs => {targetBitrate} bps => ~{(float)targetBitrate / 1024} kbps bitrate");
                return $"{targetBitrate}";
            }

            return "0";
        }

        public static Dictionary<string, string> GetAudioArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("bitrate", form.encAudQualUpDown.Text.ToLower());
            dict.Add("ac", form.encAudChannelsBox.Text.Split(' ')[0].Trim());
            return dict;
        }

        #region Load Media Info Into UI Where Needed

        public static void InitAudioChannels(int? ch)
        {
            if (ch == null || ch < 1)
            {
                form.encAudChannelsBox.SelectedIndex = 1;
                return;
            }

            for (int i = 0; i < form.encAudChannelsBox.Items.Count; i++)
            {
                if (form.encAudChannelsBox.Items[i].ToString().Split(' ').First().GetInt() == ch)
                    form.encAudChannelsBox.SelectedIndex = i;
            }
        }

        public static void InitBurnCombox()
        {
            ComboBox burnBox = Program.mainForm.encSubBurnBox;

            burnBox.Items.Clear();
            burnBox.Items.Add("Disabled");

            for (int i = 0; i < TrackList.current.SubtitleStreams.Count; i++)
            {
                string lang = TrackList.current.SubtitleStreams[i].Language.Trim();
                burnBox.Items.Add($"Subtitle Track {i + 1}{(lang.Length > 1 ? $" ({lang})" : "")}");
            }

            burnBox.SelectedIndex = 0;
        }

        #endregion

        #region Metadata Tab 

        public static string lastMap = "";

        public static void LoadMetadataGrid()
        {
            if (TrackList.current == null)
                return;

            string currMap = TrackList.GetMapArgs();

            if (currMap == lastMap)
                return;

            lastMap = currMap;

            DataGridView grid = Program.mainForm.metaGrid;
            MediaFile c = TrackList.current;

            if (grid.Columns.Count != 3)
            {
                grid.Columns.Clear();
                grid.Columns.Add("1", "Track");
                grid.Columns.Add("2", "Title");
                grid.Columns.Add("3", "Lang");
            }

            grid.Rows.Clear();

            grid.Rows.Add($"File", TrackList.current.Title, TrackList.current.Language);

            var checkStreamEntries = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(x => Program.mainForm.streamListBox.CheckedItems.Contains(x));
            List<VideoStream> vStreams = checkStreamEntries.Where(e => e.Stream.Type == Stream.StreamType.Video).Select(s => (VideoStream)s.Stream).ToList();

            List<AudioStream> aStreams = checkStreamEntries.Where(e => e.Stream.Type == Stream.StreamType.Audio).Select(s => (AudioStream)s.Stream).ToList();
            
            List<SubtitleStream> sStreams = checkStreamEntries.Where(e => e.Stream.Type == Stream.StreamType.Subtitle).Select(s => (SubtitleStream)s.Stream).ToList();

            for (int i = 0; i < vStreams.Count; i++)
                    grid.Rows.Add($"Video Track {i + 1}", vStreams[i].Title, vStreams[i].Language);

            for (int i = 0; i < aStreams.Count; i++)
                    grid.Rows.Add($"Audio Track {i + 1}", aStreams[i].Title, aStreams[i].Language);

            for (int i = 0; i < sStreams.Count; i++)
                    grid.Rows.Add($"Subtitle Track {i + 1}", sStreams[i].Title, sStreams[i].Language);

            grid.Columns[0].ReadOnly = true;
            grid.Columns[0].AutoSizeMode = grid.Columns[1].AutoSizeMode = grid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns[0].FillWeight = 20;
            grid.Columns[1].FillWeight = 70;
            grid.Columns[2].FillWeight = 10;
        }

        public static string GetMetadataArgs()
        {
            bool attachments = TrackList.current.AttachmentStreams.Count > 0;
            string stripStr = attachments ? ":s:t 0:s:t" : " -1"; // If there are attachments, only copy the attachment metadata, otherwise none
            int cfg = Config.GetInt(Config.Key.metaMode);

            if (cfg == 2) // 2 = Strip All
                return $"-map_metadata{stripStr}"; 

            bool map = cfg == 0;  // 0 = Copy + Apply Editor Tags - 1 = Strip Others + Apply Editor Tags
            DataGridView grid = Program.mainForm.metaGrid;
            int defaultAudio = Program.mainForm.trackListDefaultAudioBox.SelectedIndex;
            int defaultSubs = Program.mainForm.trackListDefaultSubsBox.SelectedIndex - 1;
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
                        args.Add($"-metadata:s:v:{idx} title=\"{title}\" -metadata:s:v:{idx} language=\"{lang}\"");

                    if (track.ToLower().Contains("audio"))
                        args.Add($"-metadata:s:a:{idx} title=\"{title}\" -metadata:s:a:{idx} language=\"{lang}\" -disposition:a:{idx} {(defaultAudio == idx ? "default" : "0")}");

                    if (track.ToLower().Contains("subtitle"))
                        args.Add($"-metadata:s:s:{idx} title=\"{title}\" -metadata:s:s:{idx} language=\"{lang}\" -disposition:s:{idx} {(defaultSubs == idx ? "default" : "0")}");
                }
            }

            return $"-map_metadata{(map ? " 0" : stripStr)} {string.Join(" ", args)}";
        }

        #endregion

        #region Get Args

        public static string GetMuxingArgsFromUi()
        {
            Containers.Container c = (Containers.Container)Program.mainForm.ffmpegContainerBox.SelectedIndex;
            return Containers.GetMuxingArgs(c);
        }

        public static async Task<string> GetVideoFilterArgs(Codecs.VideoCodec vCodec, CodecArgs codecArgs = null)
        {
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (TrackList.current.VideoStreams.Count < 1 || (vCodec == Codecs.VideoCodec.Copy || vCodec == Codecs.VideoCodec.StripVideo))
                return "";

            VideoStream vs = TrackList.current.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if (Program.mainForm.encSubBurnBox.SelectedIndex > 0) // Check Filter: Subtitle Burn-In
            {
                string filename = FormatUtils.GetFilterPath(TrackList.current.TruePath);
                filters.Add($"subtitles={filename.Wrap()}:si={Program.mainForm.encSubBurnBox.Text.GetInt() - 1}");
            }

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = Program.mainForm.encScaleBoxW.Text.Trim().ToLower();
            string scaleH = Program.mainForm.encScaleBoxH.Text.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(MiscUtils.GetScaleFilter(scaleW, scaleH));

            if (Program.mainForm.encCropModeBox.SelectedIndex > 0) // Check Filter: Crop/Cropdetect
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(false));

            filters = filters.Where(x => x.Trim().Length > 2).ToList(); // Strip empty filters

            if (filters.Count > 0)
                return $"-vf {string.Join(",", filters)}";
            else
                return "";
        }

        public static string GetOutPath (Codecs.VideoCodec c)
        {
            string uiPath = Program.mainForm.ffmpegOutputBox.Text.Trim();

            if (Codecs.IsSequence(c))
            {
                Directory.CreateDirectory(uiPath);
                string ext = Program.mainForm.encVidCodecsBox.Text.Split(' ')[0].ToLower();
                return Path.Combine(uiPath, $"%8d.{ext}");
            }
            else
            {
                return uiPath;
            }
        }

        #endregion

        public static Fraction GetUiFps()
        {
            TextBox fpsBox = Program.mainForm.encVidFpsBox;
            return MiscUtils.GetFpsFromString(fpsBox.Text);
        }
    }
}
