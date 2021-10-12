using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class LibVpx : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get; } = "Vp9";
        public string FriendlyName { get; } = "VP9 (VPX)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 3;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv420p10le" };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 63;
        public int QDefault { get; } = 24;
        public string QInfo { get; } = "CRF (0-63 - Lower is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}" : $"-crf {q}";
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.defaultKeyIntSecs));
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libvpx-vp9 {rc} -tile-columns 1 -tile-rows 1 -row-mt 1 -cpu-used {preset} {g} -pix_fmt {pixFmt} {cust}");
        }
    }
}
