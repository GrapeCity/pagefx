using System.IO;
using NUnit.Framework;

namespace DataDynamics.Tests
{
    [TestFixture]
    public class BitReaderTest
    {
        [Test]
        public void ReadBit()
        {
            byte[] data = {0xD3, 0x4A};
            int[] bits = {1, 1, 0, 1,  0, 0, 1, 1,  0, 1, 0, 0, 1, 0, 1, 0};
            var reader = new BitReader(data);
            foreach (int expected in bits)
            	Assert.AreEqual(expected, reader.ReadBit() ? 1 : 0);
        }

        [Test]
        public void ReadCode()
        {
            byte[] data = { 0x01, 0x23, 0x45, 0x67, 0x89 };
            int[] codes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var reader = new BitReader(data);
            foreach (int expected in codes)
            	Assert.AreEqual(expected, reader.ReadCode(4));
        }
    }
}