using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using static Nmkoder.UI.Tasks.QuickConvertUi;

namespace Nmkoder.UI.Tasks
{
    class QuickConvert
    {
        public enum Backend { Ffmpeg, Av1an }
        public Backend currentBackend = Backend.Ffmpeg;

        public static void Init()
        {
            QuickConvertUi.Init();
        }

        public static async Task Run ()
        {
            Program.mainForm.SetWorking(true);
            string args = "";
            
            try
            {
                Codecs.VideoCodec vCodec = GetCurrentCodecV();
                Codecs.AudioCodec aCodec = GetCurrentCodecA();
                Codecs.SubtitleCodec sCodec = GetCurrentCodecS();
                string inFiles = TrackList.GetInputFilesString();
                string outPath = GetOutPath(vCodec);
                string map = TrackList.GetMapArgs();
                CodecArgs codecArgs = Codecs.GetArgs(vCodec, GetVideoArgsFromUi(), TrackList.current);
                string v = codecArgs.Arguments;
                string vf = await GetVideoFilterArgs(vCodec, codecArgs);
                string a = Codecs.GetArgs(aCodec, GetAudioArgsFromUi());
                string s = Codecs.GetArgs(sCodec);
                string meta = GetMetadataArgs();
                string custIn = Program.mainForm.customArgsInBox.Text.Trim();
                string custOut = Program.mainForm.customArgsOutBox.Text.Trim();
                string muxing = GetMuxingArgsFromUi();
                args = $"{custIn} {inFiles} {map} {v} {vf} {a} {s} {meta} {custOut} {muxing} {outPath.Wrap()}";
            }
            catch(Exception e)
            {
                Logger.Log($"Error creating FFmpeg command: {e.Message}\n{e.StackTrace}");
                return;
            }
            
            Logger.Log($"Running:\nffmpeg {args}", true, false, "ffmpeg");

            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.OnlyLastLine, AvProcess.TaskType.Encode, true);

            Program.mainForm.SetWorking(false);
        }

        
    }
}
