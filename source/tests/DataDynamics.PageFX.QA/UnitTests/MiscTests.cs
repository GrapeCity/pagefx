using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace DataDynamics.Tests
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

        [Test]
        public void TestMinBits()
        {
            Assert.AreEqual(1, BitHelper.GetMinBits((byte)1));
            Assert.AreEqual(1, BitHelper.GetMinBits(0));
            Assert.AreEqual(11, BitHelper.GetMinBits(1000, 1000));
            Assert.AreEqual(1, BitHelper.GetMinBits(-1));
            Assert.AreEqual(2, BitHelper.GetMinBits(-2));

            for (int i = 0; i < 32; ++i)
            {
                uint n = (uint)(1 << i);
                Assert.AreEqual(i + 1, BitHelper.GetMinBits(n));
            }
        }

        public void TestToFullPath(string path, string expected)
        {
            string fp = PathHelper.ToFullPath(path);
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
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance(null));
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance(""));
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance("{}"));
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance("aaa { bbb } ccc"));
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance("{{"));
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance("}}"));
            Assert.IsTrue(ParseHelper.CheckFormatBraceBalance("{{{}}}"));

            Assert.IsFalse(ParseHelper.CheckFormatBraceBalance("{"));
            Assert.IsFalse(ParseHelper.CheckFormatBraceBalance("}"));
            Assert.IsFalse(ParseHelper.CheckFormatBraceBalance("aaa {"));
            Assert.IsFalse(ParseHelper.CheckFormatBraceBalance("aaa }"));
            Assert.IsFalse(ParseHelper.CheckFormatBraceBalance("aaa { {{bbb } ccc"));
            Assert.IsFalse(ParseHelper.CheckFormatBraceBalance("aaa { }}bbb } ccc"));
        }
    }
}