using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    public class VideoColorData
    {
        public int ColorTransfer { get; set; } = 2;
        public int ColorMatrixCoeffs { get; set; } = 2;
        public int ColorPrimaries { get; set; } = 2;
        public int ColorRange { get; set; } = 0;
        public string RedX { get; set; } = "";
        public string RedY { get; set; } = "";
        public string GreenX { get; set; } = "";
        public string GreenY { get; set; } = "";
        public string BlueX { get; set; } = "";
        public string BlueY { get; set; } = "";
        public string WhiteX { get; set; } = "";
        public string WhiteY { get; set; } = "";
        public string LumaMin { get; set; } = "";
        public string LumaMax { get; set; } = "";

        public override string ToString()
        {
            //return
            //$"Color transfer: {ColorTransfer}" +
            //$"\nColor matrix coefficients: {ColorMatrixCoeffs}" +
            //$"\nColor primaries: {ColorPrimaries}" +
            //$"\nColor range: {ColorRange}" +
            //$"\nRed color coordinate x: {RedX}" +
            //$"\nRed color coordinate y: {RedY}" +
            //$"\nGreen color coordinate x: {GreenX}" +
            //$"\nGreen color coordinate y: {GreenY}" +
            //$"\nBlue color coordinate y: {BlueX}" +
            //$"\nBlue color coordinate x: {BlueY}" +
            //$"\nWhite color coordinate y: {WhiteX}" +
            //$"\nWhite color coordinate x: {WhiteY}" +
            //$"\nMaximum luminance: {LumaMax}" +
            //$"\nMinimum luminance: {LumaMin}";

            List<string> lines = new List<string>();

            try
            {
                lines.Add($"Color transfer: {ColorTransfer} ({ColorDataUtils.GetColorTransferName(ColorTransfer)})");
                lines.Add($"Colour matrix coefficients: {ColorMatrixCoeffs} ({ColorDataUtils.GetColorMatrixCoeffsName(ColorMatrixCoeffs)})");
                lines.Add($"Colour primaries: {ColorPrimaries} ({ColorDataUtils.GetColorPrimariesName(ColorPrimaries)})");
                lines.Add($"Colour range: {ColorRange} ({ColorDataUtils.GetColorRangeName(ColorRange)})");
                if (!string.IsNullOrWhiteSpace(RedX) && !string.IsNullOrWhiteSpace(RedY)) lines.Add($"Red color coordinates X/Y: {RedX}/{RedY}");
                if (!string.IsNullOrWhiteSpace(GreenX) && !string.IsNullOrWhiteSpace(GreenY)) lines.Add($"Green color coordinates X/Y: {GreenX}/{GreenY}");
                if (!string.IsNullOrWhiteSpace(BlueX) && !string.IsNullOrWhiteSpace(BlueY)) lines.Add($"Blue color coordinates X/Y: {BlueX}/{BlueY}");
                if (!string.IsNullOrWhiteSpace(WhiteX) && !string.IsNullOrWhiteSpace(WhiteY)) lines.Add($"White color coordinates X/Y: {WhiteX}/{WhiteY}");
                if (!string.IsNullOrWhiteSpace(LumaMin)) lines.Add($"Minimum luminance: {LumaMin}");
                if (!string.IsNullOrWhiteSpace(LumaMax)) lines.Add($"Maximum luminance: {LumaMax}");
            }
            catch { }

            return string.Join("\n", lines);
        }
    }
}
