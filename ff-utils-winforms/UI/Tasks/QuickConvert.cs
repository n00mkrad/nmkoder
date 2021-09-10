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
                form.encEncoderBox.Items.Add(Codecs.GetFriendlyName(c));

            ConfigParser.LoadComboxIndex(form.encEncoderBox);
            
            foreach (Containers.Container c in Containers.containers)
                form.containerBox.Items.Add(c.Extension.ToUpper());
            
            ConfigParser.LoadComboxIndex(form.containerBox);
        }

        public static async Task Run ()
        {

        }

        public static void EncoderSelected(int index)
        {
            //Codecs.Codec c = Codecs.vCodecs[index];

            form.encPresetBox.Items.Clear();

            
        }

        private static string GetStreamArgs()
        {
            return "";
        }
    }
}
