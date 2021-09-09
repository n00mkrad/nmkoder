using Nmkoder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class Encoders
    {
        public class Encoder
        {
            public string Name;
            public string FriendlyName;
            public string[] Presets;
            public string[] ColorFormats;

            public Encoder(string name, string friendlyName, string[] presets, string[] colorFormats)
            {
                Name = name;
                FriendlyName = friendlyName;
                Presets = presets;
                ColorFormats = colorFormats;
            }
        }

        public static string[] libxPresets = new string[] { "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" };
        public static string[] libxColors = new string[] { "yuv420p", "yuv444p", "yuv420p10le" };
        public static Encoder X265 = new Encoder("libx265", "X265", libxPresets, libxColors);
        public static Encoder X264 = new Encoder("libx264", "X264", libxPresets, libxColors);

        public static string[] vpxVp9Presets = new string[] { "0", "1", "2", "3", "4", "5" };
        public static Encoder VpxVp9 = new Encoder("libvpx-vp9", "VPX-VP9", vpxVp9Presets, libxColors);

        public static string[] svtAv1Presets = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        public static Encoder SvtAv1 = new Encoder("libsvtav1", "SVT-AV1", svtAv1Presets, new string[] { "yuv420p", "yuv420p10le" });

        //public static Encoder ProResKs = new Encoder("prores_ks", "ProRes", svtAv1Presets, new string[] { "yuv420p", "yuv420p10le" });
    }
}
