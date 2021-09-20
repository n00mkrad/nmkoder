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
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;
using Nmkoder.UI.Tasks;
using Nmkoder.Main;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
using Nmkoder.Properties;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        // Quick Convert - Video
        public ComboBox containerBox { get { return containers; } }
        public ComboBox encVidCodecsBox { get { return encVidCodec; } }
        public NumericUpDown encVidQualityBox { get { return encVidQuality; } }
        public Label qInfoLabel { get { return qInfo; } }
        public ComboBox encVidPresetBox { get { return encVidPreset; } }
        public ComboBox encVidColorsBox { get { return encVidColors; } }
        public TextBox encVidFpsBox { get { return encVidFps; } }
        public TextBox encScaleBoxW { get { return encScaleW; } }
        public TextBox encScaleBoxH { get { return encScaleH; } }
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
        public TextBox customArgsInBox { get { return encCustomArgsIn; } }
        public TextBox customArgsOutBox { get { return encCustomArgsOut; } }

        public void InitQuickConvert ()
        {
            encAudEnc = encAudCodec;
            encAudBr = encAudBitrate;
            encAudCh = encAudChannels;

            encSubEnc = encSubCodec;
            encSubBurnBox = encSubBurn;

            encAudChannels.SelectedIndex = 1;
            encCropMode.SelectedIndex = 0;
        }
    }
}
