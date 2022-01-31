using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.OS;
using Nmkoder.UI;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class AvProcess
    {
        public static string lastOutputAv1an;
        public static string lastTempDirAv1an;

        public enum LogMode { Visible, OnlyLastLine, Hidden }

        #region FFmpeg

        public class FfmpegSettings
        {
            public string Args { get; set; } = "";
            public string WorkingDir { get; set; } = "";
            public LogMode LoggingMode { get; set; } = LogMode.Hidden;
            public string LogLevel { get; set; } = "warning";
            public bool ReliableOutput { get; set; } = false;
            public bool SetBusy { get; set;} = false;
            public bool ProgressBar { get; set; } = false;
            public NmkoderProcess.ProcessType ProcessType { get; set; } = NmkoderProcess.ProcessType.Primary;
        }

        public static async Task<string> RunFfmpeg(FfmpegSettings settings)
        {
            bool show = Config.GetInt(Config.Key.CmdDebugMode) > 0;
            string processOutput = "";
            Process ffmpeg = OsUtils.NewProcess(!show, settings.ProcessType);
            NmkdStopwatch timeSinceLastOutput = new NmkdStopwatch();

            string beforeArgs = $"-hide_banner -stats -loglevel {settings.LogLevel} -y";

            if (!string.IsNullOrWhiteSpace(settings.WorkingDir))
                ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {settings.WorkingDir.Wrap()} & {Path.Combine(GetDir(), "ffmpeg.exe").Wrap()} {beforeArgs} {settings.Args}";
            else
                ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & ffmpeg.exe {beforeArgs} {settings.Args}";

            if (settings.LoggingMode != LogMode.Hidden) Logger.Log("Running FFmpeg...", false);
            Logger.Log($"ffmpeg {beforeArgs} {settings.Args}", true, false, "ffmpeg");

            if (!show)
            {
                string[] ignore = GetIgnoreStringsFromFfmpegCmd(settings.Args);
                ffmpeg.OutputDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, ignore, ref processOutput, "ffmpeg", settings.LoggingMode, settings.ProgressBar); timeSinceLastOutput.sw.Restart(); };
                ffmpeg.ErrorDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, ignore, ref processOutput, "ffmpeg", settings.LoggingMode, settings.ProgressBar); timeSinceLastOutput.sw.Restart(); };
            }

            if (settings.SetBusy) Program.mainForm.SetWorking(true);
            ffmpeg.Start();
            ffmpeg.PriorityClass = ProcessPriorityClass.BelowNormal;

            if (!show)
            {
                ffmpeg.BeginOutputReadLine();
                ffmpeg.BeginErrorReadLine();
            }

            while (!ffmpeg.HasExited) await Task.Delay(10);
            while (settings.ReliableOutput && timeSinceLastOutput.ElapsedMs < 200) await Task.Delay(50);

            if (settings.SetBusy) Program.mainForm.SetWorking(false);

            if (settings.ProgressBar)
                Program.mainForm.SetProgress(0);

            return processOutput;
        }

        private static string[] GetIgnoreStringsFromFfmpegCmd (string cmd)
        {
            List<string> strs = new List<string>();
            
            try
            {
                strs.Add(cmd.Split(" -i ")[1].Split("\"")[1].Trim());
            }
            catch { }

            try
            {
                string[] splitByQuotes = cmd.Split("\"");
                strs.Add(splitByQuotes[splitByQuotes.Length - 2]);
            }
            catch { }
            
            return strs.ToArray();
        }

        public class FfprobeSettings
        {
            public string Args { get; set; } = "";
            public LogMode LoggingMode { get; set; } = LogMode.Hidden;
            public string LogLevel { get; set; } = "panic";
            public bool SetBusy { get; set; } = false;
            public NmkoderProcess.ProcessType ProcessType { get; set; } = NmkoderProcess.ProcessType.Background;
        }

        public static async Task<string> RunFfprobe(FfprobeSettings settings)
        {
            bool show = Config.GetInt(Config.Key.CmdDebugMode) > 0;
            string processOutput = "";
            Process ffprobe = OsUtils.NewProcess(!show, settings.ProcessType);
            NmkdStopwatch timeSinceLastOutput = new NmkdStopwatch();

            ffprobe.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & ffprobe -v {settings.LogLevel} {settings.Args}";

            if (settings.LoggingMode != LogMode.Hidden) Logger.Log("Running FFprobe...", false);
            Logger.Log($"ffprobe -v {settings.LogLevel} {settings.Args}", true, false, "ffmpeg");

            if (!show)
            {
                string[] ignore = new string[0];
                ffprobe.OutputDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, ignore, ref processOutput, "ffmpeg", settings.LoggingMode, false); timeSinceLastOutput.sw.Restart(); };
                ffprobe.ErrorDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, ignore, ref processOutput, "ffmpeg", settings.LoggingMode, false); timeSinceLastOutput.sw.Restart(); };
            }

            ffprobe.Start();
            ffprobe.PriorityClass = ProcessPriorityClass.BelowNormal;

            if (!show)
            {
                ffprobe.BeginOutputReadLine();
                ffprobe.BeginErrorReadLine();
            }

            while (!ffprobe.HasExited) await Task.Delay(10);
            while (timeSinceLastOutput.ElapsedMs < 200) await Task.Delay(50);

            return processOutput;
        }

        #endregion

        #region av1an

        public static async Task RunAv1an(string args, LogMode logMode, bool progressBar = false)
        {
            await RunAv1an(args, "", logMode, progressBar);
        }

        public static async Task RunAv1an(string args, string workingDir, LogMode logMode, bool progressBar = false)
        {
            try
            {
                string dir = Path.Combine(GetDir(), "av1an");
                bool show = Config.GetBool(Config.Key.Av1anCmdVisible, true); // = Config.GetInt(Config.Key.cmdDebugMode) > 0;
                lastOutputAv1an = "";
                Process av1an = OsUtils.NewProcess(!show, NmkoderProcess.ProcessType.Primary);

                string vsynthPath = Path.Combine(dir, "vsynth");
                string encPath = Path.Combine(dir, "enc");
                string ffmpegPath = Paths.GetBinPath();

                if (!show)
                {
                    av1an.StartInfo.EnvironmentVariables["Path"] = $"{vsynthPath};{encPath};{ffmpegPath};{av1an.StartInfo.EnvironmentVariables["Path"]}";
                    av1an.StartInfo.Arguments = $"/C cd /D {dir.Wrap()} && av1an.exe {args}";
                }
                else
                {
                    string batPath = WriteBatchFile(dir, new string[] { vsynthPath, encPath, ffmpegPath }, args);
                    av1an.StartInfo.Arguments = $"/C {batPath.Wrap()}";
                }

                if (logMode != LogMode.Hidden) Logger.Log("Running av1an...", false);

                if (!show)
                {
                    av1an.OutputDataReceived += (sender, outLine) => { Av1anOutputHandler.LogOutput(outLine.Data, "av1an", logMode, progressBar); };
                    av1an.ErrorDataReceived += (sender, outLine) => { Av1anOutputHandler.LogOutput(outLine.Data, "av1an", logMode, progressBar); };
                }

                Logger.Log($"cmd {av1an.StartInfo.Arguments}", true, false, "av1an");
                //Task.Run(() => Av1anOutputHandler.ParseProgressLoop());
                av1an.Start();
                av1an.PriorityClass = ProcessPriorityClass.BelowNormal;

                if (!show)
                {
                    av1an.BeginOutputReadLine();
                    av1an.BeginErrorReadLine();
                }

                while (!av1an.HasExited)
                    await Task.Delay(10);

                if (progressBar)
                    Program.mainForm.SetProgress(0);
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}");
            }
        }

        private static string WriteBatchFile (string workingDir, string[] paths, string av1anArgs)
        {
            Logger.Log($"Writing batch file to launch: av1an {av1anArgs}", true, false, "av1an");
            List<string> lines = new List<string>();
            lines.Add($"@echo off");
            lines.Add($"cd /D {workingDir.Wrap()}");
            lines.Add($"SET PATH={string.Join(";", paths)};%PATH%");
            lines.Add($"av1an {av1anArgs}");
            lines.Add($"timeout /t 10");
            string path = Path.Combine(Paths.GetSessionDataPath(), "av1an.bat");
            File.WriteAllLines(path, lines);
            return path;
        }

        #endregion

        #region MkvToolNix

        public static async Task<string> RunMkvExtract(string args, NmkoderProcess.ProcessType processType)
        {
            bool show = Config.GetInt(Config.Key.CmdDebugMode) > 0;
            string processOutput = "";

            try
            {
                Process mkve = OsUtils.NewProcess(!show, processType);

                mkve.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & mkvextract.exe {args}";

                Logger.Log($"mkvextract {args}", true, false, "mkvextract");

                mkve.OutputDataReceived += (sender, outLine) => { processOutput += Environment.NewLine + outLine.Data; Logger.Log($"[mkvextract] {outLine.Data}", true, false, "mkvextract"); };
                mkve.ErrorDataReceived += (sender, outLine) => { processOutput += Environment.NewLine + outLine.Data; };

                mkve.Start();
                mkve.PriorityClass = ProcessPriorityClass.BelowNormal;
                mkve.BeginOutputReadLine();
                mkve.BeginErrorReadLine();

                while (!mkve.HasExited) await Task.Delay(10);
            }
            catch(Exception e)
            {
                Logger.Log($"Error running MkvExtract: {e.Message}");
            }

            return processOutput;
        }

        public static async Task<string> RunMkvMerge(string args, NmkoderProcess.ProcessType processType, bool log = false)
        {
            bool show = Config.GetInt(Config.Key.CmdDebugMode) > 0;
            string processOutput = "";

            try
            {
                Process mkvm = OsUtils.NewProcess(!show, processType);

                mkvm.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & mkvmerge.exe {args}";

                Logger.Log($"mkvmerge {args}", true, false, "mkvmerge");

                mkvm.OutputDataReceived += (sender, outLine) => {
                    string s = (outLine != null && outLine.Data != null) ? outLine.Data : "";
                    processOutput += Environment.NewLine + s;
                    Logger.Log($"[mkvmerge] {s}", !log || s.Trim().Length < 1, Logger.LastUiLine.Trim().EndsWith("%"), "mkvmerge");
                };

                mkvm.ErrorDataReceived += (sender, outLine) => {
                    string s = (outLine != null && outLine.Data != null) ? outLine.Data : "";
                    processOutput += Environment.NewLine + s;
                    Logger.Log($"[mkvmerge] [E] {s}", !log || s.Trim().Length < 1, false, "mkvmerge");
                };

                mkvm.Start();
                mkvm.PriorityClass = ProcessPriorityClass.BelowNormal;
                mkvm.BeginOutputReadLine();
                mkvm.BeginErrorReadLine();

                while (!mkvm.HasExited) await Task.Delay(10);
            }
            catch (Exception e)
            {
                Logger.Log($"Error running MkvMerge: {e.Message}");
            }

            return processOutput;
        }

        public static async Task<string> RunMkvInfo(string args, NmkoderProcess.ProcessType processType, bool log = false)
        {
            bool show = Config.GetInt(Config.Key.CmdDebugMode) > 0;
            string processOutput = "";
            NmkdStopwatch timeSinceLastOutput = new NmkdStopwatch();

            try
            {
                Process mkvi = OsUtils.NewProcess(!show, processType);

                mkvi.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & mkvinfo.exe {args}";

                Logger.Log($"mkvinfo {args}", true, false, "mkvinfo");

                mkvi.OutputDataReceived += (sender, outLine) => { processOutput += Environment.NewLine + outLine.Data; if(log) Logger.Log($"[mkvinfo] {outLine.Data}", true, false, "ocr"); timeSinceLastOutput.sw.Restart(); };
                mkvi.ErrorDataReceived += (sender, outLine) => { processOutput += Environment.NewLine + outLine.Data; timeSinceLastOutput.sw.Restart(); };

                mkvi.Start();
                mkvi.PriorityClass = ProcessPriorityClass.BelowNormal;
                mkvi.BeginOutputReadLine();
                mkvi.BeginErrorReadLine();

                while (!mkvi.HasExited) await Task.Delay(10);
                while (timeSinceLastOutput.ElapsedMs < 200) await Task.Delay(50);
            }
            catch (Exception e)
            {
                Logger.Log($"Error running MkvInfo: {e.Message}");
            }

            return processOutput;
        }

        #endregion

        static string GetDir()
        {
            return Paths.GetBinPath();
        }

        public static string GetCmdArg()
        {
            bool stayOpen = Config.GetInt(Config.Key.CmdDebugMode) == 2;

            if (stayOpen)
                return "/K";
            else
                return "/C";
        }
    }
}
