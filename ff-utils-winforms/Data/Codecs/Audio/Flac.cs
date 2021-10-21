using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class Flac : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "FLAC (Free Lossless Audio Coding)";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public string[] ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; }
        public int QMax { get; }
        public int QDefault { get; }
        public string QInfo { get; }
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
		public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string channels = encArgs.ContainsKey("ac") ? encArgs["ac"] : "2";
            return new CodecArgs($"-c:a flac {CodecUtils.GetAudioArgsForEachStream(mediaFile, -1, channels.GetInt())}");
        }
    }
}
