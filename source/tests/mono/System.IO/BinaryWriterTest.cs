//
// System.IO.StringWriter
//
// Authors: 
//	Ville Palo (vi64pa@kolumbus.fi)
//
// (C) 2003 Ville Palo
//

using NUnit.Framework;
using System.IO;
using System.Text;
using System;

namespace MonoTests.System.IO
{

    [TestFixture]
    public class BinaryWriterTest : Assertion
    {
#if NOT_PFX
        string TempFolder = Path.Combine(Path.GetTempPath(), "MonoTests.System.IO.Tests");
#endif

        [SetUp]
        public void SetUp()
        {
#if NOT_PFX
            if (Directory.Exists(TempFolder))
                Directory.Delete(TempFolder, true);
            Directory.CreateDirectory(TempFolder);
#endif
        }

        [TearDown]
        public void TearDown()
        {
#if NOT_PFX
            if (Directory.Exists(TempFolder))
                Directory.Delete(TempFolder, true);
#endif
        }

        [Test]
        public void Ctor()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            AssertEquals("test#01", true, writer.BaseStream.CanRead);
            AssertEquals("test#02", true, writer.BaseStream.CanSeek);
            AssertEquals("test#03", true, writer.BaseStream.CanWrite);

            writer = new BinaryWriter(stream, Encoding.ASCII);
            AssertEquals("test#04", true, writer.BaseStream.CanRead);
            AssertEquals("test#05", true, writer.BaseStream.CanSeek);
            AssertEquals("test#06", true, writer.BaseStream.CanWrite);

        }

        /// <summary>
        /// Throws an exception if stream is null
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullExceptionStream()
        {
            BinaryWriter reader = new BinaryWriter(null);
        }

        /// <summary>
        /// Throws an exception if encoding is null
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNullExceptionStreamEncoding()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter reader = new BinaryWriter(stream, null);
        }

        /// <summary>
        /// Throws an exception if stream is closed
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CtorExceptionStreamClosed()
        {
            MemoryStream stream = new MemoryStream();
            stream.Close();
            BinaryWriter writer = new BinaryWriter(stream);
        }

#if NOT_PFX
        /// <summary>
        /// Throws an exception if stream does not support writing
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CtorArgumentExceptionStreamCannotWrite()
        {
            string path = TempFolder + "/BinaryWriterTest.1";
            DeleteFile(path);
            FileStream stream = null;
            BinaryWriter reader = null;

            try
            {
                stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                reader = new BinaryWriter(stream);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();
                DeleteFile(path);
            }
        }
#endif

        [Test]
        public void TestEncoding()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write("t*st");

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 4, bytes[0]);
            AssertEquals("test#02", 116, bytes[1]);
            AssertEquals("test#03", 42, bytes[2]);
            AssertEquals("test#04", 115, bytes[3]);
            AssertEquals("test#05", 116, bytes[4]);
            AssertEquals("test#06", 0, bytes[5]);

            stream = new MemoryStream();
            writer = new BinaryWriter(stream, new UnicodeEncoding());
            writer.Write("t*st");

            bytes = stream.GetBuffer();
            AssertEquals("test#07", 8, bytes[0]);
            AssertEquals("test#08", 116, bytes[1]);
            AssertEquals("test#09", 0, bytes[2]);
            AssertEquals("test#10", 42, bytes[3]);
            AssertEquals("test#11", 0, bytes[4]);
            AssertEquals("test#12", 115, bytes[5]);
            AssertEquals("test#13", 0, bytes[6]);
            AssertEquals("test#14", 116, bytes[7]);
            AssertEquals("test#15", 0, bytes[8]);

            stream = new MemoryStream();
            writer = new BinaryWriter(stream, Encoding.UTF7);
            writer.Write("t*st");

            bytes = stream.GetBuffer();
            AssertEquals("test#16", 8, bytes[0]);
            AssertEquals("test#17", 116, bytes[1]);
            AssertEquals("test#18", 43, bytes[2]);
            AssertEquals("test#19", 65, bytes[3]);
            AssertEquals("test#21", 67, bytes[4]);
            AssertEquals("test#22", 111, bytes[5]);
            AssertEquals("test#23", 45, bytes[6]);
            AssertEquals("test#24", 115, bytes[7]);
            AssertEquals("test#25", 116, bytes[8]);
            AssertEquals("test#26", 0, bytes[9]);
            AssertEquals("test#27", 0, bytes[10]);

            stream = new MemoryStream();
            writer = new BinaryWriter(stream, Encoding.ASCII);
            writer.Write("t*st");
            bytes = stream.GetBuffer();
            AssertEquals("test#28", 4, bytes[0]);
            AssertEquals("test#29", 116, bytes[1]);
            AssertEquals("test#30", 42, bytes[2]);
            AssertEquals("test#31", 115, bytes[3]);
            AssertEquals("test#32", 116, bytes[4]);
            AssertEquals("test#33", 0, bytes[5]);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Close1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);
            writer.Close();
            writer.Write("Test");
        }

        [Test]
        public void Close2()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);
            writer.Close();
            writer.Flush();
            stream.Flush();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Close3()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);
            writer.Close();
            writer.Seek(1, SeekOrigin.Begin);
        }

        [Test]
        public void Close4()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);
            writer.Close();
            AssertEquals("test#01", false, writer.BaseStream.CanRead);
            AssertEquals("test#01", false, writer.BaseStream.CanWrite);
            AssertEquals("test#01", false, writer.BaseStream.CanSeek);
        }

        [Test]
        public void Seek()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);

            writer.Write("Test");
            writer.Seek(2, SeekOrigin.Begin);
            writer.Write("-");
            writer.Seek(400, SeekOrigin.Begin);
            writer.Write("-");
            writer.Seek(-394, SeekOrigin.End);
            writer.Write("-");
            writer.Seek(-2, SeekOrigin.Current);
            writer.Write("-");

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 512, bytes.Length);
            AssertEquals("test#02", 4, bytes[0]);
            AssertEquals("test#03", 84, bytes[1]);
            AssertEquals("test#04", 1, bytes[2]);
            AssertEquals("test#05", 45, bytes[3]);
            AssertEquals("test#06", 116, bytes[4]);
            AssertEquals("test#07", 0, bytes[5]);
            AssertEquals("test#08", 0, bytes[6]);
            AssertEquals("test#09", 0, bytes[7]);
            AssertEquals("test#10", 1, bytes[8]);
            AssertEquals("test#11", 45, bytes[9]);
            AssertEquals("test#12", 0, bytes[10]);
            AssertEquals("test#13", 0, bytes[11]);
            AssertEquals("test#14", 0, bytes[12]);
            AssertEquals("test#15", 1, bytes[400]);
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void SeekException()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write("Test");
            writer.Seek(-12, SeekOrigin.Begin);
        }

        [Test]
        public void WriteCharArray()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(new char[] { 'm', 'o', 'n', 'o', ':', ':' });
            writer.Write(new char[] { ':', ':', 'O', 'N', 'O', 'M' });

            byte[] bytes = stream.GetBuffer();

            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 109, bytes[0]);
            AssertEquals("test#03", 111, bytes[1]);
            AssertEquals("test#04", 110, bytes[2]);
            AssertEquals("test#05", 111, bytes[3]);
            AssertEquals("test#06", 58, bytes[4]);
            AssertEquals("test#07", 58, bytes[5]);
            AssertEquals("test#08", 58, bytes[6]);
            AssertEquals("test#09", 58, bytes[7]);
            AssertEquals("test#10", 79, bytes[8]);
            AssertEquals("test#11", 78, bytes[9]);
            AssertEquals("test#12", 79, bytes[10]);
            AssertEquals("test#13", 77, bytes[11]);
            AssertEquals("test#14", 0, bytes[12]);
            AssertEquals("test#15", 0, bytes[13]);
        }

        [Test]
        public void WriteByteArray()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(new byte[] { 1, 2, 3, 4, 5, 6 });
            writer.Write(new byte[] { 6, 5, 4, 3, 2, 1 });

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 1, bytes[0]);
            AssertEquals("test#03", 2, bytes[1]);
            AssertEquals("test#04", 3, bytes[2]);
            AssertEquals("test#05", 4, bytes[3]);
            AssertEquals("test#06", 5, bytes[4]);
            AssertEquals("test#07", 6, bytes[5]);
            AssertEquals("test#08", 6, bytes[6]);
            AssertEquals("test#09", 5, bytes[7]);
            AssertEquals("test#10", 4, bytes[8]);
            AssertEquals("test#11", 3, bytes[9]);
            AssertEquals("test#12", 2, bytes[10]);
            AssertEquals("test#13", 1, bytes[11]);
            AssertEquals("test#14", 0, bytes[12]);
            AssertEquals("test#15", 0, bytes[13]);

        }

        [Test]
        public void WriteInt()
        {
            short s = 64;
            int i = 64646464;
            long l = 9999999999999;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(s);
            byte[] bytes;
            bytes = stream.GetBuffer();
            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 64, bytes[0]);
            AssertEquals("test#03", 0, bytes[1]);

            writer.Write(i);
            bytes = stream.GetBuffer();
            AssertEquals("test#04", 256, bytes.Length);
            AssertEquals("test#05", 64, bytes[0]);
            AssertEquals("test#06", 0, bytes[1]);
            AssertEquals("test#07", 64, bytes[2]);
            AssertEquals("test#08", 109, bytes[3]);
            AssertEquals("test#09", 218, bytes[4]);
            AssertEquals("test#10", 3, bytes[5]);
            AssertEquals("test#11", 0, bytes[6]);

            writer.Write(l);
            bytes = stream.GetBuffer();
            AssertEquals("test#12", 256, bytes.Length);
            AssertEquals("test#13", 255, bytes[6]);
            AssertEquals("test#14", 159, bytes[7]);
            AssertEquals("test#15", 114, bytes[8]);
            AssertEquals("test#16", 78, bytes[9]);
            AssertEquals("test#17", 24, bytes[10]);
            AssertEquals("test#18", 9, bytes[11]);
            AssertEquals("test#19", 0, bytes[12]);
        }

        //NOTE: Decimal is not supported yet
#if NOT_PFX
        [Test]
        public void WriteDecimal()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            decimal d1 = 19932143214312.32M;
            decimal d2 = -8995034512332157M;

            writer.Write(d1);
            writer.Write(d2);
            byte[] bytes = stream.GetBuffer();

            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 192, bytes[0]);
            AssertEquals("test#03", 18, bytes[1]);
            AssertEquals("test#04", 151, bytes[2]);
            AssertEquals("test#05", 95, bytes[3]);
            AssertEquals("test#06", 209, bytes[4]);
            AssertEquals("test#07", 20, bytes[5]);
            AssertEquals("test#08", 7, bytes[6]);
            AssertEquals("test#09", 0, bytes[7]);
            AssertEquals("test#10", 0, bytes[8]);
            AssertEquals("test#11", 0, bytes[9]);
            AssertEquals("test#12", 0, bytes[10]);
            AssertEquals("test#13", 0, bytes[11]);
            AssertEquals("test#14", 0, bytes[12]);
            AssertEquals("test#15", 0, bytes[13]);
            AssertEquals("test#16", 2, bytes[14]);
            AssertEquals("test#17", 0, bytes[15]);
            AssertEquals("test#18", 125, bytes[16]);
            AssertEquals("test#19", 149, bytes[17]);
            AssertEquals("test#20", 217, bytes[18]);
            AssertEquals("test#21", 172, bytes[19]);
            AssertEquals("test#22", 239, bytes[20]);
            AssertEquals("test#23", 244, bytes[21]);
            AssertEquals("test#24", 31, bytes[22]);
            AssertEquals("test#25", 0, bytes[23]);
            AssertEquals("test#26", 0, bytes[24]);
            AssertEquals("test#27", 0, bytes[25]);
            AssertEquals("test#28", 0, bytes[26]);
            AssertEquals("test#29", 0, bytes[27]);
            AssertEquals("test#30", 0, bytes[28]);
            AssertEquals("test#31", 0, bytes[29]);
            AssertEquals("test#32", 0, bytes[30]);
            AssertEquals("test#33", 128, bytes[31]);
            AssertEquals("test#34", 0, bytes[32]);
            AssertEquals("test#35", 0, bytes[33]);
            AssertEquals("test#36", 0, bytes[34]);
            AssertEquals("test#37", 0, bytes[35]);
            AssertEquals("test#38", 0, bytes[36]);
            AssertEquals("test#39", 0, bytes[37]);
            AssertEquals("test#40", 0, bytes[38]);
            AssertEquals("test#41", 0, bytes[39]);
            AssertEquals("test#42", 0, bytes[40]);
            AssertEquals("test#43", 0, bytes[41]);
            AssertEquals("test#44", 0, bytes[42]);
            AssertEquals("test#45", 0, bytes[43]);
            AssertEquals("test#46", 0, bytes[44]);
            AssertEquals("test#47", 0, bytes[45]);
            AssertEquals("test#48", 0, bytes[46]);
        }
#endif

        [Test]
        public void WriteFloat()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            float f1 = 1.543E+10F;
            float f2 = -9.6534E-6f;
            writer.Write(f1);
            writer.Write(f2);

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 199, bytes[0]);
            AssertEquals("test#03", 236, bytes[1]);
            AssertEquals("test#04", 101, bytes[2]);
            AssertEquals("test#05", 80, bytes[3]);
            AssertEquals("test#06", 10, bytes[4]);
            AssertEquals("test#07", 245, bytes[5]);
            AssertEquals("test#08", 33, bytes[6]);
            AssertEquals("test#09", 183, bytes[7]);
            AssertEquals("test#10", 0, bytes[8]);
            AssertEquals("test#11", 0, bytes[9]);
        }

        [Test]
        public void WriteDouble()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            double d1 = 1.543E+100;
            double d2 = -9.6534E-129;
            writer.Write(d1);
            writer.Write(d2);

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 49, bytes[0]);
            AssertEquals("test#03", 69, bytes[1]);
            AssertEquals("test#04", 15, bytes[2]);
            AssertEquals("test#05", 157, bytes[3]);
            AssertEquals("test#06", 211, bytes[4]);
            AssertEquals("test#07", 55, bytes[5]);
            AssertEquals("test#08", 188, bytes[6]);
            AssertEquals("test#09", 84, bytes[7]);
            AssertEquals("test#10", 76, bytes[8]);
            AssertEquals("test#11", 59, bytes[9]);
            AssertEquals("test#12", 59, bytes[10]);
            AssertEquals("test#13", 60, bytes[11]);
            AssertEquals("test#14", 4, bytes[12]);
            AssertEquals("test#15", 196, bytes[13]);
            AssertEquals("test#16", 90, bytes[14]);
            AssertEquals("test#17", 165, bytes[15]);
            AssertEquals("test#18", 0, bytes[16]);
        }

        [Test]
        public void WriteByteAndChar()
        {
            byte b1 = 12;
            byte b2 = 64;
            char c1 = '-';
            char c2 = 'M';
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(b1);
            writer.Write(c1);
            writer.Write(b2);
            writer.Write(c2);

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 12, bytes[0]);
            AssertEquals("test#03", 45, bytes[1]);
            AssertEquals("test#04", 64, bytes[2]);
            AssertEquals("test#05", 77, bytes[3]);
            AssertEquals("test#06", 0, bytes[4]);
        }

        [Test]
        public void WriteString()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            string s1 = "abc";
            string s2 = "DeF\n";
            writer.Write(s1);
            writer.Write(s2);

            byte[] bytes = stream.GetBuffer();
            AssertEquals("test#01", 256, bytes.Length);
            AssertEquals("test#02", 3, bytes[0]);
            AssertEquals("test#03", 97, bytes[1]);
            AssertEquals("test#04", 98, bytes[2]);
            AssertEquals("test#05", 99, bytes[3]);
            AssertEquals("test#06", 4, bytes[4]);
            AssertEquals("test#07", 68, bytes[5]);
            AssertEquals("test#08", 101, bytes[6]);
            AssertEquals("test#09", 70, bytes[7]);
            AssertEquals("test#10", 10, bytes[8]);
            AssertEquals("test#11", 0, bytes[9]);
        }

#if NOT_PFX
        private void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
#endif
    }
}

