#if NUNIT
using System;
using DataDynamics.PageFX.FLI.IL;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
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