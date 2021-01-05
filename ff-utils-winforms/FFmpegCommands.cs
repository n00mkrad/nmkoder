using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ff_utils_winforms
{
    class FFmpegCommands
    {
        static string yuv420p = "-pix_fmt yuv420p";
        static string faststart = "-movflags +faststart";

        public static void VideoToFrames (string inputFile, bool hdr, bool delSrc)
        {
            string frameFolderPath = Path.ChangeExtension(inputFile, null) + "-frames";
            if (!Directory.Exists(frameFolderPath))
                Directory.CreateDirectory(frameFolderPath);
            string hdrStr = "";
            if (hdr) hdrStr = FFmpegStrings.hdrFilter;
            string args = "-i \"" + inputFile + "\" " + hdrStr + " \"" + frameFolderPath + "/%04d.png\"";
            FFmpeg.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static void ExtractSingleFrame(string inputFile, int frameNum, bool hdr, bool delSrc)
        {
            string hdrStr = "";
            if (hdr) hdrStr = FFmpegStrings.hdrFilter;
            string args = "-i \"" + inputFile + "\" " + hdrStr
                + " -vf \"select=eq(n\\," + frameNum + ")\" -vframes 1  \"" + inputFile + "-frame" + frameNum + ".png\"";
            FFmpeg.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static void FramesToMp4 (string inputDir, bool useH265, int crf, float fps, string prefix, bool delSrc)
        {
            string[] pngFrames = Directory.GetFiles(inputDir, "*.png");
            if(pngFrames.Length < 1)
            {
                MessageBox.Show("Can't find any PNG frames in this folder!", "Message");
                return;
            }
            int nums = IOUtils.GetFilenameCounterLength(pngFrames[0], prefix);
            string enc = useH265 ? "libx265" : "libx264";
            string fpsStr = fps.ToString().Replace(",", ".");
            string args = $"-r {fpsStr} -i \"{inputDir}\\{prefix}%{nums}d.png\" -c:v {enc} -crf {crf} {yuv420p} {faststart} -c:a copy \"{inputDir}.mp4\"";
            FFmpeg.Run(args);
            DeleteSource(inputDir, delSrc);
        }

        public static void FramesToMp4Concat(string concatFile, string outPath, bool useH265, int crf, float fps)
        {
            string vfrFilename = Path.GetFileName(concatFile);
            string enc = useH265 ? "libx265" : "libx264";
            string rate = fps.ToString().Replace(",", ".");
            string args = $"-loglevel error -vsync 0 -safe 0 -f concat -r {rate} -i {concatFile.Wrap()} -c:v {enc} -crf {crf} {yuv420p} {faststart} -c:a copy {outPath.Wrap()}";
            //string args = $"-r {fpsStr} -i \"{inputDir}\\{prefix}%{nums}d.png\" -c:v {enc} -crf {crf} {yuv420p} {faststart} -c:a copy \"{inputDir}.mp4\"";
            FFmpeg.Run(args);
        }

        public static void FramesToApng (string inputDir, bool opti, float fps, string prefix, bool delSrc)
        {
            int nums = IOUtils.GetFilenameCounterLength(Directory.GetFiles(inputDir, "*.png")[0], prefix);
            string filter = opti ? "-vf \"split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"" : "";
            string fpsStr = fps.ToString().Replace(",", ".");
            string args = $"-r {fpsStr} -i \"{inputDir}\\{prefix}%{nums}d.png\" -f apng -plays 0 {filter} \"{inputDir}.png\"";
            FFmpeg.Run(args);
            DeleteSource(inputDir, delSrc);
        }

        public static void FramesToGif (string inputDir, bool opti, float fps, string prefix, bool delSrc)
        {
            int nums = IOUtils.GetFilenameCounterLength(Directory.GetFiles(inputDir, "*.png")[0], prefix);
            string filter = opti ? "-vf \"split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"" : "";
            string fpsStr = fps.ToString().Replace(",", ".");
            string args = $"-r {fpsStr} -i \"{inputDir}\\{prefix}%{nums}d.png\" -f gif {filter} \"{inputDir}.gif\"";
            FFmpeg.Run(args);
            DeleteSource(inputDir, delSrc);
        }

        public static void LoopVideo (string inputFile, int times, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string args = "-stream_loop " + times + " -i \"" + inputFile + "\"  -c copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            FFmpeg.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static void LoopVideoEnc (string inputFile, int times, bool useH265, int crf, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string enc = "libx264";
            if (useH265) enc = "libx265";
            string args = "-stream_loop " + times + " -i \"" + inputFile +  "\"  -c:v " + enc + " -crf " + crf + " -pix_fmt yuv420p -movflags +faststart -c:a copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            FFmpeg.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static void ChangeSpeed(string inputFile, int newSpeedPercent, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            float val = newSpeedPercent / 100f;
            string speedVal = (1f / val).ToString("0.0000").Replace(",", ".");
            string args = "-itsscale " + speedVal + " -i \"" + inputFile + "\"  -c copy \"" + pathNoExt + "-" + newSpeedPercent + "pcSpeed" + ext + "\"";
            FFmpeg.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static void EncodeMux (string inputFile, string vcodec, string acodec, int crf, int audioKbps, bool delSrc)
        {
            string filenameNoExt = Path.ChangeExtension(inputFile, null);
            string outPath = filenameNoExt + "-convert.mp4";
            string audioArg = string.IsNullOrWhiteSpace(acodec) ? "-an" : $"-c:a {acodec} -b:a {audioKbps}";
            string args = $"-i {inputFile.Wrap()} -c:v {vcodec} -crf {crf} -pix_fmt yuv420p -movflags +faststart {audioArg} {outPath.Wrap()}";
            FFmpeg.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static void CreateComparison(string input1, string input2, bool vertical, string vcodec, int crf, bool delSrc)
        {
            string stackStr = vertical ? "\"vstack,format = yuv420p\"" : "\"hstack,format = yuv420p\"";
            string args = $"-i \"INPATH1\" -i \"INPATH2\" -filter_complex {stackStr} -vsync vfr -c:v VCODEC -crf CRF \"OUTPATH\"";
            args = args.Replace("VCODEC", vcodec);
            args = args.Replace("CRF", crf.ToString());
            string fname1 = Path.ChangeExtension(input1, null);
            string fname2 = Path.GetFileName(Path.ChangeExtension(input2, null));
            args = args.Replace("INPATH1", input1);
            args = args.Replace("INPATH2", input2);
            if(!vertical)
                args = args.Replace("OUTPATH", fname1 + "-" + fname2 + "-comparison.mp4");
            else
                args = args.Replace("OUTPATH", fname1 + "-" + fname2 + "-vcomparison.mp4");
            FFmpeg.Run(args);
            DeleteSource(input1, delSrc);
            DeleteSource(input2, delSrc);
        }

        public enum Track { Audio, Video }
        public static void Delay (string input, Track track, float delay, bool delSrc)
        {
            string suffix = (track == Track.Audio) ? "adelay" : "vdelay";
            string outPath = Path.ChangeExtension(input, null) + $"-{suffix}" + Path.GetExtension(input);
            string args = $"-i {input.Wrap()} ";
            if(track == Track.Audio)
                args += $"-itsoffset {delay.ToString().Replace(",", ".")} -i {input.Wrap()} -map 0:v -map 1:a -c copy {outPath.Wrap()}";
            if (track == Track.Video)
                args += $"-itsoffset {delay.ToString().Replace(",",".")} -i {input.Wrap()} -map 1:v -map 0:a -c copy {outPath.Wrap()}";
            FFmpeg.Run(args);
            DeleteSource(input, delSrc);
        }

        static void DeleteSource (string path, bool doDelete = true)
        {
            if (!doDelete) return;
            Program.Print("Deleting input file: " + path);
            IOUtils.TryDeleteIfExists(path);
        }
    }
}
