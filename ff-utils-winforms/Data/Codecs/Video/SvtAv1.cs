using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nmkoder.Data.Codecs
{
    class SvtAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (SVT-AV1)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        public int PresetDefault { get; } = 5;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv420p10le" };
        public int ColorFormatDefault { get; } = 1;
        public int QMin { get; } = 0;
        public int QMax { get; } = 50;
        public int QDefault { get; } = 20;
        public string QInfo { get; } = "CRF (0-50 - Lower is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool SupportsTwoPass { get; } = false;
		public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.defaultKeyIntSecs), "");
            bool vmaf = encArgs.ContainsKey("qMode") && (UI.Tasks.Av1an.QualityMode)encArgs["qMode"].GetInt() == UI.Tasks.Av1an.QualityMode.TargetVmaf;

            string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
            string tiles = CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-rows ", "--tile-columns ");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($" -e svt-av1 --force -v \" --preset {preset} --crf {q} --keyint {g} --lp 4 --film-grain {grain} {tiles} {cust} \" --pix-format {pixFmt}");
        }
    }
}
