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
            form.encAudConfModeBox.SelectedIndex = 0;
        }

        public static void InitFile(string path)
        {
            try
            {
                Program.mainForm.ffmpegOutputBox.Text = path;

                if (!RunTask.runningBatch) // Don't load new values into UI in batch mode since we apply the same for all files
                {
                    Program.mainForm.encScaleBoxW.Text = Program.mainForm.encScaleBoxH.Text = "";
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
                dict.Add("bitrate", GetVideoKbps().ToString());
            else
                dict.Add("q", form.encVidQualityBox.Value.ToString());
                
            dict.Add("preset", form.encVidPresetBox.Text.ToLower());
            dict.Add("pixFmt", form.encVidColorsBox.Text.ToLower());
            dict.Add("qMode", form.encQualModeBox.SelectedIndex.ToString());
            //dict.Add("custom", form.customArgsOutBox.Text.Trim());

            return dict;
        }

        private static int GetVideoKbps()
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
                bool aud = !CodecUtils.GetCodec(GetCurrentCodecA()).DoesNotEncode;
                int audioTracks = form.streamListBox.CheckedItems.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Audio).Count();
                int audioBps = aud ? ((int)form.encAudQualUpDown.Value * 1024) * audioTracks : 0;
                double durationSecs = TrackList.current.File.DurationMs / (double)1000;
                float targetMbytes = form.encVidQualityBox.Text.GetFloat();
                long targetBits = (long)Math.Round(targetMbytes * 8 * 1024 * 1024); 
                int targetVidBitrate = (int)Math.Floor(targetBits / durationSecs) - audioBps; // Round down since undershooting is better than overshooting here
                string brTotal = (((float)targetVidBitrate + audioBps) / 1024).ToString("0.0");
                string brVid = ((float)targetVidBitrate / 1024).ToString("0");
                string brAud = ((float)audioBps / 1024).ToString("0");
                Logger.Log($"Target Filesize Mode: Using bitrate of {brTotal} kbps ({brVid}k Video, {brAud}k Audio) over {durationSecs.ToString("0.0")} seconds to hit {targetMbytes} megabytes.");
                return ((float)targetVidBitrate / 1024).RoundToInt();
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

        public static void InitBurnCombox()
        {
            ComboBox burnBox = Program.mainForm.encSubBurnBox;

            burnBox.Items.Clear();
            burnBox.Items.Add("Disabled");

            for (int i = 0; i < TrackList.current.File.SubtitleStreams.Count; i++)
            {
                string lang = TrackList.current.File.SubtitleStreams[i].Language.Trim();
                burnBox.Items.Add($"Subtitle Track {i + 1}{(lang.Length > 1 ? $" ({lang})" : "")}");
            }

            burnBox.SelectedIndex = 0;
        }

        #endregion

        #region Metadata Tab 

        //public static List<UniqueMetadataEntry> metaEntries = new List<UniqueMetadataEntry>();

        public static void LoadMetadataGrid()
        {
            if (TrackList.current == null)
                return;

            DataGridView grid = Program.mainForm.metaGrid;
            MediaFile c = TrackList.current.File;

            if (grid.Columns.Count != 3)
            {
                grid.Columns.Clear();
                grid.Columns.Add("0", "Field");
                grid.Columns.Add("1", "Title");
                grid.Columns.Add("2", "Lang");
            }

            //GetMetadata();

            grid.Rows.Clear();

            if (TrackList.current != null)
                grid.Rows.Add($"Output File", TrackList.current.Title, TrackList.current.Language);

            var streamEntries = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().ToArray();

            for (int i = 0; i < streamEntries.Count(); i++)
            {
                MediaStreamListEntry entry = streamEntries[i];
                string title = string.IsNullOrWhiteSpace(entry.TitleEdited) ? entry.Title : entry.TitleEdited;
                string lang = string.IsNullOrWhiteSpace(entry.LanguageEdited) ? entry.Language : entry.LanguageEdited;
                int newIdx = grid.Rows.Add($"{entry.ToString().Split(')')[0].Remove("[").Remove("]").Replace(" - ", " ")})", title, lang);
                grid.Rows[newIdx].Visible = Program.mainForm.streamListBox.GetItemChecked(i);
            }

            //if (FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).Any())
            //    grid.Rows.Add("-1", $"File", metaEntries[0].Title, metaEntries[0].Language);

            //var checkStreamEntries = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(x => Program.mainForm.streamListBox.CheckedItems.Contains(x));
            //List<MediaStreamListEntry> vStreams = checkStreamEntries.Where(e => e.Stream.Type == Stream.StreamType.Video).ToList();
            //List<MediaStreamListEntry> aStreams = checkStreamEntries.Where(e => e.Stream.Type == Stream.StreamType.Audio).ToList();
            //List<MediaStreamListEntry> sStreams = checkStreamEntries.Where(e => e.Stream.Type == Stream.StreamType.Subtitle).ToList();
            //
            //for (int i = 0; i < vStreams.Count; i++)
            //{
            //    UniqueMetadataEntry e = new UniqueMetadataEntry(TrackList.current.SourcePath, Stream.StreamType.Video, vStreams[i].Stream.Index);
            //    List<UniqueMetadataEntry> match = metaEntries.Where(x => x.GetPseudoHash() == e.GetPseudoHash()).ToList();
            //    if (match.Count < 1)  continue;
            //    string title = match.Count > 0 ? match[0].Title : vStreams[i].Stream.Title;
            //    string lang = match.Count > 0 ? match[0].Language : vStreams[i].Stream.Language;
            //    if (!FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).Any()) continue;
            //    int fileIdx = FileList.currentFiles.IndexOf(FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).First());
            //    grid.Rows.Add(fileIdx, e.StreamIndex, $"Video Track {i + 1}", title, lang);
            //}
            //
            //for (int i = 0; i < aStreams.Count; i++)
            //{
            //    UniqueMetadataEntry e = new UniqueMetadataEntry(TrackList.current.SourcePath, Stream.StreamType.Audio, aStreams[i].Stream.Index);
            //    List<UniqueMetadataEntry> match = metaEntries.Where(x => x.GetPseudoHash() == e.GetPseudoHash()).ToList();
            //    if (match.Count < 1) continue;
            //    string title = match.Count > 0 ? match[0].Title : sStreams[i].Stream.Title;
            //    string lang = match.Count > 0 ? match[0].Language : aStreams[i].Stream.Language;
            //    if (!FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).Any()) continue;
            //    int fileIdx = FileList.currentFiles.IndexOf(FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).First());
            //    grid.Rows.Add(fileIdx, e.StreamIndex, $"Audio Track {i + 1}", title, lang);
            //}
            //
            //for (int i = 0; i < sStreams.Count; i++)
            //{
            //    UniqueMetadataEntry e = new UniqueMetadataEntry(TrackList.current.SourcePath, Stream.StreamType.Subtitle, sStreams[i].Stream.Index);
            //    List<UniqueMetadataEntry> match = metaEntries.Where(x => x.GetPseudoHash() == e.GetPseudoHash()).ToList();
            //    if (match.Count < 1) continue;
            //    string title = match.Count > 0 ? match[0].Title : sStreams[i].Stream.Title;
            //    string lang = match.Count > 0 ? match[0].Language : sStreams[i].Stream.Language;
            //    if (!FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).Any()) continue;
            //    int fileIdx = FileList.currentFiles.IndexOf(FileList.currentFiles.Where(x => x.SourcePath == e.SourceFilePath).First());
            //    grid.Rows.Add(fileIdx, e.StreamIndex, $"Subtitle Track {i + 1}", title, lang);
            //}

            grid.RowHeadersVisible = false;
            grid.Columns[0].ReadOnly = true;
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
            grid.Columns[0].Visible = true;
            grid.Columns[1].Visible = true;
            grid.Columns[0].FillWeight = 34;
            grid.Columns[1].FillWeight = 60;
            grid.Columns[2].FillWeight = 6;
        }

        // private static void GetMetadata ()
        // {
        //     var checkedStreams = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(x => Program.mainForm.streamListBox.CheckedItems.Contains(x));
        //     List<Stream> allStreams = checkedStreams.Select(s => s.Stream).ToList();
        //     List<VideoStream> vStreams = checkedStreams.Where(e => e.Stream.Type == Stream.StreamType.Video).Select(s => (VideoStream)s.Stream).ToList();
        //     List<AudioStream> aStreams = checkedStreams.Where(e => e.Stream.Type == Stream.StreamType.Audio).Select(s => (AudioStream)s.Stream).ToList();
        //     List<SubtitleStream> sStreams = checkedStreams.Where(e => e.Stream.Type == Stream.StreamType.Subtitle).Select(s => (SubtitleStream)s.Stream).ToList();
        // 
        //     metaEntries.Add(new UniqueMetadataEntry(TrackList.current.File.SourcePath, Stream.StreamType.Data, -1, TrackList.current.Title, TrackList.current.Language));
        // 
        //     for (int i = 0; i < checkedStreams.Count(); i++)
        //     {
        //         MediaStreamListEntry e = checkedStreams.ElementAt(i);
        //         metaEntries.Add(new UniqueMetadataEntry(e.MediaFile.SourcePath, e.Stream.Type, i, e.Stream.Title, e.Stream.Language));
        //     }
        // }

        public static void SaveMetadata ()
        {
            DataGridView grid = Program.mainForm.metaGrid;

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                DataGridViewRow row = grid.Rows[i];
                int idx = i - 1;

                string title = row.Cells[1].Value?.ToString().Trim();
                string lang = row.Cells[2].Value?.ToString().Trim();

                if (idx < 0)
                {
                    TrackList.current.TitleEdited = title;
                    TrackList.current.LanguageEdited = lang;
                    //Logger.Log($"Saved file metadata: {title} | {lang}");
                }
                else
                {
                    MediaStreamListEntry entry = (MediaStreamListEntry)Program.mainForm.streamListBox.Items[idx];

                    if (title.Trim() != entry.Title.Trim())
                        entry.TitleEdited = title;

                    if (lang.Trim() != entry.Language.Trim())
                        entry.LanguageEdited = lang;

                    //Logger.Log($"Saved file metadata: {entry.TitleEdited} | {entry.LanguageEdited}");
                }
            }
        }

        public static string GetMetadataArgs()
        {
            bool attachments = TrackList.current.File.AttachmentStreams.Count > 0;
            string stripStr = attachments ? ":s:t 0:s:t" : " -1"; // If there are attachments, only copy the attachment metadata, otherwise none
            int cfg = Config.GetInt(Config.Key.metaMode);
            DataGridView grid = Program.mainForm.metaGrid;
            int defaultAudio = Program.mainForm.trackListDefaultAudioBox.SelectedIndex;
            int defaultSubs = Program.mainForm.trackListDefaultSubsBox.SelectedIndex - 1;
            List<string> argsDispo = new List<string>();

            int relIdxAud = 0;
            int relIdxSub = 0;

            for (int i = 0; i < grid.Rows.Count; i++) // Disposition args
            {
                DataGridViewRow row = grid.Rows[i];
                string trackTitle = row.Cells[0].Value?.ToString();

                if (i > 0 && Program.mainForm.streamListBox.GetItemChecked(i - 1))
                {
                    if (trackTitle.ToLower().Contains("audio"))
                    {
                        argsDispo.Add($"-disposition:a:{relIdxAud} {(defaultAudio == relIdxAud ? "default" : "0")}");
                        relIdxAud++;
                    }

                    if (trackTitle.ToLower().Contains("subtitle"))
                    {
                        argsDispo.Add($"-disposition:s:{relIdxSub} {(defaultSubs == relIdxSub ? "default" : "0")}");
                        relIdxSub++;
                    }
                }
            }

            if (cfg == 2) // 2 = Strip All
                return $"-map_metadata{stripStr} {string.Join(" ", argsDispo)}"; 

            bool map = cfg == 0 || cfg == 1;  // 0 = Copy + Apply Editor Tags - 1 = Strip Others + Apply Editor Tags
            List<string> argsMeta = new List<string>();

            if (map)
            {
                if (!string.IsNullOrWhiteSpace(TrackList.current.LanguageEdited))
                    argsMeta.Add($"-metadata title=\"{TrackList.current.LanguageEdited}\"");

                var streamEntries = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().ToArray();

                for (int i = 0; i < streamEntries.Count(); i++)
                {
                    MediaStreamListEntry entry = streamEntries[i];

                    if (!string.IsNullOrWhiteSpace(entry.TitleEdited))
                        argsMeta.Add($"-metadata:s:{i} title=\"{entry.TitleEdited}\"");
                    else if (cfg == 1 && entry.TitleEdited.Trim() != entry.Title)
                        argsMeta.Add($"-metadata:s:{i} language=\"{entry.Title}\"");

                    if (!string.IsNullOrWhiteSpace(entry.LanguageEdited))
                        argsMeta.Add($"-metadata:s:{i} title=\"{entry.LanguageEdited}\"");
                    else if(cfg == 1 && entry.LanguageEdited.Trim() != entry.Language)
                        argsMeta.Add($"-metadata:s:{i} language=\"{entry.Language}\"");
                }
            }

            if (cfg == 0) // 0 = Map all and add titles/langs
                return $"-map_metadata 0 {string.Join(" ", argsMeta)} {string.Join(" ", argsDispo)}";
            else if (cfg == 0) // 1 = Strip but add titles/langs
                return $"-map_metadata{stripStr} {string.Join(" ", argsMeta)} {string.Join(" ", argsDispo)}";

            Logger.Log($"Metadata mode not 0, 1, or 2!! cfg = {cfg}", true);
            return $"-map_metadata{stripStr} {string.Join(" ", argsMeta)} {string.Join(" ", argsDispo)}";
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
            MediaFile currFile = TrackList.current.File;
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (currFile.VideoStreams.Count < 1 || vCodec.DoesNotEncode)
                return "";

            VideoStream vs = currFile.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if (Program.mainForm.encSubBurnBox.SelectedIndex > 0) // Check Filter: Subtitle Burn-In
            {
                string subFilePath = FormatUtils.GetFilterPath(currFile.TruePath);
                filters.Add($"subtitles={subFilePath}:si={Program.mainForm.encSubBurnBox.Text.GetInt() - 1}");
            }

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = Program.mainForm.encScaleBoxW.Text.Trim().ToLower();
            string scaleH = Program.mainForm.encScaleBoxH.Text.Trim().ToLower();

            if (Program.mainForm.encCropModeBox.SelectedIndex > 0) // Check Filter: Crop/Cropdetect
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(currFile.TruePath, false));

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(MiscUtils.GetScaleFilter(scaleW, scaleH));

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
