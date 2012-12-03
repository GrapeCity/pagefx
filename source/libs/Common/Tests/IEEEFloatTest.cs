#if NUNIT
using System;
using NUnit.Framework;

namespace DataDynamics.PageFX
{
	[TestFixture]
	public class IEEEFloatTest
	{
        static unsafe byte[] GetBytes(double val)
        {
            var buf = new byte[8];
            fixed (byte* p = buf)
                *((double*)p) = val;
            return buf;
        }

        static unsafe byte[] GetBytes(float val)
        {
            var buf = new byte[4];
            fixed (byte* p = buf)
                *((float*)p) = val;
            return buf;
        }

        private static void AreEqual(byte[] expected, byte[] actual)
        {
            int n = expected.Length;
            Assert.AreEqual(n, actual.Length);
            for (int i = 0; i < n; ++i)
            {
                if (expected[i] != actual[i])
                    Assert.Fail("Byte arrays differ at index {0}. Expected is {1}, actual is {2}.",
                        i, expected[i], actual[i]);
            }
        }

        private static void TestNumber(double value)
        {
            var expected = GetBytes(value);
            double result = IEEEFloat.ToDouble(expected, 0);
            var actual = GetBytes(result);
            AreEqual(expected, actual);
        }

        private static void TestNumber(float value)
        {
            var expected = GetBytes(value);
            float result = IEEEFloat.ToFloat(expected, 0);
            var actual = GetBytes(result);
            AreEqual(expected, actual);
        }

	    [Test]
		public void TestDoublePrecision()
		{
            TestNumber(0.0);
            //TestNumber(-0.0);
			TestNumber(4.3);
            TestNumber(Math.PI);
            TestNumber(double.NegativeInfinity);
            TestNumber(double.PositiveInfinity);
            TestNumber(double.NaN);

            int start = 120; // pick val < 127
            int end = 135; // pick val > 128

            var buf = new byte[8];
            for (int b0 = start; b0 < end; ++b0)
            {
                for (int b1 = start; b1 < end; ++b1)
                {
                    for (int b2 = start; b2 < end; ++b2)
                    {
                        for (int b3 = start; b3 < end; ++b3)
                        {
                            buf[0] = (byte)b0;
                            buf[1] = (byte)b1;
                            buf[2] = (byte)b2;
                            buf[3] = (byte)b3;
                            buf[4] = (byte)b0;
                            buf[5] = (byte)b1;
                            buf[6] = (byte)b2;
                            buf[7] = (byte)b3;
                            double val = IEEEFloat.ToDouble(buf, 0);
                            if (!double.IsNaN(val))
                            {
                                var buf2 = GetBytes(val);
                                AreEqual(buf, buf2);
                            }
                        }
                    }
                }
            }
		}

        [Test]
        public void TestSinglePrecision()
        {
            TestNumber(0f);
            //TestNumber(-0f);
            TestNumber(4.3f);
            TestNumber(float.NegativeInfinity);
            TestNumber(float.PositiveInfinity);
            TestNumber(float.NaN);

            int start = 120; // pick val < 127
            int end = 135; // pick val > 128

            var buf = new byte[4];
            for (int b0 = start; b0 < end; ++b0)
            {
                for (int b1 = start; b1 < end; ++b1)
                {
                    for (int b2 = start; b2 < end; ++b2)
                    {
                        for (int b3 = start; b3 < end; ++b3)
                        {
                            buf[0] = (byte) b0;
                            buf[1] = (byte) b1;
                            buf[2] = (byte) b2;
                            buf[3] = (byte) b3;
                            float val = IEEEFloat.ToFloat(buf, 0);
                            if (!float.IsNaN(val))
                            {
                                var buf2 = GetBytes(val);
                                AreEqual(buf, buf2);
                            }
                        }
                    }
                }
            }
        }
	}
}
#endif