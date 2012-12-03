namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    public class SwfTagAny : SwfTag
    {
        public SwfTagAny(SwfTagCode tagCode)
        {
            _tagCode = tagCode;
        }

        public SwfTagAny(SwfTagCode tagCode, byte[] data)
        {
            _tagCode = tagCode;
            _data = data;
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private byte[] _data;

        public override SwfTagCode TagCode
        {
            get { return _tagCode; }
        }
        private readonly SwfTagCode _tagCode;

        public override byte[] GetData()
        {
            return _data;
        }

        public override void ReadTagData(SwfReader reader)
        {
            _data = reader.ReadAllBytes();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            if (_data != null)
                writer.Write(_data);
        }

        public override SwfTag Clone()
        {
            if (_data != null)
            {
                var data = (byte[])_data.Clone();
                return new SwfTagAny(_tagCode, data);
            }
            return new SwfTagAny(_tagCode);
        }
    }
}