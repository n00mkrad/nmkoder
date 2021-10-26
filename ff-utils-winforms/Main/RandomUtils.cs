using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Main
{
    class RandomUtils
    {
        public static async Task RecoverDoneJson()
        {
            string dir = @"C:\Files\Videos\Encoding\tenet\mrg";

            FileInfo[] chunks = IoUtils.GetFileInfosSorted(dir, false, "*.ivf");

            string s = "";

            foreach (FileInfo f in chunks)
            {
                int frames = await Media.FfmpegCommands.ReadFrameCountFfmpegAsync(f.FullName);
                s += $"\"{f.Name.Split('.')[0]}\":{frames},";
                Logger.Log($"{f.Name} => {frames} frames");
            }

            string outPath = Path.Combine(dir, "chunks.txt");
            File.WriteAllText(outPath, s);

            Logger.Log($"Done");
        }

        public static async Task ValidateDoneJson()
        {
            try
            {
                string jsonPath = @"C:\Software\Nmkoder\data\av1anTemp\1635170121824\done.json";
                string chunksDir = @"C:\Software\Nmkoder\data\av1anTemp\1635170121824\encode";

                string[] filenames = File.ReadAllText(jsonPath).Split('{')[2].Split('}')[0].Split(',').Select(x => x.Split(':')[0].Remove("\"")).ToArray();
                //string[] chunks = IoUtils.GetFilesSorted(chunksDir, false, "*.ivf");

                foreach (string f in filenames)
                {
                    bool exists = File.Exists(Path.Combine(chunksDir, f + ".ivf"));
                    Logger.Log($"{f} => {(exists ? "exists" : "does not exist!")}");
                }
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
            }

            Logger.Log($"Done");
        }
    }
}
