using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    public class UniqueMetadataEntry
    {
        public string SourceFilePath { get; set; }
        public long SourceFileSize { get; set; }
        public Streams.Stream.StreamType Type { get; set; }
        public int StreamIndex { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }

        public UniqueMetadataEntry(string path, Streams.Stream.StreamType type, int streamIndex)
        {
            FileInfo f = new FileInfo(path);
            SourceFileSize = f.Length;
            Type = type;
            StreamIndex = streamIndex;
        }

        public UniqueMetadataEntry(string path, Streams.Stream.StreamType type, int streamIndex, string title, string lang)
        {
            FileInfo f = new FileInfo(path);
            SourceFileSize = f.Length;
            Type = type;
            StreamIndex = streamIndex;
            Title = title;
            Language = lang;
        }

        public string GetPseudoHash ()
        {
            return $"{SourceFilePath}-{SourceFileSize}-{StreamIndex}";
        }
    }
}
