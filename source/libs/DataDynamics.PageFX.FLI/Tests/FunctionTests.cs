#if NUNIT
using System.Globalization;
using NUnit.Framework;

namespace DataDynamics.PageFX.FlashLand.Tests
{
    [TestFixture]
    public class FunctionTests
    {
        [Test]
        public void EmptyFunction()
        {
            var f = Function.Parse("f()");
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(0, f.Arguments.Count, "argc");
        }

        [Test]
        public void EmptyString()
        {
            var f = Function.Parse("");
            Assert.IsNull(f);
        }

        [Test]
        public void String1()
        {
            var f = Function.Parse("f('aaa')");
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(1, f.Arguments.Count, "argc");
            Assert.IsNull(f[0].Name);
            Assert.AreEqual("aaa", f[0].Value, "value of arg0");
        }

        [Test]
        public void String2()
        {
            var f = Function.Parse("f('\\\\\\n')");
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(1, f.Arguments.Count, "argc");
            Assert.IsNull(f[0].Name);
            Assert.AreEqual("\\\n", f[0].Value, "value of arg0");
        }

        [Test]
        public void True()
        {
            var f = Function.Parse("f(true)");
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(1, f.Arguments.Count, "argc");
            Assert.IsNull(f[0].Name);
            Assert.AreEqual(true, f[0].Value, "value of arg0");
        }

        [Test]
        public void False()
        {
            var f = Function.Parse("f(false)");
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(1, f.Arguments.Count, "argc");
            Assert.IsNull(f[0].Name);
            Assert.AreEqual(false, f[0].Value, "value of arg0");
        }

        static void TestNumber(string ns)
        {
            double n = double.Parse(ns, new NumberFormatInfo { NumberDecimalSeparator = "." });
            string s = string.Format("f({0})", ns);
            var f = Function.Parse(s);
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(1, f.Arguments.Count, "argc");
            Assert.IsNull(f[0].Name);
            Assert.AreEqual(n, f[0].Value, "value of arg0");
        }

        [Test]
        public void Numbers()
        {
            TestNumber("1");
            TestNumber("-1");
            TestNumber("+1");
            TestNumber("2.5");
            TestNumber("-2.5");
            TestNumber("+2.5");
            TestNumber("2.5e-5");
            TestNumber("-2.5E-5");
            TestNumber("+2.5E+5");
        }

        [Test]
        public void NameValue1()
        {
            var f = Function.Parse("f(source = 'aaa', q = true)");
            Assert.IsNotNull(f);
            Assert.AreEqual("f", f.Name, "name");
            Assert.AreEqual(2, f.Arguments.Count, "argc");
            Assert.AreEqual("source", f[0].Name, "name of arg0");
            Assert.AreEqual("aaa", f[0].Value, "value of arg0");
            Assert.AreEqual("q", f[1].Name, "name of arg1");
            Assert.AreEqual(true, f[1].Value, "value of arg1");
        }

        [Test]
        public void NestedFunction()
        {
            var f = Function.Parse("foo(a = bar (1), b = 'aaa')");
            Assert.IsNotNull(f);
            Assert.AreEqual("foo", f.Name, "name");
            Assert.AreEqual(2, f.Arguments.Count, "argc");
            Assert.AreEqual("a", f[0].Name, "name of arg0");
            Assert.AreEqual("b", f[1].Name, "name of arg1");
            Assert.AreEqual("aaa", f[1].Value, "value of arg1");

            var bar = f[0].Value as Function;
            Assert.IsNotNull(bar, "#1");
            Assert.AreEqual(1, bar.Arguments.Count, "#2");
            Assert.IsNull(bar[0].Name, "#3");
            Assert.AreEqual(1, bar[0].Value, "#4");
        }
    }
}
#endif