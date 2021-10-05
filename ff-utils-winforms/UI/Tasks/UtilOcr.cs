using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.Utils;

namespace Nmkoder.UI.Tasks
{
    class UtilOcr
    {
        public static async Task Run ()
        {
            Program.mainForm.SetWorking(true);
            string inPath = TrackList.current.TruePath;
            string outDir = new FileInfo(inPath).DirectoryName;
            string outName = $"subs0";
            await OcrUtils.RunOcr(inPath, "0:s:0", outDir, outName);
            Program.mainForm.SetWorking(false);
        }
    }
}
