﻿using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Forms.Utils;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI.Tasks
{
    class UtilConcat
    {
        public static async Task Run()
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.Batch)
            {
                Logger.Log($"Color Data Utility: Didn't run because this util only works in Muxing Mode!");
                return;
            }

            Program.mainForm.SetWorking(true);

            try
            {
                List<FileListEntry> fileListEntries = Program.mainForm.fileListBox.Items.Cast<ListViewItem>().Select(x => (FileListEntry)x.Tag).ToList();
                List<string> paths = fileListEntries.Where(x => x.File.ImportPath == x.File.SourcePath).Select(x => x.File.ImportPath).ToList();
                string filename = new FileInfo(paths[0]).Directory.Name + "-merge.mkv";
                string outPath = Path.Combine(new FileInfo(paths[0]).Directory.FullName, filename);
                IoUtils.TryDeleteIfExists(outPath);
                await ConcatUtils.ConcatMkvMerge(paths, outPath);
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
            }

            Program.mainForm.SetWorking(false);
        }
    }
}
