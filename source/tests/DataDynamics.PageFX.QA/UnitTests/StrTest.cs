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
            Assert.AreEqual(s, Str.Unquote(s), "#1");

            s = "\"";
            Assert.AreEqual(s, Str.Unquote(s), "#2");

            s = "\'";
            Assert.AreEqual(s, Str.Unquote(s), "#3");

            s = "aaa";
            Assert.AreEqual(s, Str.Unquote(s), "#4");

            s = "\"aaa";
            Assert.AreEqual(s, Str.Unquote(s), "#5");

            s = "\'aaa";
            Assert.AreEqual(s, Str.Unquote(s), "#6");

            s = "aaa\"";
            Assert.AreEqual(s, Str.Unquote(s), "#7");

            s = "aaa\'";
            Assert.AreEqual(s, Str.Unquote(s), "#8");

            s = "\"aaa\"";
            Assert.AreEqual("aaa", Str.Unquote(s), "#9");

            s = "\'aaa\'";
            Assert.AreEqual("aaa", Str.Unquote(s), "#10");
        }

        [Test]
        public void TestReplaceVars()
        {
            string t = "bla $x = $v bla";
            Assert.AreEqual("bla a = aaa bla",
                            Str.ReplaceVars(t, "x", "a", "v", "aaa"), t);

            t = "public $TYPE()";
            Assert.AreEqual("public Vector()",
                            Str.ReplaceVars(t, "TYPE", "Vector"), t);

            t = "public $_TYPE()";
            Assert.AreEqual("public Vector()",
                            Str.ReplaceVars(t, "_TYPE", "Vector"), t);

            t = "public $_TYPE1()";
            Assert.AreEqual("public Vector()",
                            Str.ReplaceVars(t, "_TYPE1", "Vector"), t);
        }
    }
}