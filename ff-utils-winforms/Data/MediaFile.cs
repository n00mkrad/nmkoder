using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
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
        public FileInfo File;
        public string Ext;
        public string Title;
        public int StreamCount;
        public int TotalKbits;
        public List<Stream> AllStreams = new List<Stream>();
        public List<VideoStream> VideoStreams = new List<VideoStream>();
        public List<AudioStream> AudioStreams = new List<AudioStream>();
        public List<SubtitleStream> SubtitleStreams = new List<SubtitleStream>();
        public List<DataStream> DataStreams = new List<DataStream>();
        public bool Initialized = false;

        public MediaFile (string path)
        {
            File = new FileInfo(path);
            Ext = File.Extension.Split('.').Last();
        }

        public async Task Initialize (string path = null, bool progressBar = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    path = File.FullName;

                await LoadFormatInfo(path);
                AllStreams = await FfmpegUtils.GetStreams(path, progressBar, StreamCount);
                VideoStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Video).Select(x => (VideoStream)x).ToList();
                AudioStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x).ToList();
                SubtitleStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x).ToList();
                DataStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Data).Select(x => (DataStream)x).ToList();
                Logger.Log($"Loaded and sorted streams for {File.Name}", true);
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
            StreamCount = await FfmpegUtils.GetStreamCount(path);
            TotalKbits = (await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "bit_rate")).GetInt() / 1024;
        }
    }
}
