#if NUNIT
using System;
using System.IO;
using System.Linq;
using Ionic.Zip;
using NUnit.Framework;

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
	        var zip = new ZipFile();
	        zip.AddEntry("A", "", dataA);
	        zip.AddEntry("B", "", dataB);
			zip.Save(ms);
            ms.Close();

            byte[] zipBytes = ms.ToArray();
            zip = ZipFile.Read(zipBytes);
            Assert.AreEqual(2, zip.Count);

            var A = zip.FirstOrDefault(x => x.FileName.Equals("A", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(A, "A != null");
            AssertData(A, dataA);

            var B = zip.FirstOrDefault(x => x.FileName.Equals("B", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(B, "B != null");
            AssertData(B, dataB);
        }

        static void AssertData(ZipEntry e, byte[] data)
        {
            var ms = e.OpenReader().ToMemoryStream();
            ms.Close();
            byte[] d = ms.ToArray();
            Assert.AreEqual(data, d);
        }
    }
}
#endif