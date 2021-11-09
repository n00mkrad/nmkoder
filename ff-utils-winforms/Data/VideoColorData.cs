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
        public string ColorTransfer { get; set; } = "";
        public string ColorMatrixCoeffs { get; set; } = "";
        public string ColorPrimaries { get; set; } = "";
        public string ColorRange { get; set; } = "";
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
            return
            $"Color transfer: {ColorTransfer}" +
            $"\nColor matrix coefficients: {ColorMatrixCoeffs}" +
            $"\nColor primaries: {ColorPrimaries}" +
            $"\nColor range: {ColorRange}" +
            $"\nRed color coordinate x: {RedX}" +
            $"\nRed color coordinate y: {RedY}" +
            $"\nGreen color coordinate x: {GreenX}" +
            $"\nGreen color coordinate y: {GreenY}" +
            $"\nBlue color coordinate y: {BlueX}" +
            $"\nBlue color coordinate x: {BlueY}" +
            $"\nWhite color coordinate y: {WhiteX}" +
            $"\nWhite color coordinate x: {WhiteY}" +
            $"\nMaximum luminance: {LumaMax}" +
            $"\nMinimum luminance: {LumaMin}";
        }
    }
}
