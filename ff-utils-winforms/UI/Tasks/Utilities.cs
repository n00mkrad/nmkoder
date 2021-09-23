using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.Utils;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    class Utilities
    {
        public enum FrameFormat { PNG, JPG, WEBP }
        public static FrameFormat frameFormat = FrameFormat.PNG;

        public static async Task RunVideoToFrames()
        {
            Program.mainForm.SetWorking(true);

            string frameFolderPath = Path.ChangeExtension(MediaInfo.current.SourcePath, null) + "-frames";
            Directory.CreateDirectory(frameFolderPath);

            string imgArgs = GetImgArgs(frameFormat.ToString());
            string custIn = Program.mainForm.customArgsInBox.Text.Trim();
            string custOut = Program.mainForm.customArgsOutBox.Text.Trim();

            string args = $"{custIn} -i {MediaInfo.current.TruePath.Wrap()} {imgArgs} {custOut} \"{frameFolderPath}/%08d.{frameFormat.ToString().ToLower()}\"";
            Logger.Log($"Running:\nffmpeg {args}", true, false, "ffmpeg");

            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.OnlyLastLine, AvProcess.TaskType.ExtractFrames, true);

            Program.mainForm.SetWorking(false);
        }

        public static async Task RunReadBitrates()
        {
            Program.mainForm.SetWorking(true);
            Logger.Log("Analyzing streams... This can take a few minutes on slower hard drives.");

            foreach(MediaStreamListEntry entry in Program.mainForm.streamListBox.Items)
            {
                if (!Program.mainForm.streamListBox.GetItemChecked(Program.mainForm.streamListBox.Items.IndexOf(entry)))
                    continue;

                Stream s = entry.Stream;
                FfmpegUtils.StreamSizeInfo info = await FfmpegUtils.GetStreamSizeBytes(MediaInfo.current.TruePath, s.Index);
                string percent = FormatUtils.RatioInt(info.Bytes, MediaInfo.current.Size).ToString("0.0");
                string br = info.Kbps > 1 ? FormatUtils.Bitrate(info.Kbps.RoundToInt()) : info.Kbps.ToString("0.0") + " kbps";
                Logger.Log($"Stream #{s.Index} ({s.Type}) - Bitrate: {br} - Size: {FormatUtils.Bytes(info.Bytes)} ({percent}%)");
            }

            Program.mainForm.SetWorking(false);
        }

        static string GetImgArgs(string extension, bool includePixFmt = true, bool alpha = true)
        {
            extension = extension.ToLower().Remove(".").Replace("jpeg", "jpg");
            string pixFmt = "-pix_fmt rgb24";
            string args = "";

            if (extension.Contains("png"))
            {
                pixFmt = alpha ? "rgba" : "rgb24";
                args = $"-compression_level 3";
            }

            if (extension.Contains("jpg"))
            {
                pixFmt = "yuvj420p";
                args = $"-qmin 0 -q:v 0";
            }

            if (extension.Contains("webp"))
            {
                pixFmt = "yuv420p";
                args = $"-q:v 100";
            }

            if (includePixFmt)
                args += $" -pix_fmt {pixFmt}";

            return args;
        }
    }
}
