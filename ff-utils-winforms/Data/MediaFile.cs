using Nmkoder.Data.Streams;
using Nmkoder.Extensions;
using Nmkoder.Forms;
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
    public class MediaFile
    {
        public bool IsDirectory;
        public FileInfo FileInfo;
        public DirectoryInfo Directory;
        public int FileCount;
        public string Name;
        public string SourcePath;
        public string ImportPath;
        public string Format;
        public string Title;
        public string Language;
        public Fraction? InputRate = null;
        public long DurationMs;
        public int StreamCount;
        public int TotalKbits;
        public long Size;
        public List<Stream> AllStreams = new List<Stream>();
        public List<VideoStream> VideoStreams = new List<VideoStream>();
        public List<AudioStream> AudioStreams = new List<AudioStream>();
        public List<SubtitleStream> SubtitleStreams = new List<SubtitleStream>();
        public List<DataStream> DataStreams = new List<DataStream>();
        public List<AttachmentStream> AttachmentStreams = new List<AttachmentStream>();
        public VideoColorData ColorData = null;
        public long CreationTime;
        public bool Initialized = false;
        public bool SequenceInitialized = false;

        public MediaFile(string path, bool requestFpsInputIfUnset = true)
        {
            CreationTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds; // Unix Timestamp as UID

            if (IoUtils.IsPathDirectory(path))
            {
                IsDirectory = true;
                Directory = new DirectoryInfo(path);
                Name = Directory.Name;
                SourcePath = Directory.FullName;
                Format = "Folder";

                if(requestFpsInputIfUnset && InputRate == null)
                {
                    PromptForm form = new PromptForm("Enter Frame Rate", $"Please enter a frame rate to use for the image sequence '{Name.Trunc(80)}'.", "30");
                    form.ShowDialog();
                    InputRate = new Fraction(form.EnteredText);
                }
            }
            else
            {
                FileInfo = new FileInfo(path);
                Name = FileInfo.Name;
                SourcePath = FileInfo.FullName;
                ImportPath = FileInfo.FullName;
                Format = FileInfo.Extension.Remove(".").ToUpper();
                FileCount = 1;
                InputRate = new Fraction(-1, 1);
            }

            Size = GetSize();
        }

        public async Task InitializeSequence()
        {
            try
            {
                if (SequenceInitialized) return;

                Logger.Log($"Preparing image sequence...");
                Logger.Log($"MediaFile {Name}: Preparing image sequence", true);
                string seqPath = Path.Combine(Paths.GetFrameSeqPath(), CreationTime.ToString(), "frames.concat");
                string chosenExt = IoUtils.GetUniqueExtensions(SourcePath).FirstOrDefault();
                int fileCount = FfmpegUtils.CreateConcatFile(SourcePath, seqPath, new List<string> { chosenExt });
                ImportPath = seqPath;
                FileCount = fileCount;
                Logger.Log($"Created concat file with {fileCount} files.", true);
                SequenceInitialized = true;
            }
            catch (Exception e)
            {
                Logger.Log($"Error preparing frame sequence: {e.Message}\n{e.StackTrace}");
                FileCount = 0;
            }
        }

        public async Task Initialize(bool progressBar = true)
        {
            Logger.Log($"MediaFile {Name}: Initializing", true);

            try
            {
                if(IsDirectory && !SequenceInitialized)
                    await InitializeSequence();

                await LoadFormatInfo(ImportPath);
                AllStreams = await FfmpegUtils.GetStreams(ImportPath, progressBar, StreamCount, (Fraction)InputRate);
                VideoStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Video).Select(x => (VideoStream)x).ToList();
                AudioStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x).ToList();
                SubtitleStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Subtitle).Select(x => (SubtitleStream)x).ToList();
                DataStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Data).Select(x => (DataStream)x).ToList();
                AttachmentStreams = AllStreams.Where(x => x.Type == Stream.StreamType.Attachment).Select(x => (AttachmentStream)x).ToList();
                Logger.Log($"Loaded and sorted streams for {Name}", true);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialized MediaFile: {e.Message}", true);
            }

            Initialized = true;
        }

        private async Task LoadFormatInfo(string path)
        {
            Title = await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "TAG:title");
            Language = await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "TAG:language");
            DurationMs = (await FfmpegCommands.GetDurationMs(path));
            StreamCount = await FfmpegUtils.GetStreamCount(path);
            TotalKbits = (await GetVideoInfo.GetFfprobeInfoAsync(path, GetVideoInfo.FfprobeMode.ShowFormat, "bit_rate")).GetInt() / 1000;
        }

        public string GetName()
        {
            if (IsDirectory)
                return Directory.Name;
            else
                return FileInfo.Name;
        }

        public string GetPath()
        {
            if (IsDirectory)
                return Directory.FullName;
            else
                return FileInfo.FullName;
        }

        public long GetSize()
        {
            if (IsDirectory)
                return IoUtils.GetDirSize(GetPath(), true);
            else
                return FileInfo.Length;
        }

        public override string ToString()
        {
            return $"{GetName()} ({FormatUtils.Bytes(Size)})";
        }
    }
}
