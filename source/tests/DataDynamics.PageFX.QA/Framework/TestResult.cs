using System;
using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX
{
    public class TestResult
    {
        public List<TestCase> FailedTestCases;
        public List<TestSuite> Suites;

        public void Sort()
        {
            Suites.Sort(delegate(TestSuite x, TestSuite y)
                            {
                                return string.Compare(x.FullName, y.FullName, true);
                            });
        }

        public bool HasSuite(TestSuite ts)
        {
            return Suites.Contains(ts);
        }

        public void GenerateHtmlReport(string path)
        {
            using (var writer = new StreamWriter(path))
                GenerateHtmlReport(writer);
        }

        public void GenerateHtmlReport(TextWriter writer)
        {
            writer.WriteLine("<html>");

            writer.WriteLine("<head>");
            writer.WriteLine("<style>");
            writer.WriteLine("\t.bold { font-weight: bold; }");
            writer.WriteLine("\t.err { background-color: red; font-weight: bold; }");
            writer.WriteLine("\t.ok { background-color: lime; }");
            writer.WriteLine("\ttd, th { border-style: solid; border-color: black; border-width: 1px; }");
            writer.WriteLine("</style>");
            writer.WriteLine("</head>");

            writer.WriteLine("<body>");

            if (Suites != null && Suites.Count > 0)
            {
                writer.WriteLine("<h1>Test Results</h1>");
                writer.WriteLine("<p>Creation Date: {0:yyyy-MM-dd}</p>", DateTime.Now);

                writer.WriteLine("<table cellspacing=\"0\">");
                writer.WriteLine("<tr>");
                writer.WriteLine("<th>Suite Name</th>");
                writer.WriteLine("<th>N</th>");
                writer.WriteLine("<th>Passed</th>");
                writer.WriteLine("<th>Failed</th>");
                writer.WriteLine("<th>Percentage</th>");
                writer.WriteLine("</tr>");

                foreach (var ts in Suites)
                {
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<td><b>{0}</b></td>", ts.FullName);
                    writer.WriteLine("<td>{0}</td>", ts.Total);
                    writer.WriteLine("<td>{0}</td>", ts.TotalPassed);
                    writer.WriteLine("<td>{0}</td>", ts.TotalFailed);
                    writer.WriteLine("<td>{0:P}</td>", ts.Percentage);
                    writer.WriteLine("</tr>");
                }

                writer.WriteLine("</table>");
            }

            if (FailedTestCases != null && FailedTestCases.Count > 0)
            {
                writer.WriteLine("<h1>Failed Tests</h1>");

                writer.WriteLine("<table cellspacing=\"0\">");
                writer.WriteLine("<tr>");
                writer.WriteLine("<th>Test Name</th>");
                writer.WriteLine("<th>CLR Output</th>");
                writer.WriteLine("<th>AVM Output</th>");
                writer.WriteLine("</tr>");

                foreach (var tc in FailedTestCases)
                {
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<td>{0}</td>", tc.FullDisplayName);
                    writer.WriteLine("<td><pre>\n{0}\n</pre></td>", XmlHelper.EntifyString(tc.Output1));
                    writer.WriteLine("<td><pre>\n{0}\n</pre></td>", XmlHelper.EntifyString(tc.Output2));
                    writer.WriteLine("</tr>");
                }

                writer.WriteLine("</table>");
            }

            writer.WriteLine("</body>");

            writer.WriteLine("</html>");
        }

        public void GenerateWikiReport(string path)
        {
            using (var writer = new StreamWriter(path))
                GenerateWikiReport(writer);
        }

        public void GenerateWikiReport(TextWriter writer)
        {
            writer.WriteLine("!!PageFX Tests Results");
            writer.WriteLine("*Creation Date*: {0:yyyy-MM-dd}", DateTime.Now);

            if (Suites != null && Suites.Count > 0)
            {
                writer.WriteLine("!!Summary Table");
                writer.WriteLine("||*Suite Name*||*N*||*Passed*||*Failed*||*Percentage*||");

                foreach (var ts in Suites)
                {
                    writer.WriteLine("||*{0}*||{1}||{2}||{3}||{4:P}||",
                                     ts.Name, ts.Total, ts.TotalPassed, ts.TotalFailed, ts.Percentage);
                }
            }

            //if (FailedTestCases != null && FailedTestCases.Count > 0)
            //{
            //    writer.WriteLine("!!Failed Tests");

            //    writer.WriteLine("||*Test Name*||*CLR Output*||*AVM Output*||");
            //    foreach (TestCase tc in FailedTestCases)
            //    {
            //        writer.WriteLine("||*{0}*||{1}||{2}||",
            //                         tc.FullDisplayName, tc.Output1, tc.Output2);
            //    }
            //}
        }
    }
}