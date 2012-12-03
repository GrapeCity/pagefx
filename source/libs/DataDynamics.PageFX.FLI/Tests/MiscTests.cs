#if NUNIT
using DataDynamics.PageFX.FLI;
using NUnit.Framework;

namespace DataDynamics.PageFX.FlashLand.Tests
{
	public class MiscTests
	{
		[Test]
		public void TestGetMinBits()
		{
			Assert.AreEqual(1, ((byte)1).GetMinBits());
			Assert.AreEqual(1, 0.GetMinBits());
			Assert.AreEqual(11, 1000.GetMinBits(1000));
			Assert.AreEqual(1, (-1).GetMinBits());
			Assert.AreEqual(2, (-2).GetMinBits());

			for (int i = 0; i < 32; ++i)
			{
				uint n = (uint)(1 << i);
				Assert.AreEqual(i + 1, n.GetMinBits());
			}
		}
	}
}
#endif