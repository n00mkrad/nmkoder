using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.UI;
using Nmkoder.UI.Tasks;
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

            if (lineWithoutPath.Contains("Unknown pixel format"))
            {
                RunTask.Cancel($"Error: {line}");
                return;
            }

            if (lineWithoutPath.Contains("exactly one stream"))
            {
                RunTask.Cancel($"Error: {line}\n\nYou cannot mux multiple tracks into this container.");
                return;
            }

            List<string> genericErrors = new List<string>() { "Error ", "Unable to ", "Could not open file", "Failed " };

            if (genericErrors.Any(x => lineWithoutPath.Contains(x)))
            {
                RunTask.Cancel($"Error: {line}\n\n{log}");
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

                long durationMs = 0;
                
                if(overrideTargetDurationMs > 0)
                {
                    durationMs = overrideTargetDurationMs;
                }
                else if(QuickConvertUi.currentTrim != null && !QuickConvertUi.currentTrim.IsUnset)
                {
                    if(QuickConvertUi.currentTrim.TrimMode == Forms.TrimForm.TrimSettings.Mode.FrameNumbers)
                    {
                        if (TrackList.current != null && TrackList.current.File.VideoStreams.Count > 0)
                            durationMs = 1000 * ((double)QuickConvertUi.currentTrim.Duration * (1f / TrackList.current.File.VideoStreams[0].Rate.GetFloat())).RoundToLong();
                    }
                    else
                    {
                        durationMs = QuickConvertUi.currentTrim.Duration;
                    }
                    
                }
                else if (TrackList.current != null)
                {
                    durationMs = TrackList.current.File.DurationMs;
                }

                if (durationMs < 1)
                {
                    Program.mainForm.SetProgress(0);
                    return;
                }

                long currentMs = FormatUtils.TimestampToMs(ffmpegTime);
                int progress = (((double)currentMs / (double)durationMs) * (double)100).RoundToInt();
                Program.mainForm.SetProgress(progress);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to get ffmpeg progress: {e.Message}", true);
            }
        }

        static bool HideMessage(string msg)
        {
            string[] hiddenMsgs = new string[] { 
                "can produce invalid output", 
                "pixel format", 
                "provided invalid", 
                "Non-monotonous", 
                "not enough frames to estimate rate", 
                "invalid dropping", 
                "message repeated", 
                "missing, emulating",
                "Consider increasing the value"
            };

            foreach (string str in hiddenMsgs)
                if (msg.MatchesWildcard($"*{str}*"))
                    return true;

            return false;
        }
    }
}
