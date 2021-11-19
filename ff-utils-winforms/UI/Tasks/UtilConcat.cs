using Nmkoder.Data;
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

namespace Nmkoder.UI.Tasks
{
    class UtilConcat
    {
        public static async Task Run()
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess)
            {
                Logger.Log($"Color Data Utility: Didn't run because this util only works in Multi File Mode!");
                return;
            }

            Program.mainForm.SetWorking(true);

            try
            {
                List<string> paths = Program.mainForm.fileListBox.Items.OfType<FileListEntry>().Where(x => x.File.TruePath == x.File.SourcePath).Select(x => x.File.TruePath).ToList();
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
