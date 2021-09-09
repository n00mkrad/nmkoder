namespace Nmkoder.Data.Streams
{
    class AudioStream : Stream
    {
        public string Language;
        public string Title;
        public int Kbits;
        public int SampleRate;
        public int Channels;
        public string Layout;

        public AudioStream(string language, string title, string codec, string codecLong, int kbits, int sampleRate, int channels, string layout)
        {
            base.Type = StreamType.Audio;
            Language = language;
            Title = title;
            Codec = codec;
            CodecLong = codecLong;
            Kbits = kbits;
            SampleRate = sampleRate;
            Channels = channels;
            Layout = layout;
        }

        public override string ToString()
        {
            string title = string.IsNullOrWhiteSpace(Title.Trim()) ? "None" : Title;
            return $"{base.ToString()} - Language: {Language} - Title: {title} - Kbits: {Kbits} - SampleRate: {SampleRate} - Channels: {Channels} - Layout: {Layout}";
        }
    }
}
