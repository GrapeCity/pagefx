using System;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    public enum VM
    {
        AVM,
        CLR,
    }

    [Flags]
    public enum TestCaseFlags
    {
        None,
        Started = 1,
        Finished = 2,
        Cancelled = 4,
    }
}