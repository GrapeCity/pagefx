// Int64Test.cs - NUnit Test Cases for the System.Int64 struct
//
// Author: Martin Weindel (martin.weindel@t-online.de)
//
// (C) Martin Weindel, 2001
// 
// tests ToString and Parse function with the culture independent 
// NumberFormatInfo.InvariantInfo

using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading;

using AssertType = NUnit.Framework.Assert;

    /// <summary>
    /// Tests for System.Int64
    /// </summary>
namespace MonoTests.System
{

    [TestFixture]
    public class Int64Test : Assertion
    {
        private const Int64 MyInt64_1 = -42;
        private const Int64 MyInt64_2 = -9223372036854775808;
        private const Int64 MyInt64_3 = 9223372036854775807;
        private const string MyString1 = "-42";
        private const string MyString2 = "-9223372036854775808";
        private const string MyString3 = "9223372036854775807";
        private string[] Formats1 = { "c", "d", "e", "f", "g", "n", "p", "x" };
        private string[] Formats2 = { "c5", "d5", "e5", "f5", "g5", "n5", "p5", "x5" };

        private string[] Results1 = {"", "-9223372036854775808", "-9.223372e+018", "-9223372036854775808.00",
                                      "-9223372036854775808", "-9,223,372,036,854,775,808.00", "-922,337,203,685,477,580,800.00 %", "8000000000000000"};
        private string[] Results2 = {"", "9223372036854775807", "9.22337e+018", "9223372036854775807.00000",
                                      "9.2234e+18", "9,223,372,036,854,775,807.00000", "922,337,203,685,477,580,700.00000 %", "7fffffffffffffff"};

        private string[] ResultsNfi1 = {"("+NumberFormatInfo.InvariantInfo.CurrencySymbol+"9,223,372,036,854,775,808.00)", "-9223372036854775808", "-9.223372e+018", "-9223372036854775808.00",
	                                  "-9223372036854775808", "-9,223,372,036,854,775,808.00", "-922,337,203,685,477,580,800.00 %", "8000000000000000"};
        private string[] ResultsNfi2 = {""+NumberFormatInfo.InvariantInfo.CurrencySymbol+"9,223,372,036,854,775,807.00000", "9223372036854775807", "9.22337e+018", "9223372036854775807.00000",
	                                  "9.2234e+18", "9,223,372,036,854,775,807.00000", "922,337,203,685,477,580,700.00000 %", "7fffffffffffffff"};

        private static readonly long[] vals = { 0, Int64.MaxValue, Int64.MinValue,
            1L, 12L, 123L, 1234L, -123L, 1234567890123456L, 6543210987654321L };

        private const long val1 = -1234567L;
        private const long val2 = 1234567L;
        private const string sval1Test1 = "  -1,234,567   ";
        private const string sval1Test2 = "  -1234567   ";
        //private const string sval1Test3 = "  -12345,,,,67   "; // interesting: this case works on SDK Beta2, but the specification says nothing about this case
        private const string sval1Test4 = "  -12345 67   ";
        private string sval1Test5 = "  -" + NumberFormatInfo.InvariantInfo.CurrencySymbol + "1,234,567.00 ";
        private string sval1Test6 = "(" + NumberFormatInfo.InvariantInfo.CurrencySymbol + "1,234,567.00)";
        private const string sval1Test7 = "-1,234,567.00";
        private const string sval1UserCur1 = "1234/5/67:000 XYZ-";
        private const string sval2UserCur1 = "1234/5/67:000 XYZ";
        private const string sval1UserPercent1 = "-%%%1~2~3~4~5~6~7~0~0;0";
        private const string sval2UserPercent1 = "%%%1~2~3~4~5~6~7~0~0;0";
        private const NumberStyles style1 = NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign
                        | NumberStyles.AllowTrailingWhite | NumberStyles.AllowThousands;

        private NumberFormatInfo Nfi = NumberFormatInfo.InvariantInfo;

        private NumberFormatInfo NfiUser;

        private CultureInfo old_culture;

        [SetUp]
        public void SetUp()
        {
#if NOT_PFX
old_culture = Thread.CurrentThread.CurrentCulture;

            // Set culture to en-US and don't let the user override.
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
#endif

            int cdd = NumberFormatInfo.CurrentInfo.CurrencyDecimalDigits;
            string csym = NumberFormatInfo.CurrentInfo.CurrencySymbol;
            string csuffix = (cdd > 0 ? "." : "").PadRight(cdd + (cdd > 0 ? 1 : 0), '0');

            string decimals = new String('0', NumberFormatInfo.CurrentInfo.NumberDecimalDigits);
            string perPattern = new string[] { "n %", "n%", "%n" }[NumberFormatInfo.CurrentInfo.PercentPositivePattern];

            Results1[0] = "(" + csym + "9,223,372,036,854,775,808" + csuffix + ")";
            Results1[3] = "-9223372036854775808." + decimals;
            Results1[5] = "-9,223,372,036,854,775,808." + decimals;
            Results1[6] = perPattern.Replace("n", "-922,337,203,685,477,580,800.00");

            Results2[0] = csym + "9,223,372,036,854,775,807.00000";
            Results2[6] = perPattern.Replace("n", "922,337,203,685,477,580,700.00000");

            NfiUser = new NumberFormatInfo();
            NfiUser.CurrencyDecimalDigits = 3;
            NfiUser.CurrencyDecimalSeparator = ":";
            NfiUser.CurrencyGroupSeparator = "/";
            NfiUser.CurrencyGroupSizes = new int[] { 2, 1, 0 };
            NfiUser.CurrencyNegativePattern = 10;  // n $-
            NfiUser.CurrencyPositivePattern = 3;  // n $
            NfiUser.CurrencySymbol = "XYZ";
            NfiUser.PercentDecimalDigits = 1;
            NfiUser.PercentDecimalSeparator = ";";
            NfiUser.PercentGroupSeparator = "~";
            NfiUser.PercentGroupSizes = new int[] { 1 };
            NfiUser.PercentNegativePattern = 2;
            NfiUser.PercentPositivePattern = 2;
            NfiUser.PercentSymbol = "%%%";
        }

#if NOT_PFX
[TearDown]
        public void TearDown()
        {
            Thread.CurrentThread.CurrentCulture = old_culture;
        }
#endif

        public void TestMinMax()
        {

            AssertEquals(Int64.MinValue, MyInt64_2);
            AssertEquals(Int64.MaxValue, MyInt64_3);
        }

        public void TestCompareTo()
        {
            int t = 1;
            try
            {
                Assert(MyInt64_3.CompareTo(MyInt64_2) > 0);
                ++t;
                Assert(MyInt64_2.CompareTo(MyInt64_2) == 0);
                ++t;
                Assert(MyInt64_1.CompareTo((object)(Int64)(-42)) == 0);
                ++t;
                Assert(MyInt64_2.CompareTo(MyInt64_3) < 0);
                ++t;
            }
            catch (Exception e)
            {
                Fail(string.Format("Unexpected exception in assert #{0}: {1}", t, e));
            }
            try
            {
                MyInt64_2.CompareTo((object)(Int16)100);
                Fail("Should raise a System.ArgumentException");
            }
            catch (Exception e)
            {
                Assert(typeof(ArgumentException) == e.GetType());
            }
        }

        public void TestEquals()
        {
            Assert(MyInt64_1.Equals(MyInt64_1));
            Assert(MyInt64_1.Equals((object)(Int64)(-42)));
            Assert(MyInt64_1.Equals((object)(SByte)(-42)) == false);
            Assert(MyInt64_1.Equals(MyInt64_2) == false);
        }

        public void TestGetHashCode()
        {
            try
            {
                MyInt64_1.GetHashCode();
                MyInt64_2.GetHashCode();
                MyInt64_3.GetHashCode();
            }
            catch
            {
                Fail("GetHashCode should not raise an exception here");
            }
        }

        public void TestRoundTripGeneral()
        {
            foreach (long lv in vals)
            {
                string s = lv.ToString(Nfi);
                long lv2 = Int64.Parse(s);
                Assert(lv == lv2);
                long lv3 = Int64.Parse(s, NumberStyles.Integer, Nfi);
                Assert(lv == lv3);
            }
        }

        public void TestRoundTripHex()
        {
            foreach (long lv in vals)
            {
                string s = lv.ToString("x", Nfi);
                long lv2 = Int64.Parse(s, NumberStyles.HexNumber, Nfi);
                Assert(lv == lv2);
            }
        }

        public void TestParseNull()
        {
            try
            {
                Int64.Parse(null);
                Fail("Should raise System.ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // ok
            }
        }

        public void TestParse()
        {
            long lv;

            lv = Int64.Parse(sval1Test1, style1, Nfi);
            AssertEquals("Long value should be equal for Test1", val1, lv);

            try
            {
                lv = Int64.Parse(sval1Test1, Nfi);
                Fail("Should raise FormatException 1");
            }
            catch (FormatException)
            {
                // ok
            }

            lv = Int64.Parse(sval1Test2, style1, Nfi);
            AssertEquals("Value should be the same for Test2 with style1", val1, lv);
            lv = Int64.Parse(sval1Test2, Nfi);
            AssertEquals("Value should be the same for Test2 without style1", val1, lv);

            try
            {
                lv = Int64.Parse(sval1Test4, style1, Nfi);
                Fail("Should raise FormatException 3");
            }
            catch (FormatException)
            {
                // ok
            }

            lv = Int64.Parse(sval1Test5, NumberStyles.Currency, Nfi);
            AssertEquals("Value should be the same for Test5 and currency style", val1, lv);

            //test Parse(string s)
            Assert(MyInt64_1 == Int64.Parse(MyString1));
            Assert(MyInt64_2 == Int64.Parse(MyString2));
            Assert(MyInt64_3 == Int64.Parse(MyString3));
            try
            {
                Int64.Parse(null);
                Fail("#1:Should raise a System.ArgumentNullException");
            }
            catch (Exception e)
            {
                Assert("#2", typeof(ArgumentNullException) == e.GetType());
            }
            try
            {
                Int64.Parse("not-a-number");
                Fail("#3:Should raise a System.FormatException");
            }
            catch (Exception e)
            {
                Assert("#4", typeof(FormatException) == e.GetType());
            }
            //test Parse(string s, NumberStyles style)
            try
            {
                double OverInt = (double)Int64.MaxValue + 1;
                Int64.Parse(OverInt.ToString(), NumberStyles.Float);
                Fail("#5:Should raise a System.OverflowException");
            }
            catch (Exception e)
            {
                Assert("#6", typeof(OverflowException) == e.GetType());
            }
            try
            {
                Int64.Parse("10000000000000000", NumberStyles.HexNumber);
                Fail("#7:Should raise a System.OverflowException");
            }
            catch (Exception e)
            {
                Assert("#8", typeof(OverflowException) == e.GetType());
            }

            try
            {
#if NOT_PFX
                double OverInt = (double)Int64.MaxValue + 1;
                Int64.Parse(OverInt.ToString(), NumberStyles.Integer);
#else
                Int64.Parse("3.14", NumberStyles.Integer);
#endif
                Fail("#9:Should raise a System.FormatException");
            }
            catch (Exception e)
            {
                Assert("#10: " + e.GetType(), typeof(FormatException) == e.GetType());
            }

            AssertEquals("A1", (long)42, Int64.Parse(" " + NumberFormatInfo.CurrentInfo.CurrencySymbol + "42 ", NumberStyles.Currency));
            try
            {
                Int64.Parse(NumberFormatInfo.CurrentInfo.CurrencySymbol + "42", NumberStyles.Integer);
                Fail("#11:Should raise a System.FormatException");
            }
            catch (Exception e)
            {
                Assert("#12", typeof(FormatException) == e.GetType());
            }
            //test Parse(string s, IFormatProvider provider)
            Assert("A2", -42 == Int64.Parse(" -42 ", Nfi));
            try
            {
                Int64.Parse("%42", Nfi);
                Fail("#13:Should raise a System.FormatException");
            }
            catch (Exception e)
            {
                Assert("#14", typeof(FormatException) == e.GetType());
            }
            //test Parse(string s, NumberStyles style, IFormatProvider provider)
            Assert("A3", 16 == Int64.Parse(" 10 ", NumberStyles.HexNumber, Nfi));
            try
            {
                Int64.Parse(NumberFormatInfo.CurrentInfo.CurrencySymbol + "42", NumberStyles.Integer, Nfi);
                Fail("#15:Should raise a System.FormatException");
            }
            catch (Exception e)
            {
                Assert("#16", typeof(FormatException) == e.GetType());
            }
            try
            {
                long.Parse("9223372036854775808");
                AssertType.Fail("#17:should raise an OverflowException");
            }
            catch (Exception e)
            {
                Assert("#18", typeof(OverflowException) == e.GetType());
            }
            try
            {
                long.Parse("9223372036854775808", CultureInfo.InvariantCulture);
                AssertType.Fail("#19:should raise an OverflowException");
            }
            catch (Exception e)
            {
                Assert("#20", typeof(OverflowException) == e.GetType());
            }
        }

        public void TestToString()
        {
            string s;

            s = val1.ToString("c", Nfi);
            Assert("val1 does not become sval1Test6", s.Equals(sval1Test6));

            s = val1.ToString("n", Nfi);
            AssertEquals("val1 does not become sval1Test7", sval1Test7, s);

            //test ToString()
            AssertEquals("MyInt64_1.ToString()", MyString1, MyInt64_1.ToString());
            AssertEquals("MyInt64_2.ToString()", MyString2, MyInt64_2.ToString());
            AssertEquals("MyInt64_3.ToString()", MyString3, MyInt64_3.ToString());

            //test ToString(string format)
            //for (int i = 0; i < Formats1.Length; i++)
            //{
            //    AssertEquals("MyInt64_2.ToString(Formats1[" + i + "])", Results1[i], MyInt64_2.ToString(Formats1[i]));
            //    AssertEquals("MyInt64_3.ToString(Formats2[" + i + "])", Results2[i], MyInt64_3.ToString(Formats2[i]));
            //}

            //test ToString(string format, IFormatProvider provider);
            for (int i = 0; i < Formats1.Length; i++)
            {
                AssertEquals("MyInt64_2.ToString(Formats1[" + i + "], Nfi)", ResultsNfi1[i], MyInt64_2.ToString(Formats1[i], Nfi));
                AssertEquals("MyInt64_3.ToString(Formats2[" + i + "], Nfi)", ResultsNfi2[i], MyInt64_3.ToString(Formats2[i], Nfi));
            }
            try
            {
                MyInt64_1.ToString("z");
                Fail("Should raise a System.FormatException");
            }
            catch (Exception e)
            {
                AssertEquals("Exception is wrong type", typeof(FormatException), e.GetType());
            }
        }

        //public void TestUserCurrency()
        //{
        //    string s = "";
        //    long v;
        //    int iTest = 1;
        //    try
        //    {
        //        s = val1.ToString("c", NfiUser);
        //        iTest++;
        //        AssertEquals("Currency value type 1 is not what we want to try to parse", sval1UserCur1, s);
        //        iTest++;
        //        v = Int64.Parse(s, NumberStyles.Currency, NfiUser);
        //        iTest++;
        //        Assert(v == val1);
        //    }
        //    catch (Exception e)
        //    {
        //        Fail("1 Unexpected exception at iTest = " + iTest + ", s = " + s + ":e = " + e);
        //    }

        //    iTest = 1;
        //    try
        //    {
        //        s = val2.ToString("c", NfiUser);
        //        iTest++;
        //        AssertEquals("Currency value type 2 is not what we want to try to parse", sval2UserCur1, s);
        //        iTest++;
        //        v = Int64.Parse(s, NumberStyles.Currency, NfiUser);
        //        iTest++;
        //        Assert(v == val2);
        //    }
        //    catch (Exception e)
        //    {
        //        Fail("2 Unexpected exception at iTest = " + iTest + ":e = " + e);
        //    }
        //}

        //public void TestUserPercent()
        //{
        //    string s;

        //    s = val1.ToString("p", NfiUser);
        //    Assert(s.Equals(sval1UserPercent1));

        //    s = val2.ToString("p", NfiUser);
        //    Assert(s.Equals(sval2UserPercent1));
        //}

        [Test]
        public void Parse_MaxValue()
        {
            AssertEquals("9223372036854775807", Int64.MaxValue, Int64.Parse("9223372036854775807"));
        }

        [Test]
        public void Parse_MinValue()
        {
            AssertEquals("-9223372036854775808,10", Int64.MinValue, Int64.Parse("-9223372036854775808"));
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void Parse_OverByOneMaxValue()
        {
            Int64.Parse("9223372036854775808");
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void Parse_WayOverMaxValue()
        {
            Int64.Parse("1" + Int64.MaxValue.ToString());
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void Parse_OverByOneMinValue()
        {
            Int64.Parse("-9223372036854775809");
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void Parse_WayOverMinValue()
        {
            Int64.Parse(Int64.MinValue.ToString() + "1");
        }

        [Test]
        public void ToString_Defaults()
        {
            Int64 i = 254;
            // everything defaults to "G"
            string def = i.ToString("G");
            AssertEquals("ToString()", def, i.ToString());
            AssertEquals("ToString((IFormatProvider)null)", def, i.ToString((IFormatProvider)null));
            AssertEquals("ToString((string)null)", def, i.ToString((string)null));
            AssertEquals("ToString(empty)", def, i.ToString(String.Empty));
            AssertEquals("ToString(null,null)", def, i.ToString(null, null));
            AssertEquals("ToString(empty,null)", def, i.ToString(String.Empty, null));

            AssertEquals("ToString(G)", "254", def);
        }
    }
}
