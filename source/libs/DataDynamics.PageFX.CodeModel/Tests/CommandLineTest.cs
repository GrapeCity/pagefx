#if NUNIT
using NUnit.Framework;

namespace DataDynamics.Tests
{
    [TestFixture]
    public class CommandLineTest
    {
        [Test]
        public void TestOptions()
        {
            string[] values = { "aaa", "111", "\"aaa\"" };
            for (int i = 0; i < values.Length; ++i)
            {
                string v = values[i];
                var cl = CommandLine.Parse(string.Format("/o:{0}", v));
                Assert.AreEqual(v.Unquote(), cl.GetOption(null, "o"), "#" + i);
            }
        }

        [Test]
        public void TestQuotedOptions()
        {
            string s = "/r:\"c:\\pfx\"\\framework\\corlib.dll";
            var cl = CommandLine.Parse(s);
            string r = cl.GetOption(null, "r");
            Assert.AreEqual("c:\\pfx\\framework\\corlib.dll", r, "#1");
        }
    }
}
#endif