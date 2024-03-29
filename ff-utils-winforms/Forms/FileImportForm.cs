﻿using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms
{
    public partial class FileImportForm : Form
    {
        private string[] files;
        public List<string> ImportFiles { get; set; } = new List<string>();

        public bool Clear { get; set; }

        public FileImportForm(string[] files, bool allowClear)
        {
            InitializeComponent();
            Text = $"Import {files.Length} File{(files.Length != 1 ? "s" : "")}";
            this.files = files;
            importClearBtn.Visible = allowClear;
            AcceptButton = importAppendBtn;
        }

        private void FileImportForm_Load(object sender, EventArgs e)
        {

        }

        private async void FileImportForm_Shown(object sender, EventArgs e)
        {
            if (!importClearBtn.Visible)
                importAppendBtn.Text = importAppendBtn.Text.Split(' ')[0];

            fileList.Items.Clear();

            foreach (string file in files)
            {
                MediaFile mediaFile = new MediaFile(file, false);
                FileListEntry entry = new FileListEntry(mediaFile);
                fileList.Items.Add(entry);
                fileList.SetItemChecked(fileList.Items.Count - 1, true);
            }
        }

        private void importClearBtn_Click(object sender, EventArgs e)
        {
            Clear = true;
            Done();
        }

        private void importAppendBtn_Click(object sender, EventArgs e)
        {
            Clear = false;
            Done();
        }

        private void Done ()
        {
            ImportFiles.Clear();

            for (int i = 0; i < fileList.Items.Count; i++)
            {
                if (fileList.GetItemChecked(i))
                    ImportFiles.Add(((FileListEntry)fileList.Items[i]).File.SourcePath);
            }

            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                Close();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}
