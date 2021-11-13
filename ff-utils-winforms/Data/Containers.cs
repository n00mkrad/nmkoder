using System;
using System.Collections.Generic;
using System.Linq;
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
                return new VC[] { VC.Libx264, VC.Libx265, VC.H264Nvenc, VC.H265Nvenc, VC.LibSvtAv1 };

            if (c == Container.Mkv)
                return new VC[] { VC.Libx264, VC.Libx265, VC.H264Nvenc, VC.H265Nvenc, VC.LibVpx, VC.LibSvtAv1, VC.Png, VC.Jpg };

            if (c == Container.Webm)
                return new VC[] { VC.LibVpx, VC.LibSvtAv1 };

            if (c == Container.Mov)
                return new VC[] { VC.Libx264, VC.Libx265, VC.H264Nvenc, VC.H265Nvenc };

            return new VC[0];
        }

        public static AC[] GetSupportedAudioCodecs(Container c)
        {
            if (c == Container.Mp4)
                return new AC[] { AC.Aac, AC.Mp3 };

            if (c == Container.Mkv)
                return new AC[] { AC.Aac, AC.Opus, AC.Vorbis, AC.Mp3, AC.Flac };

            if (c == Container.Webm)
                return new AC[] { AC.Opus, AC.Vorbis };

            if (c == Container.Mov)
                return new AC[] { AC.Aac, AC.Mp3 };

            if (c == Container.M4a)
                return new AC[] { AC.Aac };

            if (c == Container.Ogg)
                return new AC[] { AC.Opus, AC.Vorbis };

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

        public static bool ContainerSupports(Container c, IEncoder enc)
        {
            string name = enc.Name;
            //Logger.Log($"ContainerSupports - Container {c}, IEncoder.Name {enc.Name} - Container supports {string.Join("/", GetSupportedVideoCodecs(c).Select(x => x.ToString()))}");

            if(enc.Type == Streams.Stream.StreamType.Video)
                return name == VC.CopyVideo.ToString() || name == VC.StripVideo.ToString() || GetSupportedVideoCodecs(c).Select(x => x.ToString()).Contains(name);

            if (enc.Type == Streams.Stream.StreamType.Audio)
                return name == AC.CopyAudio.ToString() || name == AC.StripAudio.ToString() || GetSupportedAudioCodecs(c).Select(x => x.ToString()).Contains(name);

            if (enc.Type == Streams.Stream.StreamType.Subtitle)
                return name == SC.CopySubs.ToString() || name == SC.StripSubs.ToString() || GetSupportedSubtitleCodecs(c).Select(x => x.ToString()).Contains(name);

            return false;
        }

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
                return "-default_mode infer_no_subs -max_interleave_delta 0"; // -default_mode: Disable first sub track being set as default, -max_interleave_delta: Fix audio muxing problems

            if (c == Container.Webm)
                return "";

            if (c == Container.Mov)
                return "";

            return "";
        }
    }
}
