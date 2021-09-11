using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nmkoder.Data;
using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.Media;
using static Nmkoder.UI.Tasks.QuickConvertUi;

namespace Nmkoder.UI.Tasks
{
    class QuickConvert
    {
        public static void Init()
        {
            QuickConvertUi.Init();
        }

        public static async Task Run ()
        {
            string inPath = MediaInfo.current.File.FullName;
            string outPath = Program.mainForm.outputBox.Text.Trim();
            string map = MediaInfo.GetMapArgs();
            string video = Codecs.GetArgs(GetCurrentCodecV(), GetVideoArgsFromUi());
            string audio = Codecs.GetArgs(GetCurrentCodecA(), GetAudioArgsFromUi());
            string subs = Codecs.GetArgs(GetCurrentCodecS());
            string custom = Program.mainForm.customArgsBox.Text.Trim();

            string args = $"-i {inPath.Wrap()} {map} {video} {audio} {subs} {custom} {outPath.Wrap()}";
            Logger.Log(args);

            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.Visible, AvProcess.TaskType.Encode, false);
        }

        
    }
}
