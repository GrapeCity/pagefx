using DataDynamics.PageFX.Flash.Swf.Actions;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Buttons
{
    [SwfTag(SwfTagCode.DefineButton)]
    public sealed class SwfTagDefineButton : SwfCharacter
    {
        /// <summary>
        /// Gets characters that make up the button.
        /// </summary>
        public SwfButtonList Characters
        {
            get { return _chars; }
        }
        private readonly SwfButtonList _chars = new SwfButtonList();

        public SwfActionList Actions
        {
            get { return _actions; }
        }
        private readonly SwfActionList _actions = new SwfActionList();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineButton; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            _chars.Read(reader, TagCode);
            _actions.Read(reader);
        }

        protected override void WriteBody(SwfWriter writer)
        {
            _chars.Write(writer, TagCode);
            _actions.Write(writer);
        }
    }
}