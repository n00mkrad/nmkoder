﻿using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.OS;
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
    class FfmpegCommands
    {
        //public static string padFilter = "pad=width=ceil(iw/2)*2:height=ceil(ih/2)*2:color=black@0";
        public static string hdrFilter = @"-vf zscale=t=linear:npl=100,format=gbrpf32le,zscale=p=bt709,tonemap=tonemap=hable:desat=0,zscale=t=bt709:m=bt709:r=tv,format=yuv420p";
        public static string pngCompr = "-compression_level 3";
        public static string mpDecDef = "\"mpdecimate\"";
        public static string mpDecAggr = "\"mpdecimate=hi=64*32:lo=64*32:frac=0.1\"";

        public static int GetPadding ()
        {
            //return (Interpolate.current.ai.aiName == Implementations.flavrCuda.aiName) ? 8 : 2;     // FLAVR input needs to be divisible by 8
            // TODO: CHECK IF CODEC NEEDS mod2 etc
            return 2;
        }

        public static string GetPadFilter ()
        {
            int padPixels = GetPadding();
            return $"pad=width=ceil(iw/{padPixels})*{padPixels}:height=ceil(ih/{padPixels})*{padPixels}:color=black@0";
        }

        public static async Task ConcatVideos(string concatFile, string outPath, int looptimes = -1, bool showLog = true)
        {
            Logger.Log($"ConcatVideos('{Path.GetFileName(concatFile)}', '{outPath}', {looptimes})", true, false, "ffmpeg");

            if(showLog)
                Logger.Log($"Merging videos...", false, Logger.LastUiLine.Contains("frame"));

            IoUtils.RenameExistingFile(outPath);
            string loopStr = (looptimes > 0) ? $"-stream_loop {looptimes}" : "";
            string vfrFilename = Path.GetFileName(concatFile);
            string args = $" {loopStr} -vsync 1 -f concat -i {vfrFilename} -c copy -movflags +faststart -fflags +genpts {outPath.Wrap()}";
            FfmpegSettings settings = new FfmpegSettings() { Args = args, WorkingDir = concatFile.GetParentDir(), LoggingMode = LogMode.Hidden };
            await RunFfmpeg(settings);
        }

        public static async Task LoopVideo(string inputFile, int times, bool delSrc = false)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string loopSuffix = $"{times}x";
            string outpath = $"{pathNoExt}{loopSuffix}{ext}";
            IoUtils.RenameExistingFile(outpath);
            string args = $" -stream_loop {times} -i {inputFile.Wrap()} -c copy {outpath.Wrap()}";

            FfmpegSettings settings = new FfmpegSettings() { Args = args, LoggingMode = LogMode.Hidden };
            await RunFfmpeg(settings);

            if (delSrc)
                DeleteSource(inputFile);
        }

        public static async Task ChangeSpeed(string inputFile, float newSpeedPercent, bool delSrc = false)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            float val = newSpeedPercent / 100f;
            string speedVal = (1f / val).ToString("0.0000").Replace(",", ".");
            string args = " -itsscale " + speedVal + " -i \"" + inputFile + "\" -c copy \"" + pathNoExt + "-" + newSpeedPercent + "pcSpeed" + ext + "\"";

            FfmpegSettings settings = new FfmpegSettings() { Args = args, LoggingMode = LogMode.OnlyLastLine };
            await RunFfmpeg(settings);

            if (delSrc)
                DeleteSource(inputFile);
        }

        public static async Task<long> GetDurationMs(string inputFile)
        {
            Logger.Log($"GetDuration({inputFile}) - Reading Duration using ffprobe.", true, false, "ffmpeg");
            string args = $"-select_streams v:0 -show_entries format=duration -of csv=s=x:p=0 -sexagesimal {inputFile.Wrap()}";
            FfprobeSettings settings = new FfprobeSettings() { Args = args };
            string output = await RunFfprobe(settings);

            return FormatUtils.TimestampToMs(output);
        }

        public static async Task<Fraction> GetFramerate(string inputFile, bool preferFfmpeg = false, int streamIndex = 0)
        {
            Logger.Log($"GetFramerate(inputFile = '{inputFile}', preferFfmpeg = {preferFfmpeg})", true, false, "ffmpeg");
            Fraction ffprobeFps = new Fraction(0, 1);
            Fraction ffmpegFps = new Fraction(0, 1);

            try
            {
                string ffprobeOutput = await GetVideoInfo.GetFfprobeInfoAsync(inputFile, GetVideoInfo.FfprobeMode.ShowStreams, "r_frame_rate", streamIndex);
                string fpsStr = ffprobeOutput.SplitIntoLines().First();
                string[] numbers = fpsStr.Split('/');
                Logger.Log($"Fractional FPS from ffprobe: {numbers[0]}/{numbers[1]} = {((float)numbers[0].GetInt() / numbers[1].GetInt())}", true, false, "ffmpeg");
                ffprobeFps = new Fraction(numbers[0].GetInt(), numbers[1].GetInt());
            }
            catch (Exception ffprobeEx)
            {
                Logger.Log("GetFramerate ffprobe Error: " + ffprobeEx.Message, true, false);
            }

            try
            {
                string ffmpegOutput = await GetVideoInfo.GetFfmpegInfoAsync(inputFile);
                string[] entries = ffmpegOutput.Split(',');

                foreach (string entry in entries)
                {
                    if (entry.Contains(" fps") && !entry.Contains("Input "))    // Avoid reading FPS from the filename, in case filename contains "fps"
                    {
                        string num = entry.Replace(" fps", "").Trim().Replace(",", ".");
                        Logger.Log($"Float FPS from ffmpeg: {num.GetFloat()}", true, false, "ffmpeg");
                        ffmpegFps = new Fraction(num.GetFloat());
                    }
                }
            }
            catch(Exception ffmpegEx)
            {
                Logger.Log("GetFramerate ffmpeg Error: " + ffmpegEx.Message, true, false);
            }

            if (preferFfmpeg)
            {
                if (ffmpegFps.GetFloat() > 0)
                    return ffmpegFps;
                else
                    return ffprobeFps;
            }
            else
            {
                if (ffprobeFps.GetFloat() > 0)
                    return ffprobeFps;
                else
                    return ffmpegFps;
            }
        }

        public static async Task<Size> GetSize(string filePath)
        {
            Logger.Log($"GetSize('{filePath}')", true, false, "ffmpeg");
            string args = $"{filePath.GetConcStr()} -select_streams v:0 -show_entries stream=width,height -of csv=s=x:p=0 {filePath.Wrap()}";
            FfprobeSettings settings = new FfprobeSettings() { Args = args };
            string[] outputLines = (await RunFfprobe(settings)).SplitIntoLines();

            foreach (string line in outputLines)
            {
                if (!line.Contains("x") || line.Trim().Length < 3)
                    continue;

                string[] numbers = line.Split('x');
                return new Size(numbers[0].GetInt(), numbers[1].GetInt());
            }

            return new Size(0, 0);
        }

        public static async Task<int> GetFrameCountAsync(string path, bool tryPacketCount = true, bool tryFfprobe = true, bool tryFfmpeg = true, NmkoderProcess.ProcessType processType = NmkoderProcess.ProcessType.Secondary)
        {
            if (tryPacketCount)
            {
                string a = $"-select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0 {path.Wrap()}";
                string o = await RunFfprobe(new FfprobeSettings() { Args = a, ProcessType = processType });
                string[] lines = o.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                if (lines != null && lines.Length > 0 && lines.Last().GetInt() > 0) return lines.Last().GetInt();
            }

            if (tryFfprobe)
            {
                string a = $"{path.GetConcStr()} -threads 0 -select_streams v:0 -show_entries stream=nb_frames -of default=noprint_wrappers=1 {path.Wrap()}";
                string o = await RunFfprobe(new FfprobeSettings() { Args = a, ProcessType = processType });
                string[] entries = o.SplitIntoLines();

                foreach (string entry in entries)
                    if (entry.Contains("nb_frames=") && entry.GetInt() > 0)
                        return entry.GetInt();
            }

            if (tryFfmpeg)
            {
                string a = $"{path.GetConcStr()} -i {path.Wrap()} -map 0:v:0 -c copy -f null - ";
                FfmpegSettings settings = new FfmpegSettings() { Args = a, LoggingMode = LogMode.Hidden, SetBusy = true, LogLevel = "panic", ReliableOutput = true, ProcessType = processType };
                string[] lines = (await RunFfmpeg(settings)).SplitIntoLines();

                try
                {
                    string lastLine = lines.Last().ToLower();
                    int fr = lastLine.Substring(0, lastLine.IndexOf("fps")).GetInt();
                    if (fr > 0) return fr;
                } 
                catch { }
            }

            Logger.Log("Failed to get total frame count of video.");
            return 0;
        }

        public static async Task<bool> IsEncoderCompatible(string enc)
        {
            Logger.Log($"IsEncoderCompatible('{enc}')", true, false, "ffmpeg");
            string args = $"-loglevel error -f lavfi -i color=black:s=540x540 -vframes 1 -an -c:v {enc} -f null -";
            FfmpegSettings settings = new FfmpegSettings() { Args = args, LoggingMode = LogMode.Hidden, LogLevel = "error", ReliableOutput = true };
            string output = await RunFfmpeg(settings);
            return !output.ToLower().Contains("error");
        }

        public static void DeleteSource(string path)
        {
            Logger.Log("[FFCmds] Deleting input file/dir: " + path, true);

            if (IoUtils.IsPathDirectory(path) && Directory.Exists(path))
                Directory.Delete(path, true);

            if (!IoUtils.IsPathDirectory(path) && File.Exists(path))
                File.Delete(path);
        }
    }
}
