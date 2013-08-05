namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
    public sealed class SwfSimpleAction : SwfAction
    {
        public SwfSimpleAction(SwfActionCode code)
        {
            _code = code;
        }

        public override SwfActionCode ActionCode
        {
            get { return _code; }
        }
        private readonly SwfActionCode _code;

        public override void ReadBody(SwfReader reader)
        {
        }

        public override void WriteBody(SwfWriter writer)
        {
        }
    }
}