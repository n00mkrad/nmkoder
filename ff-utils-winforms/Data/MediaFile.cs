using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class MediaFile
    {
        public FileInfo File;
        public string Ext;
        public List<VideoStream> VideoStreams = new List<VideoStream>();
        public List<AudioStream> AudioStreams = new List<AudioStream>();

        public MediaFile (string path)
        {
            File = new FileInfo(path);
            Ext = File.Extension.Split('.').Last();
        }
    }
}
