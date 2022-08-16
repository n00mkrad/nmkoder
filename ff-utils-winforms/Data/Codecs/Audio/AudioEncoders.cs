using Nmkoder.Data.Colors;
using Nmkoder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs.Audio
{
    #region Encode

    class Aac : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "AAC (Advanced Audio Coding)";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 8;
        public int QMax { get; } = 640;
        public int QDefault { get; } = 160;
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

    class Eac3 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return "E-AC-3"; } }
        public string FriendlyName { get; } = "E-AC-3 (Dolby Digital Plus)";
        public string[] Presets { get; } = new string[] { };
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 64;
        public int QMax { get; } = 1536;
        public int QDefault { get; } = 224;
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
            return new CodecArgs($"-c:a eac3 {CodecUtils.GetAudioArgsForEachStream(mediaFile, bitrate.GetInt(), channels.GetInt())}");
        }
    }

    class Flac : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "FLAC (Free Lossless Audio Coding)";
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
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            string channels = encArgs.ContainsKey("ac") ? encArgs["ac"] : "2";
            return new CodecArgs($"-c:a flac {CodecUtils.GetAudioArgsForEachStream(mediaFile, -1, channels.GetInt())}");
        }
    }

    class Vorbis : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Vorbis";
        public string[] Presets { get; }
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 32;
        public int QMax { get; } = 480;
        public int QDefault { get; } = 160;
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
            List<string> extraArgs = new List<string>();
            return new CodecArgs($"-c:a libvorbis {CodecUtils.GetAudioArgsForEachStream(mediaFile, bitrate.GetInt(), channels.GetInt(), extraArgs)}");
        }
    }

    class Mp3 : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "MP3";
        public string[] Presets { get; }
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 32;
        public int QMax { get; } = 1280;
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
            return new CodecArgs($"-c:a libmp3lame {CodecUtils.GetAudioArgsForEachStream(mediaFile, bitrate.GetInt(), channels.GetInt())}");
        }
    }

    class Opus : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Opus";
        public string[] Presets { get; }
        public int PresetDefault { get; }
        public List<PixelFormats> ColorFormats { get; }
        public int ColorFormatDefault { get; }
        public int QMin { get; } = 8;
        public int QMax { get; } = 640;
        public int QDefault { get; } = 128;
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
            List<string> extraArgs = new List<string>();
            extraArgs.Add("-mapping_family 1");
            return new CodecArgs($"-c:a libopus {CodecUtils.GetAudioArgsForEachStream(mediaFile, bitrate.GetInt(), channels.GetInt(), extraArgs)}");
        }
    }

    #endregion

    #region Mux

    class StripAudio : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Disable (Strip Audio)";
        public string[] Presets { get; }
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
            return new CodecArgs($"-an");
        }
    }

    class CopyAudio : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Audio;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Copy Audio Without Re-Encoding";
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
            return new CodecArgs($"-c:a copy");
        }
    }

    #endregion
}
