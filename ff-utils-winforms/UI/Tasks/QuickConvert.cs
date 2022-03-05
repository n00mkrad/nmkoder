using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Nmkoder.Data;
using Nmkoder.Data.Codecs;
using Nmkoder.Data.Ui;
using Nmkoder.Extensions;
using Nmkoder.Forms;
using Nmkoder.IO;
using Nmkoder.Main;
using Nmkoder.Media;
using Nmkoder.OS;
using static Nmkoder.UI.Tasks.QuickConvertUi;

namespace Nmkoder.UI.Tasks
{
    class QuickConvert
    {
        public enum QualityMode { Crf, TargetKbps, TargetMbytes }

        public static void Init()
        {
            QuickConvertUi.Init();
        }

        public static async Task Run()
        {
            Program.mainForm.SetWorking(true);
            SuspendResume.SetPauseButtonStyle(false);
            string args = "";

            try
            {
                IEncoder vCodec = CodecUtils.GetCodec(GetCurrentCodecV());
                CodecUtils.AudioCodec aCodec = GetCurrentCodecA();
                CodecUtils.SubtitleCodec sCodec = GetCurrentCodecS();
                bool anyVideoStreams = Program.mainForm.streamList.CheckedItems.Cast<ListViewItem>().Where(x => ((StreamListEntry)x.Tag).Stream.Type == Data.Streams.Stream.StreamType.Video).Count() > 0;
                bool anyAudioStreams = Program.mainForm.streamList.CheckedItems.Cast<ListViewItem>().Where(x => ((StreamListEntry)x.Tag).Stream.Type == Data.Streams.Stream.StreamType.Audio).Count() > 0;
                bool crf = (QualityMode)Program.mainForm.encQualModeBox.SelectedIndex == QualityMode.Crf;
                bool twoPass = anyVideoStreams && vCodec.SupportsTwoPass && (vCodec.ForceTwoPass || !crf);
                Dictionary<string, string> videoArgs = vCodec.DoesNotEncode ? new Dictionary<string, string>() : GetVideoArgsFromUi(!crf);

                string inFiles = TrackList.GetInputFilesString();
                string outPath = GetOutPath(vCodec);
                string map = TrackList.GetMapArgs(vCodec.IsFixedFormat);
                string a = anyAudioStreams ? CodecUtils.GetCodec(aCodec).GetArgs(GetAudioArgsFromUi(), TrackList.current.File).Arguments : "";
                string s = CodecUtils.GetCodec(sCodec).GetArgs().Arguments;
                string meta = GetMetadataArgs();
                string miscIn = GetMiscInputArgs();
                string miscOut = GetMiscOutputArgs();
                string custIn = Program.mainForm.customArgsInBox.Text.Trim();
                string custOut = Program.mainForm.customArgsOutBox.Text.Trim();
                string muxing = GetMuxingArgs();

                if (twoPass && anyVideoStreams)
                {
                    CodecArgs codecArgsPass1 = vCodec.GetArgs(videoArgs, TrackList.current.File, Pass.OneOfTwo);
                    string v1 = codecArgsPass1.Arguments;
                    string vf1 = vCodec.DoesNotEncode ? "" : await GetVideoFilterArgs(vCodec, codecArgsPass1);
                    CodecArgs codecArgsPass2 = vCodec.GetArgs(videoArgs, TrackList.current.File, Pass.TwoOfTwo);
                    string v2 = codecArgsPass2.Arguments;
                    string vf2 = vCodec.DoesNotEncode ? "" : await GetVideoFilterArgs(vCodec, codecArgsPass2);

                    args = $"{miscIn} {custIn} {inFiles} -map v {v1} {vf1} {miscOut} {custOut} -f null - && ffmpeg -y -loglevel warning -stats " +
                           $"{miscIn} {custIn} {inFiles} {map} {v2} {vf2} {a} {s} {meta} {miscOut} {custOut} {muxing} {outPath.Wrap()}";
                }
                else
                {
                    CodecArgs codecArgs = vCodec.GetArgs(videoArgs, TrackList.current.File, Pass.OneOfOne);
                    string v = anyVideoStreams ? codecArgs.Arguments : "";
                    string vf = anyVideoStreams && !vCodec.DoesNotEncode ? await GetVideoFilterArgs(vCodec, codecArgs) : "";

                    args = $"{miscIn} {custIn} {inFiles} {map} {v} {vf} {a} {s} {meta} {miscOut} {custOut} {muxing} {outPath.Wrap()}";
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Error creating FFmpeg command: {e.Message}\n{e.StackTrace}");
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Shift) // Allow reviewing and editing command if shift is held
            {
                EditCommandForm form = new EditCommandForm("ffmpeg", args);
                form.ShowDialog();

                if (string.IsNullOrWhiteSpace(form.Args))
                {
                    Program.mainForm.SetWorking(false);
                    return;
                }

                args = form.Args;
            }

            if (RunTask.canceled) return;

            Logger.Log($"Running:\nffmpeg {args}", true, false, "ffmpeg");

            AvProcess.FfmpegSettings settings = new AvProcess.FfmpegSettings() { Args = args, LoggingMode = AvProcess.LogMode.OnlyLastLine, ProgressBar = true };
            await AvProcess.RunFfmpeg(settings);
        }
    }
}
