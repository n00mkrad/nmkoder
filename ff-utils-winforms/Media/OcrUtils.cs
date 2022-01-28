using Newtonsoft.Json;
using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.UI;
using Nmkoder.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class OcrUtils
    {
        public static ConcurrentDictionary<string, int> progressTracker = new ConcurrentDictionary<string, int>();
        public static int procsStarted = 0;
        public static int procsFinished = 0;

        public static async Task<bool> RunOcrOnStreams(string inPath, List<SubtitleStream> streams, string outDir)
        {
            string tempDir = Path.Combine(Paths.GetSessionDataPath(), "subs-temp");
            Directory.CreateDirectory(tempDir);
            IoUtils.DeleteContentsOfDir(tempDir);
            Logger.Log($"Muxing subs from input to all-subs.mkv", true, false, "ocr");
            string ffArgs = $"-i {inPath.Wrap()} -map 0:s -c copy \"{tempDir}/subs.mkv\"";
            AvProcess.FfmpegSettings settings = new AvProcess.FfmpegSettings() { Args = ffArgs, LoggingMode = AvProcess.LogMode.Hidden };
            await AvProcess.RunFfmpeg(settings);

            Logger.Log($"starting {streams.Count} ocr tasks", true, false, "ocr");

            procsStarted = streams.Count;
            procsFinished = 0;

            for (int i = 0; i < streams.Count; i++)
            {
                int iCopy = i;
                int subStreamIdx = TrackList.current.File.SubtitleStreams.IndexOf(streams[iCopy]);
                string srtDir = Path.Combine(tempDir, $"{subStreamIdx}");
                Directory.CreateDirectory(srtDir);
                Task.Run(() => RunOcrOnSingleStream(tempDir, outDir, streams[iCopy], subStreamIdx));
            }

            while (procsFinished < procsStarted)
            {
                if (progressTracker != null && progressTracker.Count > 0)
                {
                    int progSum = progressTracker.Select(x => x.Value).Sum();
                    int avgProg = ((float)progSum / progressTracker.Count).RoundToInt();
                    int running = progressTracker.Where(x => x.Value < 100).Count();
                    Logger.Log($"Running {running} OCR Instances - Average Progress: {avgProg}%", false, Logger.LastUiLine.EndsWith("%"));
                    Program.mainForm.SetProgress(avgProg);

                    if (RunTask.canceled)
                        break;
                }

                await Task.Delay(100);
            }

            Logger.Log($"All OCR processes have finished.", false, Logger.LastUiLine.EndsWith("%"));

            return true;
        }

        public static async Task<bool> RunOcrOnSingleStream(string subTempDir, string finalOutDir, SubtitleStream ss, int subIndex)
        {
            if (IoUtils.GetAmountOfFiles(subTempDir, false, "*.mkv") < 1)
                return false;

            Logger.Log($"[Subtitle Stream {subIndex}] RunOcrOnSingleStream: Running OCR for subtitle stream", true, false, "ocr");
            string outPath = Path.Combine(finalOutDir, GetSrtName(subIndex, ss) + ".srt");
            string srtPath = Path.Combine(subTempDir, subIndex.ToString());

            string templateName = $"{ss.Language.Trim().ToLower()}.template";
            string replArg = File.Exists(Path.Combine(OcrProcess.GetDir(), templateName)) ? $"/multiplereplace:{templateName}" : "";

            await OcrProcess.RunSubtitleEdit($"/convert {Path.Combine(subTempDir, "subs.mkv")} srt /ocrengine:tesseract /track-number:{subIndex + 1} /outputfolder:{srtPath.Wrap()} {replArg}", true, true);
            Logger.Log($"[Subtitle Stream {subIndex}] RunOcrOnSingleStream: Fininshed OCR.", true, false, "ocr");
            Directory.CreateDirectory(finalOutDir);

            FileInfo[] srts = IoUtils.GetFileInfosSorted(srtPath, false, "*.srt");

            if (srts.Length < 1)
                Logger.Log($"Warning: OCR for {GetSrtName(subIndex, ss)} did not produce an output file - Possibly the track is empty.");
            else
                PostprocessAndMove(srts[0], subIndex, ss, outPath);

            return true;
        }

        static string GetSrtName(int subIndex, SubtitleStream ss)
        {
            string title = ss.Title.CleanString().Trunc(30, false).Trim();
            string lang = ss.Language.CleanString().ToUpper().Trunc(3, false).Trim();
            return $"SubtitleTrack{subIndex}-Index{ss.Index}{(title.Length > 0 ? $"-{title}" : "")}{(lang.Length > 0 ? $"-{lang}" : "")}";
        }

        static void PostprocessAndMove(FileInfo file, int subIndex, SubtitleStream ss, string outPath)
        {
            string templateName = $"{ss.Language.Trim().ToLower()}.repl.json";
            string templateFile = Path.Combine(OcrProcess.GetDir(), templateName);

            if (File.Exists(templateFile))
            {
                Logger.Log($"[Subtitle Stream {subIndex}] PostprocessAndMove: There is a multi-replace file, applying it.", true, false, "ocr");
                Dictionary<string, string> findAndReplacePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(templateFile));

                string srtText = File.ReadAllText(file.FullName);

                foreach (KeyValuePair<string, string> pair in findAndReplacePairs)
                    srtText = srtText.Replace(pair.Key, pair.Value);

                File.WriteAllText(outPath, srtText);
            }
            else
            {
                file.MoveTo(outPath);
            }
        }
    }
}
