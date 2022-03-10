using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.IO;
using Nmkoder.Main;
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

        public static void LoadFiles(string[] paths, bool clearExisting)
        {
            if (clearExisting)
                Program.mainForm.fileListBox.Items.Clear();

            Random r = new Random();

            foreach (string file in paths)
            {
                MediaFile mediaFile = new MediaFile(file); // Create MediaFile without initializing
                FileListEntry entry = new FileListEntry(mediaFile);
                Color color = Program.mainForm.fileListBox.Items.Count == 0 ? Color.FromArgb(64, 64, 64) : Color.FromArgb(r.Next(16, 128), r.Next(16, 128), r.Next(16, 128));
                Program.mainForm.fileListBox.Items.Add(new ListViewItem() { Text = entry.ToString(), Tag = entry, BackColor = color });
            }

            Program.mainForm.RefreshFileListUi();
        }

        public static async Task HandleFiles(string[] paths, bool clearExisting)
        {
            if (paths == null || paths.Length == 0)
                return;

            Media.GetFrameCountCached.ClearCache();
            Media.GetMediaResolutionCached.ClearCache();
            Media.GetVideoInfo.ClearCache();

            if (clearExisting)
            {
                ThumbnailView.ClearUi();
                TrackList.ClearCurrentFile();
                Logger.ClearLogBox();
            }

            bool runInstantly = RunTask.RunInstantly();

            if (!runInstantly)
                Program.mainForm.MainTabList.SelectedIndex = 0;

            Logger.Log($"Added {paths.Length} file{((paths.Length == 1) ? "" : "s")} to list.");
            LoadFiles(paths, clearExisting);

            if (RunTask.currentFileListMode == RunTask.FileListMode.Mux && Program.mainForm.fileListBox.Items.Count == 1)
            {
                await TrackList.SetAsMainFile(Program.mainForm.fileListBox.Items[0]);
                await TrackList.AddStreamsToList(((FileListEntry)Program.mainForm.fileListBox.Items[0].Tag).File, Program.mainForm.fileListBox.Items[0].BackColor, true);
            }

            if (runInstantly)
                Program.mainForm.runBtn_Click();
        }
    }
}
