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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    partial class QuickConvertUi : QuickConvert
    {
        private static MainForm Form;
        public static CropConfig CurrentCrop;
        public static TrimForm.TrimSettings CurrentTrim;

        public static void Init()
        {
            Form = Program.mainForm;

            Form.encVidCodecsBox.Items.AddRange(Enum.GetValues(typeof(CodecUtils.VideoCodec)).Cast<CodecUtils.VideoCodec>().Select(c => CodecUtils.GetCodec(c).FriendlyName).ToArray()); // Load video codecs

            ConfigParser.LoadComboxIndex(Form.encVidCodecsBox);

            foreach (QuickConvert.QualityMode qm in Enum.GetValues(typeof(QuickConvert.QualityMode)))  // Load quality modes
                Form.encQualModeBox.Items.Add(qm.ToString().Replace("Crf", "CRF").Replace("TargetKbps", "Target Bitrate (Kbps)").Replace("TargetMbytes", "Target Filesize (MB)"));

            Form.encQualModeBox.SelectedIndex = 0;

            Form.encAudCodecBox.Items.AddRange(Enum.GetValues(typeof(CodecUtils.AudioCodec)).Cast<CodecUtils.AudioCodec>().Select(c => CodecUtils.GetCodec(c).FriendlyName).ToArray()); // Load audio codecs

            ConfigParser.LoadComboxIndex(Form.encAudCodecBox);

            foreach (CodecUtils.SubtitleCodec c in Enum.GetValues(typeof(CodecUtils.SubtitleCodec)))  // Load audio codecs
                Form.encSubCodecBox.Items.Add(CodecUtils.GetCodec(c).FriendlyName);

            ConfigParser.LoadComboxIndex(Form.encSubCodecBox);

            foreach (string c in Enum.GetNames(typeof(Containers.Container)))   // Load containers
                Form.ffmpegContainerBox.Items.Add(c.ToUpper());

            ConfigParser.LoadComboxIndex(Form.ffmpegContainerBox);
            Form.encAudConfModeBox.SelectedIndex = 0;
        }

        public static void InitFile(string path = "")
        {
            try
            {
                if (path.IsNotEmpty())
                {
                    Program.mainForm.FfmpegOutputBox.Text = Path.ChangeExtension(path, null);
                    ValidateContainer();
                }

                RefreshFileListRelatedOptions();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialize media file: {e.Message}\n{e.StackTrace}");
            }
        }

        public static void RefreshFileListRelatedOptions()
        {
            //Logger.Log($"RefreshFileListRelatedOptions [removeme]");
            RefreshSubtitleBurnInBox();
            RefreshMetadataAndChapterOptions();
            LoadMetadataGrid();
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
            Program.mainForm.encVidColorsBox.Enabled = !enc.DoesNotEncode && enc.ColorFormats != null && enc.ColorFormats.Count > 0;
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
            if (index < 0)
                return;

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
            if (Form.encQualModeBox.SelectedIndex == 0)
            {
                if (enc.QMax > 0)
                    Form.encVidQualityBox.Maximum = enc.QMax;
                else
                    Form.encVidQualityBox.Maximum = 100;

                Form.encVidQualityBox.Minimum = enc.QMin;

                if (enc.QDefault >= 0)
                    Form.encVidQualityBox.Value = enc.QDefault;
                else
                    Form.encVidQualityBox.Text = "";
            }

            Form.encVidQualityBox.Text = Form.encVidQualityBox.Value.ToString();
        }

        static void LoadPresets(IEncoder enc)
        {
            Form.encVidPresetBox.Items.Clear();

            if (enc.Presets != null)
                foreach (string p in enc.Presets)
                    Form.encVidPresetBox.Items.Add(p.ToTitleCase()); // Add every preset to the dropdown

            if (Form.encVidPresetBox.Items.Count > 0)
                Form.encVidPresetBox.SelectedIndex = enc.PresetDefault; // Select default preset
        }

        static void LoadColorFormats(IEncoder enc)
        {
            Form.encVidColorsBox.Items.Clear();

            if (enc.ColorFormats != null)
                foreach (PixelFormats p in enc.ColorFormats)
                    Form.encVidColorsBox.Items.Add(PixFmtUtils.GetFormat(p).FriendlyName); // Add every pix_fmt to the dropdown

            if (Form.encVidColorsBox.Items.Count > 0)
                Form.encVidColorsBox.SelectedIndex = enc.ColorFormatDefault; // Select default pix_fmt
        }

        #endregion

        #region Load Audio Options

        static void LoadAudBitrate(IEncoder enc)
        {
            int channels = Form.encAudChannelsBox.Text.Split(' ')[0].GetInt();

            if (enc.QDefault >= 0)
            {
                Form.encAudQualUpDown.Value = enc.QDefault;
                Form.encAudQualUpDown.Text = Form.encAudQualUpDown.Value.ToString();
            }
            else
            {
                Form.encAudQualUpDown.Value = 0;
                Form.encAudQualUpDown.Text = "";
            }
        }

        #endregion

        public static void ValidateContainer()
        {
            if (Form.ffmpegContainerBox.SelectedIndex < 0)
                return;

            // CodecUtils.VideoCodec vCodec = (CodecUtils.VideoCodec)form.encVidCodecsBox.SelectedIndex;
            // CodecUtils.AudioCodec aCodec = (CodecUtils.AudioCodec)form.encAudCodecBox.SelectedIndex;
            // CodecUtils.SubtitleCodec sCodec = (CodecUtils.SubtitleCodec)form.encSubCodecBox.SelectedIndex;

            ValidatePath();
        }

        public static void ValidatePath()
        {
            if (TrackList.current == null)
                return;

            if (File.Exists(UiData.GetOutPath()))
                Program.mainForm.FfmpegOutputBox.Text = Path.ChangeExtension(IoUtils.GetAvailableFilename(UiData.GetOutPath()), null);
        }

        #region Get Current Codec

        public static CodecUtils.VideoCodec GetCurrentCodecV()
        {
            return (CodecUtils.VideoCodec)Form.encVidCodecsBox.SelectedIndex;
        }

        public static CodecUtils.AudioCodec GetCurrentCodecA()
        {
            return (CodecUtils.AudioCodec)Form.encAudCodecBox.SelectedIndex;
        }

        public static CodecUtils.SubtitleCodec GetCurrentCodecS()
        {
            return (CodecUtils.SubtitleCodec)Form.encSubCodecBox.SelectedIndex;
        }

        #endregion

        public static Dictionary<string, string> GetVideoArgsFromUi(bool vbr)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (vbr)
                dict.Add("bitrate", GetVideoKbps().ToString());
            else
                dict.Add("q", Form.encVidQualityBox.Value.ToString());

            dict.Add("preset", Form.encVidPresetBox.Text.ToLower());

            IEncoder enc = CodecUtils.GetCodec((CodecUtils.VideoCodec)Program.mainForm.encVidCodecsBox.SelectedIndex);

            if (enc.ColorFormats != null)
                dict.Add("pixFmt", PixFmtUtils.GetFormat(enc.ColorFormats[Program.mainForm.encVidColorsBox.SelectedIndex]).Name);

            dict.Add("qMode", Form.encQualModeBox.SelectedIndex.ToString());
            //dict.Add("custom", form.customArgsOutBox.Text.Trim());

            return dict;
        }

        private static int GetVideoKbps()
        {
            if ((QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.TargetKbps)
            {
                string br = Form.encVidQualityBox.Text.ToLower().Trim();
                if (br.EndsWith("k")) return br.GetInt() * 1024;
                if (br.EndsWith("m")) return br.GetInt() * 1024 * 1024;

                return br.GetInt();
            }

            if ((QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.TargetMbytes)
            {
                return BitrateCalculation.GetTargetSizeKbps(CodecUtils.GetCodec(GetCurrentCodecA()));
            }

            return 0;
        }

        public static Dictionary<string, string> GetAudioArgsFromUi()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("bitrate", Form.encAudQualUpDown.Text.ToLower());
            dict.Add("ac", Form.encAudChannelsBox.Text.Split(' ')[0].Trim());
            return dict;
        }

        #region Load Media Info Into UI Where Needed

        public static void RefreshSubtitleBurnInBox()
        {
            if (RunTask.runningBatch)
                return;

            ComboBox burnBox = Program.mainForm.encSubBurnBox;

            burnBox.Items.Clear();
            burnBox.Items.Add("Disabled");

            if (TrackList.current != null && TrackList.current.File != null)
            {
                for (int i = 0; i < TrackList.current.File.SubtitleStreams.Count; i++)
                {
                    bool zeroIdx = Config.GetBool(Config.Key.UseZeroIndexedStreams);
                    var stream = TrackList.current.File.SubtitleStreams[i];

                    List<string> items = new List<string>();
                    items.Add($"#{(zeroIdx ? i : i + 1).ToString().PadLeft(2, '0')}");
                    items.Add(stream.Language.ToUpper().Trunc(6));
                    items.Add(stream.Title.Trunc(25));
                    burnBox.Items.Add($"{string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x)))} ({Aliases.GetNicerCodecName(stream.Codec).Trunc(12)})");
                }
            }

            burnBox.SelectedIndex = 0;
        }

        #endregion

        #region Metadata Tab 

        public static void RefreshMetadataAndChapterOptions()
        {
            if (RunTask.runningBatch)
                return;

            var metaBox = Program.mainForm.EncMetaCopySource;
            var chapBox = Program.mainForm.EncMetaChapterSource;

            metaBox.Items.Clear();
            chapBox.Items.Clear();
            metaBox.Items.Add("None");
            chapBox.Items.Add("None");

            List<string> filePaths = Program.mainForm.fileListBox.Items.Cast<ListViewItem>().Select(x => ((FileListEntry)x.Tag).File.SourcePath).ToList();

            if (RunTask.currentFileListMode == RunTask.FileListMode.Mux)
            {
                for (int i = 0; i < filePaths.Count(); i++)
                {
                    string ext = Path.GetExtension(filePaths[i]);
                    string text = $"#{i + 1} ({Path.GetFileNameWithoutExtension(filePaths[i]).Trunc(30 - ext.Length) + ext})";
                    metaBox.Items.Add(text);
                    chapBox.Items.Add(text);
                }
            }
            else
            {
                string text = "#1 - Current Input File";
                metaBox.Items.Add(text);
                chapBox.Items.Add(text);
            }

            metaBox.SelectedIndex = metaBox.Items.Count > 1 ? 1 : 0; // Use 1st file if there is one, otherwise select "None"
            chapBox.SelectedIndex = chapBox.Items.Count > 1 ? 1 : 0; // "
        }

        public static void LoadMetadataGrid()
        {
            FileListEntry curr = TrackList.current;

            if (RunTask.runningBatch || curr == null)
                return;

            Logger.Log($"Reloading metadata grid.", true);
            DataGridView grid = Program.mainForm.EncMetadataGrid;
            MediaFile c = curr.File;

            if (grid.Columns.Count != 3)
            {
                grid.Columns.Clear();
                grid.Columns.Add("0", "Field");
                grid.Columns.Add("1", "Title");
                grid.Columns.Add("2", "Lang");
            }

            grid.Rows.Clear();

            grid.Rows.Add($"Output File", (curr.TitleEdited != null) ? curr.TitleEdited : curr.Title, (curr.LanguageEdited != null) ? curr.LanguageEdited : curr.Language);

            var streamEntries = Program.mainForm.streamList.Items.Cast<ListViewItem>().Select(x => (StreamListEntry)x.Tag).ToArray();

            for (int i = 0; i < streamEntries.Count(); i++)
            {
                StreamListEntry entry = streamEntries[i];
                string title = string.IsNullOrWhiteSpace(entry.TitleEdited) ? entry.Title : entry.TitleEdited;
                string lang = string.IsNullOrWhiteSpace(entry.LanguageEdited) ? entry.Language : entry.LanguageEdited;
                int newIdx = grid.Rows.Add(entry.GetString(false, false), title, lang);
                grid.Rows[newIdx].Visible = Program.mainForm.streamList.Items[i].Checked;
            }

            grid.RowHeadersVisible = false;
            grid.Columns[0].ReadOnly = true;
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
            grid.Columns[0].Visible = true;
            grid.Columns[1].Visible = true;
            grid.Columns[0].FillWeight = 25;
            grid.Columns[1].FillWeight = 67;
            grid.Columns[2].FillWeight = 8;
        }

        public static void SaveMetadata()
        {
            if (TrackList.current == null)
                return;

            DataGridView grid = Program.mainForm.EncMetadataGrid;
            Logger.Log($"Saving metadata.", true);

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                DataGridViewRow row = grid.Rows[i];
                int idx = i - 1;

                string title = row.Cells[1].Value?.ToString().Trim();
                string lang = row.Cells[2].Value?.ToString().Trim();

                if (idx < 0)
                {
                    TrackList.current.TitleEdited = (title == null) ? "" : title;
                    TrackList.current.LanguageEdited = (lang == null) ? "" : lang;
                }
                else
                {
                    StreamListEntry entry = (StreamListEntry)Program.mainForm.streamList.Items[idx].Tag;
                    entry.TitleEdited = title;
                    entry.LanguageEdited = lang;
                }
            }
        }

        public static string GetMetadataArgs()
        {
            SaveMetadata();
            MainForm form = Program.mainForm;
            List<StreamListEntry> checkedEntries = form.streamList.CheckedItems.Cast<ListViewItem>().Select(x => ((StreamListEntry)x.Tag)).ToList();

            int metaFileIndex = (Program.mainForm.EncMetaCopySource.SelectedIndex - 1).Clamp(-1, int.MaxValue);
            int chapFileIndex = (Program.mainForm.EncMetaChapterSource.SelectedIndex - 1).Clamp(-1, int.MaxValue);

            DataGridView grid = form.EncMetadataGrid;

            #region Attachment Metadata (Only needed when doing -map_metadata -1)

            string argsAttachmentData = "";

            if (metaFileIndex == -1 && checkedEntries.Where(x => x.Stream.Type == Stream.StreamType.Attachment).Any()) // When stripping all other metadata, we must still add attachment data as muxing attachments without filename doesn't seem to be possible
            {
                for (int i = 0; i < form.fileListBox.Items.Count; i++)
                {
                    MediaFile file = ((FileListEntry)form.fileListBox.Items[i].Tag).File;

                    if (checkedEntries.Where(x => x.MediaFile.SourcePath == file.SourcePath).Where(x => x.Stream.Type == Stream.StreamType.Attachment).Any())
                        argsAttachmentData += $"-map_metadata:s:t {i}:s:t";
                }
            }

            #endregion

            #region Dispositions (Set default track)

            List<string> argListDispositions = new List<string>();

            int defaultAudio = form.trackListDefaultAudioBox.SelectedIndex;
            int defaultSubs = form.trackListDefaultSubsBox.SelectedIndex - 1;

            int relIdxAud = 0;
            int relIdxSub = 0;

            for (int i = 0; i < grid.Rows.Count; i++) // Disposition args
            {
                DataGridViewRow row = grid.Rows[i];
                string trackTitle = row.Cells[0].Value?.ToString();

                if (i > 0 && form.streamList.Items[i - 1].Checked)
                {
                    if (trackTitle.ToLower().Contains("audio"))
                    {
                        argListDispositions.Add($"-disposition:a:{relIdxAud} {(defaultAudio == relIdxAud ? "default" : "0")}");
                        relIdxAud++;
                    }

                    if (trackTitle.ToLower().Contains("subtitle"))
                    {
                        argListDispositions.Add($"-disposition:s:{relIdxSub} {(defaultSubs == relIdxSub ? "default" : "0")}");
                        relIdxSub++;
                    }
                }
            }

            string argsDispo = string.Join(" ", argListDispositions);

            #endregion

            #region Track Titles/Languages from Grid

            string argsMetaGrid = "";

            if (Program.mainForm.EncMetaApplyGrid.Checked)
            {
                List<string> argListMetaGrid = new List<string>();

                argListMetaGrid.Add($"-metadata title=\"{TrackList.current.TitleEdited}\"");
                argListMetaGrid.Add($"-metadata title=\"{TrackList.current.LanguageEdited}\"");

                var streamEntries = form.streamList.CheckedItems.Cast<ListViewItem>().Select(x => (StreamListEntry)x.Tag).ToArray();

                for (int i = 0; i < streamEntries.Count(); i++)
                {
                    StreamListEntry entry = streamEntries[i];

                    argListMetaGrid.Add($"-metadata:s:{i} title=\"{entry.TitleEdited}\"");
                    argListMetaGrid.Add($"-metadata:s:{i} language=\"{entry.LanguageEdited}\"");
                }

                argsMetaGrid = string.Join(" ", argListMetaGrid);
            }

            #endregion

            return $"-map_metadata {metaFileIndex} " +
                $"{argsAttachmentData} " +
                $"-map_chapters {chapFileIndex} " +
                $"{argsMetaGrid} " +
                $"{argsDispo}";
        }

        #endregion

        public static void InitAdvFilterGrid()
        {
            DataGridView grid = Program.mainForm.EncAdvancedFiltersGrid;
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("0", "Filter");
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
        }

        #region Get Args

        public static string GetMuxingArgs()
        {
            Containers.Container c = (Containers.Container)Program.mainForm.ffmpegContainerBox.SelectedIndex;
            return Containers.GetMuxingArgs(c);
        }

        public static string GetMiscInputArgs()
        {
            List<string> args = new List<string>();

            if (CurrentTrim != null && !CurrentTrim.IsUnset && CurrentTrim.TrimMode == TrimForm.TrimSettings.Mode.TimeKeyframe)
                args.Add(CurrentTrim.StartArg);

            return string.Join(" ", args);
        }

        public static string GetMiscOutputArgs()
        {
            List<string> args = new List<string>();

            if (CurrentTrim != null && !CurrentTrim.IsUnset)
            {
                if (CurrentTrim.TrimMode == TrimForm.TrimSettings.Mode.TimeExact)
                    args.Add(CurrentTrim.StartArg);

                args.Add(CurrentTrim.DurationArg);
            }

            return string.Join(" ", args);
        }

        public static async Task<string> GetVideoFilterArgs(IEncoder vCodec, CodecArgs codecArgs = null, bool quiet = false)
        {
            MediaFile currFile = TrackList.current.File;
            List<string> filters = new List<string>();

            if (codecArgs != null && codecArgs.ForcedFilters != null)
                filters.AddRange(codecArgs.ForcedFilters);

            if (currFile.VideoStreams.Count < 1 || (vCodec != null && vCodec.DoesNotEncode))
                return "";

            VideoStream vs = currFile.VideoStreams.First();
            Fraction fps = GetUiFps();

            if (CurrentTrim != null && !CurrentTrim.IsUnset && CurrentTrim.TrimMode == TrimForm.TrimSettings.Mode.FrameNumbers) // Check Filter: Frame Number Trim
                filters.Add(CurrentTrim.StartArg);

            if (fps.GetFloat() > 0.01f && vs.Rate.GetFloat() != fps.GetFloat()) // Check Filter: Framerate Resampling
                filters.Add($"fps=fps={fps}");

            if (Program.mainForm.encSubBurnBox.SelectedIndex > 0) // Check Filter: Subtitle Burn-In
            {
                int subIndex = Program.mainForm.encSubBurnBox.SelectedIndex - 1;
                bool bitmapSubs = TrackList.current.File.SubtitleStreams[subIndex].Bitmap;

                if (bitmapSubs)
                {
                    filters.Add($"[0:s:{subIndex}]overlay=shortest=1");
                }
                else
                {
                    string subFilePath = FormatUtils.GetFilterPath(currFile.ImportPath);
                    filters.Add($"subtitles={subFilePath}:si={subIndex}");
                }
            }

            if ((vs.Resolution.Width % 2 != 0) || (vs.Resolution.Height % 2 != 0)) // Check Filter: Pad for mod2
                filters.Add(FfmpegUtils.GetPadFilter(2));

            string scaleW = Program.mainForm.encScaleBoxW.Text.Trim().ToLower();
            string scaleH = Program.mainForm.encScaleBoxH.Text.Trim().ToLower();

            if (Program.mainForm.encCropModeBox.Text.ToLower().Contains("manual") && CurrentCrop != null) // Check Filter: Manual Crop
                filters.Add($"crop={CurrentCrop.GetFilterArgs(vs.Resolution)}");

            if (Program.mainForm.encCropModeBox.Text.ToLower().Contains("auto")) // Check Filter: Autocrop
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(currFile.ImportPath, quiet));

            if (!string.IsNullOrWhiteSpace(scaleW) || !string.IsNullOrWhiteSpace(scaleH)) // Check Filter: Scale
                filters.Add(MiscUtils.GetScaleFilter(scaleW, scaleH));

            filters.AddRange(GetCustomFilters());

            filters = filters.Where(x => x.Trim().Length > 2).ToList(); // Strip empty filters

            string firstVideoMap = (await TrackList.GetMapArgs(vCodec, true, false, false)).Split("-map ")[1];
            string filterChain = "";

            for (int i = 0; i < filters.Count; i++)
            {
                bool first = i == 0;
                bool last = i == filters.Count - 1;

                filterChain += $"[{(first ? firstVideoMap : "vf")}]{filters[i]}";
                filterChain += $"[vf]{(last ? "" : ";")}";
            }

            if (filters.Count > 0)
                return $"-filter_complex {filterChain}";
            else
                return "";
        }

        private static List<string> GetCustomFilters()
        {
            DataGridView grid = Program.mainForm.EncAdvancedFiltersGrid;
            return grid.Rows.Cast<DataGridViewRow>().ToList().Select(x => (string)x.Cells[0].Value).Where(x => x != null).ToList();
        }

        #endregion

        public static Fraction GetUiFps()
        {
            TextBox fpsBox = Program.mainForm.encVidFpsBox;
            return MiscUtils.GetFpsFromString(fpsBox.Text);
        }
    }
}
