using Nmkoder.Data.Streams;
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
        public int TotalBitrate;
        public List<Stream> AllStreams = new List<Stream>();
        public List<VideoStream> VideoStreams = new List<VideoStream>();
        public List<AudioStream> AudioStreams = new List<AudioStream>();
        public List<SubtitleStream> SubtitleStreams = new List<SubtitleStream>();
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

                AllStreams = await FfmpegUtils.GetStreams(path, progressBar);
                VideoStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Video).Select(x => (VideoStream)x).ToList();
                AudioStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x).ToList();
                SubtitleStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x).ToList();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialized MediaFile: {e.Message}");
            }

            Initialized = true;
        }
    }
}
