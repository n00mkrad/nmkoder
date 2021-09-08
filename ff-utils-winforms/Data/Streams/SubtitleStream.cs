namespace Nmkoder.Data.Streams
{
    class SubtitleStream : Stream
    {
        public string Language;

        public SubtitleStream(string language, string codec, string codecLong)
        {
            base.Type = StreamType.Subtitle;
            Language = language;
            Codec = codec;
            CodecLong = codecLong;
        }

        public override string ToString()
        {
            string lang = string.IsNullOrWhiteSpace(Language.Trim()) ? "?" : Language;
            return $"{base.ToString()} - Language: {lang}";
        }
    }
}
