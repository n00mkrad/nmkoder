using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ff_utils_winforms
{
    public static class ExtensionMethods
    {
        public static string TrimNumbers(this string s, bool allowDotComma = false)
        {
            if (!allowDotComma)
                s = Regex.Replace(s, "[^0-9]", "");
            else
                s = Regex.Replace(s, "[^.,0-9]", "");
            return s.Trim();
        }

        public static int GetInt (this TextBox textbox)
        {
            return int.Parse(textbox.Text.TrimNumbers());
        }

        public static int GetInt (this ComboBox combobox)
        {
            return int.Parse(combobox.Text.TrimNumbers());
        }

        public static int GetInt(this string str)
        {
            if (str.Length < 1 || str == null)
                return 0;
            try { return int.Parse(str.TrimNumbers()); }
            catch (Exception e)
            {
                //Logger.Log("Failed to parse \"" + str + "\" to int: " + e, true);
                return 0;
            }
        }

        public static float GetFloat(this string str)
        {
            if (str.Length < 1 || str == null)
                return 0f;
            string num = str.TrimNumbers(true).Replace(",", ".");
            float value;
            float.TryParse(num, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
            return value;
        }

        public static float GetFloat(this TextBox textbox)
        {
            return textbox.Text.GetFloat();
        }

        public static float GetFloat(this ComboBox combobox)
        {
            return combobox.Text.GetFloat();
        }

        public static string Wrap(this string path, bool addSpaceFront = false, bool addSpaceEnd = false)
        {
            string s = "\"" + path + "\"";
            if (addSpaceFront)
                s = " " + s;
            if (addSpaceEnd)
                s = s + " ";
            return s;
        }

        public static string GetParentDir(this string path)
        {
            return Directory.GetParent(path).FullName;
        }

        public static string ToStringDot(this float f, string format = "")
        {
            if (string.IsNullOrWhiteSpace(format))
                return f.ToString().Replace(",", ".");
            else
                return f.ToString(format).Replace(",", ".");
        }

        public static string[] SplitIntoLines(this string str)
        {
            return Regex.Split(str, "\r\n|\r|\n");
        }

        public static string TrimWhitespacesSafe(this string str)      // Trim whitespaces, unless they are "quoted" (to avoid trimming file paths)
        {
            var result = str.Split('"').Select((element, index) => index % 2 == 0 ? element.TrimWhitespaces() : element.Wrap());
            string resultJoined = string.Join("", result);

            result = resultJoined.Split('\'').Select((element, index) => index % 2 == 0 ? element.TrimWhitespaces() : "'" + element + "'");
            resultJoined = string.Join("", result);

            return resultJoined;
        }

        public static string TrimWhitespaces(this string str)
        {
            if (str == null) return str;
            var newString = new StringBuilder();
            bool previousIsWhitespace = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsWhiteSpace(str[i]))
                {
                    if (previousIsWhitespace)
                        continue;
                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }
                newString.Append(str[i]);
            }
            return newString.ToString();
        }

        public static int RoundDiv2(this int n)     // Round to a number that's divisible by 2 (for h264)
        {
            int a = (n / 2) * 2;    // Smaller multiple
            int b = a + 2;   // Larger multiple
            return (n - a > b - n) ? b : a; // Return of closest of two
        }
    }
}
