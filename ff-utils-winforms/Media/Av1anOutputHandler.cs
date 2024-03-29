﻿using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
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

        public static void LogOutput(string line, string logFilename, LogMode logMode, bool showProgressBar)
        {
            if (RunTask.canceled || string.IsNullOrWhiteSpace(line) || line.Length < 6)
                return;

            lastOutputAv1an = lastOutputAv1an + "\n" + line;

            bool hidden = logMode == LogMode.Hidden;

            if (HideMessage(line)) // Don't print certain warnings 
                hidden = true;

            bool replaceLastLine = logMode == LogMode.OnlyLastLine;

            Logger.Log(line, hidden, replaceLastLine, "av1an");

            if (line.Contains("Could not open file"))
            {
                RunTask.Cancel($"Error: {line}");
                return;
            }
        }

        public static async Task ParseProgressLoop()
        {
            int workers = Config.GetInt(Config.Key.av1anOptsWorkerCount);
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

            NmkdStopwatch sw = new NmkdStopwatch();

            Dictionary<int, int> etas = new Dictionary<int, int>();

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

                    if(currentQueueSize == 0)
                    {
                        string[] sc = logLines.Where(x => x.Contains("SC: Now at ")).ToArray();
                        currentQueueSize = sc.Length > 0 ? sc[0].Split("SC: Now at ")[1].Split(' ')[0].GetInt() : 0;
                    }

                    int ratio = FormatUtils.RatioInt(encodedChunks, currentQueueSize);
                    Program.mainForm.SetProgress(ratio);

                    int etaSecs = 0;

                    if (etas.ContainsKey(encodedChunks))
                    {
                        etaSecs = etas[encodedChunks];
                    }
                    else
                    {
                        float secsPerChunk = ((float)sw.ElapsedMs / 1000) / encodedChunks;
                        etaSecs = ((currentQueueSize - encodedChunks) * secsPerChunk).RoundToInt();
                        etas[encodedChunks] = etaSecs;
                    }
                    
                    string etaStr = encodedChunks > workers ? $" ETA: <{FormatUtils.Time(new TimeSpan(0, 0, etaSecs), false)}" : "";

                    Logger.Log($"AV1AN is running - Encoded {encodedChunks}/{currentQueueSize} chunks ({ratio}%).{etaStr}", false, Logger.LastUiLine.Contains("Encoded"));

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
