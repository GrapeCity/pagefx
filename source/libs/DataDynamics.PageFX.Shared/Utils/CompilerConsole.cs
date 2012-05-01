using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#region csc /help
/*
Microsoft (R) Visual C# 2005 Compiler version 8.00.50727.42
for Microsoft (R) Windows (R) 2005 Framework version 2.0.50727
Copyright (C) Microsoft Corporation 2001-2005. All rights reserved.

                      Visual C# 2005 Compiler Options

                        - OUTPUT FILES -
/out:<file>                   Specify output file name (default: base name of file with main class or first file)
/target:exe                   Build a console executable (default) (Short form: /t:exe)
/target:winexe                Build a Windows executable (Short form: /t:winexe)
/target:library               Build a library (Short form: /t:library)
/target:module                Build a module that can be added to another assembly (Short form: /t:module)
/delaysign[+|-]               Delay-sign the assembly using only the public portion of the strong name key
/doc:<file>                   XML Documentation file to generate
/keyfile:<file>               Specify a strong name key file
/keycontainer:<string>        Specify a strong name key container
/platform:<string>            Limit which platforms this code can run on: x86, Itanium, x64, or anycpu. The default is anycpu.

                        - INPUT FILES -
/recurse:<wildcard>           Include all files in the current directory and subdirectories according to the wildcard specifications
/reference:<alias>=<file>     Reference metadata from the specified assembly file using the given alias (Short form: /r)
/reference:<file list>        Reference metadata from the specified assembly files (Short form: /r)
/addmodule:<file list>        Link the specified modules into this assembly

                        - RESOURCES -
/win32res:<file>              Specify a Win32 resource file (.res)
/win32icon:<file>             Use this icon for the output
/resource:<resinfo>           Embed the specified resource (Short form: /res)
/linkresource:<resinfo>       Link the specified resource to this assembly (Short form: /linkres)
                              Where the resinfo format is <file>[,<string name>[,public|private]]

                        - CODE GENERATION -
/debug[+|-]                   Emit debugging information
/debug:{full|pdbonly}         Specify debugging type ('full' is default, and enables attaching a debugger to a running program)
/optimize[+|-]                Enable optimizations (Short form: /o)

                        - ERRORS AND WARNINGS -
/warnaserror[+|-]             Report all warnings as errors
/warnaserror[+|-]:<warn list> Report specific warnings as errors
/warn:<n>                     Set warning level (0-4) (Short form: /w)
/nowarn:<warn list>           Disable specific warning messages

                        - LANGUAGE -
/checked[+|-]                 Generate overflow checks
/unsafe[+|-]                  Allow 'unsafe' code
/define:<symbol list>         Define conditional compilation symbol(s) (Short form: /d)
/langversion:<string>         Specify language version mode: ISO-1 or Default

                        - MISCELLANEOUS -
@<file>                       Read response file for more options
/help                         Display this usage message (Short form: /?)
/nologo                       Suppress compiler copyright message
/noconfig                     Do not auto include CSC.RSP file

                        - ADVANCED -
/baseaddress:<address>        Base address for the library to be built
/bugreport:<file>             Create a 'Bug Report' file.
/codepage:<n>                 Specify the codepage to use when opening source files
/utf8output                   Output compiler messages in UTF-8 encoding
/main:<type>                  Specify the type that contains the entry point (ignore all other possible entry points) (Short form: /m)
/fullpaths                    Compiler generates fully qualified paths
/filealign:<n>                Specify the alignment used for output file sections
/pdb:<file>                   Specify debug information file name (default: output file name with .pdb extension)
/nostdlib[+|-]                Do not reference standard library (mscorlib.dll)
/lib:<file list>              Specify additional directories to search in for references
/errorreport:<string>         Specify how to handle internal compiler errors: prompt, send, queue, or none. The default is queue.
/moduleassemblyname:<string>  Name of the assembly which this module will be a part of.
*/
#endregion

namespace DataDynamics.PageFX
{
    #region enum CompilerTarget
    public enum CompilerTarget
    {
        ConsoleApp,
        Library,
        WinApp,
        Module
    }
    #endregion

    #region enum CompilerLanguage
    public enum CompilerLanguage
    {
        CSharp,
        VB,
        JSharp,
        CIL
    }
    #endregion

    #region class CompilerOptions
    public class CompilerOptions
    {
        public CompilerOptions()
        {
            NoLogo = true;
            Debug = true;
        }

        #region Simple Options
        public CompilerLanguage Language { get; set; }

        /// <summary>
        /// /target:exe       - Build a console executable (default) (Short form: /t:exe)
        /// /target:winexe    - Build a Windows executable (Short form: /t:winexe)
        /// /target:library   - Build a library (Short form: /t:library)
        /// /target:module    - Build a module that can be added to another assembly (Short form: /t:module)
        /// </summary>
        public CompilerTarget Target { get; set; }

        /// <summary>
        /// /nologo - Suppress compiler copyright message
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// /debug[+|-]           - Emit debugging information
        /// /debug:{full|pdbonly} - Specify debugging type ('full' is default, and enables attaching a debugger to a running program)
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// /optimize[+|-] - Enable optimizations (Short form: /o)
        /// </summary>
        public bool Optimize { get; set; }

        /// <summary>
        /// /nostdlib[+|-] - Do not reference standard library (mscorlib.dll)
        /// </summary>
        public bool NoStdlib { get; set; }

        /// <summary>
        /// /noconfig  - Do not auto include CSC.RSP file
        /// </summary>
        public bool NoConfig { get; set; }

        /// <summary>
        /// /unsafe[+|-] - Allow 'unsafe' code
        /// </summary>
        public bool Unsafe { get; set; }

        /// <summary>
        /// /checked[+|-] - Generate overflow checks
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// /out:&lt;file&gt;   - Specify output file name (default: base name of file with main class or first file)
        /// </summary>
        public string Output { get; set; }

        public bool NoVBRuntime { get; set; }

        /// <summary>
        /// Sets path to alternate Visual Basic runtime.
        /// </summary>
        public string VBRuntime { get; set; }

        public bool CompactFramework { get; set; }

        public string SDKPath { get; set; }

        public bool NoWarn { get; set; }
        #endregion

        #region /define
        /// <summary>
        /// /define:&lt;symbol list&gt; - Define conditional compilation symbol(s) (Short form: /d)
        /// </summary>
        public List<string> Defines
        {
            get { return _defines; }
        }
        private readonly List<string> _defines = new List<string>();

        public void Define(string def)
        {
            _defines.Add(def);
        }
        #endregion

        #region /references
        /// <summary>
        /// /reference:&lt;alias&gt;=&lt;file&gt; - Reference metadata from the specified assembly file using the given alias (Short form: /r)
        /// /reference:&lt;file list&gt; - Reference metadata from the specified assembly files (Short form: /r)
        /// </summary>
        public List<string> References
        {
            get { return _refs; }
        }
        readonly List<string> _refs = new List<string>();

        public void AddRef(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
			if (_refs.Contains(name, StringComparer.CurrentCultureIgnoreCase)) return;
            _refs.Add(name);
        }

        public void AddRefs(params string[] refs)
        {
            if (refs != null)
            {
                foreach (var r in refs)
                    AddRef(r);
            }
        }

        public void AddRefs(IEnumerable<string> refs)
        {
            if (refs != null)
            {
                foreach (var r in refs)
                    AddRef(r);
            }
        }
        #endregion

        #region /resource, /linkresource
        /// <summary>
        /// /resource:&lt;resinfo&gt; - Embed the specified resource (Short form: /res)
        /// </summary>
        public List<string> Resources
        {
            get { return _resources; }
        }
        readonly List<string> _resources = new List<string>();

        public void AddRes(string path, string name)
        {
            _resources.Add(path + "," + name);
        }

        public void AddRes(string path)
        {
            AddRes(path, Path.GetFileName(path));
        }

        /// <summary>
        ///  /linkresource:&lt;resinfo&gt; -
        /// Link the specified resource to this assembly (Short form: /linkres)
        /// Where the resinfo format is &lt;file&gt;[,&lt;string name&gt;[,public|private]]
        /// </summary>
        public List<string> LinkResources
        {
            get { return _linkResources; }
        }
        private readonly List<string> _linkResources = new List<string>();
        #endregion

        public List<string> ResponseFiles
        {
            get { return _responseFiles; }
        }
        readonly List<string> _responseFiles = new List<string>();

        /// <summary>
        /// Include all files in the current directory and subdirectories according to the wildcard specifications
        /// </summary>
        public List<string> Recurse
        {
            get { return _recurse; }
        }
        readonly List<string> _recurse = new List<string>();

        public List<string> Input
        {
            get { return _input; }
        }
        readonly List<string> _input = new List<string>();

        bool HasChecked
        {
            get
            {
                switch (Language)
                {
                    case CompilerLanguage.VB:
                        return false;
                }
                return true;
            }
        }

        #region ToString
        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                if (NoLogo)
                    writer.Write("/nologo ");

                if (Debug)
                    writer.Write("/debug ");
                if (Optimize)
                    writer.Write("/optimize ");

                if (NoStdlib)
                    writer.Write("/nostdlib ");
                if (NoConfig)
                    writer.Write("/noconfig ");
                if (Unsafe)
                    writer.Write("/unsafe ");

                if (HasChecked)
                    WriteSignOption(writer, "checked", Checked);

                if (Language == CompilerLanguage.VB)
                {
                    if (NoWarn)
                        writer.Write("/nowarn ");

                    if (CompactFramework)
                        writer.Write("/netcf ");

                    if (NoVBRuntime)
                    {
                        WriteSignOption(writer, "vbruntime", false);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(VBRuntime))
                            writer.Write("/vbruntime:{0} ", VBRuntime);
                    }

                    if (!string.IsNullOrEmpty(SDKPath))
                        writer.Write("/sdkpath:{0} ", SDKPath);
                }

                switch (Target)
                {
                    case CompilerTarget.ConsoleApp:
                        writer.Write("/target:exe ");
                        break;

                    case CompilerTarget.Library:
                        writer.Write("/target:library ");
                        break;

                    case CompilerTarget.Module:
                        writer.Write("/target:module ");
                        break;

                    case CompilerTarget.WinApp:
                        writer.Write("/target:winexe ");
                        break;
                }

                if (Debug) writer.Write("/define:DEBUG ");
                WriteList(writer, _defines, "/define:", false);
                
                if (!string.IsNullOrEmpty(Output))
                    writer.Write("/out:{0} ", Output);

                WriteList(writer, _refs, "/r:", true);
                WriteList(writer, _resources, "/res:", true);
                WriteList(writer, _linkResources, "/linkres:", true);
                WriteList(writer, _recurse, "/recurse:", true);

                TextFormatter.WriteList(writer, _input, " ");

                return writer.ToString();
            }
        }
        #endregion

        #region Utils
        static void WriteSignOption(TextWriter writer, string name, bool value)
        {
            writer.Write("/{0}{1} ", name, value ? "+" : "-");
        }

        static void WriteList(TextWriter writer, IEnumerable<string> arr, string prefix, bool path)
        {
            if (arr == null) return;
            foreach (var s in arr)
            {
                writer.Write(prefix);
                if (path && s.IndexOf(' ') >= 0)
                    writer.Write("\"" + s + "\"");
                else
                    writer.Write(s);
                writer.Write(" ");
            }
        }
        #endregion

        #region SetOption
        public bool SetOption(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) return false;

            name = name.ToLower();

            switch (name)
            {
                case "nologo":
                    NoLogo = true;
                    return true;

                case "debug":
                case "debug+":
                    Debug = true;
                    return true;

                case "debug-":
                    Debug = false;
                    return true;

                case "optimize":
                case "optimize+":
                case "o":
                case "o+":
                    Optimize = true;
                    return true;

                case "optimize-":
                case "o-":
                    Optimize = false;
                    return true;

                case "unsafe":
                case "unsafe+":
                    Unsafe = true;
                    return true;

                case "unsafe-":
                    Unsafe = true;
                    return true;

                case "checked":
                case "checked+":
                    Checked = true;
                    return true;

                case "checked-":
                    Checked = false;
                    return true;

                case "nostdlib":
                    NoStdlib = true;
                    return true;

                case "noconfig":
                    NoConfig = true;
                    return true;

                case "target":
                case "t":
                    if (string.IsNullOrEmpty(value))
                        return false;
                    switch (value.ToLower())
                    {
                        case "exe":
                            Target = CompilerTarget.ConsoleApp;
                            break;

                        case "winexe":
                            Target = CompilerTarget.WinApp;
                            break;

                        case "library":
                            Target = CompilerTarget.Library;
                            break;

                        case "module":
                            Target = CompilerTarget.Module;
                            break;

                        default:
                            return false;
                    }
                    return true;

                case "define":
                case "d":
                    _defines.Add(value);
                    return true;

                case "resource":
                case "res":
                    _resources.Add(value);
                    return true;

                case "linkresource":
                case "linkres":
                    _linkResources.Add(value);
                    return true;

                case "recurse":
                    _recurse.Add(value);
                    return true;

                case "reference":
                case "r":
                    _refs.Add(value);
                    return true;

                case "vbruntime-":
                    NoVBRuntime = true;
                    return true;

                case "vbruntime+":
                    NoVBRuntime = false;
                    return true;

                case "vbruntime":
                    VBRuntime = value;
                    return true;
            }

            return true;
        }
        #endregion
    }
    #endregion

    #region class CompilerConsole
    public static class CompilerConsole
    {
        static string GetFileName(CompilerLanguage lang)
        {
            switch (lang)
            {
                case CompilerLanguage.CSharp:
                    return "csc.exe";
                case CompilerLanguage.VB:
                    return "vbc.exe";
                case CompilerLanguage.JSharp:
                    return "jsc.exe";
                default:
                    throw new ArgumentOutOfRangeException("lang");
            }
        }

    	private static readonly FrameworkVersion[] FrameworkVersions =
    		{
    			FrameworkVersion.NET_3_5,
    			FrameworkVersion.NET_2_0
    			//FrameworkVersion.NET_1_1, 
    			//FrameworkVersion.NET_1_0, 
    		};

        /// <summary>
        /// Gets path to compiler.
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetPath(CompilerLanguage lang)
        {
            string exeName = GetFileName(lang);
            foreach (var v in FrameworkVersions)
            {
                string root = FrameworkInfo.GetRoot(v);
                string path = Path.Combine(root, exeName);
                if (File.Exists(path)) return path;
            }
            return "";
        }

        static void ParseLocation(string loc, out string file, out int line, out int col)
        {
            line = 0;
            col = 0;

            int i = loc.LastIndexOf(')');
            if (i < 0)
            {
                file = loc;
                return;
            }

            int i2 = loc.LastIndexOf('(', i);
            
            if (i2 >= 0 && i2 < i)
            {
                file = loc.Substring(0, i2).Trim();
                string path = loc.Substring(i2 + 1, i - i2 - 1).Trim();
                int i3 = path.IndexOf(',');
                if (i3 >= 0)
                {
                    line = int.Parse(path.Substring(0, i3).Trim());
                    col = int.Parse(path.Substring(i3 + 1).Trim());
                }
                else
                {
                    line = int.Parse(path.Trim());
                }
            }
            else
            {
                file = loc;
            }
        }

        static bool Is(string s, int i, string value)
        {
            if (i + value.Length > s.Length)
                return false;
            for (int j = 0; j < value.Length; ++i, ++j)
            {
                if (char.ToLower(s[i]) != value[j])
                    return false;
            }
            return true;
        }

        static string substr(string s, int start, int end)
        {
            return s.Substring(start, end - start + 1);
        }

        static readonly string[] ErrorLevels =
            {
                "error",
                "warning",
                "warn"
            };

        static int FindError(string s, ref string level, ref string errorNumber)
        {
            int n = s.Length;
            for (int i = 0; i < n; ++i)
            {
                foreach (string el in ErrorLevels)
                {
                	if (Is(s, i, el))
                	{
                		int si = i + el.Length + 1;
                		int colon = s.IndexOf(':', si);
                		if (colon >= 0)
                		{
                			level = el;
                			errorNumber = substr(s, si, colon - 1).Trim();
                			return i;
                		}
                	}
                }
            }
            return -1;
        }

        public static CompilerError ParseError(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            string file = null;
            int line = 0, col = 0;
            string level = "warning";
            string errorNumber = "CS0000";
            string errorText;

            int i = FindError(s, ref level, ref errorNumber);
            if (i >= 0)
            {
                string loc = s.Substring(0, i).Trim().TrimEnd(':');
                ParseLocation(loc, out file, out line, out col);
                errorText = s.Substring(i + level.Length + errorNumber.Length + 3).Trim();
            }
            else
            {
                errorText = s;
            }

            var err = new CompilerError(file, line, col, errorNumber, errorText);
            if (level.StartsWith("warn"))
                err.IsWarning = true;

            return err;
        }

        public static CompilerErrorCollection ParseOutput(string output)
        {
            using (var reader = new StringReader(output))
            {
                var errors = new CompilerErrorCollection();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var err = ParseError(line);
                    if (err != null)
                        errors.Add(err);
                }
                return errors;
            }
        }

        public static bool HasErrors(string output)
        {
            return ParseOutput(output).HasErrors;
        }

        public static string Run(CompilerOptions options, bool redirect)
        {
            var lang = options.Language;
            string path = GetPath(lang);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new FileNotFoundException(
                    string.Format("Unable to find {0} compiler.", lang));
            }
            string args = options.ToString();
            int exitCode;
            return CommandPromt.Run(path, args, out exitCode, false, redirect);
        }

        public static string Run(CompilerOptions options)
        {
            return Run(options, true);
        }

        public static CompilerErrorCollection RunErr(CompilerOptions options)
        {
            string cout = Run(options, true);
            var errors = ParseOutput(cout);
            if (errors.HasErrors)
                Console.WriteLine(cout);
            return errors;
        }

        public static string SimpleRun(CompilerLanguage lang, string input, string output, bool debug, bool optimize)
        {
            var options = new CompilerOptions
                              {
                                  Language = lang,
                                  Debug = debug,
                                  Optimize = optimize,
                                  Unsafe = true
                              };
            options.Input.Add(input);
            options.Output = output;
            string res = Run(options, true);
            if (HasErrors(res))
            {
                return res;
            }
            return null;
        }
    }
    #endregion
}