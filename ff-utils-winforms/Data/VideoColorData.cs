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
            $"Colour transfer: {ColorTransfer}" +
            $"\nColour matrix coefficients: {ColorMatrixCoeffs}" +
            $"\nColour primaries: {ColorPrimaries}" +
            $"\nColour range: {ColorRange}" +
            $"\nRed colour coordinate x: {RedX}" +
            $"\nRed colour coordinate y: {RedY}" +
            $"\nGreen colour coordinate x: {GreenX}" +
            $"\nGreen colour coordinate y: {GreenY}" +
            $"\nBlue colour coordinate y: {BlueX}" +
            $"\nBlue colour coordinate x: {BlueY}" +
            $"\nWhite colour coordinate y: {WhiteX}" +
            $"\nWhite colour coordinate x: {WhiteY}" +
            $"\nMaximum luminance: {LumaMax}" +
            $"\nMinimum luminance: {LumaMin}";
        }
    }
}
