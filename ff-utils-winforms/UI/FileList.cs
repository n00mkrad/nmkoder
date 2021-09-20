using Nmkoder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nmkoder.UI
{
    class FileList
    {
        public static List<MediaFile> currentFiles = new List<MediaFile>();

        public static void RefreshList ()
        {
            foreach (MediaFile mediaFile in currentFiles)
                Program.mainForm.fileListBox.Items.Add(mediaFile.Name);
        }

        public static void LoadFiles (string[] paths, bool clearExisting)
        {
            if(clearExisting)
                Program.mainForm.fileListBox.Items.Clear();

            foreach (string file in paths)
            {
                MediaFile mediaFile = new MediaFile(file); // Create MediaFile without initializing
                Program.mainForm.fileListBox.Items.Add(mediaFile);
            }
        }
    }
}
