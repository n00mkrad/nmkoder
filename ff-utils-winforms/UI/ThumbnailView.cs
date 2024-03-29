﻿using Nmkoder.Data;
using Nmkoder.Extensions;
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

namespace Nmkoder.UI
{
    class ThumbnailView
    {
        private static long currHash;
        private static Dictionary<string, Image> currThumbs;
        private static int currThumbIndex;
        private static bool busy;

        private static readonly Image placeholderImg = Resources.baseline_image_white_48dp_4x_25pcAlphaPad;
        private static readonly Image loadingImg = Resources.loadingThumbsText;

        public static void ClearUi()
        {
            IoUtils.DeleteContentsOfDir(Paths.GetThumbsPath());
            Program.mainForm.ThumbnailBox.Image = placeholderImg;
            Program.mainForm.ThumbLabel.Text = "";
            busy = false;
        }

        public static void LoadUi()
        {
            IoUtils.DeleteContentsOfDir(Paths.GetThumbsPath());
            Program.mainForm.ThumbnailBox.Image = loadingImg;
            Program.mainForm.ThumbLabel.Text = "Loading Thumbnails...";
            busy = true;
        }

        public static async Task GenerateThumbs(string path)
        {
            LoadUi();
            Directory.CreateDirectory(Paths.GetThumbsPath());
            //string format = "jpg";
            int randThumbs = 4;

            try
            {
                if (!IoUtils.IsPathDirectory(path))     // If path is video - Extract frames
                {
                    string imgPath = Path.Combine(Paths.GetThumbsPath(), $"thumb0-s0.jpg");
                    await FfmpegExtract.ExtractSingleFrame(path, imgPath, 1, 360);
                    await LoadThumbnailsOnce();

                    int duration = (int)Math.Floor((float)(await FfmpegCommands.GetDurationMs(path)) / 1000);

                    if (duration > randThumbs)   // Only generate random thumbs if duration is long enough
                    {
                        await FfmpegExtract.ExtractThumbs(path, Paths.GetThumbsPath(), randThumbs * 2);
                        FileInfo[] thumbs = IoUtils.GetFileInfosSorted(Paths.GetThumbsPath(), false, $"*.*");

                        var smallerHalf = thumbs.Skip(1).OrderBy(f => f.Length).Take(randThumbs).ToList(); // Get smaller half of thumbs

                        foreach (FileInfo f in smallerHalf) // Delete smaller thumbs to only have high-information thumbs
                            f.Delete();
                    }
                }
                else     // Path is frame folder - Copy frames
                {
                    FileInfo[] frames = IoUtils.GetFileInfosSorted(path, false, "*.*");
                    frames[0].CopyTo(Path.Combine(Paths.GetThumbsPath(), $"thumb0{frames[0].Extension}"));
                    Random rnd = new Random();
                    List<FileInfo> picks = frames.Skip(1).OrderBy(x => rnd.Next()).Take(randThumbs * 2).ToList();
                    picks = picks.OrderBy(f => f.Length).Skip(randThumbs).ToList(); // Delete smaller half of thumbs

                    int idx = 1;

                    foreach (FileInfo pick in picks)
                    {
                        pick.CopyTo(Path.Combine(Paths.GetThumbsPath(), $"thumb{idx}{pick.Extension}"));
                        idx++;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"GetThumbnails Error: {e.Message}\n{e.StackTrace}", true);
            }

            RemoveInvalidImages();
            //await SlideshowLoop();

            if (IoUtils.GetAmountOfFiles(Paths.GetThumbsPath(), false, $"*.*p*") > 0)
                await LoadThumbnailsOnce();
            else
                Fail();
        }

        static void RemoveInvalidImages()
        {
            foreach (string imgFile in IoUtils.GetFilesSorted(Paths.GetThumbsPath(), false))
            {
                try { int x = IoUtils.GetImage(imgFile).Width; }
                catch { IoUtils.TryDeleteIfExists(imgFile); }
            }
        }

        static void Fail()
        {
            Program.mainForm.ThumbnailBox.Image = placeholderImg;
            Program.mainForm.ThumbLabel.Text = $"Failed to extract thumbnails.";
        }

        public static async Task LoadThumbnailsOnce(string format = "*")
        {
            try
            {
                Logger.Log($"LoadThumbnailsOnce({format})", true);
                string[] files = IoUtils.GetFilesSorted(Paths.GetThumbsPath(), false, $"*.{format}");
                Image[] thumbs = files.Select(x => IoUtils.GetImage(x)).Where(x => x != null).ToArray();
                string[] filenames = files.Select(x => Path.GetFileName(x)).ToArray();
                currThumbs = Enumerable.Range(0, filenames.Length).ToDictionary(idx => filenames[idx], idx => thumbs[idx]);
                currThumbIndex = currThumbs.Count > 1 ? 1 : 0;
                busy = false;
                Logger.Log($"Loaded {currThumbs.Count} thumbnail images", true);
                ShowThumb();
            }
            catch (Exception e)
            {
                Logger.Log($"LoadThumbnailsOnce Exception: {e.Message}\n{e.StackTrace}", true);
            }
        }

        public static void ThumbnailClick()
        {
            if (busy || Program.mainForm.ThumbnailBox.Image == placeholderImg || Program.mainForm.ThumbnailBox.Image == loadingImg)
                return;

            ShowThumb(true);
        }

        public static void ShowThumb(bool next = false)
        {
            if (currThumbs == null || currThumbs.Count < 1)
                return;

            Program.mainForm.ThumbnailBox.Enabled = true;

            if (next)
            {
                currThumbIndex++;

                if (currThumbIndex >= currThumbs.Count)
                    currThumbIndex = 0;
            }

            bool hasTime = currThumbs.ElementAt(currThumbIndex).Key.Contains("-s");

            if (hasTime)
            {
                int s = currThumbs.ElementAt(currThumbIndex).Key.Split("-s")[1].GetInt();
                string time = TimeSpan.FromSeconds(s).ToString(@"hh\:mm\:ss");
                Program.mainForm.ThumbLabel.Text = $"Showing Thumbnail {currThumbIndex + 1}/{currThumbs.Count} ({time}).{(currThumbs.Count > 1 ? $" Click for next thumbnail." : "")}";
            }
            else
            {
                Program.mainForm.ThumbLabel.Text = $"Showing Thumbnail {currThumbIndex + 1}/{currThumbs.Count}.{(currThumbs.Count > 1 ? $" Click for next thumbnail." : "")}";
            }

            Program.mainForm.ThumbnailBox.Image = currThumbs.ElementAt(currThumbIndex).Value;
        }

        public static async Task SlideshowLoop(int interval = 2)
        {
            Logger.Log($"Slideshow.RunFromPath - imgsDir = {Paths.GetThumbsPath()}", true);
            string inputFile = TrackList.current.File.SourcePath;
            long inputFileSize = new FileInfo(inputFile).Length;

            for (int i = 0; true; i++)
            {
                if (inputFile != TrackList.current.File.SourcePath || inputFileSize != new FileInfo(inputFile).Length)
                    return;

                string[] files = IoUtils.GetFilesSorted(Paths.GetThumbsPath(), false, "*.*p*");

                if (files.Length < 1)
                {
                    await Task.Delay(interval * 500);
                    continue;
                }

                if (i >= files.Length)
                    i = 0;

                long newHash = files.Select(x => new FileInfo(x).Length).Sum();

                if (newHash != currHash) // Only reload images if hash (sum of all thumb filesizes) mismatches
                {
                    try
                    {
                        if (currThumbs != null)
                            foreach (var kvp in currThumbs)
                                if (kvp.Value != null)
                                    kvp.Value.Dispose();

                        Image[] thumbs = files.Select(x => IoUtils.GetImage(x)).Where(x => x != null).ToArray();
                        string[] filenames = files.Select(x => Path.GetFileName(x)).ToArray();
                        currThumbs = Enumerable.Range(0, filenames.Length).ToDictionary(idx => filenames[idx], idx => thumbs[idx]);
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Slideshow Error - Failed to load extracted thumbs: {e.Message}", true);
                    }

                    currHash = newHash;
                }

                currThumbIndex = i;
                ShowThumb();
                await Task.Delay(interval * 1000);
            }
        }
    }
}
