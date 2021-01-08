using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ff_utils_winforms.Data
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
			return Path.Combine(IOUtils.GetOwnFolder(), "bin");
		}
	}
}
