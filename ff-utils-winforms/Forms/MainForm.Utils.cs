﻿using Nmkoder.Extensions;
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

        public RunTask.TaskType GetUtilsTaskType()
        {
            return currentTask;
        }

        private void SelectReadBitrates(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.UtilReadBitrates;
            UpdatePanels();
        }

        private void SelectGetMetrics(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.UtilGetMetrics;
            UpdatePanels();
            utilsMetricsConfBtn_Click(null, null);
        }

        private void utilsMetricsConfBtn_Click(object sender, EventArgs e)
        {
            if (fileListBox.Items.Count < 2)
            {
                Logger.Log($"You need to load at least 2 files into the file list to use this utility!");
                return;
            }

            Utils.UtilsMetricsForm form = new Utils.UtilsMetricsForm();
            form.ShowDialog();
            SetMetricsVarsFromForm(form);
        }

        public void SetMetricsVarsFromForm (Utils.UtilsMetricsForm form)
        {
            if (form.DialogResult != DialogResult.OK)
                return;

            UtilGetMetrics.subsample = form.Subsample;
            UtilGetMetrics.alignMode = form.AlignMode;
            UtilGetMetrics.vmafModel = form.VmafModel;
            UtilGetMetrics.runVmaf = form.CheckedBoxes[0];
            UtilGetMetrics.runSsim = form.CheckedBoxes[1];
            UtilGetMetrics.runPsnr = form.CheckedBoxes[2];
            UtilGetMetrics.vidLq = form.VideoLq;
            UtilGetMetrics.vidHq = form.VideoHq;
        }

        private void SelectColorData(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.UtilColorData;
            UpdatePanels();
        }


        private void utilsColorDataConfBtn_Click(object sender, EventArgs e)
        {
            Utils.UtilsColorDataForm form = new Utils.UtilsColorDataForm();

            if (form.IsDisposed)
                return;

            form.ShowDialog();
            SetColorDataVarsFromForm(form);
        }

        public void SetColorDataVarsFromForm(Utils.UtilsColorDataForm form)
        {
            if (form == null || form.DialogResult != DialogResult.OK)
                return;

            UtilColorData.copyColorSpace = form.TransferColorSpace;
            UtilColorData.copyHdrData = form.TransferHdrData;
            UtilColorData.vidSrc = form.VideoSrc;
            UtilColorData.vidTarget = form.VideoTarget;
        }

        private void SelectOcr(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.UtilOcr;
            UpdatePanels();
        }

        private void SelectConcat(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.UtilConcat;
            UpdatePanels();
        }

        private void SelectBitratePlotUtil(object sender, EventArgs e)
        {
            currentTask = RunTask.TaskType.PlotBitrate;
            UpdatePanels();
        }

        private void UpdatePanels()
        {
            utilsBitratesPanel.BorderStyle = (currentTask == RunTask.TaskType.UtilReadBitrates) ? BorderStyle.FixedSingle : BorderStyle.None;
            utilsMetricsPanel.BorderStyle = (currentTask == RunTask.TaskType.UtilGetMetrics) ? BorderStyle.FixedSingle : BorderStyle.None;
            //utilsOcrPanel.BorderStyle = (currentTask == RunTask.TaskType.UtilOcr) ? BorderStyle.FixedSingle : BorderStyle.None;
            utilsColorDataPanel.BorderStyle = (currentTask == RunTask.TaskType.UtilColorData) ? BorderStyle.FixedSingle : BorderStyle.None;
            utilsConcatPanel.BorderStyle = (currentTask == RunTask.TaskType.UtilConcat) ? BorderStyle.FixedSingle : BorderStyle.None;
            utilsBitratePlotPanel.BorderStyle = (currentTask == RunTask.TaskType.PlotBitrate) ? BorderStyle.FixedSingle : BorderStyle.None;
        }
    }
}
