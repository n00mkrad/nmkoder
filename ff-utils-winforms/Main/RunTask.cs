using Nmkoder.Data;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.UI;
using Nmkoder.UI.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nmkoder.Main
{
    public class RunTask
    {
        public enum TaskType { Null, None, Convert, VideoToFrames, FramesToVideo, ReadBitrates };
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

            if (currentFileListMode == FileListMode.MultiFileInput && MediaInfo.current == null)
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

            if (taskType == TaskType.Convert)
                await QuickConvert.Run();

            if (taskType == TaskType.VideoToFrames)
                await Utilities.RunVideoToFrames();

            if (taskType == TaskType.ReadBitrates)
                await Utilities.RunReadBitrates();

            Logger.Log($"Done.");
            Program.mainForm.SetProgress(0);
        }

        public static async Task StartBatch ()
        {
            TaskType batchTask = Program.mainForm.GetCurrentTaskType();

            if (batchTask == TaskType.None)
            {
                MessageBox.Show("No task selected for batch processing! Please select an option (Quick Encode or one of the actions in Utilities).", "Error");
                return;
            }

            MediaInfo.ClearCurrentFile();
            System.Windows.Forms.ListBox fileList = Program.mainForm.fileListBox;

            object[] taskFileList = new object[fileList.Items.Count];
            fileList.Items.CopyTo(taskFileList, 0);

            runningBatch = true;

            for (int i = 0; i < taskFileList.Length; i++)
            {
                MediaFile mf = (MediaFile)taskFileList[i];
                Logger.Log($"Queue: Starting task {i + 1}/{taskFileList.Length} for {mf.Name}.");
                MediaInfo.ClearCurrentFile();
                await MediaInfo.LoadFirstFile(mf, false, false); // Load file info
                await Start(batchTask); // Run task
                fileList.Items.RemoveAt(0);
            }

            runningBatch = false;

            Logger.Log($"Queue: Completed {taskFileList.Length} tasks.");
        }

        public static bool RunInstantly ()
        {
            return Config.GetInt("taskMode") == 1;
        }
    }
}
