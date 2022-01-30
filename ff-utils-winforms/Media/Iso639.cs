using Microsoft.VisualBasic.FileIO;
using Nmkoder.Data;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class Iso639
    {
        public static List<IsoLanguage> languages = new List<IsoLanguage>();

        public class IsoLanguage
        {
            public string Family { get; set; }
            public string EnglishName { get; set; }
            public string NativeName { get; set; }
            public string[] IsoCodes { get; set; }
        }

        private static void Init()
        {
            if (languages == null || languages.Count == 0)
                LoadFromCsv();
        }

        private static void LoadFromCsv()
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
            Init();

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
            return $"{lang.EnglishName}{(includeNativeName ? $"/{lang.NativeName}" : "")}{(includeIsoCodes ? $" ({string.Join("/", lang.IsoCodes)})" : "")}";
        }
    }
}
