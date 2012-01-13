using System;
using DataDynamics.PageFX.FLI;

namespace DataDynamics.PageFX
{
    partial class PFC
    {
        #region Tests
#if DEBUG
        const int test = -1;

        static bool RunTests()
        {
            switch (test)
            {
                    //config
                case 0:
                    Console.WriteLine(PfxConfig.Compiler.ExceptionBreak);
                    return true;

                case 1:
                    RunCodeIn(@"E:\Work\Projects\PageFX\Source\sdks\flex.3.0\libs",
                              () =>
                              WrapperGenerator.Wrap("/wrap /xdoc:flex3.xml /dir:mx_src /out:flex3.dll framework.swc"));
                    return true;

                case 2:
                    cl = CommandLine.Parse("/ccs");
                    MainCore();
                    return true;


                default:
                    return false;
            }
        }
#endif
        #endregion
    }
}