using System;
using System.Collections.Generic;

namespace Nmkoder.Data
{
    class CodecArgs
    {
        public string Arguments = "";
        public List<string> ForcedFilters = new List<string>();

        public CodecArgs()
        {

        }

        public CodecArgs(string arguments, string forcedFilter = "")
        {
            Arguments = arguments;
            ForcedFilters = new List<string> { forcedFilter };
        }

        public CodecArgs(string arguments, List<string> forcedFilters)
        {
            Arguments = arguments;
            ForcedFilters = forcedFilters;
        }
    }
}
