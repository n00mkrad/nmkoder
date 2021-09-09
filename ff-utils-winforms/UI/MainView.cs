using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    class MainView
    {
        public static MediaFile currentFile; 

        public static async Task HandleFiles (string[] paths)
        {
            if(paths.Length == 1)
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
            MediaFile mediaFile = new MediaFile(path);
            await mediaFile.Initialize();
            Logger.Log($"Initialized MediaFile.");
            currentFile = mediaFile;
            CheckedListBox box = Program.mainForm.streamListBox;
            box.Items.Clear();
            //Program.mainForm.inputInfoTextbox.Text = string.Join("\n", mediaFile.VideoStreams) + "\n" + string.Join("\n", mediaFile.AudioStreams);

            for (int i = 0; i < currentFile.AllStreams.Count; i++)
            {
                foreach(Stream s in currentFile.AllStreams) // This is somewhat unnecessary but ensures that the order of the list matches the stream index.
                {
                    if(s.Index == i)
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

                        Program.mainForm.streamListBox.SetItemChecked(i, true);
                    }
                }
            }
        }
    }
}
