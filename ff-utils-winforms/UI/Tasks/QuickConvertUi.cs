using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.UI.Tasks
{
    partial class QuickConvertUi : QuickConvert
    {
        private static Form1 form;

        public static void Init()
        {
            form = Program.mainForm;

            foreach (Codecs.VideoCodec c in Enum.GetValues(typeof(Codecs.VideoCodec)))
                form.encEncoderBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);

            ConfigParser.LoadComboxIndex(form.encEncoderBox);

            foreach (string c in Enum.GetNames(typeof(Containers.Container)))
                form.containerBox.Items.Add(c.ToUpper());

            ConfigParser.LoadComboxIndex(form.containerBox);
        }

        public static void VidEncoderSelected(int index)
        {
            Codecs.VideoCodec c = (Codecs.VideoCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);

            LoadQualityLevel(info);
            LoadPresets(info);
            LoadColorFormats(info);
            CheckContainer();
        }

        static void LoadQualityLevel (CodecInfo info)
        {
            if (info.QDefault >= 0)
                form.encQualityBox.Text = info.QDefault.ToString();
            else
                form.encQualityBox.Text = "";
        }

        static void LoadPresets(CodecInfo info)
        {
            form.encPresetBox.Items.Clear();

            if (info.Presets != null)
                foreach (string p in info.Presets)
                    form.encPresetBox.Items.Add(p.ToTitleCase()); // Add every preset to the dropdown

            if (form.encPresetBox.Items.Count > 0)
                form.encPresetBox.SelectedIndex = info.PresetDef; // Select default preset
        }

        static void LoadColorFormats(CodecInfo info)
        {
            form.encColorsBox.Items.Clear();

            if (info.ColorFormats != null)
                foreach (string p in info.ColorFormats)
                    form.encColorsBox.Items.Add(p.ToUpper()); // Add every pix_fmt to the dropdown

            if (form.encColorsBox.Items.Count > 0)
                form.encColorsBox.SelectedIndex = info.ColorFormatDef; // Select default pix_fmt

            
        }

        public static void CheckContainer()
        {
            if (form.containerBox.SelectedIndex < 0)
                return;

            Codecs.VideoCodec vCodec = (Codecs.VideoCodec)form.encEncoderBox.SelectedIndex;
            Codecs.AudioCodec aCodec = (Codecs.AudioCodec)form.encAudioEnc.SelectedIndex;

            aCodec = Codecs.AudioCodec.Aac;

            Containers.Container c = (Containers.Container)form.containerBox.SelectedIndex;

            if (!(Containers.ContainerSupports(c, vCodec) && Containers.ContainerSupports(c, aCodec)))
            {
                Containers.Container supported = Containers.GetSupportedContainer(vCodec, aCodec);

                Logger.Log($"{c} doesn't support one of the selected codecs - Auto-selected {supported} instead.");

                for (int i = 0; i < form.containerBox.Items.Count; i++)
                    if (form.containerBox.Items[i].ToString().ToUpper() == supported.ToString().ToUpper())
                        form.containerBox.SelectedIndex = i;
            }

            Containers.Container current = (Containers.Container)form.containerBox.SelectedIndex;
            string path = Path.ChangeExtension(form.outputBox.Text.Trim(), current.ToString().ToLower());
            Program.mainForm.outputBox.Text = path;
        }

        public static Codecs.VideoCodec GetCurrentCodecV ()
        {
            return (Codecs.VideoCodec)form.encEncoderBox.SelectedIndex;
        }

        public static Codecs.AudioCodec GetCurrentCodecA()
        {
            return (Codecs.AudioCodec)form.encAudioEnc.SelectedIndex;
        }

        public static Dictionary<string, string> GetVideoArgs ()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("preset", form.encPresetBox.Text.ToLower());
            dict.Add("pixFmt", form.encColorsBox.Text.ToLower());
            return dict;
        }

        public static Dictionary<string, string> GetAudioArgs()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("bitrate", form.encAudioBr.Text.ToLower());
            //dict.Add("ac", form.encAudioCh.Text.ToLower());
            return dict;
        }

        private static string GetStreamArgs()
        {
            return "";
        }
    }
}
