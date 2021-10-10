using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Codecs
{
    interface IEncoder
    {
        Streams.Stream.StreamType Type { get; }
        string Name { get; }
        string FriendlyName { get; }
        string[] Presets { get; }
        int PresetDefault { get; }
        string[] ColorFormats { get; }
        int ColorFormatDefault { get; }
        int QMin { get; }
        int QMax { get; }
        int QDefault { get; }
        string QInfo { get; }
        string PresetInfo { get; }

        bool DoesNotEncode { get; }
        bool IsFixedFormat { get; }
        bool IsSequence { get; }

        CodecArgs GetArgs(Dictionary<string, string> encArgs = null, MediaFile mediaFile = null);
    }
}
