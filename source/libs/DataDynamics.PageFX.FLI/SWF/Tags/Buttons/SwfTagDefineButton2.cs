namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.DefineButton2)]
    public class SwfTagDefineButton2 : SwfCharacter
    {
        /// <summary>
        /// Gets characters that make up the button.
        /// </summary>
        public SwfButtonList Characters
        {
            get { return _chars; }
        }
        private readonly SwfButtonList _chars = new SwfButtonList();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineButton2; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            _chars.Read(reader, TagCode);
        }

        protected override void WriteBody(SwfWriter writer)
        {
            _chars.Write(writer, TagCode);
        }
    }
}