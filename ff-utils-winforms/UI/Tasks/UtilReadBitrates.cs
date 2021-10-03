using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.Utils;
using Stream = Nmkoder.Data.Streams.Stream;

namespace Nmkoder.UI.Tasks
{
    class UtilReadBitrates
    {
        public static async Task Run()
        {
            Program.mainForm.SetWorking(true);
            Logger.Log("Analyzing streams... This can take a few minutes on slower hard drives.");

            foreach (MediaStreamListEntry entry in Program.mainForm.streamListBox.Items)
            {
                if (RunTask.canceled || !Program.mainForm.streamListBox.GetItemChecked(Program.mainForm.streamListBox.Items.IndexOf(entry)))
                    continue;

                Stream s = entry.Stream;
                FfmpegUtils.StreamSizeInfo info = await FfmpegUtils.GetStreamSizeBytes(TrackList.current.TruePath, s.Index);
                string percent = FormatUtils.RatioInt(info.Bytes, TrackList.current.Size).ToString("0.0");
                string br = info.Kbps > 1 ? FormatUtils.Bitrate(info.Kbps.RoundToInt()) : info.Kbps.ToString("0.0") + " kbps";
                Logger.Log($"Stream #{s.Index} ({s.Type}) - Bitrate: {br} - Size: {FormatUtils.Bytes(info.Bytes)} ({percent}%)");
            }

            Program.mainForm.SetWorking(false);
        }
    }
}
