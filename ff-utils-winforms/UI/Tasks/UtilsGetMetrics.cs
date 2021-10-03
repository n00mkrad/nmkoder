using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.Utils;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    class UtilGetMetrics
    {
        public static string vidLq;
        public static string vidHq;
        public static bool runVmaf = true;
        public static bool runSsim;
        public static bool runPsnr;

        public static async Task Run(bool fixRate = true)
        {
            Program.mainForm.SetWorking(true);
            Logger.Log("Analyzing video...");

            string r = fixRate ? "-r 24" : "";

            if (runVmaf)
            {
                string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex libvmaf={Paths.GetVmafPath(true)}:n_threads={Environment.ProcessorCount} -f null -";
                string output = await AvProcess.GetFfmpegOutputAsync(args, false, true);
                List<string> vmafLines = output.SplitIntoLines().Where(x => x.Contains("VMAF score: ")).ToList();

                if (vmafLines.Count < 1)
                {
                    Logger.Log($"Failed to get VMAF!");
                }
                else
                {
                    string vmafStr = vmafLines[0].Split("VMAF score: ").LastOrDefault();
                    Logger.Log($"VMAF Score: {vmafStr}");
                }
            }

            if (runSsim)
            {
                string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex ssim -f null -";
                string output = await AvProcess.GetFfmpegOutputAsync(args, false, true);
                List<string> ssimLines = output.SplitIntoLines().Where(x => x.Contains("] SSIM ")).ToList();

                if (ssimLines.Count < 1)
                {
                    Logger.Log($"Failed to get SSIM!");
                }
                else
                {
                    string scoreStr = ssimLines[0].Split(" All:").LastOrDefault();
                    Logger.Log($"SSIM Score: {scoreStr}");
                }
            }

            if (runPsnr)
            {
                string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex psnr -f null -";
                string output = await AvProcess.GetFfmpegOutputAsync(args, false, true);
                List<string> psnrLines = output.SplitIntoLines().Where(x => x.Contains("] PSNR ")).ToList();

                if (psnrLines.Count < 1)
                {
                    Logger.Log($"Failed to get PSNR!");
                }
                else
                {
                    string scoreStr = psnrLines[0].Split("average:").LastOrDefault().Split(' ')[0];
                    Logger.Log($"PSNR Score: {scoreStr}");
                }
            }

            Program.mainForm.SetWorking(false);
        }
    }
}
