using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CB = System.Windows.Forms.ComboBox;

namespace ff_utils_winforms
{
    class EncodeTabHelper
    {
        public static void Encode (string file, CB contCombox, CB vcodecCombox, TextBox fpsBox, CB acodecCombox, CB aMixCombox, CB vQualTbox, CB aQualTbox, CheckBox delSrc)
        {
            string vcodec = "";
            string acodec = "";
            int crf;
            int audioBitrate;
            int aChannels = -1;
            float fps = fpsBox.GetFloat();

            switch (vcodecCombox.SelectedIndex)
            {
                case 1: vcodec = "copy"; break;
                case 2: vcodec = "libx264"; break;
                case 3: vcodec = "libx265"; break;
                case 4: vcodec = "rav1e"; break;
            }

            switch (acodecCombox.SelectedIndex)
            {
                case 1: acodec = "copy"; break;
                case 2: acodec = "aac"; break;
                case 3: acodec = "ac3"; break;
                case 4: acodec = "libmp3lame"; break;
                case 5: acodec = "libvorbis"; break;
                case 6: acodec = "libopus"; break;
                case 7: acodec = "flac"; break;
            }

            switch (aMixCombox.SelectedIndex)
            {
                case 1: aChannels = 1; break;
                case 2: aChannels = 2; break;
                case 3: aChannels = 6; break;
                case 4: aChannels = 7; break;
            }

            crf = vQualTbox.GetInt();
            audioBitrate = aQualTbox.GetInt();

            string extension = Path.GetExtension(file).Replace(".", "");
            string filenameNoExt = Path.ChangeExtension(file, null);

            if (contCombox.SelectedIndex > 0)
                extension = contCombox.Text.Trim().Split(' ')[0].ToLower();

            string outPath = filenameNoExt + $"-convert.{extension}";
            FFmpegCommands.EncodeMux(file, outPath, vcodec, fps, acodec, aChannels, crf, audioBitrate, delSrc.Checked);
        }

        public static void VideoToGif (string file, CheckBox optimizeCbox, TextBox fpsBox)
        {
            string outPath = Path.ChangeExtension(file, "gif");
            FFmpegCommands.VideoToGif(file, outPath, optimizeCbox.Checked, fpsBox.GetFloat());
        }

        public static void VideoToApng(string file, CheckBox optimizeCbox, TextBox fpsBox)
        {
            string outPath = Path.ChangeExtension(file, "png");
            FFmpegCommands.VideoToApng(file, outPath, optimizeCbox.Checked, fpsBox.GetFloat());
        }
    }
}
