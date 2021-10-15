﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Codecs;
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
                IEncoder vCodec = CodecUtils.GetCodec(GetCurrentCodecV());
                CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                CodecUtils.SubtitleCodec sCodec = GetCurrentCodecS();
                bool crf = (QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.Crf;
                bool twoPass = vCodec.SupportsTwoPass && !crf;
                Dictionary<string, string> videoArgs = vCodec.DoesNotEncode ? new Dictionary<string, string>() : GetVideoArgsFromUi(!crf);

                string inFiles = TrackList.GetInputFilesString();
                string outPath = GetOutPath(vCodec);
                string map = TrackList.GetMapArgs();
                string a = CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi()).Arguments;
                string s = CodecUtils.GetCodec(sCodec).GetArgs().Arguments;
                string meta = GetMetadataArgs();
                string custIn = Program.mainForm.customArgsInBox.Text.Trim();
                string custOut = Program.mainForm.customArgsOutBox.Text.Trim();
                string muxing = GetMuxingArgsFromUi();

                if(twoPass)
                {
                    CodecArgs codecArgsPass1 = vCodec.GetArgs(videoArgs, Pass.OneOfTwo, TrackList.current);
                    string v1 = codecArgsPass1.Arguments;
                    string vf1 = vCodec.DoesNotEncode ? "" : await GetVideoFilterArgs(vCodec, codecArgsPass1);
                    CodecArgs codecArgsPass2 = vCodec.GetArgs(videoArgs, Pass.TwoOfTwo, TrackList.current);
                    string v2 = codecArgsPass2.Arguments;
                    string vf2 = vCodec.DoesNotEncode ? "" : await GetVideoFilterArgs(vCodec, codecArgsPass2);

                    args = $"{custIn} {inFiles} -map v {v1} {vf1} -f null - && ffmpeg -y -loglevel warning -stats " +
                           $"{custIn} {inFiles} {map} {v2} {vf2} {a} {s} {meta} {custOut} {muxing} {outPath.Wrap()}";
                }
                else
                {
                    CodecArgs codecArgs = vCodec.GetArgs(videoArgs, Pass.OneOfOne, TrackList.current);
                    string v = codecArgs.Arguments;
                    string vf = vCodec.DoesNotEncode ? "" : await GetVideoFilterArgs(vCodec, codecArgs);

                    args = $"{custIn} {inFiles} {map} {v} {vf} {a} {s} {meta} {custOut} {muxing} {outPath.Wrap()}";
                }
            }
            catch(Exception e)
            {
                Logger.Log($"Error creating FFmpeg command: {e.Message}\n{e.StackTrace}");
                return;
            }
            
            Logger.Log($"Running:\nffmpeg {args}", true, false, "ffmpeg");

            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.OnlyLastLine, true);
        }
    }
}
