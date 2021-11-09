using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Utils
{
    class ColorDataUtils
    {
        public static async Task<VideoColorData> GetColorData(string path)
        {
            VideoColorData data = new VideoColorData();

            string infoFfprobe = AvProcess.GetFfprobeOutput($"-v quiet -show_frames -read_intervals \"%+#1\" {path.Wrap()}");

            if (infoFfprobe.Contains("side_data_type=Mastering display metadata"))
            {
                string[] linesFfprobe = infoFfprobe.Split("side_data_type=Mastering display metadata")[1].Split("[/SIDE_DATA]")[0].SplitIntoLines();

                foreach (string line in linesFfprobe)
                {
                    if (line.Contains("red_x="))
                        data.RedX = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("red_y="))
                        data.RedY = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("green_x="))
                        data.GreenX = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("green_y="))
                        data.GreenY = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("blue_x="))
                        data.BlueY = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("blue_y="))
                        data.BlueX = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("white_point_x="))
                        data.WhiteY = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("white_point_y="))
                        data.WhiteX = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("max_luminance="))
                        data.LumaMax = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();

                    else if (line.Contains("min_luminance="))
                        data.LumaMin = line.Contains("/") ? FractionToFloat(line.Split('=').Last()) : line.Split('=').Last();
                }
            }

            string infoMkvinfo = await AvProcess.RunMkvInfo($"{path.Wrap()}");
            
            if (infoMkvinfo.Contains("+ Video track"))
            {
                string[] lines = infoMkvinfo.Split("+ Video track")[1].Split("+ Track")[0].Split("+ Tags")[0].SplitIntoLines();

                foreach (string line in lines)
                {
                    if (line.Contains("+ Colour transfer:"))
                        data.ColorTransfer = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Colour matrix coefficients:"))
                        data.ColorMatrixCoeffs = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Colour primaries:"))
                        data.ColorPrimaries = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Colour range:"))
                        data.ColorRange = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Red colour coordinate x:"))
                        data.RedX = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Red colour coordinate y:"))
                        data.RedY = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Green colour coordinate x:"))
                        data.GreenX = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Green colour coordinate y:"))
                        data.GreenY = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Blue colour coordinate y:"))
                        data.BlueY = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Blue colour coordinate x:"))
                        data.BlueX = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ White colour coordinate y:"))
                        data.WhiteY = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ White colour coordinate x:"))
                        data.WhiteX = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Maximum luminance:"))
                        data.LumaMax = ValidateNumber(line.Split(':')[1]);

                    else if (line.Contains("+ Minimum luminance:"))
                        data.LumaMin = ValidateNumber(line.Split(':')[1]);
                }
            }

            return data;
        }

        private static string FractionToFloat(string fracString)
        {
            string[] fracNums = fracString.Split('/');
            return ((float)fracNums[0].GetInt() / (float)fracNums[1].GetInt()).ToString("0.#######", new CultureInfo("en-US"));
        }

        private static string ValidateNumber(string numStr)
        {
            return Double.Parse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture).ToString("0.#######", new CultureInfo("en-US"));
        }

        public static async Task SetColorData(string path, VideoColorData d)
        {
            try
            {
                string tmpPath = IoUtils.FilenameSuffix(path, ".tmp");
                string args = $"-o {tmpPath.Wrap()} " +
                    $"--colour-matrix 0:{d.ColorMatrixCoeffs} " +
                    $"--colour-range 0:{d.ColorRange} " +
                    $"--colour-transfer-characteristics 0:{d.ColorTransfer} " +
                    $"--colour-primaries 0:{d.ColorPrimaries} " +
                    $"--max-luminance 0:{d.LumaMax} " +
                    $"--min-luminance 0:{d.LumaMin} " +
                    $"--chromaticity-coordinates 0:{d.RedX},{d.RedY},{d.GreenX},{d.GreenY},{d.BlueX},{d.BlueY} " +
                    $"--white-colour-coordinates 0:{d.WhiteX},{d.WhiteY} " +
                    $"{path.Wrap()}";

                await AvProcess.RunMkvMerge(args, true);

                //long filesizeDiff = Math.Abs(new FileInfo(path).Length - new FileInfo(tmpPath).Length);
                float filesizeFactor = new FileInfo(tmpPath).Length / new FileInfo(path).Length;
                Logger.Log($"{MethodBase.GetCurrentMethod().DeclaringType}: Filesize ratio of remuxed file against original: {filesizeFactor}");

                if (filesizeFactor < 0.95f || filesizeFactor > 1.05f)
                {
                    Logger.Log($"Warning: Output file size is not within 5% of the original size! Won't delete original to be sure.");
                }
                else
                {
                    File.Delete(path);
                    File.Move(tmpPath, path);
                }
            }
           catch(Exception e)
            {
                Logger.Log($"SetColorData Error: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
