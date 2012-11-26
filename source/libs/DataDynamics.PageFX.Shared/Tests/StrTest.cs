#if NUNIT
using NUnit.Framework;

namespace DataDynamics.Tests
{
    [TestFixture]
    public class StrTest
    {
        [Test]
        public void TestUnquote()
        {
            string s;

            s = "";
            Assert.AreEqual(s, s.Unquote(), "#1");

            s = "\"";
            Assert.AreEqual(s, s.Unquote(), "#2");

            s = "\'";
            Assert.AreEqual(s, s.Unquote(), "#3");

            s = "aaa";
            Assert.AreEqual(s, s.Unquote(), "#4");

            s = "\"aaa";
            Assert.AreEqual(s, s.Unquote(), "#5");

            s = "\'aaa";
            Assert.AreEqual(s, s.Unquote(), "#6");

            s = "aaa\"";
            Assert.AreEqual(s, s.Unquote(), "#7");

            s = "aaa\'";
            Assert.AreEqual(s, s.Unquote(), "#8");

            s = "\"aaa\"";
            Assert.AreEqual("aaa", s.Unquote(), "#9");

            s = "\'aaa\'";
            Assert.AreEqual("aaa", s.Unquote(), "#10");
        }

        [Test]
        public void TestReplaceVars()
        {
            string t = "bla $x = $v bla";
            Assert.AreEqual("bla a = aaa bla",
                            t.ReplaceVars("x", "a", "v", "aaa"), t);

            t = "public $TYPE()";
            Assert.AreEqual("public Vector()",
                            t.ReplaceVars("TYPE", "Vector"), t);

            t = "public $_TYPE()";
            Assert.AreEqual("public Vector()",
                            t.ReplaceVars("_TYPE", "Vector"), t);

            t = "public $_TYPE1()";
            Assert.AreEqual("public Vector()",
                            t.ReplaceVars("_TYPE1", "Vector"), t);
        }
    }
}
#endif