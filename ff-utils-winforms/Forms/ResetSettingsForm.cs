using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms
{
    public partial class ResetSettingsForm : Form
    {

        public ResetSettingsForm()
        {
            InitializeComponent();
        }

        private void ResetSettingsForm_Load(object sender, EventArgs e)
        {
            var properties = typeof(ResetSettingsOnNewFile).GetProperties();

            foreach (var prop in properties)
            {
                if (!prop.Name.StartsWith("Reset"))
                    continue;

                settingsList.Items.Add(new ListViewItem { Text = ResetSettingsOnNewFile.NiceNames[prop.Name], Checked = (bool)prop.GetValue(null, null) });
            }
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            Set();
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }

        private void Set ()
        {
            var reversedNiceNames = ResetSettingsOnNewFile.NiceNames.ToDictionary(x => x.Value, x => x.Key);

            foreach (ListViewItem item in settingsList.Items.Cast<ListViewItem>())
            {
                string propName = reversedNiceNames[item.Text];
                typeof(ResetSettingsOnNewFile).GetProperty(propName).SetValue(null, item.Checked);
            }
        }
    }
}
