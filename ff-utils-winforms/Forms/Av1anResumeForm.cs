using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.IO;
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

namespace Nmkoder.Forms
{
    public partial class Av1anResumeForm : Form
    {

        public Av1anFolderEntry ChosenEntry { get; set; } = null;
        public bool Resume { get; set; }
        public bool UseSavedCommand { get; set; }

        public Av1anResumeForm()
        {
            InitializeComponent();
        }

        private void Av1anResumeForm_Load(object sender, EventArgs e)
        {

        }

        private void Av1anResumeForm_Shown(object sender, EventArgs e)
        {
            ReloadList();
        }

        private void ReloadList ()
        {
            folderList.Items.Clear();
            string av1anDir = Paths.GetAv1anTempPath();
            folderList.Items.AddRange(new DirectoryInfo(av1anDir).GetDirectories().Select(x => new Av1anFolderEntry(x.FullName)).ToArray());

            if(folderList.Items.Count > 0)
                folderList.SelectedIndex = 0;
            else
                folderList.SelectedIndex = -1;
        }

        private void Done()
        {
            ChosenEntry = (Av1anFolderEntry)folderList.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }

        private void resumeWithSavedSettings_Click(object sender, EventArgs e)
        {
            Resume = true;
            UseSavedCommand = true;
            Done();
        }

        private void resumeWithNewSettings_Click(object sender, EventArgs e)
        {
            Resume = true;
            UseSavedCommand = false;
            Done();
        }

        private void DeleteFull_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderList.SelectedItem == null)
                    return;

                Av1anFolderEntry entry = (Av1anFolderEntry)folderList.SelectedItem;
                IoUtils.DeleteIfExists(entry.DirInfo.FullName);
                IoUtils.DeleteIfExists(entry.DirInfo.FullName + ".json");
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to delete av1an folder: {ex.Message}");
            }

            ReloadList();
        }

        private void DeleteChunks_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderList.SelectedItem == null)
                    return;

                Av1anFolderEntry entry = (Av1anFolderEntry)folderList.SelectedItem;
                IoUtils.DeleteIfExists(Path.Combine(entry.DirInfo.FullName, "encode"));
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to delete av1an encode folder: {ex.Message}");
            }

            ReloadList();
        }
    }
}
