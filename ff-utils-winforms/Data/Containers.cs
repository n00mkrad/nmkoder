using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class Containers
    {
        public class Container
        {
            public string Extension;
            public Codecs.Codec[] SupportedCodecs;
        }

        public Container Mp4 = new Container { 
            Extension = "mp4", 
            SupportedCodecs = new Codecs.Codec[] { Codecs.Avc, Codecs.Hevc, Codecs.Av1 } 
        };

        public Container Mkv = new Container
        {
            Extension = "mkv",
            SupportedCodecs = new Codecs.Codec[] { Codecs.Avc, Codecs.Hevc, Codecs.Vp9, Codecs.Av1, Codecs.ProRes }
        };

        public Container Webm = new Container
        {
            Extension = "mkv",
            SupportedCodecs = new Codecs.Codec[] { Codecs.Vp9, Codecs.Av1 }
        };
    }
}
