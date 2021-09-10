using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.UI.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Main
{
    public class RunTask
    {
        public enum TaskType { None, Convert };
        //public static TaskType currentTask;
        public static bool canceled = false;

        public static void Cancel(string reason = "", bool noMsgBox = false)
        {
            //if (current == null)
            //    return;

            canceled = true;
            Program.mainForm.SetStatus("Canceled.");
            Program.mainForm.SetProgress(0);
            AvProcess.Kill();

            Program.mainForm.SetWorking(false);
            // Program.mainForm.SetTab("interpolation");
            // Logger.LogIfLastLineDoesNotContainMsg("Canceled interpolation.");

            //if (!string.IsNullOrWhiteSpace(reason) && !noMsgBox)
            //    Utils.ShowMessage($"Canceled:\n\n{reason}");
        }

        public static async Task Start ()
        {
            if (Program.mainForm.GetCurrentTaskType() == TaskType.Convert)
                await QuickConvert.Run();
        }

        
    }
}
