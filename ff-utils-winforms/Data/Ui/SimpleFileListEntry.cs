using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Ui
{
    class SimpleFileListEntry
    {
        public FileInfo File;
        public long Size { get { return File == null ? -1 : File.Length; } }

        public SimpleFileListEntry ()
        {
            
        }

        public SimpleFileListEntry (string path)
        {
            File = new FileInfo(path);
        }

        public SimpleFileListEntry(FileInfo file)
        {
            File = file;
        }

        public override string ToString()
        {
            return $"{File.Name} ({FormatUtils.Bytes(Size)})";
        }
    }
}
