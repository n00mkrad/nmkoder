using System.Drawing;

namespace Nmkoder.Data.Streams
{
    public class VideoStream : Stream
    {
        public string PixelFormat;
        public int Kbits;
        public Size Resolution;
        public Size Sar;
        public Size Dar;
        public Fraction Rate;

        public VideoStream(string language, string title, string codec, string codecLong, string pixFmt, int kbits, Size resolution, Size sar, Size dar, Fraction rate)
        {
            base.Type = StreamType.Video;
            Codec = codec;
            CodecLong = codecLong;
            PixelFormat = pixFmt;
            Kbits = kbits;
            Resolution = resolution;
            Sar = sar;
            Dar = dar;
            Rate = rate;
            Language = language;
            Title = title;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Language: {Language} - Colors: {PixelFormat} - Size: {Resolution.Width}x{Resolution.Height} - FPS: {Rate}";
        }
    }
}
