using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class Codecs
    {
        public enum CodecType { Video, AnimImage, Image }

        public class Codec
        {
            public string Name;
            public string LongName;
            public CodecType Type;
        }

        public static Codec Avc = new Codec { Name = "H.264", LongName = "H.264 (AVC)" };
        public static Codec Hevc = new Codec { Name = "H.265", LongName = "H.265 (HEVC)" };
        public static Codec Vp9 = new Codec { Name = "VP9", LongName = "Google VP9" };
        public static Codec Av1 = new Codec { Name = "AV1", LongName = "AOMedia Video 1 (AV1)" };
        public static Codec ProRes = new Codec { Name = "ProRes", LongName = "Apple ProRes" };
        public static Codec Gif = new Codec { Name = "GIF", LongName = "Animated GIF" };
    }
}
