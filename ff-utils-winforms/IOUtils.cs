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

        public static bool IsPathDirectory(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            path = path.Trim();
            if (Directory.Exists(path))
            {
                return true;
            }
            if (File.Exists(path))
            {
                return false;
            }
            if (new string[2]
            {
                "\\",
                "/"
            }.Any((string x) => path.EndsWith(x)))
            {
                return true;
            }
            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }

        public static bool TryDeleteIfExists(string path)      // Returns true if no exception occurs
        {
            try
            {
                if (path == null)
                    return false;
                DeleteIfExists(path);
                return true;
            }
            catch (Exception e)
            {
                //Logger.Log($"TryDeleteIfExists: Error trying to delete {path}: {e.Message}", true);
                return false;
            }
        }

        public static bool DeleteIfExists(string path)		// Returns true if the file/dir exists
        {
            if (!IsPathDirectory(path) && File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            if (IsPathDirectory(path) && Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }
            return false;
        }
    }
}
