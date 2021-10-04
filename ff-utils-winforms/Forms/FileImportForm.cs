using Nmkoder.Data.Ui;
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
            Text = $"Import {files.Length} Files...";
            this.files = files;
            importClearBtn.Visible = allowClear;
            AcceptButton = importAppendBtn;
        }

        private void FileImportForm_Load(object sender, EventArgs e)
        {

        }

        private void FileImportForm_Shown(object sender, EventArgs e)
        {
            fileList.Items.Clear();

            foreach (string file in files)
            {
                SimpleFileListEntry entry = new SimpleFileListEntry(file);
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
                    ImportFiles.Add(((SimpleFileListEntry)fileList.Items[i]).File.FullName);
            }

            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }
    }
}
