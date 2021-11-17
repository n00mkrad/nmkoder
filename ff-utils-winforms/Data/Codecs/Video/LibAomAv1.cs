using Nmkoder.Extensions;
using Nmkoder.IO;
using System.Collections.Generic;
using System.Linq;

namespace Nmkoder.Data.Codecs
{
    class LibAomAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (AOM-AV1)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 5;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv420p10le" };
        public int ColorFormatDefault { get; } = 1;
        public int QMin { get; } = 0;
        public int QMax { get; } = 63;
        public int QDefault { get; } = 20;
        public string QInfo { get; } = "CRF (0-63 - Lower is better)";
        public string PresetInfo { get; } = "Lower = Better compression";

        public bool SupportsTwoPass { get; } = true;
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
            string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
            //string denoise = encArgs.ContainsKey("grainSynthDenoise") ? (encArgs["grainSynthDenoise"].GetBool() ? "1" : "0") : "0";
            string tiles = CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "-tile-columns ", "-tile-rows ");
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : $"-crf {q} -b:v 0";
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-pass 1" : "-pass 2");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libaom-av1 {p} {rc} -cpu-used {preset} -row-mt 1 -denoise-noise-level {grain} {tiles} {g} -pix_fmt {pixFmt} {cust}");
        }
    }
}
