using ff_utils_winforms.Data;
using ff_utils_winforms.IO;
using ff_utils_winforms.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ff_utils_winforms
{
    class AvProcess
    {
        public static Process lastProcess;

        public static string lastOutputFfmpeg;
        public static string lastOutputGifski;

        public static async Task Run (string args)
        {
            string ffmpegPath = IOUtils.GetFfmpegExePath();
            string ffmpegDir = Path.GetDirectoryName(ffmpegPath);
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.RedirectStandardError = true;
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.FileName = "cmd.exe";
            ffmpeg.StartInfo.Arguments = $"/C cd /D {ffmpegDir.Wrap()} & ffmpeg -hide_banner {(args.Contains("-loglevel") ? "" : "-loglevel warning")} -y -stats {args}".TrimWhitespacesSafe();
            Program.Print("Running ffmpeg...");
            Program.Print("Args: " + args);
            ffmpeg.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            ffmpeg.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            ffmpeg.Start();
            ffmpeg.BeginOutputReadLine();
            ffmpeg.BeginErrorReadLine();
            while (!ffmpeg.HasExited)
                Task.Delay(100);
            Program.Print("Done running ffmpeg.");
        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine == null || outLine.Data == null || string.IsNullOrWhiteSpace(outLine.Data.Trim()))
                return;
            Program.Print(outLine.Data);
        }

        public static string GetFfmpegOutput(string args)
        {
            Process ffmpeg = OSUtils.NewProcess(true);
            lastProcess = ffmpeg;
            ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {Paths.GetBinPath().Wrap()} & ffmpeg.exe -hide_banner -y -stats {args}";
            Logger.Log("cmd.exe " + ffmpeg.StartInfo.Arguments, true, false, "ffmpeg");
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            string output = ffmpeg.StandardOutput.ReadToEnd();
            string err = ffmpeg.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err)) output += "\n" + err;
            return output;
        }

        public static async Task<string> GetFfmpegOutputAsync(string args, bool setBusy = false)
        {
            //if (Program.busy)
            //    setBusy = false;
            lastOutputFfmpeg = "";
            Process ffmpeg = OSUtils.NewProcess(true);
            lastProcess = ffmpeg;
            ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {Paths.GetBinPath().Wrap()} & ffmpeg.exe -hide_banner -y -stats {args}";
            Logger.Log("cmd.exe " + ffmpeg.StartInfo.Arguments, true, false, "ffmpeg");
            //if (setBusy)
            //    Program.mainForm.SetWorking(true);
            ffmpeg.Start();
            ffmpeg.OutputDataReceived += new DataReceivedEventHandler(FfmpegOutputHandlerSilent);
            ffmpeg.ErrorDataReceived += new DataReceivedEventHandler(FfmpegOutputHandlerSilent);
            ffmpeg.Start();
            ffmpeg.BeginOutputReadLine();
            ffmpeg.BeginErrorReadLine();
            while (!ffmpeg.HasExited)
                await Task.Delay(1);
            //if (setBusy)
            //    Program.mainForm.SetWorking(false);
            return lastOutputFfmpeg;
        }

        static void FfmpegOutputHandlerSilent(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine == null || outLine.Data == null || outLine.Data.Trim().Length < 2)
                return;
            string line = outLine.Data;

            if (!string.IsNullOrWhiteSpace(lastOutputFfmpeg))
                lastOutputFfmpeg += "\n";
            lastOutputFfmpeg = lastOutputFfmpeg + line;
            Logger.Log(line, true, false, "ffmpeg");
        }

        public static string GetFfprobeOutput(string args)
        {
            Process ffprobe = OSUtils.NewProcess(true);
            ffprobe.StartInfo.Arguments = $"{GetCmdArg()} cd /D {Paths.GetBinPath().Wrap()} & ffprobe.exe {args}";
            Logger.Log("cmd.exe " + ffprobe.StartInfo.Arguments, true, false, "ffmpeg");
            ffprobe.Start();
            ffprobe.WaitForExit();
            string output = ffprobe.StandardOutput.ReadToEnd();
            string err = ffprobe.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err)) output += "\n" + err;
            return output;
        }

        public static async Task<string> GetFfprobeOutputAsync(string args)
        {
            Process ffprobe = OSUtils.NewProcess(true);
            ffprobe.StartInfo.Arguments = $"{GetCmdArg()} cd /D {Paths.GetBinPath().Wrap()} & ffprobe.exe {args}";
            Logger.Log("cmd.exe " + ffprobe.StartInfo.Arguments, true, false, "ffmpeg");
            ffprobe.Start();
            while (!ffprobe.HasExited)
                await Task.Delay(1);
            string output = ffprobe.StandardOutput.ReadToEnd();
            string err = ffprobe.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err)) output += "\n" + err;
            return output;
        }

        static string GetCmdArg()
        {
            return "/C";
        }
    }
}
