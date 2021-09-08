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
		public static string GetLogPath()
		{
			string path = Path.Combine(IOUtils.GetOwnFolder(), "logs");
			Directory.CreateDirectory(path);
			return path;
		}

		public static string GetBinPath()
		{
			string path = Path.Combine(IOUtils.GetOwnFolder(), "bin");
			Directory.CreateDirectory(path);
			return path;
		}

		public static string GetDataPath()
		{
			string path = Path.Combine(IOUtils.GetOwnFolder(), "data");
			Directory.CreateDirectory(path);
			return path;
		}
	}
}
