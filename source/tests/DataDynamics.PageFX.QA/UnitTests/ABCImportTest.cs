using System;
using DataDynamics.PageFX.FLI.ABC;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
{
    [TestFixture]
    public class ABCImportTest
    {
        [Test]
        public void ImportString1()
        {
            var abc1 = new AbcFile();
            var c1 = abc1.DefineString("aaa");

            var abc2 = new AbcFile();
            var c2 = abc2.ImportConst(c1);

            Assert.AreEqual(c1.Index, c2.Index, "#1");
            Assert.AreEqual(c1.Value, c2.Value, "#2");
        }

        [Test]
        public void ImportQName1()
        {
            var abc1 = new AbcFile();
            var c1 = abc1.DefineQName("ns", "name");

            var abc2 = new AbcFile();
            var c2 = abc2.ImportConst(c1);

            Assert.AreEqual(c1.Index, c2.Index, "#1");
            Assert.AreEqual(c1.Namespace.Index, c2.Namespace.Index, "#2");
            Assert.AreEqual(c1.Namespace.NameString, c2.Namespace.NameString, "#2");
        }
    }
}