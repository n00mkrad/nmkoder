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
        public ComboBox containerBox;
        public ComboBox encVidCodecsBox;
        public NumericUpDown encVidQualityBox;
        public ComboBox encVidPresetBox;
        public ComboBox encVidColorsBox;
        public TextBox encVidFpsBox;
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
            encCropMode.SelectedIndex = 0;
        }
    }
}
