using Nmkoder.Data.Codecs;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class CodecUtils
    {
        //public enum CodecType { Video, AnimImage, Image, Audio }

        public enum Av1anCodec { AomAv1, SvtAv1, Vpx, X265 };
        public enum VideoCodec { CopyVideo, StripVideo, Libx264, Libx265, H264Nvenc, H265Nvenc, LibVpx, LibSvtAv1, Gif, Png, Jpg };
        public enum AudioCodec { CopyAudio, StripAudio, Aac, Opus, Mp3, Flac };
        public enum SubtitleCodec { CopySubs, StripSubs, MovText, Srt, WebVtt };

        public static IEncoder GetCodec(VideoCodec c)
        {
            if (c == VideoCodec.StripVideo) return new StripVideo();
            if (c == VideoCodec.CopyVideo) return new CopyVideo();
            if (c == VideoCodec.Libx264) return new Libx264();
            if (c == VideoCodec.H264Nvenc) return new H264Nvenc();
            if (c == VideoCodec.Libx265) return new Libx265();
            if (c == VideoCodec.H265Nvenc) return new H265Nvenc();
            if (c == VideoCodec.LibVpx) return new LibVpx();
            if (c == VideoCodec.LibSvtAv1) return new LibSvtAv1();
            if (c == VideoCodec.Gif) return new Gif();
            if (c == VideoCodec.Png) return new Png();
            if (c == VideoCodec.Jpg) return new Jpg();
            return null;
        }

        public static IEncoder GetCodec(Av1anCodec c)
        {
            if (c == Av1anCodec.AomAv1) return new AomAv1();
            if (c == Av1anCodec.SvtAv1) return new SvtAv1();
            if (c == Av1anCodec.Vpx) return new Vpx();
            if (c == Av1anCodec.X265) return new X265();
            return null;
        }

        public static IEncoder GetCodec(AudioCodec c)
        {
            if (c == AudioCodec.StripAudio) return new StripAudio();
            if (c == AudioCodec.CopyAudio) return new CopyAudio();
            if (c == AudioCodec.Aac) return new Aac();
            if (c == AudioCodec.Opus) return new Opus();
            if (c == AudioCodec.Mp3) return new Mp3();
            if (c == AudioCodec.Flac) return new Flac();
            return null;
        }

        public static IEncoder GetCodec(SubtitleCodec c)
        {
            if (c == SubtitleCodec.StripSubs) return new StripSubs();
            if (c == SubtitleCodec.CopySubs) return new CopySubs();
            if (c == SubtitleCodec.MovText) return new MovText();
            if (c == SubtitleCodec.Srt) return new Srt();
            if (c == SubtitleCodec.WebVtt) return new WebVtt();
            return null;
        }

        public static string GetKeyIntArg(MediaFile mediaFile, int intervalSeconds, string arg = "-g ")
        {
            if (mediaFile == null || mediaFile.VideoStreams.Count < 1)
                return "";

            int keyInt = ((float)(mediaFile?.VideoStreams.FirstOrDefault().Rate.GetFloat() * intervalSeconds)).RoundToInt();
            return keyInt >= 24 ? $"{arg}{keyInt}" : "";
        }

        public static string GetAudioArgsForEachStream(MediaFile mf, int baseBitrate, int overrideChannels)
        {
            List<string> args = new List<string>();

            List<Streams.Stream> checkedStreams = Program.mainForm.streamListBox.CheckedItems.OfType<Ui.MediaStreamListEntry>().Select(x => x.Stream).ToList();
            List<Streams.AudioStream> streams = checkedStreams.Where(x => x.Type == Streams.Stream.StreamType.Audio).OfType<Streams.AudioStream>().ToList();

            foreach (Streams.AudioStream s in streams)
            {
                int audioIdx = mf.AudioStreams.IndexOf(s);
                int ac = overrideChannels > 0 ? overrideChannels : s.Channels;
                int kbps = (baseBitrate * MiscUtils.GetAudioBitrateMultiplier(ac)).RoundToInt();
                args.Add($"-b:a:{audioIdx} {kbps}k -ac:{audioIdx} {ac}");
            }

            return string.Join(" ", args);
        }
    }
}
