using Nmkoder.Extensions;
using Nmkoder.IO;
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
    public partial class CropForm : Form
    {
        public int[] CropValues { get; set; }
        private Size originalDimensions = new Size();

        private bool ready = false;
        private bool ignoreNextEvent = false;

        public CropForm(Size resolution)
        {
            originalDimensions = resolution;

            InitializeComponent();
        }

        private void CropForm_Load(object sender, EventArgs e)
        {
            noVidPanel.Visible = originalDimensions.IsEmpty;

            int originalWidth = originalDimensions.IsEmpty ? 16380 : originalDimensions.Width;
            int originalHeight = originalDimensions.IsEmpty ? 16380 : originalDimensions.Height;

            cropAreaX.Maximum = 0;
            cropAreaY.Maximum = 0;
            
            cropAreaW.Maximum = originalWidth;
            cropAreaH.Maximum = originalHeight;
            
            cropAreaX.Maximum = originalWidth;
            cropAreaY.Maximum = originalHeight;

            cropAreaW.Value = originalWidth;
            cropAreaH.Value = originalHeight;

            cropAreaX.Value = 0;
            cropAreaY.Value = 0;

            cropTop.Maximum = cropBot.Maximum = originalHeight;
            cropLeft.Maximum = cropRight.Maximum = originalWidth;

            ready = true;
        }

        private void CropBotTopChanged(object sender, EventArgs e)
        {
            if (!ready || ignoreNextEvent || originalDimensions.IsEmpty) return;

            cropAreaH.Value = originalDimensions.Height - cropTop.Value - cropBot.Value;
            cropAreaY.Value = cropTop.Value;
        }

        private void CropLeftRightChanged(object sender, EventArgs e)
        {
            if (!ready || ignoreNextEvent || originalDimensions.IsEmpty) return;

            cropAreaW.Value = originalDimensions.Width - cropLeft.Value - cropRight.Value;
            cropAreaX.Value = cropLeft.Value;
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            ignoreNextEvent = true;
            cropTop.Value = cropBot.Value = cropLeft.Value = cropRight.Value = 0;
            CropForm_Load(null, null);
            ignoreNextEvent = false;
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            CropValues = new int[4] { (int)cropAreaW.Value, (int)cropAreaH.Value, (int)cropAreaX.Value, (int)cropAreaY.Value };
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }
    }
}
