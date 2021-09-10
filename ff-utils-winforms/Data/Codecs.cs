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
        public enum CodecType { Video, AnimImage, Image, Audio }

       //public class Codec
       //{
       //    public string Name;
       //    public string LongName;
       //    public CodecType Type = CodecType.Video;
       //
       //    public Codec (string name, string longName, CodecType type, Encoders.Encoder encoder)
       //    {
       //        Name = name;
       //        LongName = longName;
       //        Type = type;
       //        Encoders.Encoder Encoder = encoder;
       //    }
       //}

        public enum VideoCodec { Copy, StripVideo, H264, H265, Vp9, Av1 };
        public enum AudioCodec { Copy, StripAudio, Aac, Opus };

        public static string GetArgs(VideoCodec c, Dictionary<string, string> args)
        {
            if (c == VideoCodec.StripVideo)
            {
                return $"-vn";
            }

            if (c == VideoCodec.H264)
            {
                string preset = args.ContainsKey("preset") ? args["preset"] : "medium";
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : "yuv420p";
                return $"-c:v libx264 -preset {preset} -pix_fmt {pixFmt}";
            }

            if (c == VideoCodec.H265)
            {
                string preset = args.ContainsKey("preset") ? args["preset"] : "medium";
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : "yuv420p";
                return $"-c:v libx265 -preset {preset} -pix_fmt {pixFmt}";
            }

            if (c == VideoCodec.Vp9)
            {
                string preset = args.ContainsKey("preset") ? args["preset"] : "3";
                string pixFmt = args.ContainsKey("pixFmt") ? args["pixFmt"] : "yuv420p";
                return $"-c:v libvpx-vp9 -cpu-used {preset} -pix_fmt {pixFmt}";
            }

            return "";
        }

        public static string GetArgs(AudioCodec c, Dictionary<string, string> args)
        {
            if (c == AudioCodec.StripAudio)
            {
                return $"-an";
            }

            if (c == AudioCodec.Aac)
            {
                string bitrate = args.ContainsKey("bitrate") ? args["bitrate"] : "128k";
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a aac -b:a {bitrate} -ac {channels}";
            }

            if (c == AudioCodec.Opus)
            {
                string bitrate = args.ContainsKey("bitrate") ? args["bitrate"] : "128k";
                string channels = args.ContainsKey("ac") ? args["ac"] : "2";
                return $"-c:a libopus -b:a {bitrate} -ac {channels}";
            }

            return "";
        } 

        public static string GetFriendlyName (VideoCodec c)
        {
            if (c == VideoCodec.StripVideo) return "Disable (Strip Video)";
            if (c == VideoCodec.H264) return "H.264 (AVC - Advanced Video Coding)";
            if (c == VideoCodec.H265) return "H.265 (HEVC - High Efficiency Video Coding)";
            if (c == VideoCodec.Vp9) return "VP9 (Google VP9)";
            if (c == VideoCodec.Av1) return "AV1 (AOMedia Video 1)";
            return c.ToString();
        }

        public static string GetFriendlyName(AudioCodec c)
        {
            if (c == AudioCodec.StripAudio) return "Disable (Strip Audio)";
            if (c == AudioCodec.Aac) return "AAC (Advanced Audio Coding)";
            if (c == AudioCodec.Opus) return "Opus";
            return c.ToString();
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
