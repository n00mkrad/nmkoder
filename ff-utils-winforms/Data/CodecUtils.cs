using Nmkoder.Data.Codecs;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static string GetAudioArgsForEachStream(MediaFile mf, int baseBitrate, int overrideChannels, List<string> extraArgs = null)
        {
            List<string> args = new List<string>();
            List<AudioStream> allAudioStreams = Program.mainForm.streamListBox.Items.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x.Stream).ToList();
            List<AudioStream> checkedStreams = Program.mainForm.streamListBox.CheckedItems.OfType<MediaStreamListEntry>().Where(x => x.Stream.Type == Stream.StreamType.Audio).Select(x => (AudioStream)x.Stream).ToList();
            List<AudioConfigurationEntry> audioConf = TrackList.currentAudioConfig != null ? TrackList.currentAudioConfig.GetConfig(mf) : null;

            foreach (AudioStream s in checkedStreams)
            {
                int indexTotal = allAudioStreams.IndexOf(s);
                int indexChecked = checkedStreams.IndexOf(s);
                int ac = overrideChannels > 0 ? overrideChannels : s.Channels;

                if (audioConf != null)
                    ac = audioConf[indexTotal].ChannelCount;

                if (baseBitrate > 0)
                {
                    int kbps = (baseBitrate * MiscUtils.GetAudioBitrateMultiplier(ac)).RoundToInt();

                    if (audioConf != null)
                        kbps = audioConf[indexTotal].BitrateKbps;

                    args.Add($"-b:a:{indexChecked} {kbps}k");
                }

                args.Add($"-ac:a:{indexChecked} {ac}");

                if (extraArgs != null)
                {
                    foreach (var arg in extraArgs)
                    {
                        string[] split = arg.Split(' ');

                        if (split.Length == 2)
                            args.Add($"{split[0]}:{indexChecked} {split[1]}");
                    }
                }
            }

            return string.Join(" ", args);
        }

        public static string GetTilingArgs(Size resolution, string colArg, string rowArg)
        {
            int cols = 0;
            if (resolution.Width >= 1920) cols = 1;
            if (resolution.Width >= 3840) cols = 2;
            if (resolution.Width >= 7680) cols = 3;

            int rows = 0;
            if (resolution.Height >= 1600) cols = 1;
            if (resolution.Height >= 3200) cols = 2;
            if (resolution.Height >= 6400) cols = 3;

            return $"{colArg}{cols} {rowArg}{rows}";
        }
    }
}
