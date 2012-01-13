// EnumTest.cs - NUnit Test Cases for the System.Enum class
//
// David Brandt (bucky@keystreams.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// 

using NUnit.Framework;
using System;
using System.Reflection;

namespace MonoTests.System
{
    public class EnumTest : TestCase
    {
        public EnumTest() { }

        enum TestingEnum { This, Is, A, Test };
        enum TestingEnum2 { This, Is, A, Test };
        enum TestingEnum3 : ulong { This, Is, A, Test = 0xffffffffffffffff };
        enum TestingEnum4 : byte { This, Is, A, Test = byte.MaxValue }
        enum TestingEnum5 : short { This, Is, A, Test = short.MaxValue }
        enum TestingEnum6 { This, Is, A, Test = int.MaxValue }

        public void TestCompareTo()
        {
            Enum e1 = new TestingEnum();
            Enum e2 = new TestingEnum();
            Enum e3 = new TestingEnum2();

            AssertEquals("An enum should equal itself",
                     0, e1.CompareTo(e1));
            AssertEquals("An enum should equal a copy",
                     0, e1.CompareTo(e2));

            TestingEnum x = TestingEnum.This;
            TestingEnum y = TestingEnum.Is;
            AssertEquals("should equal", 0, x.CompareTo(x));
            AssertEquals("less than", -1, x.CompareTo(y));
            AssertEquals("greater than", 1, y.CompareTo(x));

            {
                bool errorThrown = false;
                try
                {
                    e1.CompareTo(e3);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("1) Compare type mismatch not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    ((Enum)e1).CompareTo((Enum)e3);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("2) Compare type mismatch not caught.",
                       errorThrown);
            }
        }

        public void TestEquals()
        {
            Enum e1 = new TestingEnum();
            Enum e2 = new TestingEnum();
            Enum e3 = new TestingEnum2();

            Assert("An enum should equal itself", e1.Equals(e1));
            Assert("An enum should equal a copy", e1.Equals(e2));

            Assert("Shouldn't match", !e1.Equals(e3));
            Assert("Shouldn't match null", !e1.Equals(null));
        }

#if NOT_PFX
        public void TestFormat_Args()
        {
            try
            {
                TestingEnum x = TestingEnum.Test;
                Enum.Format(null, x, "G");
                Fail("#A1: null first arg not caught.");
            }
            catch (ArgumentNullException ex)
            {
                AssertEquals("#A2", "enumType", ex.ParamName);
            }
            catch (Exception e)
            {
                Fail("#A3: first arg null, wrong exception: " + e.ToString());
            }

            try
            {
                Enum.Format(typeof(TestingEnum), null, "G");
                Fail("#B1: null second arg not caught.");
            }
            catch (ArgumentNullException ex)
            {
                AssertEquals("#B2", "value", ex.ParamName);
            }
            catch (Exception e)
            {
                Fail("#B3: second arg null, wrong exception: " + e.ToString());
            }

            try
            {
                TestingEnum x = TestingEnum.Test;
                Enum.Format(x.GetType(), x, null);
                Fail("#C1: null third arg not caught.");
            }
            catch (ArgumentNullException ex)
            {
                AssertEquals("#C2", "format", ex.ParamName);
            }
            catch (Exception e)
            {
                Fail("#C3: third arg null, wrong exception: " + e.ToString());
            }

            try
            {
                TestingEnum x = TestingEnum.Test;
                Enum.Format(typeof(string), x, "G");
                Fail("#D1: bad type arg not caught.");
            }
            catch (ArgumentException)
            {
                // Type provided must be an Enum
            }
            catch (Exception e)
            {
                Fail("#D2: bad type, wrong exception: " + e.ToString());
            }

            try
            {
                TestingEnum x = TestingEnum.Test;
                TestingEnum2 y = TestingEnum2.Test;
                Enum.Format(y.GetType(), x, "G");
                Fail("#E1: wrong enum type not caught.");
            }
            catch (ArgumentException ex)
            {
                // Object must be the same type as the enum. The type passed in was
                // MonoTests.System.EnumTest.TestingEnum2; the enum type was
                // MonoTests.System.EnumTest.TestingEnum
                AssertNotNull("#E2", ex.Message);
                Assert("#E3", ex.Message.IndexOf(typeof(TestingEnum2).FullName) != -1);
                Assert("#E4", ex.Message.IndexOf(typeof(TestingEnum).FullName) != -1);
            }
            catch (Exception e)
            {
                Fail("#E5: wrong enum type, wrong exception: " + e.ToString());
            }

            try
            {
                String bad = "huh?";
                TestingEnum x = TestingEnum.Test;
                Enum.Format(x.GetType(), bad, "G");
                Fail("#F1: non-enum object not caught.");
            }
            catch (ArgumentException ex)
            {
                // Enum underlying type and the object must be the same type or
                // object. Type passed in was String.String; the enum underlying
                // was System.Int32
                AssertNotNull("#F2", ex.Message);
                Assert("#F3", ex.Message.IndexOf(typeof(string).FullName) != -1);
                Assert("#F4", ex.Message.IndexOf(typeof(int).FullName) != -1);
            }
            catch (Exception e)
            {
                Fail("#F5: non-enum object, wrong exception: " + e.ToString());
            }

            string[] codes = {"a", "b", "c", "ad", "e", "af", "ag", "h", 
				  "i", "j", "k", "l", "m", "n", "o", "p", 
				  "q", "r", "s", "t", "u", "v", "w", "ax", 
				  "y", "z"};
            foreach (string code in codes)
            {
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    Enum.Format(x.GetType(), x, code);
                    Fail("bad format code not caught - " + code);
                }
                catch (FormatException)
                {
                    // do nothing
                }
                catch (Exception e)
                {
                    Fail(String.Format("bad format code ({0}), wrong exception: {1}",
                                         code, e.ToString()));
                }
            }

            TestingEnum test = TestingEnum.Test;
            //NOTE: Asserts below is failed in CLR and AVM
#if NOT_PFX
            AssertEquals("decimal format wrong", "3", Enum.Format(test.GetType(), test, "d"));

            AssertEquals("decimal format wrong for ulong enums",
                     "18446744073709551615", Enum.Format(typeof(TestingEnum3), TestingEnum3.Test, "d"));
#endif

            AssertEquals("named format wrong",
                     "Test", Enum.Format(test.GetType(), test, "g"));
            AssertEquals("hex format wrong",
                     "00000003", Enum.Format(test.GetType(), test, "x"));
            AssertEquals("bitfield format wrong",
                     "Test", Enum.Format(test.GetType(), test, "f"));
        }
#endif

#if NOT_PFX
        public void TestFormat_FormatSpecifier()
        {
            ParameterAttributes pa = ParameterAttributes.In | ParameterAttributes.HasDefault;
            const string fFormatOutput = "In, HasDefault";
            const string xFormatOutput = "00001001";
            string fOutput = Enum.Format(pa.GetType(), pa, "f");
            AssertEquals("#F_FS:f", fFormatOutput, fOutput);
            string xOutput = Enum.Format(pa.GetType(), pa, "x");
            AssertEquals("#F_FS:x", xFormatOutput, xOutput);

            AssertEquals("#F_FSX:01", "00", TestingEnum4.This.ToString("x"));
            AssertEquals("#F_FSX:02", "00", TestingEnum4.This.ToString("X"));
#if !TARGET_JVM // This appears not to work under .Net
            AssertEquals("#F_FSX:03", "ff", TestingEnum4.Test.ToString("x"));
#endif // TARGET_JVM
            AssertEquals("#F_FSX:04", "FF", TestingEnum4.Test.ToString("X"));

            AssertEquals("#F_FSX:05", "0000", TestingEnum5.This.ToString("x"));
            AssertEquals("#F_FSX:06", "0000", TestingEnum5.This.ToString("X"));
#if !TARGET_JVM // This appears not to work under .Net
            AssertEquals("#F_FSX:07", "7fff", TestingEnum5.Test.ToString("x"));
#endif // TARGET_JVM
            AssertEquals("#F_FSX:08", "7FFF", TestingEnum5.Test.ToString("X"));

            AssertEquals("#F_FSX:09", "00000000", TestingEnum6.This.ToString("x"));
            AssertEquals("#F_FSX:10", "00000000", TestingEnum6.This.ToString("X"));
#if !TARGET_JVM // This appears not to work under .Net
            AssertEquals("#F_FSX:11", "7fffffff", TestingEnum6.Test.ToString("x"));
#endif // TARGET_JVM
            AssertEquals("#F_FSX:12", "7FFFFFFF", TestingEnum6.Test.ToString("X"));

            AssertEquals("#F_FSX:13", "0000000000000000", TestingEnum3.This.ToString("x"));
            AssertEquals("#F_FSX:14", "0000000000000000", TestingEnum3.This.ToString("X"));

            //NOTE: CLR uses only uppercase leters for assert below
#if NOT_PFX
            AssertEquals("#F_FSX:15", "ffffffffffffffff", TestingEnum3.Test.ToString("x"));
#endif

            AssertEquals("#F_FSX:16", "FFFFFFFFFFFFFFFF", TestingEnum3.Test.ToString("X"));
        }
#endif

        public void TestGetHashCode()
        {
            Enum e1 = new TestingEnum();
            Enum e2 = new TestingEnum2();

            AssertEquals("hash code is deterministic",
                     e1.GetHashCode(), e1.GetHashCode());
        }

        public void TestGetName()
        {
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    Enum.GetName(null, x);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null first arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    Enum.GetName(x.GetType(), null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null second arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    TestingEnum x = TestingEnum.Test;
                    Enum.GetName(bad.GetType(), x);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum type not caught.",
                       errorThrown);
            }
#if NOT_MSCLR
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    TestingEnum2 y = TestingEnum2.Test;
                    Enum.GetName(y.GetType(), x);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("wrong enum type not caught.",
                       errorThrown);
            }
#endif
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    TestingEnum x = TestingEnum.Test;
                    Enum.GetName(x.GetType(), bad);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum object not caught.",
                       errorThrown);
            }
            {
                TestingEnum x = TestingEnum.This;
                TestingEnum y = TestingEnum.Is;
                TestingEnum z = TestingEnum.A;

                AssertEquals("first name doesn't match",
                         "This", Enum.GetName(x.GetType(), x));
                AssertEquals("second name doesn't match",
                         "Is", Enum.GetName(y.GetType(), y));
                AssertEquals("third name doesn't match",
                         "A", Enum.GetName(z.GetType(), z));
            }
        }

#if NOT_PFX
        public void TestGetNames()
        {
            {
                bool errorThrown = false;
                try
                {
                    Enum.GetNames(null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null type not caught.",
                       errorThrown);
            }
            {
                TestingEnum x = TestingEnum.This;
                string[] match = { "This", "Is", "A", "Test" };
                string[] names = Enum.GetNames(x.GetType());
                AssertNotNull("Got no names", names);
                AssertEquals("names wrong size",
                         match.Length, names.Length);
                for (int i = 0; i < names.Length; i++)
                {
                    AssertEquals("name mismatch",
                             match[i], names[i]);
                }
            }
        }
#endif

        public void TestGetTypeCode()
        {
            TestingEnum x = TestingEnum.This;
            TestingEnum y = new TestingEnum();
            AssertEquals("01 bad type code",
                     TypeCode.Int32, x.GetTypeCode());
            AssertEquals("02 bad type code",
                     TypeCode.Int32, y.GetTypeCode());
        }

        enum TestShortEnum : short { zero, one, two, three, four, five, six };
        public void TestGetUnderlyingType()
        {
            {
                bool errorThrown = false;
                try
                {
                    Enum.GetUnderlyingType(null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null type not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    Enum.GetUnderlyingType(bad.GetType());
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum type not caught.",
                       errorThrown);
            }
            {
                short sh = 5;
                int i = 5;
                Enum t1 = new TestingEnum();
                Enum t2 = new TestShortEnum();
                AssertEquals("Wrong default underlying type",
                         i.GetType(),
                         Enum.GetUnderlyingType(t1.GetType()));
                AssertEquals("Not short underlying type",
                         sh.GetType(),
                         Enum.GetUnderlyingType(t2.GetType()));
            }
        }

#if NOT_PFX
        public void TestGetValues()
        {
            {
                bool errorThrown = false;
                try
                {
                    Enum.GetValues(null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null type not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    Enum.GetValues(bad.GetType());
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum type not caught.",
                       errorThrown);
            }
            {
                Enum t1 = new TestingEnum();
                Array a1 = Enum.GetValues(t1.GetType());
                for (int i = 0; i < a1.Length; i++)
                {
                    AssertEquals("wrong enum value",
                             (TestingEnum)i,
                             a1.GetValue(i));
                }
            }
            {
                Enum t1 = new TestShortEnum();
                Array a1 = Enum.GetValues(t1.GetType());
                for (short i = 0; i < a1.Length; i++)
                {
                    AssertEquals("wrong short enum value",
                             (TestShortEnum)i,
                             a1.GetValue(i));
                }
            }
        }
#endif

        public void TestIsDefined()
        {
            {
                bool errorThrown = false;
                try
                {
                    Enum.IsDefined(null, 1);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null first arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    Enum.IsDefined(x.GetType(), null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null second arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    int i = 4;
                    Enum.IsDefined(bad.GetType(), i);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum type not caught.",
                       errorThrown);
            }

            try
            {
                TestingEnum x = TestingEnum.Test;
                short i = 4;
                Enum.IsDefined(x.GetType(), i);
                Fail("wrong underlying type not caught.");
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                Fail("wrong Exception thrown (" + e.ToString() + ")for underlying type not caught.");
            }

            // spec says yes, MS impl says no.
            //{
            //bool errorThrown = false;
            //try {
            //String bad = "huh?";
            //TestingEnum x = TestingEnum.Test;
            //Enum.IsDefined(x.GetType(), bad);
            //} catch (ExecutionEngineException) {
            //errorThrown = true;
            //}
            //Assert("non-enum object not caught.", 
            //errorThrown);
            //}

#if NOT_PFX
            {
                Enum t1 = new TestingEnum();
                int i = 0;
                for (i = 0;
                     i < Enum.GetValues(t1.GetType()).Length; i++)
                {
                    Assert("should have value for i=" + i,
                           Enum.IsDefined(t1.GetType(), i));
                }
                Assert("Shouldn't have value",
                       !Enum.IsDefined(t1.GetType(), i));
            }
#endif
        }

#if NOT_PFX
        public void TestParse1()
        {
            {
                bool errorThrown = false;
                try
                {
                    String name = "huh?";
                    Enum.Parse(null, name);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null first arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    Enum.Parse(x.GetType(), null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null second arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    Enum.Parse(bad.GetType(), bad);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum type not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    String bad = "";
                    Enum.Parse(x.GetType(), bad);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("empty string not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    String bad = " ";
                    Enum.Parse(x.GetType(), bad);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("space-only string not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    TestingEnum x = TestingEnum.Test;
                    Enum.Parse(x.GetType(), bad);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("not-in-enum error not caught.",
                       errorThrown);
            }
            {
                TestingEnum t1 = new TestingEnum();

                AssertEquals("parse first enum",
                         TestingEnum.This,
                         Enum.Parse(t1.GetType(), "This"));

                AssertEquals("parse second enum",
                         TestingEnum.Is,
                         Enum.Parse(t1.GetType(), "Is"));

                AssertEquals("parse third enum",
                         TestingEnum.A,
                         Enum.Parse(t1.GetType(), "A"));

                AssertEquals("parse last enum",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "Test"));

                AssertEquals("parse last enum with whitespace",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "    \n\nTest\t"));

                AssertEquals("parse bitwise-or enum",
                         TestingEnum.Is,
                         Enum.Parse(t1.GetType(), "This,Is"));

                AssertEquals("parse bitwise-or enum",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "This,Test"));

                AssertEquals("parse bitwise-or enum",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "This,Is,A"));

                AssertEquals("parse bitwise-or enum with whitespace",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "   \n\tThis \t\n,    Is,A \n"));
            }
        }
#endif

#if NOT_PFX
        public void TestParse2()
        {
            {
                bool errorThrown = false;
                try
                {
                    String name = "huh?";
                    Enum.Parse(null, name, true);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null first arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    Enum.Parse(x.GetType(), null, true);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null second arg not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    Enum.Parse(bad.GetType(), bad, true);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("non-enum type not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    String bad = "";
                    Enum.Parse(x.GetType(), bad, true);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("empty string not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    TestingEnum x = TestingEnum.Test;
                    String bad = " ";
                    Enum.Parse(x.GetType(), bad, true);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("space-only string not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "huh?";
                    TestingEnum x = TestingEnum.Test;
                    Enum.Parse(x.GetType(), bad, true);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("not-in-enum error not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    String bad = "test";
                    TestingEnum x = TestingEnum.Test;
                    Enum.Parse(x.GetType(), bad, false);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("not-in-enum error not caught.",
                       errorThrown);
            }
            {
                TestingEnum t1 = new TestingEnum();
                AssertEquals("parse first enum",
                         TestingEnum.This,
                         Enum.Parse(t1.GetType(), "this", true));
                AssertEquals("parse second enum",
                         TestingEnum.Is,
                         Enum.Parse(t1.GetType(), "is", true));
                AssertEquals("parse third enum",
                         TestingEnum.A,
                         Enum.Parse(t1.GetType(), "a", true));
                AssertEquals("parse last enum",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "test", true));

                AssertEquals("parse last enum with whitespace",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "    \n\ntest\t", true));

                AssertEquals("parse bitwise-or enum",
                         TestingEnum.Is,
                         Enum.Parse(t1.GetType(), "This,is", true));
                AssertEquals("parse bitwise-or enum",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "This,test", true));
                AssertEquals("parse bitwise-or enum",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "This,is,A", true));

                AssertEquals("parse bitwise-or enum with whitespace",
                         TestingEnum.Test,
                         Enum.Parse(t1.GetType(), "   \n\tThis \t\n,    is,a \n",
                                    true));
            }
        }
#endif

#if NOT_PFX
        [Test]
        public void TestParseNumericValues()
        {
            AssertEquals("Parse numeric value", TestingEnum3.This, Enum.Parse(typeof(TestingEnum3), "0", false));
            //AssertEquals("Parse numeric value", TestingEnum3.Is, Enum.Parse(typeof(TestingEnum3), "1", false));
            //AssertEquals("Parse numeric value", TestingEnum3.A, Enum.Parse(typeof(TestingEnum3), "2", false));
        }
#endif

        public void TestToObject()
        {
            {
                bool errorThrown = false;
                try
                {
                    Enum.ToObject(null, 1);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null type not caught.",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    Enum.ToObject("huh?".GetType(), 1);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("null type not caught.",
                       errorThrown);
            }
            {
                TestingEnum t1 = new TestingEnum();
                AssertEquals("Should get object",
                         TestingEnum.This,
                         Enum.ToObject(t1.GetType(), 0));
            }
            // TODO - should probably test all the different underlying types
        }

        [Flags]
        enum SomeEnum { a, b, c };

        [Flags]
        enum SomeByteEnum : byte { a, b, c };

        [Flags]
        enum SomeInt64Enum : long { a, b, c };

        public void TestToString()
        {
            int i = 0;
            try
            {
                i++;
                AssertEquals("invalid string", "This",
                         TestingEnum.This.ToString());
                i++;
                AssertEquals("invalid string", "Is",
                         TestingEnum.Is.ToString());
                i++;
                AssertEquals("invalid string", "A",
                         TestingEnum.A.ToString());
                i++;
                AssertEquals("invalid string", "Test",
                         TestingEnum.Test.ToString());

                Enum is1 = TestingEnum.Is;

                i++;
                AssertEquals("decimal parse wrong",
                         "1", is1.ToString("d"));
                i++;
                AssertEquals("named format wrong",
                         "Is", is1.ToString("g"));
                i++;
                AssertEquals("hex format wrong",
                         "00000001", is1.ToString("x"));
                i++;
                AssertEquals("bitfield format wrong",
                         "Is", is1.ToString("f"));

                i++;
                AssertEquals("bitfield with flags format wrong for Int32 enum",
                         "b, c", ((SomeEnum)3).ToString("f"));
                i++;
                AssertEquals("bitfield with flags format wrong for Byte enum",
                         "b, c", ((SomeByteEnum)3).ToString("f"));
                i++;
                AssertEquals("bitfield with flags format wrong for Int64 enum",
                         "b, c", ((SomeInt64Enum)3).ToString("f"));

                i++;
                AssertEquals("bitfield with unknown flags format wrong for Int32 enum",
                         "12", ((SomeEnum)12).ToString("f"));
                i++;
                AssertEquals("bitfield with unknown flags format wrong for Byte enum",
                         "12", ((SomeByteEnum)12).ToString("f"));
                i++;
                AssertEquals("bitfield with unknown flags format wrong for Int64 enum",
                         "12", ((SomeInt64Enum)12).ToString("f"));

            }
            catch (Exception e)
            {
                Fail("Unexpected exception at i = " + i + " with e=" + e);
            }
        }

        enum E
        {
            Aa = 0,
            Bb = 1,
            Cc = 2,
            Dd = 3,
        }

        [Flags]
        enum E2
        {
            Aa,
            Bb,
            Cc,
            Dd,
        }

#if NOT_PFX
        [Test]
        public void TestFlags()
        {
            int[] evalues = new int[4] { 0, 1, 2, 3 };
            E[] e = new E[4] { E.Aa, E.Bb, E.Cc, E.Dd };

            for (int i = 0; i < 4; ++i)
            {
                Assertion.AssertEquals("#" + i,
                    e[i].ToString(),
                    Enum.Format(typeof(E), evalues[i], "f"));
            }
        }
#endif


#if NOT_PFX
        [Test]
        public void TestFlags2()
        {
            int invalidValue = 1000;

            Assertion.AssertEquals("#01",
                    invalidValue.ToString(),
                    Enum.Format(typeof(E2), invalidValue, "g"));
        }
#endif

        enum E3 { A = 0, B = 1, C = 2, D = 3, }
        enum UE : ulong { A = 1, B = 2, C = 4, D = 8, }
        enum EA { A = 0, B = 2, C = 3, D = 4 }

#if NOT_PFX
        [Test]
        public void TestAnotherFormatBugPinned()
        {
            Assertion.AssertEquals("#01", "100", Enum.Format(typeof(E3), 100, "f"));
        }
#endif

#if NOT_PFX
        [Test]
        public void TestLogicBugPinned()
        {
            string format = null;
            string[] names = new string[] { "A", "B", "C", "D", };
            string[] fmtSpl = null;
            UE ue = UE.A | UE.B | UE.C | UE.D;

            //all flags must be in format return
            format = Enum.Format(typeof(UE), ue, "f");
            fmtSpl = format.Split(',');
            for (int i = 0; i < fmtSpl.Length; ++i)
                fmtSpl[i] = fmtSpl[i].Trim();

            foreach (string nval in fmtSpl)
                Assertion.Assert(nval + " is not a valid enum value name", Array.IndexOf(names, nval) >= 0);

            foreach (string nval in names)
                Assertion.Assert(nval + " is not contained in format return.", Array.IndexOf(fmtSpl, nval) >= 0);
        }
#endif

        // TODO - ToString with IFormatProviders

        [Flags]
        enum FlagsNegativeTestEnum
        {
            None = 0,
            One = 1,
            Two = 2,
            Negative = unchecked((int)0xFFFF0000)
        }

        [Test]
        public void TestFlags3()
        {
            FlagsNegativeTestEnum t;

            t = FlagsNegativeTestEnum.None;
            Assertion.AssertEquals("#01", "None", t.ToString());
            t = FlagsNegativeTestEnum.One;
            Assertion.AssertEquals("#02", "One", t.ToString());
        }
    }
}
