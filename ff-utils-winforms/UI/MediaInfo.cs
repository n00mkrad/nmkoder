﻿using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.Properties;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI
{
    class MediaInfo
    {
        public static MediaFile current;

        public static async Task HandleFiles (string[] paths)
        {
            ThumbnailView.ClearUi();

            if (paths.Length == 1)
            {
                await LoadFileInfo(paths[0]);
            }
            else
            {
                // many
            }
        }

        public static async Task LoadFileInfo (string path)
        {
            NmkdStopwatch sw = new NmkdStopwatch();
            //Task.Run(() => ThumbnailView.GenerateThumbs(path)); // Generate thumbs in background

            MediaFile mediaFile = new MediaFile(path);
            int streamCount = await FfmpegUtils.GetStreamCount(path);
            Logger.Log($"Loading info for {streamCount} streams.");
            await mediaFile.Initialize();
            Logger.Log($"Loaded all media info.");
            current = mediaFile;

            string getTitle = await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "TAG:title");
            string titleStr = getTitle.Trim().Length > 2 ? $"Title: {getTitle.Trunc(28)} - " : "";
            string br = current.TotalKbits > 0 ? $" - Total Bitrate: {FormatUtils.Bitrate(current.TotalKbits)}" : "";
            string dur = FormatUtils.MsToTimestamp(FfmpegCommands.GetDurationMs(path));
            Program.mainForm.formatInfoLabel.Text = $"{titleStr}Format: {current.Ext.ToUpper()} - Streams: {current.StreamCount} - Duration: {dur}{br}";

            CheckedListBox box = Program.mainForm.streamListBox;
            box.Items.Clear();

            for (int i = 0; i < current.AllStreams.Count; i++)
            {
                try
                {
                    foreach (Stream s in current.AllStreams) // This is somewhat unnecessary but ensures that the order of the list matches the stream index.
                    {
                        if (s.Index == i)
                        {
                            string codec = FormatUtils.CapsIfShort(s.Codec, 5);
                            const int maxChars = 50;

                            if (s.Type == Stream.StreamType.Video)
                            {
                                VideoStream vs = (VideoStream)s;
                                string codecStr = vs.Kbits > 0 ? $"{codec} at {FormatUtils.Bitrate(vs.Kbits)}" : codec;
                                box.Items.Add($"#{i}: {s.Type} ({codecStr}) - {vs.Resolution.Width}x{vs.Resolution.Height} - {vs.Rate.GetString()} FPS");
                            }

                            if (s.Type == Stream.StreamType.Audio)
                            {
                                AudioStream @as = (AudioStream)s;
                                string title = string.IsNullOrWhiteSpace(@as.Title.Trim()) ? " " : $" - {@as.Title.Trunc(maxChars)} ";
                                string codecStr = @as.Kbits > 0 ? $"{codec} at {FormatUtils.Bitrate(@as.Kbits)}" : codec;
                                box.Items.Add($"#{i}: {s.Type} ({codecStr}){title}- {@as.Layout.ToTitleCase()}");
                            }

                            if (s.Type == Stream.StreamType.Subtitle)
                            {
                                SubtitleStream ss = (SubtitleStream)s;
                                string lang = string.IsNullOrWhiteSpace(ss.Language.Trim()) ? " " : $" - {FormatUtils.CapsIfShort(ss.Language, 4).Trunc(maxChars)} ";
                                string ttl = string.IsNullOrWhiteSpace(ss.Title.Trim()) ? " " : $" - {FormatUtils.CapsIfShort(ss.Title, 4).Trunc(maxChars)} ";
                                box.Items.Add($"#{i}: {s.Type} ({codec}){lang}{ttl}");
                            }

                            if (s.Type == Stream.StreamType.Data)
                            {
                                DataStream ds = (DataStream)s;
                                box.Items.Add($"#{i}: {s.Type} ({codec})");
                            }

                            if(i >= 0 && i < Program.mainForm.streamListBox.Items.Count)
                                Program.mainForm.streamListBox.SetItemChecked(i, true);
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.Log($"Error trying to load streams into UI: {e.Message}\n{e.StackTrace}");
                }
                
            }

            Program.mainForm.outputBox.Text = IoUtils.FilenameSuffix(current.File.FullName, ".convert");
            QuickConvertUi.ValidateContainer();
            Program.mainForm.mainTabList.SelectedIndex = 0;

            Task.Run(() => ThumbnailView.GenerateThumbs(path)); // Generate thumbs in background
        }

        public static string GetStreamDetails(int index)
        {
            if (index < 0)
                return "";

            Stream stream = current.AllStreams[index];
            List<string> lines = new List<string>();
            lines.Add($"Codec: {stream.CodecLong}");

            if(stream.Type == Stream.StreamType.Video)
            {
                VideoStream v = (VideoStream)stream;
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
            }

            return string.Join(Environment.NewLine, lines);
        }

        public static string GetMapArgs ()
        {
            List<string> args = new List<string>();

            for (int i = 0; i < Program.mainForm.streamListBox.Items.Count; i++)
            {
                if (Program.mainForm.streamListBox.GetItemChecked(i))
                    args.Add($"-map 0:{i}");
            }

            if (args.Count == Program.mainForm.streamListBox.Items.Count)
                return "-map 0";
            else
                return string.Join(" ", args);
        }
    }
}
