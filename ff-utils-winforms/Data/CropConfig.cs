using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Nmkoder.Data
{
    public class CropConfig
    {
        public int CropLeft;
        public int CropRight;
        public int CropTop;
        public int CropBot;

        public CropConfig (int left = 0, int right = 0, int top = 0, int bot = 0)
        {
            CropLeft = left;
            CropRight = right;
            CropTop = top;
            CropBot = bot;
        }

        public string GetFilterArgs (Size dimensions)
        {
            var cropWidth = dimensions.Width - CropLeft - CropRight;
            var cropHeight = dimensions.Height - CropTop - CropBot;
            var cropX = CropLeft;
            var cropY = CropTop;
            return $"{cropWidth}:{cropHeight}:{cropX}:{cropY}";
        }

        public int GetCroppedWidth(Size dimensions)
        {
            return dimensions.Width - CropLeft - CropRight;
        }

        public int GetCroppedHeight(Size dimensions)
        {
            return dimensions.Height - CropTop - CropBot;
        }
    }
}
