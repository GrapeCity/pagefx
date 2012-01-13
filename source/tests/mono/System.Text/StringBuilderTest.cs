// -*- Mode: C; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*-
//
// StringBuilderTest.dll - NUnit Test Cases for the System.Text.StringBuilder class
// 
// Author: Marcin Szczepanski (marcins@zipworld.com.au)
//
// NOTES: I've also run all these tests against the MS implementation of 
// System.Text.StringBuilder to confirm that they return the same results
// and they do.
//
// TODO: Add tests for the AppendFormat methods once the AppendFormat methods
// are implemented in the StringBuilder class itself
//
// TODO: Potentially add more variations on Insert / Append tests for all the
// possible types.  I don't really think that's necessary as they all
// pretty much just do .ToString().ToCharArray() and then use the Append / Insert
// CharArray function.  The ToString() bit for each type should be in the unit
// tests for those types, and the unit test for ToCharArray should be in the 
// string test type.  If someone wants to add those tests here for completness 
// (and some double checking) then feel free :)
//

using NUnit.Framework;
using System.Text;
using System;

namespace MonoTests.System.Text
{

    [TestFixture]
    public class StringBuilderTest : Assertion
    {

        private StringBuilder sb;

        public void TestConstructor1()
        {
            // check the parameterless ctor
            sb = new StringBuilder();
            AssertEquals(String.Empty, sb.ToString());
            AssertEquals(0, sb.Length);
            AssertEquals(16, sb.Capacity);
        }

        public void TestConstructor2()
        {
            // check ctor that specifies the capacity
            sb = new StringBuilder(10);
            AssertEquals(String.Empty, sb.ToString());
            AssertEquals(0, sb.Length);
            //NOTE: SB.Capacity is not working in PageFX
#if NOT_PFX
            // check that capacity is not less than default
            AssertEquals(10, sb.Capacity);
#endif

            sb = new StringBuilder(42);
            AssertEquals(String.Empty, sb.ToString());
            AssertEquals(0, sb.Length);
            //NOTE: SB.Capacity is not working in PageFX
#if NOT_PFX
            // check that capacity is set
            AssertEquals(42, sb.Capacity);
#endif
        }

        public void TestConstructor3()
        {
            // check ctor that specifies the capacity & maxCapacity
            sb = new StringBuilder(444, 1234);
            AssertEquals(String.Empty, sb.ToString());
            AssertEquals(0, sb.Length);
            //NOTE: SB.Capacity is not working in PageFX
#if NOT_PFX
            AssertEquals(444, sb.Capacity);
            AssertEquals(1234, sb.MaxCapacity);
#endif
        }

        public void TestConstructor4()
        {
            // check for exception in ctor that specifies the capacity & maxCapacity
            try
            {
                sb = new StringBuilder(9999, 15);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            // if we didn't catch an exception, then we have a problem Houston.
            NUnit.Framework.Assertion.Fail("Capacity exeeds MaxCapacity");
        }

        public void TestConstructor5()
        {
            String someString = null;
            sb = new StringBuilder(someString);
            AssertEquals("Should be empty string", String.Empty, sb.ToString());
        }

        public void TestConstructor6()
        {
            // check for exception in ctor that prevents startIndex less than zero
            try
            {
                String someString = "someString";
                sb = new StringBuilder(someString, -1, 3, 18);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            // if we didn't catch an exception, then we have a problem Houston.
            NUnit.Framework.Assertion.Fail("StartIndex not allowed to be less than zero.");
        }

        public void TestConstructor7()
        {
            // check for exception in ctor that prevents length less than zero
            try
            {
                String someString = "someString";
                sb = new StringBuilder(someString, 2, -222, 18);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            // if we didn't catch an exception, then we have a problem Houston.
            NUnit.Framework.Assertion.Fail("Length not allowed to be less than zero.");
        }

        public void TestConstructor8()
        {
            // check for exception in ctor that ensures substring is contained in given string
            // check that startIndex is not too big
            try
            {
                String someString = "someString";
                sb = new StringBuilder(someString, 10000, 4, 18);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            // if we didn't catch an exception, then we have a problem Houston.
            NUnit.Framework.Assertion.Fail("StartIndex and length must refer to a location within the string.");
        }

        public void TestConstructor9()
        {
            // check for exception in ctor that ensures substring is contained in given string
            // check that length doesn't go beyond end of string
            try
            {
                String someString = "someString";
                sb = new StringBuilder(someString, 4, 4000, 18);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            // if we didn't catch an exception, then we have a problem Houston.
            NUnit.Framework.Assertion.Fail("StartIndex and length must refer to a location within the string.");
        }

        public void TestConstructor10()
        {
            // check that substring is taken properly and made into a StringBuilder
            String someString = "someString";
            sb = new StringBuilder(someString, 4, 6, 18);
            string expected = "String";
            AssertEquals(expected, sb.ToString());
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor11()
        {
            new StringBuilder(-1);
        }

        public void TestAppend()
        {
            StringBuilder sb = new StringBuilder("Foo");
            sb.Append("Two");
            string expected = "FooTwo";
            AssertEquals(expected, sb.ToString());
        }

        public void TestInsert()
        {
            StringBuilder sb = new StringBuilder();

            AssertEquals(String.Empty, sb.ToString());
            /* Test empty StringBuilder conforms to spec */

            sb.Insert(0, "Foo"); /* Test insert at start of empty string */

            AssertEquals("Foo", sb.ToString());

            sb.Insert(1, "!!"); /* Test insert not at start of string */

            AssertEquals("F!!oo", sb.ToString());

            sb.Insert(5, ".."); /* Test insert at end of string */

            AssertEquals("F!!oo..", sb.ToString());

            sb.Insert(0, 1234.ToString()); /* Test insert of a number (at start of string) */

            // FIX: Why does this test fail?
            //AssertEquals( "1234F!!oo..", sb.ToString() );

            sb.Insert(5, 1.5.ToString()); /* Test insert of a decimal (and end of string) */

            // FIX: Why does this test fail?
            //AssertEquals( "1234F1.5!!oo..", sb.ToString() );

#if NOT_PFX
            sb.Insert(4, 'A'); /* Test char insert in middle of string */
#endif

            // FIX: Why does this test fail?
            //AssertEquals( "1234AF1.5!!oo..", sb.ToString() );

        }

        public void TestReplace()
        {
            StringBuilder sb = new StringBuilder("Foobarbaz");

            sb.Replace("bar", "!!!");             /* Test same length replace in middle of string */

            AssertEquals("Foo!!!baz", sb.ToString());

            sb.Replace("Foo", "ABcD");            /* Test longer replace at start of string */

            AssertEquals("ABcD!!!baz", sb.ToString());

            sb.Replace("baz", "00");              /* Test shorter replace at end of string */

            AssertEquals("ABcD!!!00", sb.ToString());

            sb.Replace(sb.ToString(), null);      /* Test string clear as in spec */

            AssertEquals(String.Empty, sb.ToString());

            /*           |         10        20        30
            /*         |0123456789012345678901234567890| */
            sb.Append("abc this is testing abc the abc abc partial replace abc");

            sb.Replace("abc", "!!!", 0, 31); /* Partial replace at start of string */

            AssertEquals("!!! this is testing !!! the !!! abc partial replace abc", sb.ToString());

            sb.Replace("testing", "", 0, 15); /* Test replace across boundary */

            AssertEquals("!!! this is testing !!! the !!! abc partial replace abc", sb.ToString());

            sb.Replace("!!!", ""); /* Test replace with empty string */

            AssertEquals(" this is testing  the  abc partial replace abc", sb.ToString());
        }

        public void TestAppendFormat()
        {
        }

        [Test]
        public void MoreReplace()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("First");
            sb.Append("Second");
            sb.Append("Third");
            sb.Replace("Second", "Gone", 2, sb.Length - 2);
            AssertEquals("#01", "FirstGoneThird", sb.ToString());

            sb.Length = 0;
            sb.Append("This, is, a, list");
            sb.Replace(",", "comma-separated", 11, sb.Length - 11);
            AssertEquals("#02", "This, is, acomma-separated list", sb.ToString());
        }

        [Test]
        public void Insert0()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("testtesttest");
            sb.Insert(0, '^');
            AssertEquals("#01", "^testtesttest", sb.ToString());
        }

        [Test]
        public void AppendToEmpty()
        {
            StringBuilder sb = new StringBuilder();
            char[] ca = new char[] { 'c' };
            sb.Append(ca);
            AssertEquals("#01", "c", sb.ToString());
        }


        [Test]
        public void TestRemove()
        {
            StringBuilder b = new StringBuilder();
            b.Append("Hello, I am a StringBuilder");
            b.Remove(0, 7);  // Should remove "Hello, "
            AssertEquals("#01", "I am a StringBuilder", b.ToString());
        }

        [Test]
        public void Insert1()
        {
            StringBuilder sb = new StringBuilder();
            sb.Insert(0, "aa");
            AssertEquals("#01", "aa", sb.ToString());

            char[] charArr = new char[] { 'b', 'c', 'd' };
            sb.Insert(1, charArr, 1, 1);
            AssertEquals("#02", "aca", sb.ToString());

            sb.Insert(1, null, 0, 0);
            AssertEquals("#03", "aca", sb.ToString());

            try
            {
                sb.Insert(1, null, 1, 1);
                Assertion.Fail("#04: Value must not be null if startIndex and charCount > 0");
            }
            catch (ArgumentNullException) { }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_StartIndexOverflow()
        {
            new StringBuilder("Mono", Int32.MaxValue, 1, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_LengthOverflow()
        {
            new StringBuilder("Mono", 1, Int32.MaxValue, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToString_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.ToString(Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToString_LengthOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.ToString(1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Remove_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Remove(Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Remove_LengthOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Remove(1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReplaceChar_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Replace('o', '0', Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReplaceChar_CountOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Replace('0', '0', 1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReplaceString_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Replace("o", "0", Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReplaceString_CountOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Replace("o", "0", 1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AppendCharArray_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Append(new char[2], Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AppendCharArray_CharCountOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Append(new char[2], 1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AppendString_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Append("!", Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AppendString_CountOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Append("!", 1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertCharArray_StartIndexOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Insert(0, new char[2], Int32.MaxValue, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertCharArray_CharCountOverflow()
        {
            StringBuilder sb = new StringBuilder("Mono");
            sb.Insert(0, new char[2], 1, Int32.MaxValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MaxCapacity_Overflow1()
        {
            StringBuilder sb = new StringBuilder(2, 3);
            sb.Append("Mono");
        }

        [Test]
        public void MaxCapacity_Overflow2()
        {
            StringBuilder sb = new StringBuilder(2, 3);
            try
            {
                sb.Append("Mono");
            }
            catch (ArgumentOutOfRangeException)
            {
            }

#if NOT_PFX
		AssertEquals (2, sb.Capacity);
            AssertEquals(3, sb.MaxCapacity);
#endif
        }

        //NOTE: Test below is version dependent
#if NOT_PFX
	[Test]
#if ONLY_1_1
	[ExpectedException (typeof (ArgumentOutOfRangeException))]
	[Category ("NotWorking")] // Mono follows 2.0 behaviour in this case
#endif
	public void MaxCapacity_Overflow3 ()
	{
		//
		// When the capacity (4) gets doubled, it is greater than the
		// max capacity. This makes sure that before throwing an exception
		// we first attempt to go for a smaller size.
		//
		new StringBuilder(4, 7).Append ("foo").Append ("bar");
		new StringBuilder(4, 6).Append ("foo").Append ("bar");
		// this throws ArgumentOutOfRangeException on MS 1.1 SP1
	}
#endif

        [Test]
        public void CapacityFromString()
        {
            StringBuilder sb = new StringBuilder("hola").Append("lala");
            AssertEquals("#01", "holalala", sb.ToString());
        }

        [Test]
        public void ReplaceWithLargerString()
        {
            StringBuilder sb = new StringBuilder("ABCDE");
            AssertEquals("#1", "ABCDE", sb.ToString());
            sb.Replace("ABC", "abcaa", 0, 3);
            AssertEquals("#2", "abcaaDE", sb.ToString());
        }

        [Test]
        public void SetLength()
        {
            StringBuilder sb = new StringBuilder("Text");
            AssertEquals("#1", 4, sb.Length);
            AssertEquals("#2", "Text", sb.ToString());
            sb.Length = 8;
            AssertEquals("#3", 8, sb.Length);
            AssertEquals("#4", "Text\0\0\0\0", sb.ToString());
        }
    }
}
