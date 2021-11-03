using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Ui
{
    public class FileListEntry
    {
        public MediaFile File { get; }
        public string Title { get { return File.Title; } }
        public string TitleEdited { get; set; } = "";
        public string Language { get { return File.Language; } }
        public string LanguageEdited { get; set; } = "";

        public FileListEntry ()
        {
            
        }

        public FileListEntry(MediaFile file)
        {
            File = file;
        }

        public override string ToString()
        {
            return File.ToString();
        }
    }
}
