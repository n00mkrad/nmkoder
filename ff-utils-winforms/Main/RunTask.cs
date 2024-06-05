using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.OS;
using Nmkoder.UI;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Nmkoder.Main
{
    public class RunTask
    {
        public enum TaskType { Null, None, Convert, Av1an, UtilReadBitrates, UtilGetMetrics, UtilOcr, UtilColorData, UtilConcat, PlotBitrate };
        //public static TaskType currentTask;

        public enum FileListMode { Mux, Batch };
        public static FileListMode currentFileListMode;

        public static bool runningBatch = false;
        public static bool canceled = false;

        public static void Cancel(string reason = "", bool noMsgBox = false)
        {
            canceled = true;
            Program.mainForm.SetStatus("Canceled.");
            Program.mainForm.SetProgress(0);

            ProcessManager.KillPrimary();
            ProcessManager.KillSecondary();

            Program.mainForm.SetWorking(false);
            Logger.LogIfLastLineDoesNotContainMsg("Canceled.");

            if (!string.IsNullOrWhiteSpace(reason) && !noMsgBox)
                UiUtils.ShowMessageBox($"Canceled:\n\n{reason}", UiUtils.MessageType.Error); 
        }

        public static async Task Start(TaskType batchTask = TaskType.Null)
        {
            if (batchTask == TaskType.Null)
                runningBatch = false;

            TaskType task = batchTask == TaskType.Null ? Program.mainForm.SelectedTask : batchTask;

            if (Program.mainForm.fileListBox.Items.Count < 1)
            {
                UiUtils.ShowMessageBox("No input files in file list! Please add one or more files first.");
                Program.mainForm.MainTabList.SelectedIndex = 0;
                return;
            }
            else
            {
                List<MediaFile> mediaFiles = Program.mainForm.fileListBox.Items.Cast<ListViewItem>().Select(x => ((FileListEntry)x.Tag).File).ToList();
                var missingFiles = mediaFiles.Where(x => !x.CheckFiles()).Select(x => x.SourcePath);

                if (missingFiles.Any())
                {
                    UiUtils.ShowMessageBox($"The following files have been imported but are no longer accessible:\n\n{string.Join("\n", missingFiles)}\n\n" +
                        $"Possibly they were deleted, moved, or renamed.\nPlease either restore them or remove them from the file list.", UiUtils.MessageType.Error);
                    return;
                }
            }

            bool loadedFileRequired = task == TaskType.Convert || task == TaskType.Av1an || task == TaskType.UtilReadBitrates || task == TaskType.UtilOcr;

            if (loadedFileRequired && (currentFileListMode == FileListMode.Mux && TrackList.current == null))
            {
                UiUtils.ShowMessageBox("No input file loaded! Please load one first (File List).");
                return;
            }

            if (task == TaskType.None)
            {
                if (!RunInstantly())
                    UiUtils.ShowMessageBox("No task selected! Please select an option (Quick Encode or one of the actions in Utilities).");

                return;
            }

            canceled = false;
            FfmpegOutputHandler.overrideTargetDurationMs = -1;
            NmkdStopwatch sw = new NmkdStopwatch();

            Program.mainForm.RunningTask = task;
            if (task == TaskType.Convert) await QuickConvert.Run();
            else if (task == TaskType.Av1an) await Av1an.Run();
            else if (task == TaskType.UtilReadBitrates) await UtilReadBitrates.Run();
            else if (task == TaskType.UtilGetMetrics) await UtilGetMetrics.Run();
            else if (task == TaskType.UtilOcr) await UtilOcr.Run();
            else if (task == TaskType.UtilColorData) await UtilColorData.Run();
            else if (task == TaskType.UtilConcat) await UtilConcat.Run();
            else if (task == TaskType.PlotBitrate) await UtilPlotBitrate.Run();
            Program.mainForm.RunningTask = TaskType.None;

            Logger.Log($"Done - Finished task in {sw}.");
            Program.mainForm.SetProgress(0);
            Program.mainForm.SetWorking(false);
        }

        public static async Task StartBatch()
        {
            canceled = false;
            TaskType batchTask = Program.mainForm.SelectedTask;

            if (batchTask == TaskType.None)
            {
                UiUtils.ShowMessageBox("No task selected for batch processing! Please select an option (Quick Encode, AV1AN or one of the actions in Utilities).");
                return;
            }

            TrackList.ClearCurrentFile();
            ListView fileList = Program.mainForm.fileListBox;

            ListViewItem[] taskFileListItems = new ListViewItem[fileList.Items.Count];
            fileList.Items.CopyTo(taskFileListItems, 0);

            runningBatch = true;
            int finishedTasks = 0;
            NmkdStopwatch sw = new NmkdStopwatch();

            for (int i = 0; i < taskFileListItems.Length; i++)
            {
                if (canceled)
                    break;

                FileListEntry entry = (FileListEntry)taskFileListItems[i].Tag;
                Logger.Log($"Queue: Starting task {i + 1}/{taskFileListItems.Length} for {entry.File.Name}.");
                TrackList.ClearCurrentFile();
                await TrackList.SetAsMainFile(taskFileListItems[i], false, false); // Load file info
                await TrackList.AddStreamsToList(((FileListEntry)taskFileListItems[i].Tag).File, taskFileListItems[i].BackColor, true); // Load tracks into list (readonly for user)
                await Start(batchTask); // Run task

                if (!canceled)
                    finishedTasks++;
            }

            TrackList.ClearCurrentFile(true);
            runningBatch = false;

            Logger.Log($"Queue: Completed {finishedTasks}/{taskFileListItems.Length} tasks{(canceled ? " (Canceled)" : "")}. Total time: {sw}");
        }

        public static bool RunInstantly()
        {
            return Config.GetInt("taskMode") == 1;
        }
    }
}
