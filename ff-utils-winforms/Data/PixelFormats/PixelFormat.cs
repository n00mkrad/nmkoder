using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Colors
{
    public class PixelFormat
    {
        public string Name { get; }
        public string FriendlyName { get; }
        public string Channels { get; }
        public int[] Subsampling { get; }
        public int Depth { get; }

        public PixelFormat () { }

        public PixelFormat (string name, string channels, int depth, int[] subsampling = null)
        {
            Name = name;
            Channels = channels;
            Depth = depth;
            Subsampling = subsampling != null ? subsampling : new int[0];

            FriendlyName = $"{Channels.ToUpper()} - {((Subsampling == null || Subsampling.Length < 1) ? "" : $"{string.Join(":", Subsampling)} - ")}{Depth} bit";
        }
    }

    public enum PixelFormats { Yuv420P8, Yuva420P8, Yuv420P10, Yuv422P8, Yuv422P10, Yuv444P8, Yuv444P10, P010, Rgb24, Rgba, Rgb48, Rgba64 }
}
