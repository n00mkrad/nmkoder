using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.Forms
{
    partial class MainForm
    {
        public ComboBox av1anContainerBox { get { return av1anContainer; } }
        public ComboBox av1anCodecBox { get { return av1anCodec; } }
        public NumericUpDown av1anQualityUpDown { get { return av1anQuality; } }
        public ComboBox av1anQualModeBox { get { return av1anQualityMode; } }
        public ComboBox av1anPresetBox { get { return av1anPreset; } }
        public ComboBox av1anColorsBox { get { return av1anColorSpace; } }
        public TextBox av1anFpsBox { get { return av1anFps; } }
        public TextBox av1anScaleBoxW { get { return av1anScaleW; } }
        public TextBox av1anScaleBoxH { get { return av1anScaleH; } }
        public ComboBox av1anCropBox { get { return av1anCrop; } }


        public ComboBox av1anAudCodecBox { get { return av1anAudCodec; } }
        public NumericUpDown av1anAudQualUpDown { get { return av1anAudQuality; } }
        public ComboBox av1anAudChannelsBox { get { return av1anAudChannels; } }

        public TextBox av1anOutputPathBox { get { return av1anOutputPath; } }
        public TextBox av1anCustomArgsBox { get { return av1anCustomArgs; } }

        private void av1anCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            Av1anUi.VidEncoderSelected(av1anCodec.SelectedIndex);
        }

        private void av1anQualityMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MiscUtils.ParseEnum<Av1an.QualityMode>(av1anQualityMode.Text.Trim().Remove(" ")) == Av1an.QualityMode.TargetVmaf)
            {
                av1anQuality.Minimum = 10;
                av1anQuality.Maximum = 99;
                av1anQuality.Value = 95;
            }
            else
            {
                CodecInfo info = Codecs.GetCodecInfo((Codecs.Av1anCodec)av1anCodecBox.SelectedIndex);
                av1anQuality.Minimum = info.QMin;
                av1anQuality.Maximum = info.QMax;
                av1anQuality.Value = info.QDefault;
            }
        }

        private void av1anContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUiConfig();
            QuickConvertUi.ValidateContainer();
        }
    }
}
