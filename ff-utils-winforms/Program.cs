using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.IO;
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
            Paths.Init();
            Config.Init();
            Cleanup();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine(Environment.CurrentDirectory);
            Application.Run(new MainForm());
        }

        public static void Cleanup ()
        {
            try
            {
                foreach (DirectoryInfo dir in new DirectoryInfo(Paths.GetLogPath(true)).GetDirectories())
                {
                    string[] split = dir.Name.Split('-');
                    int daysOld = (DateTime.Now - new DateTime(split[0].GetInt(), split[1].GetInt(), split[2].GetInt())).Days;

                    if (daysOld > 7 || dir.GetFiles("*", SearchOption.AllDirectories).Length < 1) // keep logs for 7 days
                        dir.Delete(true);
                }

                IoUtils.DeleteContentsOfDir(Paths.GetSessionDataPath()); // Clear this session's temp files...

                foreach (DirectoryInfo dir in new DirectoryInfo(Paths.GetSessionsPath()).GetDirectories())
                {
                    string[] split = dir.Name.Split('-');
                    int daysOld = (DateTime.Now - new DateTime(split[0].GetInt(), split[1].GetInt(), split[2].GetInt())).Days;

                    if (daysOld > 2 || dir.GetFiles("*", SearchOption.AllDirectories).Length < 1) // keep temp files for 2 days
                        dir.Delete(true);
                }

                foreach (string file in IoUtils.GetFilesSorted(Paths.GetBinPath(), true, "*.log*"))
                    IoUtils.TryDeleteIfExists(file);
            }
            catch(Exception e)
            {
                Logger.Log($"Cleanup Error: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
