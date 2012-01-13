using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
//using ScintillaNet;
//using PluginCore;
using System.IO;
using System.Text.RegularExpressions;
//using ASCompletion.Settings;
using System.Collections;
using DataDynamics.PageFX.VisualStudio.Debugger;
using Thread=System.Threading.Thread;

namespace FdbPlugin
{
    #region public structures

    public class FdbMsg
    {
        public string filefillpath;
        public string filename;
        public int line;
        public List<string> output;
        public bool ismove;

        public void SetParam(string filefillpath, string filename, int line, bool ismove)
        {
            this.filefillpath = filefillpath;
            this.filename = filename;
            this.line = line;
            this.ismove = ismove;
        }
    }
    public class PrintArg
    {
        public string valname;
        public PrintType printtype;
        public List<string> output;

        public PrintArg(string valname, PrintType printtype, List<string> output)
        {
            this.valname = valname;
            this.printtype = printtype;
            this.output = new List<string>(output);
        }
    }

    public delegate void ContinueEventHandler(object sender, FdbMsg e);
    public delegate void PrintEventHandler(object sender, PrintArg e);
    public delegate void fdbEventHandler(object sender);
    public delegate void TraceEventHandler(object sender, string trace);

    public enum FdbState
    {
        INIT,
        START,
        PRELOAD,
        STOP,
        PAUSE,
        PAUSE_SET_BREAKPOINT,
        CONTINUE,
        BREAK,
        STEP,
        NEXT,
        WAIT,
        UNLOAD,
        EXCEPTION
    }

    public enum PrintType
    {
        LOCAL,
        LOCALEXPAND,
        LOCALEXPANDUPDATE,
        DATATIP,
        DATATIPEXPAND
    }
    #endregion

    public class FdbWrapper
    {
        #region internal structures
        class SourceFileInfo
        {
            private string filefullpath;
            private string filename;
            private string filenum;

            public SourceFileInfo(string filefullpath, string filename, string filenum)
            {
                this.filefullpath = filefullpath;
                this.filename = filename;
                this.filenum = filenum;
            }
            public string FileFullPath
            {
                get { return this.filefullpath; }
                set { this.filefullpath = value; }
            }
            public string Filename
            {
                get { return this.filename; }
                set { this.filename = value; }
            }
            public string Filenum
            {
                get { return this.filenum; }
                set { this.filenum = value; }
            }
        }

        class BreakPointInfo
        {
            private string filefullpath;
            private string filename;
            private string breakpointnum;
            private int breakpointline;

            public BreakPointInfo(string filefullpath, string filename, string breakpointnum, int breakpointline)
            {
                this.filefullpath = filefullpath;
                this.filename = filename;
                this.breakpointnum = breakpointnum;
                this.breakpointline = breakpointline;
            }
            public string FileFullPath
            {
                get { return this.filefullpath; }
                set { this.filefullpath = value; }
            }
            public string Filename
            {
                get { return this.filename; }
                set { this.filename = value; }
            }
            public string BreakPointNum
            {
                get { return this.breakpointnum; }
                set { this.breakpointnum = value; }
            }
            public int BreakPoinLine
            {
                get { return this.breakpointline; }
                set { this.breakpointline = value; }
            }
        }

        class BreakPointBuf
        {
            private string filefullpath;
            private string filename;
            private List<int> breakpointlist;

            //key:line, val:on,off
            private Dictionary<int, bool> breakpointstateDic = new Dictionary<int,bool>();

            public BreakPointBuf(string filefullpath, string filename)
            {
                this.filefullpath = filefullpath;
                this.filename = filename;
                breakpointlist = new List<int>();
            }
            public string FileFullPath
            {
                get { return this.filefullpath; }
                set { this.filefullpath = value; }
            }
            public string Filename
            {
                get { return this.filename; }
                set { this.filename = value; }
            }

            public List<int> BreakPointList
            {
                get { return this.breakpointlist; }
                set { this.breakpointlist = value; }
            }

            public Dictionary<int, bool> BreakPointStateDic
            {
                get { return this.breakpointstateDic; }
            }

            public void SetBreakPointState(int line, bool ismark)
            {
                if (breakpointstateDic.ContainsKey(line))
                {
                    breakpointstateDic[line] = ismark;
                }
                else
                {
                    breakpointstateDic.Add(line, ismark);
                }
            }

            public bool GetBreakPointState(int line)
            {
                if (breakpointstateDic.ContainsKey(line))
                {
                    return breakpointstateDic[line];
                }
                else
                {
                    return false;
                }
            }
        }

        class CurrentFileInfo
        {
            public string filefullpath;
            public string filename;
            public string function;
            public int line;

            public void SetParam(string filefullpath, string filename, string function, int line)
            {
                this.filefullpath = filefullpath;
                this.filename = filename;
                this.function = function;
                this.line = line;
            }
        }

        private enum StepState
        {
            STEP,
            STEP_IN_INST,
            STEP_INOUT,
            STEP_BREAK,
            STEP_BREAK_JUMP
        }
        #endregion 
        
        private const bool VERBOSE = false;

        private CurrentFileInfo currentFileInfo = new CurrentFileInfo();
        private bool outputstart = false;
        private bool outputend = false;
        private List<string> bufferlist = new List<string>();
        private Process process;
        private bool isDebugStart = false;
        private bool isprocess = false;
        private Queue<string> filefullpathQueue = new Queue<string>();

#if NOT_PFX
        private ProjectManager.Projects.Project currentProject;
        private IContextSettings currentSettings;
#endif

        private string fdbexefile;
        private string checkstart = string.Empty;
        private string checkend = string.Empty;
        private string rootdir;
        private string outputfilefullpath;
        private List<string> classpathlist = new List<string>();
        public event ContinueEventHandler ContinueEvent = null;
        public event PrintEventHandler PrintEvent = null;
        public event ContinueEventHandler LocalValuesEvent = null;
        //public event fdbEventHandler StartEvent = null;
        public event fdbEventHandler StopEvent = null;
        public event TraceEventHandler TraceEvent = null;
        public event fdbEventHandler PauseEvent = null;
        public event fdbEventHandler ExceptionEvent = null;
        public event fdbEventHandler PuaseNotRespondEvent = null;

        private StepState currentStepState;
        private int issomecmd = 0;
        char[] chTrimsStart = { '\\' };

        private FdbState currentfdbState;

        //key:breakpoint
        private Dictionary<string, BreakPointInfo> breakPointInfoDic = new Dictionary<string, BreakPointInfo>();

        //key:fullpath
        private Dictionary<string, BreakPointBuf> breakPointBufferDic = new Dictionary<string, BreakPointBuf>();
        
        //key:output
        private Dictionary<string, MethodInvoker> execdic = new Dictionary<string, MethodInvoker>();

        #region regular expressions
        //start
        //[SWF] D:\ActionScrip\Test0\bin\Test0.swf - 2,931 bytes after decompression
        static Regex regfdbStart = new Regex(@"^\[SWF\].*?(bytes after decompression)", RegexOptions.Compiled);       

        //info sources
        //(fdb) Test0.as#1 
        //Test1.as#2
        static Regex regInfoSource = new Regex(@"(.*?\s(?<filename>.*).*?#(?<filenum>.*))|((?<filename>.*).*?#(?<filenum>.*))", RegexOptions.Compiled);

        //list
        // 1     package
        // 1     package {
        //Regex reRootPackage = new Regex(@"(.*?(package)$)|(.*?(package)(\s*)$)|(.*?(package)(\s*?\{.*)$)", RegexOptions.Compiled);
        
        //list
        // 1     package tmp1
        // 1     package tmp1 {
        Regex rePackage = new Regex(@"(.*?(package)\s+(?<path>.*)(.*?(\s|\{)))|(.*?(package)\s+(?<path>.*))", RegexOptions.Compiled);

        //(fdb) Breakpoint 1: file Test0.as, line 17
        //(fdb) Breakpoint not set; no executable code at line 19 of Test0.as#1
        Regex reBreak = new Regex(@"Breakpoint (?<breakpoint>.*).*?: file (?<filename>.*).*?, line (?<line>.*)", RegexOptions.Compiled);

        //Breakpoint 1, Test0$iinit() at Test0.as:17
        //Breakpoint 2, getOra() at Test1.as:19
        Regex reContinue = new Regex(@"Breakpoint (?<breakpoint>.*).*?,(?<function>.*).*?(at)", RegexOptions.Compiled);

        //(fdb) Breakpoint 1, Tweener_Test.as:25
        Regex reContinue2 = new Regex(@"Breakpoint (?<breakpoint>.*).*?,(?<filename>.*).*?:(?<line>.*)", RegexOptions.Compiled);

        //step in(inst)         : Execution halted, global$init() at Test2.as:8
        //                        8      public class Test2
        Regex reStepInInst = new Regex(@"Execution halted, global\$init\(\) at (?<filename>.*).*?:", RegexOptions.Compiled);

        //step in(func)        :Execution halted, getOra() at Test1.as:17
        //                       17             public function getOra():String
        //step out(inst, func) :Execution halted, Test0() at Test0.as:46
        //                  46                     stt2 = new tmp1.Test2();
        Regex reStepInorOut = new Regex(@"Execution halted, .*?(at )(?<filename>.*).*?:(?<line>.*)", RegexOptions.Compiled);

        //step break point: Breakpoint 1, Test0() at Test0.as:40
        //                  40                     st2 = new tmp2.Test2();
        Regex reStepBreakPoint = new Regex(@"Breakpoint (?<breakpoint>.*).*?(,\s)(?<function>.*).*?( at ).*?:(?<line>.*)", RegexOptions.Compiled);

        //step            : 15                     _punch = "tmp2Test2";
        Regex reStep = new Regex(@"(?<line>\d*).*?\s", RegexOptions.IgnoreCase);

        Regex reStepExecutionhaltedFunction = new Regex(@"Execution halted, (?<function>.*).*?( at )(?<filename>.*).*?:(?<line>.*)", RegexOptions.Compiled);

        //Execution halted, Main.mxml:4
        Regex reStepExecutionhalted = new Regex(@"Execution halted, (?<filename>.*).*?:(?<line>.*)", RegexOptions.IgnoreCase);


        //(fdb) p chi1.
        //$3 = chi1 = [Object 15011577, class='flash.display::Sprite']
        Regex rePrintObject = new Regex(@".*?(\s=\s)(?<name>.*).*?(\s=\s).*", RegexOptions.Compiled);

        //(fdb) $1 = 10 (0xa)
        Regex rePrintVal = new Regex(@".*?(\s=\s)(?<value>.*)", RegexOptions.Compiled);

        //Variable tmpp unknown
        //Expression could not be evaluated.
        Regex rePrintValUnknown = new Regex(@"Variable\s\S*\sunknown", RegexOptions.Compiled);

        //info stack
        //#0   this = [Object 15869633, class='tmp2::Test2'].Test2/dy() at Test2.as:19
        //#1   this = [Object 15011313, class='Test0'].Test0$iinit() at Test0.as:43

        //#0   this = [Object 15598953, class='Test1'].Test1/getOra() at Test1.as:17
        //#1   this = [Object 15011313, class='Test0'].Test0$iinit() at Test0.as:37

        //#0   this = [Object 15917809, class='caurina.transitions::Tweener$'].Tweener$/addTween(p_arg1=[Object 15917369, class='flash.display::Sprite'], p_arg2=[Object 15670753, class='Object']) at Tweener.as:91
        //#1   this = [Object 15769665, class='global'].() at Tweener_Test.as:25
        Regex reStack = new Regex(@"(\[(Object).*?(, )class=.*?\])", RegexOptions.Compiled);
        Regex reStackPath = new Regex(@".*?(class=')(?<path>.*).*?(::)", RegexOptions.Compiled);
        Regex reStackFilename = new Regex(@".*?( at )(?<filename>.*).*?:", RegexOptions.Compiled);
        Regex reStackPathFrame = new Regex(@".*?(class=')(?<path>.*).*?'", RegexOptions.Compiled);

        //end
        //(fdb) [UnloadSWF] D:\desktop\src\ActionScrip\Test0\bin\Test0.swf
        //Player session terminated
        Regex reUnloadSWF = new Regex(@"^?\[UnloadSWF\]", RegexOptions.Compiled);

        //(fdb) si.i = 10 (0xa)
        Regex reFirstPrintOutput = new Regex(@"\(fdb\)\s(?<name>.*).*?(\s=\s)(?<value>.*)", RegexOptions.Compiled);

        //(fdb) [trace] ....
        static Regex reTrace = new Regex(@"^?\[trace\]", RegexOptions.Compiled);
        
        //step
        //(fdb) Player did not respond to the command as expected; command aborted.
        static Regex reNotRespond = new Regex(@"Player did not respond to the command as expected; command aborted\.", RegexOptions.Compiled);

        //pause
        //possible continue
        //Execution halted 000095c6at 0xExecution halted 000095c6 (38342)
        //static Regex rePause = new Regex(@"Execution halted ([0-9]|[a-z])* 0xExecution halted ([0-9]|[a-z])* \(\d*\)", RegexOptions.Compiled);
        //Execution halted in 'test.swf' ffffffffat 0xExecution halted in 'test.swf' ffffffff (-1)
        static Regex rePause = new Regex(@"Execution halted .* 0xExecution halted .* \([-\d]*\)", RegexOptions.Compiled);
        
        //[Fault] exception, information=Error: my error
        //Fault, dy() at Test2.as:21
        // 21                     throw new Error("my error");
        static Regex reException = new Regex(@"^?\[Fault\]", RegexOptions.Compiled);

        //(fdb) [Fault] exception, information=Error: my error
        //21    	throw new Error("my error"); 
        static Regex reExceptionInfo = new Regex(@".*?(,\s)(?<function>.*).*?(\sat\s)(?<filename>.*).*?:(?<line>\d+)", RegexOptions.Compiled);
        static Regex reExceptionline = new Regex(@"\s?(?<line>\d+).*?\s.*", RegexOptions.Compiled);

        //PreLoad
        //(fdb) Additional ActionScript code has been loaded from a SWF or a frame.
        static Regex rePreLoad = new Regex(@"Additional ActionScript code has been loaded from a SWF or a frame", RegexOptions.Compiled);
        #endregion

        string CompileTargetFileFullPath;

        public string Outputfilefullpath
        {
            get { return this.outputfilefullpath; }
            set { this.outputfilefullpath = value; }
        }

#if NOT_PFX
        public ProjectManager.Projects.Project CurrentProject
        {
            get { return this.currentProject; }
            set
            {
                this.currentProject = value;
                if (currentProject == null)
                    return;
                CompileTargetFileFullPath = Path.Combine(this.currentProject.Directory, this.currentProject.CompileTargets[0]);
                rootdir = Path.GetDirectoryName(CompileTargetFileFullPath);
                if (currentProject.AbsoluteClasspaths != null)
                {
                    foreach (string path in currentProject.AbsoluteClasspaths)
                    {
                        if (!classpathlist.Contains(path))
                            classpathlist.Add(path);
                    }
                }
                if (classpathlist.Count == 0 && !classpathlist.Contains(currentProject.Directory))
                    classpathlist.Add(currentProject.Directory);
            }
        }

        public IContextSettings CurrentSettings
        {
            get { return this.currentSettings; }
            set
            {
                this.currentSettings = value;

                AS3Context.AS3Settings as3setting = AS3Context.PluginMain.Settings;
                fdbexefile = System.IO.Path.Combine(as3setting.FlexSDK, @"bin\fdb.exe");
                if (as3setting.UserClasspath != null)
                {
                    foreach (string path in as3setting.UserClasspath)
                    {
                        if (!classpathlist.Contains(path))
                            classpathlist.Add(path);
                    }
                }
            }
        }

#endif

        public bool IsDebugStart
        {
            get 
            {     
                if (process == null)
                    return false;
                else
                    return isDebugStart;
            }
        }

        public FdbState State
        {
            get { return currentfdbState; }
        }

        public FdbWrapper()
        {
            init();
        }

        private void init()
        {
            execdic.Add(getUnknownCommandOutput(getendtcmd("break")), exit_Break);
            execdic.Add(getUnknownCommandOutput(getendtcmd("continue")), exit_Continue);
            execdic.Add(getUnknownCommandOutput(getendtcmd("step")), exit_Step);
            execdic.Add(getUnknownCommandOutput(getendtcmd("NonCmd")), exit_Non);
            execdic.Add(getUnknownCommandOutput(getendtcmd("info locals")), exit_infolocals);
            execdic.Add(getUnknownCommandOutput(getendtcmd("print")), exit_Print);
            execdic.Add("(fdb) " + getUnknownCommandOutput(getendtcmd("clearbreakpoint")), exit_ClearBreakPoint);            
            execdic.Add(getUnknownCommandOutput(getendtcmd("step_checkpackage")), exit_CheckPackage);
            execdic.Add(getUnknownCommandOutput(getendtcmd("info stack")), exit_InfoStack);
            execdic.Add(getUnknownCommandOutput(getendtcmd("next")), exit_Next);
            execdic.Add(getUnknownCommandOutput(getendtcmd("finish")), exit_Finish);
            execdic.Add(getUnknownCommandOutput(getendtcmd("show directories")), exit_ShowDirectories);
            execdic.Add("(fdb) Delete all breakpoints? (y or n) (fdb) Unknown command 'delete_end', ignoring it", exit_Delete);
        }

        private void exit_terminated()
        {
            isunload = true;
            isprocess = false;
            currentfdbState = FdbState.UNLOAD;

            process.Kill();
            breakPointInfoDic.Clear();
            breakPointBufferDic.Clear();

            isDebugStart = false;
            if (StopEvent != null)
                StopEvent(this);
        }

#if NOT_PFX

        public void MarkBreakPoint(ScintillaControl sender, int line)
        {
            int re = (sender.MarkerGet(line) & GetMarkerMask(1));
            line++;
            if (re == 0) //no
            {
                if (breakPointBufferDic.ContainsKey(sender.FileName))
                {
                    if (breakPointBufferDic[sender.FileName].BreakPointList.IndexOf(line) >= 0)
                    {
                        breakPointBufferDic[sender.FileName].BreakPointList.Remove(line);
                    }
                }
                else
                {
                    BreakPointBuf buf = new BreakPointBuf(sender.FileName, Path.GetFileName(sender.FileName));
                    buf.BreakPointList.Add(line);
                    breakPointBufferDic.Add(sender.FileName, buf);
                }
                breakPointBufferDic[sender.FileName].SetBreakPointState(line, false);
            }
            else //mark
            {
                if (breakPointBufferDic.ContainsKey(sender.FileName))
                {
                    if (breakPointBufferDic[sender.FileName].BreakPointList.IndexOf(line) < 0)
                    {
                        breakPointBufferDic[sender.FileName].BreakPointList.Add(line);
                    }
                }
                else
                {
                    BreakPointBuf buf = new BreakPointBuf(sender.FileName, Path.GetFileName(sender.FileName));
                    buf.BreakPointList.Add(line);    
                    breakPointBufferDic.Add(sender.FileName, buf);
                }
                breakPointBufferDic[sender.FileName].SetBreakPointState(line, true);
            }
        }

        public void GetBreakPointMark(ITabbedDocument[] documents)
        {
            breakPointInfoDic.Clear();
            breakPointBufferDic.Clear();

            foreach (ITabbedDocument docment in documents)
            {
                ScintillaControl sci = docment.SciControl;
                string filefullapth = sci.FileName;
                List<Int32> lines = GetMarkers(sci);

                BreakPointBuf buf = new BreakPointBuf(filefullapth, Path.GetFileName(filefullapth));
                buf.BreakPointList = lines;
                breakPointBufferDic.Add(filefullapth, buf);
                foreach (int key in lines)
                {
                    int line = key + 1;
                    breakPointBufferDic[filefullapth].SetBreakPointState(line, true);
                }
            }
        }
        private List<Int32> GetMarkers(ScintillaControl sci)
        {
            Int32 line = 0;
            Int32 maxLine = 0;
            List<Int32> markerLines = new List<Int32>();
            while (true)
            {
                int i= sci.MarkerNext(line, GetMarkerMask(1));
                if ((sci.MarkerNext(line, GetMarkerMask(1)) == -1) || (line > sci.LineCount)) break;
                line = sci.MarkerNext(line, GetMarkerMask(1));
                markerLines.Add(line);
                maxLine = Math.Max(maxLine, line);
                line++;
            }
            return markerLines;
        }

        private static Int32 GetMarkerMask(Int32 marker)
        {
            return 1 << marker;
        }

        private bool IsException(List<string> buflist)
        {
            if (reException.IsMatch(buflist[0]))
            {
                currentfdbState = FdbState.EXCEPTION;
                Match m;
                if ((m = reExceptionInfo.Match(bufferlist[1])).Success)
                {
                    string func = m.Groups["function"].Value;
                    if (currentFileInfo.function != func)
                    {
                    }
                    currentFileInfo.function = func;
                    currentFileInfo.filename = m.Groups["filename"].Value;
                    currentFileInfo.line = int.Parse(m.Groups["line"].Value);
                }
                cmd_InfoStack();
                waitCommandFinish();
                return true;
            }
            return false;
        }
#endif

        private bool IsRootPackage(string filefullpath)
        {
            string dir = Path.GetDirectoryName(filefullpath);
            if (dir == rootdir)
            {
                return true;
            }
            return false;
        }

        public void Start()
        {
            isprepause = false;
            isunload = false;
            currentfdbState = FdbState.INIT;       
            ProcessStart(outputfilefullpath);

            while (true)
            {
                if (currentfdbState == FdbState.INIT)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                else
                    break;
            }

            cmd_ShowDirectories();
            waitCommandFinish();

            if (Path.GetExtension(CompileTargetFileFullPath).ToLower() == ".mxml")
            {
                currentfdbState = FdbState.INIT;
                process.StandardInput.WriteLine("c");
                while (true)
                {
                    if (currentfdbState == FdbState.INIT)
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                    else
                        break;
                }      
            }

            int breakpointcont = 0;
            foreach (string filefullpath in breakPointBufferDic.Keys)
            {
                breakpointcont += breakPointBufferDic[filefullpath].BreakPointList.Count;
            }
            if (breakpointcont == 0)
            {
                //set dummy breakpoint 
                issomecmd = 1;
                cmd_Break("", "", -1);
            }
            else
            {
                cmd_Break();
            }
            waitSomeCommandFinish();
            
            cmd_Countnue();
            waitCommandFinish();
            
            //PreLoad
            if (currentfdbState == FdbState.PRELOAD)
            {
                if (breakpointcont > 0)
                {
                    breakPointInfoDic.Clear();

                    cmd_Delete();
                    waitCommandFinish();

                    cmd_Break();
                    waitSomeCommandFinish();
                }
                cmd_Countnue();
                waitCommandFinish();
            }

            breakPointBufferDic.Clear();

            if (currentfdbState == FdbState.EXCEPTION)
            {
                string fullpath = currentFileInfo.filefullpath;
                cmd_InfoStack();
                waitCommandFinish();
                bool isjump = currentFileInfo.filefullpath == fullpath ? false : true;
                FdbMsg msg = new FdbMsg();
                msg.SetParam(currentFileInfo.filefullpath, currentFileInfo.filename, currentFileInfo.line, isjump);
                if (ContinueEvent != null)
                    ContinueEvent(this, msg);
            }
        }

        public void Cleanup()
        {
            isunload = true;
            isprocess = false;
            currentfdbState = FdbState.UNLOAD;
            try
            {
                if (process != null && !process.HasExited) process.Kill();
            }
            catch { }
        }

        public void Stop(string ProcessName, bool CheckStop)
        {
            switch (currentfdbState)
            {
                case FdbState.CONTINUE:
                    Util.CloseWindow(ProcessName, CheckStop);
                    break;
                case FdbState.STEP:
                case FdbState.NEXT:
                    if (isprocess)
                        Util.CloseWindow(ProcessName, CheckStop);
                    else
                        this.ProcessStop(ProcessName, CheckStop);
                    break;
                case FdbState.PAUSE:
                    Util.CloseWindow(ProcessName, CheckStop);
                    break;
                case FdbState.EXCEPTION:
                default:
                    this.ProcessStop(ProcessName, CheckStop);
                    break;
            }
        }
        
        private void ProcessStop(string ProcessName, bool CheckStop)
        {
            DialogResult res;
            if (!CheckStop)
                res = DialogResult.OK;
            else
                res = MessageBox.Show(string.Format("Close {0} ?", ProcessName), "", MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {
                waitCommandFinish();
                process.StandardInput.Close();
                process.WaitForExit();
            }
        }

        private void ProcessStop()
        {
            waitCommandFinish();
            process.StandardInput.Close();
            process.WaitForExit();
        }

        public void Continue()
        {
            isprepause = false;
            string function = currentFileInfo.function;

            cmd_Break();
            waitSomeCommandFinish();
            cmd_Countnue();
            waitCommandFinish();
            breakPointBufferDic.Clear();
            if (currentfdbState == FdbState.EXCEPTION)
            {
                string fullpath = currentFileInfo.filefullpath;
                cmd_InfoStack();
                waitCommandFinish();
                bool isjump = (currentFileInfo.filefullpath != fullpath || currentFileInfo.function != function) ? true : false;
                FdbMsg msg = new FdbMsg();
                msg.SetParam(currentFileInfo.filefullpath, currentFileInfo.filename, currentFileInfo.line, isjump);
                if (ContinueEvent != null)
                    ContinueEvent(this, msg);
            }
        }

        public void ExceptionContinue()
        {
            isprepause = false;
            string function = currentFileInfo.function;

            cmd_Break();
            waitSomeCommandFinish();

            currentfdbState = FdbState.CONTINUE;
            writeStartCommand("continue");
            writeCommand("continue");
            if (ExceptionEvent != null)
                ExceptionEvent(this);
            writeEndCommand("continue");
            waitCommandFinish();

            breakPointBufferDic.Clear();
            if (currentfdbState == FdbState.EXCEPTION)
            {
                string fullpath = currentFileInfo.filefullpath;
                cmd_InfoStack();
                waitCommandFinish();
                bool isjump = (currentFileInfo.filefullpath != fullpath || currentFileInfo.function != function) ? true : false;
                FdbMsg msg = new FdbMsg();
                msg.SetParam(currentFileInfo.filefullpath, currentFileInfo.filename, currentFileInfo.line, isjump);
                if (ContinueEvent != null)
                    ContinueEvent(this, msg);
            }
        }

        public void Step()
        {
            bool move = false;
            string function = currentFileInfo.function;

            currentfdbState = FdbState.STEP;
            cmd_Step();
            waitCommandFinish();

            if (currentfdbState == FdbState.UNLOAD)
            {
                return;
            }
            else if (currentfdbState == FdbState.EXCEPTION)
            {
                string fullpath = currentFileInfo.filefullpath;
                cmd_InfoStack();
                waitCommandFinish();
                move = (currentFileInfo.filefullpath != fullpath || currentFileInfo.function != function) ? true : false;
            }
            else
            {
                switch (currentStepState)
                {
                    case StepState.STEP_IN_INST:
                        move = true;
                        cmd_CheckPackage();
                        waitCommandFinish();
                        if (!IsRootPackage(currentFileInfo.filefullpath))
                        {
                            cmd_Non("step");
                        }
                        cmd_Step();
                        waitCommandFinish();
                        if (currentStepState == StepState.STEP_INOUT)
                        {
                            cmd_InfoStack();
                            waitCommandFinish();
                        }
                        break;
                    case StepState.STEP_INOUT:
                        move = true;
                        cmd_InfoStack();
                        waitCommandFinish();
                        break;
                    case StepState.STEP_BREAK_JUMP:
                        move = true;
                        break;
                    case StepState.STEP_BREAK:
                        move = false;
                        break;
                    case StepState.STEP:
                        break;
                }
            }

            FdbMsg msg = new FdbMsg();
            msg.SetParam(currentFileInfo.filefullpath, currentFileInfo.filename, currentFileInfo.line, move);
            if (ContinueEvent != null)
                ContinueEvent(this, msg);
        }

        public void Next()
        {
            string function = currentFileInfo.function;
            int line = currentFileInfo.line;
            currentfdbState = FdbState.NEXT;
            cmd_Next();
            waitCommandFinish();
            bool move = false;

            if (currentfdbState == FdbState.EXCEPTION)
            {
                string filename = currentFileInfo.filename;
                string fullpath = currentFileInfo.filefullpath;
                
                cmd_InfoStack();
                waitCommandFinish();
                if (currentFileInfo.filename == "<null>")
                {
                    currentFileInfo.filename = filename;
                    currentFileInfo.filefullpath = fullpath;
                    currentFileInfo.line = line;
                }
                move = (currentFileInfo.filefullpath != fullpath || currentFileInfo.function != function) ? true : false;
            }
            else
            {
                switch (currentStepState)
                {
                    case StepState.STEP_INOUT:
                        move = true;
                        cmd_InfoStack();
                        waitCommandFinish();
                        break;
                    case StepState.STEP_BREAK_JUMP:
                        move = true;
                        break;
                    case StepState.STEP_BREAK:
                        move = false;
                        break;
                    case StepState.STEP:
                        break;
                }
            }
            FdbMsg msg = new FdbMsg();
            msg.SetParam(currentFileInfo.filefullpath, currentFileInfo.filename, currentFileInfo.line, move);
            if (ContinueEvent != null)
                ContinueEvent(this, msg);
        }

        public void InfoLocals()
        {
            waitCommandFinish();
            switch (currentfdbState)
            {
                case FdbState.START:
                    cmd_Countnue();
                    break;
                case FdbState.UNLOAD:
                case FdbState.PAUSE:
                    return;
                default:
                    cmd_InfoLocals();
                    break;
            }
        }

        public void Print(string valname, PrintType printtype)
        {
            cmd_Print(valname, printtype);
        }

        public void Finish()
        {
            cmd_Finish();
            cmd_InfoStack();
            waitCommandFinish();
            FdbMsg msg = new FdbMsg();
            msg.SetParam(currentFileInfo.filefullpath, currentFileInfo.filename, currentFileInfo.line, true);
            if (ContinueEvent != null)
                ContinueEvent(this, msg);
        }

        public void DeleteAllBreakPoints()
        {
            breakPointInfoDic.Clear();
            breakPointBufferDic.Clear();
            cmd_Delete();
            waitCommandFinish();
        }

        private void ProcessStart(string outputfile)
        {
            process = new Process();
            process.StartInfo.FileName = fdbexefile;
            process.StartInfo.Arguments = '"' + outputfile + '"';
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            process.Exited += new EventHandler(process_Exited);
            process.Start();
            process.BeginOutputReadLine();
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            if (VERBOSE && TraceEvent != null) TraceEvent(this, e.Data);

            if (e.Data == checkstart)
            {
                outputstart = true;
                outputend = false;
                bufferlist.Clear();
            }
            else if (execdic.ContainsKey(e.Data))
            {
                outputstart = false;
                outputend = true;
                execdic[e.Data]();
                isprocess = false;
            }
            else
            {
                if (outputstart)
                {
                    if (reTrace.IsMatch(e.Data))
                    {
                        if (TraceEvent != null)
                            TraceEvent(this, e.Data);
                    }
                    else if (reNotRespond.IsMatch(e.Data))
                    {
                        if (currentfdbState == FdbState.PAUSE)
                        {
                            if (PuaseNotRespondEvent != null)
                                PuaseNotRespondEvent(this);
                        }
                    }
                    else if (e.Data == "Player session terminated"
                        && (currentfdbState == FdbState.PAUSE
                            || currentfdbState == FdbState.UNLOAD))
                    {
                        //if (currentfdbState == FdbState.PAUSE
                        //    || currentfdbState == FdbState.UNLOAD)
                        //{
                            Thread.Sleep(50);
                            process.StandardInput.WriteLine("quit");
                       // }
                    }
                    else if (rePause.IsMatch(e.Data))
                    {
                        isprepause = true;
                        outputstart = false;
                        outputend = true;
                        exit_Pause();
                    }
                    else
                    {
                        bufferlist.Add(e.Data);
                        outputend = true;
                    }
                }
                else if (e.Data == "Set additional breakpoints as desired, and then type 'continue'.")
                {
                    currentfdbState = FdbState.START;
                }
                else
                {
                    if (regfdbStart.IsMatch(e.Data))
                    {
                        isDebugStart = true;
                        currentfdbState = FdbState.START;
                    }
                }
            }
        }

        void process_Exited(object sender, EventArgs e)
        {
            try
            {
                process.Close();
            }
            finally
            {
                process.Dispose();
                breakPointInfoDic.Clear();
                breakPointBufferDic.Clear();

                outputstart = false;
                outputend = true;
                isprocess = false;

                isDebugStart = false;
                currentfdbState = FdbState.UNLOAD;

                if (StopEvent != null)
                    StopEvent(this);
            }
        }

        private void waitCommandFinish()
        {
            while (true)
            {
                if (isprocess && currentfdbState != FdbState.UNLOAD)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                else
                    break;
            }
        }

        private void waitSomeCommandFinish()
        {
            while (true)
            {
                if (issomecmd > 0)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                else
                {
                    isprocess = false;
                    break;
                }
            }
        }
        
        private string getUnknownCommandOutput(string cmd)
        {
            return string.Format("(fdb) Unknown command '{0}', ignoring it", cmd);
        }

        private string getstartcmd(string cmd)
        {
            string command = cmd.Trim();
            return command.Replace(" ", "_") + "_start";
        }
        private string getendtcmd(string cmd)
        {
            string command = cmd.Trim();
            return command.Replace(" ", "_") + "_end";
        }

        private void writeStartCommand(string cmd)
        {
            waitCommandFinish();

            isprocess = true;
            string start = getstartcmd(cmd);
            string end = getendtcmd(cmd);
            checkstart = getUnknownCommandOutput(start);
            checkend = getUnknownCommandOutput(end);

            process.StandardInput.WriteLine(start);
        }

        private void writeCommand(string cmd)
        {
            while (true)
            {
                if (!outputstart)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                else
                    break;
            }

            if (VERBOSE && TraceEvent != null) TraceEvent(this, "-> " + cmd);
            process.StandardInput.WriteLine(cmd);
        }

        private void writeEndCommand(string cmd)
        {
            while (true)
            {
                if (!outputend && isprocess)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                else
                    break;
            }

            if (isprepause && !isunload)
            {
                isprepause = false;
                return;
            }

            string end = getendtcmd(cmd);
            process.StandardInput.WriteLine(end);
        }

        private void inputCommad(string cmd)
        {
            inputCommad(cmd, cmd);
        }

        private void inputCommad(string cmd, string start_endCmd)
        {
            if (currentfdbState == FdbState.UNLOAD)
            {
                return;
            }

            writeStartCommand(start_endCmd);
            writeCommand(cmd);
            writeEndCommand(start_endCmd);
        }

        private void inputCommad2(string cmd, string start_endCmd)
        {
            waitCommandFinish();
            isprocess = true;
            string start = getstartcmd(start_endCmd);
            string end = getendtcmd(start_endCmd);
            checkstart = getUnknownCommandOutput(start);
            checkend = "(fdb) " + getUnknownCommandOutput(end);
            process.StandardInput.WriteLine(start);

            writeCommand(cmd);
            while (true)
            {
                if (!outputstart)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                else
                    break;
            }
            process.StandardInput.WriteLine(end);
        }

        private void cmd_Break(string filefullpath, string classpath, int line)
        {
            waitCommandFinish();

            filefullpathQueue.Enqueue(filefullpath);
            string pack = filefullpath.Substring(classpath.Length, filefullpath.Length - classpath.Length);
            pack = pack.TrimStart(chTrimsStart);
            pack = pack.Replace(@"\",".");
            inputCommad(string.Format("break {0}:{1}", pack, line), "break");
        }
        private void cmd_Break()
        {
            filefullpathQueue.Clear();
            filefullpathQueue.TrimExcess();

            string path = string.Empty;
            foreach (string filefullpath in breakPointBufferDic.Keys)
            {
                path = classpathlist.Find(delegate(string classpath)
                {
                    return filefullpath.Contains(classpath);
                });
                issomecmd += breakPointBufferDic[filefullpath].BreakPointStateDic.Count;
                foreach (int line in breakPointBufferDic[filefullpath].BreakPointStateDic.Keys)
                {
                    bool ismark = breakPointBufferDic[filefullpath].BreakPointStateDic[line];
                    if (ismark)
                    {
                        cmd_Break(filefullpath, path, line);
                    }
                    else
                    {
                        cmd_ClearBreakPoint(filefullpath, path, line);
                    }
                }
            }
        }
        private void exit_Break()
        {
            if (filefullpathQueue.Count == 0)
            {
                throw new ArgumentException();
            }
            string buf = bufferlist[0];
            string filefullpath = filefullpathQueue.Dequeue();
            Match m = null;
            if ((m = reBreak.Match(buf)).Success)
            {
                string breakpoint = m.Groups["breakpoint"].Value;
                string filename = m.Groups["filename"].Value;
                int line = int.Parse(m.Groups["line"].Value);
                if(!breakPointInfoDic.ContainsKey(breakpoint))
                {
                    breakPointInfoDic.Add(breakpoint, new BreakPointInfo(filefullpath, filename, breakpoint, line));
                }
            }
            issomecmd--;
        }

        private void cmd_ClearBreakPoint(string filefullpath, string classpath, int line)
        {
            waitCommandFinish();
            string pack = filefullpath.Substring(classpath.Length, filefullpath.Length - classpath.Length);
            pack = pack.TrimStart(chTrimsStart);
            pack = pack.Replace(@"\", ".");
            inputCommad2(string.Format("clear {0}:{1}", pack, line), "clearbreakpoint");
        }
        private void exit_ClearBreakPoint()
        {
            issomecmd--;
        }
        private void cmd_Countnue()
        {
            currentfdbState = FdbState.CONTINUE;
            inputCommad("continue");
        }
        private bool isunload = false;
        private void exit_Continue()
        {
            int i;
            int j;
            string buf = bufferlist[0];
            Match m = null;

            //check UnloadSWF
            m = reUnloadSWF.Match(buf);
            if (m.Success)
            {
                currentfdbState = FdbState.UNLOAD;
                isunload = true;
                isprocess = false;
                ProcessStop();
                return;
            }

            m = reException.Match(buf);
            if (m.Success)
            {
                currentfdbState = FdbState.EXCEPTION;
                if ((m = reExceptionInfo.Match(bufferlist[1])).Success)
                {
                    currentFileInfo.function = m.Groups["function"].Value;
                    currentFileInfo.filename = m.Groups["filename"].Value;
                    currentFileInfo.line = int.Parse(m.Groups["line"].Value);
                }
                foreach (string s in bufferlist)
                {
                    TraceEvent(this, s);
                }
            }
            else
            {
                if (rePreLoad.IsMatch(buf))
                {
                    currentfdbState = FdbState.PRELOAD;
                }
                else
                {
                    m = reContinue.Match(buf);
                    if (!m.Success)
                    {
                        m = reContinue2.Match(buf);
                    }
                    if (m.Success)
                    {
                        string breakpoint = m.Groups["breakpoint"].Value;
                        if (breakPointInfoDic.ContainsKey(breakpoint))
                        {
                            currentfdbState = FdbState.BREAK;
                            BreakPointInfo info = breakPointInfoDic[breakpoint];
                            bool isjump = false;
                            string function = m.Groups["function"].Value;
                            if (currentFileInfo.function != function
                                || currentFileInfo.filefullpath != info.FileFullPath)
                            {
                                isjump = true;
                            }
                            currentFileInfo.SetParam(info.FileFullPath, info.Filename, function, info.BreakPoinLine);

                            FdbMsg msg = new FdbMsg();
                            msg.SetParam(info.FileFullPath, info.Filename, info.BreakPoinLine, isjump);
                            if (ContinueEvent != null)
                                ContinueEvent(this, msg);
                        }
                    }
                }
            }
        }

        private bool CheckUnloadSWF(List<string> datalist)
        {
            foreach (string data in datalist)
            {
                if (reUnloadSWF.IsMatch(data))
                {
                    return true;
                }
            }
            return false;
        }

        private void cmd_Step()
        {
            currentfdbState = FdbState.STEP;
            inputCommad("step");
        }
        private void exit_Step()
        {
            if (CheckUnloadSWF(bufferlist))
            {
                currentfdbState = FdbState.UNLOAD;
                isunload = true;
                isprocess = false;
                ProcessStop();
                return;
            }

            string buf = bufferlist[0].Replace("(fdb)", "");
            buf = buf.Trim();
            Match m = null;

            if ((m = reUnloadSWF.Match(buf)).Success)
            {
                currentfdbState = FdbState.UNLOAD;
                isunload = true;
                isprocess = false;
                ProcessStop();
                return;
            }

            if ((m = reException.Match(buf)).Success)
            {
                currentfdbState = FdbState.EXCEPTION;
                if (bufferlist.Count == 2)
                {
                    if ((m = reExceptionline.Match(bufferlist[1])).Success)
                    {
                        currentFileInfo.line = int.Parse(m.Groups["line"].Value.Trim());
                    }
                }
                else if (bufferlist.Count == 3)
                {
                    if ((m = reExceptionInfo.Match(bufferlist[1])).Success)
                    {
                        currentFileInfo.function = m.Groups["function"].Value;
                        currentFileInfo.filename = m.Groups["filename"].Value;
                        currentFileInfo.line = int.Parse(m.Groups["line"].Value);
                    }
                }

                foreach (string s in bufferlist)
                {
                    TraceEvent(this, s);
                }
            }
            else if ((m = reStepExecutionhaltedFunction.Match(buf)).Success)
            {
                currentFileInfo.filename = m.Groups["filename"].Value;
                currentFileInfo.function = m.Groups["function"].Value;
                currentFileInfo.line = int.Parse(m.Groups["line"].Value);
                if (currentFileInfo.function == "global$init()")
                    currentStepState = StepState.STEP_IN_INST;
                else
                    currentStepState = StepState.STEP_INOUT;
            }
            else if ((m = reStepExecutionhalted.Match(buf)).Success)
            {
                string filename = m.Groups["filename"].Value.Trim();
                int line = int.Parse(m.Groups["line"].Value.Trim());
                currentFileInfo.line = line;
                if (filename != currentFileInfo.filename)
                {
                    currentFileInfo.filename = filename;
                    currentStepState = StepState.STEP_INOUT;
                }
                else
                    currentStepState = StepState.STEP;
            }
            else if ((m = reStepBreakPoint.Match(buf)).Success)
            {
                string breakpoint = m.Groups["breakpoint"].Value;
                BreakPointInfo info = breakPointInfoDic[breakpoint];
                if (breakPointInfoDic.ContainsKey(breakpoint))
                {
                    if (currentFileInfo.function != m.Groups["fuction"].Value
                        || currentFileInfo.filefullpath != info.FileFullPath)
                    {
                        currentStepState = StepState.STEP_BREAK_JUMP;
                    }
                    else
                    {
                        currentStepState = StepState.STEP_BREAK;
                    }
                    currentFileInfo.SetParam(info.FileFullPath, info.Filename, m.Groups["function"].Value, info.BreakPoinLine);
                }
            }
            else if ((m = reStep.Match(buf)).Success)
            {
                int line = int.Parse(m.Groups["line"].Value.Trim());
                currentFileInfo.line = line;
                currentStepState = StepState.STEP;
            }
        }
        
        private void cmd_Non(string cmd)
        {
            inputCommad(cmd, "NonCmd");
        }
        private void exit_Non()
        {
        }

        private string objname_msg;
        private PrintType ptype;
        private void cmd_Print(string objname, PrintType printtype)
        {
            waitCommandFinish();
            objname_msg = objname;
            ptype = printtype;
            inputCommad(string.Format("print {0}", objname), "print");
        }
        private void exit_Print()
        {
            string buf = bufferlist[0];
            Match m = null;
            if ((m = rePrintValUnknown.Match(buf)).Success) return;

            if (bufferlist.Count == 1)
            {
                if ((m = rePrintObject.Match(buf)).Success)
                {
                    //unknown
                    bufferlist[0] = string.Format("{0} = {1}", objname_msg, "unknown");
                }
                else if ((m = rePrintVal.Match(buf)).Success)
                {
                    bufferlist[0] = string.Format("{0} = {1}", objname_msg, m.Groups["value"].Value);
                }
            }
            else
            {
                if ((m = rePrintObject.Match(buf)).Success)
                {
                    bufferlist.RemoveAt(0);
                }
            }
            PrintArg arg = new PrintArg(objname_msg, ptype, bufferlist);
            if (PrintEvent != null)
                PrintEvent(this, arg);
        }

        private void cmd_InfoLocals()
        {
            inputCommad("info locals");
        }
        private void exit_infolocals()
        {
            Match m = null;
            string buf = bufferlist[0];
            if ((m = reFirstPrintOutput.Match(buf)).Success)
            {
                bufferlist[0] = string.Format("{0} = {1}", m.Groups["name"].Value, m.Groups["value"].Value);
            }

            FdbMsg msg = new FdbMsg();
            msg.output = new List<string>(bufferlist);
            if (LocalValuesEvent != null)
                LocalValuesEvent(this, msg);
        }

        private void cmd_CheckPackage()
        {
            inputCommad("step", "step_checkpackage");
        }
        private void exit_CheckPackage()
        {
            string fullpath = string.Empty;
            string buf = bufferlist[0];
            if (rePackage.IsMatch(buf))
            {
                Match m = rePackage.Match(buf);
                string package = m.Groups["path"].Value;
                package = package.Trim();
                package = package.Replace(@".", @"\");

                fullpath = Path.Combine(Path.Combine(rootdir, package), currentFileInfo.filename);
            }
            else
            {
                fullpath = Path.Combine(rootdir, currentFileInfo.filename);
            }
            currentFileInfo.filefullpath = fullpath;
            currentFileInfo.filename = Path.GetFileName(fullpath);
        }

        private void cmd_InfoStack()
        {
            inputCommad("info stack");
        }
        private void exit_InfoStack()
        {
            string buf = bufferlist[0];
            Match m;
            if ((m=reStack.Match(buf)).Success)
            {
                string filename, fullpath;

                Match mfilename = reStackFilename.Match(buf);
                if (mfilename.Success)
                {
                    filename = mfilename.Groups["filename"].Value;
                    if (filename == "<null>")
                    {
                        currentFileInfo.filename = filename;
                        return;
                    }
                    Match mpath = reStackPath.Match(m.Groups[0].Value);
                    string path;
                    //string path = mpath.Groups["path"].Value;
                    //if (path != string.Empty)
                    if (mpath.Success)
                    {
                        path = mpath.Groups["path"].Value;
                        fullpath = Path.Combine(path.Replace(".", @"\"), filename);
                    }
                    else
                    {
                        if ((mpath = reStackPath.Match(m.Groups[0].Value)).Success)
                        {
                            mpath = reStackPathFrame.Match(m.Groups[0].Value);
                            path = mpath.Groups["path"].Value;
                            fullpath = path.Replace("_Main_", "");
                            fullpath = fullpath.Replace("_", @"\") + ".as"; // TODO: investigate .as
                        }
                        else
                        {
                            //(fdb) #0   this = [Object 21086369, class='LabsArticleSample'].LabsArticleSample/btnClick(obj=[Object 21328033, class='mx.controls::Button']) at eventHandlers.as:19
                            mpath = reStackFilename.Match(buf);
                            fullpath = mpath.Groups["filename"].Value;
                        }
                    }

                    foreach (string classpath in classpathlist)
                    {
                        if (File.Exists(Path.Combine(classpath, fullpath)))
                        {
                            fullpath = Path.Combine(classpath, fullpath);
                            break;
                        }
                    }
                    currentFileInfo.filefullpath = fullpath;
                    currentFileInfo.filename = Path.GetFileName(fullpath);
                }
            }
        }

        private void cmd_Next()
        {
            inputCommad("next");
        }
        private void exit_Next()
        {
            exit_Step();
        }

        private void cmd_Finish()
        {
            inputCommad("finish");
        }
        private void exit_Finish()
        {
            string buf = bufferlist[0];
            Match m;
            if ((m = reStepExecutionhalted.Match(buf)).Success)
            {
                currentFileInfo.filename = m.Groups["filename"].Value;
                currentFileInfo.function = m.Groups["function"].Value;
                currentFileInfo.line = int.Parse(m.Groups["line"].Value);     
            }
        }

        private bool isprepause = false;
        public void Pause()
        {
            isprepause = true;
            cmd_Pause();
        }
        private void cmd_Pause()
        {
            currentfdbState = FdbState.PAUSE;
            process.StandardInput.WriteLine("dummy_cmd");
            process.StandardInput.WriteLine("y");
        }
        private void exit_Pause()
        {
            currentfdbState = FdbState.PAUSE_SET_BREAKPOINT;
            if (PauseEvent != null)
                PauseEvent(this);
            isprocess = false;
        }

        private void cmd_ShowDirectories()
        {
            inputCommad("show directories");
        }
        private void exit_ShowDirectories()
        {
            //Source directories searched:
            //C:\flex_sdk_3\frameworks\projects\airframework\src
            bufferlist.RemoveAt(0);
            bufferlist.ForEach(delegate(string path)
            {
                classpathlist.Add(path);
            });
        }

        private void cmd_Delete()
        {
            process.StandardInput.WriteLine("delete");
            process.StandardInput.WriteLine("y");
            string end = getendtcmd("delete");
            process.StandardInput.WriteLine(end);
        }
        private void exit_Delete()
        {
        }
    }
}
