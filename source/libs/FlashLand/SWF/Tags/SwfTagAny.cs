namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    public sealed class SwfTagAny : SwfTag
    {
        public SwfTagAny(SwfTagCode tagCode)
        {
            _tagCode = tagCode;
        }

        public SwfTagAny(SwfTagCode tagCode, byte[] data)
        {
            _tagCode = tagCode;
            Data = data;
        }

	    public byte[] Data { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return _tagCode; }
        }
        private readonly SwfTagCode _tagCode;

        public override byte[] GetData()
        {
            return Data;
        }

        public override void ReadTagData(SwfReader reader)
        {
            Data = reader.ReadAllBytes();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            if (Data != null)
                writer.Write(Data);
        }

        public override SwfTag Clone()
        {
            if (Data != null)
            {
                var data = (byte[])Data.Clone();
                return new SwfTagAny(_tagCode, data);
            }
            return new SwfTagAny(_tagCode);
        }
    }
}