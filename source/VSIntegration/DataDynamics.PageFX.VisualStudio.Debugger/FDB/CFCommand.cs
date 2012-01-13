using System.Text.RegularExpressions;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    class CFCommand : Command
    {
        public string File;
        public string ID;
        public int Line;

        public CFCommand() : base("cf")
        {
        }

        public override bool IsMulti
        {
            get { return false; }
        }

        static readonly Regex re = new Regex(@"(\(fdb\) )?(?<file>.*)#(?<id>\d+):(?<line>\d+)");

        public override void Handler(string s)
        {
            var m = re.Match(s);
            if (m.Success)
            {
                File = m.Groups["file"].Value;
                ID = m.Groups["id"].Value;
                Line = int.Parse(m.Groups["line"].Value.Trim());
            }
        }
    }
}