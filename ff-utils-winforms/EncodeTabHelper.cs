using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ff_utils_winforms
{
    class EncodeTabHelper
    {
        public static void Run (string file, ComboBox vcodecCombox, ComboBox acodecCombox, ComboBox vQualTbox, ComboBox aQualTbox, CheckBox delSrc)
        {
            string vcodec = "copy";
            string acodec = "";
            int crf;
            int audioBitrate;

            switch (vcodecCombox.SelectedIndex)
            {
                case 1: vcodec = "libx264"; break;
                case 2: vcodec = "libx265"; break;
                case 3: vcodec = "rav1e"; break;
            }

            switch (acodecCombox.SelectedIndex)
            {
                case 1: acodec = "aac"; break;
                case 2: acodec = "ac3"; break;
                case 3: acodec = "libmp3lame"; break;
                case 4: acodec = "libopus"; break;
                case 5: acodec = "flac"; break;
            }

            crf = vQualTbox.GetInt();
            audioBitrate = aQualTbox.GetInt();

            FFmpegCommands.EncodeMux(file, vcodec, acodec, crf, audioBitrate, delSrc.Checked);
        }
    }
}
