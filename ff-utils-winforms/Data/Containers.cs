using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoCodec = Nmkoder.Data.Codecs.VideoCodec;
using AudioCodec = Nmkoder.Data.Codecs.AudioCodec;
using SubtitleCodec = Nmkoder.Data.Codecs.SubtitleCodec;
using Nmkoder.IO;

namespace Nmkoder.Data
{
    class Containers
    {
        public enum Container { Mp4, Mkv, Webm, Mov, M4a, Ogg };

        public static VideoCodec[] GetSupportedVideoCodecs (Container c)
        {
            if (c == Container.Mp4)
                return new VideoCodec[] { VideoCodec.H264, VideoCodec.H265, VideoCodec.Av1 };

            if (c == Container.Mkv)
                return new VideoCodec[] { VideoCodec.H264, VideoCodec.H265, VideoCodec.Vp9, VideoCodec.Av1 };

            if (c == Container.Webm)
                return new VideoCodec[] { VideoCodec.Vp9, VideoCodec.Av1 };

            if (c == Container.Mov)
                return new VideoCodec[] { VideoCodec.H264, VideoCodec.H265 };

            return new VideoCodec[0];
        }

        public static AudioCodec[] GetSupportedAudioCodecs(Container c)
        {
            if (c == Container.Mp4)
                return new AudioCodec[] { AudioCodec.Aac, AudioCodec.Opus };

            if (c == Container.Mkv)
                return new AudioCodec[] { AudioCodec.Aac, AudioCodec.Opus };

            if (c == Container.Webm)
                return new AudioCodec[] { AudioCodec.Opus };

            if (c == Container.Mov)
                return new AudioCodec[] { AudioCodec.Aac };

            if (c == Container.M4a)
                return new AudioCodec[] { AudioCodec.Aac };

            if (c == Container.Ogg)
                return new AudioCodec[] { AudioCodec.Opus };

            return new AudioCodec[0];
        }

        public static SubtitleCodec[] GetSupportedSubtitleCodecs(Container c)
        {
            if (c == Container.Mp4)
                return new SubtitleCodec[] { SubtitleCodec.MovText, SubtitleCodec.Srt };

            if (c == Container.Mkv)
                return new SubtitleCodec[] { SubtitleCodec.MovText, SubtitleCodec.Srt, SubtitleCodec.WebVtt };

            if (c == Container.Webm)
                return new SubtitleCodec[] { SubtitleCodec.WebVtt };

            if (c == Container.Mov)
                return new SubtitleCodec[] { SubtitleCodec.MovText };

            return new SubtitleCodec[0];
        }

        public static bool ContainerSupports(Container c, VideoCodec cv)
        {
            bool s = cv == VideoCodec.Copy || cv == VideoCodec.StripVideo || GetSupportedVideoCodecs(c).Contains(cv);
            Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {cv.ToString().ToUpper()}!", true);
            return s;
        }

        public static bool ContainerSupports(Container c, AudioCodec ca)
        {
            bool s = ca == AudioCodec.Copy || ca == AudioCodec.StripAudio || GetSupportedAudioCodecs(c).Contains(ca);
            Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {ca.ToString().ToUpper()}!", true);
            return s;
        }

        public static bool ContainerSupports(Container c, SubtitleCodec cs)
        {
            bool s = cs == SubtitleCodec.Copy || cs == SubtitleCodec.StripSubs || GetSupportedSubtitleCodecs(c).Contains(cs);
            Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {cs.ToString().ToUpper()}!", true);
            return s;
        }

        public static Container GetSupportedContainer(VideoCodec cv, AudioCodec ca, SubtitleCodec cs)
        {
            if (ContainerSupports(Container.Mp4, cv) && ContainerSupports(Container.Mp4, ca) && ContainerSupports(Container.Mp4, cs))
                return Container.Mp4;

            if (ContainerSupports(Container.Mkv, cv) && ContainerSupports(Container.Mkv, ca) && ContainerSupports(Container.Mkv, cs))
                return Container.Mkv;

            if (ContainerSupports(Container.Webm, cv) && ContainerSupports(Container.Webm, ca) && ContainerSupports(Container.Webm, cs))
                return Container.Webm;

            if (ContainerSupports(Container.Mov, cv) && ContainerSupports(Container.Mov, ca) && ContainerSupports(Container.Mov, cs))
                return Container.Mov;

            return Container.Mkv;
        }
    }
}
