using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics;
using DataDynamics.PageFX;
using DataDynamics.PageFX.FLI;

namespace pfx_bench
{
    class Program
    {
        const string ReportsDir = @"c:\pfxdata\bench";
        
        private static string lang = "csharp";
        private static string buildNumber = "2.7.1.8";
        private const int NumberOfTimesToRunBenchmark = 5;

        static void RunEngine(string ext)
        {
            var tds = new TestDriverSettings();
            var collection = LoadBenchmarks(ext);
            tds.OutputFormat = "swf";
            var duration = new Dictionary<string, int>();
            foreach (var tc in collection)
            {
                tc.UsePfc = true;
                tc.IsBenchmark = true;
                Console.WriteLine(" Running [  {0}  ] benchmark...", tc.Name);
                int d = 0;
                for (int i = 0; i < NumberOfTimesToRunBenchmark; i++)
                {
                    TestEngine.RunTestCase(tc, tds);
                    d += GetDuration(tc);
                }
                duration[Path.GetFileNameWithoutExtension(tc.Name)] = (d / NumberOfTimesToRunBenchmark);
            }
            WritePerformanceReport(duration);
        }

        static int GetDuration(TestCase tc)
        {
            try
            {
                using (var reader = new StringReader(tc.Output2))
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine("error: bad output in benchmark {0}", tc.Name);
                        return 0;
                    }
                    return Convert.ToInt32(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error: bad output in benchmark {0}", tc.Name);
                return 0;
            }
        }

        private static string GetOutputFileName(string name)
        {
            string path = Path.Combine(ReportsDir, name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, buildNumber + ".txt");
        }

        private static void WritePerformanceReport(Dictionary<string, int> duration)
        {
            foreach (var testCase in duration.Keys)
            {
                using (var writer = new StreamWriter(GetOutputFileName(testCase)))
                {
                    writer.Write(duration[testCase]);
                }
            }
            
        }

        private static IEnumerable<TestCase> LoadBenchmarks(string ext)
        {
            return SimpleTestCases.Load("PageFX.Benchmark", ext);
        }

        static int Main(string[] args)
        {
            var cl = CommandLine.Parse(args);
            if (cl == null)
                cl = new CommandLine();

            try
            {
                buildNumber = cl.GetOption(null, "b", "build");

                // TODO: add parse lang from console

                if (lang == "csharp")
                {
                    RunEngine(".cs");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }
    }
}
