using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.Tools;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Ecma335;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.FlashLand.Swf;
using DataDynamics.PageFX.TestRunner.Tools;
using DataDynamics.PageFX.TestRunner.UI;
using Microsoft.Win32;
using NUnit.Framework;



namespace DataDynamics.PageFX.TestRunner.Framework
{
    public static class QA
    {
        public static void ShowUI()
        {
            LoadGlobalOptions();
            using (var dlg = new QAForm())
                dlg.ShowDialog();
            SaveGlobalOptions();
        }

        #region Global Options
        public const string KeyBaseDir = "QA.BaseDir";
        public const string KeyOptimizeCode = "CG.Optimize";
        public const string KeyEmitDebugInfo = "CG.Debug";
        public const string KeyUseCommonDir = "UseCommonDir";
        public const string KeyNightlyBuild = "NightlyBuild";

        public static string BaseDir = "C:\\QA";

        public static bool OptimizeCode
        {
            get
            {
                return _optimizeCode;
            }
            set
            {
               if (value != _optimizeCode)
               {
                   _optimizeCode = value;
                   SaveGlobalOptions();
               }
            }
        }
        static bool _optimizeCode = true;

        public static bool EmitDebugInfo
        {
            get
            {
                return _emitDebugInfo;
            }
            set
            {
                if (value != _emitDebugInfo)
                {
                    _emitDebugInfo = value;
                    SaveGlobalOptions();
                }
            }
        }
        static bool _emitDebugInfo = true;

        public static bool LoadNUnitTests = true;
        public static bool TestDebugSupport;
        public static bool ProtectNUnitTest = true;
        public static bool RunSuiteAsOneTest = true;

        public static bool IsNUnitSession { get; set; }

        public static TestRunnerGeneratorOptions TestRunnerOptions
        {
            get
            {
                return new TestRunnerGeneratorOptions
                {
                    Protect = ProtectNUnitTest,
                    EndMarker = FlashPlayer.MarkerEnd,
                    FailString = FlashPlayer.MarkerFail,
                    SuccessString = FlashPlayer.MarkerSuccess
                };
            }
        }

        public static bool UseCommonDirectory
        {
            get
            {
                if (IsNUnitSession)
                    return true;
                return _useCommonDir;
            }
            set
            {
                if (value != _useCommonDir)
                {
                    _useCommonDir = value;
                    SaveGlobalOptions();
                }
            }
        }
        static bool _useCommonDir;

        public static void LoadGlobalOptions()
        {
            BaseDir = GetValue(KeyBaseDir, "C:\\QA");
            if (string.IsNullOrEmpty(BaseDir))
                BaseDir = "C:\\QA";

            _optimizeCode = GetValue(KeyOptimizeCode, true);
            _emitDebugInfo = GetValue(KeyEmitDebugInfo, true);
            _useCommonDir = GetValue(KeyUseCommonDir, false);
        }

        public static void SaveGlobalOptions()
        {
            SetValue(KeyBaseDir, BaseDir);
            SetValue(KeyOptimizeCode, _optimizeCode);
            SetValue(KeyEmitDebugInfo, _emitDebugInfo);
            SetValue(KeyUseCommonDir, _useCommonDir);
        }

        private static bool ToBool(object value)
        {
            if (value is bool)
                return (bool)value;
            var c = value as IConvertible;
            if (c != null)
                return c.ToBoolean(null);
            throw new InvalidCastException();
        }

        public static string GetOptionKey(GlobalOptionName name)
        {
            switch (name)
            {
                case GlobalOptionName.OptimizeCode:
                    return KeyOptimizeCode;

                case GlobalOptionName.EmitDebugInfo:
                    return KeyEmitDebugInfo;

                case GlobalOptionName.UseCommonDirectory:
                    return KeyUseCommonDir;

                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        public static void SetOption(GlobalOptionName name, object value)
        {
            switch (name)
            {
                case GlobalOptionName.OptimizeCode:
                    OptimizeCode = ToBool(value);
                    break;

                case GlobalOptionName.EmitDebugInfo:
                    EmitDebugInfo = ToBool(value);
                    break;

                case GlobalOptionName.UseCommonDirectory:
                    UseCommonDirectory = ToBool(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        public static object GetOption(GlobalOptionName name)
        {
            switch (name)
            {
                case GlobalOptionName.OptimizeCode:
                    return OptimizeCode;

                case GlobalOptionName.EmitDebugInfo:
                    return EmitDebugInfo;

                case GlobalOptionName.UseCommonDirectory:
                    return UseCommonDirectory;

                default:
                    throw new ArgumentOutOfRangeException("name");
            }
        }

        public static bool GetBoolOption(GlobalOptionName name)
        {
            return ToBool(GetOption(name));
        }
        #endregion

        #region Reg Utils
        public const string HKCU_RegPath = "HKEY_CURRENT_USER\\Software\\Data Dynamics\\PageFX\\QA";
        public const string HKLM_RegPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Data Dynamics\\PageFX\\QA";

        public static string GetKeyName(string path)
        {
            string res = HKCU_RegPath;
            string key = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(key))
                res += "\\" + key;
            return res;
        }

        public static T GetValue<T>(string path, T defval)
        {
            try
            {
                string name = Path.GetFileName(path);
                string key = GetKeyName(path);
                return (T)Registry.GetValue(key, name, defval);
            }
            catch
            {
                return defval;
            }
        }

        public static bool GetValue(string path, bool defval)
        {
            try
            {
                string name = Path.GetFileName(path);
                string key = GetKeyName(path);
                var v = Registry.GetValue(key, name, defval);
                var c = v as IConvertible;
                if (c != null) return c.ToBoolean(null);
                return defval;
            }
            catch
            {
                return defval;
            }
        }

        public static Point GetValue(string path, Point defval)
        {
            string s = GetValue(path, "");
            if (string.IsNullOrEmpty(s)) return defval;
            int i = s.IndexOf(';');
            if (i < 0 || i >= s.Length - 1) return defval;
            string xs = s.Substring(0, i).Trim();
            string ys = s.Substring(i + 1).Trim();
            int x, y;
            if (!int.TryParse(xs, out x)) return defval;
            if (!int.TryParse(ys, out y)) return defval;
            return new Point(x, y);
        }

        public static Size GetValue(string path, Size defval)
        {
            string s = GetValue(path, "");
            if (string.IsNullOrEmpty(s)) return defval;
            int i = s.IndexOf(';');
            if (i < 0 || i >= s.Length - 1) return defval;
            string xs = s.Substring(0, i).Trim();
            string ys = s.Substring(i + 1).Trim();
            int x, y;
            if (!int.TryParse(xs, out x) || x < 0) return defval;
            if (!int.TryParse(ys, out y) || y < 0) return defval;
            return new Size(x, y);
        }

        public static void SetValue(string path, object value)
        {
            try
            {
                string name = Path.GetFileName(path);
                string key = GetKeyName(path);
                Registry.SetValue(key, name, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetValue(string path, Point pt)
        {
            SetValue(path, pt.X + ";" + pt.Y);
        }

        public static void SetValue(string path, Size value)
        {
            SetValue(path, value.Width + ";" + value.Height);
        }
        #endregion

        #region Path Utils
        public static string BinDir
        {
            get
            {
                var asm = typeof(QA).Assembly;
                return Path.GetDirectoryName(asm.Location);
            }
        }

        public static string TestResultsReportPath
        {
            get
            {
                return Path.Combine(BinDir, "report.htm");
            }
        }

        public static string Root
        {
            get { return Path.Combine(BaseDir, "PageFX"); }
        }

        public static string RootTestCases
        {
            get { return Path.Combine(Root, "TestCases"); }
        }

        public static string RootReports
        {
            get { return Path.Combine(Root, "Reports"); }
        }

        public static string CommonDirectory
        {
            get { return GetTestCasePath("common"); }
        }

        public static string GetTestCasePath(string subpath)
        {
            return Path.Combine(RootTestCases, subpath);
        }

        public static string GetReportPath(string subpath)
        {
            return Path.Combine(RootReports, subpath);
        }

        public static string NUnitTestsDirectory
        {
            get
            {
                return Path.Combine(GlobalSettings.HomeDirectory, "tests");
            }
        }

        public static void CopyNUnitTests()
        {
            string dir = NUnitTestsDirectory;
            if (Directory.Exists(dir))
            {
                try
                {
                    string dstDir = CommonDirectory;
                    Directory.CreateDirectory(dstDir);
                    GetNUnitFrameworkPath(dstDir);
                    foreach (var srcFile in Directory.GetFiles(dir))
                    {
                        string dstFile = Path.Combine(dstDir, Path.GetFileName(srcFile));
                        File.Copy(srcFile, dstFile, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        #endregion

        public static void SaveSwf(SwfMovie swf, string name)
        {
            string dir = Path.Combine(Root, "swf");
            Directory.CreateDirectory(dir);
            swf.Save(Path.Combine(dir, name));
        }

        public static string ReadAllText(Stream input)
        {
            using (var reader = new StreamReader(input))
                return reader.ReadToEnd();
        }

        public static void ShowBrowser(string title, string path, bool dlg)
        {
            var form = new Form
                           {
                               Text = title,
                               StartPosition = FormStartPosition.CenterScreen,
                               Size = new Size(800, 600)
                           };
            var browser = new WebBrowser
                              {
                                  Dock = DockStyle.Fill,
                                  Url = new Uri(path)
                              };
            form.Controls.Add(browser);
            if (dlg) form.ShowDialog();
            else form.Show();
        }

        #region ResolveReferences
        private static void ResolveReference(string dir, string name, ICollection<string> list)
        {
            const StringComparison cmptype = StringComparison.InvariantCultureIgnoreCase;
            if (name.EndsWith(".abc", cmptype))
            {
                var rs = typeof(QA).GetResourceStream(name);
                if (rs == null)
                    throw new InvalidOperationException();

                string path = Path.Combine(dir, name);
                rs.Save(path);

                string dll = Path.ChangeExtension(name, ".dll");
                WrapperGenerator.Generate(path, dll, null);

                list.Add(dll);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unable to resolve ref: {0}", name));
            }
        }

        private static void ResolveReferences(string dir, IEnumerable<string> refs, ICollection<string> list)
        {
            if (refs == null) return;
            foreach (var name in refs)
            {
                ResolveReference(dir, name, list);
            }
        }
        #endregion

        #region Compile
        public static bool Compile(TestCase tc)
        {
            tc.CopySourceFiles();

            var lang = tc.Language;
            if (lang == CompilerLanguage.CIL)
            {
                return ILAsm.Run(tc);
            }

            using (tc.Root.ChangeCurrentDirectory())
            {
            	var options = new CompilerOptions();
            	try
            	{
            		ResolveReferences(tc.Root, tc.References, options.References);
            	}
            	catch (Exception exc)
            	{
            		tc.Error = string.Format("Unable to resolve test case {0} references. Exception:\n{1}",
            		                         tc.Name, exc);
            		return false;
            	}

            	SetCompilerOptions(tc, lang, options);

            	try
            	{
            		string cout = CompilerConsole.Run(options, true);
            		if (CompilerConsole.HasErrors(cout))
            		{
            			tc.Error = string.Format("Unable to compile test case {0}.\n{1}", tc.Name, cout);
            			return false;
            		}
            	}
            	catch (Exception exc)
            	{
            		tc.Error = string.Format("Unable to compile test case {0}. Exception:\n{1}", tc.Name, exc);
            		return false;
            	}
            }

            return true;
        }

        static void SetCompilerOptions(TestCase tc, CompilerLanguage lang, CompilerOptions options)
        {
            options.Language = lang;
            options.Debug = tc.Debug;
            options.Optimize = tc.Optimize;
            options.Unsafe = tc.Unsafe;
            options.Output = tc.ExePath;
            options.Target = CompilerTarget.ConsoleApp;
            options.NoLogo = true;
            options.Checked = false;

            options.Define("TARGET_JVM"); //to remove unsafe
            options.Define("NET_2_0");
            options.Defines.AddRange(tc.Defines);

            if (tc.VM == VM.CLR)
                options.Define("MSCLR");
            else if (tc.VM == VM.AVM)
                options.Define("AVM");

            //common refs
            if (tc.VM == VM.AVM)
            {
                GlobalSettings.AddCommonReferences(options);
                options.NoConfig = true;
                options.NoStdlib = true;

                if (lang == CompilerLanguage.VB)
                {
                    //options.NoVBRuntime = true;
                    options.VBRuntime = GlobalSettings.Libs.VBRuntime;
                    //options.CompactFramework = true;
                    options.SDKPath = GlobalSettings.Dirs.Libs;
                    options.NoWarn = true;
                }
            }
            else
            {
                options.AddRef("System.Core.dll");
            }

            if (tc.IsNUnit)
            {
                if (tc.VM == VM.CLR)
                {
                    string nunit = GetNUnitFrameworkPath(UseCommonDirectory ? "" : tc.Root);
                    options.AddRef(nunit);
                }
                else
                {
                    options.AddRef(GlobalSettings.GetLibPath(PfxNUnitFrameworkDLL));
                }
                options.AddRef(tc.GetNUnitTestsPath(tc.Root));
            }

            options.Input.AddRange(tc.SourceFiles.Names);
        }

        static string CopyTo(string dir, string path)
        {
            Directory.CreateDirectory(dir);
            string destFileName = Path.Combine(dir, Path.GetFileName(path));
            if (!File.Exists(destFileName))
                File.Copy(path, destFileName);
            return destFileName;
        }

        static string GetNUnitFrameworkPath(string copyto)
        {
            string path = typeof(TestAttribute).Assembly.Location;

            if (string.IsNullOrEmpty(copyto))
                return path;

            return CopyTo(copyto, path);
        }

        const string PfxNUnitFrameworkDLL = "NUnit.Framework.dll";
        #endregion

	    #region ToCSharp
        public static void ToCSharp(IAssembly asm, string path)
        {
            try
            {
                ExportService.ToFile(asm, "c#", path);
            }
            catch (Exception e)
            {
                //string.Format("Unable to serialize c# source code.\nException:\n{0}", e);
            }
        }
        #endregion

        #region Compare Utils
        public static string CompareFiles(string path1, string path2)
        {
            var a = File.ReadAllBytes(path1);
            var b = File.ReadAllBytes(path2);
            return CompareByteArrays(a, b);
        }

        public static string CompareByteArrays(byte[] a, byte[] b, out int index)
        {
            index = -1;
            var err = new StringBuilder();
            int an = a.Length;
            int bn = b.Length;
            if (an != bn)
                err.AppendFormat("Length of byte arrays is different. {0} != {1}.\n", an, bn);

            int n = Math.Min(an, bn);
            for (int i = 0; i < n; ++i)
            {
                if (a[i] != b[i])
                {
                    index = i;
                    err.AppendFormat("Byte arrays are different at index {0}. Expected {1}, but was {2}.\n",
                                     i, a[i], b[i]);
                    break;
                }
            }

            if (err.Length > 0)
                return err.ToString();
            return null;
        }

        public static string CompareByteArrays(byte[] a, byte[] b)
        {
            int index;
            return CompareByteArrays(a, b, out index);
        }

        class Lines
        {
            readonly List<string> _1 = new List<string>();
            readonly List<string> _2 = new List<string>();
            int _diff;
            int _maxlen;

            public void Add(string line1, string line2)
            {
                if (line1 == null)
                    line1 = "";
                if (line2 == null)
                    line2 = "";
                _1.Add(line1);
                _2.Add(line2);
                _maxlen = Math.Max(_maxlen, line1.Length);
            }

            public void SetDiffLine()
            {
                _diff = _1.Count;
            }

            string FormatLine(int i)
            {
                int n = _1.Count;
                int maxNumLen = (n + ". ").Length;

                int num = i + 1;
                string s = num + ". ";
                s = s.PadLeft(maxNumLen);
                string f = string.Format("{{0,-{0}}}{{1}}", _maxlen + 4);

                string l1 = _1[i];
                string l2 = _2[i];
                return s + string.Format(f, l1, l2);
            }

            public override string ToString()
            {
                var sb = new StringBuilder(256);
                sb.AppendFormat("Outputs are different at line {0}.", _diff);
                sb.AppendLine();
                sb.AppendLine("Output:");

                int n = Math.Min(_1.Count, _2.Count);
                for (int i = 0; i < n; ++i)
                {
                    sb.AppendLine(FormatLine(i));
                }
                return sb.ToString();
            }
        }

        public static string CompareLines(string text1, string text2, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(text1))
            {
                if (!string.IsNullOrEmpty(text2))
                {
                    return "Outputs are different. Second output is not empty, but first is empty.";
                }
            }

            bool notFast = !IsNUnitSession;

            using (var reader1 = new StringReader(text1))
            using (var reader2 = new StringReader(text2))
            {
                var lines = new Lines();
                while (true)
                {
                    string line1 = reader1.ReadLine();
                    string line2 = reader2.ReadLine();
                    if (line1 == null)
                    {
                        if (line2 == null)
                        {
                            //ok!!!
                            return null;
                        }

                        lines.Add(null, line2);
                        lines.SetDiffLine();

                        if (notFast)
                            ReadRestLines(reader1, reader2, null, line2, lines);
                        break;
                    }

                    if (line2 == null)
                    {
                        lines.Add(line1, null);
                        lines.SetDiffLine();

                        if (notFast)
                            ReadRestLines(reader1, reader2, line1, null, lines);
                        break;
                    }

                    if (line1.Length == 0)
                    {
                        if (line2 == "null")
                        {
                            lines.Add(line1, line2);
                            continue;
                        }
                    }

                    lines.Add(line1, line2);

                    if (string.Compare(line1, line2, ignoreCase) != 0)
                    {
                        lines.SetDiffLine();
                        if (notFast)
                            ReadRestLines(reader1, reader2, line1, line2, lines);
                        break;


                    }
                }
                return lines.ToString();

            }
        }

        static void ReadRestLines(TextReader reader1, TextReader reader2, string line1, string line2, Lines lines)
        {
            while (true)
            {
                if (line1 != null)
                    line1 = reader1.ReadLine();
                if (line2 != null)
                    line2 = reader2.ReadLine();
                if (line1 == null && line2 == null)
                    break;
                lines.Add(line1, line2);
            }
        }
        #endregion

        #region Utils

	    public static string GetLastPart(string name)
        {
            int i = name.LastIndexOf('.');
            if (i >= 0)
                return name.Substring(i + 1).Trim();
            return name;
        }

        public static void SetupCLI(VM vm)
        {
            if (vm == VM.AVM)
            {
                CommonLanguageInfrastructure.ResolveLabels = false;
                CommonLanguageInfrastructure.SubstituteFrameworkAssemblies = true;
                CommonLanguageInfrastructure.EnableDecompiler = false;
                CommonLanguageInfrastructure.ClearCache();
            }
            else
            {
                CommonLanguageInfrastructure.ResolveLabels = true;
                CommonLanguageInfrastructure.SubstituteFrameworkAssemblies = false;
                CommonLanguageInfrastructure.EnableDecompiler = true;
            }
        }

        public static bool VerifyAssembly;

        public static IAssembly LoadAssembly(string path, VM vm, string tcroot, ref string error)
        {
            SetupCLI(vm);

#if DEBUG
            CommonLanguageInfrastructure.Debug = true;
            CommonLanguageInfrastructure.TestCaseDirectory = tcroot;
#endif

            IAssembly asm;
            try
            {
                asm = CommonLanguageInfrastructure.Deserialize(path, null);
            }
            catch (Exception e)
            {
                error = string.Format("Unable to deserialize assembly.\nException:\n{0}", e);
                return null;
            }

            return asm;
        }
        #endregion

        public static string RemovePrefix(string s, string prefix)
        {
            if (s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return s.Substring(prefix.Length);
            return s;
        }

        public static string RemoveSuffix(string s, string suffix)
        {
            if (s.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                return s.Substring(0, s.Length - suffix.Length);
            return s;
        }

        #region Generation of NUnit Fixtures
        internal static string TestSet = "all";

        static string GetNUnitMethodName(TestCase tc)
        {
            string s = tc.FullName;
            if (!tc.IsNUnit)
            {
                s = RemoveSuffix(s, tc.FileExtension);
            }
            s = RemovePrefix(s, "PageFX.");
            s = RemovePrefix(s, "Simple.");
            s = s.Replace('-', '_');
            s = s.Replace('.', '_');
            return s;
        }

        static readonly string[] CriticalSubstrings =
        {
            "mono.System.ArrayTest",
            "mono.System.StringTest",
            "mono.System.ConvertTest",
            "CSharp.Delegates.",
            "CSharp.Generics.",
            "CSharp.Nullable."
        };

        static bool IsCriticalTest(TestCase tc)
        {
            string fn = tc.FullName;
            if (CriticalSubstrings.Any(fn.Contains))
            {
                if (fn.Contains("CSharp.Generics.Casting"))
                    return false;
                return true;
            }
            return false;
        }

        static bool IsFrameworkTest(TestCase tc)
        {
            if (IsCriticalTest(tc)) return false;
            string fn = tc.FullName;
            if (fn.Contains("mono.VB.")) return false;
            if (fn.Contains("mono.")) return true;
            return false;
        }

        static bool ShouldInclude(TestCase tc)
        {
            if (string.IsNullOrEmpty(TestSet)
                || string.Compare(TestSet, "all", true) == 0)
                return true;

            if (string.Compare(TestSet, "critical", true) == 0)
                return IsCriticalTest(tc);

            if (string.Compare(TestSet, "framework", true) == 0)
                return IsFrameworkTest(tc);

            if (string.Compare(TestSet, "rest", true) == 0)
            {
                if (IsCriticalTest(tc)) return false;
                if (IsFrameworkTest(tc)) return false;
                return true;
            }

            return true;
        }

        public static void GenerateNUnitTestFixture(string path, string ns, string classname, string format, IEnumerable<TestCase> testCases)
        {
            string dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);

            if (string.IsNullOrEmpty(ns))
                ns = "DataDynamics.PageFX.FLI.Tests";

            if (string.IsNullOrEmpty(classname))
                classname = "SWFALL";

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("//WARNING: This file is auto generated!!!");
                writer.WriteLine();

                writer.WriteLine("using NUnit.Framework;");
                writer.WriteLine();

                writer.WriteLine("namespace {0}", ns);
                writer.WriteLine("{");

                writer.WriteLine("\t[TestFixture]");
                writer.WriteLine("\tpublic class {0}", classname);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\t[SetUp]");
                writer.WriteLine("\t\tpublic void SetUp()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tQA.IsNUnitSession = true;");
                if (RunSuiteAsOneTest)
                    writer.WriteLine("\t\t\tQA.RunSuiteAsOneTest = true;");
                writer.WriteLine("\t\t}");

                writer.WriteLine("\t\tstatic void Run(string fullname)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tTestEngine.RunTestCase(fullname, \"{0}\");", format);
                writer.WriteLine("\t\t}");

                foreach (var tc in testCases)
                {
                    if (!ShouldInclude(tc)) continue;

                    writer.WriteLine("\t\t[Test]");
                    writer.WriteLine("\t\tpublic void {0}()", GetNUnitMethodName(tc));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tRun(\"{0}\");", tc.FullName);
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");

                writer.WriteLine("}");
            }
        }

        public static void GenerateNUnitTestFixture(string path, string ns, string classname, string format)
        {
            GenerateNUnitTestFixture(path, ns, classname, format, SimpleTestCases.GetAllTestCases());
        }
        #endregion
    }

    public enum GlobalOptionName
    {
        OptimizeCode,
        EmitDebugInfo,
        UseCommonDirectory
    }
}