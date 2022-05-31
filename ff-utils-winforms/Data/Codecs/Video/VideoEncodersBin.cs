using Nmkoder.Data.Colors;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs.Video
{
    class AomAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (AOM)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 6;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv422P8, PixelFormats.Yuv444P8, PixelFormats.Yuv420P10, PixelFormats.Yuv422P10, PixelFormats.Yuv444P10 };
        public int ColorFormatDefault { get; } = 3;
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
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string denoise = encArgs.ContainsKey("grainSynthDenoise") ? (encArgs["grainSynthDenoise"].GetBool() ? "1" : "0") : "0";
            string tiles = mediaFile.VideoStreams.Count > 0 ? CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-rows=", "--tile-columns=") : "";
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            string adv = encArgs.ContainsKey("advanced") ? encArgs["advanced"] : "";
            string colors = "";

            if (mediaFile != null && mediaFile.ColorData != null)
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

    class SvtAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (SVT-AV1)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        public int PresetDefault { get; } = 5;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv420P10 };
        public int ColorFormatDefault { get; } = 1;
        public int QMin { get; } = 0;
        public int QMax { get; } = 63;
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
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
            string denoise = encArgs.ContainsKey("grainSynthDenoise") ? (encArgs["grainSynthDenoise"].GetBool() ? "1" : "0") : "0";
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string tiles = ""; // TEMP DISABLED AS IT SEEMS TO SLOW THINGS DOWN // = CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-rows ", "--tile-columns ");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            string adv = encArgs.ContainsKey("advanced") ? encArgs["advanced"].Replace("=", " ") : "";
            string colors = "";

            if (mediaFile != null && mediaFile.ColorData != null)
            {
                int range = mediaFile.ColorData.ColorRange == 2 ? 1 : 0; // SVT range is 0 (tv) and 1 (full), not 0 (unspecified), 1 (tv), 2 (full) like in VideoColorData
                colors = $"--color-primaries {mediaFile.ColorData.ColorPrimaries} --transfer-characteristics {mediaFile.ColorData.ColorTransfer} --matrix-coefficients {mediaFile.ColorData.ColorMatrixCoeffs} --color-range {range}";
            }

            return new CodecArgs($" -e svt-av1 --force -v \" --preset {preset} --crf {q} --keyint {g} --lp {thr} --film-grain {grain} --film-grain-denoise {denoise} {colors} {tiles} {adv} {cust} \" --pix-format {pixFmt}");
        }
    }

    class Vpx : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "VP9 (VPX)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 3;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuva420P8, PixelFormats.Yuv444P8, PixelFormats.Yuv420P10, PixelFormats.Yuv444P10 };
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
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs), "");
            bool vmaf = encArgs.ContainsKey("qMode") && (UI.Tasks.Av1an.QualityMode)encArgs["qMode"].GetInt() == UI.Tasks.Av1an.QualityMode.TargetVmaf;
            string q = vmaf ? "0" : encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            bool is420 = !(pixFmt.Contains("444") || pixFmt.Contains("422"));
            int b = pixFmt.Split('p').LastOrDefault().GetInt();
            b = (b > 0) ? b : 8; // Make bit depth default to 8 if it was detected as 0 (e.g. when using yuv420p which does not explicitly specify 8-bit)
            int p = b > 8 ? (is420 ? 2 : 3) : (is420 ? 0 : 1); // Profile 0: 4:2:0 8-bit | Profile 1: 4:2:2/4:4:4 8-bit | Profile 2: 4:2:0 10/12-bit | Profile 3: 4:2:2/4:4:4 10/12-bit
            string tiles = mediaFile.VideoStreams.Count > 0 ? CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "--tile-columns=", "--tile-rows=") : "";
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";

            return new CodecArgs($" -e vpx --force -v \" --codec=vp9 --profile={p} --bit-depth={b} " +
                $"--end-usage=q --cpu-used={preset} --cq-level={q} " +
                $"--kf-max-dist={g} --threads={thr} --row-mt=1 {tiles} {cust} \" --pix-format {pixFmt}");
        }
    }

    class X265 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "H.265 / HEVC (x265)";
        public string[] Presets { get; } = new string[] { "veryslow", "slower", "slow", "medium", "fast", "faster", "veryfast", "superfast" };
        public int PresetDefault { get; } = 3;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv422P8, PixelFormats.Yuv444P8, PixelFormats.Yuv420P10, PixelFormats.Yuv422P10, PixelFormats.Yuv444P10 };
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
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            int bitDepth = FormatUtils.GetBitDepthFromPixelFormat(pixFmt);
            string thr = encArgs.ContainsKey("threads") ? encArgs["threads"] : "0";
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            string adv = encArgs.ContainsKey("advanced") ? encArgs["advanced"] : "";
            string colors = "";

            if (mediaFile != null && mediaFile.ColorData != null)
            {
                string range = mediaFile.ColorData.ColorRange == 2 ? "full" : "limited"; // x265 range is "limited" (tv) and "full", not 0 (unspecified), 1 (tv), 2 (full) like in VideoColorData
                colors = $"--colorprim {mediaFile.ColorData.ColorPrimaries} --transfer {mediaFile.ColorData.ColorTransfer} --colormatrix {mediaFile.ColorData.ColorMatrixCoeffs} --range {range}";
            }

            return new CodecArgs($" -e x265 --force -v \" --crf {q} --preset {preset} --keyint {g} --frame-threads {thr} --output-depth {bitDepth} {colors} {adv} {cust} \" --pix-format {pixFmt}");
        }
    }
}
