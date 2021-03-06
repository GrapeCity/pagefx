using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    /// <summary>
    /// Represents test case (data for unit test)
    /// </summary>
    public class TestCase : ITestItem
    {
        #region Constructors
        public TestCase(string fullname)
        {
	        FullName = fullname;
        }

	    public TestCase(string fullname, string text) : this(fullname)
        {
            var sf = new SourceFile();
            _srcfiles.Add(sf);
            sf.Name = Name;
            sf.Text = text;
        }
        #endregion

        #region Public Properties
	    /// <summary>
	    /// Gets the full name of this test case.
	    /// </summary>
	    public string FullName { get; private set; }

	    public IEnumerable<ITestItem> GetChildren()
	    {
		    return Enumerable.Empty<ITestItem>();
	    }

	    public bool UsePfc { set; get; }

	    private bool HasExt
        {
            get
            {
                int i = FullName.LastIndexOf('.');
                if (i >= 0 && i >= FullName.Length - 4)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets the name of this test case
        /// </summary>
        public string Name
        {
            get
            {
                int i = FullName.LastIndexOf('.');
                if (i >= 0)
                {
                    if (HasExt)
                    {
                        i = FullName.LastIndexOf('.', i - 1);
                        if (i >= 0)
                            return FullName.Substring(i + 1);
                    }
                    return FullName.Substring(i + 1);
                }
                return FullName;
            }
        }

	    public string Root
        {
            get
            {
                if (GlobalOptions.UseCommonDirectory)
                    return QA.CommonDirectory;

                if (string.IsNullOrEmpty(_root) 
                    || _rootCount != _srcfiles.Count)
                {
                    _rootCount = _srcfiles.Count;
                    if (_rootCount > 0)
                    {
                        string s = FullName;
                        if (_rootCount == 1)
                        {
                            int i = s.LastIndexOf('.');
                            s = s.Substring(0, i);
                        }
                        s = s.Replace('.', '\\');
                        _root = QA.GetTestCasePath(s);
                    }
                    else
                    {
                        _root = QA.RootTestCases;
                    }
                }
                return _root;
            }
        }
        private int _rootCount = -1;
        private string _root;

        private static CompilerLanguage FromExt(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return CompilerLanguage.CSharp;

	        if (ext.StartsWith("."))
		        ext = ext.Substring(1);

	        switch (ext.ToLower())
	        {
		        case "vb": return CompilerLanguage.VB;
		        case "il": return CompilerLanguage.CIL;
		        case "js": return CompilerLanguage.JSharp;
		        case "fs": return CompilerLanguage.FSharp;
				default: return CompilerLanguage.CSharp;
	        }
        }

        public string FileExtension
        {
            get
            {
                int n = _srcfiles.Count;
                if (n > 0)
                {
                    return Path.GetExtension(_srcfiles[0].Name);
                }
                return "";
            }
        }

        public CompilerLanguage Language
        {
            get
            {
                return FromExt(FileExtension);
            }
        }

        public bool CompareOutputs
        {
            get
            {
                if (FullName.StartsWith("PageFX"))
                    return true;
                return false;
            }
        }

        public bool IsBenchmark { get; set; }

        public bool CheckExitCode
        {
            get { return !CompareOutputs; }
        }

        public bool Unsafe
        {
            get
            {
                return FullName.Contains("Unsafe");
            }
        }
        
        public bool Debug = GlobalOptions.EmitDebugInfo;
        public bool Optimize = GlobalOptions.OptimizeCode;

        public SourceFileList SourceFiles
        {
            get { return _srcfiles; }
        }
        private readonly SourceFileList _srcfiles = new SourceFileList();

        public bool IsSimple
        {
            get { return SourceFiles.Count == 1; }
        }

        public bool IsComplex
        {
            get { return SourceFiles.Count > 1; }
        }

        public void CopySourceFiles()
        {
            Directory.CreateDirectory(Root);

            //copy source files to test case dir
            foreach (var f in SourceFiles)
            {
                File.WriteAllText(Path.Combine(Root, f.Name), f.Text);
            }
        }

        public bool IsNUnit { get; set; }

    	public string NUnitBasePath { get; set; }

    	public string GetNUnitTestsPath(string root)
        {
            string suffix = VM == VM.AVM ? ".avm.dll" : ".clr.dll";
            string fullpath = NUnitBasePath + suffix;
            string dest = Path.Combine(root, Path.GetFileName(fullpath));
            if (!GlobalOptions.UseCommonDirectory)
                File.Copy(fullpath, dest, true);
            return dest;
        }

        public List<string> Defines
        {
            get { return _defines; }
        }
        private readonly List<string> _defines = new List<string>();

        public void Define(string symbol)
        {
            _defines.Add(symbol);
        }

        public List<string> References
        {
            get { return _refs; }
        }
        readonly List<string> _refs = new List<string>();

        public void AddRef(string path)
        {
            _refs.Add(path);
        }

        public string Output { get; set; }

        public bool HasOutput
        {
            get { return !string.IsNullOrEmpty(Output); }
        }

        public VM VM { get; set; }

        public string OutputPath { set; get; }

        public bool CompileAVM
        {
            get
            {
                if (Language == CompilerLanguage.VB) return true;
                if (IsBenchmark) return true;
                if (HasOutput) return true;
                return false;
            }
        }

        public bool CompileCLR
        {
            get
            {
                if (IsBenchmark) return false;
                if (HasOutput) return false;
                return true;
            }
        }
        #endregion

        #region Results & Temp Variables
        const string clr_exe = "clr.exe";
        const string avm_exe = "avm.exe";

        string ExeName
        {
            get
            {
                switch (VM)
                {
                    case VM.CLR:
                        if (!CompileCLR) return avm_exe;
                        return clr_exe;
                    case VM.AVM:
                        if (!CompileAVM) return clr_exe;
                        return avm_exe;
                }
                return "out.exe";
            }
        }

        public string ExePath
        {
            get { return Path.Combine(Root, ExeName); }
        }

        public string Error { get; set; }

        public bool HasErrors
        {
            get
            {
                return !string.IsNullOrEmpty(Error);
            }
        }

        public bool IsFailed
        {
            get { return HasErrors; }
        }

        public bool IsPassed
        {
            get { return !HasErrors; }
        }

        public string DecompiledCode { get; set; }
        public string AvmDump { get; set; }
        public string AbcDump { get; set; }
        public string Output1 { get; set; }
        public string Output2 { get; set; }

        public void Reset()
        {
            Error = null;
            DecompiledCode = null;
            AvmDump = null;
            Output1 = null;
            Output2 = null;
        }
        #endregion

        #region Flags
        TestCaseFlags _flags;

        public bool IsStarted
        {
            get { return (_flags & TestCaseFlags.Started) != 0; }
            set
            {
                if (value) _flags |= TestCaseFlags.Started;
                else _flags &= ~TestCaseFlags.Started;
            }
        }

        public bool IsFinished
        {
            get { return (_flags & TestCaseFlags.Finished) != 0; }
            set
            {
                if (value)
                {
                    IsStarted = false;
                    _flags |= TestCaseFlags.Finished;
                }
                else _flags &= ~TestCaseFlags.Finished;
            }
        }

        public bool IsCancelled
        {
            get { return (_flags & TestCaseFlags.Cancelled) != 0; }
            set
            {
                if (value)
                {
                    IsStarted = false;
                    IsFinished = false;
                    _flags |= TestCaseFlags.Cancelled;
                }
                else _flags &= ~TestCaseFlags.Cancelled;
            }
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return FullName;
        }
        #endregion

        #region LoadConfig
        public void LoadConfig(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);

            var root = doc.DocumentElement;
            foreach (XmlElement refElem in root.GetElementsByTagName("ref"))
            {
                string name = refElem.GetAttribute("name");
                if (string.IsNullOrEmpty(name))
                    throw new InvalidOperationException("Reference name can not be null or empty string");
                _refs.Add(name);
            }
            
            var elem = root["output"];
            if (elem != null)
            {
                var sb = new StringBuilder();
                foreach (XmlElement l in elem.GetElementsByTagName("l"))
                {
                    sb.AppendLine(l.InnerText);
                }
                Output = sb.ToString();
            }
        }
        #endregion

        #region LoadAssembly
        public IAssembly LoadAssembly()
        {
            string err = null;
            var asm = AssemblyLoader.Load(ExePath, VM, Root, ref err);
            Error = err;
            return asm;
        }
        #endregion
    }
}