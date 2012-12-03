using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DataDynamics.PageFX.FlashLand.Core.CodeProvider;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    class InfoStackCommand : Command
    {
        readonly Engine m_engine;

        public InfoStackCommand(Engine engine)
            : base("info stack")
        {
            m_engine = engine;
        }

        public List<StackFrame> Stack = new List<StackFrame>();

        public override bool IsMulti
        {
            get { return true; }
        }

        //info stack
        //#0   this = [Object 15869633, class='tmp2::Test2'].Test2/dy() at Test2.as:19
        //#1   this = [Object 15011313, class='Test0'].Test0$iinit() at Test0.as:43

        //#0   this = [Object 15598953, class='Test1'].Test1/getOra() at Test1.as:17
        //#1   this = [Object 15011313, class='Test0'].Test0$iinit() at Test0.as:37

        //#0   this = [Object 15917809, class='caurina.transitions::Tweener$'].Tweener$/addTween(p_arg1=[Object 15917369, class='flash.display::Sprite'], p_arg2=[Object 15670753, class='Object']) at Tweener.as:91
        //#1   this = [Object 15769665, class='global'].() at Tweener_Test.as:25

        //"(fdb) #0   this = [Object 19240801, class='TestApp$::App'].App/OnBreak_flash.events.MouseEvent(e=[Object 21592881, class='flash.events::MouseEvent']) at Application.cs:64"
        //(fdb) #0   this = [Object 20028929, class='SampleTests::App'].App() at FE433A5C6473616D706C65735C4E556E69745357465F7372635C53616D706C6554657374735C53756974652E6373:12

        //(fdb) #0   enterDebugger() at <null>:0

        const string pat = @"\[(Object)\s+\d+, class='(?<class>.*)'\]\.(?<func>.*)\((?<args>.*)(\) at )(?<file>.*):(?<line>.*)";
        const string pat2 = @"#\d+\s+(?<func>.*)\(.*\)(\sat\s)(?<file>.*):(?<line>.*)";
        static readonly Regex rx = new Regex(pat, RegexOptions.Compiled);
        static readonly Regex rx2 = new Regex(pat2, RegexOptions.Compiled);

        public override void Handler(string s)
        {
            //if (rx == null)
            //{
            //    try
            //    {
            //        rx = new Regex(pat, RegexOptions.Compiled);
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}

            if (R1(s)) return;
            if (R2(s)) return;
        }

        bool R1(string s)
        {
            var m = rx.Match(s);
            if (!m.Success) return false;
            try
            {
                string klass = m.Groups["class"].Value;
                string func = m.Groups["func"].Value;
                //string args = m.Groups["args"].Value;
                string file = m.Groups["file"].Value;
                string line = m.Groups["line"].Value;
                var f = new StackFrame(m_engine)
                            {
                                File = DebugUtils.Decode(file),
                                Line = int.Parse(line),
                                Function = klass + "." + func
                            };
                Stack.Add(f);
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());
                return false;
            }
            return true;
        }

        bool R2(string s)
        {
            var m = rx2.Match(s);
            if (!m.Success) return false;

            try
            {
                string func = m.Groups["func"].Value;
                string file = m.Groups["file"].Value;
                string line = m.Groups["line"].Value;
                var f = new StackFrame(m_engine)
                {
                    File = file,
                    Line = int.Parse(line),
                    Function = func
                };
                Stack.Add(f);
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());
                return false;
            }

            return true;
        }
    }
}