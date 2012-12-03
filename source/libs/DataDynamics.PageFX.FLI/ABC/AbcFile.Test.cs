using System;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    partial class AbcFile
    {
        internal void Test()
        {
            var b = ToByteArray();
            var abc = new AbcFile(b);
            var b2 = abc.ToByteArray();
            if (!AreEquals(b, b2))
                throw new InvalidOperationException();
        }

        static bool AreEquals(byte[] a, byte[] b)
        {
            if (a == null) return b == null;
            if (b == null) return false;
            int n = a.Length;
            if (n != b.Length) return false;
            for (int i = 0; i < n; ++i)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
    }
}