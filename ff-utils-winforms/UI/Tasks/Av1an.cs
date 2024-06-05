using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using Nmkoder.Data;
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
        public enum ChunkMethod { BestSource, LSMASH, DGDecNV, FFMS2, Segment, Hybrid, Select }

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
                if (overrideArgs.IsEmpty())
                {
                    Logger.Log($"Preparing encoding arguments...");
                    CodecUtils.Av1anCodec vCodec = GetCurrentCodecV();
                    CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                    inPath = TrackList.current.File.ImportPath;
                    ValidatePath();
                    outPath = UiData.GetOutPath();
                    string custEnc = Program.mainForm.av1anCustomEncArgsBox.Text.Trim();
                    TrackList.current.File.ColorData = await ColorDataUtils.GetColorData(TrackList.current.File.SourcePath);
                    CodecArgs codecArgs = CodecUtils.GetCodec(vCodec).GetArgs(GetVideoArgsFromUi(), TrackList.current.File, Data.Codecs.Pass.OneOfOne);
                    string vf = await GetVideoFilterArgs(codecArgs);
                    string ffAud = CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi()).Arguments;
                    string ffSubs = Program.mainForm.checkAv1anCopySubs.Checked ? "-c:s copy" : "-sn";
                    string ffDat = Program.mainForm.checkAv1anCopyData.Checked ? "" : "-dn";
                    string ffAttach = Program.mainForm.checkAv1anCopyAttachs.Checked ? "-map 0:t?" : "-map -0:t?";
                    var form = Program.mainForm;

                    if (RunTask.canceled) return;

                    string ffArgs = $"{ffAud} {ffSubs} {ffDat} {ffAttach}";
                    // args = $"-i {inPath.Wrap()} -y --verbose --keep {s} {m} {c} {t} {GetScDownscaleArg()} {o} {cust} {v} -f \" {vf} \" -a \" {ffArgs} \" -w {w} {x} -o {outPath.Wrap()}";
                    
                    args = $"-i {inPath.Wrap()} -y --verbose --keep " +
                        $"--split-method {(form.av1anOptsSplitModeBox.SelectedIndex == 0 ? "none" : "av-scenechange")} " +
                        $"-m {form.av1anOptsChunkModeBox.Text.ToLower().Trim()} " +
                        $"-c {form.av1anOptsConcatModeBox.Text.ToLower().Trim()} " +
                        $"--chunk-order {form.av1anOptsChunkOrderBox.Text.Split('(')[0].ToLower().Trim()} " +
                        $"--sc-downscale-height {GetScDownscaleHeight()} " +
                        $"{Program.mainForm.av1anCustomArgsBox.Text.Trim()} " +
                        $"{codecArgs.Arguments} " +
                        $"-f \" {vf} \" " +
                        $"-a \" {ffArgs} \" " +
                        $"-w {Program.mainForm.av1anOptsWorkerCountUpDown.Value} " +
                        $"{CodecUtils.GetKeyIntArg(TrackList.current.File, Config.GetInt(Config.Key.DefaultKeyIntSecs), "-x ")} " +
                        $"-o {outPath.Wrap()}";

                    if (IsUsingVmaf())
                    {
                        int q = (int)Program.mainForm.av1anQualityUpDown.Value;
                        string filters = vf.Length > 3 ? $"--vmaf-filter \" {vf.Split("-vf ").LastOrDefault()} \"" : "";
                        args += $" --target-quality {q} --vmaf-path {Paths.GetVmafPath(false).Wrap()} {filters} --vmaf-threads 2";
                    }
                }
                else
                {
                    inPath = overrideArgs.Split("-i \"")[1].Split("\"")[0].Trim();
                    outPath = overrideArgs.Split(" -o \"").Last().Remove("\"").Trim();
                    args = overrideArgs;
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

                string tempDirName = overrideTempDir.IsNotEmpty() ? overrideTempDir : timestamp;
                tempDir = Directory.CreateDirectory(Path.Combine(Paths.GetAv1anTempPath(), tempDirName)).FullName;
                AvProcess.lastTempDirAv1an = tempDir;

                args = $"{(resume ? "-r" : "")} --temp {tempDir.Wrap()} {args}";

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
            Task.Run(() => AskDeleteTempFolder(tempDir));
        }

        private static int GetScDownscaleHeight()
        {
            if (TrackList.current.File == null || TrackList.current.File.VideoStreams.Count < 1)
                return 0;

            int h = TrackList.current.File.VideoStreams[0].Resolution.Height;
            float mult = 1f;

            if (h >= 720) mult = 0.7500f;
            if (h >= 900) mult = 0.7083f;
            if (h >= 1080) mult = 0.6667f;
            if (h >= 1440) mult = 0.5000f;
            if (h >= 2160) mult = 0.4166f;
            if (h >= 4320) mult = 0.3333f;

            return (h * mult).RoundToInt().Clamp(360, 2160); // Apply multiplicator but clamp to sane values
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
            if (!args.MatchesWildcard("* -o \"*.mkv\"*")) return; // Only do this if output is MKV

            while (!IsAv1anRunning()) await Task.Delay(200);
            while(!IsAudioDone(tempFolder)) await Task.Delay(500);
            await Task.Delay(500);

            try
            {
                string txtPath = Path.Combine(Paths.GetSessionDataPath(), $"av1an-{DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss")}.txt");
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
                Logger.Log($"IsAudioDone Error: {ex.Message}", true);
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
