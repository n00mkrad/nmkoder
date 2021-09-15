using Nmkoder.Extensions;
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
        // Quick Convert - Video
        public ComboBox containerBox;
        public ComboBox encVidCodecsBox;
        public NumericUpDown encVidQualityBox;
        public ComboBox encVidPresetBox;
        public ComboBox encVidColorsBox;
        public TextBox encVidFpsBox;
        public NumericUpDown encScaleBoxW { get { return encScaleW; } }
        public NumericUpDown encScaleBoxH { get { return encScaleH; } }
        public HTAlt.WinForms.HTButton encScaleLinkButton { get { return encScaleLinkBtn; } }
        public ComboBox encCropModeBox { get { return encCropMode; } }
        // Quick Convert - Audio
        public ComboBox encAudEnc;
        public NumericUpDown encAudBr;
        public ComboBox encAudCh;
        // Quick Convert - Subs
        public ComboBox encSubEnc;
        public ComboBox encSubBurnBox;
        // Quick Convert - Other
        public DataGridView metaGrid;
        public TextBox outputBox;
        public TextBox customArgsBox;

        public void AssignControlsQuickConvert ()
        {
            containerBox = containers;
            encVidCodecsBox = encVidCodec;
            encVidQualityBox = encVidQuality;
            encVidPresetBox = encVidPreset;
            encVidColorsBox = encVidColors;
            encVidFpsBox = encVidFps;

            encAudEnc = encAudCodec;
            encAudBr = encAudBitrate;
            encAudCh = encAudChannels;

            encSubEnc = encSubCodec;
            encSubBurnBox = encSubBurn;


            encAudChannels.SelectedIndex = 1;
        }

        private void encScaleLinkBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
