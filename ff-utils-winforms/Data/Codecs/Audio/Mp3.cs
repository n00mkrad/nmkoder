using Nmkoder.IO;
using System;
using System.Collections.Generic;

namespace Nmkoder.Data.Codecs
{
    class Mp3 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "MP3";
        public string[] Presets { get; }
        public int PresetDefault { get; }
        public string[] ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 32;
        public int QMax { get; } = 1280;
        public int QDefault { get; } = 320;
        public string QInfo { get; }
        public string PresetInfo { get; }

        public bool SupportsTwoPass { get; } = false;
		public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, Pass pass = Pass.OneOfOne, MediaFile mediaFile = null)
        {
            string bitrate = encArgs.ContainsKey("bitrate") ? encArgs["bitrate"] : "320k";
            string channels = encArgs.ContainsKey("ac") ? encArgs["ac"] : "2";
            return new CodecArgs($"-c:a libmp3lame -b:a {bitrate}k -ac {channels}");
        }
    }
}
