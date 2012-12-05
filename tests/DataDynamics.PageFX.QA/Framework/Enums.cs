using System;

namespace DataDynamics.PageFX
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