namespace Nmkoder.Data.Streams
{
    class AttachmentStream : Stream
    {
        public AttachmentStream(string codec, string codecLong)
        {
            base.Type = StreamType.Attachment;
            Codec = codec;
            CodecLong = codecLong;
        }

        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}
