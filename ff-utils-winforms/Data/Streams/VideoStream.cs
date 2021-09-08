using System.Drawing;

namespace Nmkoder.Data.Streams
{
    class VideoStream : Stream
    {
        public string ColorSpace;
        public Size Resolution;
        public Size Sar;
        public Size Dar;
        public Fraction Rate;

        public VideoStream(string codec, string codecLong, string colorSpace, Size resolution, Size sar, Size dar, Fraction rate)
        {
            base.Type = StreamType.Video;
            Codec = codec;
            CodecLong = codecLong;
            ColorSpace = colorSpace;
            Resolution = resolution;
            Sar = sar;
            Dar = dar;
            Rate = rate;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Colors: {ColorSpace} - Size: {Resolution.Width}x{Resolution.Height} - FPS: {Rate}";
        }
    }
}
