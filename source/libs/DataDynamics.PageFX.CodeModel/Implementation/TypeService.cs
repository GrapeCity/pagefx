using System;
using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX.CodeModel
{
    public static class TypeService
    {
        public static T FindMember<T>(IType type, bool inherit, Converter<IType, T> finder)
            where T : class 
        {
            while (type != null)
            {
                var member = finder(type);
                if (member != null) return member;
                if (!inherit) break;
                type = type.BaseType;
            }
            return null;
        }

        public static IField FindField(IType type, string name, bool inherit)
        {
            return FindMember(type, inherit, t => t.Fields[name]);
        }

        public static IProperty FindProperty(IType type, string name, bool inherit)
        {
            return FindMember(type, inherit,
                              t => Algorithms.Find(t.Properties, p => !p.IsIndexer && p.Name == name));
        }

        public static IType GetCommonBaseType(IType A, IType B)
        {
            if (A == null) return B;
            if (B == null) return A;
            if (A == B) return A;

            //A : B
            if (IsSubclassOf(A, B))
                return B;

            //B : A
            if (IsSubclassOf(B, A))
                return A;

            //A : C
            //B : C
            var C = A.BaseType;
            while (C != null)
            {
                if (IsSubclassOf(B, C))
                    return C;
                C = C.BaseType;
            }

            return SystemTypes.Object;
        }

        private static IType GetCommonInterface(IType A, IType B)
        {
            if (A == null) return B;
            if (B == null) return A;
            if (A == B) return A;

            //A : B
            if (Implements(A, B))
                return B;

            //B : A
            if (Implements(B, A))
                return A;

            //A : C
            //B : C

            foreach (var C in A.Interfaces)
            {
                if (Implements(B, C))
                    return C;
            }

            return SystemTypes.Object;
        }

        public static IType GetCommonAncestor(IType A, IType B)
        {
            if (A == null) return B;
            if (B == null) return A;
            if (A == B) return A;

            if (A.IsEnum)
            {
                if (B.IsEnum)
                    return GetCommonAncestor(A.ValueType, B.ValueType);
                if (B.SystemType != null)
                    return GetCommonAncestor(A.ValueType, B);
            }

            if (B.IsEnum)
            {
                if (A.SystemType != null)
                    return GetCommonAncestor(A, B.ValueType);
            }

            var a = A.SystemType;
            if (a != null)
            {
                var b = B.SystemType;
                if (b == null) return A;

                var cd = SystemTypes.GetCommonDenominator(A, B);
                if (cd != null) return cd;
            }

            if (A.IsInterface)
                return B.IsInterface ? GetCommonInterface(A, B) : A;

            return B.IsInterface ? B : GetCommonBaseType(A, B);
        }

        private static readonly string[] GenericArrayInterfaces =
        {
            CLRNames.Types.IEnumerableT,
            CLRNames.Types.ICollectionT,
            CLRNames.Types.IListT
        };

        public static bool IsGenericInstance(IType type, string fullname)
        {
            if (type == null) return false;
            var gi = type as IGenericInstance;
            if (gi == null) return false;
            return gi.Type.FullName == fullname;
        }

        public static bool IsIEnumerableInstance(IType type)
        {
            return IsGenericInstance(type, CLRNames.Types.IEnumerableT);
        }

        public static bool IsIEnumerableInstance(IType type, IType arg)
        {
            if (!IsIEnumerableInstance(type)) return false;
            var gi = (IGenericInstance)type;
            return gi.GenericArguments[0] == arg;
        }

        public static bool IsIEnumerableChar(IType type)
        {
            return IsIEnumerableInstance(type, SystemTypes.Char);
        }

        public static bool IsIEnumeratorInstance(IType type)
        {
            return IsGenericInstance(type, CLRNames.Types.IEnumeratorT);
        }

        public static bool IsIEnumerator(IType type)
        {
            if (type == null) return false;
            return type.FullName == CLRNames.Types.IEnumerator;
        }

        public static bool IsNullableInstance(IType type)
        {
            return IsGenericInstance(type, CLRNames.Types.NullableT);
        }

        public static bool IsGenericArrayInterface(IType type)
        {
            var gi = type as IGenericInstance;
            if (gi == null) return false;
            string name = gi.Type.FullName;
            return Array.IndexOf(GenericArrayInterfaces, name) >= 0;
        }

        public static bool IsGenericArrayInterface(IType type, IType arg)
        {
            if (!IsGenericArrayInterface(type)) return false;
            return ((IGenericInstance)type).GenericArguments[0] == arg;
        }

        public static IType GetTypeArg(IType type, int arg)
        {
            var gi = type as IGenericInstance;
            if (gi == null)
                throw new InvalidOperationException(
                    string.Format("given type {0} is not generic instance", type.FullName));
            return gi.GenericArguments[arg];
        }

        public static bool IsImplicitCast(IType source, IType target)
        {
            if (source == target) return true;
            if (source == null) return true;

            if (target == SystemTypes.Object)
                return true;

            switch (target.TypeKind)
            {
                case TypeKind.Interface:
                    return !Implements(source, target);

                case TypeKind.Class:
                case TypeKind.Delegate:
                    return !IsSubclassOf(source, target);

                case TypeKind.Array:
                    {
                        if (source.IsArray)
                        {
                            var from = (IArrayType)source;
                            var to = (IArrayType)target;
                            if (from.Rank != to.Rank)
                                return false;
                            if (IsImplicitCast(from.ElementType, to.ElementType))
                                return true;
                        }
                    }
                    break;

                case TypeKind.Primitive:
                    {
                        var t = target.SystemType;
                        if (t == null) return false;
                        var s = source.SystemType;
                        if (s == null) return false;
                        switch (t.Code)
                        {
                            case SystemTypeCode.Boolean:
                                return false;
                            case SystemTypeCode.Char:
                            case SystemTypeCode.UInt16:
                                switch (s.Code)
                                {
                                    case SystemTypeCode.Int8:
                                    case SystemTypeCode.UInt8:
                                    case SystemTypeCode.Char:
                                    case SystemTypeCode.UInt16:
                                        return true;
                                }
                                break;

                            case SystemTypeCode.Int16:
                                switch (s.Code)
                                {
                                    case SystemTypeCode.Int8:
                                    case SystemTypeCode.UInt8:
                                        return true;
                                }
                                break;

                            case SystemTypeCode.Int32:
                            case SystemTypeCode.UInt32:
                                switch (s.Code)
                                {
                                        case SystemTypeCode.Int8:
                                        case SystemTypeCode.UInt8:
                                        case SystemTypeCode.Int16:
                                        case SystemTypeCode.UInt16:
                                        case SystemTypeCode.Char:
                                            return true;
                                }
                                break;

                            case SystemTypeCode.Double:
                            case SystemTypeCode.Single:
                                switch (s.Code)
                                {
                                    case SystemTypeCode.Int8:
                                    case SystemTypeCode.UInt8:
                                    case SystemTypeCode.Int16:
                                    case SystemTypeCode.UInt16:
                                    case SystemTypeCode.Int32:
                                    case SystemTypeCode.UInt32:
                                    case SystemTypeCode.Char:
                                    case SystemTypeCode.Single:
                                        return true;
                                }
                                break;
                        }
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Determines whether given type implements given interface.
        /// </summary>
        /// <param name="type">given type to inspect</param>
        /// <param name="iface">given interface to take into account</param>
        /// <returns></returns>
        public static bool Implements(IType type, IType iface)
        {
            if (iface == null) return false;

            if (type.IsArray)
            {
                var elemType = (IArrayType)type;
                if (IsGenericArrayInterface(iface))
                {
                    var arg = GetTypeArg(iface, 0);
                    return IsImplicitCast(elemType, arg);
                }
            }

            if (type.IsInterface)
                return Algorithms.Contains(type.Interfaces, i => i == iface);

            while (type != null)
            {
                if (type == SystemTypes.Object) break;
                if (Algorithms.Contains(type.Interfaces, i => i == iface))
                    return true;
                type = type.BaseType;
            }
            return false;
        }

        public static bool IsSubclassOf(IType type, IType baseType)
        {
            if (type == null) return false;
            if (baseType == null) return false;
            var bt = type.BaseType;
            while (bt != null)
            {
                if (bt == baseType) return true;
                bt = bt.BaseType;
            }
            return false;
        }

        public static bool IsSubclassOf(IType type, string baseType)
        {
            if (type == null) return false;
            if (baseType == null) return false;
            var bt = type.BaseType;
            while (bt != null)
            {
                if (bt.FullName == baseType) return true;
                bt = bt.BaseType;
            }
            return false;
        }

        public static IEnumerable<IType> GetBaseTypes(IType type)
        {
            for (var bt = type.BaseType; bt != null; bt = bt.BaseType)
                yield return bt;
        }

        public static bool IsDecimalOrInt64(IType type)
        {
            if (type == null) return false;
            var st = type.SystemType;
            if (st == null) return false;
            return st.IsDecimalOrInt64;
        }

        #region Conversion Utils
        private static readonly bool[,] ImplicitNumericConversions;
        private static readonly bool[,] ExplicitNumericConversions;

        static TypeService()
        {
            const int N = (int)SystemTypeCode.Max;
            ImplicitNumericConversions = new bool[N,N];
            ExplicitNumericConversions = new bool[N,N];

            string text = ResourceHelper.GetText(typeof(TypeService), "implicit.txt");
            LoadConversions(text, ImplicitNumericConversions);

            text = ResourceHelper.GetText(typeof(TypeService), "explicit.txt");
            LoadConversions(text, ExplicitNumericConversions);
        }

        private static void LoadConversions(string text, bool[,] table)
        {
            using (var reader = new StringReader(text))
            {
                char[] sep = {' ', '\t', ','};
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0) continue;

                    //comment?
                    if (line.StartsWith("#")) continue;

                    var arr = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    int n = arr.Length;
                    var from = SystemType.ParseCode(arr[0]);
                    if (from == SystemTypeCode.None)
                        throw new InvalidOperationException();
                    for (int i = 1; i < n; ++i)
                    {
                        var to = SystemType.ParseCode(arr[i]);
                        if (to != SystemTypeCode.None)
                            table[(int)from, (int)to] = true;
                    }
                }
            }
        }

        private static bool HasCustomImplicitOperator(IType from, IType to)
        {
            return false;
        }

        public static bool HasImplicitConversion(IType from, IType to)
        {
            if (from == to) return true;
            var sf = from.SystemType;
            if (sf == null)
                return HasCustomImplicitOperator(from, to);
            var st = to.SystemType;
            if (st == null)
                return HasCustomImplicitOperator(from, to);
            if (ImplicitNumericConversions[(int)sf.Code, (int)st.Code])
                return true;
            return HasCustomImplicitOperator(from, to);
        }
        #endregion

        public static bool IsThisMethod(IMethod thisMethod, IMethod method)
        {
            if (thisMethod.IsStatic) return false;
            if (method.IsStatic) return false;
            var type = method.DeclaringType;
            var t = thisMethod.DeclaringType;
            while (t != null)
            {
                if (type == t) return true;
                t = t.BaseType;
            }
            return false;
        }

        public static bool IsBaseMethod(IMethod thisMethod, IMethod method)
        {
            if (thisMethod.IsStatic) return false;
            if (method.IsStatic) return false;
            var type = method.DeclaringType;
            var t = thisMethod.DeclaringType.BaseType;
            while (t != null)
            {
                if (type == t) return true;
                t = t.BaseType;
            }
            return false;
        }

        public static bool IsBoxableType(IType type)
        {
            //NOTE: enums is also boxable types
            if (type == null) return false;
            if (type.IsEnum) return true;
            var st = type.SystemType;
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Boolean:
                    case SystemTypeCode.Int8:
                    case SystemTypeCode.Int16:
                    case SystemTypeCode.Int32:
                    case SystemTypeCode.UInt8:
                    case SystemTypeCode.UInt16:
                    case SystemTypeCode.UInt32:
                    case SystemTypeCode.Single:
                    case SystemTypeCode.Double:
                    case SystemTypeCode.Char:
                    case SystemTypeCode.UIntPtr:
                    case SystemTypeCode.IntPtr:
                        return true;
                }
            }
            return false;
        }

        public static IType UnwrapRef(IType type)
        {
            while (type != null)
            {
                if (type.TypeKind != TypeKind.Reference) break;
                type = ((ICompoundType)type).ElementType;
            }
            return type;
        }

        public static void CompleteProperty(IProperty prop)
        {
            if (prop.Type == null)
            {
                var m = prop.Getter;
                if (m != null)
                {
                    prop.Type = m.Type;
                    foreach (var p in m.Parameters)
                    {
                        prop.Parameters.Add(p);
                    }
                    return;
                }
                m = prop.Setter;
                if (m != null)
                {
                    int n = m.Parameters.Count;
                    prop.Type = m.Parameters[n - 1].Type;
                    for (int i = 0; i < n - 1; ++i)
                    {
                        prop.Parameters.Add(m.Parameters[i]);
                    }
                }
            }
        }

        public static bool IsVoid(IMethod method)
        {
            if (method.Type == null) return true;
            if (method.IsConstructor) return true;
            return method.Type == SystemTypes.Void;
        }
    }
}