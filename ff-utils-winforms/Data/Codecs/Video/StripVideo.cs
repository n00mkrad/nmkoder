using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs
{
    class StripVideo : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Disable (Strip Video)";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public string[] ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; }
        public int QMax { get; }
        public int QDefault { get; }
        public string QInfo { get; }
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = true;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-vn {cust}");
        }
    }
}
