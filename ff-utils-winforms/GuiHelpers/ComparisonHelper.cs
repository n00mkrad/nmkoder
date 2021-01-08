using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ff_utils_winforms.GuiHelpers
{
    class ComparisonHelper
    {
        public static async Task CreateComparison (string[] files, bool vertical, int crf)
        {
            string[] sortedFiles = files.OrderBy(f => f).ToArray();

            try
            {
                await FFmpegCommands.CreateComparison(sortedFiles, vertical, crf, false);
            }
            catch (Exception e)
            {
                Program.Print("CreateComparison Error: " + e.Message);
            }
        }
    }
}
