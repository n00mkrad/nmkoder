using Nmkoder.Extensions;
using Nmkoder.IO;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class Libx265 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "H.265 / HEVC (x265)";
        public string[] Presets { get; } = new string[] { "veryslow", "slower", "slow", "medium", "fast", "faster", "veryfast", "superfast" };
        public int PresetDefault { get; } = 3;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv444p", "yuv420p10le", "yuv444p10le" };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 51;
        public int QDefault { get; } = 22;
        public string QInfo { get; } = "CRF (0-51 - Lower is better)";
        public string PresetInfo { get; } = "Slower = Better compression";

        public bool SupportsTwoPass { get; } = true;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.defaultKeyIntSecs));
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : $"-crf {q}";
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-x265-params pass=1" : "-x265-params pass=2");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libx265 {p} {rc} -preset {preset} {g} -pix_fmt {pixFmt} {cust}");
        }
    }
}
