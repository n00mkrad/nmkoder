using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            List<StreamListEntry> checkedStreams = Program.mainForm.streamList.CheckedItems.Cast<ListViewItem>().Select(x => (StreamListEntry)x.Tag).ToList();
            List<SubtitleStream> subStreams = checkedStreams.Where(x => x.Stream.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x.Stream).ToList();
            int i = 0;

            List<SubtitleStream> streamsText = subStreams.Where(x => !x.Bitmap).ToList();
            List<SubtitleStream> streamsBitmap = subStreams.Where(x => x.Bitmap).ToList();

            string inPath = TrackList.current.File.ImportPath;
            string outDirName = Path.GetFileNameWithoutExtension(inPath).CleanString().Trunc(50, false) + "-Subtitles";
            string outDir = Path.Combine(new FileInfo(inPath).DirectoryName, outDirName);

            if(!Directory.Exists(OcrProcess.GetDir()) || IoUtils.GetAmountOfFiles(OcrProcess.GetDir(), true, "*.exe") < 1)
            {
                Logger.Log($"OCR binaries not found! Did you download a build without OCR?");
                return;
            }

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
    }
}
