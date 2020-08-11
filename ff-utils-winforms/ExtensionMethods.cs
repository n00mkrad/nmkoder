using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ff_utils_winforms
{
    public static class ExtensionMethods
    {
        public static string TrimNumbers (this string s)
        {
            s = Regex.Replace(s, "[^.0-9]", "");
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
    }
}
