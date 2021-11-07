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
        public static async Task RecoverDoneJson(string dir)
        {
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

        public static async Task ValidateDoneJson(string jsonPath, string chunksDir)
        {
            try
            {
                int minSizeKb = 10;

                string doneJsonText = File.ReadAllText(jsonPath);
                string[] filenames = doneJsonText.Split('{')[2].Split('}')[0].Split(',').Select(x => x.Split(':')[0].Remove("\"")).ToArray();
                //string[] chunks = IoUtils.GetFilesSorted(chunksDir, false, "*.ivf");

                string newJsonText = doneJsonText.Split('{')[1] + "{";

                foreach (string f in filenames)
                {
                    string path = Path.Combine(chunksDir, f + ".ivf");
                    bool valid = File.Exists(path) && new FileInfo(path).Length > (1024 * minSizeKb);
                    //Logger.Log($"{f} => {(valid ? "exists" : "does not exist!")}");

                    if (valid)
                        newJsonText += $"{(f + ".ivf").Wrap()}";
                    else
                        Logger.Log($"Chunk {f} does not exist!");
                }

                newJsonText += "},\"audio_done\":true}";

                File.Move(jsonPath, jsonPath + ".bak");
                File.WriteAllText(jsonPath, newJsonText);
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
            }

            Logger.Log($"Done");
        }
    }
}
