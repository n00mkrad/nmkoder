using Nmkoder.Extensions;
using Nmkoder.GuiHelpers;
using Nmkoder.IO;
using Nmkoder.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using ImageMagick;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data;
using Nmkoder.Data.Ui;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        private RunTask.TaskType currentTask;

        public RunTask.TaskType GetUtilsTaskType ()
        {
            return currentTask;
        }

        private void utilsVidToFramesSelBtn_Click(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.VideoToFrames;
            UpdatePanels();
        }

        private void utilsFramesToVidSelBtn_Click(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.FramesToVideo;
            UpdatePanels();
        }

        private void utilsBitratesSelBtn_Click(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.ReadBitrates;
            UpdatePanels();
        }

        private void UpdatePanels ()
        {
            utilsVidToFramesPanel.BorderStyle = (currentTask == RunTask.TaskType.VideoToFrames) ? BorderStyle.FixedSingle : BorderStyle.None;
            utilsFramesToVidPanel.BorderStyle = (currentTask == RunTask.TaskType.FramesToVideo) ? BorderStyle.FixedSingle : BorderStyle.None;
            utilsBitratesPanel.BorderStyle = (currentTask == RunTask.TaskType.ReadBitrates) ? BorderStyle.FixedSingle : BorderStyle.None;
        }
    }
}
