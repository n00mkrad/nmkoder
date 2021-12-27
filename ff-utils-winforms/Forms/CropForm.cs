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
        public int[] CropValues { get; set; } // W, H, X, Y

        private Size originalDimensions = new Size();
        private int[] savedCropValues = null;
        private bool ready = false;
        private bool ignoreNextEvent = false;

        public CropForm(Size resolution, int[] cropValues = null)
        {
            originalDimensions = resolution;
            savedCropValues = cropValues;

            InitializeComponent();
        }

        private void CropForm_Load(object sender, EventArgs e)
        {
            noVidPanel.Visible = originalDimensions.IsEmpty;

            int originalWidth = originalDimensions.IsEmpty ? 16384 : originalDimensions.Width;
            int originalHeight = originalDimensions.IsEmpty ? 16384 : originalDimensions.Height;

            cropAreaX.Maximum = 0;
            cropAreaY.Maximum = 0;
            
            cropAreaW.Maximum = originalWidth;
            cropAreaH.Maximum = originalHeight;
            
            cropAreaX.Maximum = originalWidth;
            cropAreaY.Maximum = originalHeight;

            cropAreaW.Value = savedCropValues == null ? originalWidth : savedCropValues[0];
            cropAreaH.Value = savedCropValues == null ? originalHeight : savedCropValues[1];

            cropAreaX.Value = savedCropValues == null ? 0 : savedCropValues[2];
            cropAreaY.Value = savedCropValues == null ? 0 : savedCropValues[3];

            cropTop.Maximum = cropBot.Maximum = originalHeight;
            cropLeft.Maximum = cropRight.Maximum = originalWidth;

            if(savedCropValues != null)
            {
                int w = savedCropValues[0];
                int h = savedCropValues[1];
                int x = savedCropValues[2];
                int y = savedCropValues[3];

                int totalCropH = originalHeight - h;
                cropTop.Value = y;
                cropBot.Value = totalCropH - y;

                int totalCropW = originalWidth - w;
                cropLeft.Value = x;
                cropRight.Value = totalCropW - x;
            }

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
