using Nmkoder.Data;
using Nmkoder.Forms;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: System.Windows.Media.DisableDpiAwareness] // Disable Dpi awareness in the application assembly.

namespace Nmkoder
{
    static class Program
    {
        public static bool busy;
        public static MainForm mainForm;

        [STAThread]
        static void Main()
        {
            Config.Init();
            IoUtils.DeleteContentsOfDir(Paths.GetLogPath());
            IoUtils.DeleteContentsOfDir(Paths.GetThumbsPath());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine(Environment.CurrentDirectory);
            Application.Run(new MainForm());
            
        }
    }
}
