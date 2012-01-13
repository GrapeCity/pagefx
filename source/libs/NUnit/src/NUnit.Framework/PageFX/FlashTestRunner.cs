using System;
using System.IO;
using System.Collections.Generic;

namespace DataDynamics.PageFX.NUnit
{
    class FlashTestRunner
    {
        static readonly List<Test> Tests = new List<Test>();
        static readonly Report Report = new Report();
        
        public static void Register(Test test)
        {
            Tests.Add(test);
        }

        public static void Run()
        {
            foreach (var test in Tests)
            {
                if (CategoryFilter.IsIncluded(test.Categories))
                {
                    test.Run();
                    Report.AddTest(test);
                }
            }
            var w = new StringWriter();
            Report.Write(w);
            Console.WriteLine(w.ToString());
            Console.WriteLine("<%END%>");
        }
    }
}