using Nmkoder.Data.Colors;
using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs.Video
{
    #region x26x

    class Libx264 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "H.264 / AVC (x264)";
        public string[] Presets { get; } = new string[] { "veryslow", "slower", "slow", "medium", "fast", "faster", "veryfast", "superfast" };
        public int PresetDefault { get; } = 2;

        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv422P8, PixelFormats.Yuv444P8, PixelFormats.Yuv420P10, PixelFormats.Yuv422P10, PixelFormats.Yuv444P10 };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 51;
        public int QDefault { get; } = 18;
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
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs));
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : (q.GetInt() > 0 ? $"-crf {q}" : "-qp 0");
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-pass 1" : "-pass 2");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libx264 {p} {rc} -preset {preset} {g} -pix_fmt {pixFmt} {cust}");
        }
    }

    class Libx265 : IEncoder
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
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs));
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : (q.GetInt() > 0 ? $"-crf {q}" : "-x265-params lossless=1");
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-x265-params pass=1" : "-x265-params pass=2");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libx265 {p} {rc} -preset {preset} {g} -pix_fmt {pixFmt} {cust}");
        }
    }

    #endregion

    #region NVENC

    class H264Nvenc : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "H.264 / AVC (NVIDIA NVENC)";
        public string[] Presets { get; } = new string[] { "p5", "p4", "p3", "p2", "p1" };
        public int PresetDefault { get; } = 0;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv444P8 };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 51;
        public int QDefault { get; } = 18;
        public string QInfo { get; } = "CRF (0-51 - Lower is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            int br = encArgs.ContainsKey("bitrate") ? encArgs["bitrate"].GetInt() : 0;
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string rc = vbr ? $"-b:v {br}k -minrate {br / 4}k -maxrate {br * 2}k -bufsize {br}k" : (q.GetInt() > 0 ? $"-b:v 0 -cq {q}" : "-tune lossless");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v h264_nvenc {rc} -preset {preset} -pix_fmt {pixFmt} {cust}");
        }
    }

    class H265Nvenc : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "H.265 / HEVC (NVIDIA NVENC)";
        public string[] Presets { get; } = new string[] { "p7", "p6", "p5", "p4", "p3", "p2", "p1" };
        public int PresetDefault { get; } = 0;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv444P8, PixelFormats.P010 };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 51;
        public int QDefault { get; } = 22;
        public string QInfo { get; } = "CRF (0-51 - Lower is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            int br = encArgs.ContainsKey("bitrate") ? encArgs["bitrate"].GetInt() : 0;
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string rc = vbr ? $"-b:v {br}k -minrate {br / 4}k -maxrate {br * 2}k -bufsize {br}k" : (q.GetInt() > 0 ? $"-b:v 0 -cq {q}" : "-tune lossless");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v hevc_nvenc {rc} -preset {preset} -pix_fmt {pixFmt} {cust}");
        }
    }

    #endregion

    #region Google/AOM

    class LibVpx : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "VP9 (VPX)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 3;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv422P8, PixelFormats.Yuva420P8, PixelFormats.Yuv444P8, PixelFormats.Yuv420P10, PixelFormats.Yuv422P10, PixelFormats.Yuv444P10 };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 63;
        public int QDefault { get; } = 24;
        public string QInfo { get; } = "CRF (0-63 - Lower is better)";
        public string PresetInfo { get; } = "Lower = Better compression";

        public bool SupportsTwoPass { get; } = true;
        public bool ForceTwoPass { get; } = true;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : $"-crf {q}";
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs));
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-pass 1" : "-pass 2");
            string tiles = mediaFile.VideoStreams.Count > 0 ? CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "-tile-columns ", "-tile-rows ") : "";
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libvpx-vp9 {p} {rc} {tiles} -row-mt 1 -cpu-used {preset} {g} -pix_fmt {pixFmt} {cust}");
        }
    }

    class LibSvtAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (SVT-AV1)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        public int PresetDefault { get; } = 7;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv420P10 };
        public int ColorFormatDefault { get; } = 1;
        public int QMin { get; } = 0;
        public int QMax { get; } = 50;
        public int QDefault { get; } = 24;
        public string QInfo { get; } = "CRF (0-50 - Lower is better)";
        public string PresetInfo { get; } = "Lower = Better compression";

        public bool SupportsTwoPass { get; } = true;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            if (vbr && pass == Pass.OneOfTwo) Logger.Log($"WARNING: The 2-Pass implementation of SVT-AV1 is experimental. It might crash or produce inaccurate results.");
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string rc = vbr ? $"-rc vbr -b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : $"-qp {q}";
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs), "-g ", vbr ? 255 : 480); // SVT can't do GOP size >255 in VBR mode
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-pass 1" : "-pass 2");
            string tiles = ""; // TEMP DISABLED AS IT SEEMS TO SLOW THINGS DOWN // CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "-tile_rows ", "-tile_columns ");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libsvtav1 {p} {rc} -preset {preset} {g} {tiles} -pix_fmt {pixFmt} {cust}");
        }
    }

    class LibAomAv1 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AV1 (AOM-AV1)";
        public string[] Presets { get; } = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public int PresetDefault { get; } = 6;
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv422P8, PixelFormats.Yuv444P8, PixelFormats.Yuv420P10, PixelFormats.Yuv422P10, PixelFormats.Yuv444P10 };
        public int ColorFormatDefault { get; } = 3;
        public int QMin { get; } = 0;
        public int QMax { get; } = 63;
        public int QDefault { get; } = 20;
        public string QInfo { get; } = "CRF (0-63 - Lower is better)";
        public string PresetInfo { get; } = "Lower = Better compression";

        public bool SupportsTwoPass { get; } = true;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string g = CodecUtils.GetKeyIntArg(mediaFile, Config.GetInt(Config.Key.DefaultKeyIntSecs));
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : PixFmtUtils.GetFormat(ColorFormats[ColorFormatDefault]).Name;
            string grain = encArgs.ContainsKey("grainSynthStrength") ? encArgs["grainSynthStrength"] : "0";
            //string denoise = encArgs.ContainsKey("grainSynthDenoise") ? (encArgs["grainSynthDenoise"].GetBool() ? "1" : "0") : "0";
            string tiles = mediaFile.VideoStreams.Count > 0 ? CodecUtils.GetTilingArgs(mediaFile.VideoStreams.FirstOrDefault().Resolution, "-tile-rows ", "-tile-columns ") : "";
            string rc = vbr ? $"-b:v {(encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "0")}k" : $"-crf {q} -b:v 0";
            string p = pass == Pass.OneOfOne ? "" : (pass == Pass.OneOfTwo ? "-pass 1" : "-pass 2");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v libaom-av1 {p} {rc} -cpu-used {preset} -row-mt 1 -denoise-noise-level {grain} {tiles} {g} -pix_fmt {pixFmt} {cust}");
        }
    }

    #endregion

    #region Image Formats

    class Gif : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "GIF [Animated GIF]";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 0;
        public int QMax { get; } = 256;
        public int QDefault { get; } = 128;
        public string QInfo { get; } = "Color Palette Size (Higher is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = true;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-f gif -gifflags -offsetting {cust}", $"split[s0][s1];[s0]palettegen={q}[p];[s1][p]paletteuse=dither=floyd_steinberg");
        }
    }

    class Jpg : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "JPEG [Image Sequence]";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Yuv420P8, PixelFormats.Yuv422P8, PixelFormats.Yuv444P8 };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 1;
        public int QMax { get; } = 31;
        public int QDefault { get; } = 3;
        public string QInfo { get; } = "Quality (1-31 - Lower is better)";
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = true;
        public bool IsSequence { get; } = true;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v mjpeg -qmin 1 -q:v {q} {cust}");
        }
    }

    class Png : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "PNG [Image Sequence]";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; } = new List<PixelFormats>() { PixelFormats.Rgb24, PixelFormats.Rgba, PixelFormats.Rgb48, PixelFormats.Rgba64 };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 0;
        public int QDefault { get; } = 0;
        public string QInfo { get; }
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = true;
        public bool IsSequence { get; } = true;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v png -compression_level 3 {cust}");
        }
    }

    #endregion

    #region Mux

    class CopyVideo : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Copy Video Without Re-Encoding";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; }
        public int QMax { get; }
        public int QDefault { get; }
        public string QInfo { get; } = "Does not alter quality.";
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = true;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v copy {cust}");
        }
    }

    class StripVideo : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Disable (Strip Video)";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; }
        public int QMax { get; }
        public int QDefault { get; }
        public string QInfo { get; }
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = true;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-vn {cust}");
        }
    }

    #endregion
}
