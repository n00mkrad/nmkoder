namespace Nmkoder.Data.Streams
{
    class AudioStream : Stream
    {
        public string Title;
        public int Kbits;
        public int SampleRate;
        public int Channels;
        public string Layout;

        public AudioStream(string title, string codec, string codecLong, int kbits, int sampleRate, int channels, string layout)
        {
            base.Type = StreamType.Audio;
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
            return $"{base.ToString()} - Title: {title} - Kbits: {Kbits} - SampleRate: {SampleRate} - Channels: {Channels} - Layout: {Layout}";
        }
    }
}
