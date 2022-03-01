using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PF = Nmkoder.Data.Colors.PixelFormats;

namespace Nmkoder.Data.Colors
{

    public class PixFmtUtils
    {
        public static PixelFormat GetFormat(PF fmt)
        {
            string name = "yuv420p";

            if (fmt == PF.Yuv420P8) name = "yuv420p";
            else if (fmt == PF.Yuv420P10) name = "yuv420p10le";

            string channels = "yuv";

            if (fmt == PF.Rgb24) channels = "rgb";
            else if (fmt == PF.Rgba) channels = "rgba";
            else if (fmt == PF.Yuva420P8) channels = "yuva";

            int depth = 8;

            if (fmt == PF.Yuv420P10 || fmt == PF.Yuv422P10 || fmt == PF.Yuv444P10 || fmt == PF.P010) depth = 10;
            else if (fmt == PF.P016) depth = 16;

            int[] subsampling = null;

            if (fmt == PF.Yuv420P8 || fmt == PF.Yuva420P8 || fmt == PF.Yuv420P10 || fmt == PF.P010 || fmt == PF.P016) subsampling = new int[] { 4, 2, 0 };
            else if (fmt == PF.Yuv422P8 || fmt == PF.Yuv422P10) subsampling = new int[] { 4, 2, 2 };
            else if (fmt == PF.Yuv444P8 || fmt == PF.Yuv444P10) subsampling = new int[] { 4, 4, 4 };

            return new PixelFormat(name, channels, depth, subsampling);
        }
    }
}
