using System;
using DataDynamics;
using DataDynamics.PageFX.NUnit;

namespace pfx_nunit_console
{
    class Program
    {
        static CommandLine cl;

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
#if DEBUG
                if (RunTests())
                {
                    return 0;
                }
#endif
                ConsoleTestRunner.Usage();
                return 0;
            }

            cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Console.WriteLine("Unable to parse command line.");
                return -1;
            }
            ConsoleTestRunner.cl = cl;

            if (cl.HasOption("help", "?"))
            {
                ConsoleTestRunner.Usage();
                return 0;
            }

            MainCore();
            return 0;
        }

        static void MainCore()
        {
            ConsoleTestRunner.Run(cl);
        }

        static void RunCodeIn(string dir, Action action)
        {
            string curdir = Environment.CurrentDirectory;
            try
            {
                Environment.CurrentDirectory = dir;
                action();
            }
            finally
            {
                Environment.CurrentDirectory = curdir;
            }
        }

#if DEBUG
        const int test = -1;

        static bool RunTests()
        {
            switch (test)
            {
                case 1:
                    RunCodeIn(@"c:\dsamples\nunit-tests\1",
                              () =>
                                  {
                                      //cl = CommandLine.Parse("/nunit suite.dll suite2.dll");
                                      //cl = CommandLine.Parse("/nunit /xmlConsole suite.dll suite2.dll");
                                      //cl = CommandLine.Parse("/nunit /xml:report.xml suite.dll suite2.dll");
                                      //cl = CommandLine.Parse("/nunit /fixture:NS1.Suite /xml:report.xml suite.dll suite2.dll");
                                      cl =
                                          CommandLine.Parse(
                                              "/nunit /include:A|C /xml:report.xml suite.dll suite2.dll");
                                      //cl = CommandLine.Parse("/nunit /exclude:B /xml:report.xml suite.dll suite2.dll");
                                      //cl = CommandLine.Parse("/nunit /transform:summary.xslt /transformOut:report.txt /xml:report.xml suite.dll suite2.dll");
                                      MainCore();
                                  }
                        );
                    return true;

                case 2:
                    RunCodeIn(@"c:\dsamples\TestSuite1",
                              () =>
                                  {
                                      cl = CommandLine.Parse("/nunit /xmlConsole suite.swf");
                                      MainCore();
                                  }
                        );
                    return true;

                default:
                    return false;
            }
        }
#endif
    }
}
