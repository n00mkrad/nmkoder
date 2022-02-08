using Nmkoder.Extensions;
using Nmkoder.IO;
using Nmkoder.UI.Tasks;
using Nmkoder.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data.Ui
{
    public class Av1anFolderEntry
    {
        public Dictionary<string, string> jsonInfo { get; } = null;
        public DirectoryInfo DirInfo { get; }
        public FileInfo InputFile { get; }
        public FileInfo[] ChunkFiles { get; }
        public string InputFilename { get; } = "";
        public string TempFolderName { get; } = "";
        public string Args { get; } = "";
        public DateTime CreationDate { get; }
        public DateTime LastRunDate { get; }
        public TimeSpan TimeSinceCreation { get;  }
        public TimeSpan TimeSinceLastRun { get; }

        public Av1anFolderEntry(string path)
        {
            DirInfo = new DirectoryInfo(path);
            jsonInfo = Av1an.LoadJson(DirInfo.Name);
            ChunkFiles = IoUtils.GetFileInfosSorted(Path.Combine(path, "encode"));

            if (jsonInfo == null) return;

            InputFilename = (jsonInfo.ContainsKey("fileName") ? jsonInfo["fileName"] : DirInfo.Name).Trunc(35);

            if (jsonInfo.ContainsKey("filePath"))
                InputFile = File.Exists(jsonInfo["filePath"]) ? new FileInfo(jsonInfo["filePath"]) : null;

            CreationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (jsonInfo.ContainsKey("creationTimestamp"))
                CreationDate = CreationDate.AddMilliseconds(long.Parse(jsonInfo["creationTimestamp"]));

            LastRunDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (jsonInfo.ContainsKey("lastRunTimestamp"))
                LastRunDate = LastRunDate.AddMilliseconds(long.Parse(jsonInfo["lastRunTimestamp"]));

            if (jsonInfo.ContainsKey("tempFolderName"))
                TempFolderName = jsonInfo["tempFolderName"];

            if (jsonInfo.ContainsKey("args"))
                Args = jsonInfo["args"];

            TimeSinceCreation = DateTime.Now - CreationDate;
            TimeSinceLastRun = DateTime.Now - LastRunDate;
        }

        public override string ToString()
        {
            string created = "???";

            if (CreationDate != new DateTime(1970, 1, 1, 0, 0, 0, 0))
                created = $"{(TimeSinceCreation.TotalMinutes >= 120 ? $"{TimeSinceCreation.TotalHours.RoundToInt()}h" : $"{TimeSinceCreation.TotalMinutes.RoundToInt()}m")} ago";

            string lastRun = "???";

            if (LastRunDate != new DateTime(1970, 1, 1, 0, 0, 0, 0))
                lastRun = $"{(TimeSinceLastRun.TotalMinutes >= 120 ? $"{TimeSinceLastRun.TotalHours.RoundToInt()}h" : $"{TimeSinceLastRun.TotalMinutes.RoundToInt()}m")} ago";

            string chunks = $"{ChunkFiles.Length} Chunks - {FormatUtils.Bytes(ChunkFiles.Sum(x => x.Length))}";
            return $"{InputFilename} - {chunks} - Created: {created} - Last Run: {lastRun}";
        }
    }
}
