using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nmkoder.Data.Codecs
{
    class Vpx : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "VP9 (VPX)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 3;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv420p10le" };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 50;
        public int QDefault { get; } = 20;
        public string QInfo { get; } = "CRF (0-50 - Lower is better)";
        public string PresetInfo { get; } = "Lower = Better compression";

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.defaultKeyIntSecs), "");
            bool vmaf = encArgs.ContainsKey("qMode") && (UI.Tasks.Av1an.QualityMode)encArgs["qMode"].GetInt() == UI.Tasks.Av1an.QualityMode.TargetVmaf;
            string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            bool is420 = !(pixFmt.Contains("444") || pixFmt.Contains("422"));
            int b = pixFmt.Split('p').LastOrDefault().GetInt();
            b = (b > 0) ? b : 8; // Make bit depth default to 8 if it was detected as 0 (e.g. when using yuv420p which does not explicitly specify 8-bit)
            int p = b > 8 ? (is420 ? 2 : 3) : (is420 ? 0 : 1); // Profile 0: 4:2:0 8-bit | Profile 1: 4:2:2/4:4:4 8-bit | Profile 2: 4:2:0 10/12-bit | Profile 3: 4:2:2/4:4:4 10/12-bit
            string tiles = CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-columns=", "--tile-rows=");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";

            return new CodecArgs($" -e vpx --force -v \" --codec=vp9 --profile={p} --bit-depth={b} " +
                $"--end-usage=q --cpu-used={preset} --cq-level={q} " +
                $"--kf-max-dist={g} --threads={thr} --row-mt=1 {tiles} {cust} \" --pix-format {pixFmt}");
        }
    }
}
