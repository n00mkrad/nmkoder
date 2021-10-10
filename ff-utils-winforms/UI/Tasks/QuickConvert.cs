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
        public enum QualityMode { Crf, TargetKbps, TargetMbytes }

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
                bool twoPass = (QualityMode)Program.mainForm.encQualModeBox.SelectedIndex != QualityMode.Crf;
                CodecUtils.VideoCodec vCodec = GetCurrentCodecV();
                CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                CodecUtils.SubtitleCodec sCodec = GetCurrentCodecS();
                string inFiles = TrackList.GetInputFilesString();
                string outPath = GetOutPath(vCodec);
                string map = TrackList.GetMapArgs();
                CodecArgs codecArgs = CodecUtils.GetCodec(vCodec).GetArgs(GetVideoArgsFromUi(twoPass), TrackList.current); // CodecUtils.GetArgs(vCodec, GetVideoArgsFromUi(twoPass), twoPass, TrackList.current);
                string v = codecArgs.Arguments;
                string vf = await GetVideoFilterArgs(vCodec, codecArgs);
                string a = CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi()).Arguments; // CodecUtils.GetArgs(aCodec, GetAudioArgsFromUi());
                string s = CodecUtils.GetCodec(sCodec).GetArgs().Arguments; // CodecUtils.GetArgs(sCodec);
                string meta = GetMetadataArgs();
                string custIn = Program.mainForm.customArgsInBox.Text.Trim();
                string custOut = Program.mainForm.customArgsOutBox.Text.Trim();
                string muxing = GetMuxingArgsFromUi();
                args = $"{custIn} {inFiles} {map} {v} {vf} {a} {s} {meta} {custOut} {muxing} {outPath.Wrap()}";

                if(twoPass)
                {
                    args = $"{custIn} {inFiles} -map v {v} {vf} -pass 1 -f null - && " +
                        $"ffmpeg -y -loglevel warning -stats {custIn} {inFiles} {map} {v} {vf} {a} {s} {meta} {custOut} {muxing} -pass 2 {outPath.Wrap()}";
                }
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
