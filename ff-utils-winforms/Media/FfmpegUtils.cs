﻿using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nmkoder.Media.GetVideoInfo;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.Media
{
    class FfmpegUtils
    {
        private readonly static FfprobeMode showStreams = FfprobeMode.ShowStreams;
        private readonly static FfprobeMode showFormat = FfprobeMode.ShowFormat;

        public static async Task<int> GetStreamCount(string path)
        {
            Logger.Log($"GetStreamCount({path})", true);
            string output = await GetFfmpegInfoAsync(path, "Stream #0:");

            if (string.IsNullOrWhiteSpace(output.Trim()))
                return 0;

            return output.SplitIntoLines().Where(x => x.MatchesWildcard("*Stream #0:*: *: *")).Count();
        }

        public static async Task<List<Stream>> GetStreams(string path, bool progressBar, int streamCount, Fraction defaultFps)
        {
            List<Stream> streamList = new List<Stream>();

            try
            {
                string output = await GetFfmpegInfoAsync(path, "Stream #0:");
                string[] streams = output.SplitIntoLines().Where(x => x.MatchesWildcard("*Stream #0:*: *: *")).ToArray();

                foreach (string streamStr in streams)
                {
                    try
                    {
                        int idx = streamStr.Split(':')[1].Split('[')[0].Split('(')[0].GetInt();
                        bool def = await GetFfprobeInfoAsync(path, showStreams, "DISPOSITION:default", idx) == "1";

                        if (progressBar)
                            Program.mainForm.SetProgress(FormatUtils.RatioInt(idx + 1, streamCount));

                        if (streamStr.Contains(": Video:"))
                        {
                            string lang = await GetFfprobeInfoAsync(path, showStreams, "TAG:language", idx);
                            string title = await GetFfprobeInfoAsync(path, showStreams, "TAG:title", idx);
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            string pixFmt = (await GetFfprobeInfoAsync(path, showStreams, "pix_fmt", idx)).ToUpper();
                            int kbits = (await GetFfprobeInfoAsync(path, showStreams, "bit_rate", idx)).GetInt() / 1024;
                            Size res = await GetMediaResolutionCached.GetSizeAsync(path);
                            Size sar = SizeFromString(await GetFfprobeInfoAsync(path, showStreams, "sample_aspect_ratio", idx));
                            Size dar = SizeFromString(await GetFfprobeInfoAsync(path, showStreams, "display_aspect_ratio", idx));
                            Fraction fps = path.IsConcatFile() ? defaultFps : await IoUtils.GetVideoFramerate(path);
                            VideoStream vStream = new VideoStream(lang, title, codec, codecLong, pixFmt, kbits, res, sar, dar, fps);
                            vStream.Index = idx;
                            vStream.IsDefault = def;
                            Logger.Log($"Added video stream: {vStream}", true);
                            streamList.Add(vStream);
                            continue;
                        }

                        if (streamStr.Contains(": Audio:"))
                        {
                            string lang = await GetFfprobeInfoAsync(path, showStreams, "TAG:language", idx);
                            string title = await GetFfprobeInfoAsync(path, showStreams, "TAG:title", idx);
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string profile = await GetFfprobeInfoAsync(path, showStreams, "profile", idx);
                            if (codec.ToLower() == "dts" && profile != "unknown") codec = profile;
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            int kbits = (await GetFfprobeInfoAsync(path, showStreams, "bit_rate", idx)).GetInt() / 1024;
                            int sampleRate = (await GetFfprobeInfoAsync(path, showStreams, "sample_rate", idx)).GetInt();
                            int channels = (await GetFfprobeInfoAsync(path, showStreams, "channels", idx)).GetInt();
                            string layout = (await GetFfprobeInfoAsync(path, showStreams, "channel_layout", idx));
                            AudioStream aStream = new AudioStream(lang, title, codec, codecLong, kbits, sampleRate, channels, layout);
                            aStream.Index = idx;
                            aStream.IsDefault = def;
                            Logger.Log($"Added audio stream: {aStream}", true);
                            streamList.Add(aStream);
                            continue;
                        }

                        if (streamStr.Contains(": Subtitle:"))
                        {
                            string lang = await GetFfprobeInfoAsync(path, showStreams, "TAG:language", idx);
                            string title = await GetFfprobeInfoAsync(path, showStreams, "TAG:title", idx);
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            bool bitmap = await IsSubtitleBitmapBased(path, idx, codec);
                            SubtitleStream sStream = new SubtitleStream(lang, title, codec, codecLong, bitmap);
                            sStream.Index = idx;
                            sStream.IsDefault = def;
                            Logger.Log($"Added subtitle stream: {sStream}", true);
                            streamList.Add(sStream);
                            continue;
                        }

                        if (streamStr.Contains(": Data:"))
                        {
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            DataStream dStream = new DataStream(codec, codecLong);
                            dStream.Index = idx;
                            dStream.IsDefault = def;
                            Logger.Log($"Added data stream: {dStream}", true);
                            streamList.Add(dStream);
                            continue;
                        }

                        if (streamStr.Contains(": Attachment:"))
                        {
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            string filename = await GetFfprobeInfoAsync(path, showStreams, "TAG:filename", idx);
                            string mimeType = await GetFfprobeInfoAsync(path, showStreams, "TAG:mimetype", idx);
                            AttachmentStream aStream = new AttachmentStream(codec, codecLong, filename, mimeType);
                            aStream.Index = idx;
                            aStream.IsDefault = def;
                            Logger.Log($"Added attachment stream: {aStream}", true);
                            streamList.Add(aStream);
                            continue;
                        }

                        Logger.Log($"Unknown stream (not vid/aud/sub/dat/att): {streamStr}", true);
                        Stream stream = new Stream { Codec = "Unknown", CodecLong = "Unknown", Index = idx, IsDefault = def, Type = Stream.StreamType.Unknown };
                        streamList.Add(stream);
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Error scanning stream: {e.Message}\n{e.StackTrace}");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"GetStreams Exception: {e.Message}\n{e.StackTrace}", true);
            }

            Logger.Log($"Video Streams: {string.Join(", ", streamList.Where(x => x.Type == Stream.StreamType.Video).Select(x => string.IsNullOrWhiteSpace(x.Title) ? "No Title" : x.Title))}", true);
            Logger.Log($"Audio Streams: {string.Join(", ", streamList.Where(x => x.Type == Stream.StreamType.Audio).Select(x => string.IsNullOrWhiteSpace(x.Title) ? "No Title" : x.Title))}", true);
            Logger.Log($"Subtitle Streams: {string.Join(", ", streamList.Where(x => x.Type == Stream.StreamType.Subtitle).Select(x => string.IsNullOrWhiteSpace(x.Title) ? "No Title" : x.Title))}", true);

            if (progressBar)
                Program.mainForm.SetProgress(0);

            return streamList;
        }

        public static async Task<bool> IsSubtitleBitmapBased(string path, int streamIndex, string codec = "")
        {
            if (codec == "ssa" || codec == "ass" || codec == "mov_text" || codec == "srt" || codec == "subrip" || codec == "text" || codec == "webvtt")
                return false;

            if (codec == "dvdsub" || codec == "dvd_subtitle" || codec == "pgssub" || codec == "hdmv_pgs_subtitle" || codec.StartsWith("dvb_"))
                return true;

            // If codec was not listed above, manually check if it's compatible by trying to encode it:
            //string ffmpegCheck = await GetFfmpegOutputAsync(path, $"-map 0:{streamIndex} -c:s srt -t 0 -f null -");
            //return ffmpegCheck.Contains($"encoding currently only possible from text to text or bitmap to bitmap");

            return false;
        }

        public static string GetPadFilter(int px = 2)
        {
            return $"pad=width=ceil(iw/{px})*{px}:height=ceil(ih/{px})*{px}:color=black@0";
        }

        public static async Task<string> GetCurrentAutoCrop(string path, bool quiet)
        {
            string msg = "Detecting crop... This can take a while for long videos.";
            Logger.Log(msg, quiet);
            NmkdStopwatch sw = new NmkdStopwatch();
            int sampleCount = Config.GetInt(Config.Key.AutoCropSamples, 10);
            long duration = (int)Math.Floor((float)(await FfmpegCommands.GetDurationMs(path)) / 1000);
            int interval = (int)Math.Floor((float)duration / sampleCount);
            List<string> detectedCrops = new List<string>();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < sampleCount; i++)
            {
                if (!quiet)
                    Program.mainForm.SetProgress(FormatUtils.RatioInt(i + 1, sampleCount));

                int t = interval * (i + 1) - (interval > 1 ? 1 : 0);

                string output = await GetFfmpegOutputAsync(path, $"-r 1 -ss {t}", "-an -sn -sn -vf cropdetect=round=2 -vframes 6 -r 1/10 -f null - 2>&1 1>nul | findstr crop=", "", false, OS.NmkoderProcess.ProcessType.Secondary);

                foreach (string l in output.SplitIntoLines().Where(x => x.MatchesWildcard("*:*:*:*")))
                    detectedCrops.Add(l.Split(" crop=").Last());
            }

            if(detectedCrops.Count < 1)
            {
                Logger.Log($"Couldn't detect crop - The video might be too short for automatic crop detection.");
                return "";
            }

            detectedCrops = detectedCrops.Where(x => (x.Split(':')[0].GetInt() > 0) && (x.Split(':')[1].GetInt() > 0)).OrderByDescending(x => (x.Split(':')[0].GetInt() * x.Split(':')[1].GetInt())).ToList();

            if (detectedCrops.Count < 1)
                return "";

            string mostCommon = detectedCrops.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            string largest = detectedCrops.First();
            int commonCertainty = (detectedCrops.CountOccurences(mostCommon) / detectedCrops.Count * 100f).RoundToInt();
            string chosen = commonCertainty > 80 ? mostCommon : largest; // Use most common if it's >80% common, otherwise use largest to be safe (thanks Nolan)
            Logger.Log($"GetCurrentAutoCrop - Largest: {largest} - Smallest: {detectedCrops.Last()} - Most Common: {mostCommon} ({commonCertainty}%) - Chosen: {chosen} [T = {sw}]", true);
            string[] cropVals = chosen.Split(':');
            bool repl = Logger.LastUiLine.Contains(msg);
            Logger.Log($"Automatically detected crop: {cropVals[0]}x{cropVals[1]} (X = {cropVals[2]}, Y = {cropVals[3]})", quiet, !quiet && repl);

            return $"crop={chosen}";
        }

        public struct StreamSizeInfo { public float Kbps; public long Bytes; }

        public static async Task<StreamSizeInfo> GetStreamSizeBytes(string path, int streamIndex = 0)
        {
            try
            {
                string decodeOutput = await GetFfmpegOutputAsync(path, $"-map 0:{streamIndex} -c copy -f matroska NUL 2>&1 1>nul | findstr /L \"time video\"", "", true);
                string[] outputLines = decodeOutput.SplitIntoLines();
                string sizeLine = outputLines.Where(l => l.Contains("size=") && !l.Contains("size=N/A")).Last();
                string bitrateLine = outputLines.Where(l => l.Contains("bitrate=") && !l.Contains("bitrate=N/A")).Last();
                long bytes = sizeLine.Split("size= ")[1].Split("kB")[0].GetInt();
                float bitrate = bitrateLine.Split("bitrate=")[1].Split("kbits/s")[0].GetFloat();
                return new StreamSizeInfo() { Bytes = bytes * 1024, Kbps = bitrate };
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to get stream size/bitrate! {ex.Message}\n{ex.StackTrace}", true);
                return new StreamSizeInfo() { Bytes = 0, Kbps = 0 };
            }
        }

        public static int CreateConcatFile(string inputFilesDir, string outputPath, List<string> validExtensions = null)
        {
            if (IoUtils.GetAmountOfFiles(inputFilesDir, false) < 1)
                return 0;
            
            Directory.CreateDirectory(outputPath.GetParentDir());

            if(validExtensions == null)
                validExtensions = new List<string>();

            validExtensions = validExtensions.Select(x => x.Remove(".").ToLower()).ToList(); // Ignore "." in extensions
            string concatFileContent = "";
            string[] files = IoUtils.GetFilesSorted(inputFilesDir);
            int fileCount = 0;

            foreach (string file in files.Where(x => validExtensions.Contains(Path.GetExtension(x).Replace(".", "").ToLower())))
            {
                fileCount++;
                concatFileContent += $"file '{file.Replace(@"\", "/")}'\n";
            }

            File.WriteAllText(outputPath, concatFileContent);
            return fileCount;
        }

        public static Size SizeFromString(string str, char delimiter = ':')
        {
            try
            {
                string[] nums = str.Remove(" ").Trim().Split(delimiter);
                return new Size(nums[0].GetInt(), nums[1].GetInt());
            }
            catch
            {
                return new Size();
            }
        }
    }
}
