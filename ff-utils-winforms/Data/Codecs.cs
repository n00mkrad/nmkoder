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
        }

        public static Codec[] vCodecs = new Codec[] { Avc, Hevc, Vp9, Av1, ProRes, Gif };
        public static Codec[] aCodecs = new Codec[] { Aac, Opus };
        public static Codec[] iCodecs = new Codec[] { Png, Jpeg };

        public static Codec Avc = new Codec { Name = "H.264", LongName = "H.264 (AVC)" };
        public static Codec Hevc = new Codec { Name = "H.265", LongName = "H.265 (HEVC)" };
        public static Codec Vp9 = new Codec { Name = "VP9", LongName = "Google VP9" };
        public static Codec Av1 = new Codec { Name = "AV1", LongName = "AV1 (AOMedia Video 1)" };
        public static Codec ProRes = new Codec { Name = "ProRes", LongName = "Apple ProRes" };
        public static Codec Gif = new Codec { Name = "GIF", LongName = "Animated GIF", Type = CodecType.AnimImage };

        public static Codec Aac = new Codec { Name = "AAC", LongName = "AAC (Advanced Audio Coding)", Type = CodecType.Audio };
        public static Codec Opus = new Codec { Name = "Opus", LongName = "Opus", Type = CodecType.Audio };
        
        public static Codec Png = new Codec { Name = "PNG", LongName = "PNG (Portable Network Graphics)", Type = CodecType.Image };
        public static Codec Jpeg = new Codec { Name = "JPEG", LongName = "JPEG (Joint Photographic Experts Group)", Type = CodecType.Image };
    }
}
