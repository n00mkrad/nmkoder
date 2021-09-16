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
            encCropMode.SelectedIndex = 1;
        }

        private void encScaleLinkBtn_Click(object sender, EventArgs e)
        {
            QuickConvertUi.scaleLink = !QuickConvertUi.scaleLink;
            encScaleLinkBtn.ButtonImage = (QuickConvertUi.scaleLink) ? Resources.baseline_link_white_24dp : Resources.baseline_unlink_white_24dp;
        }

        bool ignoreScaleValueChanged;

        // private void encScaleW_ValueChanged(object sender, EventArgs e)
        // {
        //     if (ignoreScaleValueChanged || MediaInfo.current == null || MediaInfo.current.VideoStreams.Count < 1 || MediaInfo.current.VideoStreams[0].Resolution.IsEmpty)
        //         return;
        // 
        //     int w = ((int)encScaleW.Value).RoundMod(2);
        // 
        //     if(w < 32)
        //     {
        //         ignoreScaleValueChanged = true;
        //         encScaleW.Value = MediaInfo.current.VideoStreams[0].Resolution.Width;
        //         ignoreScaleValueChanged = false;
        //         return;
        //     }
        // 
        //     if (!QuickConvertUi.scaleLink)
        //         return;
        // 
        //     float wFactor = (float)w / (float)MediaInfo.current.VideoStreams[0].Resolution.Width;
        //     ignoreScaleValueChanged = true;
        //     encScaleH.Value = (MediaInfo.current.VideoStreams[0].Resolution.Height * wFactor).RoundToInt();
        //     ignoreScaleValueChanged = false;
        // }
        // 
        // private void encScaleH_ValueChanged(object sender, EventArgs e)
        // {
        //     if (!QuickConvertUi.scaleLink || ignoreScaleValueChanged || MediaInfo.current == null || MediaInfo.current.VideoStreams.Count < 1 || MediaInfo.current.VideoStreams[0].Resolution.IsEmpty)
        //         return;
        // 
        //     int h = ((int)encScaleH.Value).RoundMod(2);
        // 
        //     if (h < 32)
        //     {
        //         ignoreScaleValueChanged = true;
        //         encScaleH.Value = MediaInfo.current.VideoStreams[0].Resolution.Height;
        //         ignoreScaleValueChanged = false;
        //         return;
        //     }
        // 
        //     if (!QuickConvertUi.scaleLink)
        //         return;
        // 
        //     float hFactor = (float)encScaleH.Value / (float)MediaInfo.current.VideoStreams[0].Resolution.Height;
        //     ignoreScaleValueChanged = true;
        //     encScaleW.Value = (MediaInfo.current.VideoStreams[0].Resolution.Width * hFactor).RoundToInt();
        //     ignoreScaleValueChanged = false;
        // }
    }
}
