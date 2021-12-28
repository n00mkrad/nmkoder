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

        public static async Task HandleFiles(string[] paths, bool clearExisting)
        {
            if (clearExisting)
            {
                ThumbnailView.ClearUi();
                ClearCurrentFile();
                Logger.ClearLogBox();
            }

            bool runInstantly = RunTask.RunInstantly();

            if(!runInstantly)
                Program.mainForm.mainTabList.SelectedIndex = 0;

            Logger.Log($"Added {paths.Length} file{((paths.Length == 1) ? "" : "s")} to list.");
            FileList.LoadFiles(paths, clearExisting);

            if (RunTask.currentFileListMode == RunTask.FileListMode.MultiFileInput && Program.mainForm.fileListBox.Items.Count == 1)
                await LoadFirstFile(Program.mainForm.fileListBox.Items[0]);

            if (runInstantly)
                Program.mainForm.runBtn_Click();
        }

        public static void ClearCurrentFile()
        {
            current = null;
            Program.mainForm.ffmpegOutputBox.Text = "";
            Program.mainForm.streamList.Items.Clear();
            Program.mainForm.streamDetailsBox.Text = "";
            Program.mainForm.formatInfoLabel.Text = "";
            Program.mainForm.metaGrid.Columns.Clear();
            Program.mainForm.metaGrid.Rows.Clear();
            ThumbnailView.ClearUi();
            QuickConvertUi.currentCropValues = null;
            Av1anUi.currentCropValues = null;
        }

        // public static async Task LoadFirstFile(string path, bool switchToTrackList = true, bool generateThumbs = true)
        // {
        //     MediaFile mediaFile = new MediaFile(path);
        //     await LoadFirstFile(mediaFile, switchToTrackList, generateThumbs);
        // }

        public static async Task LoadFirstFile(ListViewItem item, bool switchToTrackList = true, bool generateThumbs = true)
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
            Program.mainForm.formatInfoLabel.Text = $"{titleStr}Format: {current.File.Format} - Duration: {dur}{br} - Size: {FormatUtils.Bytes(current.File.Size)}";
            Program.mainForm.streamList.Items.Clear();
            currentAudioConfig = null;
            await AddStreamsToList(current.File, item.BackColor, switchToTrackList);

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

            foreach (Stream s in mediaFile.AllStreams)
            {
                try
                {
                    MediaStreamListEntry entry = new MediaStreamListEntry(mediaFile, s);
                    list.Items.Add(new ListViewItem { Text = entry.ToString(), Tag = entry, BackColor = color });
                    bool check = s.Codec.ToLower().Trim() != "unknown" && !(s.Type == Stream.StreamType.Video && alreadyHasVidStream);
                    Program.mainForm.ignoreNextStreamListItemCheck = true;
                    list.Items.Cast<ListViewItem>().Last().Checked = check;
                }
                catch (Exception e)
                {
                    Logger.Log($"Error trying to load streams into UI: {e.Message}\n{e.StackTrace}");
                }
            }

            if (switchToList && !RunTask.RunInstantly())
                Program.mainForm.mainTabList.SelectedIndex = 1;

            Program.mainForm.UpdateDefaultStreamsUi();
            Program.mainForm.UpdateTrackListUpDownBtnsState();
        }

        public static void Refresh ()
        {
            List<string> loadedPaths = Program.mainForm.fileListBox.Items.Cast<ListViewItem>().Select(x => ((MediaFile)x.Tag).ImportPath).ToList();

            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                MediaStreamListEntry entry = (MediaStreamListEntry)Program.mainForm.streamList.Items[i].Tag;

                if (entry.MediaFile == null || !loadedPaths.Contains(entry.MediaFile.ImportPath))
                {
                    Program.mainForm.streamList.Items.Remove(Program.mainForm.streamList.Items[i]);
                    i = 0; // Reset loop index, otherwise removing will result in skipped entries
                }
            }
        }

        public static string GetStreamDetails(Stream stream, MediaFile mediaFile = null)
        {
            if (stream == null)
                return "";

            List<string> lines = new List<string>();
            lines.Add($"Source File: {(mediaFile != null ? Path.GetFileName(mediaFile.SourcePath) : "Unknown")}");
            lines.Add($"Codec: {stream.CodecLong}");

            if (stream.Type == Stream.StreamType.Video)
            {
                VideoStream v = (VideoStream)stream;
                lines.Add($"Title: {((v.Title.Trim().Length > 1) ? v.Title : "None")}");
                lines.Add($"Resolution and Aspect Ratio: {v.Resolution.ToStringShort()} - SAR {v.Sar.ToStringShort(":")} - DAR {v.Dar.ToStringShort(":")}");
                lines.Add($"Color Space: {v.ColorSpace}{(v.ColorSpace.ToLower().Contains("p10") ? " (10-bit)" : " (8-bit)")}");
                lines.Add($"Frame Rate: {v.Rate} (~{v.Rate.GetString()} FPS)");
            }

            if (stream.Type == Stream.StreamType.Audio)
            {
                AudioStream a = (AudioStream)stream;
                lines.Add($"Title: {((a.Title.Trim().Length > 1) ? a.Title : "None")}");
                lines.Add($"Sample Rate: {((a.SampleRate > 1) ? $"{a.SampleRate} KHz" : "None")}");
                lines.Add($"Channels: {((a.Channels > 0) ? $"{a.Channels}" : "Unknown")} {(a.Layout.Trim().Length > 1 ? $"as {a.Layout.ToTitleCase()}" : "")}");
                lines.Add($"Language: {((a.Language.Trim().Length > 1) ? $"{FormatUtils.CapsIfShort(a.Language, 4)}" : "Unknown")}");
            }

            if (stream.Type == Stream.StreamType.Subtitle)
            {
                SubtitleStream s = (SubtitleStream)stream;
                lines.Add($"Title: {((s.Title.Trim().Length > 1) ? s.Title : "None")}");
                lines.Add($"Language: {((s.Language.Trim().Length > 1) ? $"{FormatUtils.CapsIfShort(s.Language, 4)}" : "Unknown")}");
                lines.Add($"Type: {((s.Bitmap) ? $"Bitmap-based" : "Text-based")}");
            }

            return string.Join(Environment.NewLine, lines);
        }

        public static List<string> GetInputFiles()
        {
            List<string> paths = Program.mainForm.streamList.Items.Cast<ListViewItem>().Select(x => ((MediaStreamListEntry)x.Tag).MediaFile.ImportPath).ToList();
            List<string> pathsUnique = paths.Select(x => x).Distinct().ToList();

            Logger.Log($"Input Files: {string.Join(", ", pathsUnique)}", true);
            return pathsUnique;
        }

        public static string GetInputFilesString()
        {
            List<string> args = new List<string>();
            List<string> addedFiles = new List<string>();

            foreach ( ListViewItem item in Program.mainForm.fileListBox.Items)
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

                    int fileIdx = entry.ToString().Split('-')[0].GetInt() - 1;

                    if (!fileIndexesToMap.Contains(fileIdx))
                        fileIndexesToMap.Add(fileIdx);

                    args.Add($"-map {fileIdx}:{entry.Stream.Index}");
                }
            }

            return string.Join(" ", args);
        }

        #region Stream Selection

        public static void CheckAll (bool check)
        {
            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                Program.mainForm.ignoreNextStreamListItemCheck = i < (Program.mainForm.streamList.Items.Count - 1);
                Program.mainForm.streamList.Items[i].Checked = check;
            }
        }

        public static void InvertSelection(object sender = null, EventArgs e = null)
        {
            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                Program.mainForm.ignoreNextStreamListItemCheck = i < (Program.mainForm.streamList.Items.Count - 1);
                Program.mainForm.streamList.Items[i].Checked = !Program.mainForm.streamList.Items[i].Checked;
            }
        }

        public static void CheckTracksOfType(Stream.StreamType type)
        {
            for (int i = 0; i < Program.mainForm.streamList.Items.Count; i++)
            {
                Program.mainForm.ignoreNextStreamListItemCheck = i < (Program.mainForm.streamList.Items.Count - 1);
                Program.mainForm.streamList.Items[i].Checked = ((MediaStreamListEntry)Program.mainForm.streamList.Items[i].Tag).Stream.Type == type;
            }
        }

        #endregion
    }
}
