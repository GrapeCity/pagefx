namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
    public class SwfActionUnknown : SwfAction
    {
        public SwfActionUnknown(SwfActionCode code)
        {
            _code = code;
        }

        public SwfActionUnknown(SwfActionCode code, byte[] data)
        {
            _code = code;
            _data = data;
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private byte[] _data;

        public override SwfActionCode ActionCode
        {
            get { return _code; }
        }
        private readonly SwfActionCode _code;

        public override byte[] GetData()
        {
            return _data;
        }

        public override void ReadBody(SwfReader reader)
        {
            _data = reader.ReadAllBytes();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.Write(_data);
        }
    }
}