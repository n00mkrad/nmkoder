using Nmkoder.IO;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class Jpg : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "JPEG [Image Sequence]";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public string[] ColorFormats { get; } = new string[] { "yuvj420p", "yuvj444p" };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 1;
        public int QMax { get; } = 31;
        public int QDefault { get; } = 3;
        public string QInfo { get; } = "Quality (1-31 - Lower is better)";
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = true;
        public bool IsSequence { get; } = true;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v mjpeg -qmin 1 -q:v {q} {cust}");
        }
    }
}
