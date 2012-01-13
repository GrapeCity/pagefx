using System;
using System.Collections;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    internal static class AbcHelper
    {
        public static bool IsNamespace(AbcConstKind k)
        {
            switch (k)
            {
                case AbcConstKind.ExplicitNamespace:
                case AbcConstKind.InternalNamespace:
                case AbcConstKind.PackageNamespace:
                case AbcConstKind.PrivateNamespace:
                case AbcConstKind.ProtectedNamespace:
                case AbcConstKind.PublicNamespace:
                case AbcConstKind.StaticProtectedNamespace:
                    return true;
            }
            return false;
        }

        public static double ToDouble(float value)
        {
            // Standard explicit conversion of float to doube with "(double)" doesn't work.
            // Read http://www.yoda.arachsys.com/csharp/floatingpoint.html

            if (float.IsNaN(value))
                return double.NaN;
            if (float.IsPositiveInfinity(value))
                return double.PositiveInfinity;
            if (float.IsNegativeInfinity(value))
                return double.NegativeInfinity;

            // decimal max/min
            if (value > -79228162514264337593543950335.0f && value < 79228162514264337593543950335.0f)
                return Convert.ToDouble(Convert.ToDecimal(value));

            return value;
        }

        public static bool AreEquals(AbcConst<string> s1, string s2)
        {
            if (s1 == null)
                return string.IsNullOrEmpty(s2);
            if (string.IsNullOrEmpty(s1.Value))
                return string.IsNullOrEmpty(s2);
            return s1.Value == s2;
        }

        public static int FindEntry(AbcFile file, int offset, IList list, int begin, int end, bool cpool, bool writeN)
        {
            if (offset >= begin && offset < end)
            {
                int n = list.Count;
                if (cpool && n <= 1)
                    return -1;
                int off = begin;
                if (writeN)
                {
                    off += SwfWriter.SizeOfUIntEncoded((uint)n);
                }
                for (int i = cpool ? 1 : 0; i < n; ++i)
                {
                    var atom = list[i] as ISwfAtom;
                    if (atom != null)
                    {
                        int size = AbcIO.SizeOf(file, atom);
                        if (offset >= off && offset < off + size)
                        {
                            return i;
                        }
                        off += size;
                    }
                }
            }
            return -1;
        }

        public static string FormatOffset(AbcFile file, int offset, IList list, int begin, int end, string name, bool cpool, bool writeN)
        {
            int i = FindEntry(file, offset, list, begin, end, cpool, writeN);
            if (i >= 0)
                return string.Format("{0} entry {1}", name, i);
            return null;
        }
    }
}