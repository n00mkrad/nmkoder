using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Utils
{
    class MiscUtils
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetScaleFilter(string w, string h, bool logWarnings = true)
        {
            string argW = w.Replace("w", "iw").Replace("h", "ih");
            string argH = h.Replace("w", "iw").Replace("h", "ih");

            if (w.EndsWith("%"))
                argW = $"iw*{((float)w.GetInt() / 100).ToStringDot()}";
            else if (string.IsNullOrWhiteSpace(w))
                argW = "-2";

            if (h.EndsWith("%"))
                argH = $"ih*{((float)h.GetInt() / 100).ToStringDot()}";
            else if (string.IsNullOrWhiteSpace(h))
                argH = "-2";

            string forceDiv = (argW.Contains("*") || argH.Contains("*")) ? ":force_original_aspect_ratio=increase:force_divisible_by=2" : "";

            if (logWarnings && forceDiv.Length > 0 && (argW.Contains("*") && argH.Contains("*")))
                Logger.Log($"Info: Scaling using percentages enforces the original aspect ratio. You cannot use different percentages for width and height.");

            return $"scale={argW}:{argH}{forceDiv},setsar=1:1";
        }

        public static Fraction GetFpsFromString (string str)
        {
            if (Program.mainForm.encVidFpsBox.Text.Contains("/"))   // Parse fraction
            {
                string[] split = str.Split('/');
                Fraction frac = new Fraction(split[0].GetInt(), split[1].GetInt());
                return frac;
            }
            else    // Parse float
            {
                str = str.TrimNumbers(true);
                return new Fraction(str.GetFloat());
            }

            return new Fraction();
        }
    }
}
