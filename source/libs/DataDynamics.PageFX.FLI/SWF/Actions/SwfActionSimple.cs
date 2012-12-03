namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
    public class SwfActionSimple : SwfAction
    {
        public SwfActionSimple(SwfActionCode code)
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