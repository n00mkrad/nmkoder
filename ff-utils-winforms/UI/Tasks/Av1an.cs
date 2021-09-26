using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using static Nmkoder.UI.Tasks.Av1anUi;

namespace Nmkoder.UI.Tasks
{
    class Av1an
    {
        public enum QualityMode { Crf, TargetVmaf }

        public static void Init()
        {
            Av1anUi.Init();
        }

        public static async Task Run()
        {
            Program.mainForm.SetWorking(true);
            string args = "";

            try
            {
                Codecs.Av1anCodec vCodec = GetCurrentCodecV();
                Codecs.AudioCodec aCodec = GetCurrentCodecA();
                string inPath = MediaInfo.current.TruePath;
                string outPath = GetOutPath();
                CodecArgs codecArgs = Codecs.GetArgs(vCodec, GetVideoArgsFromUi(), MediaInfo.current);
                string v = codecArgs.Arguments;
                string vf = await GetVideoFilterArgs(vCodec, codecArgs);
                string a = Codecs.GetArgs(aCodec, GetAudioArgsFromUi());
                string cust = Program.mainForm.av1anCustomArgsBox.Text.Trim();
                string muxing = GetMuxingArgsFromUi();
                IoUtils.TryDeleteIfExists(outPath);
                args = $"-i {inPath.Wrap()} {v} -f \" {vf} \" -a \" {a} \" {cust} -o {outPath.Wrap()} -w 4";
                Logger.Log("av1an " + args);
            }
            catch (Exception e)
            {
                Logger.Log($"Error creating av1an command: {e.Message}\n{e.StackTrace}");
                return;
            }

            Logger.Log($"Running:\nav1an {args}", true, false, "ffmpeg");

            await AvProcess.RunAv1an(args, AvProcess.LogMode.OnlyLastLine, true);

            Program.mainForm.SetWorking(false);
        }
    }
}
