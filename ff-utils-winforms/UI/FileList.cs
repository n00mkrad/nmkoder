using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    class FileList
    {
        public static List<MediaFile> currentFiles = new List<MediaFile>();

        public static void LoadFiles (string[] paths, bool clearExisting)
        {
            if(clearExisting)
                Program.mainForm.fileListBox.Items.Clear();

            Random r = new Random();

            foreach (string file in paths)
            {
                MediaFile mediaFile = new MediaFile(file); // Create MediaFile without initializing
                FileListEntry entry = new FileListEntry(mediaFile);
                Color color = Program.mainForm.fileListBox.Items.Count == 0 ? Color.FromArgb(64, 64, 64) : Color.FromArgb(r.Next(16, 128), r.Next(16, 128), r.Next(16, 128));
                Program.mainForm.fileListBox.Items.Add(new ListViewItem() { Text = entry.ToString(), Tag = entry, BackColor = color });
            }
        }
    }
}
