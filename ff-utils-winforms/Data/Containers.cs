using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoCodec = Nmkoder.Data.Codecs.VideoCodec;
using AudioCodec = Nmkoder.Data.Codecs.AudioCodec;
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

        public static bool ContainerSupports(Container c, VideoCodec cv)
        {
            //Logger.Log($"{c} supports {vCodec}: {GetSupportedVideoCodecs(c).Contains(vCodec)}");
            return cv == VideoCodec.Copy || cv == VideoCodec.StripVideo || GetSupportedVideoCodecs(c).Contains(cv);
        }

        public static bool ContainerSupports(Container c, AudioCodec ca)
        {
            //Logger.Log($"{c} supports {aCodec}: {GetSupportedAudioCodecs(c).Contains(aCodec)}");
            return ca == AudioCodec.Copy || ca == AudioCodec.StripAudio || GetSupportedAudioCodecs(c).Contains(ca);
        }

        public static Container GetSupportedContainer(VideoCodec cv, AudioCodec ca)
        {
            if (ContainerSupports(Container.Mp4, cv) && ContainerSupports(Container.Mp4, ca))
                return Container.Mp4;

            if (ContainerSupports(Container.Webm, cv) && ContainerSupports(Container.Webm, ca))
                return Container.Webm;

            if (ContainerSupports(Container.Mov, cv) && ContainerSupports(Container.Mov, ca))
                return Container.Mov;

            return Container.Mkv;
        }
    }
}
