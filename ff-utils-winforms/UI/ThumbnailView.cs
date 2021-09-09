using Nmkoder.Data;
using Nmkoder.IO;
using Nmkoder.Media;
using Nmkoder.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    class ThumbnailView
    {
        private static long currHash;
        private static Image[] currThumbs;

        public static void RemoveThumbs()
        {
            Program.mainForm.thumbnailBox.Image = Resources.loadingThumbsText;
            IoUtils.DeleteContentsOfDir(Paths.GetThumbsPath());
        }

        public static async Task SaveThumbnails(string path)
        {
            Directory.CreateDirectory(Paths.GetThumbsPath());
            int randThumbs = 4;

            try
            {
                if (!IoUtils.IsPathDirectory(path))     // If path is video - Extract frames
                {
                    string imgPath = Path.Combine(Paths.GetThumbsPath(), "thumb0.jpg");
                    await FfmpegExtract.ExtractSingleFrame(path, imgPath, 1, 360);

                    await FfmpegExtract.ExtractThumbs(path, Paths.GetThumbsPath(), randThumbs * 2);
                    FileInfo[] thumbs = IoUtils.GetFileInfosSorted(Paths.GetThumbsPath(), false, "*.*");

                    var smallerHalf = thumbs.Skip(1).OrderBy(f => f.Length).Take(randThumbs).ToList(); // Get smaller half of thumbs

                    foreach (FileInfo f in smallerHalf) // Delete smaller thumbs to only have high-information thumbs
                        f.Delete();
                }
                else     // Path is frame folder - Copy frames
                {
                    FileInfo[] frames = IoUtils.GetFileInfosSorted(path, false, "*.*");
                    Image img1 = IoUtils.GetImage(frames[0].FullName);
                    img1.Save(Path.Combine(Paths.GetThumbsPath(), $"thumb0.jpg"), ImageFormat.Jpeg);
                    Random rnd = new Random();
                    List<FileInfo> picks = frames.Skip(1).OrderBy(x => rnd.Next()).Take(randThumbs * 2).ToList();
                    Logger.Log(string.Join(", ", picks.Select(x => (x.Length / 1024).ToString())));
                    picks = picks.OrderBy(f => f.Length).Take(randThumbs).ToList(); // Delete smaller half of thumbs
                    Logger.Log(string.Join(", ", picks.Select(x => (x.Length / 1024).ToString())));

                    int idx = 1;

                    foreach (FileInfo pick in picks)
                    {
                        Logger.Log($"Saving thumb " + pick.Name);
                        IoUtils.GetImage(pick.FullName).Save(Path.Combine(Paths.GetThumbsPath(), $"thumb{idx}.jpg"), ImageFormat.Jpeg);
                        idx++;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("GetThumbnails Error: " + e.Message, true);
            }

            await ThumbnailView.SlideshowLoop(Program.mainForm.thumbnailBox, Paths.GetThumbsPath());
        }

        public static async Task SlideshowLoop (PictureBox box, string imgsDir, string ext = "jpg", int interval = 2)
        {
            Logger.Log($"Slideshow.RunFromPath - imgsDir = {imgsDir}", true);
            string inputFile = MediaInfo.current.File.FullName;
            long inputFileSize = new FileInfo(inputFile).Length;

            for (int i = 0; true; i++)
            {
                if (inputFile != MediaInfo.current.File.FullName || inputFileSize != new FileInfo(inputFile).Length)
                    return;

                string[] files = IoUtils.GetFilesSorted(imgsDir, false, $"*.{ext}");

                if (files.Length < 1)
                {
                    await Task.Delay(interval * 500);
                    continue;
                }

                if (i >= files.Length)
                    i = 0;

                long newHash = files.Select(x => new FileInfo(x).Length).Sum();

                if(newHash != currHash) // Only reload images if hash (sum of all thumb filesizes) mismatches
                {
                    try
                    {
                        if (currThumbs != null)
                            foreach (Image img in currThumbs)
                                if (img != null)
                                    img.Dispose();

                        currThumbs = files.Select(x => IoUtils.GetImage(x)).ToArray();
                    }
                    catch(Exception e)
                    {
                        Logger.Log($"Slideshow Error - Failed to load extracted thumbs: {e.Message}", true);
                    }

                    currHash = newHash;
                }

                box.Image = currThumbs[i];
                await Task.Delay(interval * 1000);
            }
        }
    }
}
