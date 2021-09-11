using Nmkoder.Extensions;
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

        public enum VideoCodec { Copy, StripVideo, H264, H265, Vp9, Av1 };
        public enum AudioCodec { Copy, StripAudio, Aac, Opus };
        public enum SubtitleCodec { Copy, StripSubs, MovText, Srt };

        public static string GetArgs(VideoCodec c, Dictionary<string, string> args)
        {
            CodecInfo info = GetCodecInfo(c);

            if (c == VideoCodec.StripVideo)
            {
                return $"-vn";
            }

            if (c == VideoCodec.Copy)
            {
                return $"-c:v copy";
            }

            if (c == VideoCodec.H264)
            {
                string q = args.ContainsKey("q") ? args["q"] : info.Presets[info.QDefault];
                string preset = args.ContainsKey("preset") ? args["preset"] : info.Presets[info.PresetDef];
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return $"-c:v libx264 -crf {q} -preset {preset} -pix_fmt {pixFmt}";
            }

            if (c == VideoCodec.H265)
            {
                string q = args.ContainsKey("q") ? args["q"] : info.Presets[info.QDefault];
                string preset = args.ContainsKey("preset") ? args["preset"] : info.Presets[info.PresetDef];
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return $"-c:v libx265 -crf {q} -preset {preset} -pix_fmt {pixFmt}";
            }

            if (c == VideoCodec.Vp9)
            {
                string q = args.ContainsKey("q") ? args["q"] : info.Presets[info.QDefault];
                string preset = args.ContainsKey("preset") ? args["preset"] : info.Presets[info.PresetDef];
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return $"-c:v libvpx-vp9 -crf {q} -cpu-used {preset} -pix_fmt {pixFmt}";
            }

            if (c == VideoCodec.Av1)
            {
                string q = args.ContainsKey("q") ? args["q"] : info.Presets[info.QDefault];
                string preset = args.ContainsKey("preset") ? args["preset"] : info.Presets[info.PresetDef];
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : info.ColorFormats[info.ColorFormatDef];
                return $"-c:v libsvtav1 -qp {q} -tile_columns 2 -tile_rows 1 -preset {preset} -pix_fmt {pixFmt}";
            }

            return "";
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
                return $"-c:a aac -b:a {bitrate}k -ac {channels}";
            }

            if (c == AudioCodec.Opus)
            {
                string bitrate = args.ContainsKey("bitrate") ? args["bitrate"] : "128k";
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a libopus -b:a {bitrate}k -ac {channels}";
            }

            return "";
        }

        public static string GetArgs(SubtitleCodec c)
        {
            if (c == SubtitleCodec.StripSubs)
            {
                return $"-sn";
            }

            if (c == SubtitleCodec.Copy)
            {
                return $"-c:s copy";
            }

            if (c == SubtitleCodec.MovText)
            {
                return $"-c:s mov_text";
            }

            if (c == SubtitleCodec.Srt)
            {
                return $"-c:s srt";
            }

            return "";
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
                string frName = "H.264 (AVC - Advanced Video Coding)";
                string[] presets = new string[] { "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                return new CodecInfo(c.ToString(), frName, presets, 4, colors, 0, 0, 51, 18);
            }

            if (c == VideoCodec.H265)
            {
                string frName = "H.265 (HEVC - High Efficiency Video Coding)";
                string[] presets = new string[] { "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                return new CodecInfo(c.ToString(), frName, presets, 4, colors, 0, 0, 51, 22);
            }

            if (c == VideoCodec.Vp9)
            {
                string frName = "VP9 (Google VP9)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5" };
                string[] colors = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
                return new CodecInfo(c.ToString(), frName, presets, 3, colors, 0, 0, 63, 28);
            }

            if (c == VideoCodec.Av1)
            {
                string frName = "AV1 (AOMedia Video 1)";
                string[] presets = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
                string[] colors = new string[] { "yuv420p", "yuv420p10le" };
                return new CodecInfo(c.ToString(), frName, presets, 7, colors, 1, 0, 50, 26);
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
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = 128 };
            }

            if (c == AudioCodec.Opus)
            {
                string frName = "Opus";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName, QDefault = 96 };
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
                string frName = "Mov_Text (3GPP Timed Text)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName };
            }

            if (c == SubtitleCodec.Srt)
            {
                string frName = "SRT (SubRip Text)";
                return new CodecInfo { Name = c.ToString(), FriendlyName = frName };
            }

            return new CodecInfo();
        }

        //public static Codec Strip = new Codec("None", "Disable (Strip)");
        //public static Codec Copy = new Codec("Copy", "Copy Stream Without Re-Encoding");

        //public static Codec Avc = new Codec("H.264", "H.264 (AVC)", CodecType.Video, Encoders.X264);
        //public static Codec Hevc = new Codec("H.265", "H.265 (HEVC)", CodecType.Video, Encoders.X265);
        //public static Codec Vp9 = new Codec("VP9", "Google VP9", CodecType.Video, Encoders.VpxVp9);
        //public static Codec Av1 = new Codec("AV1", "AV1 (AOMedia Video 1)", CodecType.Video, Encoders.SvtAv1);
        //public static Codec ProRes = new Codec("ProRes", "Apple ProRes", CodecType.Video);
        //public static Codec Gif = new Codec("GIF", "Animated GIF", CodecType.AnimImage);

        //public static Codec Aac = new Codec("AAC", "AAC (Advanced Audio Coding)", CodecType.Audio);
        //public static Codec Opus = new Codec("Opus", "Opus", CodecType.Audio);
        //
        //public static Codec Png = new Codec("PNG", "PNG (Portable Network Graphics)", CodecType.Image);
        //public static Codec Jpeg = new Codec("JPEG", "JPEG (Joint Photographic Experts Group)", CodecType.Image);

        //public static List<Codec> all = new List<Codec> { Avc, Hevc, Vp9, Av1 };
        //public static List<Codec> vCodecs = new List<Codec> { Avc, Hevc, Vp9, Av1 };
        //public static List<Codec> aCodecs = new List<Codec> { Aac, Opus };
        //public static List<Codec> iCodecs = new List<Codec> { Png, Jpeg };
    }
}
