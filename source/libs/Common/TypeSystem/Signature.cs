using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.TypeSystem
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

        public static bool TypeEquals(IType type1, IType type2)
        {
	        if (ReferenceEquals(type1, type2))
		        return true;

            if (type1.TypeKind != type2.TypeKind)
                return false;

            switch (type1.TypeKind)
            {
	            case TypeKind.Array:
		            return Equals(type1.ArrayDimensions, type2.ArrayDimensions)
		                   && TypeEquals(type1.ElementType, type2.ElementType);

	            case TypeKind.Pointer:
	            case TypeKind.Reference:
		            return TypeEquals(type1.ElementType, type2.ElementType);

	            case TypeKind.GenericParameter:
		            {
			            if (type1.DeclaringMethod != null)
			            {
				            if (type2.DeclaringMethod == null)
					            return false;
			            }
			            else if (type2.DeclaringMethod != null)
				            return false;
						return type1.GetGenericParameterInfo().Position == type2.GetGenericParameterInfo().Position;
		            }

	            case TypeKind.Enum:
		            return ReferenceEquals(type1, type2);
            }

	        if (type1.IsGenericInstance())
            {
	            if (!type2.IsGenericInstance()) return false;
				if (!ReferenceEquals(type1.Type, type2.Type)) return false;
                int n = type1.GenericArguments.Count;
                if (type2.GenericArguments.Count != n) return false;
                for (int i = 0; i < n; ++i)
                {
                    var t1 = type1.GenericArguments[i];
                    var t2 = type2.GenericArguments[i];
                    if (!TypeEquals(t1, t2))
                        return false;
                }
                return true;
            }

            return ReferenceEquals(type1, type2);
        }

        public static bool Equals(IMethod m1, IMethod m2, bool checkReturnTypes)
        {
            if (m1.Parameters.Count != m2.Parameters.Count) return false;
            if (m1.Name != m2.Name) return false;
            if (checkReturnTypes && !TypeEquals(m1.Type, m2.Type)) return false;
            return Equals(m1.Parameters, m2.Parameters);
        }

		public static bool Equals(IEnumerable<IType> sig1, IEnumerable<IType> sig2)
		{
			return sig1.EqualsTo(sig2, (x, y) => TypeEquals(x, y));
		}

        public static bool Equals(IParameterCollection list1, IParameterCollection list2)
        {
	        return list1.EqualsTo(list2, (x, y) => TypeEquals(x.Type, y.Type));
        }

        public static bool Equals(IParameterCollection list, params IType[] types)
        {
            int n = list.Count;
            if (types == null || types.Length == 0)
                return n == 0;
            
            if (n != types.Length)
                return false;

	        return list.Select(x => x.Type).EqualsTo(types, (x, y) => TypeEquals(x, y));
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