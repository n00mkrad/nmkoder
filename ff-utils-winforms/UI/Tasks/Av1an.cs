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
        public enum ChunkMethod { Hybrid, Lsmash, Ffms2, Segment, Select }

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
                CodecUtils.Av1anCodec vCodec = GetCurrentCodecV();
                CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                bool vmaf = IsUsingVmaf();
                string inPath = TrackList.current.TruePath;
                string outPath = GetOutPath();
                string cust = Program.mainForm.av1anCustomArgsBox.Text.Trim();
                string custEnc = Program.mainForm.av1anCustomEncArgsBox.Text.Trim();
                CodecArgs codecArgs = CodecUtils.GetCodec(vCodec).GetArgs(GetVideoArgsFromUi(), TrackList.current);
                string v = codecArgs.Arguments;
                string vf = await GetVideoFilterArgs(codecArgs);
                string a = CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi()).Arguments;
                string w = Program.mainForm.av1anOptsWorkerCountUpDown.Value.ToString();
                string sm = GetSplittingMethod();
                string cm = GetChunkGenMethod();
                IoUtils.TryDeleteIfExists(outPath);

                args = $"-i {inPath.Wrap()} --verbose --keep --split-method {sm} -m {cm} {v} -f \" {vf} \" -a \" {a} \" {cust} -w {w} -o {outPath.Wrap()}";

                if (vmaf)
                {
                    int q = (int)Program.mainForm.av1anQualityUpDown.Value;
                    string filters = vf.Length > 3 ? $"--vmaf-filter \" {vf.Split("-vf ").LastOrDefault()} \"" : "";
                    args += $" --target-quality {q} --vmaf-path {Paths.GetVmafPath(false).Wrap()} {filters} --vmaf-threads 2";
                }
               
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

        public static int GetDefaultWorkerCount ()
        {
            return (int)Math.Ceiling((double)Environment.ProcessorCount * 0.4f);
        }
    }
}
