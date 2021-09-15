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
    class MediaInfo
    {
        public static MediaFile current;
        public static bool streamListLoaded;

        public static async Task HandleFiles (string[] paths)
        {
            RunTask.currentFileListMode = RunTask.FileListMode.BatchProcess;
            ThumbnailView.ClearUi();
            Logger.ClearLogBox();
            
            Logger.Log($"Added {paths.Length} file{((paths.Length == 1) ? "" : "s")} to list.");

            Program.mainForm.ClearCurrentFile();

            FileList.LoadFiles(paths);

            await LoadFileInfo(paths[0]);
        }

        public static async Task LoadFileInfo(string path, bool switchToTrackList = true, bool generateThumbs = true)
        {
            streamListLoaded = false;
            MediaFile mediaFile = new MediaFile(path);
            int streamCount = await FfmpegUtils.GetStreamCount(path);
            Logger.Log($"Scanning '{mediaFile.File.Name}' (Streams: {streamCount})...");
            await mediaFile.Initialize();
            PrintFoundStreams(mediaFile);
            current = mediaFile;
            QuickConvertUi.lastMap = "";

            string titleStr = current.Title.Trim().Length > 2 ? $"Title: {current.Title.Trunc(30)} - " : "";
            string br = current.TotalKbits > 0 ? $" - Bitrate: {FormatUtils.Bitrate(current.TotalKbits)}" : "";
            string dur = FormatUtils.MsToTimestamp(current.DurationMs);
            Program.mainForm.formatInfoLabel.Text = $"{titleStr}Format: {current.Ext.ToUpper()} - Duration: {dur}{br} - Size: {FormatUtils.Bytes(current.SizeKb * 1024)}";

            await AddStreamsToList(current, true);

            streamListLoaded = true;
            Program.mainForm.outputBox.Text = IoUtils.FilenameSuffix(current.File.FullName, ".convert");
            Program.mainForm.encVidFpsBox.Text = current.VideoStreams.FirstOrDefault()?.Rate.ToString();
            QuickConvertUi.InitFile();

            if(switchToTrackList)
                Program.mainForm.mainTabList.SelectedIndex = 1;

            if(generateThumbs)
                Task.Run(() => ThumbnailView.GenerateThumbs(path)); // Generate thumbs in background
        }

        private static void PrintFoundStreams(MediaFile mediaFile)
        {
            List<string> foundTracks = new List<string>();

            if (mediaFile.VideoStreams.Count > 0) foundTracks.Add($"{mediaFile.VideoStreams.Count} video track{(mediaFile.VideoStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.AudioStreams.Count > 0) foundTracks.Add($"{mediaFile.AudioStreams.Count} audio track{(mediaFile.AudioStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.SubtitleStreams.Count > 0) foundTracks.Add($"{mediaFile.SubtitleStreams.Count} subtitle track{(mediaFile.SubtitleStreams.Count == 1 ? "" : "s")}");
            if (mediaFile.DataStreams.Count > 0) foundTracks.Add($"{mediaFile.DataStreams.Count} data track{(mediaFile.DataStreams.Count == 1 ? "" : "s")}");

            if (foundTracks.Count > 0)
                Logger.Log($"Found {string.Join(", ", foundTracks)}.");
            else
                Logger.Log($"Found no media streams in '{mediaFile.File.Name}'!");
        }

        public static async Task AddStreamsToList (MediaFile mediaFile, bool clear, bool printInit = true)
        {
            CheckedListBox box = Program.mainForm.streamListBox;
            int uniqueFileCount = (from x in box.Items.OfType<MediaStreamListEntry>().Select(x => x.MediaFile.File.FullName) select x).Distinct().Count();

            if (uniqueFileCount > 0 && RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess)
            {
                RunTask.currentFileListMode = RunTask.FileListMode.MultiFileInput; // Disable batch processing when using multiple input files
                Logger.Log($"Using multiple files as input for one output file - Batch Processing is disabled until you load a new set of files.");
            }

            if(clear)
                box.Items.Clear();

            if (!mediaFile.Initialized)
            {
                if (printInit)
                    Logger.Log($"Scanning '{mediaFile.File.Name}'...");

                await mediaFile.Initialize();

                if(printInit)
                    PrintFoundStreams(mediaFile);
            }

            bool alreadyHasVidStream = box.Items.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Video).Count() > 0;

            foreach (Stream s in mediaFile.AllStreams)
            {
                try
                {
                    box.Items.Add(new MediaStreamListEntry(mediaFile, s, uniqueFileCount));
                    bool check = s.Codec.ToLower().Trim() != "unknown" && !alreadyHasVidStream;
                    box.SetItemChecked(box.Items.Count - 1, check);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error trying to load streams into UI: {e.Message}\n{e.StackTrace}");
                }
            }

            //for (int i = 0; i < mediaFile.AllStreams.Count; i++)
            //{
            //    try
            //    {
            //        foreach (Stream s in mediaFile.AllStreams) // This is somewhat unnecessary but ensures that the order of the list matches the stream index.
            //        {
            //            if (s.Index == i)
            //            {
            //                box.Items.Add(new MediaStreamListEntry(mediaFile, s, fileIdx));
            //
            //                if (i >= 0 && i < box.Items.Count && s.Codec.ToLower().Trim() != "unknown")
            //                    box.SetItemChecked(i, true);
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Logger.Log($"Error trying to load streams into UI: {e.Message}\n{e.StackTrace}");
            //    }
            //}
        }

        public static string GetStreamDetails(Stream stream)
        {
            if (stream == null)
                return "";

            List<string> lines = new List<string>();
            lines.Add($"Codec: {stream.CodecLong}");

            if(stream.Type == Stream.StreamType.Video)
            {
                VideoStream v = (VideoStream)stream;
                lines.Add($"Title: {((v.Title.Trim().Length > 1) ? v.Title : "None")}");
                lines.Add($"Resolution and Aspect Ratio: {v.Resolution.ToStringShort()} - SAR {v.Sar.ToStringShort(":")} - DAR {v.Dar.ToStringShort(":")}");
                lines.Add($"Color Space: {v.ColorSpace}{(v.ColorSpace.ToLower().Contains("p10") ? " (10-bit)" : " (8-bit)")}");
                lines.Add($"Frame Rate: {v.Rate} (~{v.Rate.GetString()} FPS)");
                lines.Add($"Language: {((v.Language.Trim().Length > 1) ? $"{FormatUtils.CapsIfShort(v.Language, 4)}" : "Unknown")}");
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

        public static string GetInputFiles()
        {
            List<string> files = new List<string>();

            foreach(MediaStreamListEntry entry in Program.mainForm.streamListBox.Items)
            {
                if (entry.Stream.Index == 0)
                    files.Add($"-i {entry.MediaFile.File.FullName.Wrap()}");
            }

            return string.Join(" ", files);
        }

        public static string GetMapArgs ()
        {
            List<string> args = new List<string>();
            List<string> files = new List<string>();

            //for (int i = 0; i < Program.mainForm.streamListBox.Items.Count; i++)
            //{
            //    if (Program.mainForm.streamListBox.GetItemChecked(i))
            //    {
            //        Stream stream = ((MediaStreamListEntry)Program.mainForm.streamListBox.Items[i]).Stream;
            //        args.Add($"-map {}:{stream.Index}");
            //    }
            //}

            bool all = true;

            int fileIdx = -1;

            foreach (MediaStreamListEntry entry in Program.mainForm.streamListBox.Items)
            {
                if (entry.Stream.Index == 0)
                {
                    fileIdx++;
                    files.Add($"-map {fileIdx}");
                }

                if (Program.mainForm.streamListBox.GetItemChecked(Program.mainForm.streamListBox.Items.IndexOf(entry)))
                    args.Add($"-map {fileIdx}:{entry.Stream.Index}");
                else
                    all = false;
            }

            if (all)
                return string.Join(" ", files);
            else
                return string.Join(" ", args);
        }
    }
}
