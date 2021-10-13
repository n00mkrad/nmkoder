using Nmkoder.Data;
using Nmkoder.Data.Codecs;
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

            form.encVidCodecsBox.Items.AddRange(Enum.GetValues(typeof(CodecUtils.VideoCodec)).Cast<CodecUtils.VideoCodec>().Select(c => CodecUtils.GetCodec(c).FriendlyName).ToArray()); // Load video codecs

            ConfigParser.LoadComboxIndex(form.encVidCodecsBox);

            foreach (QuickConvert.QualityMode qm in Enum.GetValues(typeof(QuickConvert.QualityMode)))  // Load quality modes
                form.encQualModeBox.Items.Add(qm.ToString().Replace("Crf", "CRF").Replace("TargetKbps", "Target Bitrate (Kbps)").Replace("TargetMbytes", "Target Filesize (MB)"));

            form.encQualModeBox.SelectedIndex = 0;

            form.encAudCodecBox.Items.AddRange(Enum.GetValues(typeof(CodecUtils.AudioCodec)).Cast<CodecUtils.AudioCodec>().Select(c => CodecUtils.GetCodec(c).FriendlyName).ToArray()); // Load audio codecs

            ConfigParser.LoadComboxIndex(form.encAudCodecBox);

            foreach (CodecUtils.SubtitleCodec c in Enum.GetValues(typeof(CodecUtils.SubtitleCodec)))  // Load audio codecs
                form.encSubCodecBox.Items.Add(CodecUtils.GetCodec(c).FriendlyName);

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
            CodecUtils.VideoCodec c = (CodecUtils.VideoCodec)index;
            IEncoder enc = CodecUtils.GetCodec(c);
            Program.mainForm.ffmpegContainerBox.Visible = !CodecUtils.GetCodec(c).IsFixedFormat; // Disable container selection for fixed formats (GIF, PNG etc)
            bool noRateControl = c == CodecUtils.VideoCodec.Gif || c == CodecUtils.VideoCodec.Png || c == CodecUtils.VideoCodec.Jpg;
            Program.mainForm.encVidQualityBox.Enabled = !enc.DoesNotEncode && enc.QMin != enc.QMax;
            Program.mainForm.encQualModeBox.Enabled = !enc.DoesNotEncode && !noRateControl;
            Program.mainForm.encVidPresetBox.Enabled = !enc.DoesNotEncode && enc.Presets.Length > 0;
            Program.mainForm.encVidColorsBox.Enabled = !enc.DoesNotEncode && enc.ColorFormats.Length > 0;
            Program.mainForm.encVidFpsBox.Enabled = !enc.DoesNotEncode;
            Program.mainForm.encScaleBoxW.Enabled = Program.mainForm.encScaleBoxH.Enabled = !enc.DoesNotEncode;
            Program.mainForm.encCropModeBox.Enabled = !enc.DoesNotEncode;
            Program.mainForm.qInfoLabel.Text = enc.QInfo;
            Program.mainForm.presetInfoLabel.Text = enc.PresetInfo;
            LoadQualityLevel(enc);
            LoadPresets(enc);
            LoadColorFormats(enc);
            ValidateContainer();
        }

        public static void AudEncoderSelected(int index)
        {
            CodecUtils.AudioCodec c = (CodecUtils.AudioCodec)index;
            IEncoder enc = CodecUtils.GetCodec(c);

            Program.mainForm.encAudChannelsBox.Enabled = !(c == CodecUtils.AudioCodec.CopyAudio || c == CodecUtils.AudioCodec.StripAudio);
            Program.mainForm.encAudQualUpDown.Enabled = enc.QDefault >= 0 && Math.Abs(enc.QMin - enc.QMax) > 0;
            LoadAudBitrate(enc);
            ValidateContainer();
        }

        #region Load Video Options

        static void LoadQualityLevel(IEncoder enc)
        {
            if(form.encQualModeBox.SelectedIndex == 0)
            {
                if (enc.QMax > 0)
                    form.encVidQualityBox.Maximum = enc.QMax;
                else
                    form.encVidQualityBox.Maximum = 100;

                form.encVidQualityBox.Minimum = enc.QMin;

                if (enc.QDefault >= 0)
                    form.encVidQualityBox.Value = enc.QDefault;
                else
                    form.encVidQualityBox.Text = "";
            }

            form.encVidQualityBox.Text = form.encVidQualityBox.Value.ToString();
        }

        static void LoadPresets(IEncoder enc)
        {
            form.encVidPresetBox.Items.Clear();

            if (enc.Presets != null)
                foreach (string p in enc.Presets)
                    form.encVidPresetBox.Items.Add(p.ToTitleCase()); // Add every preset to the dropdown

            if (form.encVidPresetBox.Items.Count > 0)
                form.encVidPresetBox.SelectedIndex = enc.PresetDefault; // Select default preset
        }

        static void LoadColorFormats(IEncoder enc)
        {
            form.encVidColorsBox.Items.Clear();

            if (enc.ColorFormats != null)
                foreach (string p in enc.ColorFormats)
                    form.encVidColorsBox.Items.Add(p.ToUpper()); // Add every pix_fmt to the dropdown

            if (form.encVidColorsBox.Items.Count > 0)
                form.encVidColorsBox.SelectedIndex = enc.ColorFormatDefault; // Select default pix_fmt
        }

        #endregion

        #region Load Audio Options

        static void LoadAudBitrate(IEncoder enc)
        {
            int channels = form.encAudChannelsBox.Text.Split(' ')[0].GetInt();

            if (enc.QDefault >= 0)
            {
                form.encAudQualUpDown.Value = (enc.QDefault * MiscUtils.GetAudioBitrateMultiplier(channels)).RoundToInt();
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

            CodecUtils.VideoCodec vCodec = (CodecUtils.VideoCodec)form.encVidCodecsBox.SelectedIndex;
            CodecUtils.AudioCodec aCodec = (CodecUtils.AudioCodec)form.encAudCodecBox.SelectedIndex;
            CodecUtils.SubtitleCodec sCodec = (CodecUtils.SubtitleCodec)form.encSubCodecBox.SelectedIndex;
            IEncoder encV = CodecUtils.GetCodec(vCodec);
            IEncoder encA = CodecUtils.GetCodec(aCodec);
            IEncoder encS = CodecUtils.GetCodec(sCodec);

            Containers.Container c = (Containers.Container)form.ffmpegContainerBox.SelectedIndex;

            if (!(Containers.ContainerSupports(c, encV) && Containers.ContainerSupports(c, encA) && Containers.ContainerSupports(c, encS)))
            {
                Containers.Container supported = Containers.GetSupportedContainer(encV, encA, encS);

                //Logger.Log($"{c.ToString().ToUpper()} doesn't support one of the selected codecs - Auto-selected {supported.ToString().ToUpper()} instead.");

                for (int i = 0; i < form.ffmpegContainerBox.Items.Count; i++)
                    if (form.ffmpegContainerBox.Items[i].ToString().ToUpper() == supported.ToString().ToUpper())
                        form.ffmpegContainerBox.SelectedIndex = i;
            }

            bool fixedFormat = CodecUtils.GetCodec(vCodec).IsFixedFormat;

            if (fixedFormat)
            {
                string format = vCodec.ToString().ToLower();
                Program.mainForm.ffmpegOutputBox.Text = Path.ChangeExtension(form.ffmpegOutputBox.Text.Trim(), format);
            }
            else
            {
                Containers.Container current = MiscUtils.ParseEnum<Containers.Container>(form.ffmpegContainerBox.Text);
                Program.mainForm.ffmpegOutputBox.Text = Path.ChangeExtension(form.ffmpegOutputBox.Text.Trim(), current.ToString().ToLower());
            }
            
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

        public static CodecUtils.VideoCodec GetCurrentCodecV()
        {
            return (CodecUtils.VideoCodec)form.encVidCodecsBox.SelectedIndex;
        }

        public static CodecUtils.AudioCodec GetCurrentCodecA()
        {
            return (CodecUtils.AudioCodec)form.encAudCodecBox.SelectedIndex;
        }

        public static CodecUtils.SubtitleCodec GetCurrentCodecS()
        {
            return (CodecUtils.SubtitleCodec)form.encSubCodecBox.SelectedIndex;
        }

        #endregion

        public static Dictionary<string, string> GetVideoArgsFromUi(bool vbr)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (vbr)
                dict.Add("bitrate", GetVideoBitrate().ToString());
            else
                dict.Add("q", form.encVidQualityBox.Value.ToString());
                
            dict.Add("preset", form.encVidPresetBox.Text.ToLower());
            dict.Add("pixFmt", form.encVidColorsBox.Text.ToLower());
            dict.Add("qMode", form.encQualModeBox.SelectedIndex.ToString());
            //dict.Add("custom", form.customArgsOutBox.Text.Trim());

            return dict;
        }

        private static int GetVideoBitrate()
        {
            if((QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.TargetKbps)
            {
                string br = form.encVidQualityBox.Text.ToLower().Trim();
                if (br.EndsWith("k")) return br.GetInt() * 1024;
                if (br.EndsWith("m")) return br.GetInt() * 1024 * 1024;

                return br.GetInt();
            }

            if ((QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.TargetMbytes)
            {
                int audioBps = (int)form.encAudQualUpDown.Value * 1024;
                double durationSecs = TrackList.current.DurationMs / (double)1000;
                float targetMbytes = form.encVidQualityBox.Text.GetFloat();
                long targetBits = (long)Math.Round(targetMbytes * 8 * 1024 * 1024); 
                int targetVidBitrate = (int)Math.Floor(targetBits / durationSecs) - audioBps; // Round down since undershooting is better than overshooting here
                string brTotal = (((float)targetVidBitrate + audioBps) / 1024).ToString("0.0");
                string brVid = ((float)targetVidBitrate / 1024).ToString("0");
                string brAud = form.encAudQualUpDown.Value.ToString();
                Logger.Log($"Target Filesize Mode: Using bitrate of {brTotal} kbps ({brVid}k Video, {brAud}k Audio) over {durationSecs.ToString("0.0")} seconds to hit {targetMbytes} megabytes.");
                return targetVidBitrate;
            }

            return 0;
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

        public static async Task<string> GetVideoFilterArgs(IEncoder vCodec, CodecArgs codecArgs = null)
        {
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (TrackList.current.VideoStreams.Count < 1 || vCodec.DoesNotEncode)
                return "";

            VideoStream vs = TrackList.current.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if (Program.mainForm.encSubBurnBox.SelectedIndex > 0) // Check Filter: Subtitle Burn-In
            {
                string subFilePath = FormatUtils.GetFilterPath(TrackList.current.TruePath);
                filters.Add($"subtitles={subFilePath}:si={Program.mainForm.encSubBurnBox.Text.GetInt() - 1}");
            }

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = Program.mainForm.encScaleBoxW.Text.Trim().ToLower();
            string scaleH = Program.mainForm.encScaleBoxH.Text.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(MiscUtils.GetScaleFilter(scaleW, scaleH));

            if (Program.mainForm.encCropModeBox.SelectedIndex > 0) // Check Filter: Crop/Cropdetect
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(TrackList.current.TruePath, false));

            filters = filters.Where(x => x.Trim().Length > 2).ToList(); // Strip empty filters

            if (filters.Count > 0)
                return $"-vf {string.Join(",", filters)}";
            else
                return "";
        }

        public static string GetOutPath (IEncoder c)
        {
            string uiPath = Program.mainForm.ffmpegOutputBox.Text.Trim();

            if (c.IsSequence)
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
