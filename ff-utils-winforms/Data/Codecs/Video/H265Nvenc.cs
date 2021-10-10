using Nmkoder.Extensions;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class H265Nvenc : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get; } = "H265Nvenc";
        public string FriendlyName { get; } = "H.265 / HEVC (NVIDIA NVENC)";
        public string[] Presets { get; } = new string[] { "p7", "p6", "p5", "p4", "p3", "p2", "p1" };
        public int PresetDefault { get; } = 0;
        public string[] ColorFormats  { get; } = new string[] { "yuv420p", "yuv444p", "yuv444p16le" };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 51;
        public int QDefault { get; } = 22;
        public string QInfo { get; } = "CRF (0-51 - Lower is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null)
        {
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v hevc_nvenc -b:v 0 {(q.GetInt() > 0 ? $"-cq {q}" : "-tune lossless")} -preset {preset} -pix_fmt {pixFmt} {cust}");
        }
    }
}
