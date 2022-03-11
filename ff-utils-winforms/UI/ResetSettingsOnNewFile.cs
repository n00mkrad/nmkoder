using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nmkoder.UI
{
    class ResetSettingsOnNewFile
    {
        public static bool ResetTrim { get; set; }
        public static bool ResetFpsResample { get; set; }
        public static bool ResetResize { get; set; }
        public static bool ResetCrop { get; set; }
        public static bool ResetCustomInArgs { get; set; }
        public static bool ResetCustomOutArgs { get; set; }
        public static bool ResetCustomFilters { get; set; }

        public static Dictionary<string, string> NiceNames
        {
            get
            {
                Dictionary<string, string> d = new Dictionary<string, string>();
                d.Add(nameof(ResetTrim), "Trim");
                d.Add(nameof(ResetFpsResample), "Frame Rate");
                d.Add(nameof(ResetResize), "Resize");
                d.Add(nameof(ResetCrop), "Crop");
                d.Add(nameof(ResetCustomInArgs), "Custom Input Args");
                d.Add(nameof(ResetCustomOutArgs), "Custom Output Args");
                d.Add(nameof(ResetCustomFilters), "Custom Filters");
                return d;
            }
        }

        public static void ResetAll(Label labelToSet = null)
        {
            ResetTrim = false;
            ResetFpsResample = false;
            ResetResize = false;
            ResetCrop = false;
            ResetCustomInArgs = false;
            ResetCustomOutArgs = false;
            ResetCustomFilters = false;

            if (labelToSet != null)
                labelToSet.Text = GetString();
        }

        public static string GetString()
        {
            List<string> list = new List<string>();

            if (ResetTrim) list.Add(NiceNames[nameof(ResetTrim)]);
            if (ResetFpsResample) list.Add(NiceNames[nameof(ResetFpsResample)]);
            if (ResetResize) list.Add(NiceNames[nameof(ResetResize)]);
            if (ResetCrop) list.Add(NiceNames[nameof(ResetCrop)]);
            if (ResetCustomInArgs) list.Add(NiceNames[nameof(ResetCustomInArgs)]);
            if (ResetCustomOutArgs) list.Add(NiceNames[nameof(ResetCustomOutArgs)]);
            if (ResetCustomFilters) list.Add(NiceNames[nameof(ResetCustomFilters)]);

            if (list.Count > 0)
                return string.Join(", ", list.Select(x => ShortenName(x)));
            else
                return "None";
        }

        public static string ShortenName(string s)
        {
            return s.Replace("Custom Input", "In").Replace("Custom Output", "Out").Replace("Custom Filters", "Filters").Replace("Frame Rate", "FPS");
        }

        public static void Save ()
        {
            List<string> list = new List<string>();

            var properties = typeof(ResetSettingsOnNewFile).GetProperties();

            foreach (var prop in properties)
            {
                if (!prop.Name.StartsWith("Reset"))
                    continue;

                list.Add($"{prop.Name}={(bool)prop.GetValue(null, null)}");
            }

            Config.Set(Config.Key.ResetSettingsList, string.Join(",", list));
        }

        public static void Load (Label labelToSet = null)
        {
            string data = Config.Get(Config.Key.ResetSettingsList);

            if (data == null || string.IsNullOrWhiteSpace(data))
                return;

            foreach (string prop in data.Split(','))
            {
                try
                {
                    string propName = prop.Split('=')[0];
                    bool propVal = bool.Parse(prop.Split('=')[1]);
                    typeof(ResetSettingsOnNewFile).GetProperty(propName).SetValue(null, propVal);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to set saved ResetSettingsOnNewFile property: {ex.Message}");
                }
            }

            if (labelToSet != null)
                labelToSet.Text = GetString();
        }
    }
}
