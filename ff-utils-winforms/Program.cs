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
        public static TextBox logTbox;

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

        public static void Print(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return;
            Console.WriteLine(s);
            s = s.Replace("\n", Environment.NewLine);
            logTbox.AppendText(Environment.NewLine + s);
        }
    }
}
