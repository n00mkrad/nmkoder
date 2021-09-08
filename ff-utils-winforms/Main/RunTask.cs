using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Main
{
    class RunTask
    {
        public static bool canceled = false;

        public static void Cancel(string reason = "", bool noMsgBox = false)
        {
            //if (current == null)
            //    return;

            canceled = true;
            Program.mainForm.SetStatus("Canceled.");
            Program.mainForm.SetProgress(0);
            //AvProcess.Kill();

            // if (!current.stepByStep && !Config.GetBool(Config.Key.keepTempFolder))
            // {
            //     if (false /* IOUtils.GetAmountOfFiles(Path.Combine(current.tempFolder, Paths.resumeDir), true) > 0 */)   // TODO: Uncomment for 1.23
            //     {
            //         DialogResult dialogResult = MessageBox.Show($"Delete the temp folder (Yes) or keep it for resuming later (No)?", "Delete temporary files?", MessageBoxButtons.YesNo);
            //         if (dialogResult == DialogResult.Yes)
            //             Task.Run(async () => { await IoUtils.TryDeleteIfExistsAsync(current.tempFolder); });
            //     }
            //     else
            //     {
            //         Task.Run(async () => { await IoUtils.TryDeleteIfExistsAsync(current.tempFolder); });
            //     }
            // }

            Program.mainForm.SetWorking(false);
            // Program.mainForm.SetTab("interpolation");
            // Logger.LogIfLastLineDoesNotContainMsg("Canceled interpolation.");

            //if (!string.IsNullOrWhiteSpace(reason) && !noMsgBox)
            //    Utils.ShowMessage($"Canceled:\n\n{reason}");
        }
    }
}
