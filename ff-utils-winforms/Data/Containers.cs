using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VC = Nmkoder.Data.CodecUtils.VideoCodec;
using AC = Nmkoder.Data.CodecUtils.AudioCodec;
using SC = Nmkoder.Data.CodecUtils.SubtitleCodec;
using Nmkoder.IO;
using Nmkoder.Data.Codecs;

namespace Nmkoder.Data
{
    class Containers
    {
        public enum Container { Mp4, Mkv, Webm, Mov, M4a, Ogg };

        public static VC[] GetSupportedVideoCodecs (Container c)
        {
            if (c == Container.Mp4)
                return new VC[] { VC.H264, VC.H265, VC.H264Nvenc, VC.H265Nvenc, VC.Av1 };

            if (c == Container.Mkv)
                return new VC[] { VC.H264, VC.H265, VC.H264Nvenc, VC.H265Nvenc, VC.Vp9, VC.Av1, VC.Png, VC.Jpg };

            if (c == Container.Webm)
                return new VC[] { VC.Vp9, VC.Av1 };

            if (c == Container.Mov)
                return new VC[] { VC.H264, VC.H265, VC.H264Nvenc, VC.H265Nvenc };

            return new VC[0];
        }

        public static AC[] GetSupportedAudioCodecs(Container c)
        {
            if (c == Container.Mp4)
                return new AC[] { AC.Aac, AC.Mp3 };

            if (c == Container.Mkv)
                return new AC[] { AC.Aac, AC.Opus, AC.Mp3, AC.Flac };

            if (c == Container.Webm)
                return new AC[] { AC.Opus };

            if (c == Container.Mov)
                return new AC[] { AC.Aac, AC.Mp3 };

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

        // public static bool ContainerSupports(Container c, VC cv)
        // {
        //     string name = CodecUtils.GetCodec(cv).Name;
        //     bool s = name ==  VC.CopyVideo.ToString() || name == VC.StripVideo.ToString() || GetSupportedVideoCodecs(c).Select(x => x.ToString()).Contains(name);
        //     //Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {cv.ToString().ToUpper()}!", true);
        //     return s;
        // }

        public static bool ContainerSupports(Container c, IEncoder enc)
        {
            string name = enc.Name;

            if(enc.Type == Streams.Stream.StreamType.Video)
                return name == VC.CopyVideo.ToString() || name == VC.StripVideo.ToString() || GetSupportedVideoCodecs(c).Select(x => x.ToString()).Contains(name);

            if (enc.Type == Streams.Stream.StreamType.Audio)
                return name == AC.CopyAudio.ToString() || name == AC.StripAudio.ToString() || GetSupportedAudioCodecs(c).Select(x => x.ToString()).Contains(name);

            if (enc.Type == Streams.Stream.StreamType.Subtitle)
                return name == SC.CopySubs.ToString() || name == SC.StripSubs.ToString() || GetSupportedSubtitleCodecs(c).Select(x => x.ToString()).Contains(name);

            return false;
        }

        // public static bool ContainerSupports(Container c, AC ca)
        // {
        //     bool s = ca == AC.CopyAudio || ca == AC.StripAudio || GetSupportedAudioCodecs(c).Contains(ca);
        //     //Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {ca.ToString().ToUpper()}!", true);
        //     return s;
        // }
        // 
        // public static bool ContainerSupports(Container c, SC cs)
        // {
        //     bool s = cs == SC.CopySubs || cs == SC.StripSubs || GetSupportedSubtitleCodecs(c).Contains(cs);
        //     Logger.Log($"{c.ToString().ToUpper()} {(s ? "supports" : "doesn't support")} {cs.ToString().ToUpper()}!", true);
        //     return s;
        // }

        public static Container GetSupportedContainer(IEncoder cv, IEncoder ca, IEncoder cs)
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

        public static string GetMuxingArgs(Container c)
        {
            if (c == Container.Mp4)
                return $"{(Config.GetBool(Config.Key.mp4Faststart) ? "-movflags +faststart" : "")}"; // Web Optimize

            if (c == Container.Mkv)
                return "-max_interleave_delta 0"; // Fix muxing bug

            if (c == Container.Webm)
                return "";

            if (c == Container.Mov)
                return "";

            return "";
        }
    }
}
