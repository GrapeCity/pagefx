// CaseInsensitiveComparerTest

using System;
using System.Collections;
using System.Globalization;
using System.IO;
//using System.Runtime.Serialization.Formatters;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Threading;

using NUnit.Framework;

namespace MonoTests.System.Collections
{
    [TestFixture]
    public class CaseInsensitiveComparerTest
    {
        [Test]
        public void TestDefaultInstance()
        {
            // Make sure the instance returned by Default
            // is really a CaseInsensitiveComparer.
            Assert.IsNotNull((CaseInsensitiveComparer.Default as CaseInsensitiveComparer));
        }

        [Test]
        public void TestCompare()
        {
            CaseInsensitiveComparer cic = new CaseInsensitiveComparer();

            Assert.AreEqual(0, cic.Compare("WILD WEST", "Wild West"), "#1");
            Assert.AreEqual(0, cic.Compare("WILD WEST", "wild west"), "#2");
            Assert.AreEqual(1, cic.Compare("Zeus", "Mars"), "#3");
            Assert.AreEqual(-1, cic.Compare("Earth", "Venus"), "#4");
        }

        //NOTE: Unable to change culture using Thread
#if NOT_PFX
        [Test]
        public void TestCompare_Culture()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

                // the default ctor is initialized using Thread.CurrentCulture
                CaseInsensitiveComparer cic = new CaseInsensitiveComparer();
                Assert.AreEqual(-1, cic.Compare("I", "i"), "#A1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#A2");

                // changing the current culture does not affect an already
                // initialized CaseInsensitiveComparer
                CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
                Assert.AreEqual(-1, cic.Compare("I", "i"), "#B1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#B2");

                // but it does affect new instances
                cic = new CaseInsensitiveComparer();
                Assert.AreEqual(0, cic.Compare("I", "i"), "#C1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#C2");

                // if the culture is explicitly set, then the thread culture is
                // ignored
                cic = new CaseInsensitiveComparer(new CultureInfo("tr-TR"));
                Assert.AreEqual(-1, cic.Compare("I", "i"), "#D1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#D2");
            }
            finally
            {
                // restore original culture
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Test]
        [Category("NotWorking")] // bug #80076
        public void Default()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

                // CaseInsensitiveComparer.Default is initialized using 
                // Thread.CurrentCulture
                CaseInsensitiveComparer cic = CaseInsensitiveComparer.Default;
                Assert.AreEqual(-1, cic.Compare("I", "i"), "#A1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#A2");

                // changing the current culture does not affect an already
                // initialized CaseInsensitiveComparer
                CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
                Assert.AreEqual(-1, cic.Compare("I", "i"), "#B1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#B2");

                // but it does affect new instances
                cic = CaseInsensitiveComparer.Default;
                Assert.AreEqual(0, cic.Compare("I", "i"), "#C1");
                Assert.AreEqual(0, cic.Compare("A", "a"), "#C2");
            }
            finally
            {
                // restore original culture
                CultureInfo.CurrentCulture = originalCulture;
            }
        }
#endif

        [Test]
        public void TestIntsNEq()
        {
            int a = 1;
            int b = 2;
            Assert.AreEqual(Comparer.Default.Compare(a, b), CaseInsensitiveComparer.Default.Compare(a, b));
        }

        [Test]
        public void TestIntsEq()
        {
            int a = 1;
            int b = 1;
            Assert.AreEqual(Comparer.Default.Compare(a, b), CaseInsensitiveComparer.Default.Compare(a, b));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CtorNull()
        {
            new CaseInsensitiveComparer(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestObject()
        {
            object a = new object();
            object b = new object();
            CaseInsensitiveComparer.Default.Compare(a, b);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDiffArgs()
        {
            int a = 5;
            string b = "hola";
            CaseInsensitiveComparer.Default.Compare(a, b);
        }

        [Test]
        public void TestNull1()
        {
            string a = null;
            string b = "5";
            Assert.AreEqual(Comparer.Default.Compare(a, b), CaseInsensitiveComparer.Default.Compare(a, b));
        }

        [Test]
        public void TestNull2()
        {
            string a = null;
            string b = null;
            Assert.AreEqual(Comparer.Default.Compare(a, b), CaseInsensitiveComparer.Default.Compare(a, b));
        }

        [Test]
        public void TestStringsCaps()
        {
            string a = "AA";
            string b = "aa";
            Assert.AreEqual(0, CaseInsensitiveComparer.Default.Compare(a, b));
        }

        //NOTE: Serialization is not supported
#if NOT_PFX
        // CompareInfo does not have value for win32LCID field, and as a result
        // we have no perfect match
        [Test]
        [Category("NotWorking")]
        public void Serialize_Culture()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");

                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, new CaseInsensitiveComparer(new CultureInfo("tr-TR")));

                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);

#if NET_2_0
                Assert.AreEqual(_serializedCultureV20, buffer);
#else
				Assert.AreEqual (_serializedCultureV11, buffer);
#endif
            }
            finally
            {
                // restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Test]
#if NET_2_0
        [Category("NotWorking")] // bug #80082
#else
		[Category ("NotWorking")] // bug #80076
#endif
        public void Deserialize_Culture()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");

                MemoryStream ms = new MemoryStream();
                ms.Write(_serializedCultureV11, 0, _serializedCultureV11.Length);
                ms.Position = 0;

                BinaryFormatter bf = new BinaryFormatter();
                CaseInsensitiveComparer cic = (CaseInsensitiveComparer)
                    bf.Deserialize(ms);
                Assert.IsNotNull(cic, "#1");

#if NET_2_0
                ms = new MemoryStream();
                ms.Write(_serializedCultureV20, 0, _serializedCultureV20.Length);
                ms.Position = 0;

                bf = new BinaryFormatter();
                cic = (CaseInsensitiveComparer)bf.Deserialize(ms);
                Assert.IsNotNull(cic, "#2");
#endif
            }
            finally
            {
                // restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        // CompareInfo does not have value for win32LCID field, and as a result
        // we have no perfect match
        [Test]
        [Category("NotWorking")]
        public void Serialize_Default()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");

                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, CaseInsensitiveComparer.Default);

                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);

#if NET_2_0
                Assert.AreEqual(_serializedDefaultV20, buffer);
#else
				Assert.AreEqual (_serializedDefaultV11, buffer);
#endif
            }
            finally
            {
                // restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Test]
#if NET_2_0
        [Category("NotWorking")] // bug #80082
#else
		[Category ("NotWorking")] // bug #80076
#endif
        public void Deserialize_Default()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");

                MemoryStream ms = new MemoryStream();
                ms.Write(_serializedDefaultV11, 0, _serializedDefaultV11.Length);
                ms.Position = 0;

                BinaryFormatter bf = new BinaryFormatter();
                CaseInsensitiveComparer cic = (CaseInsensitiveComparer)
                    bf.Deserialize(ms);
                Assert.IsNotNull(cic, "#1");

#if NET_2_0
                ms = new MemoryStream();
                ms.Write(_serializedDefaultV20, 0, _serializedDefaultV20.Length);
                ms.Position = 0;

                bf = new BinaryFormatter();
                cic = (CaseInsensitiveComparer)bf.Deserialize(ms);
                Assert.IsNotNull(cic, "#2");
#endif
            }
            finally
            {
                // restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        // CompareInfo does not have value for win32LCID field, and as a result
        // we have no perfect match
        [Test]
        [Category("NotWorking")]
        public void Serialize_DefaultInvariant()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");

                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, CaseInsensitiveComparer.DefaultInvariant);

                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);

#if NET_2_0
                Assert.AreEqual(_serializedDefaultInvariantV20, buffer);
#else
				Assert.AreEqual (_serializedDefaultInvariantV11, buffer);
#endif
            }
            finally
            {
                // restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Test]
#if NET_2_0
        [Category("NotWorking")] // bug #80082
#else
		[Category ("NotWorking")] // bug #80076
#endif
        public void Deserialize_DefaultInvariant()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");

                MemoryStream ms = new MemoryStream();
                ms.Write(_serializedDefaultInvariantV11, 0, _serializedDefaultInvariantV11.Length);
                ms.Position = 0;

                BinaryFormatter bf = new BinaryFormatter();
                CaseInsensitiveComparer cic = (CaseInsensitiveComparer)
                    bf.Deserialize(ms);
                Assert.IsNotNull(cic, "#1");

#if NET_2_0
                ms = new MemoryStream();
                ms.Write(_serializedDefaultInvariantV20, 0, _serializedDefaultInvariantV20.Length);
                ms.Position = 0;

                bf = new BinaryFormatter();
                cic = (CaseInsensitiveComparer)bf.Deserialize(ms);
                Assert.IsNotNull(cic, "#2");
#endif
            }
            finally
            {
                // restore original culture
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        private static byte[] _serializedDefaultV11 = new byte[] {
			0x00, 0x01, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x01, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00,
			0x2a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x43, 0x6f, 0x6c,
			0x6c, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e, 0x43, 0x61,
			0x73, 0x65, 0x49, 0x6e, 0x73, 0x65, 0x6e, 0x73, 0x69, 0x74, 0x69,
			0x76, 0x65, 0x43, 0x6f, 0x6d, 0x70, 0x61, 0x72, 0x65, 0x72, 0x01,
			0x00, 0x00, 0x00, 0x0d, 0x6d, 0x5f, 0x63, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x20, 0x53, 0x79, 0x73,
			0x74, 0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69,
			0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70,
			0x61, 0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x09, 0x02, 0x00, 0x00,
			0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x20, 0x53, 0x79, 0x73, 0x74,
			0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69, 0x7a,
			0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x02, 0x00, 0x00, 0x00, 0x09,
			0x77, 0x69, 0x6e, 0x33, 0x32, 0x4c, 0x43, 0x49, 0x44, 0x07, 0x63,
			0x75, 0x6c, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x08, 0x08, 0x13,
			0x08, 0x00, 0x00, 0x13, 0x08, 0x00, 0x00, 0x0b };

        private static byte[] _serializedCultureV11 = new byte[] {
			0x00, 0x01, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x01, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00,
			0x2a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x43, 0x6f, 0x6c,
			0x6c, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e, 0x43, 0x61,
			0x73, 0x65, 0x49, 0x6e, 0x73, 0x65, 0x6e, 0x73, 0x69, 0x74, 0x69,
			0x76, 0x65, 0x43, 0x6f, 0x6d, 0x70, 0x61, 0x72, 0x65, 0x72, 0x01,
			0x00, 0x00, 0x00, 0x0d, 0x6d, 0x5f, 0x63, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x20, 0x53, 0x79, 0x73,
			0x74, 0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69,
			0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70,
			0x61, 0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x09, 0x02, 0x00, 0x00,
			0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x20, 0x53, 0x79, 0x73, 0x74,
			0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69, 0x7a,
			0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x02, 0x00, 0x00, 0x00, 0x09,
			0x77, 0x69, 0x6e, 0x33, 0x32, 0x4c, 0x43, 0x49, 0x44, 0x07, 0x63,
			0x75, 0x6c, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x08, 0x08, 0x1f,
			0x04, 0x00, 0x00, 0x1f, 0x04, 0x00, 0x00, 0x0b };

        private static byte[] _serializedDefaultInvariantV11 = new byte[] {
			0x00, 0x01, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x01, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00,
			0x2a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x43, 0x6f, 0x6c,
			0x6c, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e, 0x43, 0x61,
			0x73, 0x65, 0x49, 0x6e, 0x73, 0x65, 0x6e, 0x73, 0x69, 0x74, 0x69,
			0x76, 0x65, 0x43, 0x6f, 0x6d, 0x70, 0x61, 0x72, 0x65, 0x72, 0x01,
			0x00, 0x00, 0x00, 0x0d, 0x6d, 0x5f, 0x63, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x20, 0x53, 0x79, 0x73,
			0x74, 0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69,
			0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70,
			0x61, 0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x09, 0x02, 0x00, 0x00,
			0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x20, 0x53, 0x79, 0x73, 0x74,
			0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69, 0x7a,
			0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x02, 0x00, 0x00, 0x00, 0x09,
			0x77, 0x69, 0x6e, 0x33, 0x32, 0x4c, 0x43, 0x49, 0x44, 0x07, 0x63,
			0x75, 0x6c, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x08, 0x08, 0x7f,
			0x00, 0x00, 0x00, 0x7f, 0x00, 0x00, 0x00, 0x0b };

#if NET_2_0
        private static byte[] _serializedDefaultV20 = new byte[] {
			0x00, 0x01, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x01, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00,
			0x2a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x43, 0x6f, 0x6c,
			0x6c, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e, 0x43, 0x61,
			0x73, 0x65, 0x49, 0x6e, 0x73, 0x65, 0x6e, 0x73, 0x69, 0x74, 0x69,
			0x76, 0x65, 0x43, 0x6f, 0x6d, 0x70, 0x61, 0x72, 0x65, 0x72, 0x01,
			0x00, 0x00, 0x00, 0x0d, 0x6d, 0x5f, 0x63, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x20, 0x53, 0x79, 0x73,
			0x74, 0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69,
			0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70,
			0x61, 0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x09, 0x02, 0x00, 0x00,
			0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x20, 0x53, 0x79, 0x73, 0x74,
			0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69, 0x7a,
			0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x00, 0x00, 0x00, 0x09,
			0x77, 0x69, 0x6e, 0x33, 0x32, 0x4c, 0x43, 0x49, 0x44, 0x07, 0x63,
			0x75, 0x6c, 0x74, 0x75, 0x72, 0x65, 0x06, 0x6d, 0x5f, 0x6e, 0x61,
			0x6d, 0x65, 0x00, 0x00, 0x01, 0x08, 0x08, 0x13, 0x08, 0x00, 0x00,
			0x13, 0x08, 0x00, 0x00, 0x06, 0x03, 0x00, 0x00, 0x00, 0x05, 0x6e,
			0x6c, 0x2d, 0x42, 0x45, 0x0b };

        private static byte[] _serializedDefaultInvariantV20 = new byte[] {
			0x00, 0x01, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x01, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00,
			0x2a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x43, 0x6f, 0x6c,
			0x6c, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e, 0x43, 0x61,
			0x73, 0x65, 0x49, 0x6e, 0x73, 0x65, 0x6e, 0x73, 0x69, 0x74, 0x69,
			0x76, 0x65, 0x43, 0x6f, 0x6d, 0x70, 0x61, 0x72, 0x65, 0x72, 0x01,
			0x00, 0x00, 0x00, 0x0d, 0x6d, 0x5f, 0x63, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x20, 0x53, 0x79, 0x73,
			0x74, 0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69,
			0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70,
			0x61, 0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x09, 0x02, 0x00, 0x00,
			0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x20, 0x53, 0x79, 0x73, 0x74,
			0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69, 0x7a,
			0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x00, 0x00, 0x00, 0x09,
			0x77, 0x69, 0x6e, 0x33, 0x32, 0x4c, 0x43, 0x49, 0x44, 0x07, 0x63,
			0x75, 0x6c, 0x74, 0x75, 0x72, 0x65, 0x06, 0x6d, 0x5f, 0x6e, 0x61,
			0x6d, 0x65, 0x00, 0x00, 0x01, 0x08, 0x08, 0x7f, 0x00, 0x00, 0x00,
			0x7f, 0x00, 0x00, 0x00, 0x06, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0b };

        private static byte[] _serializedCultureV20 = new byte[] {
			0x00, 0x01, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x01, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00,
			0x2a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x43, 0x6f, 0x6c,
			0x6c, 0x65, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x2e, 0x43, 0x61,
			0x73, 0x65, 0x49, 0x6e, 0x73, 0x65, 0x6e, 0x73, 0x69, 0x74, 0x69,
			0x76, 0x65, 0x43, 0x6f, 0x6d, 0x70, 0x61, 0x72, 0x65, 0x72, 0x01,
			0x00, 0x00, 0x00, 0x0d, 0x6d, 0x5f, 0x63, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x20, 0x53, 0x79, 0x73,
			0x74, 0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69,
			0x7a, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70,
			0x61, 0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x09, 0x02, 0x00, 0x00,
			0x00, 0x04, 0x02, 0x00, 0x00, 0x00, 0x20, 0x53, 0x79, 0x73, 0x74,
			0x65, 0x6d, 0x2e, 0x47, 0x6c, 0x6f, 0x62, 0x61, 0x6c, 0x69, 0x7a,
			0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2e, 0x43, 0x6f, 0x6d, 0x70, 0x61,
			0x72, 0x65, 0x49, 0x6e, 0x66, 0x6f, 0x03, 0x00, 0x00, 0x00, 0x09,
			0x77, 0x69, 0x6e, 0x33, 0x32, 0x4c, 0x43, 0x49, 0x44, 0x07, 0x63,
			0x75, 0x6c, 0x74, 0x75, 0x72, 0x65, 0x06, 0x6d, 0x5f, 0x6e, 0x61,
			0x6d, 0x65, 0x00, 0x00, 0x01, 0x08, 0x08, 0x1f, 0x04, 0x00, 0x00,
			0x1f, 0x04, 0x00, 0x00, 0x06, 0x03, 0x00, 0x00, 0x00, 0x05, 0x74,
			0x72, 0x2d, 0x54, 0x52, 0x0b };
#endif
#endif
    }
}
