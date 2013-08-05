#if NUNIT
using System.Drawing;
using System.Drawing.Drawing2D;
using DataDynamics.PageFX.Flash.Swf;
using NUnit.Framework;

namespace DataDynamics.PageFX.Flash.Tests
{
    [TestFixture]
    public class SwfIOTest
    {
        [Test]
        public void TestPrimitives()
        {
            var writer = new SwfWriter();
            writer.WriteUIntEncoded(100);
            writer.WriteUIntEncoded(500);
            writer.WriteUIntEncoded(528);
            writer.WriteUIntEncoded(1000);
            writer.WriteUIntEncoded(10000);
            writer.WriteUIntEncoded(uint.MaxValue);
            writer.WriteIntEncoded(-100);
            writer.WriteIntEncoded(-500);
            writer.WriteIntEncoded(-1000);
            writer.WriteIntEncoded(-10000);
            writer.WriteIntEncoded(int.MinValue);
            writer.WriteIntEncoded(int.MaxValue);
            writer.WriteInt24(-100);
            writer.WriteInt24(-500);

            var data = writer.ToByteArray();

            var reader = new SwfReader(data);
            Assert.AreEqual(100, reader.ReadUIntEncoded());
            Assert.AreEqual(500, reader.ReadUIntEncoded());
            Assert.AreEqual(528, reader.ReadUIntEncoded());
            Assert.AreEqual(1000, reader.ReadUIntEncoded());
            Assert.AreEqual(10000, reader.ReadUIntEncoded());
            Assert.AreEqual(uint.MaxValue, reader.ReadUIntEncoded());
            Assert.AreEqual(-100, reader.ReadIntEncoded());
            Assert.AreEqual(-500, reader.ReadIntEncoded());
            Assert.AreEqual(-1000, reader.ReadIntEncoded());
            Assert.AreEqual(-10000, reader.ReadIntEncoded());
            Assert.AreEqual(int.MinValue, reader.ReadIntEncoded());
            Assert.AreEqual(int.MaxValue, reader.ReadIntEncoded());
            Assert.AreEqual(-100, reader.ReadInt24());
            Assert.AreEqual(-500, reader.ReadInt24());
        }

        [Test]
        public void TestTwipConvertions()
        {
            Assert.AreEqual(99, 4.95f.ToTwips());
        }

        private static void TestTwip(float v, int bits)
        {
            var writer = new SwfWriter();
            writer.WriteUB((uint)bits, 5);
            writer.WriteTwip(v, bits);
            writer.Align();

            var data = writer.ToByteArray();

            var reader = new SwfReader(data);
            int bits2 = (int)reader.ReadUB(5);

            Assert.AreEqual(bits, bits2);

            float v2 = reader.ReadTwip(bits2);

            Assert.AreEqual(v, v2);
        }

        [Test]
        public void TestTwip()
        {
            TestTwip(4.95f, 8);
        }

        private static void TestMatrix(Matrix m)
        {
            var writer = new SwfWriter();
            writer.WriteMatrix(m);
            
            var data = writer.ToByteArray();

            var reader = new SwfReader(data);

            var m2 = reader.ReadMatrix();
            var a = m.Elements;
            var b = m2.Elements;
            for (int i = 0; i < 6; ++i)
                Assert.AreEqual(a[i], b[i], 0.1f, "e#" + i);            
        }

        [Test]
        public void TestMatrix()
        {
            TestMatrix(new Matrix());
            TestMatrix(new Matrix(1, 0.5f, 0.5f, 0.7f, 100, 100));
            TestMatrix(new Matrix(0, 0.5f, 0.5f, 0, 100, 100));
            TestMatrix(new Matrix(1, 0.5f, 0.5f, 1, 0, 0));
            TestMatrix(new Matrix(0.5f, 0, 0, 0.5f, 0, 0));
            TestMatrix(new Matrix(1, 0, 0, 1, 100, 100));
            TestMatrix(new Matrix(1, 0, 0, 1, -100, -100));
        }

        private static void TestMatrix2(byte[] data)
        {
            var reader = new SwfReader(data);
            var mat = reader.ReadMatrix();

            var writer = new SwfWriter();
            writer.WriteMatrix(mat);

            var data2 = writer.ToByteArray();

            Assert.AreEqual(data, data2);
        }

        [Test]
        public void TestMatrix2()
        {
            TestMatrix2(new byte[] { 132, 176, 100, 44, 217, 12, 108, 128 });
        }

        private static void TestRect(RectangleF r)
        {
            var writer = new SwfWriter();
            writer.WriteRectTwip(r);

            var data = writer.ToByteArray();

            var reader = new SwfReader(data);

            var r2 = reader.ReadRectF();
            Assert.AreEqual(r.X, r2.X, 0.1f, "x");
            Assert.AreEqual(r.Y, r2.Y, 0.1f, "y");
            Assert.AreEqual(r.Width, r2.Width, 0.1f, "width");
            Assert.AreEqual(r.Height, r2.Height, 0.1f, "height");
        }

        [Test]
        public void TestRect()
        {
            TestRect(RectangleF.Empty);
            TestRect(new RectangleF(0, 0, 10, 10));
            TestRect(new RectangleF(0, 0, 100, 100));
            TestRect(new RectangleF(-10, -10, 100, 100));
        }

        [Test]
        public void TestSB()
        {
            var writer = new SwfWriter();
            writer.WriteSB(-2, 4);
            writer.WriteSB(-2, 2);
            writer.WriteSB(-1, 1);

            var data = writer.ToByteArray();

            var reader = new SwfReader(data);
            Assert.AreEqual(-2, reader.ReadSB(4));
            Assert.AreEqual(-2, reader.ReadSB(2));
            Assert.AreEqual(-1, reader.ReadSB(1));
        }
    }
}
#endif