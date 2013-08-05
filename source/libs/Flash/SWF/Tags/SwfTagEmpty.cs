namespace DataDynamics.PageFX.Flash.Swf.Tags
{
    /// <summary>
    /// Represents SWF tag that has not data. It is usefull for simple and control tags like ShowFrame.
    /// </summary>
    public sealed class SwfTagEmpty : SwfTag
    {
        public SwfTagEmpty(SwfTagCode code)
        {
            _code = code;
        }

        public override SwfTagCode TagCode
        {
            get { return _code; }
        }
        private readonly SwfTagCode _code;

        public override byte[] GetData()
        {
            return null;
        }

        public override void ReadTagData(SwfReader reader)
        {
        }

        public override void WriteTagData(SwfWriter writer)
        {
        }
    }
}