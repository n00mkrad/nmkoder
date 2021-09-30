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
        public static string GetOwnFolder()
        {
            return Environment.CurrentDirectory;
        }

        public static string GetLogPath()
        {
            string path = Path.Combine(GetOwnFolder(), "logs");
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

        public static string GetThumbsPath()
        {
            string path = Path.Combine(GetDataPath(), "thumbs");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetFrameSeqPath()
        {
            string path = Path.Combine(GetDataPath(), "frameSequences");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetAv1anTempPath()
        {
            string path = Path.Combine(GetDataPath(), "av1anTemp");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetVmafPath(bool escape)
        {
            string path = Path.Combine(GetBinPath(), "vmaf_v0.6.1.json");

            if (escape)
                return FormatUtils.GetFilterPath(path);
            else
                return path.Replace("\\", "/");
        }
    }
}
