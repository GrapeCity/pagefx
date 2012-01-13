namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    abstract class Command
    {
        protected Command(string text)
        {
            Text = text;
        }

        protected Command(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }

        /// <summary>
        /// Determines whether the command handles multiple lines
        /// </summary>
        public abstract bool IsMulti { get; }

        public bool Handled { get; set; }

        public string Text { get; set; }

        public abstract void Handler(string s);
    }
}