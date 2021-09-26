using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Utils;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Nmkoder.Media.AvProcess;

namespace Nmkoder.Media
{
    class FfmpegOutputHandler
    {
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

            if (line.Contains("time=") && (line.StartsWith("frame=") || line.StartsWith("size=")))
                line = FormatUtils.BeautifyFfmpegStats(line);

            Logger.Log(line, hidden, replaceLastLine, "ffmpeg");

            if (!hidden && showProgressBar && line.Contains("Time:"))
            {
                Regex timeRegex = new Regex("(?<=Time:).*(?= )");
                UpdateFfmpegProgress(timeRegex.Match(line).Value);
            }

            if (line.Contains("Could not open file"))
            {
                RunTask.Cancel($"Error: {line}");
                return;
            }

            if (line.Contains("No NVENC capable devices found") || line.MatchesWildcard("*nvcuda.dll*"))
            {
                RunTask.Cancel($"Error: {line}\n\nMake sure you have an NVENC-capable Nvidia GPU.");
                return;
            }

            if (line.Contains("not currently supported in container") || line.Contains("Unsupported codec id"))
            {
                RunTask.Cancel($"Error: {line}\n\nIt looks like you are trying to copy a stream into a container that doesn't support this codec.");
                return;
            }

            if (line.Contains("Subtitle encoding currently only possible from text to text or bitmap to bitmap"))
            {
                RunTask.Cancel($"Error: {line}\n\nYou cannot encode image-based subtitles into text-based subtitles. Please use the Copy Subtitles option instead, with a compatible container.");
                return;
            }

            if (line.Contains("Only VP8 or VP9 or AV1 video and Vorbis or Opus audio and WebVTT subtitles are supported for WebM"))
            {
                RunTask.Cancel($"Error: {line}\n\nIt looks like you are trying to copy an unsupported stream into WEBM!");
                return;
            }

            if (line.MatchesWildcard("*codec*not supported*"))
            {
                RunTask.Cancel($"Error: {line}\n\nTry using a different codec.");
                return;
            }

            if (line.Contains("GIF muxer supports only a single video GIF stream"))
            {
                RunTask.Cancel($"Error: {line}\n\nYou tried to mux a non-GIF stream into a GIF file.");
                return;
            }
        }

        static bool HideMessage(string msg)
        {
            string[] hiddenMsgs = new string[] { "can produce invalid output", "pixel format", "provided invalid" };

            foreach (string str in hiddenMsgs)
                if (msg.MatchesWildcard($"*{str}*"))
                    return true;

            return false;
        }
    }
}
