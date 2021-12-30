using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Nmkoder.Media.AvProcess;

namespace Nmkoder.Media
{
    partial class FfmpegExtract : FfmpegCommands
    {
        public static async Task ExtractSingleFrame(string inputFile, string outputPath, int frameNum, int maxH = 2160)
        {
            bool isPng = (Path.GetExtension(outputPath).ToLower() == ".png");
            string comprArg = isPng ? pngCompr : "-q:v 1";
            string pixFmt = "-pix_fmt " + (isPng ? $"rgb24 {comprArg}" : "yuvj420p");
            Size res = await GetMediaResolutionCached.GetSizeAsync(inputFile);
            string vf = res.Height > maxH ? $"-vf scale=-1:{maxH.RoundMod(2)}" : "";
            string args = $"-i {inputFile.Wrap()} -vf \"select=eq(n\\,{frameNum})\" -vframes 1 {pixFmt} {vf} {outputPath.Wrap()}";
            await RunFfmpeg(args, LogMode.Hidden);
        }

        public static async Task ExtractSingleFrameAtTime(string inputFile, string outputPath, int skipSeconds, int maxH = 2160, bool noKey = false)
        {
            NmkdStopwatch sw = new NmkdStopwatch();
            bool isPng = (Path.GetExtension(outputPath).ToLower() == ".png");
            string comprArg = isPng ? pngCompr : "-q:v 1";
            string pixFmt = "-pix_fmt " + (isPng ? $"rgb24 {comprArg}" : "yuvj420p");
            Size res = await GetMediaResolutionCached.GetSizeAsync(inputFile);
            string vf = res.Height > maxH ? $"-vf scale=-1:{maxH.RoundMod(2)}" : "";
            string noKeyArg = noKey ? "-skip_frame nokey" : "";
            string args = $"{noKeyArg} -ss {skipSeconds} -i {inputFile.Wrap()} -map 0:v -vframes 1 {pixFmt} {vf} {outputPath.Wrap()}";
            await RunFfmpeg(args, LogMode.Hidden);
        }

        public static async Task ExtractThumbs(string inputFile, string outputDir, int amount, int maxH = 360, string format = "jpg")
        {
            long duration = (int)Math.Floor((float)(await GetDurationMs(inputFile)) / 1000);
            int interval = (int)Math.Floor((float)duration / amount);

            Logger.Log($"Thumbnail Interval: {duration}/{amount} = {interval}", true);

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < amount; i++)
            {
                int time = interval * (i + 1);
                tasks.Add(ExtractSingleFrameAtTime(inputFile, Path.Combine(outputDir, $"thumb{i + 1}-s{time}.{format}"), time, maxH, false));
            }

            await Task.WhenAll(tasks);
        }

        //public static async Task ExtractLastFrame(string inputFile, string outputPath, Size size)
        //{
        //    if (QuickSettingsTab.trimEnabled)
        //        return;
        //
        //    if (IoUtils.IsPathDirectory(outputPath))
        //        outputPath = Path.Combine(outputPath, "last.png");
        //
        //    bool isPng = (Path.GetExtension(outputPath).ToLower() == ".png");
        //    string comprArg = isPng ? pngCompr : "";
        //    string pixFmt = "-pix_fmt " + (isPng ? $"rgb24 {comprArg}" : "yuvj420p");
        //    string sizeStr = (size.Width > 1 && size.Height > 1) ? $"-s {size.Width}x{size.Height}" : "";
        //    string trim = QuickSettingsTab.trimEnabled ? $"-ss {QuickSettingsTab.GetTrimEndMinusOne()} -to {QuickSettingsTab.trimEnd}" : "";
        //    string sseof = string.IsNullOrWhiteSpace(trim) ? "-sseof -1" : "";
        //    string args = $"{sseof} -i {inputFile.Wrap()} -update 1 {pixFmt} {sizeStr} {trim} {outputPath.Wrap()}";
        //    await RunFfmpeg(args, LogMode.Hidden, TaskType.ExtractFrames);
        //}
    }
}
