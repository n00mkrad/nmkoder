using Nmkoder.Extensions;

namespace Nmkoder.UI
{
    internal class UiData
    {

        public static string GetOutPath(bool includeExtension = true)
        {
            var f = Program.mainForm;
            string outPathText = "";
            string containerText = "";

            if(f.FfmpegOutputBox.Visible) outPathText = f.FfmpegOutputBox.Text.Trim();
            if(f.av1anOutputPathBox.Visible) outPathText = f.av1anOutputPathBox.Text.Trim();

            if (f.ffmpegContainerBox.Visible) containerText = f.ffmpegContainerBox.Text.Trim();
            if (f.av1anContainerBox.Visible) containerText = f.av1anContainerBox.Text.Trim();

            if (includeExtension && containerText.IsNotEmpty())
                outPathText = $"{outPathText}.{containerText.Lower()}";

            return outPathText;
        }
    }
}
