using Nmkoder.Data;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder
{
    static class Program
    {
        public static bool busy;
        public static Form1 mainForm;

        [STAThread]
        static void Main()
        {
            Config.Init();
            IoUtils.DeleteContentsOfDir(Paths.GetLogPath());
            IoUtils.DeleteContentsOfDir(Paths.GetThumbsPath());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine(Environment.CurrentDirectory);
            Application.Run(new Form1());
            
        }
    }
}
