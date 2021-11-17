using Nmkoder.Data;
using Nmkoder.Data.Codecs;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
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
        public TextBox av1anCustomEncArgsBox { get { return av1anCustomEncArgs; } }
        public NumericUpDown av1anGrainSynthStrengthUpDown { get { return av1anGrainSynthStrength; } }
        public CheckBox av1anGrainSynthDenoiseBox { get { return av1anGrainSynthDenoise; } }


        public ComboBox av1anAudCodecBox { get { return av1anAudCodec; } }
        public NumericUpDown av1anAudQualUpDown { get { return av1anAudQuality; } }
        public ComboBox av1anAudChannelsBox { get { return av1anAudChannels; } }

        public TextBox av1anOutputPathBox { get { return av1anOutputPath; } }
        public TextBox av1anCustomArgsBox { get { return av1anCustomArgs; } }

        public ComboBox av1anOptsSplitModeBox { get { return av1anOptsSplitMode; } }
        public ComboBox av1anOptsChunkModeBox { get { return av1anOptsChunkMode; } }
        public ComboBox av1anOptsConcatModeBox { get { return av1anOptsConcatMode; } }
        public NumericUpDown av1anOptsWorkerCountUpDown { get { return av1anOptsWorkerCount; } }

        public void InitAv1an()
        {
            av1anAudChannels.SelectedIndex = 1;
            av1anCrop.SelectedIndex = 0;
        }

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
                IEncoder enc = CodecUtils.GetCodec((CodecUtils.Av1anCodec)av1anCodecBox.SelectedIndex);
                av1anQuality.Minimum = enc.QMin;
                av1anQuality.Maximum = enc.QMax;
                av1anQuality.Value = enc.QDefault;
            }
        }

        private void av1anContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfigAv1an();
            QuickConvertUi.ValidateContainer();
        }

        public void LoadConfigAv1an()
        {
            ConfigParser.LoadComboxIndex(av1anContainer);
            ConfigParser.LoadComboxIndex(av1anCodec);
            ConfigParser.LoadComboxIndex(av1anAudCodec);
            ConfigParser.LoadComboxIndex(av1anOptsChunkMode);
            ConfigParser.LoadComboxIndex(av1anOptsConcatMode);
            ConfigParser.LoadGuiElement(av1anOptsWorkerCount, false);
        }

        public void SaveConfigAv1an(object sender = null, EventArgs e = null)
        {
            if (!initialized)
                return;

            ConfigParser.SaveComboxIndex(av1anContainer);
            ConfigParser.SaveComboxIndex(av1anCodec);
            ConfigParser.SaveComboxIndex(av1anAudCodec);
            ConfigParser.SaveComboxIndex(av1anOptsChunkMode);
            ConfigParser.SaveComboxIndex(av1anOptsConcatMode);
            ConfigParser.SaveGuiElement(av1anOptsWorkerCount);
        }

        private void av1anAudCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveConfigAv1an();
            Av1anUi.AudEncoderSelected(av1anAudCodec.SelectedIndex);
        }

        private void av1anResumeBtn_Click(object sender, EventArgs e)
        {
            Av1anResumeForm form = new Av1anResumeForm();
            form.ShowDialog();

            if (form.ChosenEntry == null)
                return;

            if (form.Resume)
            {
                if (form.ChosenEntry.jsonInfo == null)
                {
                    Logger.Log($"Cannot resume - Failed to load info from JSON.");
                    return;
                }

                if (form.ChosenEntry.InputFile == null || !File.Exists(form.ChosenEntry.InputFile.FullName))
                {
                    Logger.Log($"Cannot resume - Input file doesn't seem to exists at '{form.ChosenEntry.InputFile}'.");
                    return;
                }

                if (form.UseSavedCommand)
                {
                    Av1an.RunResumeWithSavedArgs(form.ChosenEntry.TempFolderName, form.ChosenEntry.Args);
                }
                else
                {
                    Av1an.RunResumeWithNewArgs(form.ChosenEntry.InputFile.FullName, form.ChosenEntry.TempFolderName);
                }
            }
        }
    }
}
