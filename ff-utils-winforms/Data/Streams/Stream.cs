namespace Nmkoder.Data.Streams
{
    public class Stream
    {
        public enum StreamType { Video, Audio, Subtitle, Data, Attachment, Unknown }
        public StreamType Type;
        public int Index;
        public string Codec;
        public string CodecLong;
        public string Language;
        public string Title;

        public override string ToString()
        {
            return $"Stream #{Index} - {Codec} {Type}";
        }
    }
}
