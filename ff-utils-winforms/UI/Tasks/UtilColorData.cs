using Nmkoder.Data;
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
    class UtilColorData
    {
        public static string vidSrc;
        public static string vidTarget;
        public static bool copyColorSpace = true;
        public static bool copyHdrData = true;

        public static async Task Run()
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess)
            {
                Logger.Log($"Color Data Utility: Didn't run because this util only works in Multi File Mode!");
                return;
            }

            Program.mainForm.SetWorking(true);
            Logger.Log($"Transferring color data from {Path.GetFileName(vidSrc)} to {Path.GetFileName(vidTarget)}.");

            try
            {
                VideoColorData data = await ColorDataUtils.GetColorData(vidSrc);
                Logger.Log(data.ToString());
                await ColorDataUtils.SetColorData(vidTarget, data);
            }
            catch(Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
            }
            
            Program.mainForm.SetWorking(false);
        }
    }
}
