using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ff_utils_winforms
{
    class IOUtils
    {
        public static string GetFfmpegExePath ()
        {
            return Path.Combine(Environment.CurrentDirectory, "ffmpeg.exe");
        }

        public static int GetFilenameCounterLength (string file, string prefixToRemove = "")
        {
            string filenameNoExt = Path.GetFileNameWithoutExtension(file);
            if (!string.IsNullOrEmpty(prefixToRemove))
                filenameNoExt = filenameNoExt.Replace(prefixToRemove, "");
            string onlyNumbersFilename = Regex.Replace(filenameNoExt, "[^.0-9]", "");
            return onlyNumbersFilename.Length;
        }
    }
}
