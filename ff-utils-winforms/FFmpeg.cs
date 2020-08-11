using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ff_utils_winforms
{
    class FFmpeg
    {
        public static void Run (string args)
        {
            string ffmpegPath = IOUtils.GetFfmpegExePath();
            string ffmpegDir = Path.GetDirectoryName(ffmpegPath);
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.RedirectStandardError = true;
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.FileName = "cmd.exe";
            ffmpeg.StartInfo.Arguments = "/C cd /D \"" + ffmpegDir + "\" & ffmpeg -hide_banner -loglevel warning -y -stats " + args;
            Program.Print("Running ffmpeg...");
            Program.Print("cmd.exe " + ffmpeg.StartInfo.Arguments);
            ffmpeg.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            ffmpeg.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            ffmpeg.Start();
            ffmpeg.BeginOutputReadLine();
            ffmpeg.BeginErrorReadLine();
            ffmpeg.WaitForExit();
            Program.Print("Done running ffmpeg.");
        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Program.Print(outLine.Data);
        }
    }
}
