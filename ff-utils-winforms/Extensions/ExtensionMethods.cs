﻿using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Drawing;

namespace Nmkoder.Extensions
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

        public static int GetInt(this TextBox textbox)
        {
            return GetInt(textbox.Text);
        }

        public static int GetInt(this ComboBox combobox)
        {
            return GetInt(combobox.Text);
        }

        /// <summary> Gets an int from a string. Strips non-number chars without throwing an exception. If parsing fails, it returns <paramref name="fallback"/>. </summary>
        public static int GetInt(this string str, int fallback = 0)
        {
            if (string.IsNullOrWhiteSpace(str))
                return fallback;

            str = str.Trim();
            string processedString = str.TrimNumbers();

            // Re-add '-' if string seems to be a negative number
            if (str.Length >= 2 && str[0] == '-' && str[1] != '-')
            {
                processedString = "-" + processedString;
            }

            // Try to parse the processed string
            if (int.TryParse(processedString, out int result))
                return result;

            return fallback;
        }


        public static long GetLong(this string str)
        {
            if (str == null || str.Length < 1)
                return 0;

            str = str.Trim();

            try
            {
                if (str.Length >= 2 && str[0] == '-' && str[1] != '-')
                    return long.Parse("-" + str.TrimNumbers());
                else
                    return long.Parse(str.TrimNumbers());
            }
            catch
            {
                return 0;
            }
        }

        public static bool GetBool(this string str)
        {
            try
            {
                return bool.Parse(str);
            }
            catch
            {
                return false;
            }
        }

        public static float GetFloat(this TextBox textbox)
        {
            return GetFloat(textbox.Text);
        }

        public static float GetFloat(this ComboBox combobox)
        {
            return GetFloat(combobox.Text);
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

        public static int RoundToInt(this float f)
        {
            return (int)Math.Round(f);
        }

        public static int RoundToInt(this double d)
        {
            return (int)Math.Round(d);
        }

        public static long RoundToLong(this double d)
        {
            return (long)Math.Round(d);
        }

        public static int Clamp(this int i, int min, int max)
        {
            if (i < min)
                i = min;

            if (i > max)
                i = max;

            return i;
        }

        public static float Clamp(this float i, float min, float max)
        {
            if (i < min)
                i = min;

            if (i > max)
                i = max;

            return i;
        }

        public static long Clamp(this long i, long min, long max)
        {
            if (i < min) i = min;
            if (i > max) i = max;
            return i;
        }

        public static string[] SplitIntoLines(this string str)
        {
            return Regex.Split(str, "\r\n|\r|\n");
        }

        public static string Trunc(this string inStr, int maxChars, bool addEllipsis = true)
        {
            string str = inStr.Length <= maxChars ? inStr : inStr.Substring(0, maxChars);
            if (addEllipsis && inStr.Length > maxChars)
                str += "…";
            return str;
        }

        public static string StripBadChars(this string str)
        {
            string outStr = Regex.Replace(str, @"[^\u0020-\u007E]", string.Empty);
            outStr = outStr.Remove("(").Remove(")").Remove("[").Remove("]").Remove("{").Remove("}").Remove("%").Remove("'").Remove("~");
            return outStr;
        }

        public static string StripNumbers(this string str)
        {
            return new string(str.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
        }

        public static string Remove(this string str, string stringToRemove)
        {
            if (str == null || stringToRemove == null)
                return str;

            return str.Replace(stringToRemove, "");
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

        public static string ReplaceLast(this string str, string stringToReplace, string replaceWith)
        {
            int place = str.LastIndexOf(stringToReplace);

            if (place == -1)
                return str;

            return str.Remove(place, stringToReplace.Length).Insert(place, replaceWith);
        }

        public static string[] SplitBy(this string str, string splitBy)
        {
            return str.Split(new string[] { splitBy }, StringSplitOptions.None);
        }

        public static string RemoveComments(this string str)
        {
            return str.Split('#')[0].SplitBy("//")[0];
        }

        public static string FilenameSuffix(this string path, string suffix)
        {
            string filename = Path.ChangeExtension(path, null);
            string ext = Path.GetExtension(path);
            return filename + suffix + ext;
        }

        public static string ToStringDot(this float f, string format = "")
        {
            if (string.IsNullOrWhiteSpace(format))
                return f.ToString().Replace(",", ".");
            else
                return f.ToString(format).Replace(",", ".");
        }

        public static string[] Split(this string str, string trimStr)
        {
            return str.Split(new string[] { trimStr }, StringSplitOptions.None);
        }

        public static bool MatchesWildcard(this string str, string wildcard)
        {
            WildcardPattern pattern = new WildcardPattern(wildcard);
            return pattern.IsMatch(str);
        }

        public static int RoundMod(this int n, int mod = 2)     // Round to a number that's divisible by 2 (for h264 etc)
        {
            int a = (n / 2) * 2;    // Smaller multiple
            int b = a + 2;   // Larger multiple
            return (n - a > b - n) ? b : a; // Return of closest of two
        }

        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        public static string ToStringShort(this Size s, string separator = "x")
        {
            return $"{s.Width}{separator}{s.Height}";
        }

        public static bool IsConcatFile(this string filePath)
        {
            try
            {
                return Path.GetExtension(filePath)?.ToLower() == ".concat";
            }
            catch
            {
                return false;
            }
        }

        public static string GetConcStr (this string filePath, int rate = -1)
        {
            string rateStr = rate >= 0 ? $"-r {rate} " : "";
            return filePath.IsConcatFile() ? $"{rateStr}-safe 0 -f concat " : "";
        }

        public static string GetFfmpegInputArg(this string filePath)
        {
            return $"{(filePath.IsConcatFile() ? filePath.GetConcStr() : "")} -i {filePath.Wrap()}";
        }

        public static int CountOccurences (this List<string> list, string stringToLookFor)
        {
            int count = 0;

            foreach (string s in list)
                if (s == stringToLookFor)
                    count++;

            return count;
        }

        public static string CleanString (this string str)
        {
            try
            {
                return Regex.Replace(str.Trim(), @"[^\w\._-]", "", RegexOptions.None, TimeSpan.FromSeconds(1));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static string Lower(this string s)
        {
            if (s == null)
                return s;

            return s.ToLowerInvariant();
        }

        public static string Upper(this string s)
        {
            if (s == null)
                return s;

            return s.ToUpperInvariant();
        }

        /// <summary> Shortcut for !string.IsNullOrWhiteSpace </summary>
        /// <returns> If the string is NOT null or whitespace </returns>
        public static bool IsNotEmpty(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary> Shortcut for string.IsNullOrWhiteSpace </summary>
        /// <returns> If the string is null or whitespace </returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
    }
}
