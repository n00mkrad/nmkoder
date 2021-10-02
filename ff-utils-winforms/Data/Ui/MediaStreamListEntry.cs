using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.Data.Ui
{
    class MediaStreamListEntry
    {
        public MediaFile MediaFile;
        public Stream Stream;
        public int FileIndex;

        public override string ToString()
        {
            string codec = FormatUtils.CapsIfShort(Stream.Codec, 5);
            const int maxChars = 50;
            string str = $"[File {FileIndex + 1} - Track {Stream.Index + 1}]:";

            if (Stream.Type == Stream.StreamType.Video)
            {
                VideoStream vs = (VideoStream)Stream;
                string codecStr = vs.Kbits > 0 ? $"{codec} at {FormatUtils.Bitrate(vs.Kbits)}" : codec;
                string fileCountStr = "";

                if (MediaFile.IsDirectory)
                {
                    List<string> exts = File.ReadAllLines(MediaFile.TruePath).Select(x => x.Remove("file '").Remove("'").Split('.').LastOrDefault()).ToList();
                    int formatsCount = exts.Select(x => x).Distinct().Count();
                    fileCountStr = formatsCount > 1 ? $" ({MediaFile.FileCount} Files, {formatsCount} Formats)" : $" ({MediaFile.FileCount} Files)";
                }

                return $"{str} Video ({codecStr}) - {vs.Resolution.Width}x{vs.Resolution.Height} - {vs.Rate.GetString()} FPS{fileCountStr}";
            }

            if (Stream.Type == Stream.StreamType.Audio)
            {
                AudioStream @as = (AudioStream)Stream;
                string title = string.IsNullOrWhiteSpace(@as.Title.Trim()) ? " " : $" - {@as.Title.Trunc(maxChars)} ";
                string codecStr = @as.Kbits > 0 ? $"{codec} at {FormatUtils.Bitrate(@as.Kbits)}" : codec;
                string lang = string.IsNullOrWhiteSpace(@as.Language.Trim()) ? " " : $" - {FormatUtils.CapsIfShort(@as.Language, 4).Trunc(maxChars)} ";
                return $"{str} Audio ({codecStr}){title}- {@as.Layout.ToTitleCase()}{lang}";
            }

            if (Stream.Type == Stream.StreamType.Subtitle)
            {
                SubtitleStream ss = (SubtitleStream)Stream;
                string lang = string.IsNullOrWhiteSpace(ss.Language.Trim()) ? " " : $" - {FormatUtils.CapsIfShort(ss.Language, 4).Trunc(maxChars)} ";
                string ttl = string.IsNullOrWhiteSpace(ss.Title.Trim()) ? " " : $" - {FormatUtils.CapsIfShort(ss.Title, 4).Trunc(maxChars)} ";
                return $"{str} Subtitles ({codec}){lang}{ttl}";
            }

            if (Stream.Type == Stream.StreamType.Data)
            {
                return $"{str} Data ({codec})";
            }

            if (Stream.Type == Stream.StreamType.Attachment)
            {
                return $"{str} Attachment ({codec})";
            }

            if (Stream.Type == Stream.StreamType.Unknown)
            {
                return $"{str} Unknown ({codec})";
            }

            return str;
        }

        public MediaStreamListEntry(MediaFile mediaFile, Stream stream, int fileIndex)
        {
            MediaFile = mediaFile;
            Stream = stream;
            FileIndex = fileIndex;
        }
    }
}
