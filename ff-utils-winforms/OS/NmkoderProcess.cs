using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.OS
{
    class NmkoderProcess
    {
        public Process Process { get; }
        public enum ProcessType { Primary, Secondary, Background } // Primary: Task (ffmpeg/av1an) - Secondary: Task-related (cropdetect etc) - Background: Thumbnails etc
        public ProcessType Type { get; }

        public NmkoderProcess (Process p, ProcessType type = ProcessType.Primary)
        {
            Process = p;
            Type = type;
        }
    }
}
