using System;
using NUnit.Framework;

namespace DataDynamics.Tests
{
    public class ArithmeticTest
    {
        private const uint SIGN = 0x80000000;

        public static int AddOvf(int x, int y)
        {
            uint s1 = (uint)x & SIGN;
            uint s2 = (uint)y & SIGN;
            x = x + y;
            if (s1 == s2 && ((uint)x & SIGN) != s1)
                throw new OverflowException();
            return x;
        }

        public static uint AddOvf(uint x, uint y)
        {
            uint c = x + y;
            if (c < x)
                throw new OverflowException();
            return c;
        }

        public static int SubOvf(int x, int y)
        {
            uint s1 = (uint)x & SIGN;
            uint s2 = (uint)y & SIGN;
            x = x - y;
            if (s1 != s2 && ((uint)x & SIGN) != s1)
                throw new OverflowException();
            return x;
        }

        public static uint MulOvf(uint x, uint y)
        {
            int m = Tricks.Nlz(x);
            int n = Tricks.Nlz(y);
            if (m + n <= 30)
                throw new OverflowException();
            uint t = x * (y >> 1);
            if ((int)t < 0)
                throw new OverflowException();
            uint z = t * 2;
            if ((y & 1) != 0)
            {
                z = z + x;
                if (z < x)
                    throw new OverflowException();
            }
            return z;
        }

        public static int MulOvf(int x, int y)
        {
            if (y < 0 && x == int.MinValue)
                throw new OverflowException();

            int z = x * y;
            if (y != 0 && z / y != x)
                throw new OverflowException();

            return z;
        }

        private delegate void Code();

        private static void ExpectException(Type type, Code code)
        {
            try
            {
                code();
            }
            catch (Exception exc)
            {
                Assert.IsTrue(exc.GetType() == type);
                return;
            }
            Assert.Fail("No expected exception of type {0}" + type.FullName);
        }

        [Test]
        public void TestAddOvf()
        {
            ExpectException(typeof(OverflowException), () => AddOvf(int.MaxValue, 1));
            ExpectException(typeof(OverflowException), () => AddOvf(uint.MaxValue, 1u));
            ExpectException(typeof(OverflowException), () => AddOvf(1u, uint.MaxValue));
            Assert.AreEqual(9, AddOvf(4, 5));
        }

        [Test]
        public void TestSubOvf()
        {
            ExpectException(typeof(OverflowException), () => SubOvf(int.MinValue, 1));
            Assert.AreEqual(4, SubOvf(7, 3));
        }

        private static void TestMulOvf(uint x, uint y)
        {
            uint z1 = 0;
            bool ovf = false;
            try
            {
                z1 = MulOvf(x, y);
            }
            catch (OverflowException)
            {
                ovf = true;
            }

            try
            {
                checked
                {
                    uint z2 = x * y;
                    Assert.IsFalse(ovf, "#1: overfow in z1");
                    Assert.AreEqual(z2, z1, "#2: z1 != z2");
                }
            }
            catch (OverflowException)
            {
                Assert.IsTrue(ovf, "#3: no overflow in z1");
            }
        }

        [Test]
        public void TestMulOvfUnsigned()
        {
            TestMulOvf(uint.MaxValue, uint.MaxValue);

            uint a = uint.MaxValue >> 16;
            uint b = uint.MaxValue >> 15;

            for (uint x = a; x <= b; ++x)
            {
                TestMulOvf(x, x);
                TestMulOvf(x, a);
                TestMulOvf(x, b);
                TestMulOvf(x, b - x);
            }
        }

        private static void TestMulOvf(int x, int y)
        {
            int z1 = 0;
            bool ovf = false;
            try
            {
                z1 = MulOvf(x, y);
            }
            catch (OverflowException)
            {
                ovf = true;
            }

            try
            {
                checked
                {
                    int z2 = x * y;
                    Assert.IsFalse(ovf, "#1: overfow in z1");
                    Assert.AreEqual(z2, z1, "#2: z1 != z2");
                }
            }
            catch (OverflowException)
            {
                Assert.IsTrue(ovf, "#3: no overflow in z1");
            }
        }

        [Test]
        public void TestMulOvfSigned()
        {
            TestMulOvf(int.MaxValue, int.MaxValue);
            TestMulOvf(int.MaxValue, -1);
            TestMulOvf(-1, int.MaxValue);

            int a = (int)(uint.MaxValue >> 16);
            int b = (int)(uint.MaxValue >> 15);

            for (int x = a; x < b; ++x)
            {
                TestMulOvf(x, x);
                TestMulOvf(x, a);
                TestMulOvf(x, b);
                TestMulOvf(x, b - x);
                TestMulOvf(x, -x);
                TestMulOvf(x, -a);
                TestMulOvf(x, -b);
            }
        }
    }
}