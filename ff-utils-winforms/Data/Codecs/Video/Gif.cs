using Nmkoder.IO;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class Gif : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get; } = "Gif";
        public string FriendlyName { get; } = "GIF [Animated GIF]";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public string[] ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 0;
        public int QMax { get; } = 50;
        public int QDefault { get; } = 24;
        public string QInfo { get; } = "Color Palette Size (Higher is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null)
        {
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-f gif -gifflags -offsetting {cust}", $"split[s0][s1];[s0]palettegen={q}[p];[s1][p]paletteuse=dither=floyd_steinberg");
        }
    }
}
