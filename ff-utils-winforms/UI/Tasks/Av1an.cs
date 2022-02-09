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
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.OS;
using Nmkoder.Utils;
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
            if (TrackList.current == null || TrackList.current.File.ImportPath != sourceFile)
            {
                Logger.Log($"You first need to load the input file that was used for this encode to resume with new settings!");
                Program.mainForm.fileListBox.Items.Cast<ListViewItem>().ToList().ForEach(x => x.Selected = false);
                Program.mainForm.fileListBox.Items.Cast<ListViewItem>().First().Selected = true; // Force MFM
                FileList.LoadFiles(new string[1] { sourceFile }, true); // Add input file
                await TrackList.SetAsMainFile(Program.mainForm.fileListBox.Items[0]); // Load file
            }

            await Run(true, overrideTempDir, "");
        }

        public static async Task Run(bool resume = false, string overrideTempDir = "", string overrideArgs = "")
        {
            if (overrideTempDir == "" && TrackList.current.File.IsDirectory)
            {
                RunTask.Cancel("Av1an cannot use image sequence inputs!");
                return;
            }

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
                    Logger.Log($"Preparing encoding arguments...");
                    CodecUtils.Av1anCodec vCodec = GetCurrentCodecV();
                    CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                    bool vmaf = IsUsingVmaf();
                    inPath = TrackList.current.File.ImportPath;
                    outPath = GetOutPath();
                    string cust = Program.mainForm.av1anCustomArgsBox.Text.Trim();
                    string custEnc = Program.mainForm.av1anCustomEncArgsBox.Text.Trim();
                    TrackList.current.File.ColorData = await ColorDataUtils.GetColorData(TrackList.current.File.SourcePath);
                    CodecArgs codecArgs = CodecUtils.GetCodec(vCodec).GetArgs(GetVideoArgsFromUi(), TrackList.current.File, Data.Codecs.Pass.OneOfOne);
                    string v = codecArgs.Arguments;
                    string vf = await GetVideoFilterArgs(codecArgs);
                    string a = CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi()).Arguments;
                    string w = Program.mainForm.av1anOptsWorkerCountUpDown.Value.ToString();
                    string s = GetSplittingMethodArgs();
                    string m = GetChunkGenMethod();
                    string c = GetConcatMethodArgs();
                    string o = GetChunkOrderArgs();
                    string thr = GetThreadAffArgs();

                    if (RunTask.canceled) return;

                    args = $"-i {inPath.Wrap()} -y --verbose --keep {s} {m} {c} {thr} {GetScDownscaleArg()} {o} {cust} {v} -f \" {vf} \" -a \" {a} \" -w {w} -o {outPath.Wrap()}";

                    if (vmaf)
                    {
                        int q = (int)Program.mainForm.av1anQualityUpDown.Value;
                        string filters = vf.Length > 3 ? $"--vmaf-filter \" {vf.Split("-vf ").LastOrDefault()} \"" : "";
                        args += $" --target-quality {q} --vmaf-path {Paths.GetVmafPath(false).Wrap()} {filters} --vmaf-threads 2";
                    }

                    int totalThreads = w.GetInt() * thr.GetInt();
                    Logger.Log($"Using {w} workers with {thr.GetInt()} threads each = {totalThreads} threads total. {(totalThreads <= Environment.ProcessorCount ? "Thread pinning enabled." : "")}");
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

                if (outPath == inPath)
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

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to create output folder: {e.Message}");
                Program.mainForm.SetWorking(false);
                return;
            }

            Logger.Log($"Running:\nav1an {args}", true, false, "av1an");

            Task.Run(() => CreateAttachmentMkv(args, tempDir));
            await AvProcess.RunAv1an(args, AvProcess.LogMode.OnlyLastLine, true);

            Program.mainForm.SetWorking(false);

            AskDeleteTempFolder(tempDir);
        }

        private static string GetScDownscaleArg()
        {
            if (TrackList.current.File == null || TrackList.current.File.VideoStreams.Count < 1)
                return "";

            int h = TrackList.current.File.VideoStreams[0].Resolution.Height;
            float mult = 1f;

            if (h >= 720) mult = 0.7500f;
            if (h >= 900) mult = 0.7083f;
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

        public static async Task CreateAttachmentMkv(string args, string tempFolder)
        {
            while (!IsAv1anRunning()) await Task.Delay(200);
            while(!IsAudioDone(tempFolder)) await Task.Delay(500);
            await Task.Delay(500);

            try
            {
                string txtPath = Path.Combine(Paths.GetSessionDataPath(), "av1an.txt");
                List<string> lines = new List<string> { "Encoder:", args.Split(" -e ")[1].Split(' ')[0], "", "Args:", args.Split("-v \" ")[1].Split(" \"")[0] };
                File.WriteAllLines(txtPath, lines);
                string outPath = Path.Combine(tempFolder, "audio.mkv");                

                if (File.Exists(outPath)) // Add attachment to existing audio.mkv
                {
                    string tmpOutPath = IoUtils.FilenameSuffix(outPath, ".tmp");
                    string cmd = $"-o {tmpOutPath.Wrap()} --attachment-mime-type text/plain --attach-file {txtPath.Wrap()} {outPath.Wrap()}";
                    await AvProcess.RunMkvMerge(cmd, NmkoderProcess.ProcessType.Background);
                    File.Delete(outPath);
                    File.Move(tmpOutPath, outPath);
                }
                else // Create an empty audio.mkv with just the attachment in it
                {
                    string tmpOutPath = IoUtils.FilenameSuffix(outPath, ".tmp");
                    string cmd = $"-o {outPath.Wrap()} --attachment-mime-type text/plain --attach-file {txtPath.Wrap()}";
                    await AvProcess.RunMkvMerge(cmd, NmkoderProcess.ProcessType.Background);
                }
                
                IoUtils.TryDeleteIfExists(txtPath);
            }
            catch (Exception ex)
            {
                Logger.Log($"CreateAttachmentMkv Error: {ex.Message}\n{ex.StackTrace}", true);
            }
        }

        private static bool IsAudioDone (string tempFolder)
        {
            string doneJsonPath = Path.Combine(tempFolder, "done.json");

            if (!Directory.Exists(tempFolder) || !File.Exists(doneJsonPath)) return false;

            try
            {
                var stream = File.Open(doneJsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                string contents = new StreamReader(stream).ReadToEnd();
                return contents.Contains("\"audio_done\":true");
            }
            catch(Exception ex)
            {
                Logger.Log($"IsAudioDone Error: {ex.Message}");
                return false;
            }
        }

        private static bool IsAv1anRunning()
        {
            return ProcessManager.RunningSubProcesses.Where(x => x.Type == NmkoderProcess.ProcessType.Primary && x.Process.StartInfo.Arguments.Contains("av1an.bat")).Count() > 0;
        }

        public static int GetDefaultWorkerCount()
        {
            return ((int)Math.Ceiling((double)Environment.ProcessorCount * 0.4f)).Clamp(2, 32);
        }
    }
}
