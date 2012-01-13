using System;
using DataDynamics.PageFX.FLI;
using NUnit.Core;
using NUnit.Framework;

namespace DataDynamics.PageFX.Tests
{
    public class AvmTest : NUnit.Core.TestCase
    {
        public AvmTest(TestCase tc) : base(tc.SuiteName, tc.Name)
        {
            _testCase = tc;
        }

        public override void Run(TestCaseResult result)
        {
            TestDriverSettings tds = new TestDriverSettings();
            tds.UpdateReport = true;
            tds.OutputFormat = "abc";
            _testCase.Optimize = true;
            FliTestDriver.RunTestCase(_testCase, tds);

            if (_testCase.HasErrors)
            {
                result.Error(new Exception(_testCase.Error));
            }
        }

        private readonly TestCase _testCase;
    }

    [TestFixture]
    public class AvmTests
    {
        [Suite]
        public static NUnit.Core.TestSuite AllTests
        {
            get
            {
                NUnit.Core.TestSuite suite = new NUnit.Core.TestSuite("AvmTests");
                foreach (TestCase tc in SimpleTestCases.GetAllTestCases())
                {
                    suite.Add(new AvmTest(tc));
                }
                return suite;
            }
        }
    }
}