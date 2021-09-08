namespace Nmkoder.Data.Streams
{
    class Stream
    {
        public enum StreamType { Video, Audio, Subtitle, Data, Unknown }
        public StreamType Type;
        public int Index;
        public string Codec;
        public string CodecLong;

        public override string ToString()
        {
            return $"Stream #{Index} - {Codec} {Type}";
        }
    }
}
