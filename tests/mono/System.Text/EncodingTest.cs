//
// EncodingTest.cs - Unit Tests for System.Text.Encoding
//
// Author:
//	Gert Driesen (drieseng@users.sourceforge.net)
//
// Copyright (C) 2007 Gert Driesen
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Text;

using NUnit.Framework;

namespace MonoTests.System.Text
{
    [TestFixture]
    public class EncodingTest
    {
#if NOT_PFX
        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsBrowserDisplay()
        {
            MyEncoding enc = new MyEncoding();
            Assert.IsFalse(enc.IsBrowserDisplay);
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsBrowserSave()
        {
            MyEncoding enc = new MyEncoding();
            Assert.IsFalse(enc.IsBrowserSave);
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsMailNewsDisplay()
        {
            MyEncoding enc = new MyEncoding();
            Assert.IsFalse(enc.IsMailNewsDisplay);
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsMailNewsSave()
        {
            MyEncoding enc = new MyEncoding();
            Assert.IsFalse(enc.IsMailNewsSave);
        }
#endif

        [Test]
        public void GetEncoding_CodePage_Default()
        {
            Assert.AreEqual(Encoding.Default, Encoding.GetEncoding(0));
        }

        [Test]
        public void GetEncoding_CodePage_Invalid()
        {
            try
            {
                Encoding.GetEncoding(-1);
                Assert.Fail("#A1");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType(), "#A2");
            }

            try
            {
                Encoding.GetEncoding(65536);
                Assert.Fail("#B1");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType(), "#B2");
            }
        }

        [Test]
        public void GetEncoding_Name_NotSupported()
        {
            try
            {
                Encoding.GetEncoding("doesnotexist");
                Assert.Fail("#1");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(typeof(ArgumentException), ex.GetType(), "#2");
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEncoding_Name_Null()
        {
            Encoding.GetEncoding((string)null);
        }
    }
}
