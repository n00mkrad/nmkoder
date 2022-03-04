using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.Data.Ui
{
    public class StreamListEntry
    {
        public MediaFile MediaFile;
        public Stream Stream;

        public string Title { get { return Stream.Title; } }
        public string TitleEdited { get; set; } = "";
        public string Language { get { return Stream.Language; } }
        public string LanguageEdited { get; set; } = "";

        const int maxTitleChars = 50;
        const int maxLangChars = 7;

        public override string ToString()
        {
            return GetString(true, true);
        }

        public string GetString(bool detailed, bool streamNumInBrackets)
        {
            string codec = Aliases.GetNicerCodecName(Stream.Codec);
            int fileIndex = GetFileIndex();
            int streamNumInt = (Config.GetBool(Config.Key.UseZeroIndexedStreams) ? Stream.Index : Stream.Index + 1);
            string streamNum = streamNumInBrackets ? $"[#{streamNumInt.ToString().PadLeft(2, '0')}]:" : $"#{streamNumInt.ToString().PadLeft(2, '0')}:";

            if (Stream.Type == Stream.StreamType.Video)
            {
                VideoStream vidStr = (VideoStream)Stream;
                List<string> items = new List<string>();

                items.Add(detailed && vidStr.Kbits > 0 ? $"({codec} at {FormatUtils.Bitrate(vidStr.Kbits)})" : $"({codec})");

                if (detailed)
                {
                    if (!string.IsNullOrWhiteSpace(vidStr.Language) && vidStr.Language.ToLower() != "und")
                        items.Add(vidStr.Language.ToUpper().Trunc(maxLangChars));

                    items.Add($"{vidStr.Resolution.Width}x{vidStr.Resolution.Height}");
                    items.Add($"{vidStr.Rate.GetString("0.###")} FPS");

                    if (MediaFile.IsDirectory)
                    {
                        List<string> exts = File.ReadAllLines(MediaFile.ImportPath).Select(x => Path.GetExtension(x.Remove("file '").Remove("'"))).ToList();
                        int formatsCount = exts.Distinct().Count();

                        if (formatsCount < 1)
                            items.Add($"({MediaFile.FileCount} Files)");
                        else if (formatsCount == 1)
                            items.Add($"({MediaFile.FileCount} {exts.Distinct().FirstOrDefault().ToUpper().Remove(".")} Files)");
                        else if (formatsCount > 1)
                            items.Add($"({MediaFile.FileCount} Files, {formatsCount} Formats)");
                    }
                    else
                    {
                        items.Add($"'{vidStr.Title.Trunc(maxTitleChars - 10)}'");
                    }
                }

                return $"{streamNum} Video {string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x) && x != "''"))}";
            }

            if (Stream.Type == Stream.StreamType.Audio)
            {
                AudioStream audStr = (AudioStream)Stream;
                List<string> items = new List<string>();

                items.Add(detailed && audStr.Kbits > 0 ? $"({codec} at {FormatUtils.Bitrate(audStr.Kbits)})" : $"({codec})");

                if (detailed)
                {
                    items.Add(audStr.Language.ToUpper().Trunc(maxLangChars));
                    items.Add(audStr.Layout.ToTitleCase().Split('(')[0]);
                    items.Add($"'{audStr.Title.Trunc(maxTitleChars)}'");
                }

                return $"{streamNum} Audio {string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x) && x != "''"))}";
            }

            if (Stream.Type == Stream.StreamType.Subtitle)
            {
                SubtitleStream subStr = (SubtitleStream)Stream;
                List<string> items = new List<string>();

                items.Add($"({codec})");

                if (detailed)
                {
                    items.Add(subStr.Language.ToUpper().Trunc(maxLangChars));
                    items.Add($"'{subStr.Title.Trunc(maxTitleChars)}'");
                }

                return $"{streamNum} Subtitles {string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x) && x != "''"))}";
            }

            if (Stream.Type == Stream.StreamType.Data)
            {
                return $"{streamNum} Data ({codec})";
            }

            if (Stream.Type == Stream.StreamType.Attachment)
            {
                AttachmentStream attStr = (AttachmentStream)Stream;
                List<string> items = new List<string>();

                items.Add($"({codec})");

                if (detailed)
                    items.Add(attStr.Filename.Trunc(maxTitleChars));

                return $"{streamNum} Attachment {string.Join(" - ", items.Where(x => !string.IsNullOrWhiteSpace(x) && x != "''"))}";
            }

            if (Stream.Type == Stream.StreamType.Unknown)
            {
                return $"{streamNum} Unknown ({codec})";
            }

            return streamNum;
        }

        int GetFileIndex ()
        {
            for(int i = 0; i < Program.mainForm.fileListBox.Items.Count; i++)
            {
                if (((FileListEntry)Program.mainForm.fileListBox.Items[i].Tag).File.ImportPath == MediaFile.ImportPath)
                    return i;
            }

            return -1;
        }

        public StreamListEntry(MediaFile mediaFile, Stream stream)
        {
            MediaFile = mediaFile;
            Stream = stream;
        }
    }
}
