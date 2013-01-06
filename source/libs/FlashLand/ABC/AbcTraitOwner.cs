using System;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	[Flags]
    public enum AbcTraitOwner
    {
        None,
        Instance = 1,
        Script = 2,
        MethodBody = 4,
        All = Instance | Script | MethodBody,
    }
}