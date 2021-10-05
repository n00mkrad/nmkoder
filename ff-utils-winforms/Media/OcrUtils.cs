using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class OcrUtils
    {
        public static async Task RunOcr (string inPath, string map, string outDir, string trackName)
        {
            if (inPath.IsConcatFile())
                return; // TODO: Show error?

            string seDir = Path.Combine(Paths.GetBinPath(), "SE");
            string filename = "subs";
            string seInPath = Path.Combine(seDir, $"{filename}.sup");
            string seOutPath = Path.Combine(seDir, $"{filename}.srt");
            string args = $"-i {inPath.Wrap()} -map {map} -c copy {seInPath.Wrap()}";
            Logger.Log($"Extracting subtitles to convert them...");
            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.Hidden, AvProcess.TaskType.ExtractOther, false);
            Logger.Log($"Running OCR...");
            await OcrProcess.RunSubtitleEdit($"/convert {filename}.sup srt");
            string content = File.ReadAllText(seOutPath);
            File.WriteAllText(Path.Combine(outDir, trackName + ".srt"), content.Replace("|", "I"));
            IoUtils.TryDeleteIfExists(seOutPath);
        }
    }
}
