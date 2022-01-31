using System.Drawing;

namespace Nmkoder.Data.Streams
{
    public class VideoStream : Stream
    {
        public string PixelFormat { get; }
        public int Kbits { get; }
        public Size Resolution { get; }
        public Size Sar { get; }
        public Size Dar { get; }
        public Fraction Rate { get; }

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
            return $"{base.ToString()} - Language: {Language} - Color Format: {PixelFormat} - Size: {Resolution.Width}x{Resolution.Height} - FPS: {Rate}";
        }
    }
}
