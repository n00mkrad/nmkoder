using Microsoft.VisualBasic.FileIO;
using Nmkoder.Data;
using Nmkoder.IO;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class Aliases
    {
        #region ISO-639 Languages

        public static List<IsoLanguage> languages = new List<IsoLanguage>();

        public class IsoLanguage
        {
            public string Family { get; set; }
            public string EnglishName { get; set; }
            public string NativeName { get; set; }
            public string[] IsoCodes { get; set; }
        }

        private static void LoadLangsIfNotLoaded()
        {
            if (languages == null || languages.Count == 0)
                LoadLangsFromCsv();
        }

        private static void LoadLangsFromCsv()
        {
            string csvPath = Path.Combine(Paths.GetBinPath(), "iso639.csv");
            languages.Clear();

            try
            {
                TextFieldParser csvParser = new TextFieldParser(csvPath) { CommentTokens = new string[] { "#" }, Delimiters = new string[] { "," }, HasFieldsEnclosedInQuotes = true };
                csvParser.ReadLine(); // Skip header row

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    string[] codes = new string[] { fields[3], fields[4], fields[5] }.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(); // Strip empty code fields
                    IsoLanguage lang = new IsoLanguage() { Family = fields[0], EnglishName = fields[1], NativeName = fields[2], IsoCodes = codes };
                    languages.Add(lang);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error loading language list: {ex.Message}");
                Logger.Log($"Stack Trace: {ex.StackTrace}", true);
            }
        }

        public static IsoLanguage GetLanguage(string isoCode)
        {
            LoadLangsIfNotLoaded();

            foreach (IsoLanguage lang in languages)
                if (lang.IsoCodes.Contains(isoCode))
                    return lang;

            return new IsoLanguage() { Family = "Unknown", EnglishName = "Unknown", NativeName = "Unknown", IsoCodes = new string[] { isoCode } };
        }

        public static string GetLanguageString(string isoCode, bool includeNativeName = true, bool includeIsoCodes = true)
        {
            return GetLanguageString(GetLanguage(isoCode), includeNativeName, includeIsoCodes);
        }

        public static string GetLanguageString(IsoLanguage lang, bool includeNativeName = true, bool includeIsoCodes = true)
        {
            return $"{lang.EnglishName}{(includeNativeName && lang.NativeName != lang.EnglishName ? $"/{lang.NativeName}" : "")}{(includeIsoCodes ? $" ({string.Join("/", lang.IsoCodes)})" : "")}";
        }

        #endregion

        #region Codec Names

        public static string GetNicerCodecName (string codecName)
        {
            string lower = codecName.ToLower();

            if (lower.StartsWith("hdmv_pgs")) return "PGS";
            if (lower.StartsWith("subrip")) return "SRT";
            if (lower == "truehd") return "TrueHD";
            if (lower == "opus") return "Opus";
            if (lower.StartsWith("pcm")) return codecName.ToUpper();
            if (lower == "vc1") return "VC-1";
            if (lower == "mjpeg") return "MJPEG";
            if (lower == "mpeg4") return "MPEG-4";
            if (lower == "msmpeg4v3") return "MS MPEG-4 V3";
            if (lower == "timed_id3") return "Timed ID3";
            if (lower == "rawvideo") return "Raw Video";
            if (lower == "msrle") return "MS RLE";
            if (lower == "wmav2") return "WMAV2";
            if (lower == "wmapro") return "WMA Pro";
            if (lower == "text") return "Text";

            return FormatUtils.CapsIfShort(codecName, 5);
        }

        #endregion
    }
}
