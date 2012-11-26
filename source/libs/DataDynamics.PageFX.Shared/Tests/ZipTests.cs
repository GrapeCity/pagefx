#if NUNIT
using System.IO;
using NUnit.Framework;
using DataDynamics.Compression.Zip;

namespace DataDynamics.PageFX.Tests
{
    [TestFixture]
    public class ZipTests
    {
        [Test]
        public void CreateAndRead()
        {
            var dataA = new byte[] { 10, 20, 30, 40 };
            var dataB = new byte[] { 50, 60, 70, 80 };
            var ms = new MemoryStream();
            using (var stream = new ZipOutputStream(ms))
            {
                WriteZipEntry(stream, "A", dataA);
                WriteZipEntry(stream, "B", dataB);
            }
            ms.Close();
            byte[] zipBytes = ms.ToArray();
            var zip = new ZipFile(new MemoryStream(zipBytes));
            Assert.AreEqual(2, zip.Size);

            var A = zip.GetEntry("A");
            Assert.IsNotNull(A, "A != null");
            AssertData(A, dataA);

            var B = zip.GetEntry("B");
            Assert.IsNotNull(B, "B != null");
            AssertData(B, dataB);
        }

        static void AssertData(ZipEntry e, byte[] data)
        {
            var ms = e.Data.ToMemoryStream();
            ms.Close();
            byte[] d = ms.ToArray();
            Assert.AreEqual(data, d);
        }

        static void WriteZipEntry(ZipOutputStream stream, string name, byte[] data)
        {
            stream.PutNextEntry(new ZipEntry(name));
            stream.Write(data, 0, data.Length);
        }
    }
}
#endif