namespace Nmkoder.Data.Streams
{
    class DataStream : Stream
    {
        //public string Language;

        public DataStream(string codec, string codecLong)
        {
            base.Type = StreamType.Data;
            Codec = codec;
            CodecLong = Codec;
        }

        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}
