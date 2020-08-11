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
                + " -crf " + crf + " -c:a copy \"" + inputDir + ".mp4\"";
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
            string args = " -stream_loop " + times + " -i \"" + inputFile +  "\"  -c:v " + enc + " -crf " + crf + " -c:a copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        public static void EncodeMux (string inputFile, string vcodec, string acodec, int crf, int audioKbps, bool delSrc)
        {
            string args = " -i \"INPATH\" -c:v VCODEC -crf CRF -c:a ACODEC -b:a ABITRATE";
            if (string.IsNullOrWhiteSpace(acodec))
                args = args.Replace("-c:a", "-an");
            args = args.Replace("INPATH", inputFile);
            args = args.Replace("VCODEC", vcodec);
            args = args.Replace("ACODEC", acodec);
            args = args.Replace("CRF", crf.ToString());
            args = args.Replace("ABITRATE", audioKbps.ToString());
            FFmpeg.Run(args);
            if (delSrc)
                DeleteSource(inputFile);
        }

        static void DeleteSource (string path)
        {
            Program.Print("Deleting input file: " + path);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
