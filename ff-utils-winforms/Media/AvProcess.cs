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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class AvProcess
    {
        public static Process lastAvProcess;
        public static Stopwatch timeSinceLastOutput = new Stopwatch();

        public static string lastOutputAv1an;
        public static string lastTempDirAv1an;

        public enum LogMode { Visible, OnlyLastLine, Hidden }
        //public static LogMode currentLogMode;
        //static bool showProgressBar;

        static readonly string defLogLevel = "warning";

        public static void Kill()
        {
            if (lastAvProcess == null) return;

            try
            {
                OsUtils.KillProcessTree(lastAvProcess.Id);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to kill lastAvProcess process tree: {e.Message}", true);
            }
        }

        #region FFmpeg

        public static async Task<string> RunFfmpeg(string args, LogMode logMode, bool reliableOutput = false, bool progressBar = false)
        {
            return await RunFfmpeg(args, "", logMode, defLogLevel, reliableOutput, progressBar);
        }

        public static async Task<string> RunFfmpeg(string args, LogMode logMode, string loglevel, bool reliableOutput = false, bool progressBar = false)
        {
            return await RunFfmpeg(args, "", logMode, loglevel, reliableOutput, progressBar);
        }

        public static async Task<string> RunFfmpeg(string args, string workingDir, LogMode logMode, bool reliableOutput = false, bool progressBar = false)
        {
            return await RunFfmpeg(args, workingDir, logMode, defLogLevel, reliableOutput, progressBar);
        }

        public static async Task<string> RunFfmpeg(string args, string workingDir, LogMode logMode, string loglevel, bool reliableOutput = false, bool progressBar = false)
        {
            bool show = Config.GetInt(Config.Key.cmdDebugMode) > 0;
            string processOutput = "";
            Process ffmpeg = OsUtils.NewProcess(!show);
            timeSinceLastOutput.Restart();
            lastAvProcess = ffmpeg;

            if (string.IsNullOrWhiteSpace(loglevel))
                loglevel = defLogLevel;

            string beforeArgs = $"-hide_banner -stats -loglevel {loglevel} -y";

            if (!string.IsNullOrWhiteSpace(workingDir))
                ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {workingDir.Wrap()} & {Path.Combine(GetDir(), "ffmpeg.exe").Wrap()} {beforeArgs} {args}";
            else
                ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & ffmpeg.exe {beforeArgs} {args}";

            if (logMode != LogMode.Hidden) Logger.Log("Running FFmpeg...", false);
            Logger.Log($"ffmpeg {beforeArgs} {args}", true, false, "ffmpeg");

            if (!show)
            {
                ffmpeg.OutputDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, ref processOutput, "ffmpeg", logMode, progressBar); };
                ffmpeg.ErrorDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, ref processOutput, "ffmpeg", logMode, progressBar); };
            }

            ffmpeg.Start();
            ffmpeg.PriorityClass = ProcessPriorityClass.BelowNormal;

            if (!show)
            {
                ffmpeg.BeginOutputReadLine();
                ffmpeg.BeginErrorReadLine();
            }

            //while (!ffmpeg.HasExited)
            //    await Task.Delay(10);

            while (!ffmpeg.HasExited) await Task.Delay(10);
            while (reliableOutput && timeSinceLastOutput.ElapsedMilliseconds < 200) await Task.Delay(50);

            if (progressBar)
                Program.mainForm.SetProgress(0);

            return processOutput;
        }

        public static async Task<string> GetFfmpegOutputAsync(string args, bool setBusy = false, bool progressBar = false)
        {
            if (setBusy) Program.mainForm.SetWorking(true);
            return await RunFfmpeg(args, null, LogMode.OnlyLastLine, "error", true, progressBar);
            if (setBusy) Program.mainForm.SetWorking(false);
            //timeSinceLastOutput.Restart();
            //if (Program.busy) setBusy = false;
            //lastOutputFfmpeg = "";
            //showProgressBar = progressBar;
            //Process ffmpeg = OsUtils.NewProcess(true);
            //lastAvProcess = ffmpeg;
            //ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & ffmpeg.exe -hide_banner -y -stats {args}";
            //Logger.Log($"ffmpeg {args}", true, false, "ffmpeg");
            //if (setBusy) Program.mainForm.SetWorking(true);
            //lastOutputFfmpeg = await OsUtils.GetOutputAsync(ffmpeg);
            //while (!ffmpeg.HasExited) await Task.Delay(50);
            //while (timeSinceLastOutput.ElapsedMilliseconds < 200) await Task.Delay(50);
            //if (setBusy) Program.mainForm.SetWorking(false);
            //return lastOutputFfmpeg;
        }

        public static string GetFfprobeOutput(string args)
        {
            Process ffprobe = OsUtils.NewProcess(true);
            ffprobe.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & ffprobe.exe {args}";
            Logger.Log($"ffprobe {args}", true, false, "ffmpeg");
            ffprobe.Start();
            ffprobe.WaitForExit();
            string output = ffprobe.StandardOutput.ReadToEnd();
            string err = ffprobe.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err)) output += "\n" + err;
            return output;
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
                IoUtils.TryDeleteIfExists(Paths.GetAv1anTempPath());
                string tempDir = Path.Combine(Paths.GetAv1anTempPath(), ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString());
                Directory.CreateDirectory(tempDir);
                bool show = Config.GetBool(Config.Key.av1anCmdVisible, true); // = Config.GetInt(Config.Key.cmdDebugMode) > 0;
                lastTempDirAv1an = tempDir;
                lastOutputAv1an = "";
                Process av1an = OsUtils.NewProcess(!show);
                timeSinceLastOutput.Restart();
                lastAvProcess = av1an;

                string beforeArgs = $"--temp {tempDir.Wrap()}";
                string vsynthPath = Path.Combine(dir, "vsynth");
                string encPath = Path.Combine(dir, "enc");
                string ffmpegPath = Paths.GetBinPath();

                if (!show)
                    av1an.StartInfo.EnvironmentVariables["Path"] = av1an.StartInfo.EnvironmentVariables["Path"] + $";{vsynthPath};{encPath};{ffmpegPath}";

                av1an.StartInfo.Arguments = $"/K cd /D {dir.Wrap()} && SET PATH=%PATH%;{vsynthPath};{encPath};{ffmpegPath} && av1an.exe {beforeArgs} {args}";

                if (logMode != LogMode.Hidden) Logger.Log("Running av1an...", false);

                if (!show)
                {
                    av1an.OutputDataReceived += (sender, outLine) => { Av1anOutputHandler.LogOutput(outLine.Data, "av1an", logMode, progressBar); };
                    av1an.ErrorDataReceived += (sender, outLine) => { Av1anOutputHandler.LogOutput(outLine.Data, "av1an", logMode, progressBar); };
                }

                Task.Run(() => Av1anOutputHandler.ParseProgressLoop());
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

        #endregion

        #region MKVExtract

        public static async Task<string> RunMkvExtract(string args)
        {
            bool show = Config.GetInt(Config.Key.cmdDebugMode) > 0;
            string processOutput = "";

            try
            {
                Process mkve = OsUtils.NewProcess(!show);

                mkve.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & mkvextract.exe {args}";

                Logger.Log($"mkvextract {args}", true, false, "mkvextract");

                mkve.OutputDataReceived += (sender, outLine) => { processOutput += outLine.Data; Logger.Log($"[mkvextract] {outLine.Data}", true, false, "ocr"); };
                mkve.ErrorDataReceived += (sender, outLine) => { processOutput += outLine.Data; };

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

        #endregion

        static string GetDir()
        {
            return Paths.GetBinPath();
        }

        public static string GetCmdArg()
        {
            bool stayOpen = Config.GetInt(Config.Key.cmdDebugMode) == 2;

            if (stayOpen)
                return "/K";
            else
                return "/C";
        }

        public static async Task SetBusyWhileRunning()
        {
            if (Program.busy) return;

            await Task.Delay(100);
            while (!lastAvProcess.HasExited)
                await Task.Delay(10);
        }
    }
}
