using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class VideoStream
    {
        public int Index;
        public string Codec;
        public string ColorSpace;
        public Size Resolution;
        public Size Sar;
        public Size Dar;
        public Fraction Rate;
        public int Kbits;
    }
}
