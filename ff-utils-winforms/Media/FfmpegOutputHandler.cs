using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.UI;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Nmkoder.Media.AvProcess;

namespace Nmkoder.Media
{
    class FfmpegOutputHandler
    {
        public static readonly string prefix = "[ffmpeg]";
        public static long overrideTargetDurationMs = -1;

        public static void LogOutput(string line, string[] ignoreStrings, ref string appendStr, string logFilename, LogMode logMode, bool showProgressBar)
        {
            if (RunTask.canceled || string.IsNullOrWhiteSpace(line) || line.Trim().Length < 1)
                return;

            bool hidden = logMode == LogMode.Hidden;

            if (HideMessage(line)) // Don't print certain warnings 
                hidden = true;

            bool replaceLastLine = logMode == LogMode.OnlyLastLine;

            if (line.Contains("time=") && (line.StartsWith("frame=") || line.StartsWith("size=")))
                line = FormatUtils.BeautifyFfmpegStats(line);

            appendStr += Environment.NewLine + line;
            Logger.Log($"{prefix} {line}", hidden, replaceLastLine, logFilename);

            if (!hidden && showProgressBar && line.Contains("Time:"))
            {
                Regex timeRegex = new Regex("(?<=Time:).*(?= )");
                UpdateFfmpegProgress(timeRegex.Match(line).Value);
            }

            string lineWithoutPath = RemoveStringsFromLine(line, ignoreStrings);
            string log = $"Last 4 log lines:\n{string.Join(Environment.NewLine, Logger.GetSessionLogLastLines("ffmpeg", 4))}"; 

            if (lineWithoutPath.Contains("Error ") || lineWithoutPath.Contains("Unable to ") || lineWithoutPath.Contains("Could not open file"))
            {
                RunTask.Cancel($"Error: {line}\n\n{log}");
                return;
            }

            if (lineWithoutPath.Contains("No NVENC capable devices found") || lineWithoutPath.MatchesWildcard("*nvcuda.dll*"))
            {
                RunTask.Cancel($"Error: {line}\n\nMake sure you have an NVENC-capable Nvidia GPU.");
                return;
            }

            if (lineWithoutPath.Contains("not currently supported in container") || lineWithoutPath.Contains("Unsupported codec id"))
            {
                RunTask.Cancel($"Error: {line}\n\nIt looks like you are trying to copy a stream into a container that doesn't support this codec.");
                return;
            }

            if (lineWithoutPath.Contains("Subtitle encoding currently only possible from text to text or bitmap to bitmap"))
            {
                RunTask.Cancel($"Error: {line}\n\nYou cannot encode image-based subtitles into text-based subtitles. Please use the Copy Subtitles option instead, with a compatible container.");
                return;
            }

            if (lineWithoutPath.Contains("Only VP8 or VP9 or AV1 video and Vorbis or Opus audio and WebVTT subtitles are supported for WebM"))
            {
                RunTask.Cancel($"Error: {line}\n\nIt looks like you are trying to copy an unsupported stream into WEBM!");
                return;
            }

            if (lineWithoutPath.MatchesWildcard("*codec*not supported*"))
            {
                RunTask.Cancel($"Error: {line}\n\nTry using a different codec.");
                return;
            }

            if (lineWithoutPath.Contains("GIF muxer supports only a single video GIF stream"))
            {
                RunTask.Cancel($"Error: {line}\n\nYou tried to mux a non-GIF stream into a GIF file.");
                return;
            }

            if (lineWithoutPath.Contains("Width and height of input videos must be same"))
            {
                RunTask.Cancel($"Error: {line}");
                return;
            }
        }

        private static string RemoveStringsFromLine (string s, string[] ignoreStrings)
        {
            foreach(string ign in ignoreStrings)
                s = s.Replace(ign, "");

            return s;
        }

        static void UpdateFfmpegProgress(string ffmpegTime)
        {
            try
            {
                if (TrackList.current == null && overrideTargetDurationMs < 0)
                    return;

                long currInDuration = overrideTargetDurationMs > 0 ? overrideTargetDurationMs : (TrackList.current != null ? TrackList.current.File.DurationMs : 0);

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

        static bool HideMessage(string msg)
        {
            string[] hiddenMsgs = new string[] { "can produce invalid output", "pixel format", "provided invalid", "Non-monotonous", "not enough frames to estimate rate", "invalid dropping", "message repeated", "missing, emulating" };

            foreach (string str in hiddenMsgs)
                if (msg.MatchesWildcard($"*{str}*"))
                    return true;

            return false;
        }
    }
}
