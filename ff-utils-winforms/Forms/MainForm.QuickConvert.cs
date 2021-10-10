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
using Nmkoder.Utils;
using Nmkoder.Data.Codecs;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        // Quick Convert - Video
        public ComboBox ffmpegContainerBox { get { return containers; } }
        public ComboBox encVidCodecsBox { get { return encVidCodec; } }
        public NumericUpDown encVidQualityBox { get { return encVidQuality; } }
        public ComboBox encQualModeBox { get { return encQualMode; } }
        public Label qInfoLabel { get { return qInfo; } }
        public ComboBox encVidPresetBox { get { return encVidPreset; } }
        public Label presetInfoLabel { get { return presetInfo; } }
        public ComboBox encVidColorsBox { get { return encVidColors; } }
        public TextBox encVidFpsBox { get { return encVidFps; } }
        public TextBox encScaleBoxW { get { return encScaleW; } }
        public TextBox encScaleBoxH { get { return encScaleH; } }
        public ComboBox encCropModeBox { get { return encCropMode; } }
        // Quick Convert - Audio
        public ComboBox encAudCodecBox { get { return encAudCodec; } }
        public NumericUpDown encAudQualUpDown { get { return encAudQuality; } }
        public ComboBox encAudChannelsBox { get { return encAudChannels; } }
        // Quick Convert - Subs
        public ComboBox encSubCodecBox { get { return encSubCodec; } }
        public ComboBox encSubBurnBox { get { return encSubBurn; } }
        // Quick Convert - Other
        public DataGridView metaGrid;
        public TextBox ffmpegOutputBox;
        public TextBox customArgsInBox { get { return encCustomArgsIn; } }
        public TextBox customArgsOutBox { get { return encCustomArgsOut; } }

        public void InitQuickConvert ()
        {
            encAudChannels.SelectedIndex = 1;
            encCropMode.SelectedIndex = 0;
        }

        private void encVidCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUiConfig();
            QuickConvertUi.VidEncoderSelected(encVidCodec.SelectedIndex);
        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUiConfig();
            QuickConvertUi.ValidateContainer();
        }

        private void encQualityMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            encVidQuality.DecimalPlaces = 0;

            if ((QuickConvert.QualityMode)encQualMode.SelectedIndex == QuickConvert.QualityMode.TargetKbps)
            {
                encVidQuality.Minimum = 50;
                encVidQuality.Maximum = 100000;
                encVidQuality.Value = 1500;
            }
            else if ((QuickConvert.QualityMode)encQualMode.SelectedIndex == QuickConvert.QualityMode.TargetMbytes)
            {
                encVidQuality.DecimalPlaces = 1;
                encVidQuality.Minimum = 0;
                encVidQuality.Maximum = 8192;
                encVidQuality.Value = 50;
            }
            else
            {
                IEncoder enc = CodecUtils.GetCodec((CodecUtils.VideoCodec)encVidCodec.SelectedIndex);
                encVidQuality.Minimum = enc.QMin.Clamp(0, int.MaxValue);
                encVidQuality.Maximum = enc.QMax.Clamp(0, int.MaxValue);
                encVidQuality.Value = enc.QDefault.Clamp(0, int.MaxValue);
            }
        }
    }
}
