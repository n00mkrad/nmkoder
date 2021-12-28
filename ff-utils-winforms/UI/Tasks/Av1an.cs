using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using Nmkoder.Media;
using static Nmkoder.UI.Tasks.Av1anUi;

namespace Nmkoder.UI.Tasks
{
    class Av1an
    {
        public enum QualityMode { Crf, TargetVmaf }
        public enum ChunkMethod { Hybrid, Lsmash, Ffms2, Segment, Select }

        public static void Init()
        {
            Av1anUi.Init();
        }

        public static async Task RunResumeWithSavedArgs(string overrideTempDir = "", string overrideArgs = "")
        {
            await Run(true, overrideTempDir, overrideArgs);
        }

        public static async Task RunResumeWithNewArgs(string sourceFile, string overrideTempDir = "")
        {
            if(TrackList.current == null || TrackList.current.File.ImportPath != sourceFile)
            {
                Logger.Log($"You first need to load the input file that was used for this encode to resume with new settings!");
                Program.mainForm.fileListBox.Items.Cast<ListViewItem>().ToList().ForEach(x => x.Selected = false); 
                Program.mainForm.fileListBox.Items.Cast<ListViewItem>().First().Selected = true; // Force MFM
                FileList.LoadFiles(new string[1] { sourceFile }, true); // Add input file
                await TrackList.LoadFirstFile(((FileListEntry)Program.mainForm.fileListBox.Items[0].Tag).File); // Load file
            }

            await Run(true, overrideTempDir, "");
        }

        public static async Task Run(bool resume = false, string overrideTempDir = "", string overrideArgs = "")
        {
            Program.mainForm.SetWorking(true);
            string args = "";
            string inPath = "";
            string outPath = "";
            string tempDir = "";
            string timestamp = ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();

            try
            {
                if (string.IsNullOrWhiteSpace(overrideArgs))
                {
                    CodecUtils.Av1anCodec vCodec = GetCurrentCodecV();
                    CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                    bool vmaf = IsUsingVmaf();
                    inPath = TrackList.current.File.ImportPath;
                    outPath = GetOutPath();
                    string cust = Program.mainForm.av1anCustomArgsBox.Text.Trim();
                    string custEnc = Program.mainForm.av1anCustomEncArgsBox.Text.Trim();
                    CodecArgs codecArgs = CodecUtils.GetCodec(vCodec).GetArgs(GetVideoArgsFromUi(), TrackList.current.File, Data.Codecs.Pass.OneOfOne);
                    string v = codecArgs.Arguments;
                    string vf = await GetVideoFilterArgs(codecArgs);
                    string a = CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi()).Arguments;
                    string w = Program.mainForm.av1anOptsWorkerCountUpDown.Value.ToString();
                    string s = GetSplittingMethod();
                    string m = GetChunkGenMethod();
                    string c = GetConcatMethod();
                    string thr = Program.mainForm.av1anThreadsUpDown.Value.ToString();

                    args = $"-i {inPath.Wrap()} -y --verbose --keep --split-method {s} -m {m} -c {c} --set-thread-affinity {thr} {GetScDownscaleArg()} {cust} {v} -f \" {vf} \" -a \" {a} \" -w {w} -o {outPath.Wrap()}";

                    if (vmaf)
                    {
                        int q = (int)Program.mainForm.av1anQualityUpDown.Value;
                        string filters = vf.Length > 3 ? $"--vmaf-filter \" {vf.Split("-vf ").LastOrDefault()} \"" : "";
                        args += $" --target-quality {q} --vmaf-path {Paths.GetVmafPath(false).Wrap()} {filters} --vmaf-threads 2";
                    }

                    int totalThreads = w.GetInt() * thr.GetInt();
                    Logger.Log($"Using {w} workers with {thr} threads each = {totalThreads} threads total. {(totalThreads <= Environment.ProcessorCount ? "Thread pinning enabled." : "")}");
                }
                else
                {
                    inPath = overrideArgs.Split("-i \"")[1].Split("\"")[0].Trim();
                    outPath = overrideArgs.Split(" -o \"").Last().Remove("\"").Trim();

                    try
                    {
                        int w = overrideArgs.Split("-y ")[1].Split("-w ")[1].Split(" ")[0].GetInt();
                        int thr = overrideArgs.Split("-y ")[1].Split("--set-thread-affinity ")[1].Split(" ")[0].GetInt();
                        Logger.Log($"Resuming with {w} workers with {thr} threads each = {w * thr} threads total. {((w * thr) <= Environment.ProcessorCount ? "Thread pinning enabled." : "")}");
                    }
                    catch { } // Allow this to fail, it's just some ugly string parsing and not crucial anyway
                }

                if(outPath == inPath)
                {
                    Logger.Log($"Output path can't be the same as the input path!");
                    return;
                }

                if (Path.GetExtension(outPath) == null)
                {
                    Logger.Log($"Output path must have a valid file extension!");
                    return;
                }

                string tempDirName = !string.IsNullOrWhiteSpace(overrideTempDir) ? overrideTempDir : timestamp;
                tempDir = Path.Combine(Paths.GetAv1anTempPath(), tempDirName);
                AvProcess.lastTempDirAv1an = tempDir;
                string tmp = $"--temp {tempDir.Wrap()}";
                Directory.CreateDirectory(tempDir);

                args = !string.IsNullOrWhiteSpace(overrideArgs) ? overrideArgs : args;
                args = $"{(resume ? "-r" : "")} {tmp} {args}";

                string creationTimestamp = (resume ? (LoadJson(overrideTempDir).ContainsKey("creationTimestamp") ? LoadJson(overrideTempDir)["creationTimestamp"] : "-1") : timestamp);
                SaveJson(inPath, tempDirName, args, creationTimestamp, timestamp);
            }
            catch (Exception e)
            {
                Logger.Log($"Error creating av1an command: {e.Message}\n{e.StackTrace}");
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Shift) // Allow reviewing and editing command if shift is held
            {
                EditCommandForm form = new EditCommandForm("av1an", args);
                form.ShowDialog();

                if (string.IsNullOrWhiteSpace(form.Args))
                {
                    Program.mainForm.SetWorking(false);
                    return;
                }

                args = form.Args;
            }

            Logger.Log($"Running:\nav1an {args}", true, false, "av1an");

            await AvProcess.RunAv1an(args, AvProcess.LogMode.OnlyLastLine, true);

            Program.mainForm.SetWorking(false);

            AskDeleteTempFolder(tempDir);
        }

        private static string GetScDownscaleArg ()
        {
            if (TrackList.current.File == null || TrackList.current.File.VideoStreams.Count < 1)
                return "";

            int h = TrackList.current.File.VideoStreams[0].Resolution.Height;
            float mult = 1f;

            if (h >=  720) mult = 0.7500f;
            if (h >=  900) mult = 0.7083f;
            if (h >= 1080) mult = 0.6667f;
            if (h >= 1440) mult = 0.5000f;
            if (h >= 2160) mult = 0.4166f;
            if (h >= 4320) mult = 0.3333f;

            return $"--sc-downscale-height {(h * mult).RoundToInt().Clamp(360, 2160)}"; // Apply multiplicator but clamp to sane values
        }

        private static void SaveJson(string inputFilePath, string tempFolderName, string args, string creationTimestamp, string lastRunTimestamp)
        {
            try
            {
                string jsonPath = Path.Combine(Paths.GetAv1anTempPath(), $"{tempFolderName}.json");
                Dictionary<string, string> info = new Dictionary<string, string>();

                info.Add("fileName", Path.GetFileName(inputFilePath));
                info.Add("filePath", inputFilePath);
                info.Add("tempFolderName", tempFolderName);
                info.Add("args", "-i " + args.Split(" -i ")[1]);
                info.Add("creationTimestamp", creationTimestamp);
                info.Add("lastRunTimestamp", lastRunTimestamp);

                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(info, Formatting.Indented));
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to write nmkoder av1an info json! {e.Message}", true);
            }
        }

        public static Dictionary<string, string> LoadJson(string tempFolderName)
        {
            try
            {
                string jsonPath = Path.Combine(Paths.GetAv1anTempPath(), $"{tempFolderName}.json");
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(jsonPath));
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load nmkoder av1an info json! {e.Message}", true);
                return new Dictionary<string, string>();
            }
        }

        public static int GetDefaultWorkerCount ()
        {
            return ((int)Math.Ceiling((double)Environment.ProcessorCount * 0.4f)).Clamp(2, 32);
        }
    }
}
