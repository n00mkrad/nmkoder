using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Utils
{
    class BitratePlottingUtils
    {
        public class Frame
        {
            public enum FrameType { Intra, Bidirectional, Predictive, Unknown }
            public float Timestamp { get; set; }
            public long Size { get; set; }
            public FrameType Type { get; set; }

            public Frame (string timestamp, string size, string type)
            {
                Timestamp = float.Parse(timestamp, CultureInfo.InvariantCulture.NumberFormat);
                Size = long.Parse(size);

                if (type == "I")
                    Type = FrameType.Intra;
                else if (type == "I")
                    Type = FrameType.Bidirectional;
                else if (type == "P")
                    Type = FrameType.Predictive;
                else
                    Type = FrameType.Unknown;
            }
        }

        public static async Task<List<Frame>> GetFrameInfos (string videoPath, bool progressBar = false)
        {
            List<Frame> frames = new List<Frame>();

            try
            {
                NmkdStopwatch timeSinceLastPrint = new NmkdStopwatch();
                float durationSecs = await FfmpegCommands.GetDurationMs(videoPath) / 1000f;
                string t = $"-threads {Environment.ProcessorCount}";
                string args = $"-select_streams v:0 {t} -print_format xml -show_entries frame=pict_type,pkt_pts_time,best_effort_timestamp_time,pkt_size";
                Task<string> task = Task.Run(() => AvProcess.RunFfprobe($"{args} {videoPath.Wrap()}", AvProcess.LogMode.Hidden));

                while (!task.IsCompleted)
                {
                    await Task.Delay(500);

                    try
                    {
                        string line = Logger.LastLogLine;

                        if (line.Contains("time=\""))
                        {
                            float secs = float.Parse(line.Split("timestamp_time=\"")[1].Split('"')[0], CultureInfo.InvariantCulture.NumberFormat);
                            int prc = ((float)(secs) / (durationSecs) * 100).RoundToInt();
                            Program.mainForm.SetProgress(prc);
                            Logger.Log($"Analyzed {secs}/{durationSecs} seconds ({prc}%)...", false, Logger.LastUiLine.EndsWith("..."));
                        }
                    }
                    catch { }
                }

                await Task.Delay(500);
                string[] outputLines = task.Result.SplitIntoLines();

                for (int i = 0; i < outputLines.Length; i++)
                {
                    string line = outputLines[i];

                    if (!line.Contains("timestamp_time=") || !line.Contains("pkt_size="))
                        continue;

                    string timestamp = line.Split("timestamp_time=\"")[1].Split('"')[0];
                    string pktBytes = line.Split("pkt_size=\"")[1].Split('"')[0];
                    string pktType = line.Contains("pict_type=") ? line.Split("pict_type=\"")[1].Split('"')[0] : "";

                    frames.Add(new Frame(timestamp, pktBytes, pktType));
                }

                Logger.Log($"Analyzed all frames.", false, Logger.LastUiLine.EndsWith("..."));
            }
            catch(Exception e)
            {
                Logger.Log($"GetFrameInfos Error: {e.Message}", true);
            }

            return frames;
        }

        public static Dictionary<long, long> GetBytesPerSecond(List<Frame> frameList)
        {
            Dictionary<long, long> seconds = new Dictionary<long, long>();

            if (frameList == null || frameList.Count == 0)
                return seconds;

            //long currentSec = (long)Math.Truncate(frameList[0].Timestamp);

            foreach (Frame f in frameList)  
            {
                long second = (long)Math.Truncate(f.Timestamp);

                if (seconds.ContainsKey(second))
                    seconds[second] = seconds[second] + f.Size;
                else
                    seconds[second] = f.Size;
            }

            return seconds;
        }

        public static void PrintBitrateInfo (Dictionary<long, long> seconds)
        {
            foreach (var pair in seconds)
            {
                var kbps = ((float)pair.Value * 8 / 1024);
                Logger.Log($"Second {pair.Key} => {kbps} kbps");
            }

            Logger.Log($"Min: {seconds.Min(x => BitsToKbytes(x.Value))} kbps");
            Logger.Log($"Max: {seconds.Max(x => BitsToKbytes(x.Value))} kbps");
            Logger.Log($"Avg: {seconds.Average(x => BitsToKbytes(x.Value))} kbps");
        }

        public static int BitsToKbytes(long bits)
        {
            return ((float)bits * 8 / 1024).RoundToInt();
        }
    }
}
