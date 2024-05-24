using Nmkoder.Data;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nmkoder.Forms
{
    public partial class CropForm : Form
    {
        public CropConfig Crop { get; set; }

        private Size originalDimensions = new Size();
        private CropConfig savedCrop = null;
        private bool ready = false;
        private bool ignoreNextEvent = false;

        public CropForm(Size resolution, CropConfig crop)
        {
            originalDimensions = resolution;
            savedCrop = crop;

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

            if(!originalDimensions.IsEmpty)
            {
                cropAreaW.Value = savedCrop == null ? originalWidth : savedCrop.GetCroppedWidth(originalDimensions);
                cropAreaH.Value = savedCrop == null ? originalHeight : savedCrop.GetCroppedWidth(originalDimensions);

                cropAreaX.Value = savedCrop == null ? 0 : savedCrop.CropLeft;
                cropAreaY.Value = savedCrop == null ? 0 : savedCrop.CropTop;
            }

            cropTop.Maximum = cropBot.Maximum = originalHeight;
            cropLeft.Maximum = cropRight.Maximum = originalWidth;

            if(savedCrop != null)
            {
                cropTop.Value = savedCrop.CropTop;
                cropBot.Value = savedCrop.CropBot;

                cropLeft.Value = savedCrop.CropLeft;
                cropRight.Value = savedCrop.CropRight;
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
            // CropValues = new int[4] { (int)cropAreaW.Value, (int)cropAreaH.Value, (int)cropAreaX.Value, (int)cropAreaY.Value };
            Crop = new CropConfig((int)cropLeft.Value, (int)cropRight.Value, (int)cropTop.Value, (int)cropBot.Value);
            DialogResult = DialogResult.OK;
            Close();
            Program.mainForm.BringToFront();
        }
    }
}
