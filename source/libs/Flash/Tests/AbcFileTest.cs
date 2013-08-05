#if NUNIT
using System;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using NUnit.Framework;

namespace DataDynamics.PageFX.Flash.Tests
{
    [TestFixture]
    public class AbcFileTest
    {
        [Test]
        public void TestIntPool()
        {
            var abc = new AbcFile();
            for (int i = 0; i < 10; ++i)
            {
                var c1 = abc.DefineInt(i);
                var c2 = abc.DefineInt(i);
                Assert.IsTrue(ReferenceEquals(c1, c2));

                c1 = abc.ImportConst(c1);
                c2 = abc.ImportConst(c2);
                Assert.IsTrue(ReferenceEquals(c1, c2));
            }
        }

        [Test]
        public void TestUIntPool()
        {
            var abc = new AbcFile();
            for (uint i = 0; i < 10; ++i)
            {
                var c1 = abc.DefineUInt(i);
                var c2 = abc.DefineUInt(i);
                Assert.IsTrue(ReferenceEquals(c1, c2));

                c1 = abc.ImportConst(c1);
                c2 = abc.ImportConst(c2);
                Assert.IsTrue(ReferenceEquals(c1, c2));
            }
        }

        [Test]
        public void TestDoublePool()
        {
            var abc = new AbcFile();
            for (int i = 0; i < 10; ++i)
            {
                var c1 = abc.DefineDouble(i);
                var c2 = abc.DefineDouble(i);
                Assert.IsTrue(ReferenceEquals(c1, c2));

                c1 = abc.ImportConst(c1);
                c2 = abc.ImportConst(c2);
                Assert.IsTrue(ReferenceEquals(c1, c2));
            }
        }

        [Test]
        public void TestStringPool()
        {
            var abc = new AbcFile();
            for (int i = 0; i < 10; ++i)
            {
                var c1 = abc.DefineString("name" + i);
                var c2 = abc.DefineString("name" + i);
                Assert.IsTrue(ReferenceEquals(c1, c2));

                c1 = abc.ImportConst(c1);
                c2 = abc.ImportConst(c2);
                Assert.IsTrue(ReferenceEquals(c1, c2));
            }
        }

        [Test]
        public void TestNamespaces()
        {
            var abc = new AbcFile();
            Assert.IsTrue(ReferenceEquals(abc.KnownNamespaces.GlobalPackage, abc.DefineNamespace(AbcConstKind.PackageNamespace, "")));

            for (int i = 0; i < 10; ++i)
            {
                var c1 = abc.DefinePublicNamespace("name" + i);
                var c2 = abc.DefinePublicNamespace("name" + i);
                Assert.IsTrue(ReferenceEquals(c1, c2));

                c1 = abc.ImportConst(c1);
                c2 = abc.ImportConst(c2);
                Assert.IsTrue(ReferenceEquals(c1, c2));
            }
        }

        [Test]
        public void TestQNames()
        {
            var abc = new AbcFile();
            for (int i = 0; i < 10; ++i)
            {
	            string name = "name" + i;
	            var c1 = abc.DefineName(QName.Global(name));
	            string name1 = "name" + i;
	            var c2 = abc.DefineName(QName.Global(name1));
                Assert.IsTrue(ReferenceEquals(c1, c2));

                c1 = abc.ImportConst(c1);
                c2 = abc.ImportConst(c2);
                Assert.IsTrue(ReferenceEquals(c1, c2));
            }
        }

        private static void TestBuiltinType(AbcFile abc, AvmTypeCode type)
        {
            var name = abc.BuiltinTypes[type];
            var mn = abc.ImportConst(name);
            Assert.IsTrue(ReferenceEquals(name, mn));
        }

        [Test]
        public void TestBuiltinTypes()
        {
            var abc = new AbcFile();
            foreach (AvmTypeCode type in Enum.GetValues(typeof(AvmTypeCode)))
                TestBuiltinType(abc, type);

            Assert.IsTrue(ReferenceEquals(abc.BuiltinTypes[AvmTypeCode.Void], abc.DefineName(QName.Global("void"))));
            Assert.IsTrue(ReferenceEquals(abc.BuiltinTypes[AvmTypeCode.Int32], abc.DefineName(QName.Global("int"))));
            Assert.IsTrue(ReferenceEquals(abc.BuiltinTypes[AvmTypeCode.UInt32], abc.DefineName(QName.Global("uint"))));
            Assert.IsTrue(ReferenceEquals(abc.BuiltinTypes[AvmTypeCode.String], abc.DefineName(QName.Global("String"))));
        }
    }
}
#endif