using System;
using DataDynamics.PageFX.FLI.ABC;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
{
    [TestFixture]
    public class ABCDefineTest
    {
        #region StringPool
        [Test]
        public void DefineString_Array()
        {
            var abc = new AbcFile();
            var arr = new[] { "", "aaa", "aa", "aaa", "aa" };
            for (int i = 0; i < arr.Length; ++i)
            {
                var s = arr[i];
                var c1 = abc.DefineString(s);
                var c2 = abc.DefineString(s);
                Assert.AreSame(c1, c2, "#A" + i);
            }
        }

        [Test]
        public void DefineString_Empty()
        {
            var abc = new AbcFile();
            var c1 = abc.DefineString("");
            Assert.AreEqual(1, c1.Index, "#1");
        }
        #endregion

        #region IntPool
        [Test]
        public void DefineInt32_Array()
        {
            var abc = new AbcFile();
            var arr = new[] { 0, 1, 2, 3, 100, 1000, -100 };
            for (int i = 0; i < arr.Length; ++i)
            {
                var c1 = abc.DefineInt(arr[i]);
                var c2 = abc.DefineInt(arr[i]);
                Assert.AreSame(c1, c2, "#A" + i);
            }
        }

        [Test]
        public void DefineInt32_Zero()
        {
            var abc = new AbcFile();
            var c1 = abc.DefineInt(0);
            Assert.AreEqual(1, c1.Index, "#1");
        }
        #endregion

        #region UIntPool
        [Test]
        public void DefineUInt32_Array()
        {
            var abc = new AbcFile();
            var arr = new uint[] { 0, 1, 2, 3, 100 };
            for (int i = 0; i < arr.Length; ++i)
            {
                var c1 = abc.DefineUInt(arr[i]);
                var c2 = abc.DefineUInt(arr[i]);
                Assert.AreSame(c1, c2, "#A" + i);
            }
        }

        [Test]
        public void DefineUInt32_Zero()
        {
            var abc = new AbcFile();
            var c1 = abc.DefineUInt(0);
            Assert.AreEqual(1, c1.Index, "#1");
        }
        #endregion

        #region NamespacePool
        static void DefineNamespace_Array(AbcFile abc, AbcConstKind kind)
        {
            var arr = new[] { "", "aaa", "bbb" };
            for (int i = 0; i < arr.Length; ++i)
            {
                var name = arr[i];
                var c1 = abc.DefineNamespace(kind, name);
                var c2 = abc.DefineNamespace(kind, name);
                Assert.AreSame(c1, c2, "#" + kind + ":" + i);
            }
        }

        [Test]
        public void DefineNamespace_Array()
        {
            var abc = new AbcFile();
            DefineNamespace_Array(abc, AbcConstKind.PackageNamespace);
            DefineNamespace_Array(abc, AbcConstKind.PublicNamespace);
            DefineNamespace_Array(abc, AbcConstKind.ProtectedNamespace);
            DefineNamespace_Array(abc, AbcConstKind.StaticProtectedNamespace);
            DefineNamespace_Array(abc, AbcConstKind.PrivateNamespace);
            DefineNamespace_Array(abc, AbcConstKind.InternalNamespace);
            DefineNamespace_Array(abc, AbcConstKind.ExplicitNamespace);
        }

        [Test]
        public void DefineNamespace_Global()
        {
            var abc = new AbcFile();
            var c1 = abc.DefinePackage("");
            Assert.AreEqual(1, c1.Index, "#1");
        }
        #endregion
    }
}