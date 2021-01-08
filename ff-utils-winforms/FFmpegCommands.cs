using ff_utils_winforms.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        static string divBy2 = "\"crop = trunc(iw / 2) * 2:trunc(ih / 2) * 2\"";

        public static async Task VideoToFrames (string inputFile, bool hdr, bool delSrc)
        {
            string frameFolderPath = Path.ChangeExtension(inputFile, null) + "-frames";
            if (!Directory.Exists(frameFolderPath))
                Directory.CreateDirectory(frameFolderPath);
            string hdrStr = "";
            if (hdr) hdrStr = FFmpegStrings.hdrFilter;
            string args = "-i \"" + inputFile + "\" " + hdrStr + " \"" + frameFolderPath + "/%04d.png\"";
            await AvProcess.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static async Task ExtractSingleFrame(string inputFile, int frameNum, bool hdr, bool delSrc)
        {
            string hdrStr = "";
            if (hdr) hdrStr = FFmpegStrings.hdrFilter;
            string args = "-i \"" + inputFile + "\" " + hdrStr
                + " -vf \"select=eq(n\\," + frameNum + ")\" -vframes 1  \"" + inputFile + "-frame" + frameNum + ".png\"";
            await AvProcess.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static async Task FramesToMp4 (string inputDir, bool useH265, int crf, float fps, string prefix, bool delSrc)
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
            string args = $"-r {fpsStr} -i \"{inputDir}\\{prefix}%{nums}d.png\" -c:v {enc} -crf {crf} -vf {divBy2} {yuv420p} {faststart} -c:a copy \"{inputDir}.mp4\"";
            await AvProcess.Run(args);
            DeleteSource(inputDir, delSrc);
        }

        public static async Task FramesToMp4Concat(string concatFile, string outPath, bool useH265, int crf, float fps)
        {
            string enc = useH265 ? "libx265" : "libx264";
            string rate = fps.ToStringDot();
            string args = $"-loglevel error -vsync 0 -safe 0 -f concat -r {rate} -i {concatFile.Wrap()} -c:v {enc} -crf {crf} -vf {divBy2} {yuv420p} {faststart} -c:a copy {outPath.Wrap()}";
            await AvProcess.Run(args);
        }

        public static async Task FramesToApngConcat (string concatFile, string outPath, bool palette, float fps)
        {
            string paletteFilter = palette ? "-vf \"split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"" : "";
            string rate = fps.ToStringDot();
            string args = $"-loglevel error -vsync 0 -safe 0 -f concat -r {rate} -i {concatFile.Wrap()} -f apng -plays 0 {paletteFilter} {outPath.Wrap()}";
            await AvProcess.Run(args);
        }

        public static async Task VideoToApng(string inputFile, string outPath, bool palette, float resampleFps)
        {
            string paletteFilter = palette ? "\"split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\"" : "";
            string fpsFilter = (resampleFps <= 0) ? "" : $"\"fps=fps={resampleFps.ToStringDot()}\"";
            string filters = FormatUtils.ConcatStrings(new string[] { paletteFilter, fpsFilter });
            string args = $"-i {inputFile.Wrap()} -f gif {((filters.Length > 2) ? $"-vf {filters}" : "")} {outPath.Wrap()}";
            await AvProcess.Run(args);
        }

        public static async Task FramesToGifConcat(string concatFile, string outPath, bool palette, float fps, int colors = 128)
        {
            string paletteFilter = palette ? $"-vf \"split[s0][s1];[s0]palettegen={colors}[p];[s1][p]paletteuse=dither=floyd_steinberg:diff_mode=rectangle\"" : "";
            string rate = fps.ToStringDot();
            string args = $"-loglevel error -vsync 0 -safe 0 -f concat -r {rate} -i {concatFile.Wrap()} -f gif {paletteFilter} {outPath.Wrap()}";
            await AvProcess.Run(args);
        }

        public static async Task VideoToGif(string inputFile, string outPath, bool palette, float resampleFps, int colors = 128)
        {
            string paletteFilter = palette ? $"\"split[s0][s1];[s0]palettegen={colors}[p];[s1][p]paletteuse=dither=floyd_steinberg:diff_mode=rectangle\"" : "";
            string fpsFilter = (resampleFps <= 0) ? "" : $"\"fps=fps={resampleFps.ToStringDot()}\"";
            string filters = FormatUtils.ConcatStrings(new string[] { paletteFilter, fpsFilter });
            string args = $"-i {inputFile.Wrap()} -f gif {((filters.Length > 2) ? $"-vf {filters}" : "")} {outPath.Wrap()}";
            await AvProcess.Run(args);
        }

        public static async Task LoopVideo (string inputFile, int times, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string args = "-stream_loop " + times + " -i \"" + inputFile + "\"  -c copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            await AvProcess.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static async Task LoopVideoEnc (string inputFile, int times, bool useH265, int crf, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            string enc = "libx264";
            if (useH265) enc = "libx265";
            string args = "-stream_loop " + times + " -i \"" + inputFile +  "\"  -c:v " + enc + " -crf " + crf + " -pix_fmt yuv420p -movflags +faststart -c:a copy \"" + pathNoExt + "-" + times + "xLoop" + ext + "\"";
            await AvProcess.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static async Task ChangeSpeed(string inputFile, int newSpeedPercent, bool delSrc)
        {
            string pathNoExt = Path.ChangeExtension(inputFile, null);
            string ext = Path.GetExtension(inputFile);
            float val = newSpeedPercent / 100f;
            string speedVal = (1f / val).ToString("0.0000").Replace(",", ".");
            string args = "-itsscale " + speedVal + " -i \"" + inputFile + "\"  -c copy \"" + pathNoExt + "-" + newSpeedPercent + "pcSpeed" + ext + "\"";
            await AvProcess.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static async Task EncodeMux (string inputFile, string outPath, string vCodec, float fps, string aCodec, int aCh, int crf, int audioKbps, bool delSrc)
        {
            string videoArg = string.IsNullOrWhiteSpace(vCodec) ? "-vn" : $"-c:v {vCodec}";
            string fpsArg = (fps > 0) ? $"\"fps=fps={fps.ToStringDot()}\"" : "";
            string filters = FormatUtils.ConcatStrings(new string[] { divBy2, fpsArg });
            if (vCodec.Length > 1 && vCodec != "copy") videoArg += $" -crf {crf} -vf {filters} -pix_fmt yuv420p -movflags +faststart";

            string audioArg = string.IsNullOrWhiteSpace(aCodec) ? "-an" : $"-c:a {aCodec}";
            string aMixArg = (aCh < 1) ? "" : $"-ac {aCh}";
            if (aCodec.Length > 1 && aCodec != "copy") audioArg += $" -b:a {audioKbps}k {aMixArg}";

            string args = $"-i {inputFile.Wrap()} {videoArg} {audioArg} {outPath.Wrap()}";
            await AvProcess.Run(args);
            DeleteSource(inputFile, delSrc);
        }

        public static async Task CreateComparison(string[] inputs, bool vertical, int crf, bool delSrc)
        {
            if (inputs.Length < 2) return;
            string stackStr = vertical ? "\"vstack=shortest=1\"" : "\"hstack=shortest=1\"";
            Size res1 = GetSize(inputs[0]);
            Size res2 = GetSize(inputs[1]);
            int resW = res1.Width.RoundDiv2();
            int resH = res1.Height.RoundDiv2();
            if ((res2.Width * res2.Height) > (res1.Width * res1.Height))
            {
                resW = res2.Width.RoundDiv2();
                resH = res2.Height.RoundDiv2();
            }
            string filter = $"\"[0:v]scale={resW}:{resH}[before];[1:v]scale={resW}:{resH}[after];[before][after]{stackStr}[v]\" -map \"[v]\"";
            string fname1 = Path.ChangeExtension(inputs.First(), null);
            string outpath = $"{fname1}-comparison-{(vertical ? "v" : "h")}.mp4";
            string args = " ";
            foreach (string input in inputs)
                args += $"-i {input.Wrap()} ";
            args += $"-filter_complex {filter} -vsync 2 -c:v libx264 -crf {crf} {yuv420p} {faststart} {outpath.Wrap()}";
            await AvProcess.Run(args);
            foreach (string input in inputs)
                DeleteSource(input, delSrc);
        }

        public enum Track { Audio, Video }
        public static async Task Delay (string input, Track track, float delay, bool delSrc)
        {
            string suffix = (track == Track.Audio) ? "adelay" : "vdelay";
            string outPath = Path.ChangeExtension(input, null) + $"-{suffix}" + Path.GetExtension(input);
            string args = $"-i {input.Wrap()} ";
            if(track == Track.Audio)
                args += $"-itsoffset {delay.ToString().Replace(",", ".")} -i {input.Wrap()} -map 0:v -map 1:a -c copy {outPath.Wrap()}";
            if (track == Track.Video)
                args += $"-itsoffset {delay.ToString().Replace(",",".")} -i {input.Wrap()} -map 1:v -map 0:a -c copy {outPath.Wrap()}";
            await AvProcess.Run(args);
            DeleteSource(input, delSrc);
        }

        static void DeleteSource (string path, bool doDelete = true)
        {
            if (!doDelete) return;
            Program.Print("Deleting input file: " + path);
            IOUtils.TryDeleteIfExists(path);
        }

        public static float GetFramerate(string inputFile)
        {
            //Logger.Log("Reading FPS using ffmpeg.", true, false, "ffmpeg");
            string args = $" -i {inputFile.Wrap()}";
            string output = AvProcess.GetFfmpegOutput(args);
            string[] entries = output.Split(',');
            foreach (string entry in entries)
            {
                if (entry.Contains(" fps") && !entry.Contains("Input "))    // Avoid reading FPS from the filename, in case filename contains "fps"
                {
                    //Logger.Log("[FFCmds] FPS Entry: " + entry, true);
                    string num = entry.Replace(" fps", "").Trim().Replace(",", ".");
                    float value;
                    float.TryParse(num, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                    return value;
                }
            }
            return 0f;
        }

        public static Size GetSize(string inputFile)
        {
            string args = $" -v error -select_streams v:0 -show_entries stream=width,height -of csv=s=x:p=0 {inputFile.Wrap()}";
            string output = AvProcess.GetFfprobeOutput(args);

            if (output.Length > 4 && output.Contains("x"))
            {
                string[] numbers = output.Split('x');
                return new Size(numbers[0].GetInt(), numbers[1].GetInt());
            }
            return new Size(0, 0);
        }

        public static int GetFrameCount(string inputFile)
        {
            int frames = 0;

            // TODO: Implement Config
            //Logger.Log("Reading frame count using ffprobe.", true, false, "ffmpeg");
            frames = ReadFrameCountFfprobe(inputFile, false /* Config.GetBool("ffprobeCountFrames") */);      // Try reading frame count with ffprobe
            if (frames > 0)
                return frames;

            //Logger.Log($"Failed to get frame count using ffprobe (frames = {frames}). Reading frame count using ffmpeg.", true, false, "ffmpeg");
            frames = ReadFrameCountFfmpeg(inputFile);       // Try reading frame count with ffmpeg
            if (frames > 0)
                return frames;

            //Logger.Log("Failed to get total frame count of video.");
            return 0;
        }

        static int ReadFrameCountFfprobe(string inputFile, bool readFramesSlow)
        {
            string args = $" -v panic -select_streams v:0 -show_entries stream=nb_frames -of default=noprint_wrappers=1 {inputFile.Wrap()}";
            if (readFramesSlow)
            {
                //Logger.Log("Counting total frames using FFprobe. This can take a moment...");
                args = $" -v panic -count_frames -select_streams v:0 -show_entries stream=nb_read_frames -of default=nokey=1:noprint_wrappers=1 {inputFile.Wrap()}";
            }
            string info = AvProcess.GetFfprobeOutput(args);
            string[] entries = info.SplitIntoLines();
            try
            {
                if (readFramesSlow)
                    return info.GetInt();
                foreach (string entry in entries)
                {
                    if (entry.Contains("nb_frames="))
                        return entry.GetInt();
                }
            }
            catch { }
            return -1;
        }

        static async Task<int> ReadFrameCountFfprobeAsync(string inputFile, bool readFramesSlow)
        {
            string args = $" -v panic -select_streams v:0 -show_entries stream=nb_frames -of default=noprint_wrappers=1 {inputFile.Wrap()}";
            if (readFramesSlow)
            {
                //Logger.Log("Counting total frames using FFprobe. This can take a moment...");
                await Task.Delay(10);
                args = $" -v panic -count_frames -select_streams v:0 -show_entries stream=nb_read_frames -of default=nokey=1:noprint_wrappers=1 {inputFile.Wrap()}";
            }
            string info = AvProcess.GetFfprobeOutput(args);
            string[] entries = info.SplitIntoLines();
            try
            {
                if (readFramesSlow)
                    return info.GetInt();
                foreach (string entry in entries)
                {
                    if (entry.Contains("nb_frames="))
                        return entry.GetInt();
                }
            }
            catch { }
            return -1;
        }

        static int ReadFrameCountFfmpeg(string inputFile)
        {
            string args = $" -loglevel panic -i {inputFile.Wrap()} -map 0:v:0 -c copy -f null - ";
            string info = AvProcess.GetFfmpegOutput(args);
            string[] entries = info.SplitIntoLines();
            foreach (string entry in entries)
            {
                if (entry.Contains("frame="))
                    return entry.Substring(0, entry.IndexOf("fps")).GetInt();
            }
            return -1;
        }

        static async Task<int> ReadFrameCountFfmpegAsync(string inputFile)
        {
            string args = $" -loglevel panic -i {inputFile.Wrap()} -map 0:v:0 -c copy -f null - ";
            string info = await AvProcess.GetFfmpegOutputAsync(args, true);
            try
            {
                string[] lines = info.SplitIntoLines();
                string lastLine = lines.Last();
                return lastLine.Substring(0, lastLine.IndexOf("fps")).GetInt();
            }
            catch
            {
                return -1;
            }
        }
    }
}
