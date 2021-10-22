using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.IO
{
    class PackageBuild
    {
        public static async Task Run (string ver)
        {
            Logger.Log($"Packaging...");

            try
            {
                Program.Cleanup();
                string dir = Paths.GetOwnFolder();
                string copyDir = Path.Combine(dir, $"Nmkoder{ver}");
                IoUtils.DeleteIfExists(copyDir);
                Directory.CreateDirectory(copyDir);
                CopyDir(dir, copyDir, "bin");
                CopyFile(dir, copyDir, Path.GetFileName(Paths.GetExe()));

                string path7z = @"C:\Program Files\7-Zip-Zstandard\7za.exe";
                string archivePath1 = Path.Combine(dir, $"Nmkoder{ver}.7z");
                string archivePath2 = Path.Combine(dir, $"Nmkoder{ver}-NoOCR.7z");

                Process p1 = Process.Start(path7z, $"a {archivePath1.Wrap()} -m0=flzma2 -mx9 {copyDir.Wrap()}");
                while (!p1.HasExited) await Task.Delay(100);
                IoUtils.TryDeleteIfExists(Path.Combine(copyDir, "bin", "SE"));
                Process.Start(path7z, $"a {archivePath2.Wrap()} -m0=flzma2 -mx9 {copyDir.Wrap()}");

            }
            catch(Exception e)
            {
                Logger.Log(e.Message);
            }

            Logger.Log($"Done");
        }

        private static void CopyFile(string baseDirSource, string baseDirTarget, string filename)
        {
            File.Copy(Path.Combine(baseDirSource, filename), Path.Combine(baseDirTarget, filename));
        }

        private static void CopyDir (string baseDirSource, string baseDirTarget, string dirName)
        {
            IoUtils.CopyDir(Path.Combine(baseDirSource, dirName), Path.Combine(baseDirTarget, dirName));
        }
    }
}
