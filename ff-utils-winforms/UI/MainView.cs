using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.IO;
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
                        if(s.Type == Stream.StreamType.Video)
                        {
                            VideoStream vs = (VideoStream)s;
                            box.Items.Add($"#{i}: {s.Type} ({s.Codec}) - {vs.Resolution.Width}x{vs.Resolution.Height} - {vs.Rate.GetString()} FPS");
                        }

                        if (s.Type == Stream.StreamType.Audio)
                        {
                            AudioStream @as = (AudioStream)s;
                            string title = string.IsNullOrWhiteSpace(@as.Title.Trim()) ? "No Title" : @as.Title;
                            box.Items.Add($"#{i}: {s.Type} ({s.Codec}) - {title} - {@as.Layout}");
                        }

                        if (s.Type == Stream.StreamType.Subtitle)
                        {
                            SubtitleStream ss = (SubtitleStream)s;
                            string lang = string.IsNullOrWhiteSpace(ss.Language.Trim()) ? "Unknown Language" : ss.Language;
                            box.Items.Add($"#{i}: {s.Type} ({s.Codec}) - {lang}");
                        }

                        Program.mainForm.streamListBox.SetItemChecked(i, true);
                    }
                }
            }
        }
    }
}
