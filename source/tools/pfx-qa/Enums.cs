namespace DataDynamics.PageFX
{
    internal enum Runtime
    {
        AVM,
        FP10,
    }

    internal enum NodeKind
    {
        Unknown,
        Test,
        Assembly,
        Namespace,
        Class,
        Method
    }
}