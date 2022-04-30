using Nmkoder.Data;
using Nmkoder.Data.Codecs;
using Nmkoder.Data.Streams;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Utils
{
    class BitrateCalculation
    {
        public static int GetTargetSizeKbps(IEncoder aCodec, bool silent = false)
        {
            MainForm form = Program.mainForm;
            bool aud = !aCodec.DoesNotEncode;

            string audArgs = CodecUtils.GetAudioArgsForEachStream(TrackList.current.File, (int)form.encAudQualUpDown.Value, form.encAudChannelsBox.Text.Split(' ')[0].GetInt());

            List<int> audioBitrates = audArgs.Split("-b:a:").Where(x => x.Contains("k ")).Select(x => x.Split(' ')[1].GetInt()).ToList(); //aud ? ((int)form.encAudQualUpDown.Value * 1024) * audioTracks : 0;
            int audioBps = audioBitrates.Select(x => x * 1024).Sum();

            double durationSecs = TrackList.current.File.DurationMs / (double)1000;
            float targetMbytes = form.encVidQualityBox.Text.GetFloat();
            long targetBits = (long)Math.Round(targetMbytes * 8 * 1024 * 1024);
            int targetVidBitrate = (int)Math.Floor(targetBits / durationSecs) - audioBps; // Round down since undershooting is better than overshooting here

            string brTotal = (((float)targetVidBitrate + audioBps) / 1024).ToString("0.0");
            string brVid = ((float)targetVidBitrate / 1024).ToString("0");
            string brAud = ((float)audioBps / 1024).ToString("0");

            if (targetVidBitrate < 0)
            {
                RunTask.Cancel($"Target Filesize Mode:\n\nNo bitrate left for video ({brVid}k) after {audioBitrates.Count} audio tracks ({string.Join(" + ", audioBitrates.Select(x => $"{x}k"))} = {brAud}k)." +
                    $"\n\nUse a lower audio bitrate or fewer/no audio tracks.");
                return -1;
            }

            if (!silent)
                Logger.Log($"Target Filesize Mode: Using bitrate of {brTotal} kbps ({brVid}k Video, {brAud}k Audio) over {durationSecs.ToString("0.0")} seconds to hit {targetMbytes} megabytes.");

            return ((float)targetVidBitrate / 1024).RoundToInt();
        }
    }
}
