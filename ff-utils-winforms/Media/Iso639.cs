using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Media
{
    class Iso639
    {
        public static string GetLanguageName (string code)
        {
            if (code == "en" || code == "eng") return "English";
            if (code == "es" || code == "spa") return "Spanish";
            if (code == "fr" || code == "fra" || code == "fre") return "French";
            if (code == "de" || code == "ger" || code == "deu") return "German";
            if (code == "it" || code == "ita") return "Italian";
            if (code == "pt" || code == "por") return "Portuguese";
            if (code == "pl" || code == "pol") return "Polish";
            if (code == "tr" || code == "tur") return "Turkish";
            if (code == "sv" || code == "swe") return "Swedish";
            if (code == "fi" || code == "fin") return "Finnish";
            if (code == "nl" || code == "dut" || code == "nld") return "Dutch";
            if (code == "nb" || code == "nob") return "Norwegian Bokmål";
            if (code == "no" || code == "nor") return "Norwegian";
            if (code == "ru" || code == "rus") return "Russian";
            if (code == "he" || code == "heb") return "Hebrew";
            if (code == "ar" || code == "ara") return "Arabic";
            if (code == "hi" || code == "hin") return "Hindi";
            if (code == "ta" || code == "tam") return "Tamil";
            if (code == "te" || code == "tel") return "Telugu";
            if (code == "id" || code == "ind") return "Indonesian";
            if (code == "ms" || code == "msa" || code == "may") return "Malay";
            if (code == "fil") return "Filipino";
            if (code == "th" || code == "tha") return "Thai";
            if (code == "ko" || code == "kor") return "Korean";
            if (code == "zh" || code == "zho" || code == "chi") return "Chinese";
            if (code == "ja" || code == "jpn") return "Japanese";
            if (code == "da" || code == "dan") return "Danish";
            if (code == "cs" || code == "cze" || code == "ces") return "Czech";
            if (code == "et" || code == "est") return "Estonian";
            if (code == "hu" || code == "hun") return "Hungarian";
            if (code == "is" || code == "ice" || code == "isl") return "Icelandic";
            if (code == "lv" || code == "lav") return "Latvian";
            if (code == "lt" || code == "lit") return "Lithuanian";
            if (code == "rp" || code == "ron" || code == "rum") return "Romanian";
            if (code == "sr" || code == "srp") return "Serbian";
            if (code == "sk" || code == "slk" || code == "slo") return "Slovak";
            if (code == "sl" || code == "slv") return "Slovenian";
            if (code == "so" || code == "som") return "Somali";
            if (code == "af" || code == "aar") return "Afrikaans";
            if (code == "hr" || code == "hrv") return "Croatian";
            if (code == "el" || code == "ell" || code == "gre") return "Greek";

            return code.ToUpper();
        }
    }
}
