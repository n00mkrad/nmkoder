using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class EAc3 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return "E-AC-3"; } }
        public string FriendlyName { get; } = "E-AC-3 (Dolby Digital Plus)";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public string[] ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 64;
        public int QMax { get; } = 6144;
        public int QDefault { get; } = 256;
        public string QInfo { get; }
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
        public bool ForceTwoPass { get; } = false;
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string bitrate = encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : $"{QDefault}k";
            string channels = encArgs.ContainsKey("ac") ? encArgs["ac"] : "2";
            return new CodecArgs($"-c:a aac -aac_coder twoloop {CodecUtils.GetAudioArgsForEachStream(mediaFile, bitrate.GetInt(), channels.GetInt())}");
        }
    }
}
