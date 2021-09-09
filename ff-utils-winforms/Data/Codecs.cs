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

        public class Codec
        {
            public string Name;
            public string LongName;
            public CodecType Type = CodecType.Video;

            public Codec (string name, string longName, CodecType type = CodecType.Video)
            {
                Name = name;
                LongName = longName;
                Type = type;
            }
        }

        public static Codec Avc = new Codec("H.264", "H.264 (AVC)");
        public static Codec Hevc = new Codec("H.265", "H.265 (HEVC)");
        public static Codec Vp9 = new Codec("VP9", "Google VP9");
        public static Codec Av1 = new Codec("AV1", "AV1 (AOMedia Video 1)");
        public static Codec ProRes = new Codec("ProRes", "Apple ProRes");
        public static Codec Gif = new Codec("GIF", "Animated GIF", CodecType.AnimImage);

        public static Codec Aac = new Codec("AAC", "AAC (Advanced Audio Coding)", CodecType.Audio);
        public static Codec Opus = new Codec("Opus", "Opus", CodecType.Audio);
        
        public static Codec Png = new Codec("PNG", "PNG (Portable Network Graphics)", CodecType.Image);
        public static Codec Jpeg = new Codec("JPEG", "JPEG (Joint Photographic Experts Group)",CodecType.Image);

        public static List<Codec> vCodecs = new List<Codec> { Avc, Hevc, Vp9, Av1, ProRes, Gif };
        public static List<Codec> aCodecs = new List<Codec> { Aac, Opus };
        public static List<Codec> iCodecs = new List<Codec> { Png, Jpeg };
    }
}
