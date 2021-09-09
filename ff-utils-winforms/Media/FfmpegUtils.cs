using Nmkoder.Data;
using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class FfmpegUtils
    {
        public static async Task<int> GetStreamCount(string path)
        {
            string output = await GetVideoInfoCached.GetFfmpegInfoAsync(path, "Stream #0:");
            return output.SplitIntoLines().Length;
        }

        public static async Task<List<Stream>> GetStreams (string path, bool progressBar, int streamCount)
        {
            List<Stream> streamList = new List<Stream>();

            try
            {
                string output = await GetVideoInfoCached.GetFfmpegInfoAsync(path, "Stream #0:");
                string[] streams = output.SplitIntoLines();

                foreach (string streamStr in streams)
                {
                    Logger.Log($"Found Stream: {streamStr}", true);
                    int idx = streamStr.Split(':')[1].GetInt();

                    if (progressBar)
                        Program.mainForm.SetProgress(FormatUtils.RatioInt(idx + 1, streamCount));

                    if (streamStr.Contains(": Video:"))
                    {
                        string codec = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "codec_name", idx);
                        string codecLong = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "codec_long_name", idx);
                        string pixFmt = (await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "pix_fmt", idx)).ToUpper();
                        int kbits = (await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "bit_rate", idx)).GetInt() / 1024;
                        Size res = await GetMediaResolutionCached.GetSizeAsync(path);
                        Size sar = SizeFromString(await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "sample_aspect_ratio", idx));
                        Size dar = SizeFromString(await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "display_aspect_ratio", idx));
                        Fraction fps = await IoUtils.GetVideoFramerate(path);
                        //int frames = await GetFrameCountCached.GetFrameCountAsync(path);
                        VideoStream vStream = new VideoStream(codec, codecLong, pixFmt, kbits, res, sar, dar, fps);
                        vStream.Index = idx;
                        streamList.Add(vStream);
                        continue;
                    }

                    if (streamStr.Contains(": Audio:"))
                    {
                        string title = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "TAG:title", idx);
                        string codec = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "codec_name", idx);
                        string codecLong = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "codec_long_name", idx);
                        int kbits = (await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "bit_rate", idx)).GetInt() / 1024;
                        int sampleRate = (await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "sample_rate", idx)).GetInt();
                        int channels = (await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "channels", idx)).GetInt();
                        string layout = (await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "channel_layout", idx));
                        AudioStream aStream = new AudioStream(title, codec, codecLong, kbits, sampleRate, channels, layout);
                        aStream.Index = idx;
                        streamList.Add(aStream);
                        continue;
                    }

                    if (streamStr.Contains(": Subtitle:"))
                    {
                        string lang = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "TAG:language", idx);
                        string title = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "TAG:title", idx);
                        string codec = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "codec_name", idx);
                        string codecLong = await GetVideoInfoCached.GetFfprobeInfoAsync(path, GetVideoInfoCached.FfprobeMode.ShowStreams, "codec_long_name", idx);
                        SubtitleStream sStream = new SubtitleStream(lang, title, codec, codecLong);
                        sStream.Index = idx;
                        streamList.Add(sStream);
                        continue;
                    }

                    Stream stream = new Stream();
                    stream.Type = Stream.StreamType.Unknown;
                    streamList.Add(stream);
                }
            }
            catch(Exception e)
            {
                Logger.Log($"GetStreams Exception: {e.Message}\n{e.StackTrace}", true);
            }

            if (progressBar)
                Program.mainForm.SetProgress(0);

            return streamList;
        }

        public static Size SizeFromString (string str, char delimiter = ':')
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
