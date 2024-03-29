﻿using Nmkoder.Data;
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
        public static string[] fileArgs = new string[0];
        public static string[] args = new string[0];

        public static bool busy;
        public static MainForm mainForm;

        [STAThread]
        static void Main()
        {
            Paths.Init();
            Config.Init();
            Cleanup();

            fileArgs = Environment.GetCommandLineArgs().Where(a => a[0] != '-' && File.Exists(a)).ToList().Skip(1).ToArray();
            args = Environment.GetCommandLineArgs().Where(a => a[0] == '-').Select(x => x.Trim().Substring(1).ToLowerInvariant()).ToArray();
            Logger.Log($"Command Line: {Environment.CommandLine}", true);
            Logger.Log($"Files: {(fileArgs.Length > 0 ? string.Join(", ", fileArgs) : "None")}", true);
            Logger.Log($"Args: {(args.Length > 0 ? string.Join(", ", args) : "None")}", true);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine(Environment.CurrentDirectory);
            Application.Run(new MainForm());
        }

        public static void Cleanup()
        {
            int keepLogsDays = 4;
            int keepSessionDataDays = 4;

            try
            {
                foreach (DirectoryInfo dir in new DirectoryInfo(Paths.GetLogPath(true)).GetDirectories())
                {
                    string[] split = dir.Name.Split('-');
                    int daysOld = (DateTime.Now - new DateTime(split[0].GetInt(), split[1].GetInt(), split[2].GetInt())).Days;
                    int fileCount = dir.GetFiles("*", SearchOption.AllDirectories).Length;

                    if (daysOld > keepLogsDays || fileCount < 1) // keep logs for 4 days
                    {
                        Logger.Log($"Cleanup: Log folder {dir.Name} is {daysOld} days old and has {fileCount} files - Will Delete", true);
                        IoUtils.TryDeleteIfExists(dir.FullName);
                    }
                }

                IoUtils.DeleteContentsOfDir(Paths.GetSessionDataPath()); // Clear this session's temp files...

                foreach (DirectoryInfo dir in new DirectoryInfo(Paths.GetSessionsPath()).GetDirectories())
                {
                    string[] split = dir.Name.Split('-');
                    int daysOld = (DateTime.Now - new DateTime(split[0].GetInt(), split[1].GetInt(), split[2].GetInt())).Days;
                    int fileCount = dir.GetFiles("*", SearchOption.AllDirectories).Length;

                    if (daysOld > keepSessionDataDays || fileCount < 1) // keep temp files for 2 days
                    {
                        Logger.Log($"Cleanup: Session folder {dir.Name} is {daysOld} days old and has {fileCount} files - Will Delete", true);
                        IoUtils.TryDeleteIfExists(dir.FullName);
                    }
                }

                foreach (string file in IoUtils.GetFilesSorted(Paths.GetBinPath(), true, "*.log*"))
                    IoUtils.TryDeleteIfExists(file);

                foreach (string file in IoUtils.GetFilesSorted(Paths.GetBinPath(), true, "desktop.ini"))
                    IoUtils.TryDeleteIfExists(file);
            }
            catch (Exception e)
            {
                Logger.Log($"Cleanup Error: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
