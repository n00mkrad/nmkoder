using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nmkoder.Data.Codecs
{
    class AomAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (AOM)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 6;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv420p10le", "yuv422p", "yuv422p10le", "yuv444p", "yuv444p10le" };
        public int ColorFormatDefault { get; } = 1;
        public int QMin { get; } = 0;
        public int QMax { get; } = 63;
        public int QDefault { get; } = 20;
        public string QInfo { get; } = "CRF (0-63 - Lower is better)";
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
            string denoise = encArgs.ContainsKey("grainSynthDenoise") ? (encArgs["grainSynthDenoise"].GetBool() ? "1" : "0") : "0";
            string tiles = CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-rows=", "--tile-columns=");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            string adv = encArgs.ContainsKey("advanced") ? encArgs["advanced"] : "";
            string colors = "";

            if(mediaFile != null && mediaFile.ColorData != null)
            {
                string prims = ColorDataUtils.FormatForAom(ColorDataUtils.GetColorPrimariesString(mediaFile.ColorData.ColorPrimaries));
                string transfer = ColorDataUtils.FormatForAom(ColorDataUtils.GetColorTransferString(mediaFile.ColorData.ColorTransfer));
                string matrixCoeffs = ColorDataUtils.FormatForAom(ColorDataUtils.GetColorMatrixCoeffsString(mediaFile.ColorData.ColorMatrixCoeffs));
                colors = $"{(prims != "" ? $"--color-primaries={prims}" : "")} {(transfer != "" ? $"--transfer-characteristics={transfer}" : "")} {(matrixCoeffs != "" ? $"--matrix-coefficients={matrixCoeffs}" : "")}";

                if (mediaFile.ColorData.ColorPrimaries == 9)
                    colors += " --deltaq-mode=5 --enable-chroma-deltaq=1";
            }

            return new CodecArgs($" -e aom -v \" " +
                $"--end-usage=q --cpu-used={preset} --cq-level={q} " +
                $"--disable-kf --kf-min-dist=12 --kf-max-dist={g} " +
                $"--enable-dnl-denoising={denoise} --denoise-noise-level={grain} " +
                $"{colors} --threads={thr} {tiles} {adv} {cust} \" --pix-format {pixFmt}");
        }
    }
}
