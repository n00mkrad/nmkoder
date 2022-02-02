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
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        public int PresetDefault { get; } = 5;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv420p10le" };
        public int ColorFormatDefault { get; } = 1;
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
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs), "");
            bool vmaf = encArgs.ContainsKey("qMode") && (UI.Tasks.Av1an.QualityMode)encArgs["qMode"].GetInt() == UI.Tasks.Av1an.QualityMode.TargetVmaf;

            string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string tiles = ""; // TEMP DISABLED AS IT SEEMS TO SLOW THINGS DOWN // = CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-rows ", "--tile-columns ");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            string adv = encArgs.ContainsKey("advanced") ? encArgs["advanced"] : "";
            string colors = "";

            if (mediaFile != null && mediaFile.ColorData != null)
            {
                int range = mediaFile.ColorData.ColorRange == 2 ? 1 : 0; // SVT range is 0 (tv) and 1 (full), not 0 (unspecified), 1 (tv), 2 (full) like in VideoColorData
                colors = $"--color-primaries {mediaFile.ColorData.ColorPrimaries} --transfer-characteristics {mediaFile.ColorData.ColorTransfer} --matrix-coefficients {mediaFile.ColorData.ColorMatrixCoeffs} --color-range {range}";
            }

            return new CodecArgs($" -e svt-av1 --force -v \" --preset {preset} --crf {q} --keyint {g} --lp {thr} --film-grain {grain} {colors} {tiles} {adv} {cust} \" --pix-format {pixFmt}");
        }
    }
}
