using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Nmkoder.UI.Tasks
{
    class UtilGetMetrics
    {
        public static string vidLq;
        public static string vidHq;
        public static bool runVmaf = true;
        public static bool runSsim;
        public static bool runPsnr;
        public static int alignMode = 0;
        public static int vmafModel = 0;

        public static async Task Run(bool fixRate = true)
        {
            Program.mainForm.SetWorking(true);
            Logger.Log("Analyzing video...");

            try
            {
                string r = fixRate ? "-r 24" : "";
                string f = await GetAlignFilters();

                if (runVmaf)
                {
                    string vmafFilter = $"libvmaf={Paths.GetVmafPath(true, GetVmafModel())}:n_threads={Environment.ProcessorCount}";
                    string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex {f}{vmafFilter} -f null -";
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
                    string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex {f}ssim -f null -";
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
                    string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex {f}psnr -f null -";
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
            }
            catch(Exception e)
            {
                Logger.Log($"Error trying to get metrics: {e.Message}\n{e.StackTrace}");
            }
            

            Program.mainForm.SetWorking(false);
        }

        private static async Task<string> GetAlignFilters ()
        {
            List<string> filters = new List<string>();

            if(alignMode == 1 || alignMode == 3) // Auto-Crop
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(vidHq, true));

            if (alignMode == 2 || alignMode == 3)
            {
                Size res = await GetMediaResolutionCached.GetSizeAsync(vidLq);
                filters.Add($"scale={res.Width}:{res.Height}");
            }

            if (filters.Count > 0)
                return $"[1:v]{string.Join(",", filters)},";
            else
                return "";
        }

        private static string GetVmafModel ()
        {
            if (vmafModel == 0) return "vmaf_v0.6.1";
            if (vmafModel == 1) return "vmaf_v0.6.1neg";
            if (vmafModel == 2) return "vmaf_4k_v0.6.1";
            return "";
        }
    }
}
