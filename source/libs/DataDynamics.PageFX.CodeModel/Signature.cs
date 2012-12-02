using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel
{
    public static class Signature
    {
        public static bool CheckSignature(IType type, IMethod method, string name, IType[] sig)
        {
            int n = sig.Length;
            if (method.Parameters.Count + 1 != n)
                return false;

            if (method.Name != name)
                return false;

            if (!TypeEquals(method.Type, sig[0]))
                return false;

            for (int i = 1; i < n; ++i)
            {
                var ptype = method.Parameters[i - 1].Type;
                if (!TypeEquals(ptype, sig[i]))
                    return false;
            }

            return true;
        }

        public static bool CheckSignature(IProperty prop, string name, IType[] sig)
        {
            int n = sig.Length;
            if (prop.Parameters.Count + 1 != n)
                return false;

            if (prop.Name != name)
                return false;

            if (!TypeEquals(prop.Type, sig[0]))
                return false;

            for (int i = 1; i < n; ++i)
            {
                var ptype = prop.Parameters[i - 1].Type;
                if (!TypeEquals(ptype, sig[i]))
                    return false;
            }

            return true;
        }

        public static bool Equals(IType[] sig1, IType[] sig2)
        {
            int n = sig1.Length;
            if (n != sig2.Length) return false;
            for (int i = 0; i < n; ++i)
            {
                var t1 = sig1[i];
                var t2 = sig2[i];
                if (!TypeEquals(t1, t2))
                    return false;
            }
            return true;
        }

        public static bool Equals(IReadOnlyList<IType> sig1, IReadOnlyList<IType> sig2)
        {
            int n = sig1.Count;
            if (n != sig2.Count) return false;
            for (int i = 0; i < n; ++i)
            {
                var t1 = sig1[i];
                var t2 = sig2[i];
                if (!TypeEquals(t1, t2))
                    return false;
            }
            return true;
        }

        public static bool TypeEquals(IType type1, IType type2)
        {
            if (type1.TypeKind != type2.TypeKind)
                return false;

            switch (type1.TypeKind)
            {
                case TypeKind.Array:
                    {
                        var arr1 = (IArrayType)type1;
                        var arr2 = (IArrayType)type2;
                        if (!Equals(arr1.Dimensions, arr2.Dimensions))
                            return false;
                        return TypeEquals(arr1.ElementType, arr2.ElementType);
                    }

                case TypeKind.Pointer:
                case TypeKind.Reference:
                    {
                        var ct1 = (ICompoundType)type1;
                        var ct2 = (ICompoundType)type2;
                        return TypeEquals(ct1.ElementType, ct2.ElementType);
                    }

                case TypeKind.GenericParameter:
                    {
                        var gp1 = (IGenericParameter)type1;
                        var gp2 = (IGenericParameter)type2;
                        if (gp1.DeclaringMethod != null)
                        {
                            if (gp2.DeclaringMethod == null)
                                return false;
                        }
                        else if (gp2.DeclaringMethod != null)
                            return false;
                        //return type1 == type2;
                        return gp1.Position == gp2.Position;
                    }

                case TypeKind.Enum:
                    return ReferenceEquals(type1, type2);
            }

            var gi1 = type1 as IGenericInstance;
            if (gi1 != null)
            {
                var gi2 = type2 as IGenericInstance;
                if (gi2 == null) return false;
                if (gi1.Type != gi2.Type) return false;
                int n = gi1.GenericArguments.Count;
                if (gi2.GenericArguments.Count != n) return false;
                for (int i = 0; i < n; ++i)
                {
                    var t1 = gi1.GenericArguments[i];
                    var t2 = gi2.GenericArguments[i];
                    if (!TypeEquals(t1, t2))
                        return false;
                }
                return true;
            }

            return ReferenceEquals(type1, type2);
        }

        public static bool Equals(IMethod m1, IMethod m2, bool checkReturnTypes, bool checkNames)
        {
            if (m1.Parameters.Count != m2.Parameters.Count) return false;
            if (checkNames && m1.Name != m2.Name) return false;
            if (checkReturnTypes && !TypeEquals(m1.Type, m2.Type)) return false;
            return Equals(m1.Parameters, m2.Parameters);
        }

        public static bool Equals(IMethod m1, IMethod m2, bool checkReturnTypes)
        {
            return Equals(m1, m2, checkReturnTypes, true);
        }

        public static bool Equals(IParameterCollection list1, IParameterCollection list2)
        {
            int n = list1.Count;
            if (list2.Count != n) return false;
            for (int i = 0; i < n; ++i)
            {
                var t1 = list1[i].Type;
                var t2 = list2[i].Type;
                if (!TypeEquals(t1, t2))
                    return false;
            }
            return true;
        }

        public static bool Equals(IParameterCollection list, params IType[] types)
        {
            int n = list.Count;
            if (types == null || types.Length == 0)
                return n == 0;
            
            if (n != types.Length)
                return false;

            for (int i = 0; i < n; ++i)
            {
                var t1 = list[i].Type;
                var t2 = types[i];
                if (!TypeEquals(t1, t2))
                    return false;
            }

            return true;
        }

        public static bool Equals(IMethod method, params IType[] types)
        {
            return Equals(method.Parameters, types);
        }

        public static bool SetterEquals(IParameterCollection list, params IType[] types)
        {
            int n = list.Count - 1;
            if (types == null || types.Length == 0)
                return n == 0;

            if (n != types.Length)
                return false;

            for (int i = 0; i < n; ++i)
            {
                var t1 = list[i].Type;
                var t2 = types[i];
                if (!TypeEquals(t1, t2))
                    return false;
            }

            return true;
        }

        public static bool SetterEquals(IMethod method, params IType[] types)
        {
            return SetterEquals(method.Parameters, types);
        }
    }
}