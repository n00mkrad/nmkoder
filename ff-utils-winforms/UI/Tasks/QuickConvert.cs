using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;

namespace Nmkoder.UI.Tasks
{
    class QuickConvert
    {
        private static Form1 form;
        private static readonly string disableStr = "Disable (Strip)";
        private static readonly string copyStr = "Copy Stream Without Re-Encoding";

        public static void Init ()
        {
            form = Program.mainForm;

            //form.encEncoderBox.Items.Add(disableStr);
            //form.encEncoderBox.Items.Add(copyStr);

            //foreach (Codecs.Codec c in Codecs.vCodecs)
            //    form.encEncoderBox.Items.Add(c.LongName);

            foreach (Codecs.VideoCodec c in Enum.GetValues(typeof(Codecs.VideoCodec)))
                form.encEncoderBox.Items.Add(Codecs.GetCodecInfo(c).FriendlyName);
                

            ConfigParser.LoadComboxIndex(form.encEncoderBox);
            
            foreach (Containers.Container c in Containers.containers)
                form.containerBox.Items.Add(c.Extension.ToUpper());
            
            ConfigParser.LoadComboxIndex(form.containerBox);
        }

        public static async Task Run ()
        {

        }

        public static void VidEncoderSelected(int index)
        {
            Codecs.VideoCodec c = (Codecs.VideoCodec)index;
            CodecInfo info = Codecs.GetCodecInfo(c);

            if (info.QDefault >= 0)
                form.encQualityBox.Text = info.QDefault.ToString();
            else
                form.encQualityBox.Text = "";

            form.encPresetBox.Items.Clear();

            if (info.Presets != null)
                foreach(string p in info.Presets)
                    form.encPresetBox.Items.Add(p); // Add every preset to the dropdown

            if(form.encPresetBox.Items.Count > 0)
                form.encPresetBox.SelectedIndex = info.PresetDef; // Select default preset

            form.encColorsBox.Items.Clear();

            if (info.ColorFormats != null)
                foreach (string p in info.ColorFormats)
                    form.encColorsBox.Items.Add(p); // Add every pix_fmt to the dropdown

            if (form.encColorsBox.Items.Count > 0)
                form.encColorsBox.SelectedIndex = info.ColorFormatDef; // Select default pix_fmt
        }

        private static string GetStreamArgs()
        {
            return "";
        }
    }
}
