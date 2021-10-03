using Nmkoder.Extensions;

namespace Nmkoder.Data.Streams
{
    public class SubtitleStream : Stream
    {
        public string Language;
        public string Title;
        public bool Bitmap;

        public SubtitleStream(string language, string title, string codec, string codecLong, bool bitmap)
        {
            base.Type = StreamType.Subtitle;
            Language = language;
            Title = title;
            Codec = codec;
            CodecLong = codecLong;
            Bitmap = bitmap;
        }

        public override string ToString()
        {
            string lang = string.IsNullOrWhiteSpace(Language.Trim()) ? "?" : Language;
            string ttl = string.IsNullOrWhiteSpace(Title.Trim()) ? "None" : Title;
            return $"{base.ToString()} - Language: {lang} - Title: {ttl} - Bitmap-based: {Bitmap.ToString().ToTitleCase()}";
        }
    }
}
