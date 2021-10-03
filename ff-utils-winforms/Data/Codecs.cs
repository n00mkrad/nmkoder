using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class Codecs
    {
        //public enum CodecType { Video, AnimImage, Image, Audio }

        public enum Av1anCodec { AomAv1, SvtAv1, VpxVp9, X265 };
        public enum VideoCodec { Copy, StripVideo, H264, H265, H264Nvenc, H265Nvenc, Vp9, Av1, Gif, Png, Jpg };
        public enum AudioCodec { Copy, StripAudio, Aac, Opus, Mp3, Flac };
        public enum SubtitleCodec { Copy, StripSubs, MovText, Srt, WebVtt };

        public static CodecArgs GetArgs(VideoCodec c, Dictionary<string, string> encArgs, MediaFile mediaFile = null)
        {
            CodecInfo info = GetCodecInfo(c);

            if (c == VideoCodec.StripVideo)
            {
                return new CodecArgs($"-vn");
            }

            if (c == VideoCodec.Copy)
            {
                return new CodecArgs($"-c:v copy");
            }

            if (c == VideoCodec.H264)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return new CodecArgs($"-c:v libx264 -crf {q} -preset {preset} -pix_fmt {pixFmt}");
            }

            if (c == VideoCodec.H265)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return new CodecArgs($"-c:v libx265 -crf {q} -preset {preset} -pix_fmt {pixFmt}");
            }

            if (c == VideoCodec.H264Nvenc)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return new CodecArgs($"-c:v h264_nvenc -b:v 0 {(q.GetInt() > 0 ? $"-cq {q}" : "-tune lossless")} -preset {preset} -pix_fmt {pixFmt}");
            }

            if (c == VideoCodec.H265Nvenc)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return new CodecArgs($"-c:v hevc_nvenc -b:v 0 {(q.GetInt() > 0 ? $"-cq {q}" : "-tune lossless")} -preset {preset} -pix_fmt {pixFmt}");
            }

            if (c == VideoCodec.Vp9)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return new CodecArgs($"-c:v libvpx-vp9 -crf {q} -tile-columns 2 -tile-rows 2 -row-mt 1 -cpu-used {preset} -pix_fmt {pixFmt}");
            }

            if (c == VideoCodec.Av1)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                string g = GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.av1KeyIntSecs, 8));
                return new CodecArgs($"-c:v libsvtav1 -qp {q} -tile_columns 2 -tile_rows 1 -preset {preset} {g} -pix_fmt {pixFmt}");
            }

            if (c == VideoCodec.Gif)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                return new CodecArgs($"-f gif -gifflags -offsetting", $"split[s0][s1];[s0]palettegen={q}[p];[s1][p]paletteuse=dither=floyd_steinberg");
            }

            if (c == VideoCodec.Png)
            {
                return new CodecArgs($"-c:v png -compression_level 3");
            }

            if (c == VideoCodec.Jpg)
            {
                string q = encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                return new CodecArgs($"-c:v mjpeg -qmin 1 -q:v {q}");
            }

            return new CodecArgs();
        }

        public static CodecArgs GetArgs(Av1anCodec c, Dictionary<string, string> encArgs, bool vmaf, string custom = "", MediaFile mediaFile = null)
        {
            CodecInfo info = GetCodecInfo(c);
            string g = GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.av1KeyIntSecs, 10), "");

            if (c == Av1anCodec.AomAv1)
            {
                string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
                string denoise = encArgs.ContainsKey("grainSynthDenoise") ? (encArgs["grainSynthDenoise"].GetBool() ? "1" : "0") : "0";
                return new CodecArgs($" -e aom -v \" --end-usage=q --cpu-used={preset} --cq-level={q} --kf-max-dist={g} --threads=4 --enable-dnl-denoising={denoise} --denoise-noise-level={grain}  {custom} \" --pix-format {pixFmt}");
            }

            if (c == Av1anCodec.SvtAv1)
            {
                string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
                return new CodecArgs($" -e svt-av1 --force -v \" --preset {preset} --crf {q} --keyint {g} --film-grain {grain} {custom} \" --pix-format {pixFmt}");
            }

            if (c == Av1anCodec.VpxVp9)
            {
                string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                bool is420 = pixFmt.Contains("444") || pixFmt.Contains("422");
                int b = pixFmt.Split('p').LastOrDefault().GetInt();
                int p = b > 8 ? (is420 ? 2 : 3) : (is420 ? 0 : 1); // Profile 0: 4:2:0 8-bit | Profile 1: 4:2:2/4:4:4 8-bit | Profile 2: 4:2:0 10/12-bit | Profile 3: 4:2:2/4:4:4 10/12-bit
                return new CodecArgs($" -e vpx --force -v \" --codec=vp9 --profile={p} --bit-depth={b} --end-usage=q --cpu-used={preset} --cq-level={q} --kf-max-dist={g} {custom} \" --pix-format {pixFmt}");
            }

            if (c == Av1anCodec.X265)
            {
                string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : info.Presets[info.QDefault];
                string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : info.Presets[info.PresetDef];
                string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return new CodecArgs($" -e x265 --force -v \" --crf {q} --preset {preset} --keyint {g} --frame-threads 1 {custom} \" --pix-format {pixFmt}");
            }

            return new CodecArgs();
        }

        private static string GetKeyIntArg (MediaFile mediaFile, int intervalSeconds, string arg = "-g ")
        {
            if (mediaFile == null || mediaFile.VideoStreams.Count < 1)
                return "";

            int keyInt = ((float)(mediaFile?.VideoStreams.FirstOrDefault().Rate.GetFloat() * intervalSeconds)).RoundToInt();
            return keyInt >= 24 ? $"{arg}{keyInt}" : "";
        }

        public static string GetArgs(AudioCodec c, Dictionary<string, string> args)
        {
            if (c == AudioCodec.StripAudio)
            {
                return $"-an";
            }

            if (c == AudioCodec.Copy)
            {
                return $"-c:a copy";
            }

            if (c == AudioCodec.Aac)
            {
                string bitrate = args.ContainsKey("bitrate") ? args["bitrate"] : "128k";
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a aac -b:a {bitrate}k -aac_coder twoloop -ac {channels}";
            }

            if (c == AudioCodec.Opus)
            {
                string bitrate = args.ContainsKey("bitrate") ? args["bitrate"] : "96k";
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a libopus -b:a {bitrate}k -ac {channels}";
            }

            if (c == AudioCodec.Mp3)
            {
                string bitrate = args.ContainsKey("bitrate") ? args["bitrate"] : "320k";
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a libmp3lame -b:a {bitrate}k -ac {channels}";
            }

            if (c == AudioCodec.Flac)
            {
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a flac -ac {channels}";
            }

            return "";
        }

        public static string GetArgs(SubtitleCodec c)
        {
            if (c == SubtitleCodec.StripSubs)
                return $"-sn";

            if (c == SubtitleCodec.Copy)
                return $"-c:s copy";

            if (c == SubtitleCodec.MovText)
                return $"-c:s mov_text";

            if (c == SubtitleCodec.Srt)
                return $"-c:s srt";

            if (c == SubtitleCodec.WebVtt)
                return $"-c:s webvtt";

            return "";
        }

        public static CodecInfo GetCodecInfo(Av1anCodec c)
        {

            if (c == Av1anCodec.AomAv1)
            {
                string frName = "AV1 (AOM)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5", "6" };
                string[] colors = new string[] { "yuv420p", "yuv420p10le" };
                string qInfo = "CRF (0-63 - Lower is better)";
                string pInfo = "Lower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 5, colors, 1, 0, 63, 20, qInfo, pInfo);
            }

            if (c == Av1anCodec.SvtAv1)
            {
                string frName = "AV1 (SVT-AV1)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
                string[] colors = new string[] { "yuv420p", "yuv420p10le" };
                string qInfo = "CRF (0-50 - Lower is better)";
                string pInfo = "Lower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 5, colors, 1, 0, 50, 20, qInfo, pInfo);
            }

            if (c == Av1anCodec.VpxVp9)
            {
                string frName = "VP9 (VPX-VP9)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5" };
                string[] colors = new string[] { "yuv420p", "yuv420p10le" };
                string qInfo = "CRF (0-63 - Lower is better)";
                string pInfo = "Lower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 2, colors, 0, 0, 63, 24, qInfo, pInfo);
            }

            if (c == Av1anCodec.X265)
            {
                string frName = "H.265 / HEVC (x265)";
                string[] presets = new string[] { "veryslow", "slower", "slow", "medium", "fast", "faster", "veryfast", "superfast" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                string qInfo = "CRF (0-51 - Lower is better)";
                string pInfo = "Slower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 3, colors, 0, 0, 51, 22, qInfo, pInfo);
            }

            return new CodecInfo();
        }

        public static CodecInfo GetCodecInfo (VideoCodec c)
        {
            if(c == VideoCodec.Copy)
            {
                string frName = "Copy Video Without Re-Encoding";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = -1 };
            }

            if (c == VideoCodec.StripVideo)
            {
                string frName = "Disable (Strip Video)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = -1 };
            }

            if (c == VideoCodec.H264)
            {
                string frName = "H.264 / AVC (x264)";
                string[] presets = new string[] { "veryslow", "slower", "slow", "medium", "fast", "faster", "veryfast", "superfast" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                string qInfo = "CRF (0-51 - Lower is better)";
                string pInfo = "Slower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 3, colors, 0, 0, 51, 18, qInfo, pInfo);
            }

            if (c == VideoCodec.H265)
            {
                string frName = "H.265 / HEVC (x265)";
                string[] presets = new string[] { "veryslow", "slower", "slow", "medium", "fast", "faster", "veryfast", "superfast" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                string qInfo = "CRF (0-51 - Lower is better)";
                string pInfo = "Slower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 3, colors, 0, 0, 51, 22, qInfo, pInfo);
            }

            if (c == VideoCodec.H264Nvenc)
            {
                string frName = "H.264 / AVC (NVIDIA NVENC)";
                string[] presets = new string[] { "p5", "p4", "p3", "p2", "p1" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv444p16le" };
                string qInfo = "CRF (0-51 - Lower is better)";
                string pInfo = "Higher = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 0, colors, 0, 0, 51, 18, qInfo, pInfo);
            }

            if (c == VideoCodec.H265Nvenc)
            {
                string frName = "H.265 / HEVC (NVIDIA NVENC)";
                string[] presets = new string[] { "p7", "p6", "p5", "p4", "p3", "p2", "p1" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv444p16le" };
                string qInfo = "CRF (0-51 - Lower is better)";
                string pInfo = "Higher = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 0, colors, 0, 0, 51, 22, qInfo, pInfo);
            }

            if (c == VideoCodec.Vp9)
            {
                string frName = "VP9 (vpx-vp9)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                string qInfo = "CRF (0-63 - Lower is better)";
                string pInfo = "Lower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 3, colors, 0, 0, 63, 28, qInfo, pInfo);
            }

            if (c == VideoCodec.Av1)
            {
                string frName = "AV1 (svt-av1)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
                string[] colors = new string[] { "yuv420p", "yuv420p10le" };
                string qInfo = "CRF (0-50 - Lower is better)";
                string pInfo = "Lower = Better compression";
                return new CodecInfo(c.ToString(), frName, presets, 6, colors, 1, 0, 50, 26, qInfo, pInfo);
            }

            if (c == VideoCodec.Gif)
            {
                string frName = "GIF [Animated GIF]";
                string[] presets = new string[] { };
                string[] colors = new string[] { };
                string qInfo = "Color Palette Size (Higher is better)";
                return new CodecInfo(c.ToString(), frName, presets, 0, colors, 0, 16, 256, 128, qInfo);
            }

            if (c == VideoCodec.Png)
            {
                string frName = "PNG [Image Sequence]";
                string[] presets = new string[] { };
                string[] colors = new string[] { "rgb", "rgba" };
                return new CodecInfo(c.ToString(), frName, presets, 0, colors, 1, 0, 0, 0);
            }

            if (c == VideoCodec.Jpg)
            {
                string frName = "JPEG [Image Sequence]";
                string[] presets = new string[] { };
                string[] colors = new string[] { "yuvj420p", "yuvj444p" };
                string qInfo = "JPEG Quality (Lower is better)";
                return new CodecInfo(c.ToString(), frName, presets, 0, colors, 0, 1, 31, 3, qInfo);
            }

            return new CodecInfo();
        }

        public static CodecInfo GetCodecInfo(AudioCodec c)
        {
            if (c == AudioCodec.Copy)
            {
                string frName = "Copy Audio Without Re-Encoding";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = -1 };
            }

            if (c == AudioCodec.StripAudio)
            {
                string frName = "Disable (Strip Audio)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = -1 };
            }

            if (c == AudioCodec.Aac)
            {
                string frName = "AAC (Advanced Audio Coding)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = 144 };
            }

            if (c == AudioCodec.Opus)
            {
                string frName = "Opus";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = 128 };
            }

            if (c == AudioCodec.Mp3)
            {
                string frName = "MP3";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = 320 };
            }

            if (c == AudioCodec.Flac)
            {
                string frName = "FLAC (Free Lossless Audio Coding)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = -1 };
            }

            return new CodecInfo();
        }

        public static CodecInfo GetCodecInfo(SubtitleCodec c)
        {
            if (c == SubtitleCodec.Copy)
            {
                string frName = "Copy Subtitles Without Re-Encoding";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName};
            }

            if (c == SubtitleCodec.StripSubs)
            {
                string frName = "Disable (Strip Subtitles)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName };
            }

            if (c == SubtitleCodec.MovText)
            {
                string frName = "Mov_Text (3GPP Timed Text) - For MP4, MOV";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName };
            }

            if (c == SubtitleCodec.Srt)
            {
                string frName = "SRT (SubRip Text) - For MKV";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName };
            }

            if (c == SubtitleCodec.WebVtt)
            {
                string frName = "WebVTT (Web Video Text Tracks) - For WEBM";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName };
            }

            return new CodecInfo();
        }

        public static bool IsFixedFormat(VideoCodec c)
        {
            return c == VideoCodec.Gif || c == VideoCodec.Png || c == VideoCodec.Jpg;
        }

        public static bool IsSequence(VideoCodec c)
        {
            return c == VideoCodec.Png || c == VideoCodec.Jpg;
        }
    }
}
