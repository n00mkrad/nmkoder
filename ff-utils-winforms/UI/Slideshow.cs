using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    class Slideshow
    {
        private static long currHash;
        private static Image[] currThumbs;

        public static async Task RunFromPath (PictureBox box, string imgsDir, string ext = "jpg", int interval = 2)
        {
            Logger.Log($"Slideshow.RunFromPath - imgsDir = {imgsDir}", true);
            string inputFile = MainView.currentFile.File.FullName;
            long inputFileSize = new FileInfo(inputFile).Length;

            for (int i = 0; true; i++)
            {
                if (inputFile != MainView.currentFile.File.FullName || inputFileSize != new FileInfo(inputFile).Length)
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
