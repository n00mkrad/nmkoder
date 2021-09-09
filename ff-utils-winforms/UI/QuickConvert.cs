using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.IO;

namespace Nmkoder.UI
{
    class QuickConvert
    {
        public static void Init ()
        {
            Form1 form = Program.mainForm;

            foreach (Codecs.Codec c in Codecs.vCodecs)
                form.encEncoderBox.Items.Add(c.LongName);
            
            ConfigParser.LoadComboxIndex(form.encEncoderBox);
            
            foreach (Containers.Container c in Containers.containers)
                form.containerBox.Items.Add(c.Extension.ToUpper());
            
            ConfigParser.LoadComboxIndex(form.containerBox);
        }
    }
}
