using Nmkoder.Extensions;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    internal class UiData
    {

        public static string GetOutPath(bool includeExtension = true)
        {
            var f = Program.mainForm;
            string outPathText = "";
            string containerText = "";

            if (f.RunningTask == Main.RunTask.TaskType.Convert)
            {
                outPathText = f.FfmpegOutputBox.Text.Trim();
                containerText = f.ffmpegContainerBox.Text.Trim();
            }
            else if (f.RunningTask == Main.RunTask.TaskType.Av1an)
            {
                outPathText = f.av1anOutputPathBox.Text.Trim();
                containerText = f.av1anContainerBox.Text.Trim();
            }

            if (includeExtension && containerText.IsNotEmpty())
                outPathText = $"{outPathText}.{containerText.Lower()}";

            return outPathText;
        }
    }
}
