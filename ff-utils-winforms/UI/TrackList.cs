using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.Properties;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI
{
    class TrackList
    {
        public static FileListEntry current;
        public static AudioConfiguration currentAudioConfig = null;

        public static void ClearCurrentFile(bool clearStreamList = false)
        {
            current = null;
            Program.mainForm.FfmpegOutputBox.Text = "";

            if(clearStreamList)
                Program.mainForm.streamList.Items.Clear();

            Program.mainForm.streamDetailsBox.Text = "";
            Program.mainForm.FormatInfoLabel.Text = "";
            Program.mainForm.MetaGrid.Columns.Clear();
            Program.mainForm.MetaGrid.Rows.Clear();
            ThumbnailView.ClearUi();
            QuickConvertUi.currentCropValues = null;
            Av1anUi.currentCropValues = null;
        }

        public static async Task SetAsMainFile(ListViewItem item, bool switchToTrackList = true, bool generateThumbs = true)
        {
            MediaFile mediaFile = ((FileListEntry)item.Tag).File;
            int streamCount = await FfmpegUtils.GetStreamCount(mediaFile.ImportPath);
            Logger.Log($"Scanning '{mediaFile.Name}' (Streams: {streamCount})...");
            await mediaFile.Initialize();
            PrintFoundStreams(mediaFile);
            current = new FileListEntry(mediaFile);

            string titleStr = current.Title.Trim().Length > 2 ? $"Title: {current.Title.Trunc(30)} - " : "";
            string br = current.File.TotalKbits > 0 ? $" - Bitrate: {FormatUtils.Bitrate(current.File.TotalKbits)}" : "";
            string dur = FormatUtils.MsToTimestamp(current.File.DurationMs);
            Program.mainForm.FormatInfoLabel.Text = $"{titleStr}Format: {current.File.Format} - Duration: {dur}{br} - Size: {FormatUtils.Bytes(current.File.Size)}";
            Program.mainForm.streamList.Items.Clear();
            currentAudioConfig = null;
            //await AddStreamsToList(current.File, item.BackColor, switchToTrackList);

            QuickConvertUi.InitFile(current.File.SourcePath);
            Av1anUi.InitFile(current.File.SourcePath);

            if (generateThumbs)
                Task.Run(() => ThumbnailView.GenerateThumbs(mediaFile.SourcePath)); // Generate thumbs in background
        }

        private static void PrintFoundStreams(MediaFile mediaFile)
        {
            List<string> foundTracks = new List<string>();

            if (mediaFile.VideoStreams.Count > 0) foundTracks.Add($"{mediaFile.VideoStreams.Count} video track{(mediaFile.VideoStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.AudioStreams.Count > 0) foundTracks.Add($"{mediaFile.AudioStreams.Count} audio track{(mediaFile.AudioStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.SubtitleStreams.Count > 0) foundTracks.Add($"{mediaFile.SubtitleStreams.Count} subtitle track{(mediaFile.SubtitleStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.DataStreams.Count > 0) foundTracks.Add($"{mediaFile.DataStreams.Count} data track{(mediaFile.DataStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.AttachmentStreams.Count > 0) foundTracks.Add($"{mediaFile.AttachmentStreams.Count} attachment{(mediaFile.AttachmentStreams.Count == 1 ? "" : "s")}");

            if (foundTracks.Count > 0)
                Logger.Log($"Found {string.Join(", ", foundTracks)}.");
            else
                Logger.Log($"Found no media streams in '{mediaFile.Name}'!");
        }

        public static async Task AddStreamsToList(MediaFile mediaFile, Color color, bool switchToList, bool silent = false)
        {
            ListView list = Program.mainForm.streamList;
            int uniqueFileCount = (from x in list.Items.Cast<ListViewItem>().Select(x => ((MediaStreamListEntry)x.Tag).MediaFile.ImportPath) select x).Distinct().Count();

            if (!mediaFile.Initialized)
            {
                if (!silent)
                    Logger.Log($"Scanning '{mediaFile.Name}'...");

                await mediaFile.Initialize();

                if (!silent)
                    PrintFoundStreams(mediaFile);
            }

            bool alreadyHasVidStream = list.Items.Cast<ListViewItem>().Where(x => ((MediaStreamListEntry)x.Tag).Stream.Type == Stream.StreamType.Video).Count() > 0;

            Random r = new Random();

            Program.mainForm.ignoreStreamListCheck = true;
            list.BeginUpdate();

            foreach (Stream s in mediaFile.AllStreams)
            {
                try
                {
                    MediaStreamListEntry entry = new MediaStreamListEntry(mediaFile, s);
                    bool check = s.Codec.ToLower().Trim() != "unknown" && !(s.Type == Stream.StreamType.Video && alreadyHasVidStream);
                    list.Items.Add(new ListViewItem { Text = entry.ToString(), Tag = entry, BackColor = color, Checked = check });
                }
                catch (Exception e)
                {
                    Logger.Log($"Error trying to load streams into UI: {e.Message}\n{e.StackTrace}");
                }
            }

            list.EndUpdate();

            if (switchToList && !RunTask.RunInstantly())
                Program.mainForm.MainTabList.SelectedIndex = 1;

            Program.mainForm.OnCheckedStreamsChange();
            Program.mainForm.ignoreStreamListCheck = false;
            Program.mainForm.UpdateDefaultStreamsUi();
            Program.mainForm.UpdateTrackListUpDownBtnsState();
        }

        public static void Refresh()
        {
            List<string> loadedPaths = Program.mainForm.fileListBox.Items.Cast<ListViewItem>().Select(x => ((FileListEntry)x.Tag).File.ImportPath).ToList();

            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                MediaStreamListEntry entry = (MediaStreamListEntry)Program.mainForm.streamList.Items[i].Tag;

                if (entry.MediaFile == null || !loadedPaths.Contains(entry.MediaFile.ImportPath))
                {
                    Program.mainForm.streamList.Items.Remove(Program.mainForm.streamList.Items[i]);
                    i = 0; // Reset loop index, otherwise removing will result in skipped entries
                }
            }

            Program.mainForm.RefreshFileListUi();
        }

        public static string GetStreamDetails(Stream stream, MediaFile mediaFile = null)
        {
            if (stream == null)
                return "";

            List<string> lines = new List<string>();
            string ext = Path.GetExtension(mediaFile.SourcePath);
            lines.Add($"Source File: {(mediaFile != null ? Path.GetFileNameWithoutExtension(mediaFile.SourcePath).Trunc(90 - ext.Length) + ext : "Unknown")}");
            lines.Add($"Codec: {stream.CodecLong} ({stream.Codec})");

            if (stream.Type == Stream.StreamType.Video)
            {
                VideoStream v = (VideoStream)stream;
                lines.Add($"Title: {((v.Title.Trim().Length > 1) ? v.Title.Trunc(90) : "None")}");
                lines.Add($"Resolution and Aspect Ratio: {v.Resolution.ToStringShort()} - SAR {v.Sar.ToStringShort(":")} - DAR {v.Dar.ToStringShort(":")}");
                int bitDepth = GetBitDepthFromPixelFormat(v.PixelFormat);
                lines.Add($"Color Format: {v.PixelFormat}{(bitDepth > 0 ? $" ({bitDepth}-bit)" : "")}");
                lines.Add($"Frame Rate: {v.Rate} (~{v.Rate.GetString()} FPS)");
            }

            else if (stream.Type == Stream.StreamType.Audio)
            {
                AudioStream a = (AudioStream)stream;
                lines.Add($"Title: {((a.Title.Trim().Length > 1) ? a.Title.Trunc(90) : "None")}");
                lines.Add($"Sample Rate: {((a.SampleRate > 1) ? $"{a.SampleRate} KHz" : "None")}");
                string chLayout = (a.Layout.Contains("(") && !a.Layout.Contains(" (") ? a.Layout.Replace("(", " (") : a.Layout);
                lines.Add($"Channels: {((a.Channels > 0) ? $"{a.Channels}" : "Unknown")} {(a.Layout.Trim().Length > 1 ? $"as {chLayout.ToTitleCase()}" : "")}");
                lines.Add($"Language: {((a.Language.Trim().Length > 1) ? $"{Aliases.GetLanguageString(a.Language)}" : "Unknown")}");
            }

            else if (stream.Type == Stream.StreamType.Subtitle)
            {
                SubtitleStream s = (SubtitleStream)stream;
                lines.Add($"Title: {((s.Title.Trim().Length > 1) ? s.Title.Trunc(90) : "None")}");
                lines.Add($"Language: {((s.Language.Trim().Length > 1) ? $"{Aliases.GetLanguageString(s.Language)}" : "Unknown")}");
                lines.Add($"Type: {((s.Bitmap) ? $"Bitmap-based" : "Text-based")}");
            }

            else if (stream.Type == Stream.StreamType.Attachment)
            {
                AttachmentStream a = (AttachmentStream)stream;
                lines.Add($"Filename: {((a.Filename.Trim().Length > 1) ? a.Filename.Trunc(90) : "None")}");
                lines.Add($"MIME Type: {((a.MimeType.Trim().Length > 1) ? $"{Aliases.GetLanguageString(a.MimeType)}" : "Unknown")}");
            }

            return string.Join(Environment.NewLine, lines);
        }

        private static int GetBitDepthFromPixelFormat(string pixFmt)
        {
            pixFmt = pixFmt.ToLower();
            if (pixFmt.MatchesWildcard("yuv*p")) return 8;
            if (pixFmt.MatchesWildcard("*p10?e")) return 10;
            if (pixFmt.MatchesWildcard("*p12?e")) return 12;
            if (pixFmt.MatchesWildcard("*p16?e")) return 16;
            return 0;
        }

        public static string GetInputFilesString()
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.Batch)
            {
                if (current.File.IsDirectory)
                    return $"-safe 0 -f concat -r {current.File.InputRate} -i {current.File.ImportPath.Wrap()}";
                else
                    return $"-i {current.File.ImportPath.Wrap()}";
            }

            List<string> args = new List<string>();
            List<string> addedFiles = new List<string>();

            foreach (ListViewItem item in Program.mainForm.fileListBox.Items)
            {
                FileListEntry entry = (FileListEntry)item.Tag;
                addedFiles.Add(entry.File.SourcePath);

                if (entry.File.IsDirectory)
                    args.Add($"-safe 0 -f concat -r {entry.File.InputRate} -i {entry.File.ImportPath.Wrap()}");
                else
                    args.Add($"-i {entry.File.ImportPath.Wrap()}");
            }

            Logger.Log($"Input Args: {string.Join(" ", args)}", true);
            return string.Join(" ", args);
        }

        public static string GetMapArgs()
        {
            List<string> args = new List<string>();
            List<int> fileIndexesToMap = new List<int>();

            foreach (ListViewItem item in Program.mainForm.streamList.Items.Cast<ListViewItem>())
            {
                if (item.Checked)
                {
                    MediaStreamListEntry entry = (MediaStreamListEntry)item.Tag;

                    ListViewItem correspodingFileEntry = Program.mainForm.fileListBox.Items.Cast<ListViewItem>().Where(x => ((FileListEntry)x.Tag).File == entry.MediaFile).First();
                    int fileIdx = RunTask.currentFileListMode == RunTask.FileListMode.Batch ? 0 : Program.mainForm.fileListBox.Items.IndexOf(correspodingFileEntry);

                    if (!fileIndexesToMap.Contains(fileIdx))
                        fileIndexesToMap.Add(fileIdx);

                    args.Add($"-map {fileIdx}:{entry.Stream.Index}");
                }
            }

            return string.Join(" ", args);
        }

        #region Stream Selection

        public static void CheckAll(bool check)
        {
            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                Program.mainForm.ignoreStreamListCheck = i < (Program.mainForm.streamList.Items.Count - 1);
                Program.mainForm.streamList.Items[i].Checked = check;
            }

            Program.mainForm.OnCheckedStreamsChange();
        }

        public static void InvertSelection(object sender = null, EventArgs e = null)
        {
            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                Program.mainForm.ignoreStreamListCheck = i < (Program.mainForm.streamList.Items.Count - 1);
                Program.mainForm.streamList.Items[i].Checked = !Program.mainForm.streamList.Items[i].Checked;
            }

            Program.mainForm.OnCheckedStreamsChange();
        }

        public static void CheckTracksOfType(Stream.StreamType type)
        {
            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                Program.mainForm.ignoreStreamListCheck = i < (Program.mainForm.streamList.Items.Count - 1);
                Program.mainForm.streamList.Items[i].Checked = ((MediaStreamListEntry)Program.mainForm.streamList.Items[i].Tag).Stream.Type == type;
            }

            Program.mainForm.OnCheckedStreamsChange();
        }

        public static void CheckFirstOfEachType()
        {
            ListView list = Program.mainForm.streamList;
            var firstVid = list.Items.Cast<ListViewItem>().Where(x => ((MediaStreamListEntry)x.Tag).Stream.Type == Stream.StreamType.Video).FirstOrDefault();
            var firstAud = list.Items.Cast<ListViewItem>().Where(x => ((MediaStreamListEntry)x.Tag).Stream.Type == Stream.StreamType.Audio).FirstOrDefault();
            var firstSub = list.Items.Cast<ListViewItem>().Where(x => ((MediaStreamListEntry)x.Tag).Stream.Type == Stream.StreamType.Subtitle).FirstOrDefault();

            for (int i = 0; i < list.Items.Count; i++)
            {
                Program.mainForm.ignoreStreamListCheck = i < (list.Items.Count - 1);
                list.Items[i].Checked = list.Items[i] == firstVid || list.Items[i] == firstAud || list.Items[i] == firstSub;
            }

            Program.mainForm.OnCheckedStreamsChange();
        }

        public static void CheckFirstOfEachLangOfEachType()
        {
            ListView list = Program.mainForm.streamList;
            List<string> checkedLangs = new List<string>();

            for (int i = 0; i < list.Items.Count; i++)
            {
                Program.mainForm.ignoreStreamListCheck = i < (list.Items.Count - 1);

                MediaStreamListEntry entry = (MediaStreamListEntry)list.Items[i].Tag;
                string hash = $"{entry.Stream.Type}{entry.Stream.Language}";

                if (checkedLangs.Contains(hash))
                {
                    list.Items[i].Checked = false;
                }
                else
                {
                    list.Items[i].Checked = true;
                    checkedLangs.Add(hash);
                }
            }

            Program.mainForm.OnCheckedStreamsChange();
        }

        #endregion

        #region Sort Tracks

        public enum TrackSort { Language, Title, Codec }

        public static void SortTracks(TrackSort sort, bool reverse)
        {
            ListView list = Program.mainForm.streamList;
            List<ListViewItem> itemsCopy = new List<ListViewItem>(list.Items.Cast<ListViewItem>());
            Program.mainForm.ignoreStreamListCheck = true;
            list.Items.Clear();
            List<Stream.StreamType> streamTypes = itemsCopy.Select(x => ((MediaStreamListEntry)x.Tag).Stream.Type).Distinct().ToList();

            foreach (Stream.StreamType streamType in streamTypes)
            {
                var items = itemsCopy.Where(x => ((MediaStreamListEntry)x.Tag).Stream.Type == streamType);
                var sorted = new List<ListViewItem>();

                if(sort == TrackSort.Language)
                    sorted = items.OrderBy(x => ((MediaStreamListEntry)x.Tag).Stream.Language).ToList();
                else if (sort == TrackSort.Title)
                    sorted = items.OrderBy(x => ((MediaStreamListEntry)x.Tag).Stream.Title).ToList();
                else if (sort == TrackSort.Codec)
                    sorted = items.OrderBy(x => ((MediaStreamListEntry)x.Tag).Stream.Codec).ToList();

                list.Items.AddRange(reverse ? sorted.AsEnumerable().Reverse().ToArray() : sorted.ToArray());
            }

            Program.mainForm.ignoreStreamListCheck = false;
            Program.mainForm.OnCheckedStreamsChange();
        }

        #endregion
    }
}
