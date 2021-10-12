using Nmkoder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs
{
    class H264Nvenc : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Video;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "H.264 / AVC (NVIDIA NVENC)";
        public string[] Presets { get; } = new string[] { "p5", "p4", "p3", "p2", "p1" };
        public int PresetDefault { get; } = 0;
        public string[] ColorFormats { get; } = new string[] { "yuv420p", "yuv444p", "yuv444p16le" };
        public int ColorFormatDefault { get; } = 0;
        public int QMin { get; } = 0;
        public int QMax { get; } = 51;
        public int QDefault { get; } = 18;
        public string QInfo { get; } = "CRF (0-51 - Lower is better)";
        public string PresetInfo { get; } = "Higher = Better compression";

        public bool SupportsTwoPass { get; } = false;
		public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, Pass pass = Pass.OneOfOne, MediaFile mediaFile = null)
        {
            bool vbr = encArgs.ContainsKey("qMode") && (UI.Tasks.QuickConvert.QualityMode)encArgs["qMode"].GetInt() != UI.Tasks.QuickConvert.QualityMode.Crf;
            string q = encArgs.ContainsKey("q") ? encArgs["q"] : QDefault.ToString();
            int br = encArgs.ContainsKey("bitrate") ? encArgs["bitrate"].GetInt() : 0;
            string preset = encArgs.ContainsKey("preset") ? encArgs["preset"] : Presets[PresetDefault];
            string pixFmt = encArgs.ContainsKey("pixFmt") ? encArgs["pixFmt"] : ColorFormats[ColorFormatDefault];
            string rc = vbr ? $"-b:v {br}k -minrate {br / 4}k -maxrate {br * 2}k -bufsize {br}k" : (q.GetInt() > 0 ? $"-b:v 0 -cq {q}" : "-tune lossless");
            string cust = encArgs.ContainsKey("custom") ? encArgs["custom"] : "";
            return new CodecArgs($"-c:v h264_nvenc {rc} -preset {preset} -pix_fmt {pixFmt} {cust}");
        }
    }
}
