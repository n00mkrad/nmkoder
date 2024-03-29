﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.Forms.Utils;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using static Nmkoder.Media.AvProcess;

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
        public static int subsample = 0;

        public static async Task Run(bool fixRate = true)
        {
            if(RunTask.currentFileListMode == RunTask.FileListMode.Batch)
            {
                Logger.Log($"Metrics Utility: Didn't run because this util only works in Muxing Mode!");
                return;
            }

            Program.mainForm.SetWorking(true);

            try
            {
                UtilsMetricsForm form = new UtilsMetricsForm(true);
                form.ShowDialog();
                Program.mainForm.SetMetricsVarsFromForm(form);

                Logger.Log($"Getting metrics for {Path.GetFileName(vidLq)} compared against {Path.GetFileName(vidHq)}...");

                Fraction fps = await IoUtils.GetVideoFramerate(vidLq);
                string r = fixRate ? $"-r {(fps.GetFloat() > 0f ? fps.ToString() : "24")}" : "";
                string f = await GetAlignFilters();
                FfmpegOutputHandler.overrideTargetDurationMs = await FfmpegCommands.GetDurationMs(vidLq);

                if (runVmaf)
                {
                    Logger.Log("Calculating VMAF...");
                    string vmafPath = Paths.GetVmafPath(true, GetVmafModel());
                    string vmafFilter = $"libvmaf={vmafPath}:n_threads={Environment.ProcessorCount}:n_subsample={subsample}";
                    string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex {f}{vmafFilter} -f null -";
                    FfmpegSettings settings = new FfmpegSettings() { Args = args, LoggingMode = LogMode.OnlyLastLine, LogLevel = "info", ReliableOutput = true, ProgressBar = true };
                    string output = await RunFfmpeg(settings);
                    List<string> vmafLines = output.SplitIntoLines().Where(x => x.Contains("VMAF score: ")).ToList();

                    if (vmafLines.Count < 1)
                    {
                        Logger.Log($"Failed to get VMAF!", false, ReplaceLastLine());
                    }
                    else
                    {
                        string vmafStr = vmafLines[0].Split("VMAF score: ").LastOrDefault();
                        Logger.Log($"VMAF Score: {vmafStr}", false, ReplaceLastLine());
                    }
                }

                if (runSsim)
                {
                    Logger.Log("Calculating SSIM...");
                    string select = subsample > 1 ? $"select=not(mod(n-1\\,{subsample}))," : "";
                    string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex {f}{select}ssim -f null -";
                    FfmpegSettings settings = new FfmpegSettings() { Args = args, LoggingMode = LogMode.OnlyLastLine, LogLevel = "info", ReliableOutput = true, ProgressBar = true };
                    string output = await RunFfmpeg(settings);
                    List<string> ssimLines = output.SplitIntoLines().Where(x => x.Contains("] SSIM ")).ToList();

                    if (ssimLines.Count < 1)
                    {
                        Logger.Log($"Failed to get SSIM!", false, ReplaceLastLine());
                    }
                    else
                    {
                        string scoreStr = ssimLines[0].Split(" All:").LastOrDefault();
                        Logger.Log($"SSIM Score: {scoreStr.Replace("inf", "Infinite")}", false, ReplaceLastLine());
                    }
                }

                if (runPsnr)
                {
                    Logger.Log("Calculating PSNR...");
                    string select = subsample > 1 ? $"select=not(mod(n-1\\,{subsample}))," : "";
                    string args = $"{r} {vidLq.GetFfmpegInputArg()} {r} {vidHq.GetFfmpegInputArg()} -filter_complex {f}{select}psnr -f null -";
                    FfmpegSettings settings = new FfmpegSettings() { Args = args, LoggingMode = LogMode.OnlyLastLine, LogLevel = "info", ReliableOutput = true, ProgressBar = true };
                    string output = await RunFfmpeg(settings);
                    List<string> psnrLines = output.SplitIntoLines().Where(x => x.Contains("] PSNR ")).ToList();

                    if (psnrLines.Count < 1)
                    {
                        Logger.Log($"Failed to get PSNR!", false, ReplaceLastLine());
                    }
                    else
                    {
                        string scoreStr = psnrLines[0].Split("average:").LastOrDefault().Split(' ')[0];
                        Logger.Log($"PSNR Score: {scoreStr.Replace("inf", "Infinite")}", false, ReplaceLastLine());
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Log($"Error trying to get metrics: {e.Message}\n{e.StackTrace}");
            }

            FfmpegOutputHandler.overrideTargetDurationMs = -1;
            Program.mainForm.SetWorking(false);
        }

        static bool ReplaceLastLine ()
        {
            return (new[] { "...", FfmpegOutputHandler.prefix }).Any(c => Logger.LastUiLine.Contains(c));
        }

        private static async Task<string> GetAlignFilters ()
        {
            List<string> filters = new List<string>();

            if(alignMode == 1 || alignMode == 3) // Auto-Crop
                filters.Add(await FfmpegUtils.GetCurrentAutoCrop(vidHq, true));

            if (alignMode == 2 || alignMode == 3) // Resize
            {
                Size res = await GetMediaResolutionCached.GetSizeAsync(vidHq);
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
