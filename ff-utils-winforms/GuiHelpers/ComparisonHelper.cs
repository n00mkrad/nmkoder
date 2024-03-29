﻿using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.GuiHelpers
{
    class ComparisonHelper
    {
        public static async Task CreateComparison (string[] files, bool vertical, bool split, int crf)
        {
            if (files.Length < 2) return;
            string[] sortedFiles = files.OrderBy(f => f).ToArray();

            try
            {
                //await FFmpegCommands.CreateComparison(sortedFiles[0], sortedFiles[1], vertical, split, crf, false);
            }
            catch (Exception e)
            {
                Logger.Log("CreateComparison Error: " + e.Message);
            }
        }
    }
}
