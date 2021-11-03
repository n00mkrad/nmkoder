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
            await AvProcess.RunFfmpeg(ffArgs, AvProcess.LogMode.Hidden);

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
                    Logger.Log($"Running {running} OCR Instances - Average Progress: {avgProg}%", false, Logger.GetLastLine().EndsWith("%"));
                    Program.mainForm.SetProgress(avgProg);

                    if (RunTask.canceled)
                        break;
                }

                await Task.Delay(100);
            }

            Logger.Log($"All OCR processes have finished.", false, Logger.GetLastLine().EndsWith("%"));

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

        //public static async Task<bool> RunOcr (string inPath, string map, string outDir, string trackName)
        //{
        //    try
        //    {
        //        progressTracker.Clear();
        //        Logger.Log($"RunOcr - Map: {map} - OutDir: {outDir} - TrackName: {trackName}", true);
        //
        //        if (inPath.IsConcatFile())
        //            return false; // TODO: Show error?
        //
        //        string seDir = Path.Combine(Paths.GetBinPath(), "SE");
        //        string seTempDir = Path.Combine(seDir, "temp");
        //
        //        string seInPath = Path.Combine(seDir, $"{trackName}.mkv");
        //        string seOutPath = Path.Combine(seDir, $"{trackName}.srt");
        //        Logger.Log($"Extracting subtitles to convert them...");
        //        MediaFile mediaFile = new MediaFile(inPath);
        //        await mediaFile.Initialize(true);
        //        string fpsArg = "";
        //
        //        try
        //        {
        //            fpsArg = $"/targetfps:{ mediaFile.VideoStreams[0].Rate.GetFloat().ToStringDot()}";
        //        }
        //        catch { }
        //
        //        await Split(inPath, map, mediaFile.DurationMs, seTempDir);
        //        NmkdStopwatch sw = new NmkdStopwatch();
        //
        //        await OcrProcess.RunSubtitleEdit($"/convert split/full.mkv srt /ocrengine:nOCR {fpsArg}", true, false);
        //
        //        if (IoUtils.GetFilesSorted(seTempDir, "full.*.srt").Count() < 1)
        //            return false;
        //
        //        string[] timecodes = File.ReadAllLines(IoUtils.GetFilesSorted(seTempDir, "full.*.srt").FirstOrDefault()).Where(x => x.Contains("-->")).ToArray();
        //
        //        foreach (FileInfo f in IoUtils.GetFileInfosSorted(seTempDir, false, "chunk*"))
        //            Task.Run(() => OcrProcess.RunSubtitleEdit($"/convert split/{f.Name} srt /ocrengine:tesseract {fpsArg}", true, true));
        //
        //        while (true)
        //        {
        //            if (progressTracker.Count > 0)
        //            {
        //                int progSum = progressTracker.Select(x => x.Value).Sum();
        //                int avgProg = ((float)progSum / progressTracker.Count).RoundToInt();
        //                int running = progressTracker.Where(x => x.Value < 100).Count();
        //                Logger.Log($"Running {running} OCR Instances - Progress: {avgProg}%", false, Logger.GetLastLine().EndsWith("%"));
        //                Program.mainForm.SetProgress(avgProg);
        //
        //                if (RunTask.canceled || progressTracker.All(x => x.Value == 100))
        //                    break;
        //            }
        //
        //            await Task.Delay(1000);
        //        }
        //
        //        double speed = ((double)mediaFile.DurationMs / sw.ElapsedMs);
        //        Logger.Log($"Finished OCR conversion in {sw} ({speed.ToString("0.0")}x Realtime).", false, Logger.GetLastLine().EndsWith("%"));
        //
        //        if (RunTask.canceled)
        //            return false;
        //
        //        string mergedFilePath = await Merge(seTempDir);
        //        string[] linesSrtSubs = File.ReadAllLines(mergedFilePath);
        //
        //        List<string> linesFinalSrt = new List<string>();
        //
        //        int timecodeIdx = 0;
        //
        //        for (int i = 0; i < linesSrtSubs.Length; i++)
        //        {
        //            if (linesSrtSubs[i].Contains("-->")) // If line is a timecode line,
        //            {
        //                linesFinalSrt.Add(timecodes[timecodeIdx]); // Use accurate timecode from un-split file
        //                timecodeIdx++;
        //            }
        //            else
        //            {
        //                linesFinalSrt.Add(linesSrtSubs[i].Replace('|', 'I'));
        //            }
        //        }
        //
        //        string outPath = Path.Combine(outDir, trackName + ".srt");
        //        File.WriteAllLines(outPath, linesFinalSrt);
        //        IoUtils.DeleteContentsOfDir(seTempDir);
        //        return File.Exists(outPath);
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Log($"Error occured during subtitle conversion: {e.Message}\n{e.StackTrace}");
        //        return false;
        //    }
        //}
        //
        //private static async Task Extract(string inPath, string map, long durationMs, string outDir)
        //{
        //    try
        //    {
        //        IoUtils.DeleteContentsOfDir(outDir);
        //        int secs = (int)Math.Ceiling((double)durationMs / 1000);
        //        int splitTime = (int)Math.Ceiling(((double)secs / (Environment.ProcessorCount - 1).Clamp(1, 24)) * 1);
        //        string argsFull = $"-i {inPath.Wrap()} -map {map} -c copy \"{outDir}/full.mkv\"";
        //        await AvProcess.RunFfmpeg(argsFull, AvProcess.LogMode.Hidden);
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Log($"Failed to extract subtitles: {e.Message}");
        //    }
        //}
        //
        //private static async Task Split(string inPath, string map, long durationMs, string outDir)
        //{
        //    try
        //    {
        //        IoUtils.DeleteContentsOfDir(outDir);
        //        int secs = (int)Math.Ceiling((double)durationMs / 1000);
        //        int splitTime = (int)Math.Ceiling(((double)secs / (Environment.ProcessorCount - 1).Clamp(1, 24)) * 1);
        //        string argsFull = $"-i {inPath.Wrap()} -map {map} -c copy \"{outDir}/full.mkv\""; // First, copy full un-split file to get accuratet timecodes (splitting can break them)
        //        await AvProcess.RunFfmpeg(argsFull, AvProcess.LogMode.Hidden);
        //        string argsChunks = $"-i {inPath.Wrap()} -f segment -segment_time {splitTime} -reset_timestamps 1 -map {map} -c copy \"{outDir}/chunk%4d.mkv\""; // Make chunks for multithreaded OCR
        //        await AvProcess.RunFfmpeg(argsChunks, AvProcess.LogMode.Hidden);
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Log($"Failed to split subtitles into chunks: {e.Message}");
        //    }
        //}
        //
        //private static async Task<string> Merge(string dir)
        //{
        //    try
        //    {
        //        FileInfo[] chunks = IoUtils.GetFileInfosSorted(dir, false, "chunk*.srt");
        //        string concatText = string.Join("\n", chunks.Select(x => $"file '{x.Name}'"));
        //        string concatFilePath = Path.Combine(dir, "concat.txt");
        //        File.WriteAllText(concatFilePath, concatText);
        //        string outFilePath = Path.Combine(dir, "merged.srt");
        //        string args = $"-safe 0 -f concat -i {concatFilePath.Wrap()} -map 0 -c copy {outFilePath.Wrap()}";
        //        await AvProcess.RunFfmpeg(args, AvProcess.LogMode.Hidden);
        //        return outFilePath;
        //    }
        //    catch(Exception e)
        //    {
        //        Logger.Log($"Failed to merge subtitle chunks: {e.Message}");
        //        return "";
        //    }
        //}
    }
}
