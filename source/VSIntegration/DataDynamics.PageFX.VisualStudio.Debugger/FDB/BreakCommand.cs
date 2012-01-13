using System;
using System.Text.RegularExpressions;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    class BreakCommand : Command
    {
        public BreakCommand(string file, int line)
            : base("break {0}:{1}", file, line)
        {
        }

        public override bool IsMulti
        {
            get { return true; }
        }

        public string ID;

        //(fdb) Breakpoint 1: file Test0.as, line 17
        //(fdb) Breakpoint not set; no executable code at line 19 of Test0.as#1
        //(fdb) Breakpoint 1 created, but not yet resolved.
        static readonly Regex re1 = new Regex(@"Breakpoint (?<bp>\d+):.*", RegexOptions.Compiled);
        static readonly Regex re2 = new Regex(@"Breakpoint (?<bp>\d+) created.*", RegexOptions.Compiled);

        public override void Handler(string s)
        {
            var m = re1.Match(s);
            if (m.Success)
            {
                Init(m);
                return;
            }

            m = re2.Match(s);
            if (m.Success)
            {
                Init(m);
                return;
            }
        }

        void Init(Match m)
        {
            string breakpoint = m.Groups["bp"].Value;
            ID = breakpoint;
            Handled = true;
        }
    }
}