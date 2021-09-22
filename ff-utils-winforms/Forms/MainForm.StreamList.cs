using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Properties;
using System;
using Nmkoder.UI;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        public TextBox streamDetailsBox { get { return streamDetails; } }

        public void RefreshStreamListUi()
        {
            string note = "Stream selection is not available in Batch Processing Mode.";

            if (RunTask.currentFileListMode == RunTask.FileListMode.BatchProcess)
                formatInfo.Text = note;
            else if (formatInfo.Text == note)
                formatInfo.Text = "";
        }

        private void streamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (streamList.SelectedItem == null)
                return;

            MediaStreamListEntry entry = (MediaStreamListEntry)streamList.SelectedItem;
            streamDetails.Text = MediaInfo.GetStreamDetails(entry.Stream, entry.MediaFile);
        }

        private void streamList_Leave(object sender, EventArgs e)
        {
            QuickConvertUi.LoadMetadataGrid();
        }

        private void streamList_MouseDown(object sender, MouseEventArgs e)
        {
            if (streamList.IndexFromPoint(new Point(e.X, e.Y)) <= -1) // if no item was clicked
                streamList.SelectedItems.Clear();
        }
    }
}
