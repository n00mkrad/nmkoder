using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ff_utils_winforms
{
    class FFmpegCommands
    {
        public static void VideoToFrames (string inputFile, bool hdr, bool delSrc)
        {
            string frameFolderPath = Path.ChangeExtension(inputFile, null) + "-frames";
            if (!Directory.Exists(frameFolderPath))
                Directory.CreateDirectory(frameFolderPath);
            string hdrStr = "";
            if (hdr) hdrStr = FFmpegStrings.hdrFilter;
            string args = "-i \"" + inputFile + "\" " + hdrStr + " \"" + frameFolderPath + "/%04d.png\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void ExtractSingleFrame(string inputFile, int frameNum, bool hdr, bool delSrc)
        {
            string hdrStr = "";
            if (hdr) hdrStr = FFmpegStrings.hdrFilter;
            string args = "-i \"" + inputFile + "\" " + hdrStr
                + " -vf \"select=eq(n\\," + frameNum + ")\" -vframes 1  \"" + inputFile + "-frame" + frameNum + ".png\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void FramesToMp4 (string inputDir, bool useH265, int crf, int fps, string prefix, bool delSrc)
        {
            int nums = IOUtils.GetFilenameCounterLength(Directory.GetFiles(inputDir, "*.png")[0], prefix);
            string enc = "libx264";
            if (useH265) enc = "libx265";
            string args = "-framerate " + fps + " -i \"" + inputDir + "\\" + prefix + "%0" + nums + "d.png\" -c:v " + enc
                + " -crf " + crf + " -pix_fmt yuv420p -movflags +faststart -c:a copy \"" + inputDir + ".mp4\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputDir); // CHANGE CODE TO BE ABLE TO DELETE DIRECTORIES!!
        }

        public static void FramesToApng (string inputDir, bool opti, int fps, string prefix, bool delSrc)
        {
            int nums = IOUtils.GetFilenameCounterLength(Directory.GetFiles(inputDir, "*.png")[0], prefix);
            string filter = "";
            if(opti) filter = "-vf \"split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"";
            string args = "-framerate " + fps + " -i \"" + inputDir + "\\" + prefix + "%0" + nums + "d.png\" -f apng -plays 0 " + filter + " \"" + inputDir + "-anim.png\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputDir); // CHANGE CODE TO BE ABLE TO DELETE DIRECTORIES!!
        }

        public static void FramesToGif (string inputDir, bool opti, int fps, string prefix, bool delSrc)
        {
            int nums = IOUtils.GetFilenameCounterLength(Directory.GetFiles(inputDir, "*.png")[0], prefix);
            string filter = "";
            if (opti) filter = "-vf \"split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"";
            string args = "-framerate " + fps + " -i \"" + inputDir + "\\" + prefix + "%0" + nums + "d.png\" -f gif " + filter + " \"" + inputDir + ".gif\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputDir); // CHANGE CODE TO BE ABLE TO DELETE DIRECTORIES!!
        }

        public static void LoopVideo (string inputFile, int times, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string args = " -stream_loop " + times + " -i \"" + inputFile + "\"  -c copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void LoopVideoEnc (string inputFile, int times, bool useH265, int crf, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string enc = "libx264";
            if (useH265) enc = "libx265";
            string args = " -stream_loop " + times + " -i \"" + inputFile +  "\"  -c:v " + enc + " -crf " + crf + " -pix_fmt yuv420p -movflags +faststart -c:a copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void ChangeSpeed(string inputFile, int newSpeedPercent, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            float val = newSpeedPercent / 100f;
            string speedVal = (1f / val).ToString("0.0000").Replace(",", ".");
            string args = " -itsscale " + speedVal + " -i \"" + inputFile + "\"  -c copy \"" + pathNoExt + "-" + newSpeedPercent + "pcSpeed" + ext + "\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void EncodeMux (string inputFile, string vcodec, string acodec, int crf, int audioKbps, bool delSrc)
        {
            string args = " -i \"INPATH\" -c:v VCODEC -crf CRF -pix_fmt yuv420p -movflags +faststart -c:a ACODEC -b:a ABITRATE \"OUTPATH\"";
            if (string.IsNullOrWhiteSpace(acodec))
                args = args.Replace("-c:a", "-an");
            args = args.Replace("VCODEC", vcodec);
            args = args.Replace("ACODEC", acodec);
            args = args.Replace("CRF", crf.ToString());
            args = args.Replace("ABITRATE", audioKbps.ToString());
            args = args.Replace("INPATH", inputFile);
            string filenameNoExt = Path.ChangeExtension(inputFile, null);
            args = args.Replace("OUTPATH", filenameNoExt + "-convert.mp4");
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void CreateComparison(string input1, string input2, bool vertical, string vcodec, int crf, bool delSrc)
        {
            string args = " -i \"INPATH1\" -i \"INPATH2\" -filter_complex \"hstack,format = yuv420p\" -vsync vfr -c:v VCODEC -crf CRF -movflags +faststart -an \"OUTPATH\"";
            if(vertical)
                args = args.Replace("hstack", "vstack");
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
            if (delSrc)
            {
                DeleteSource(input1);
                DeleteSource(input2);
            }
        }

        static void DeleteSource (string path)
        {
            Program.Print("Deleting input file: " + path);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
