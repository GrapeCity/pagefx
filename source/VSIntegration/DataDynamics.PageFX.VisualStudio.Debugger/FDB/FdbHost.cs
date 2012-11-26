#define FDBLOG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    internal sealed class FdbHost : IDebugger, IDisposable
    {
        #region State
        enum State
        {
            Init,
            CommandPromt, //waiting fdb commands
            Continue,
            Step,
            Pause,
            Error = 100,
        }
        #endregion

        Process m_process;
        State m_state;
        readonly Engine m_engine;
        Service m_service;
        string m_error;

        #region ctor
        public FdbHost(Engine engine)
        {
            m_engine = engine;
            m_service = new Service();
#if FDBLOG
            InitLog();
#endif
        }
        #endregion

        #region Start
        string m_appPath;

        public int Start(string apppath)
        {
            m_state = State.Init;
            int pid = StartFdb(apppath);
            if (pid > 0)
            {
                m_appPath = apppath;
                if (Wait(State.Init, 30000) == Const.TIMEOUT)
                {
                    //debug session was not started.
                    return -1;
                }
                if (m_state != State.CommandPromt)
                {
                    return -1;
                }
                StartSession();
            }
            return pid;
        }

        int StartFdb(string swf)
        {
            m_process = new Process
                            {
                                StartInfo =
                                    {
                                        FileName = Util.GetFdbPath(),
                                        Arguments = ('"' + swf + '"'),
                                        UseShellExecute = false,
                                        RedirectStandardInput = true,
                                        RedirectStandardOutput = true,
                                        RedirectStandardError = true,
                                        CreateNoWindow = true
                                    },
                                EnableRaisingEvents = true
                            };
            m_process.OutputDataReceived += OnDataReceived;
            m_process.Exited += process_Exited;
            if (!m_process.Start())
                return -1;
            m_process.BeginOutputReadLine();
            return m_process.Id;
        }

        void StartSession()
        {
            RunSimple("set $displayattributes=1");
        }
        #endregion

        #region Stop
        public void Stop()
        {
            KillPlayer();
            Terminate();
        }

        public void Terminate()
        {
            m_engine.StopComplete();
            Release();
        }

        void DoQuit()
        {
            WriteLine("quit");
            Util.Wait(1000, () => m_process == null);
            Terminate();
        }

        void EndSession()
        {
            if (m_sessionTerminated) return;
            m_killPlayer = false;
            m_sessionTerminated = true;
            Op(DoQuit);
        }

        bool m_killPlayer = true;
        bool m_waitExecutionHalted;
        bool m_sessionTerminated;

        void KillPlayer()
        {
            if (!m_killPlayer) return;
            //if (IsPlayerRunning)
            //{
            //    m_waitExecutionHalted = true;
            //    Pause();
            //}
            //KillY();
            if (IsPlayerRunning)
                KillPlayerProcess();
            else
                KillY();
            WaitSessionTermination();
        }

        void KillPlayerProcess()
        {
            var p = FindPlayerProcess();
            if (p != null)
            {
                if (p.Responding)
                    p.CloseMainWindow();
                else
                    p.Kill();
            }
        }

        Process FindPlayerProcess()
        {
            //var info = new InfoTargetsCommand();
            //Run(info);

            //var swfs = info.GetLocalPathes();
            //var swf = SwfPath;

            foreach (var process in Process.GetProcesses())
            {
                if (IsPlayer(process))
                    return process;
            }

            return null;
        }

        bool IsPlayer(Process process)
        {
            if (CheckCL(process)) return true;
            if (CheckTitle(process)) return true;
            return false;
        }

        bool CheckCL(Process process)
        {
            var si = process.StartInfo;
            if (si != null)
            {
                string args = si.Arguments;
                if (string.IsNullOrEmpty(args))
                    return false;
                if (args.ToLower().Contains(m_appPath.ToLower()))
                    return true;
            }
            return false;
        }

        static bool CheckTitle(Process process)
        {
            string title = process.MainWindowTitle;
            if (string.IsNullOrEmpty(title)) return false;
            if (title.Contains("Flash Player"))
                return true;
            return false;
        }

        void KillY()
        {
            WriteLine("kill");
            WriteLine("y");
        }

        void WaitSessionTermination()
        {
            Util.Wait(5000, () => m_sessionTerminated);
        }

        static readonly object _lock = new object();

        void Release()
        {
            var p = m_process;
            m_process = null;
            if (p != null)
            {
                p.Kill();
                p.Close();
            }

            var s = m_service;
            m_service = null;
            if (s != null)
                s.Abort();
        }

        public void Dispose()
        {
            Release();
        }
        #endregion

        #region LOG
#if FDBLOG
        const string FdbLogOutputPath = @"c:\fdblog_output.txt";
        const string FdbLogInputPath = @"c:\fdblog_input.txt";
        static int InputCounter;

        static void InitLog()
        {
            using (var w = new StreamWriter(FdbLogOutputPath))
                w.Write("");
            using (var w = new StreamWriter(FdbLogInputPath))
                w.Write("");
        }

        static void LogOutput(string s)
        {
            using (var w = new StreamWriter(FdbLogOutputPath, true))
            {
                w.WriteLine(InputCounter + "." + s);
            }
        }

        static void LogInput(string s)
        {
            using (var w = new StreamWriter(FdbLogInputPath, true))
            {
                InputCounter++;
                w.WriteLine(InputCounter + "." + s);
            }
        }
#endif
        #endregion

        #region Communication
        bool m_skipUntilUnknownCommand;
        bool m_continueAfterSkip;
        Command m_curcmd;

        bool IsPlayerRunning
        {
            get
            {
                switch (m_state)
                {
                    case State.Continue:
                    case State.Pause:
                    case State.Step:
                        return true;
                }
                return false;
            }
        }

        static bool IsSessionEnd(string s)
        {
            if (s == "Player session terminated")
                return true;

            if (CRE.UnloadSWF.IsMatch(s))
                return true;

            if (CRE.NotRespond.IsMatch(s))
                return true;

            return false;
        }

        void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            string s = e.Data;
            if (s == null) return;

#if FDBLOG
            LogOutput(s);
#endif
            if (m_state == State.Init)
            {
                if (s.StartsWith("Another Flash debugger is probably running"))
                {
                    m_state = State.Error;
                    m_error = "Another Flash debugger is probably running; please close it.";
                    return;
                }

                if (CRE.FdbStart.IsMatch(s))
                {
                    m_state = State.CommandPromt;
                }
                return;
            }

            var m = CRE.TraceMessage.Match(s);
            if (m.Success)
            {
                string msg = m.Groups["msg"].Value;
                m_engine.Trace(msg);
                return;
            }

            m = CRE.FaultMessage.Match(s);
            if (m.Success)
            {
                string msg = m.Groups["msg"].Value;
                m_engine.Trace(msg);
                return;
            }

            if (IsSessionEnd(s))
            {
                EndSession();
                return;
            }

            if (CRE.AdditionalActionScriptLoaded.IsMatch(s))
            {
                Op(SkipAndContinue);
                return;
            }

            if (IsPlayerRunning)
            {
                if (ProcessUnknownCommand(s))
                    return;
                OnHalt(s);
                return;
            }

            if (m_curcmd != null)
            {
                if (m_curcmd.IsMulti)
                {
                    if (CRE.UnknownCommand.IsMatch(s))
                    {
                        m_curcmd.Handled = true;
                        return;
                    }
                    m_curcmd.Handler(s);
                }
                else
                {
                    m_curcmd.Handler(s);
                    m_curcmd.Handled = true;
                }
                return;
            }

            ProcessUnknownCommand(s);
        }

        bool ProcessUnknownCommand(string s)
        {
            if (m_skipUntilUnknownCommand)
            {
                if (CRE.UnknownCommand.IsMatch(s))
                {
                    m_skipUntilUnknownCommand = false;
                    if (m_continueAfterSkip)
                    {
                        m_continueAfterSkip = false;
                        m_state = State.CommandPromt;
                        Op(Continue);
                    }
                }
                return true;
            }
            return false;
        }

        void process_Exited(object sender, EventArgs e)
        {
            lock (_lock)
            {
                var p = m_process;
                m_process = null;
                if (p != null)
                {
                    try
                    {
                        p.Close();
                    }
                    finally
                    {
                        p.Dispose();
                    }
                }    
            }
        }

        void Run(Command cmd)
        {
            if (m_curcmd != null)
                throw new InvalidOperationException();
            m_curcmd = cmd;
            WriteLine(cmd.Text);
            if (cmd.IsMulti)
                WriteUnknownCommand();
            Util.Wait(() => cmd.Handled);
            m_curcmd = null;
        }

        public void RunSimple(string format, params object[] args)
        {
            string cmd = string.Format(format, args);
            RunSimple(cmd);
        }

        public void RunSimple(string cmd)
        {
            WriteLine(cmd);
            Skip();
        }

        const string unknown_command = "cmd_end";

        void WriteUnknownCommand()
        {
            WriteLine(unknown_command);
        }
        #endregion

        #region OnBreak
        void OnHalt(string s)
        {
            var m = CRE.ExecutionHalted.Match(s);
            if (m.Success)
            {
                Halt(false);
                return;
            }

            m = CRE.FaultHalted.Match(s);
            if (m.Success)
            {
                Halt(true);
                return;
            }

            if (ParseBreakpoint(s))
                return;

            //TODO: Handle exceptions
            if (m_state == State.Pause)
            {
                if (s == "(fdb) Do you want to attempt to halt execution? (y or n) Attempting to halt.")
                    return;
                if (s == "To help out, try nudging the Player (e.g. press a button)")
                    return;
                if (m_waitExecutionHalted) return;
                m_state = State.CommandPromt;
                Op(DoBreak);
            }
            else if (m_state == State.Step)
            {
                if ((CRE.Step.Match(s)).Success)
                {
                    m_state = State.CommandPromt;
                    Op(DoStep);
                }
                else
                {
                    Op(Continue);
                }
            }
        }

        void Halt(bool fault)
        {
            Operation op;
            switch (m_state)
            {
                case State.Step:
                    op = DoStep;
                    break;

                default:
                    op = DoBreak;
                    break;
            }

            m_state = State.CommandPromt;
            m_service.RunOperationAsync(op);
        }

        bool ParseBreakpoint(string s)
        {
            var m = CRE.Bbreakpoint.Match(s);
            if (!m.Success)
                m = CRE.Breakpoint2.Match(s);
            if (m.Success)
            {
                m_state = State.CommandPromt;
                m_bpid = m.Groups["bp"].Value;
                var i = Util.IndexOf(m_toclear, p => p.ID == m_bpid);
                if (i >= 0)
                {
                    var bp = m_toclear[i];
                    m_toclear.RemoveAt(i);
                    Clear(bp);
                    Continue();
                    return true;
                }
                Op(DoBreakpoint);
                return true;
            }
            return false;
        }

        void DoBreak()
        {
            if (!GetInfo(true))
            {
                Pause(true);
                return;
            }

            m_engine.SendCompleteEvent(CompleteEvent.Break);
        }

        void DoStep()
        {
            DoComplete(CompleteEvent.Step);
        }

        void DoComplete(CompleteEvent e)
        {
            GetInfo(true);
            m_engine.SendCompleteEvent(e);
        }

        string m_bpid;

        void DoBreakpoint()
        {
            var pb = m_engine.FindBreakpoint(m_bpid);
            if (pb != null)
            {
                bool c = EnableBreakpoints(pb);
                if (c)
                {
                    Continue();
                    return;
                }

                if (pb.HasCondition)
                {
                    if (GetInfo(true))
                    {
                        if (EvalCondition(pb))
                        {
                            m_engine.CompleteBreakpoint(pb);
                            return;
                        }    
                    }
                    Continue();
                    return;
                }

                var bb = pb.BoundBreakpoint;
                if (bb.Hit())
                {
                    if (GetInfo(true))
                    {
                        m_engine.CompleteBreakpoint(pb);
                        return;
                    }
                }

                Continue();
                return;
            }
        }

        bool EvalCondition(PendingBreakpoint pb)
        {
            var prop = EvalExpression(0, pb.ConditionalExpression);
            if (prop == null) return false;
            switch (pb.ConditionStyle)
            {
                case enum_BP_COND_STYLE.BP_COND_WHEN_TRUE:
                    pb.OldValue = prop.Value;
                    return prop.IsTrue;

                case enum_BP_COND_STYLE.BP_COND_WHEN_CHANGED:
                    if (pb.OldValue != prop.Value)
                    {
                        pb.OldValue = prop.Value;
                        return true;
                    }
                    break;
            }
            return false;
        }

        bool EnableBreakpoints(PendingBreakpoint pb)
        {
            bool c = false;
            foreach (var p in m_engine.BreakpointManager)
            {
                if (p.IsEnabledNow != p.IsEnabled)
                {
                    EnableCore(p, p.IsEnabled);
                    if (!p.IsEnabledNow && p == pb)
                        c = true;
                }
            }
            return c;
        }

        void Skip(bool @continue)
        {
            m_skipUntilUnknownCommand = true;
            m_continueAfterSkip = @continue;
            WriteUnknownCommand();
            Util.Wait(() => m_skipUntilUnknownCommand == false);
        }

        void Skip()
        {
            Skip(false);
        }

        void SkipAndContinue()
        {
            Skip(true);
        }
        #endregion

        #region GetInfo, GetFrameInfo
        DebugThread Thread
        {
            get { return m_engine.Thread; }
        }

        /// <summary>
        /// Gets info about all stack frames
        /// </summary>
        /// <param name="skip"></param>
        /// <returns></returns>
        bool GetInfo(bool skip)
        {
            if (skip)
                Skip();

            var cmd = new InfoStackCommand(m_engine);
            Run(cmd);

            var stack = cmd.Stack;
            if (stack.Count > 0)
            {
                for (int i = 0; i < stack.Count; ++i)
                {
                    var frame = stack[i];
                    frame.Index = i;
                    if (i != 0)
                        SelectFrame(i);
                    GetFrameInfo(frame);
                }

                SelectFrame(0);

                Thread.Stack = stack.ToArray();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Selects given frame (frame command)
        /// </summary>
        /// <param name="frame"></param>
        void SelectFrame(int frame)
        {
            WriteLine("frame {0}", frame);
            Skip();
        }

        void GetFrameInfo(StackFrame frame)
        {
            GetThisInfo(frame);
            GetLocals(frame, true);
            GetLocals(frame, false);
        }

        #region GetLocals
        void GetLocals(StackFrame frame, bool args)
        {
            var cmd = new ParsePropertyCommand(frame, args ? "info arguments" : "info locals");
            Run(cmd);

            var list = cmd.Properties;
            if (args)
                frame.Args.AddRange(list);
            else
                frame.Locals.AddRange(list);

            //foreach (var prop in list)
            //{
            //    if (prop.IsComplex)
            //        EvalChildProperties(prop);
            //}
        }
        #endregion
        
        #region GetThisInfo
        void GetThisInfo(StackFrame frame)
        {
            var prop = new Property("this")
                           {
                               IsThis = true,
                               IsObject = true,
                               Frame = frame,
                               FullName = "this",
                           };
            //EvalChildProperties(prop);
            frame.Args.Add(prop);

            //var list = EvalChildProperties(frame, "this");
            //int n = list.Count;
            //if (n > 0)
            //{
            //    var prop = new Property("this")
            //    {
            //        Type = list[0].Type,
            //        IsThis = true,
            //        Frame = frame,
            //        FullName = "this",
            //    };
            //    prop.AddFrom1(list);
            //    frame.Args.Add(prop);
            //}
        }
        #endregion
        #endregion

        #region Pause
        public void Pause()
        {
            Pause(false);
        }

        void Pause(bool fcontinue)
        {
            if (m_state == State.Pause) return;
            if (fcontinue) WriteLine("continue");
            WriteLine("hi");
            WriteLine("y");
            m_state = State.Pause;
        }
        #endregion

        #region Continue
        public void Continue()
        {
            if (m_state == State.Continue) return;
            m_state = State.Continue;
            WriteLine("continue");
        }
        #endregion

        #region Breakpoints
        public bool BindBreakpoint(PendingBreakpoint bp)
        {
            var i = IndexOfClearBreakpoint(bp);
            if (i >= 0)
            {
                bp.ID = m_toclear[i].ID;
                m_toclear.RemoveAt(i);
                return true;
            }

            if (IsPlayerRunning)
            {  
                return false;
            }
            else
            {
                var cmd = new BreakCommand(bp.EncodedFile, bp.Line);
                Run(cmd);
                bp.ID = cmd.ID;

                //if (bp.HasCondition)
                //    SetCondition(bp);

                return true;
            }
        }

        int IndexOfClearBreakpoint(PendingBreakpoint bp)
        {
            return Util.IndexOf(m_toclear, p => p.File == bp.File && p.Line == bp.Line);
        }

        readonly List<PendingBreakpoint> m_toclear = new List<PendingBreakpoint>();

        public bool RemoveBreakpoint(PendingBreakpoint bp)
        {
            if (IsPlayerRunning)
            {
                m_toclear.Add(bp);
                return true;
            }
            return Clear(bp);
        }

        public void EnableCore(PendingBreakpoint bp, bool value)
        {
            if (value)
                RunSimple("enable {0}", bp.ID);
            else
                RunSimple("disable {0}", bp.ID);
            bp.IsEnabledNow = value;
        }

        public bool EnableBreakpoint(PendingBreakpoint bp, bool value)
        {
            if (IsPlayerRunning)
                return true;
            EnableCore(bp, value);
            return true;
        }

        bool Clear(PendingBreakpoint bp)
        {
            if (bp.BoundBreakpoint != null)
            {
                var cmd = new ClearCommand(bp.EncodedFile, bp.Line);
                Run(cmd);
                return cmd.OK;
            }
            return true;
        }
        #endregion

        #region Stepping
        bool IsOuterMostFrame
        {
            get
            {
                var stack = m_engine.Thread.Stack;
                if (stack == null) return false;
                return stack.Length == 1;
            }
        }

        public int Step(enum_STEPKIND kind, enum_STEPUNIT unit)
        {
            if (m_state != State.CommandPromt)
                return Const.E_FAIL;

            switch (kind)
            {
                    case enum_STEPKIND.STEP_BACKWARDS:
                        return Const.E_NOTIMPL;

                    case enum_STEPKIND.STEP_INTO:
                    {
                        m_state = State.Step;
                        WriteLine("step");
                    }
                    break;

                    case enum_STEPKIND.STEP_OUT:
                    {
                        if (IsOuterMostFrame)
                        {
                            Continue();
                        }
                        else
                        {
                            m_state = State.Step;
                            WriteLine("finish");
                        }
                    }
                    break;

                    case enum_STEPKIND.STEP_OVER:
                    {
                        m_state = State.Step;
                        WriteLine("next");
                    }
                    break;
            }

            return Const.S_OK;
        }
        #endregion

        #region Utils
        void Op(Operation op)
        {
            m_service.RunOperationAsync(op);
        }

        public void WriteLine(string s)
        {
#if FDBLOG
            LogInput(s);
#endif
            m_process.StandardInput.WriteLine(s);
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        int Wait(State state, int timeout)
        {
            return Util.Wait(timeout, () => m_state != state);
        }

        int Wait(State state)
        {
            return Wait(state, 10000);
        }
        #endregion

        #region EvalChildProperties
        PrintCommand EvalExp(StackFrame frame, string exp)
        {
            SelectFrame(frame.Index);
            var cmd = new PrintCommand(frame, exp);
            Run(cmd);
            return cmd;
        }

        PrintCommand EvalChildProperties(StackFrame frame, string prop)
        {
            return EvalExp(frame, prop + ".");
        }

        public void EvalChildProperties(Property prop)
        {
            var cmd = EvalChildProperties(prop.Frame, prop.FullName);
            var list = cmd.Properties;
            if (list.Count > 0)
            {
                prop.Type = list[0].Type;
                if (EvalNativeArray(prop, list)) return;
                if (EvalSystemArray(prop, list)) return;
                if (EvalCollectionView(prop, cmd.SpecProperties)) return;
                prop.AddFrom1(list);
                EvalValue(prop, cmd.SpecProperties);
            }
        }

        #region EvalNativeArray
        bool EvalNativeArray(Property prop, IEnumerable<Property> list)
        {
            if (!prop.IsNativeArray) return false;
            var lenProp = Util.Find(list, p => p.Name == DebugSpecProperties.NativeArrayLength);
            if (lenProp == null) return false;
            int len;
            if (!int.TryParse(lenProp.Value, out len)) return false;

            prop.Value = "Length = " + len;
            for (int i = 0; i < len; ++i)
            {
                string itemName = "[" + i + "]";
                string itemFullName = prop.FullName + itemName;
                var cmd = EvalChildProperties(prop.Frame, itemFullName);
                var itemProps = cmd.Properties;
                int pn = itemProps.Count;
                if (pn == 0) continue;
                if (pn == 1)
                {
                    var child = itemProps[0];
                    child = child.Clone();
                    child.FullName = itemFullName;
                    child.Name = itemName;
                    prop.AddChild(child);
                }
                else
                {
                    var child = new Property(itemName)
                    {
                        FullName = itemFullName,
                        Type = itemProps[0].Type,
                    };
                    prop.AddChild(child);
                    child.AddFrom1(itemProps);
                }
            }
            return true;
        }
        #endregion

        #region EvalSystemArray
        bool EvalSystemArray(Property prop, IEnumerable<Property> list)
        {
            if (!prop.IsSystemArray) return false;
            return EvalArrayProperty(prop, list, "m_value", false);
        }
        #endregion

        #region EvalArrayProperty
        bool EvalArrayProperty(Property prop, IEnumerable<Property> list, string name, bool iscollection)
        {
            Property arr = null;
            Property isdic = null;
            if (iscollection)
            {
                foreach (var p in list)
                {
                    if (p.Name == name)
                    {
                        arr = p;
                        if (isdic != null)
                            break;
                    }
                    if (p.Name == DebugSpecProperties.DictionaryMarker)
                    {
                        isdic = p;
                        if (arr != null)
                            break;
                    }
                }
            }
            else
            {
                arr = Util.Find(list, p => p.Name == name);
            }

            if (arr == null) return false;
            arr.FullName = prop.FullName + "." + arr.Name;
            EvalChildProperties(arr);
            arr.Unregister();

            int n = arr.Kids.Count;
            prop.Value = iscollection ? "Count = " + n : "Length = " + n;

            if (isdic != null)
            {
                foreach (var kid in arr.Kids)
                {
                    var key = kid["Key"];
                    var val = kid["Value"];
                    if (key != null && val != null)
                    {
                        if (!key.IsComplex)
                        {
                            kid.Name = key.Value;
                            if (val.IsComplex)
                            {
                                kid.FullName = val.FullName;
                                kid.IsObject = true;
                                kid.ReevalChildren();
                            }
                            else
                            {
                                kid.Value = val.Value;
                                kid.IsNotExpandable = true;
                            }
                        }
                    }
                }
            }

            prop.AddChildren(arr.Kids);
            prop.Register();
            //TODO: Eval array type string
            return true;
        }
        #endregion

        bool EvalCollectionView(Property prop, IEnumerable<Property> list)
        {
            return EvalArrayProperty(prop, list, DebugSpecProperties.CollectionView, true);
        }

        static void EvalValue(Property prop, IEnumerable<Property> specProps)
        {
            foreach (var specProp in specProps)
            {
                if (specProp.Name == DebugSpecProperties.DisplayExpression)
                {
                    prop.Value = EvalDisplayExpression(prop, specProp.Value);
                    return;
                }

                if (specProp.Name == DebugSpecProperties.Display)
                {
                    prop.Value = specProp.Value;
                    return;
                }
            }
        }

        private class ExpressionEvaluator : FormatExpression.IEvaluator
        {
            readonly Property _property;

            public ExpressionEvaluator(Property prop)
            {
                _property = prop;
            }

            public string Evaluate(string exp)
            {
                if (exp.IsSimpleId())
                {
                    var prop = _property[exp];
                    if (prop != null) return prop.Value;
                    return "";
                }

                exp = ExtendIds(_property.FullName + ".", exp);
                try
                {
                    var frame = _property.Frame;
                    var prop = frame.EvalExpression(exp);
                    if (prop == null) return "???";
                    return prop.Value;
                }
                catch (Exception)
                {
                    return "???";
                }
            }

            static string ExtendIds(string prefix, string exp)
            {
                var sb = new StringBuilder();
                int n = exp.Length;
                for (int i = 0; i < n; ++i)
                {
                    char c = exp[i];
					if (c.IsSimpleIdStartChar())
                    {
                        string id = "";
                        id += c;
                        for (++i; i < n; ++i)
                        {
                            c = exp[i];
                            if (!c.IsSimpleIdChar())
                            {
                                --i;
                                break;
                            }
                            id += c;
                        }
                        sb.Append(prefix + id);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }
        }

        static string EvalDisplayExpression(Property prop, string exp)
        {
            try
            {
                return FormatExpression.Eval(new ExpressionEvaluator(prop), exp);
            }
            catch (Exception e)
            {
                //TODO: Look at how CLR debugger resolves this situation.
                return "";
            }
        }
        #endregion

        #region EvalExpression
        public Property EvalExpression(int frame, string code)
        {
            return EvalExpression(Thread.Stack[0], code);
        }

        public Property EvalExpression(StackFrame frame, string code)
        {
            if (IsPlayerRunning)
                return null;
            var cmd = EvalExp(frame, code);
            var list = cmd.Properties;
            int n = list.Count;
            if (n == 1)
            {
                var p0 = list[0];
                p0 = p0.Clone();
                p0.Name = code;
                return p0;
            }
            var prop = new Property(code)
                           {
                               Frame = frame
                           };
            prop.AddFrom1(list);
            return prop;
        }
        #endregion

        #region Condition
        public bool SetCondition(PendingBreakpoint bp)
        {
            if (IsPlayerRunning)
            {
            }
            else
            {
                RunSimple("cond {0} {1}", bp.ID, bp.ConditionalExpression);
            }
            return true;
        }
        #endregion
    }

    public static class DebugSpecProperties
    {
        public const string Prefix = "pfx$debug$";
        public const string NativeArrayLength = "length";
        public const string CollectionView = Prefix + "collection$view";
        public const string DictionaryMarker = Prefix + "dictionary$marker";
        public const string DisplayExpression = Prefix + "display$exp";
        public const string Display = Prefix + "display";
    }
}
