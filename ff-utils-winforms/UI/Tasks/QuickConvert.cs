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
            string video = Codecs.GetArgs(GetCurrentCodecV(), GetVideoArgs());
            string audio = Codecs.GetArgs(GetCurrentCodecA(), GetAudioArgs());
            string subs = Codecs.GetArgs(GetCurrentCodecS());
            string custom = Program.mainForm.customArgsBox.Text.Trim();

            string args = $"-i {inPath} {map} {video} {audio} {subs} {custom} {outPath}";
            Logger.Log(args);

            await AvProcess.RunFfmpeg(args, AvProcess.LogMode.Visible, AvProcess.TaskType.Encode, false);
        }

        
    }
}
