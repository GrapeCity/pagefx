using System;
using NUnit.Framework;

namespace DataDynamics.PageFX.TestRunner.UnitTests
{
    [TestFixture]
    public class Int64TestDriver
    {
        private static readonly Random random = new Random();

        private static long RandLong()
        {
            long a = (uint)random.Next() << 32 | (uint)random.Next();
            long b = (uint)random.Next() << 32 | (uint)random.Next();
            return a - b;
        }

        private static long RandLong2()
        {
            return (uint)random.Next() << 32 | (uint)random.Next();
        }

        private static ulong RandULong()
        {
            return (uint)random.Next() << 32 | (uint)random.Next();
        }


        private static void AreEquals(bool x, bool y)
        {
            Assert.IsTrue(x == y);
        }

        private static void AreEquals(long x, Int64 y)
        {
            Assert.IsTrue(y == (Int64)x);
        }

        private static void AreEquals(ulong x, UInt64 y)
        {
            Assert.IsTrue(y == (UInt64)x);
        }

        private static void TestBinary(long a, long b)
        {
            AreEquals(a + b, (Int64)a + (Int64)b);
            AreEquals(a - b, (Int64)a - (Int64)b);
            AreEquals(a * b, (Int64)a * (Int64)b);
            AreEquals(a / b, (Int64)a / (Int64)b);
            AreEquals(a % b, (Int64)a % (Int64)b);

            //Bitwise operatios
            AreEquals(a | b, (Int64)a | (Int64)b);
            AreEquals(a & b, (Int64)a & (Int64)b);
            AreEquals(a ^ b, (Int64)a ^ (Int64)b);

            //Comparison operations
            AreEquals(a == b, (Int64)a == (Int64)b);
            AreEquals(a != b, (Int64)a != (Int64)b);
            AreEquals(a < b, (Int64)a < (Int64)b);
            AreEquals(a <= b, (Int64)a <= (Int64)b);
            AreEquals(a > b, (Int64)a > (Int64)b);
            AreEquals(a >= b, (Int64)a >= (Int64)b);
        }

        private static void TestBinary(ulong a, ulong b)
        {
            AreEquals(a + b, (UInt64)a + (UInt64)b);
            AreEquals(a - b, (UInt64)a - (UInt64)b);
            AreEquals(a * b, (UInt64)a * (UInt64)b);
            AreEquals(a / b, (UInt64)a / (UInt64)b);
            AreEquals(a % b, (UInt64)a % (UInt64)b);

            //Bitwise operatios
            AreEquals(a | b, (UInt64)a | (UInt64)b);
            AreEquals(a & b, (UInt64)a & (UInt64)b);
            AreEquals(a ^ b, (UInt64)a ^ (UInt64)b);

            //Comparison operations
            AreEquals(a == b, (UInt64)a == (UInt64)b);
            AreEquals(a != b, (UInt64)a != (UInt64)b);
            AreEquals(a < b, (UInt64)a < (UInt64)b);
            AreEquals(a <= b, (UInt64)a <= (UInt64)b);
            AreEquals(a > b, (UInt64)a > (UInt64)b);
            AreEquals(a >= b, (UInt64)a >= (UInt64)b);
        }

        private static void TestUnary(long a)
        {
            AreEquals(-a, -(Int64)a);
            AreEquals(~a, ~(Int64)a);
        }

        private static void TestUnary(ulong a)
        {
            AreEquals(~a, ~(UInt64)a);
        }

        private static void TestShifts(long a, int n)
        {
            AreEquals(a << n, (Int64)a << n);
            AreEquals(a >> n, (Int64)a >> n);
        }

        private static void TestShifts(ulong a, int n)
        {
            AreEquals(a << n, (UInt64)a << n);
            AreEquals(a >> n, (UInt64)a >> n);
        }

        private static void TestCasts(long a, Int64 b)
        {
            Assert.AreEqual((sbyte)a, (sbyte)b);
            Assert.AreEqual((byte)a, (byte)b);
            Assert.AreEqual((short)a, (short)b);
            Assert.AreEqual((ushort)a, (ushort)b);
            Assert.AreEqual((int)a, (int)b);
            Assert.AreEqual((uint)a, (uint)b);
            Assert.AreEqual((ulong)a, (ulong)b);
            Assert.AreEqual((float)a, (float)b);
            Assert.AreEqual((double)a, (double)b);
        }

        private static void TestCasts(ulong a, UInt64 b)
        {
            Assert.AreEqual((sbyte)a, (sbyte)b);
            Assert.AreEqual((byte)a, (byte)b);
            Assert.AreEqual((short)a, (short)b);
            Assert.AreEqual((ushort)a, (ushort)b);
            Assert.AreEqual((int)a, (int)b);
            Assert.AreEqual((uint)a, (uint)b);
            Assert.AreEqual((long)a, (long)b);
            Assert.AreEqual((float)a, (float)b);
            Assert.AreEqual((double)a, (double)b);
        }

        private static void Test(long a, long b)
        {
            TestBinary(a, b);
            TestUnary(a);
            for (int i = 100; i >= -100; --i)
                TestShifts(a, i);
            TestCasts(a, (Int64)a);
            TestCasts(b, (Int64)b);
        }

        private static void Test(ulong a, ulong b)
        {
            TestBinary(a, b);
            TestUnary(a);
            for (int i = 100; i >= -100; --i)
                TestShifts(a, i);
            TestCasts(a, (UInt64)a);
            TestCasts(b, (UInt64)b);
        }

        [Test]
        public void TestI64()
        {
            for (int i = 0; i < 1000; ++i)
            {
                long a = RandLong();
                long b = RandLong();
                Test(a, b);
                a = RandLong2();
                b = RandLong2();
                Test(a, b);
            }
        }

        [Test]
        public void TestU64()
        {
            for (int i = 0; i < 1000; ++i)
            {
                ulong a = RandULong();
                ulong b = RandULong();
                Test(a, b);
            }
        }

        [Test]
        public void TestMisc()
        {
            var v = UInt64.Divide(new UInt64(11u), new UInt64(10u));
            AreEquals(1UL, v);
        }
    }
}