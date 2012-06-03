#define TEST
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.CLI;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX
{
    class Program
    {
        static CommandLine cl;

        static int Main(string[] args)
        {
            cl = CommandLine.Parse(args);
            if (cl == null)
                cl = new CommandLine();

            try
            {
                string target = cl.GetOption(null, "t", "target");

                if (string.IsNullOrEmpty(target)
                    || string.Compare(target, "report", true) == 0)
                {
                    MakeReports();
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return 0;
        }

        static void MakeReports()
        {
            Global.NUnitReport = LoadNUnitReport();

            if (Global.NUnitReport == null)
            {
                //NOTE: This is as test mode supposing that all tests are passed on all runtimes
                //Console.WriteLine("error: Unable to load nunit report");
                //Environment.Exit(-1);
            }

            LoadNUnitAssembly();
            LoadTestSuites();
            LoadAPI();
            LoadApiTree();
            WriteReport();
        }

        static void WriteReport()
        {
            string dir = GetOutDir();
            string path = Path.Combine(dir, "report.htm");
            
            var apitree = Global.ApiRoot.ToXml("apitree", path);
            var testtree = Global.TestRoot.ToXml("testtree", path);

            Utils.WriteTemplate(path, "ReportTemplate.xml",
                "APITREE", apitree, "TESTTREE", testtree);
        }

        static void LoadApiTree()
        {
            foreach (var assembly in Global.API)
            {
                var assemblyNode = new AssemblyNode {Assembly = assembly};
                Global.ApiRoot.Add(assemblyNode);

                assemblyNode.Load();
            }
        }

        static string ReportsDir
        {
            get { return "c:\\pfx\\reports"; }
        }

        static string NUnitReportDir
        {
            get { return Path.Combine(ReportsDir, "nunit"); }
        }

        static string DefaultOutDir
        {
            get { return Path.Combine(ReportsDir, "QA"); }
        }

        static string GetOutDir()
        {
            return cl.GetPath(DefaultOutDir, "outdir");
        }

        #region LoadNUnitReport
        static XmlDocument LoadXmlDoc(string path)
        {
            if (!File.Exists(path)) return null;
            var doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }

        static readonly string[] NUnitReportFiles = 
        {
            "DataDynamics.PageFX.QA.dll-results.xml",
            "nunit-results.xml"
        };

        static XmlDocument TryLoadNUnitReport(string dir)
        {
            foreach (var file in NUnitReportFiles)
            {
                string path = Path.Combine(dir, file);
                if (File.Exists(path))
                    return LoadXmlDoc(path);
            }
            return null;
        }

        static XmlDocument LoadNUnitReport()
        {
            string nur = cl.GetPath(null, "nur", "nunit.report");
            if (!string.IsNullOrEmpty(nur))
                return LoadXmlDoc(nur);

            XmlDocument doc;
            string nunitdir = cl.GetPath(null, "nunit", "nunit-dir");
            if (!string.IsNullOrEmpty(nunitdir))
            {
                doc = TryLoadNUnitReport(nunitdir);
                if (doc != null)
                    return doc;
            }

            doc = TryLoadNUnitReport(NUnitReportDir);
            if (doc != null)
                return doc;

            return TryLoadNUnitReport(Environment.CurrentDirectory);
        }
        #endregion

        #region LoadNUnitAssembly
        static void LoadNUnitAssembly()
        {
            string testdir = Path.Combine(GlobalSettings.HomeDirectory, "tests");
            string path = Path.Combine(testdir, "mono.avm.dll");
            Global.NUnitAssembly = LoadAssembly(path, true);
        }
        #endregion

        #region LoadAPI
        static readonly string[] Libs =
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "System.Xml.dll"
        };

        static void LoadAPI()
        {
            Global.API = new List<IAssembly>();
            string dir = GlobalSettings.LibsDirectory;
            foreach (var lib in Libs)
            {
                string path = Path.Combine(dir, lib);
                var assembly = LoadAssembly(path, false);
                Global.API.Add(assembly);
            }
        }
        #endregion

        #region LoadTestSuites
        static bool FilterCall(IMethod call)
        {
            IType type = call.DeclaringType;
            string ns = type.Namespace;
            if (ns.StartsWith("NUnit")) return false;
            return true;
        }

        static IMethod[] GetCalls(IMethod method)
        {
            if (method == null) return null;

            var body = method.Body;
            if (body == null) return null;

            var calls = body.GetCalls();
            if (calls == null) return null;
            
            var list = new List<IMethod>();
            foreach (var call in calls)
            {
                if (!FilterCall(call)) continue;
                list.Add(call);
            }

            if (list.Count <= 0) return null;

            return list.ToArray();
        }

        static void CacheCalls(TestNode node)
        {
            var calls = GetCalls(node.Method);
            if (calls != null)
            {
                //node.Calls.AddRange(calls);

                foreach (var call in calls)
                {
                    string name = ApiInfo.GetFullMethodName(call);
                    var list = Global.TestCache[name] as List<TestNode>;
                    if (list == null)
                    {
                        list = new List<TestNode>();
                        Global.TestCache[name] = list;
                    }
                    if (!list.Contains(node))
                        list.Add(node);
                }
            }
        }

        static void LoadTestSuites()
        {
			foreach (var type in Global.NUnitAssembly.GetTestFixtures())
                LoadTestSuite(type);
        }

        static void LoadTestSuite(IType type)
        {
			foreach (var test in type.GetUnitTests())
            {
                XmlElement[] elems = null;
                if (Global.NUnitReport != null)
                {
                    elems = FindNUnitElems(test);
                    if (elems == null) continue;
                    if (elems.Length == 0) continue;
                    if (!elems.Any(IsExecuted)) continue;
                }

                string fullname = GetTestCaseFullName(test);
                fullname = Utils.RemovePrefix(fullname, "mono.");

                int dot = fullname.LastIndexOf('.');
                string klass = fullname.Substring(0, dot);

                var parent = Global.TestRoot;
                var tc = new TestNode
                             {
                                 FullName = fullname,
                                 Name = fullname.Substring(dot + 1),
                                 Method = test
                             };

                CacheCalls(tc);

                if (elems != null && elems.Length > 0)
                {
                    foreach (var elem in elems)
                    {
                        if (!IsExecuted(elem)) continue;
                        var stat = tc.GetStat(GetRuntime(elem));
                        if (stat != null)
                        {
                            stat.Success = IsSuccess(elem);
                            stat.Time = GetTime(elem);
                        }
                    }
                }
                else
                {
                    tc.Success = true;
                }

                dot = klass.LastIndexOf('.');
                var path = new[] { klass.Substring(0, dot), klass.Substring(dot + 1) };
                for (int i = 0; i < path.Length; ++i)
                {
                    string tsname = path[i];
                    var ts = (TestNode)parent[tsname];
                    if (ts == null)
                    {
                        ts = new TestNode 
                                 { 
                                     Name = tsname
                                 };
                        ts.FullName = i == 1 ? klass : ts.Name;
                        parent.Add(ts);
                        parent = ts;
                    }
                    else
                    {
                        parent = ts;
                    }

                    parent.UpdateStats(tc);
                }
                parent.Add(tc);
            }
        }
        #endregion

        #region Utils
        static IAssembly LoadAssembly(string path, bool pfx)
        {
            try
            {
                CommonLanguageInfrastructure.SubstituteFrameworkAssemblies = pfx;
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);
				return CommonLanguageInfrastructure.Deserialize(path, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
                return null;
            }
        }

        static string GetTestCaseFullName(IMethod method)
        {
			return method.GetMonoTestCaseName();
        }

        static readonly string[] TestFixtures =
        {
            "DataDynamics.PageFX.FLI.Tests.ABCALL",
            "DataDynamics.PageFX.FLI.Tests.SWFALL",
        };

        static XmlElement[] FindNUnitElems(IMethod method)
        {
			string name = method.GetMonoTestCaseName();
            name = name.Replace('.', '_');

            //var declType = method.DeclaringType;
            //string name = declType.FullName + "." + method.Name;

            var list = new List<XmlElement>();
            foreach (var fixture in TestFixtures)
            {
                string xpath = string.Format("//test-case[@name = '{0}.{1}']", fixture, name);
                var e = Global.NUnitReport.SelectSingleNode(xpath) as XmlElement;
                if (e != null)
                    list.Add(e);
            }

            return list.ToArray();
        }

        static Runtime GetRuntime(XmlElement e)
        {
            string name = e.GetAttribute("name");
            if (name.Contains("SWFALL"))
                return Runtime.FP10;
            return Runtime.AVM;
        }

        static bool IsTrue(XmlElement elem, string attr)
        {
            return string.Compare(elem.GetAttribute(attr), "true", true) == 0;
        }

        static bool IsExecuted(XmlElement elem)
        {
            return IsTrue(elem, "executed");
        }

        static bool IsSuccess(XmlElement elem)
        {
            return IsTrue(elem, "success");
        }

        static double GetTime(XmlElement elem)
        {
            try
            {
                return XmlConvert.ToDouble(elem.GetAttribute("time"));
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}
