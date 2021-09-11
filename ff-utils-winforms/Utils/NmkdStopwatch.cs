using System;
using System.Diagnostics;
using System.Linq;

namespace Nmkoder.Utils
{
    class NmkdStopwatch
    {
        public Stopwatch sw = new Stopwatch(); 

        public NmkdStopwatch (bool startOnCreation = true)
        {
            if (startOnCreation)
                sw.Restart();
        }

        public long GetElapsedMs ()
        {
            return sw.ElapsedMilliseconds;
        }

        public override string ToString()
        {
            return FormatUtils.TimeSw(sw);
        }
    }
}
