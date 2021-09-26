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
        public enum TaskType { ExtractFrames, ExtractOther, Encode, GetInfo, Merge, Other };
        public static TaskType lastTask = TaskType.Other;

        public static string lastOutputAv1an;
        public static string lastTempDirAv1an;

        public enum LogMode { Visible, OnlyLastLine, Hidden }
        public static LogMode currentLogMode;
        static bool showProgressBar;

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

        public static async Task RunFfmpeg(string args, LogMode logMode, TaskType taskType = TaskType.Other, bool progressBar = false)
        {
            await RunFfmpeg(args, "", logMode, defLogLevel, taskType, progressBar);
        }

        public static async Task RunFfmpeg(string args, LogMode logMode, string loglevel, TaskType taskType = TaskType.Other, bool progressBar = false)
        {
            await RunFfmpeg(args, "", logMode, loglevel, taskType, progressBar);
        }

        public static async Task RunFfmpeg(string args, string workingDir, LogMode logMode, TaskType taskType = TaskType.Other, bool progressBar = false)
        {
            await RunFfmpeg(args, workingDir, logMode, defLogLevel, taskType, progressBar);
        }

        public static async Task RunFfmpeg(string args, string workingDir, LogMode logMode, string loglevel, TaskType taskType = TaskType.Other, bool progressBar = false)
        {
            bool show = Config.GetInt(Config.Key.cmdDebugMode) > 0;
            lastOutputAv1an = "";
            currentLogMode = logMode;
            showProgressBar = progressBar;
            Process ffmpeg = OsUtils.NewProcess(!show);
            timeSinceLastOutput.Restart();
            lastAvProcess = ffmpeg;
            lastTask = taskType;

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
                ffmpeg.OutputDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, "ffmpeg", showProgressBar); };
                ffmpeg.ErrorDataReceived += (sender, outLine) => { FfmpegOutputHandler.LogOutput(outLine.Data, "ffmpeg", showProgressBar); };
            }
            
            ffmpeg.Start();
            ffmpeg.PriorityClass = ProcessPriorityClass.BelowNormal;

            if (!show)
            {
                ffmpeg.BeginOutputReadLine();
                ffmpeg.BeginErrorReadLine();
            }
            

            while (!ffmpeg.HasExited)
                await Task.Delay(1);

            if (progressBar)
                Program.mainForm.SetProgress(0);
        }

        public static async Task<string> GetFfmpegOutputAsync(string args, bool setBusy = false, bool progressBar = false)
        {
            timeSinceLastOutput.Restart();
            if (Program.busy) setBusy = false;
            lastOutputAv1an = "";
            showProgressBar = progressBar;
            Process ffmpeg = OsUtils.NewProcess(true);
            lastAvProcess = ffmpeg;
            ffmpeg.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & ffmpeg.exe -hide_banner -y -stats {args}";
            Logger.Log($"ffmpeg {args}", true, false, "ffmpeg");
            if (setBusy) Program.mainForm.SetWorking(true);
            lastOutputAv1an = await OsUtils.GetOutputAsync(ffmpeg);
            while (!ffmpeg.HasExited) await Task.Delay(50);
            while (timeSinceLastOutput.ElapsedMilliseconds < 200) await Task.Delay(50);
            if (setBusy) Program.mainForm.SetWorking(false);
            return lastOutputAv1an;
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

        public static void UpdateFfmpegProgress(string ffmpegTime)
        {
            try
            {
                if (MediaInfo.current == null)
                    return;

                long currInDuration = MediaInfo.current.DurationMs;

                if (currInDuration < 1)
                {
                    Program.mainForm.SetProgress(0);
                    return;
                }

                long total = currInDuration / 100;
                long current = FormatUtils.TimestampToMs(ffmpegTime);
                int progress = Convert.ToInt32(current / total);
                Program.mainForm.SetProgress(progress);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to get ffmpeg progress: {e.Message}", true);
            }
        }

        #endregion

        #region av1an

        public static async Task RunAv1an(string args, LogMode logMode, bool progressBar = false)
        {
            await RunAv1an(args, "", logMode, progressBar);
        }

        public static async Task RunAv1an(string args, string workingDir, LogMode logMode, bool progressBar = false)
        {
            string dir = Path.Combine(GetDir(), "av1an");
            IoUtils.TryDeleteIfExists(Paths.GetAv1anTempPath());
            string tempDir = Path.Combine(Paths.GetAv1anTempPath(), ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString());
            Directory.CreateDirectory(tempDir);
            bool show = Config.GetInt(Config.Key.cmdDebugMode) > 0;
            lastTempDirAv1an = tempDir;
            lastOutputAv1an = "";
            currentLogMode = logMode;
            showProgressBar = progressBar;
            Process av1an = OsUtils.NewProcess(!show);
            timeSinceLastOutput.Restart();
            lastAvProcess = av1an;

            string beforeArgs = $"--temp {tempDir.Wrap()}";

            av1an.StartInfo.Arguments = $"{GetCmdArg()} cd /D {dir.Wrap()} & av1an.exe {beforeArgs} {args}";

            if (logMode != LogMode.Hidden) Logger.Log("Running av1an...", false);
            Logger.Log($"av1an {beforeArgs} {args}", true, false, "av1an");

            if (!show)
            {
                av1an.OutputDataReceived += (sender, outLine) => { Av1anOutputHandler.LogOutput(outLine.Data, "av1an", showProgressBar); };
                av1an.ErrorDataReceived += (sender, outLine) => { Av1anOutputHandler.LogOutput(outLine.Data, "av1an", showProgressBar); };
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
                await Task.Delay(1);

            if (progressBar)
                Program.mainForm.SetProgress(0);

            //if (Directory.Exists(tempDir))
            //    Av1anUi.AskDeleteTempFolder(tempDir);
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
