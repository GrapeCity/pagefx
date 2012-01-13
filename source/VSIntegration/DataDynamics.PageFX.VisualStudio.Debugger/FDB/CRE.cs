using System.Text.RegularExpressions;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    /// <summary>
    /// Contains common regular expressions used to parse fdb output.
    /// </summary>
    static class CRE
    {
        //start
        //[SWF] D:\ActionScrip\Test0\bin\Test0.swf - 2,931 bytes after decompression
        public static readonly Regex FdbStart = new Regex(@"^\[SWF\].*?(bytes after decompression)", RegexOptions.Compiled);

        //Breakpoint 1, Test0$iinit() at Test0.as:17
        //Breakpoint 2, getOra() at Test1.as:19
        public static readonly Regex Bbreakpoint = new Regex(@"Breakpoint (?<bp>.*).*?,(?<function>.*).*?( at )", RegexOptions.Compiled);

        //(fdb) Breakpoint 1, Tweener_Test.as:25
        public static readonly Regex Breakpoint2 = new Regex(@"Breakpoint (?<bp>.*).*?, (?<file>.*).*?:(?<line>.*)", RegexOptions.Compiled);

        //end
        //(fdb) [UnloadSWF] D:\desktop\src\ActionScrip\Test0\bin\Test0.swf
        //Player session terminated
        public static readonly Regex UnloadSWF = new Regex(@"^?\[UnloadSWF\]", RegexOptions.Compiled);

        //(fdb) [trace] ....
        public static readonly Regex TraceMessage = new Regex(@"^?\[trace\]\s(?<msg>.*)", RegexOptions.Compiled);

        //(fdb) [Fault] ....
        public static readonly Regex FaultMessage = new Regex(@"^?\[Fault\]\s(?<msg>.*)", RegexOptions.Compiled);

        //(fdb) Additional ActionScript code has been loaded from a SWF or a frame.
        public static readonly Regex AdditionalActionScriptLoaded = new Regex("Additional ActionScript code has been loaded.*");

        //step
        //(fdb) Player did not respond to the command as expected; command aborted.
        public static readonly Regex NotRespond = new Regex(@"Player did not respond to the command as expected; command aborted\.", RegexOptions.Compiled);

        public static readonly Regex UnknownCommand = new Regex(@".*Unknown command '(?<cmd>.*)', ignoring it");

        public static readonly Regex Value =
            new Regex(@"(\$\d+ = )?(?<name>.*)(\s=\s)(?<value>.*)",
                RegexOptions.Compiled);

        //e = [Object 123909, class='']
        public static readonly Regex ValueObject
            = new Regex(@"\[(?<addr>Object\s+\d+), class\s*=\s*'(?<type>.*)'\]",
                RegexOptions.Compiled);

        //f = [Function 21350241, name='MethodClosure']
        public static readonly Regex ValueFunction
            = new Regex(@"\[(?<addr>Function\s+\d+), .*\]");

        //f = [Setter 21350241]
        public static readonly Regex ValueSetter
            = new Regex(@"\[(?<addr>Setter\s+\d+).*\]");

        //f = [Getter 21350241]
        public static readonly Regex ValueGetter
            = new Regex(@"\[(?<addr>Getter\s+\d+).*\]");

        //a = 10 (0x0A)
        public static readonly Regex ValueNumber
            = new Regex(@"(?<dec>\d+(.?\d*)?)\s*(\(0x(?<hex>[\da-fA-F]+)\))?",
                RegexOptions.Compiled | RegexOptions.Singleline);

        //s = "asdasd"
        public static readonly Regex ValueString
            = new Regex("\"(?<str>.*)\"",
                RegexOptions.Compiled);
        

        //15                     _punch = "tmp2Test2";
        public static readonly Regex Step = new Regex(@"(?<line>\d+).*", RegexOptions.IgnoreCase);

        //Execution halted, global$init() at Test2.as:8
        //8      public class Test2
        //Execution halted, getOra() at Test1.as:17
        //17     public function getOra():String
        //Execution halted, Test0() at Test0.as:46
        //46     stt2 = new tmp1.Test2();
        public static readonly Regex ExecutionHalted = new Regex(@"Execution halted.*", RegexOptions.Compiled);

        //[Fault] exception, information=Error: my error
        //Fault, dy() at Test2.as:21
        // 21                     throw new Error("my error");
        public static readonly Regex FaultHalted = new Regex(@"Fault, .*\(.*\) at .*", RegexOptions.Compiled);
    }
}