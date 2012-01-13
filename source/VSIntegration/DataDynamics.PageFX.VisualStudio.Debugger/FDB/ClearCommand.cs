namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    internal class ClearCommand : Command
    {
        public ClearCommand(string file, int line) 
            : base("clear {0}:{1}", file, line)
        {
        }

        public override bool IsMulti
        {
            get { return true; }
        }

        public bool OK = true;

        public override void Handler(string s)
        {
            //s == "Breakpoint location unknown."
            OK = false;
        }
    }
}