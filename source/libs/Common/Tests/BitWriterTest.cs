#if NUNIT
using System;
using System.IO;
using DataDynamics.PageFX.Common.IO;
using NUnit.Framework;

namespace DataDynamics.PageFX.Common.Tests
{
    [TestFixture]
    public class BitWriterTest
    {
        [Test]
        public void WriteBit()
        {
            var random = new Random();
            var ms = new MemoryStream();
            var writer = new BitWriter(ms);
            int n = 8 * 8;
            var bits = new bool[n];
            for (int i = 0; i < n; ++i)
            {
                bits[i] = random.Next(-10, 10) > 0;
                writer.WriteBit(bits[i]);
            }
            Assert.AreEqual(n / 8, ms.Length);
            ms.Position = 0;
            var reader = new BitReader(ms);
            for (int i = 0; i < n; ++i)
            {
                Assert.AreEqual(bits[i], reader.ReadBit());
            }
        }

        [Test]
        public void WriteCode()
        {
            int[] codes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ms = new MemoryStream();
            var writer = new BitWriter(ms);
            foreach (int code in codes)
            	writer.WriteCode(code, 4);
        	ms.Flush();
            ms.Close();
            byte[] data = { 0x01, 0x23, 0x45, 0x67, 0x89 };
            var actual = ms.ToArray();
            Assert.AreEqual(data, actual);
        }

        [Test]
        public void Align()
        {
            int[] codes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ms = new MemoryStream();
            var writer = new BitWriter(ms);
            foreach (int code in codes)
            {
            	writer.WriteCode(code, 4);
            	writer.Align();
            }
            ms.Flush();
            ms.Close();
            byte[] data = { 0x00, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90 };
            var actual = ms.ToArray();
            Assert.AreEqual(data, actual);
        }
    }
}
#endif