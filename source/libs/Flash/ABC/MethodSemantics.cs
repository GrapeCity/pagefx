using System;

namespace DataDynamics.PageFX.Flash.Abc
{
	[Flags]
	public enum MethodSemantics
	{
		Default = 0,
		Static = 1,
		Virtual = 2,
		Override = 4,
		Abstract = 8,
		VirtualOverride = Virtual | Override,
	}
}