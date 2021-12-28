using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.UI;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
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

        public enum FileListMode { MultiFileInput, BatchProcess };
        public static FileListMode currentFileListMode;

        public static bool runningBatch = false;
        public static bool canceled = false;

        public static void Cancel(string reason = "", bool noMsgBox = false)
        {
            //if (current == null)
            //    return;

            canceled = true;
            Program.mainForm.SetStatus("Canceled.");
            Program.mainForm.SetProgress(0);
            AvProcess.Kill();

            Program.mainForm.SetWorking(false);
            // Program.mainForm.SetTab("interpolation");
            Logger.LogIfLastLineDoesNotContainMsg("Canceled.");

            if (!string.IsNullOrWhiteSpace(reason) && !noMsgBox)
                MessageBox.Show($"Canceled:\n\n{reason}", "Message");
        }

        public static async Task Start (TaskType batchTask = TaskType.Null)
        {
            if (batchTask == TaskType.Null)
                runningBatch = false;

            TaskType taskType = batchTask == TaskType.Null ? Program.mainForm.GetCurrentTaskType() : batchTask;

            if (Program.mainForm.fileListBox.Items.Count < 1)
            {
                MessageBox.Show("No input files in file list! Please add one or more files first.", "Error");
                Program.mainForm.mainTabList.SelectedIndex = 0;
                return;
            }

            bool loadedFileRequired = taskType == TaskType.Convert || taskType == TaskType.Av1an || taskType == TaskType.UtilReadBitrates || taskType == TaskType.UtilOcr;

            if (loadedFileRequired && (currentFileListMode == FileListMode.MultiFileInput && TrackList.current == null))
            {
                MessageBox.Show("No input file loaded! Please load one first (File List).", "Error");
                return;
            }

            if (taskType == TaskType.None)
            {
                if(!RunInstantly())
                    MessageBox.Show("No task selected! Please select an option (Quick Encode or one of the actions in Utilities).", "Error");

                return;
            }

            canceled = false;
            FfmpegOutputHandler.overrideTargetDurationMs = -1;
            NmkdStopwatch sw = new NmkdStopwatch();

            if (taskType == TaskType.Convert) await QuickConvert.Run();
            else if (taskType == TaskType.Av1an) await Av1an.Run();
            else if (taskType == TaskType.UtilReadBitrates) await UtilReadBitrates.Run();
            else if (taskType == TaskType.UtilGetMetrics) await UtilGetMetrics.Run();
            else if (taskType == TaskType.UtilOcr) await UtilOcr.Run();
            else if (taskType == TaskType.UtilColorData) await UtilColorData.Run();
            else if (taskType == TaskType.UtilConcat) await UtilConcat.Run();
            else if (taskType == TaskType.PlotBitrate) await UtilPlotBitrate.Run();

            Logger.Log($"Done - Finished task in {sw}.");
            Program.mainForm.SetProgress(0);
            Program.mainForm.SetWorking(false);
        }

        public static async Task StartBatch ()
        {
            TaskType batchTask = Program.mainForm.GetCurrentTaskType();

            if (batchTask == TaskType.None)
            {
                MessageBox.Show("No task selected for batch processing! Please select an option (Quick Encode, AV1AN or one of the actions in Utilities).", "Error");
                return;
            }

            TrackList.ClearCurrentFile();
            System.Windows.Forms.ListView fileList = Program.mainForm.fileListBox;

            ListViewItem[] taskItems = new ListViewItem[fileList.Items.Count];
            fileList.Items.CopyTo(taskItems, 0);

            runningBatch = true;

            for (int i = 0; i < taskItems.Length; i++)
            {
                FileListEntry entry = (FileListEntry)taskItems[i].Tag;
                Logger.Log($"Queue: Starting task {i + 1}/{taskItems.Length} for {entry.File.Name}.");
                TrackList.ClearCurrentFile();
                await TrackList.LoadFirstFile(taskItems[i], false, false); // Load file info
                await Start(batchTask); // Run task
            }

            runningBatch = false;

            Logger.Log($"Queue: Completed {taskItems.Length} tasks.");
        }

        public static bool RunInstantly ()
        {
            return Config.GetInt("taskMode") == 1;
        }
    }
}
