using System;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	[Flags]
	public enum AbcMethodSemantics
	{
		Default = 0,
		Static = 1,
		Virtual = 2,
		Override = 4,
		VirtualOverride = Virtual | Override,
	}
}