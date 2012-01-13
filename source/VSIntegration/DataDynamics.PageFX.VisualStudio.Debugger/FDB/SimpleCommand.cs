namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    delegate void StringHandler(string s);

    class SimpleCommand : Command
    {
        readonly StringHandler m_handler;

        public SimpleCommand(StringHandler handler, string text)
            : base(text)
        {
            m_handler = handler;
        }

        public SimpleCommand(StringHandler handler, string format, params object[] args)
            : base(format, args)
        {
            m_handler = handler;
        }

        public override bool IsMulti
        {
            get { return true; }
        }

        public override void Handler(string s)
        {
            m_handler(s);
        }
    }
}