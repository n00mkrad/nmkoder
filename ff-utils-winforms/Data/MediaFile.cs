using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.Data
{
    class MediaFile
    {
        public bool isDirectory;
        private FileInfo File;
        private DirectoryInfo Directory;
        public string Name;
        public string Path;
        public string Format;
        public string Title;
        public string Language;
        public long DurationMs;
        public int StreamCount;
        public int TotalKbits;
        public long Size;
        public List<Stream> AllStreams = new List<Stream>();
        public List<VideoStream> VideoStreams = new List<VideoStream>();
        public List<AudioStream> AudioStreams = new List<AudioStream>();
        public List<SubtitleStream> SubtitleStreams = new List<SubtitleStream>();
        public List<DataStream> DataStreams = new List<DataStream>();
        public bool Initialized = false;

        public MediaFile (string path)
        {
            if (IoUtils.IsPathDirectory(path))
            {
                isDirectory = true;
                Directory = new DirectoryInfo(path);
                Name = Directory.Name;
                Path = Directory.FullName;
                Format = "Folder";
            }
            else
            {
                File = new FileInfo(path);
                Name = File.Name;
                Path = File.FullName;
                Format = File.Extension.Remove(".").ToUpper();
            }

            Size = GetSize();
        }

        public async Task Initialize (bool progressBar = true)
        {
            try
            {
                await LoadFormatInfo(Path);
                AllStreams = await FfmpegUtils.GetStreams(Path, progressBar, StreamCount);
                VideoStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Video).Select(x => (VideoStream)x).ToList();
                AudioStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x).ToList();
                SubtitleStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x).ToList();
                DataStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Data).Select(x => (DataStream)x).ToList();
                Logger.Log($"Loaded and sorted streams for {Name}", true);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialized MediaFile: {e.Message}", true);
            }

            Initialized = true;
        }

        private async Task LoadFormatInfo (string path)
        {
            Title = await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "TAG:title");
            Language = await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "TAG:language");
            DurationMs = FfmpegCommands.GetDurationMs(path);
            StreamCount = await FfmpegUtils.GetStreamCount(path);
            TotalKbits = (await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "bit_rate")).GetInt() / 1024;
        }

        public string GetName ()
        {
            if (isDirectory)
                return Directory.Name;
            else
                return File.Name;
        }

        public string GetPath ()
        {
            if (isDirectory)
                return Directory.FullName;
            else
                return File.FullName;
        }

        public long GetSize ()
        {
            if (isDirectory)
                return IoUtils.GetDirSize(GetPath(), true);
            else
                return File.Length;
        }

        public override string ToString()
        {
            return $"{GetName()} ({FormatUtils.Bytes(Size)})";
        }
    }
}
