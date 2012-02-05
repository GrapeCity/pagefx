using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
#if !AVM
using System.Xml.Xsl;
#endif

namespace DataDynamics.PageFX.NUnit
{
    class Report
    {
        public Report()
        {
            Name = "report";
        }

        public string Name { get; set; }

        int Total;
        int Failures;
        int NotRun;

        readonly List<TestSuite> _suites = new List<TestSuite>();

        #region AddTest
        public void AddTest(Test test)
        {
            Total++;
            if (!test.Success)
                Failures++;
            if (!test.Executed)
                NotRun++;

            var root = AddSuite(null, test.AssemblyPath);

            string fullname = test.SuiteName;
            int dot = fullname.IndexOf('.');

            TestSuite parent = root;
            TestSuite suite = null;
            bool find = true;
            bool br = false;
            while (true)
            {
                parent.Update(test);

                if (br) break;

                br = dot < 0;
                string name = br ? fullname : fullname.Substring(0, dot);

                if (find)
                    suite = FindSuite(parent, name);

                if (!find || suite == null)
                {
                    suite = new TestSuite { Name = name };
                    parent.Suites.Add(suite);
                    find = false;
                }

                parent = suite;

                if (dot >= 0)
                    dot = fullname.IndexOf('.', dot + 1);
            }

            suite.Tests.Add(test);
            suite.Description = test.SuiteDescription;
            test.Suite = suite;
        }

        TestSuite FindSuite(TestSuite parent, string name)
        {
            var list = parent == null ? _suites : parent.Suites;

            foreach (var suite in list)
            {
                if (suite.Name == name)
                    return suite;
            }

            return null;
        }

        TestSuite AddSuite(TestSuite parent, string name)
        {
            var s = FindSuite(parent, name);
            if (s != null) return s;
            s = new TestSuite {Name = name};
            if (parent != null)
                parent.Suites.Add(s);
            else
                _suites.Add(s);
            return s;
        }
        #endregion

        #region Save
        public void Write(XmlWriter writer)
        {
            writer.WriteStartElement("test-results");
            string name = Name;
            if (string.IsNullOrEmpty(name))
                name = "report";
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("total", Total.ToString());
            writer.WriteAttributeString("failures", Failures.ToString());
            writer.WriteAttributeString("not-run", NotRun.ToString());
            var dt = DateTime.Now;
            writer.WriteAttributeString("date", dt.ToString("yyyy-MM-dd"));
            writer.WriteAttributeString("time", dt.ToString("HH:mm:ss"));
            
            foreach (var suite in _suites)
                suite.Write(writer);

            writer.WriteEndElement();
        }

        public void Write(TextWriter writer)
        {
            var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
            using (var xwriter = XmlWriter.Create(writer, xws))
                Write(xwriter);
        }

#if !AVM
        public void Save(string path)
        {
            var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
            using (var writer = XmlWriter.Create(path, xws))
                Write(writer);
        }
#endif
        #endregion

        #region Transform
#if !AVM
        public static void Transform(string path, string resultsFile, string xsltPath)
        {
            if (string.IsNullOrEmpty(xsltPath)) return;

            if (!Path.IsPathRooted(xsltPath))
                xsltPath = Path.Combine(Environment.CurrentDirectory, xsltPath);

            if (!File.Exists(xsltPath))
            {
                return;
            }

            try
            {
                var transform = new XslCompiledTransform();
                transform.Load(xsltPath);
                transform.Transform(path, resultsFile);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                Environment.Exit(-1);
            }
        }
#endif
        #endregion

        public static void WriteAttrs(XmlWriter writer, IStat stat)
        {
            writer.WriteAttributeString("success", stat.Success.ToString());
            writer.WriteAttributeString("time", XmlConvert.ToString(Math.Round(stat.Time, 2)));
            writer.WriteAttributeString("asserts", XmlConvert.ToString(stat.Asserts));
        }
    }
}