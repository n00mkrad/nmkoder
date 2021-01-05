using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ff_utils_winforms
{
    class IOUtils
    {

        public static string GetOwnFolder()
        {
            return Environment.CurrentDirectory;
        }

        public static string GetFfmpegExePath ()
        {
            return Path.Combine(GetOwnFolder(), "bin", "ffmpeg.exe");
        }

        public static string GetTempPath ()
        {
            string path = Path.Combine(GetOwnFolder(), "temp");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetExe()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase.Replace("file:///", "");
        }

        public static string GetExeDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static Image GetImage(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                return Image.FromStream(stream);
        }

        public static string[] ReadLines(string path)
        {
            List<string> lines = new List<string>();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);
            }
            return lines.ToArray();
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

        public static string[] GetFilesSorted(string path, bool recursive = false, string pattern = "*")
        {
            SearchOption opt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(path, pattern, opt).OrderBy(x => Path.GetFileName(x)).ToArray();
        }

        public static string[] GetFilesSorted(string path, string pattern = "*")
        {
            return GetFilesSorted(path, false, pattern);
        }

        public static string[] GetFilesSorted(string path)
        {
            return GetFilesSorted(path, false, "*");
        }

        public static FileInfo[] GetFileInfosSorted(string path, bool recursive = false, string pattern = "*")
        {
            SearchOption opt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.GetFiles(pattern, opt).OrderBy(x => x.Name).ToArray();
        }
    }
}
