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
        public static void Init()
        {
            QuickConvertUi.Init();
        }

        public static async Task Run ()
        {
            Program.mainForm.SetWorking(true);

            string inFiles = MediaInfo.GetInputFiles();
            string outPath = Program.mainForm.outputBox.Text.Trim();
            string map = MediaInfo.GetMapArgs();
            CodecArgs codecArgs = Codecs.GetArgs(GetCurrentCodecV(), GetVideoArgsFromUi(), MediaInfo.current);
            string v = codecArgs.Arguments;
            string vf = await GetVideoFilterArgs(GetCurrentCodecV(), codecArgs);
            string a = Codecs.GetArgs(GetCurrentCodecA(), GetAudioArgsFromUi());
            string s = Codecs.GetArgs(GetCurrentCodecS());
            string meta = GetMetadataArgs();
            string custIn = Program.mainForm.customArgsInBox.Text.Trim();
            string custOut = Program.mainForm.customArgsOutBox.Text.Trim();
            string muxing = GetMuxingArgsFromUi();

            string args = $"{custIn} {inFiles} {map} {v} {vf} {a} {s} {meta} {custOut} {muxing} {outPath.Wrap()}";
            Logger.Log($"Running:\nffmpeg {args}", true, false, "ffmpeg");

            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.OnlyLastLine, AvProcess.TaskType.Encode, true);

            Program.mainForm.SetWorking(false);
        }

        
    }
}
