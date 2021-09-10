using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoCodec = Nmkoder.Data.Codecs.VideoCodec;

namespace Nmkoder.Data
{
    class Containers
    {
        public class Container
        {
            public string Extension;
            public VideoCodec[] SupportedCodecs;
        }

        public static Container Mp4 = new Container { 
            Extension = "mp4", 
            SupportedCodecs = new VideoCodec[] { VideoCodec.H264, VideoCodec.H265, VideoCodec.Av1 } 
        };

        public static Container Mkv = new Container
        {
            Extension = "mkv",
            SupportedCodecs = new VideoCodec[] { VideoCodec.H264, VideoCodec.H265, VideoCodec.Vp9, VideoCodec.Av1 }
        };

        public static Container Webm = new Container
        {
            Extension = "webm",
            SupportedCodecs = new VideoCodec[] { VideoCodec.Vp9, VideoCodec.Av1 }
        };

        public static List<Container> containers = new List<Container> { Mp4, Mkv, Webm };
    }
}
