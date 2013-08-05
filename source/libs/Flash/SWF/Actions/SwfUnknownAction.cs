namespace DataDynamics.PageFX.Flash.Swf.Actions
{
    public sealed class SwfUnknownAction : SwfAction
    {
        public SwfUnknownAction(SwfActionCode code)
        {
            _code = code;
        }

        public SwfUnknownAction(SwfActionCode code, byte[] data)
        {
            _code = code;
            Data = data;
        }

	    public byte[] Data { get; set; }

	    public override SwfActionCode ActionCode
        {
            get { return _code; }
        }
        private readonly SwfActionCode _code;

        public override byte[] GetData()
        {
            return Data;
        }

        public override void ReadBody(SwfReader reader)
        {
            Data = reader.ReadAllBytes();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.Write(Data);
        }
    }
}