#if NUNIT
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Utilities;
using NUnit.Framework;

namespace DataDynamics.PageFX.Common.Tests
{
    [TestFixture]
    public class MiscTests
    {
        [Test]
        public void TestNLZ()
        {
            Assert.AreEqual(32, Tricks.Nlz(0));
            Assert.AreEqual(31, Tricks.Nlz(1));
            Assert.AreEqual(16, Tricks.Nlz(0x0000FFFF));
            Assert.AreEqual(14, Tricks.Nlz(0x0003FFFF));
            Assert.AreEqual(0, Tricks.Nlz(0xF000FFFF));
        }

        [Test]
        public void TestNlzBitwiseViaNlz()
        {
            for (int i = -100; i < 100; ++i)
            {
                Assert.AreEqual(Tricks.NlzBitwise((uint)i), Tricks.Nlz((uint)i));
            }
        }

        //[Test]
        //public void TestNlzPerformance()
        //{
        //    int a = -1000000;
        //    int b = -a;
        //    var z1 = new List<int>();
        //    var z2 = new List<int>();

        //    int start = Environment.TickCount;
        //    for (int i = a; i < b; ++i)
        //    {
        //        int z = Tricks.Nlz((uint)i);
        //        z1.Add(z);
        //    }
        //    int t1 = Environment.TickCount - start;

        //    start = Environment.TickCount;
        //    for (int i = a; i < b; ++i)
        //    {
        //        int z = Tricks.NlzBitwise((uint)i);
        //        z2.Add(z);
        //    }
        //    int t2 = Environment.TickCount - start;

        //    Debug.WriteLine(string.Format("nlz = {0}, nlz_bitwise = {1}", t1, t2));

        //    Assert.IsTrue(z1.Count == z2.Count);
        //}

        public void TestToFullPath(string path, string expected)
        {
            string fp = path.ToFullPath();
            Assert.AreEqual(expected, fp);
        }

        [Test]
        public void TestToFullPath()
        {
            TestToFullPath("/locale/en_US/../images/aaa.png", "/locale/images/aaa.png");
        }

        [Test]
        public void CheckBraceBalanceInFormatStrings()
        {
            Assert.IsTrue(ParseExtensions.CheckFormatBraceBalance(null));
            Assert.IsTrue("".CheckFormatBraceBalance());
            Assert.IsTrue("{}".CheckFormatBraceBalance());
            Assert.IsTrue("aaa { bbb } ccc".CheckFormatBraceBalance());
            Assert.IsTrue("{{".CheckFormatBraceBalance());
            Assert.IsTrue("}}".CheckFormatBraceBalance());
            Assert.IsTrue("{{{}}}".CheckFormatBraceBalance());

            Assert.IsFalse("{".CheckFormatBraceBalance());
            Assert.IsFalse("}".CheckFormatBraceBalance());
            Assert.IsFalse("aaa {".CheckFormatBraceBalance());
            Assert.IsFalse("aaa }".CheckFormatBraceBalance());
            Assert.IsFalse("aaa { {{bbb } ccc".CheckFormatBraceBalance());
            Assert.IsFalse("aaa { }}bbb } ccc".CheckFormatBraceBalance());
        }
    }
}
#endif