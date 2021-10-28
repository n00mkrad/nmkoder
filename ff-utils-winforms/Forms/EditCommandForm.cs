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
    public partial class EditCommandForm : Form
    {
        public string Args { get; set; }

        public EditCommandForm(string exeName, string command)
        {
            InitializeComponent();
            Text = $"Edit {exeName} Arguments";
            textBox.Text = command;
            AcceptButton = confirmBtn;
        }

        private void EditCommandForm_Load(object sender, EventArgs e)
        {

        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            Args = textBox.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }
    }
}
