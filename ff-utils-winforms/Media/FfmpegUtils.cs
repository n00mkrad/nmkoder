using Nmkoder.Data;
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
            string output = await GetFfmpegInfoAsync(path, "Stream #0:");

            if (string.IsNullOrWhiteSpace(output.Trim()))
                return 0;

            return output.SplitIntoLines().Length;
        }

        public static async Task<List<Stream>> GetStreams(string path, bool progressBar, int streamCount)
        {
            List<Stream> streamList = new List<Stream>();

            try
            {
                string output = await GetFfmpegInfoAsync(path, "Stream #0:");
                string[] streams = output.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                foreach (string streamStr in streams)
                {
                    try
                    {
                        Logger.Log($"Found Stream: {streamStr}", true);
                        int idx = streamStr.Split(':')[1].Split('[')[0].Split('(')[0].GetInt();

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
                            Fraction fps = await IoUtils.GetVideoFramerate(path);
                            //int frames = await GetFrameCountCached.GetFrameCountAsync(path);
                            VideoStream vStream = new VideoStream(lang, title, codec, codecLong, pixFmt, kbits, res, sar, dar, fps);
                            vStream.Index = idx;
                            Logger.Log($"Added video stream to list: {vStream}", true);
                            streamList.Add(vStream);
                            continue;
                        }

                        if (streamStr.Contains(": Audio:"))
                        {
                            string lang = await GetFfprobeInfoAsync(path, showStreams, "TAG:language", idx);
                            string title = await GetFfprobeInfoAsync(path, showStreams, "TAG:title", idx);
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            int kbits = (await GetFfprobeInfoAsync(path, showStreams, "bit_rate", idx)).GetInt() / 1024;
                            int sampleRate = (await GetFfprobeInfoAsync(path, showStreams, "sample_rate", idx)).GetInt();
                            int channels = (await GetFfprobeInfoAsync(path, showStreams, "channels", idx)).GetInt();
                            string layout = (await GetFfprobeInfoAsync(path, showStreams, "channel_layout", idx));
                            AudioStream aStream = new AudioStream(lang, title, codec, codecLong, kbits, sampleRate, channels, layout);
                            aStream.Index = idx;
                            Logger.Log($"Added audio stream to list: {aStream}", true);
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
                            Logger.Log($"Added subtitle stream to list: {sStream}", true);
                            streamList.Add(sStream);
                            continue;
                        }

                        if (streamStr.Contains(": Data:"))
                        {
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            DataStream dStream = new DataStream(codec, codecLong);
                            dStream.Index = idx;
                            Logger.Log($"Added data stream to list: {dStream}", true);
                            streamList.Add(dStream);
                            continue;
                        }

                        if (streamStr.Contains(": Attachment:"))
                        {
                            string codec = await GetFfprobeInfoAsync(path, showStreams, "codec_name", idx);
                            string codecLong = await GetFfprobeInfoAsync(path, showStreams, "codec_long_name", idx);
                            AttachmentStream aStream = new AttachmentStream(codec, codecLong);
                            aStream.Index = idx;
                            Logger.Log($"Added attachment stream to list: {aStream}", true);
                            streamList.Add(aStream);
                            continue;
                        }

                        Stream stream = new Stream { Codec = "Unknown", CodecLong = "Unknown", Index = idx, Type = Stream.StreamType.Unknown };
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

            if (progressBar)
                Program.mainForm.SetProgress(0);

            return streamList;
        }

        public static async Task<bool> IsSubtitleBitmapBased(string path, int streamIndex, string codec = "")
        {
            if (codec == "ssa" || codec == "ass" || codec == "mov_text" || codec == "srt" || codec == "subrip" || codec == "text" || codec == "webvtt")
                return false;

            if (codec == "dvdsub" || codec == "pgssub" || codec == "hdmv_pgs_subtitle")
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

        public static async Task<string> GetCurrentAutoCrop(bool quiet)
        {
            string msg = "Detecting crop... This can take a while for long videos.";
            Logger.Log(msg, quiet);
            NmkdStopwatch sw = new NmkdStopwatch();
            int sampleCount = Config.GetInt(Config.Key.autoCropSamples, 8);
            string path = MediaInfo.current.Path;
            long duration = (int)Math.Floor((float)FfmpegCommands.GetDurationMs(path) / 1000);
            int interval = (int)Math.Floor((float)duration / sampleCount);
            List<string> detectedCrops = new List<string>();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < sampleCount; i++)
            {
                if (!quiet)
                    Program.mainForm.SetProgress(FormatUtils.RatioInt(i + 1, sampleCount));

                int t = interval * (i + 1) - (interval > 1 ? 1 : 0);

                string output = await GetFfmpegOutputAsync(path, $"-skip_frame nokey -ss {t}", "-an -sn -sn -vf cropdetect -vframes 3 -f null - 2>&1 1>nul | findstr crop=", "");

                foreach (string l in output.SplitIntoLines().Where(x => x.MatchesWildcard("*:*:*:*")))
                    detectedCrops.Add(l.Split(" crop=").Last());
            }

            var mostCommon = detectedCrops.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            string[] cropVals = mostCommon.Split(':');
            bool repl = Logger.GetLastLine().Contains(msg);
            Logger.Log($"Automatically detected crop: {cropVals[0]}x{cropVals[1]} (X = {cropVals[2]}, Y = {cropVals[3]}) (Took {sw})", quiet, !quiet && repl);
            return $"crop={mostCommon}";
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
