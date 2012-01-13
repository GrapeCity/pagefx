namespace DataDynamics.PageFX.FLI.SWF
{
    public abstract class SwfDisplayObject : SwfCharacter, ISwfDisplayObject
    {
        public ushort Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private ushort _depth;
    }
}