using Nmkoder.Extensions;
using Nmkoder.IO;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class X265 : IEncoder
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
        public int QDefault { get; } = 20;
        public string QInfo { get; } = "CRF (0-51 - Lower is better)";
        public string PresetInfo { get; } = "Slower = Better compression";

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs), "");
            bool vmaf = encArgs.ContainsKey("qMode") && (UI.Tasks.Av1an.QualityMode)encArgs["qMode"].GetInt() == UI.Tasks.Av1an.QualityMode.TargetVmaf;
            string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            string adv = encArgs.ContainsKey("advanced") ? encArgs["advanced"] : "";
            string colors = "";

            if (mediaFile != null && mediaFile.ColorData != null)
            {
                string range = mediaFile.ColorData.ColorRange == 2 ? "full" : "limited"; // x265 range is "limited" (tv) and "full", not 0 (unspecified), 1 (tv), 2 (full) like in VideoColorData
                colors = $"--colorprim {mediaFile.ColorData.ColorPrimaries} --transfer {mediaFile.ColorData.ColorTransfer} --colormatrix {mediaFile.ColorData.ColorMatrixCoeffs} --range {range}";
            }

            return new CodecArgs($" -e x265 --force -v \" --crf {q} --preset {preset} --keyint {g} --frame-threads {thr} {colors} {adv} {cust} \" --pix-format {pixFmt}");
        }
    }
}
