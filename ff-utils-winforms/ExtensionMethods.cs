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
    }
}
