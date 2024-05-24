﻿using Nmkoder.Extensions;
using Nmkoder.UI;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nmkoder.UI.Tasks;
using Nmkoder.Data;
using Nmkoder.Data.Ui;
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
        // Quick Convert - Advanced
        public DataGridView EncAdvancedFiltersGrid { get { return encAdvancedFiltersGrid; } }
        // Quick Convert - Audio
        public ComboBox encAudCodecBox { get { return encAudCodec; } }
        public ComboBox encAudConfModeBox { get { return encAudConfMode; } }
        public NumericUpDown encAudQualUpDown { get { return encAudQuality; } }
        public ComboBox encAudChannelsBox { get { return encAudChannels; } }
        // Quick Convert - Subs
        public ComboBox encSubCodecBox { get { return encSubCodec; } }
        public ComboBox encSubBurnBox { get { return encSubBurn; } }
        // Quick Convert - Other
        public TextBox FfmpegOutputBox { get { return outputPath; } }
        public TextBox customArgsInBox { get { return encCustomArgsIn; } }
        public TextBox customArgsOutBox { get { return encCustomArgsOut; } }
        // Metadata
        public DataGridView EncMetadataGrid { get { return encMetadataGrid; } }
        public ComboBox EncMetaCopySource { get { return encMetaCopySource; } }
        public ComboBox EncMetaChapterSource { get { return encMetaChapterSource; } }
        public CheckBox EncMetaApplyGrid { get { return encMetaApplyGrid; } }

        public void InitQuickConvert ()
        {
            encAudChannels.SelectedIndex = 0;
            encCropMode.SelectedIndex = 0;
        }

        private void encVidCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuickConvertUi.VidEncoderSelected(encVidCodec.SelectedIndex);
        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuickConvertUi.ValidateContainer();
        }

        private void encQualityMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            encVidQuality.DecimalPlaces = 0;

            if ((QuickConvert.QualityMode)encQualMode.SelectedIndex == QuickConvert.QualityMode.TargetKbps)
            {
                encVidQuality.Minimum = 10;
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

        private void encAudConfigureBtn_Click(object sender, EventArgs e)
        {
            AudioStreamsForm form = new AudioStreamsForm(TrackList.current.File, (int)encAudQuality.Value);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK && form.ConfigurationEntries != null && form.ConfigurationEntries.Count > 0)
                TrackList.currentAudioConfig = new AudioConfiguration(TrackList.current.File, form.ConfigurationEntries);
        }

        private void encAudConfMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = encAudConfMode.SelectedIndex;


            if(i == 1)
            {
                bool currentNull = TrackList.current == null;
                bool noEnc = !Program.mainForm.encAudQualUpDown.Enabled;
                bool noAudTracks = streamList.Items.Cast<ListViewItem>().Where(x => ((StreamListEntry)x.Tag).Stream.Type == Data.Streams.Stream.StreamType.Audio).Count() < 1;

                if (currentNull || noAudTracks || noEnc)
                {
                    if(currentNull)
                        UiUtils.ShowMessageBox("Please load a file first in order to configure its audio tracks.", UiUtils.MessageType.Error);
                    else if(noAudTracks)
                        UiUtils.ShowMessageBox("This is only available if you have at least one audio track in the track list.", UiUtils.MessageType.Error);
                    else if (noEnc)
                        UiUtils.ShowMessageBox("The selected audio encoder does not support custom bitrates.", UiUtils.MessageType.Error);

                    encAudConfMode.SelectedIndex = 0;
                    return;
                }
                else
                {
                    encAudConfigureBtn_Click(null, null);
                }
            }

            encAudPerTrackPanel.Visible = i == 1;
        }

        private void encAudConfMode_VisibleChanged(object sender, EventArgs e)
        {
            int i = encAudConfMode.SelectedIndex;

            if (i == 1)
            {
                bool noEnc = !Program.mainForm.encAudQualUpDown.Enabled;
                bool noAudTracks = streamList.Items.Cast<ListViewItem>().Where(x => ((StreamListEntry)x.Tag).Stream.Type == Data.Streams.Stream.StreamType.Audio).Count() < 1;

                if (TrackList.current == null || TrackList.currentAudioConfig == null || TrackList.currentAudioConfig.CreationFile.ImportPath != TrackList.current.File.ImportPath || noAudTracks || noEnc)
                {
                    TrackList.currentAudioConfig = null;
                    encAudConfMode.SelectedIndex = 0;
                    return;
                }
            }
        }

        private void metadataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            QuickConvertUi.SaveMetadata();
        }
    }
}
