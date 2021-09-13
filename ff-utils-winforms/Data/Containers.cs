using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VC = Nmkoder.Data.Codecs.VideoCodec;
using AC = Nmkoder.Data.Codecs.AudioCodec;
using SC = Nmkoder.Data.Codecs.SubtitleCodec;
using Nmkoder.IO;

namespace Nmkoder.Data
{
    class Containers
    {
        public enum Container { Mp4, Mkv, Webm, Mov, M4a, Ogg };

        public static VC[] GetSupportedVideoCodecs (Container c)
        {
            if (c == Container.Mp4)
                return new VC[] { VC.H264, VC.H265, VC.Av1 };

            if (c == Container.Mkv)
                return new VC[] { VC.H264, VC.H265, VC.Vp9, VC.Av1 };

            if (c == Container.Webm)
                return new VC[] { VC.Vp9, VC.Av1 };

            if (c == Container.Mov)
                return new VC[] { VC.H264, VC.H265 };

            return new VC[0];
        }

        public static AC[] GetSupportedAudioCodecs(Container c)
        {
            if (c == Container.Mp4)
                return new AC[] { AC.Aac, AC.Opus };

            if (c == Container.Mkv)
                return new AC[] { AC.Aac, AC.Opus };

            if (c == Container.Webm)
                return new AC[] { AC.Opus };

            if (c == Container.Mov)
                return new AC[] { AC.Aac };

            if (c == Container.M4a)
                return new AC[] { AC.Aac };

            if (c == Container.Ogg)
                return new AC[] { AC.Opus };

            return new AC[0];
        }

        public static SC[] GetSupportedSubtitleCodecs(Container c)
        {
            if (c == Container.Mp4)
                return new SC[] { SC.MovText, SC.Srt };

            if (c == Container.Mkv)
                return new SC[] { SC.MovText, SC.Srt, SC.WebVtt };

            if (c == Container.Webm)
                return new SC[] { SC.WebVtt };

            if (c == Container.Mov)
                return new SC[] { SC.MovText };

            return new SC[0];
        }

        public static bool ContainerSupports(Container c, VC cv)
        {
            bool s = cv == VC.Copy || cv == VC.StripVideo || GetSupportedVideoCodecs(c).Contains(cv);
            Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {cv.ToString().ToUpper()}!", true);
            return s;
        }

        public static bool ContainerSupports(Container c, AC ca)
        {
            bool s = ca == AC.Copy || ca == AC.StripAudio || GetSupportedAudioCodecs(c).Contains(ca);
            Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {ca.ToString().ToUpper()}!", true);
            return s;
        }

        public static bool ContainerSupports(Container c, SC cs)
        {
            bool s = cs == SC.Copy || cs == SC.StripSubs || GetSupportedSubtitleCodecs(c).Contains(cs);
            Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {cs.ToString().ToUpper()}!", true);
            return s;
        }

        public static Container GetSupportedContainer(VC cv, AC ca, SC cs)
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
