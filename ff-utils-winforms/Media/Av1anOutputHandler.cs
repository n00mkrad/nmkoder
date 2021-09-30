using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Nmkoder.Media.AvProcess;

namespace Nmkoder.Media
{
    class Av1anOutputHandler
    {
        static int currentQueueSize;
        public static Task currentLogReaderTask;

        public static void LogOutput(string line, string logFilename, bool showProgressBar)
        {
            timeSinceLastOutput.Restart();

            if (RunTask.canceled || string.IsNullOrWhiteSpace(line) || line.Length < 6)
                return;

            lastOutputAv1an = lastOutputAv1an + "\n" + line;

            bool hidden = currentLogMode == LogMode.Hidden;

            if (HideMessage(line)) // Don't print certain warnings 
                hidden = true;

            bool replaceLastLine = currentLogMode == LogMode.OnlyLastLine;

            Logger.Log(line, hidden, replaceLastLine, "av1an");

            // if (!hidden && showProgressBar && line.Contains("Time:"))
            // {
            //     Regex timeRegex = new Regex("(?<=Time:).*(?= )");
            //     UpdateFfmpegProgress(timeRegex.Match(line).Value);
            // }

            if (line.StartsWith("Queue: "))
                TryParseQueueSize(line);


            if (line.Contains("Could not open file"))
            {
                RunTask.Cancel($"Error: {line}");
                return;
            }
        }

        static void TryParseQueueSize(string line)
        {
            try
            {
                currentQueueSize = line.Split("Queue:")[1].Split("Workers:")[0].GetInt();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to read av1an queue size from line: {line} ({e.Message})", true);
            }
        }

        public static async Task ParseProgressLoop()
        {
            string dir = AvProcess.lastTempDirAv1an;
            string logFile = Path.Combine(dir, "log.log");

            await Task.Delay(3000);

            while (!File.Exists(logFile))
            {
                for (int i = 100; i > 0; i--)
                {
                    if (!Program.busy) return;
                    await Task.Delay(10);
                }
            }

            while (File.Exists(logFile))
            {
                if (!Program.busy) return;

                try
                {
                    var stream = File.Open(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var sr = new StreamReader(stream);
                    string contents = sr.ReadToEnd();
                    string[] logLines = contents.SplitIntoLines();
                    int encodedChunks = logLines.Where(x => x.Contains("Done: ")).Count();
                    int ratio = FormatUtils.RatioInt(encodedChunks, currentQueueSize);
                    Program.mainForm.SetProgress(ratio);
                    Logger.Log($"AV1AN is running - Encoded {encodedChunks}/{currentQueueSize} chunks ({ratio}%).", false, Logger.GetLastLine().Contains("Encoded"));

                    for (int i = 100; i > 0; i--)
                    {
                        if (!Program.busy) return;
                        await Task.Delay(10);
                    }
                }
                catch (Exception e)
                {
                    Logger.Log($"Failed to get av1an progress from log file: {e.Message}\n{e.StackTrace}", true);


                    for (int i = 100; i > 0; i--)
                    {
                        if (!Program.busy) return;
                        await Task.Delay(10);
                    }
                }
            }
        }

        static bool HideMessage(string msg)
        {
            string[] hiddenMsgs = new string[] { };

            foreach (string str in hiddenMsgs)
                if (msg.MatchesWildcard($"*{str}*"))
                    return true;

            return false;
        }
    }
}
