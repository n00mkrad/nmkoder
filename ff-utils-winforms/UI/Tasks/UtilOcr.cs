using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.Utils;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    class UtilOcr
    {
        public static async Task Run ()
        {
            Program.mainForm.SetWorking(true);
            List<MediaStreamListEntry> checkedStreams = Program.mainForm.streamListBox.CheckedItems.OfType<MediaStreamListEntry>().ToList();
            List<SubtitleStream> subStreams = checkedStreams.Where(x => x.Stream.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x.Stream).ToList();
            int i = 0;

            List<SubtitleStream> streamsText = subStreams.Where(x => !x.Bitmap).ToList();
            List<SubtitleStream> streamsBitmap = subStreams.Where(x => x.Bitmap).ToList();

            string inPath = TrackList.current.TruePath;
            string outDirName = Path.GetFileNameWithoutExtension(inPath).CleanString().Trunc(50, false) + "-Subtitles";
            string outDir = Path.Combine(new FileInfo(inPath).DirectoryName, outDirName);

            if(streamsBitmap.Count < 1)
            {
                Logger.Log($"No bitmap subtitles found to convert!");
                return;
            }

            Logger.Log($"Preparing to run OCR on subtitle streams {string.Join(", ", streamsBitmap.Select(x => $"#{x.Index + 1}"))}.");

            await OcrUtils.RunOcrOnStreams(inPath, streamsBitmap, outDir);

            if(streamsText.Count > 0)
                Logger.Log($"Won't run OCR on subtitle streams{(streamsText.Count == 1 ? "" : "s")} {string.Join(", ", streamsText.Select(x => $"#{x.Index + 1}"))} as they are text-based.");

            Program.mainForm.SetWorking(false);
        }

        //public static async Task RunOld()
        //{
        //    Program.mainForm.SetWorking(true);
        //    List<MediaStreamListEntry> checkedStreams = Program.mainForm.streamListBox.CheckedItems.OfType<MediaStreamListEntry>().ToList();
        //    List<SubtitleStream> subStreams = checkedStreams.Where(x => x.Stream.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x.Stream).ToList();
        //    int i = 0;
        //
        //    foreach (SubtitleStream ss in subStreams)
        //    {
        //        if (RunTask.canceled)
        //            continue;
        //
        //        if (!ss.Bitmap)
        //        {
        //            Logger.Log($"Won't run OCR on subtitle track '{ss}' as it's text-based.");
        //            continue;
        //        }
        //
        //        string inPath = TrackList.current.TruePath;
        //        string outDirName = Path.GetFileNameWithoutExtension(inPath).CleanString().Trunc(50, false) + "-Subtitles";
        //        string outDir = Path.Combine(new FileInfo(inPath).DirectoryName, outDirName);
        //        string outName = $"Subtitles-{ss.Index}-{ss.Title.CleanString()}-{ss.Language.CleanString()}";
        //        Directory.CreateDirectory(outDir);
        //        Logger.Log($"Subtitle Track {i + 1}/{subStreams.Count}: {ss}");
        //        bool exists = await OcrUtils.RunOcr(inPath, $"0:{ss.Index}", outDir, outName);
        //
        //        if (exists)
        //            Logger.Log($"Saved '{outName}' to folder '{outDirName}'.");
        //        else
        //            Logger.Log($"Failed to convert '{outName}'. Possibly the subtitle track is empty or broken.");
        //
        //        i++;
        //    }
        //
        //    Program.mainForm.SetWorking(false);
        //}
    }
}
