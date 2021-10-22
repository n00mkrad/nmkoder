using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class Paths
    {
        public static string sessionTimestamp;

        public static void Init ()
        {
            var n = DateTime.Now;
            sessionTimestamp = $"{n.Year}-{n.Month}-{n.Day}-{n.Hour}-{n.Minute}-{n.Second}-{n.Millisecond}";
        }

        public static string GetExe()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase.Replace("file:///", "");
        }

        public static string GetOwnFolder()
        {
            return new FileInfo(GetExe()).Directory.FullName;
        }

        public static string GetWorkingDir()
        {
            return Environment.CurrentDirectory;
        }

        public static string GetLogPath(bool noSession = false)
        {
            string path = Path.Combine(GetOwnFolder(), "logs", (noSession ? "" : sessionTimestamp));
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetBinPath()
        {
            string path = Path.Combine(GetOwnFolder(), "bin");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetDataPath()
        {
            string path = Path.Combine(GetOwnFolder(), "data");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetSessionsPath()
        {
            string path = Path.Combine(GetDataPath(), "sessions");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetSessionDataPath()
        {
            string path = Path.Combine(GetSessionsPath(), sessionTimestamp);
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetThumbsPath(bool noSession = false)
        {
            string path = Path.Combine((noSession ? GetDataPath() : GetSessionDataPath()), "thumbs");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetFrameSeqPath(bool noSession = false)
        {
            string path = Path.Combine((noSession ? GetDataPath() : GetSessionDataPath()), "frameSequences");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetAv1anTempPath()
        {
            string path = Path.Combine(GetDataPath(), "av1anTemp");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetVmafPath(bool escape, string model = "vmaf_v0.6.1")
        {
            string path = Path.Combine(GetBinPath(), $"{model}.json");

            if (escape)
                return FormatUtils.GetFilterPath(path);
            else
                return path.Replace("\\", "/");
        }
    }
}
