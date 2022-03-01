using Nmkoder.Data.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs.Subs
{
    #region Encode

    class MovText : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Subtitle;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Mov_Text (3GPP Timed Text) - For MP4/MOV";
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
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            return new CodecArgs($"-c:s mov_text");
        }
    }

    class Srt : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Subtitle;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "SRT (SubRip Text) - For MKV";
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
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            return new CodecArgs($"-c:s srt");
        }
    }

    class WebVtt : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Subtitle;
        public string Name { get; } = "WebVtt";
        public string FriendlyName { get; } = "WebVTT (Web Video Text Tracks) - For WEBM";
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
        public bool DoesNotEncode { get; } = false;
        public bool IsFixedFormat { get; } = false;
        public bool IsSequence { get; } = false;

        public CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null, Pass pass = Pass.OneOfOne)
        {
            return new CodecArgs($"-c:s webvtt");
        }
    }

    #endregion

    #region Mux

    class CopySubs : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Subtitle;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Copy Subtitles Without Re-Encoding";
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
            return new CodecArgs($"-c:s copy");
        }
    }

    class StripSubs : IEncoder
    {
        public Streams.Stream.StreamType Type { get; } = Streams.Stream.StreamType.Subtitle;
        public string Name { get { return GetType().Name; } }
        public string FriendlyName { get; } = "Disable (Strip Subtitles)";
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
            return new CodecArgs($"-sn");
        }
    }

    #endregion

}
