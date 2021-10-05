using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class OcrProcess
    {
        public static Process lastOcrProcess;
        public static string lastOutputSubEdit;

        public static void Kill()
        {
            if (lastOcrProcess == null) return;

            try
            {
                OsUtils.KillProcessTree(lastOcrProcess.Id);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to kill lastOcrProcess process tree: {e.Message}", true);
            }
        }

        public static async Task RunSubtitleEdit(string args, bool hidden = false)
        {
            bool show = Config.GetInt(Config.Key.cmdDebugMode) > 0;
            lastOutputSubEdit = "";
            Process subEdit = OsUtils.NewProcess(!show);
            //timeSinceLastOutput.Restart();
            lastOcrProcess = subEdit;
            //lastTask = taskType;


            subEdit.StartInfo.Arguments = $"{GetCmdArg()} cd /D {GetDir().Wrap()} & SubtitleEdit {args}";

            if (true /* logMode != LogMode.Hidden */) Logger.Log("Running OCR...", false);
            Logger.Log($"SubtitleEdit {args}", true, false, "ocr");

            if (!show)
            {
                subEdit.OutputDataReceived += (sender, outLine) => { LogOutput(outLine.Data, !hidden); };
                subEdit.ErrorDataReceived += (sender, outLine) => { LogOutput(outLine.Data, !hidden); };
            }

            subEdit.Start();
            subEdit.PriorityClass = ProcessPriorityClass.BelowNormal;

            if (!show)
            {
                subEdit.BeginOutputReadLine();
                subEdit.BeginErrorReadLine();
            }

            while (!subEdit.HasExited)
                await Task.Delay(1);

            if (hidden)
                Program.mainForm.SetProgress(0);
        }

        public static void LogOutput(string line, bool hidden)
        {
            //timeSinceLastOutput.Restart();

            if (RunTask.canceled || string.IsNullOrWhiteSpace(line))
                return;

            lastOutputSubEdit = lastOutputSubEdit + "\n" + line;

            //bool hidden = currentLogMode == LogMode.Hidden;
            //
            //if (HideMessage(line)) // Don't print certain warnings 
            //    hidden = true;

            bool replaceLastLine = true; //currentLogMode == LogMode.OnlyLastLine;

            Logger.Log(line, true, replaceLastLine, "ocr");

            if (!hidden && line.Contains("OCR... :"))
            {
                int percent = line.Split(':').LastOrDefault().GetInt();
                Logger.Log($"Running Optical Character Recognition: {percent}%", false, Logger.GetLastLine().EndsWith("%"));
                Program.mainForm.SetProgress(percent);
            }
                
        }

        static string GetDir()
        {
            return Path.Combine(Paths.GetBinPath(), "SE");
        }

        public static string GetCmdArg()
        {
            bool stayOpen = Config.GetInt(Config.Key.cmdDebugMode) == 2;

            if (stayOpen)
                return "/K";
            else
                return "/C";
        }
    }
}
