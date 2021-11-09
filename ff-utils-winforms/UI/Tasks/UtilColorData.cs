using Nmkoder.Data;
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
    class UtilColorData
    {
        public static string vidSrc;
        public static string vidTarget;
        public static bool copyColorSpace = true;
        public static bool copyHdrData = true;

        public static async Task Run()
        {
            // if (RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess)
            // {
            //     Logger.Log($"Color Data Utility: Didn't run because this util only works in Multi File Mode!");
            //     return;
            // }

            Program.mainForm.SetWorking(true);

            try
            {
                UtilsColorDataForm form = new UtilsColorDataForm(true);
                form.ShowDialog();
                Program.mainForm.SetColorDataFormVars(form);

                if (!File.Exists(vidTarget))
                {
                    Logger.Log($"Only one file loaded - Will only print metadata for {Path.GetFileName(vidSrc)}.");
                    VideoColorData d = await ColorDataUtils.GetColorData(vidSrc);
                    List<string> lines = new List<string>();
                    if (!string.IsNullOrWhiteSpace(d.ColorTransfer)) lines.Add($"Color transfer: {d.ColorTransfer}");
                    if (!string.IsNullOrWhiteSpace(d.ColorMatrixCoeffs)) lines.Add($"Colour matrix coefficients: {d.ColorMatrixCoeffs}");
                    if (!string.IsNullOrWhiteSpace(d.ColorMatrixCoeffs)) lines.Add($"Colour primaries: {d.ColorPrimaries}");
                    if (!string.IsNullOrWhiteSpace(d.ColorRange)) lines.Add($"Colour range: {d.ColorRange}");
                    if (!string.IsNullOrWhiteSpace(d.RedX) && !string.IsNullOrWhiteSpace(d.RedY)) lines.Add($"Red color coordinates X/Y: {d.RedX}/{d.RedY}");
                    if (!string.IsNullOrWhiteSpace(d.GreenX) && !string.IsNullOrWhiteSpace(d.GreenY)) lines.Add($"Green color coordinates X/Y: {d.GreenX}/{d.GreenY}");
                    if (!string.IsNullOrWhiteSpace(d.BlueX) && !string.IsNullOrWhiteSpace(d.BlueY)) lines.Add($"Blue color coordinates X/Y: {d.BlueX}/{d.BlueY}");
                    if (!string.IsNullOrWhiteSpace(d.WhiteX) && !string.IsNullOrWhiteSpace(d.WhiteY)) lines.Add($"White color coordinates X/Y: {d.WhiteX}/{d.WhiteY}");
                    if (!string.IsNullOrWhiteSpace(d.LumaMin)) lines.Add($"Minimum luminance: { d.LumaMin}");
                    if (!string.IsNullOrWhiteSpace(d.LumaMax)) lines.Add($"Maximum luminance: { d.LumaMax}");

                    if (lines.Count > 0)
                        Logger.Log(string.Join("\n", lines));
                    else
                        Logger.Log($"No metadata found.");
                }
                else
                {
                    Logger.Log($"Transferring color data from {Path.GetFileName(vidSrc)} to {Path.GetFileName(vidTarget)}.");
                    VideoColorData data = await ColorDataUtils.GetColorData(vidSrc);
                    Logger.Log(data.ToString());
                    await ColorDataUtils.SetColorData(vidTarget, data);
                }
            }
            catch(Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
            }
            
            Program.mainForm.SetWorking(false);
        }
    }
}
