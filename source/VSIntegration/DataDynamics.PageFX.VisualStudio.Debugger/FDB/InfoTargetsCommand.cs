using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    class InfoTargetsCommand : Command
    {
        public InfoTargetsCommand()
            : base("info targets")
        {
        }

        public override bool IsMulti
        {
            get { return true; }
        }

        public List<string> URIs
        {
            get { return _list; }
        }
        readonly List<string> _list = new List<string>();

        public List<string> GetLocalPathes()
        {
            var list = new List<string>();
            foreach (var s in _list)
            {
                try
                {
                    var uri = new Uri(s);
                    list.Add(uri.LocalPath);
                }
                catch
                {
                }
            }
            return list;
        }

        public override void Handler(string s)
        {
            _list.Add(s);
        }
    }
}