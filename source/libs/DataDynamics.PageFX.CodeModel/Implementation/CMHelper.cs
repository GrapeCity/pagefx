using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public static class CMHelper
    {
        public static readonly ICodeNode[] EmptyCodeNodes = new ICodeNode[0];

        public static readonly IType[] EmptyTypes = new IType[0];

        public static bool AreEquals(IType a, IType b)
        {
            if (a == null) return b == null;
            if (b == null) return false;
            if (ReferenceEquals(a, b)) return true;
            if (a.TypeKind != b.TypeKind) return false;
            if (!AreEquals(a.DeclaringType, b.DeclaringType)) return false;
            if (a.DeclaringMethod != b.DeclaringMethod) return false;
            if (a.FullName != b.FullName) return false;
            return true;
        }

        public static int GetHashCode(IType type)
        {
            int h = 0;
            if (type.DeclaringType != null)
                h ^= GetHashCode(type.DeclaringType);
            h ^= type.FullName.GetHashCode();
            return h;
        }

        public static IEnumerable<ICodeNode> Convert<T>(IEnumerable<T> original) where T : ICodeNode
        {
            return Algorithms.Convert<T, ICodeNode>(original);
        }

        public static IEnumerable<ICodeNode> Enumerate<T1>(T1 obj) where T1 : ICodeNode
        {
            return new ICodeNode[] {obj};
        }

        public static IEnumerable<ICodeNode> Enumerate<T1, T2>(T1 obj1, T2 obj2) 
            where T1 : ICodeNode
            where T2 : ICodeNode
        {
            return new ICodeNode[] { obj1, obj2 };
        }

        public static IEnumerable<ICodeNode> Enumerate<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3)
            where T1 : ICodeNode
            where T2 : ICodeNode
            where T3 : ICodeNode
        {
            return new ICodeNode[] { obj1, obj2, obj3 };
        }

        public static IEnumerable<ICodeNode> Enumerate<T1, T2, T3, T4>(T1 obj1, T2 obj2, T3 obj3, T4 obj4)
            where T1 : ICodeNode
            where T2 : ICodeNode
            where T3 : ICodeNode
            where T4 : ICodeNode
        {
            return new ICodeNode[] { obj1, obj2, obj3, obj4 };
        }

        public static IEnumerable<ICodeNode> Enumerate<T1, T2, T3, T4, T5>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5)
            where T1 : ICodeNode
            where T2 : ICodeNode
            where T3 : ICodeNode
            where T4 : ICodeNode
            where T5 : ICodeNode
        {
            return new ICodeNode[] { obj1, obj2, obj3, obj4, obj5 };
        }
    }
}