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
    class UtilPlotBitrate
    {
        public static async Task Run()
        {
            if (RunTask.currentFileListMode == RunTask.FileListMode.Batch)
            {
                Logger.Log($"Bitrate Plot Utility: Didn't run because this util only works in Muxing Mode!");
                return;
            }

            Program.mainForm.SetWorking(true);

            try
            {
                string path = TrackList.current.File.ImportPath;
                List<BitratePlottingUtils.Frame> frameList = await BitratePlottingUtils.GetFrameInfos(path, true);
                var seconds = BitratePlottingUtils.GetBytesPerSecond(frameList);
                BitratePlotForm form = new BitratePlotForm(seconds);
                Program.mainForm.SetWorking(false);
                form.ShowDialog();
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
