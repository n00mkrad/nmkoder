using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
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

        public static async Task<bool> RunOcr (string inPath, string map, string outDir, string trackName)
        {
            try
            {
                progressTracker.Clear();
                Logger.Log($"RunOcr - Map: {map} - OutDir: {outDir} - TrackName: {trackName}", true);

                if (inPath.IsConcatFile())
                    return false; // TODO: Show error?

                string seDir = Path.Combine(Paths.GetBinPath(), "SE");
                string seTempDir = Path.Combine(seDir, "temp");

                string seInPath = Path.Combine(seDir, $"{trackName}.mkv");
                string seOutPath = Path.Combine(seDir, $"{trackName}.srt");
                Logger.Log($"Extracting subtitles to convert them...");
                MediaFile mediaFile = new MediaFile(inPath);
                await mediaFile.Initialize(true);
                string fpsArg = "";

                try
                {
                    fpsArg = $"/targetfps:{ mediaFile.VideoStreams[0].Rate.GetFloat().ToStringDot()}";
                }
                catch { }

                await Split(inPath, map, mediaFile.DurationMs, seTempDir);
                NmkdStopwatch sw = new NmkdStopwatch();

                await OcrProcess.RunSubtitleEdit($"/convert split/full.mkv srt /ocrengine:nOCR {fpsArg}", true, false);

                if (IoUtils.GetFilesSorted(seTempDir, "full.*.srt").Count() < 1)
                    return false;

                string[] timecodes = File.ReadAllLines(IoUtils.GetFilesSorted(seTempDir, "full.*.srt").FirstOrDefault()).Where(x => x.Contains("-->")).ToArray();

                foreach (FileInfo f in IoUtils.GetFileInfosSorted(seTempDir, false, "chunk*"))
                    Task.Run(() => OcrProcess.RunSubtitleEdit($"/convert split/{f.Name} srt /ocrengine:tesseract {fpsArg}", true, true));

                while (true)
                {
                    if (progressTracker.Count > 0)
                    {
                        int progSum = progressTracker.Select(x => x.Value).Sum();
                        int avgProg = ((float)progSum / progressTracker.Count).RoundToInt();
                        int running = progressTracker.Where(x => x.Value < 100).Count();
                        Logger.Log($"Running {running} OCR Instances - Progress: {avgProg}%", false, Logger.GetLastLine().EndsWith("%"));
                        Program.mainForm.SetProgress(avgProg);

                        if (RunTask.canceled || progressTracker.All(x => x.Value == 100))
                            break;
                    }

                    await Task.Delay(1000);
                }

                double speed = ((double)mediaFile.DurationMs / sw.ElapsedMs);
                Logger.Log($"Finished OCR conversion in {sw} ({speed.ToString("0.0")}x Realtime).", false, Logger.GetLastLine().EndsWith("%"));

                if (RunTask.canceled)
                    return false;

                string mergedFilePath = await Merge(seTempDir);
                string[] linesSrtSubs = File.ReadAllLines(mergedFilePath);

                List<string> linesFinalSrt = new List<string>();

                int timecodeIdx = 0;

                for (int i = 0; i < linesSrtSubs.Length; i++)
                {
                    if (linesSrtSubs[i].Contains("-->")) // If line is a timecode line,
                    {
                        linesFinalSrt.Add(timecodes[timecodeIdx]); // Use accurate timecode from un-split file
                        timecodeIdx++;
                    }
                    else
                    {
                        linesFinalSrt.Add(linesSrtSubs[i].Replace('|', 'I'));
                    }
                }

                string outPath = Path.Combine(outDir, trackName + ".srt");
                File.WriteAllLines(outPath, linesFinalSrt);
                IoUtils.DeleteContentsOfDir(seTempDir);
                return File.Exists(outPath);
            }
            catch (Exception e)
            {
                Logger.Log($"Error occured during subtitle conversion: {e.Message}\n{e.StackTrace}");
                return false;
            }
        }

        private static async Task Extract(string inPath, string map, long durationMs, string outDir)
        {
            try
            {
                IoUtils.DeleteContentsOfDir(outDir);
                int secs = (int)Math.Ceiling((double)durationMs / 1000);
                int splitTime = (int)Math.Ceiling(((double)secs / (Environment.ProcessorCount - 1).Clamp(1, 24)) * 1);
                string argsFull = $"-i {inPath.Wrap()} -map {map} -c copy \"{outDir}/full.mkv\"";
                await AvProcess.RunFfmpeg(argsFull, AvProcess.LogMode.Hidden);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to extract subtitles: {e.Message}");
            }
        }

        private static async Task Split(string inPath, string map, long durationMs, string outDir)
        {
            try
            {
                IoUtils.DeleteContentsOfDir(outDir);
                int secs = (int)Math.Ceiling((double)durationMs / 1000);
                int splitTime = (int)Math.Ceiling(((double)secs / (Environment.ProcessorCount - 1).Clamp(1, 24)) * 1);
                string argsFull = $"-i {inPath.Wrap()} -map {map} -c copy \"{outDir}/full.mkv\""; // First, copy full un-split file to get accuratet timecodes (splitting can break them)
                await AvProcess.RunFfmpeg(argsFull, AvProcess.LogMode.Hidden);
                string argsChunks = $"-i {inPath.Wrap()} -f segment -segment_time {splitTime} -reset_timestamps 1 -map {map} -c copy \"{outDir}/chunk%4d.mkv\""; // Make chunks for multithreaded OCR
                await AvProcess.RunFfmpeg(argsChunks, AvProcess.LogMode.Hidden);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to split subtitles into chunks: {e.Message}");
            }
        }

        private static async Task<string> Merge(string dir)
        {
            try
            {
                FileInfo[] chunks = IoUtils.GetFileInfosSorted(dir, false, "chunk*.srt");
                string concatText = string.Join("\n", chunks.Select(x => $"file '{x.Name}'"));
                string concatFilePath = Path.Combine(dir, "concat.txt");
                File.WriteAllText(concatFilePath, concatText);
                string outFilePath = Path.Combine(dir, "merged.srt");
                string args = $"-safe 0 -f concat -i {concatFilePath.Wrap()} -map 0 -c copy {outFilePath.Wrap()}";
                await AvProcess.RunFfmpeg(args, AvProcess.LogMode.Hidden);
                return outFilePath;
            }
            catch(Exception e)
            {
                Logger.Log($"Failed to merge subtitle chunks: {e.Message}");
                return "";
            }
        }
    }
}
