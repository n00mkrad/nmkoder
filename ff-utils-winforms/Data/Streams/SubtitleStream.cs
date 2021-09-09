namespace Nmkoder.Data.Streams
{
    class SubtitleStream : Stream
    {
        public string Language;
        public string Title;

        public SubtitleStream(string language, string title, string codec, string codecLong)
        {
            base.Type = StreamType.Subtitle;
            Language = language;
            Title = title;
            Codec = codec;
            CodecLong = codecLong;
        }

        public override string ToString()
        {
            string lang = string.IsNullOrWhiteSpace(Language.Trim()) ? "?" : Language;
            string ttl = string.IsNullOrWhiteSpace(Title.Trim()) ? "None" : Title;
            return $"{base.ToString()} - Language: {lang} - Title: {ttl}";
        }
    }
}
