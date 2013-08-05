#if NUNIT
using System;
using DataDynamics.PageFX.Flash.IL;
using NUnit.Framework;

namespace DataDynamics.PageFX.Flash.Tests
{
	[TestFixture]
	public class InstructionsTest
	{
		[Test]
		public static void TestLoad()
		{
			foreach (var c in (InstructionCode[])Enum.GetValues(typeof(InstructionCode)))
			{
				Assert.IsNotNull(Instructions.GetInstruction(c));
			}
		}
	}
}
#endif