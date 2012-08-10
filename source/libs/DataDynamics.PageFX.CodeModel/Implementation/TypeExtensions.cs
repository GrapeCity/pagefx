using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
    public static class TypeExtensions
    {
        public static T FindMember<T>(this IType type, bool inherit, Converter<IType, T> finder)
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

        public static IField FindField(this IType type, string name, bool inherit)
        {
            return type.FindMember(inherit, t => t.Fields[name]);
        }

        public static IProperty FindProperty(this IType type, string name, bool inherit)
        {
            return type.FindMember(inherit, t => t.Properties.FirstOrDefault(p => !p.IsIndexer && p.Name == name));
        }

        public static IType GetCommonBaseType(this IType a, IType b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (a == b) return a;

            //A : B
            if (a.IsSubclassOf(b))
                return b;

            //B : A
            if (b.IsSubclassOf(a))
                return a;

            //A : C
            //B : C
            var C = a.BaseType;
            while (C != null)
            {
                if (b.IsSubclassOf(C))
                    return C;
                C = C.BaseType;
            }

            return SystemTypes.Object;
        }

        private static IType GetCommonInterface(this IType a, IType b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (a == b) return a;

            //a : b
            if (a.Implements(b))
                return b;

            //b : a
            if (b.Implements(a))
                return a;

            //a : c
            //b : c

            foreach (var c in a.Interfaces)
            {
                if (b.Implements(c))
                    return c;
            }

            return SystemTypes.Object;
        }

        public static IType GetCommonAncestor(this IType a, IType b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (a == b) return a;

            if (a.IsEnum)
            {
                if (b.IsEnum)
                    return a.ValueType.GetCommonAncestor(b.ValueType);
                if (b.SystemType != null)
                    return a.ValueType.GetCommonAncestor(b);
            }

            if (b.IsEnum)
            {
                if (a.SystemType != null)
                    return a.GetCommonAncestor(b.ValueType);
            }

        	if (a.SystemType != null)
            {
            	if (b.SystemType == null) return a;

                var cd = SystemTypes.GetCommonDenominator(a, b);
                if (cd != null) return cd;
            }

            if (a.IsInterface)
                return b.IsInterface ? a.GetCommonInterface(b) : a;

            return b.IsInterface ? b : a.GetCommonBaseType(b);
        }

        private static readonly string[] GenericArrayInterfaces =
        {
            CLRNames.Types.IEnumerableT,
            CLRNames.Types.ICollectionT,
            CLRNames.Types.IListT
        };

        public static bool IsGenericInstance(this IType type, string fullname)
        {
            if (type == null) return false;
            var gi = type as IGenericInstance;
            if (gi == null) return false;
            return gi.Type.FullName == fullname;
        }

        public static bool IsIEnumerableInstance(this IType type)
        {
            return type.IsGenericInstance(CLRNames.Types.IEnumerableT);
        }

        public static bool IsIEnumerableInstance(this IType type, IType arg)
        {
            if (!type.IsIEnumerableInstance()) return false;
            var gi = (IGenericInstance)type;
            return gi.GenericArguments[0] == arg;
        }

        public static bool IsIEnumerableChar(this IType type)
        {
            return type.IsIEnumerableInstance(SystemTypes.Char);
        }

        public static bool IsIEnumeratorInstance(this IType type)
        {
            return type.IsGenericInstance(CLRNames.Types.IEnumeratorT);
        }

        public static bool IsIEnumerator(this IType type)
        {
            if (type == null) return false;
            return type.FullName == CLRNames.Types.IEnumerator;
        }

        public static bool IsNullableInstance(this IType type)
        {
            return type.IsGenericInstance(CLRNames.Types.NullableT);
        }

        public static bool IsGenericArrayInterface(this IType type)
        {
            var gi = type as IGenericInstance;
            if (gi == null) return false;
            string name = gi.Type.FullName;
            return Array.IndexOf(GenericArrayInterfaces, name) >= 0;
        }

        public static bool IsGenericArrayInterface(this IType type, IType arg)
        {
            if (!type.IsGenericArrayInterface()) return false;
            return ((IGenericInstance)type).GenericArguments[0] == arg;
        }

        public static IType GetTypeArgument(this IType type, int arg)
        {
            var genericInstance = type as IGenericInstance;
			if (genericInstance == null)
			{
				throw new InvalidOperationException(
					String.Format("given type {0} is not generic instance", type.FullName));
			}
        	return genericInstance.GenericArguments[arg];
        }

        public static bool IsImplicitCast(this IType source, IType target)
        {
            if (source == target) return true;
            if (source == null) return true;

            if (target == SystemTypes.Object)
                return true;

            switch (target.TypeKind)
            {
                case TypeKind.Interface:
                    return !source.Implements(target);

                case TypeKind.Class:
                case TypeKind.Delegate:
                    return !source.IsSubclassOf(target);

                case TypeKind.Array:
                    {
                        if (source.IsArray)
                        {
                            var from = (IArrayType)source;
                            var to = (IArrayType)target;
                            if (@from.Rank != to.Rank)
                                return false;
                            if (@from.ElementType.IsImplicitCast(to.ElementType))
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
        public static bool Implements(this IType type, IType iface)
        {
            if (iface == null) return false;

            if (type.IsArray)
            {
                var elemType = (IArrayType)type;
                if (iface.IsGenericArrayInterface())
                {
                    var arg = iface.GetTypeArgument(0);
                    return elemType.IsImplicitCast(arg);
                }
            }

            if (type.IsInterface)
                return type.Interfaces.Any(i => i == iface);

            while (type != null)
            {
                if (type == SystemTypes.Object) break;
                if (type.Interfaces.Any(i => i == iface))
                    return true;
                type = type.BaseType;
            }

            return false;
        }

		/// <summary>
		/// Determines whether given type implements given interface.
		/// </summary>
		/// <param name="type">given type to inspect</param>
		/// <param name="iface">given interface to take into account</param>
		/// <returns></returns>
		public static bool Implements(this Type type, IType iface)
		{
			if (iface == null) return false;

			//TODO: complete
			//if (type.IsArray)
			//{
			//    var elemType = type.GetElementType();
			//    if (iface.IsGenericArrayInterface())
			//    {
			//        var arg = iface.GetTypeArgument(0);
			//        return elemType.IsImplicitCast(arg);
			//    }
			//}

			if (type.IsInterface)
			{
				return type.GetInterfaces().Any(i => i.FullName == iface.FullName);
			}

			while (type != null)
			{
				if (type == SystemTypes.Object) break;
				if (type.GetInterfaces().Any(i => i.FullName == iface.FullName))
					return true;
				type = type.BaseType;
			}

			return false;
		}

        public static bool IsSubclassOf(this IType type, IType baseType)
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

        public static bool IsSubclassOf(this IType type, string baseType)
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

		public static bool IsInstanceOf(this Type type, IType itype)
		{
			if (type == null) return false;
			if (itype == null) return false;

			if (itype.IsInterface)
			{
				return type.Implements(itype);
			}

			while (type != null)
			{
				if (type.FullName == itype.FullName) return true;
				type = type.BaseType;
			}

			return false;
		}

		public static bool IsSubclassOf(this Type type, IType baseType)
		{
			if (type == null) return false;
			if (baseType == null) return false;
			var bt = type.BaseType;
			while (bt != null)
			{
				if (bt.FullName == baseType.FullName) return true;
				bt = bt.BaseType;
			}
			return false;
		}

		public static IEnumerable<IType> GetBaseTypes(this IType type)
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

        static TypeExtensions()
        {
            const int N = (int)SystemTypeCode.Max;
            ImplicitNumericConversions = new bool[N,N];
            ExplicitNumericConversions = new bool[N,N];

            string text = typeof(TypeExtensions).GetTextResource("implicit.txt");
            LoadConversions(text, ImplicitNumericConversions);

            text = typeof(TypeExtensions).GetTextResource("explicit.txt");
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
                    if (@from == SystemTypeCode.None)
                        throw new InvalidOperationException();
                    for (int i = 1; i < n; ++i)
                    {
                        var to = SystemType.ParseCode(arr[i]);
                        if (to != SystemTypeCode.None)
                            table[(int)@from, (int)to] = true;
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
            if (@from == to) return true;
            var sf = @from.SystemType;
            if (sf == null)
                return HasCustomImplicitOperator(@from, to);
            var st = to.SystemType;
            if (st == null)
                return HasCustomImplicitOperator(@from, to);
            if (ImplicitNumericConversions[(int)sf.Code, (int)st.Code])
                return true;
            return HasCustomImplicitOperator(@from, to);
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

        public static bool IsBoxableType(this IType type)
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

        public static IType UnwrapRef(this IType type)
        {
            while (type != null)
            {
                if (type.TypeKind != TypeKind.Reference) break;
                type = ((ICompoundType)type).ElementType;
            }
            return type;
        }

        public static void ResolveTypeAndParameters(this IProperty prop)
        {
        	if (prop.Type != null) return;

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

		public static void ResolveType(this IEvent e)
		{
			if (e.Type != null) return;

			foreach (var method in new[]{e.Adder, e.Remover})
			{
				if (method == null) return;
				e.Type = method.Parameters[0].Type;
				break;
			}
		}

        public static bool IsVoid(this IMethod method)
        {
            if (method.Type == null) return true;
            if (method.IsConstructor) return true;
            return method.Type == SystemTypes.Void;
        }

    	public static bool IsEqual(this IType a, IType b)
    	{
    		if (a == null) return b == null;
    		if (b == null) return false;
    		if (ReferenceEquals(a, b)) return true;
    		if (a.TypeKind != b.TypeKind) return false;
    		if (!a.DeclaringType.IsEqual(b.DeclaringType)) return false;
    		if (a.DeclaringMethod != b.DeclaringMethod) return false;
    		if (a.FullName != b.FullName) return false;
    		return true;
    	}

    	public static int EvalHashCode(this IType type)
    	{
    		int h = 0;
    		if (type.DeclaringType != null)
    			h ^= type.DeclaringType.EvalHashCode();
    		h ^= type.FullName.GetHashCode();
    		return h;
    	}

    	public static bool IsAttributeType(this IType type)
    	{
    		if (type == null) return false;
    		const string systemAttribute = "System.Attribute";
    		return type.FullName == systemAttribute || type.IsSubclassOf(systemAttribute);
    	}

    	public static bool IsExceptionType(this IType type)
    	{
    		if (type == null) return false;
    		const string systemException = "System.Exception";
    		return type.FullName == systemException || type.IsSubclassOf(systemException);
    	}

    	public static IMethod FindConstructor(this IType type, Func<IMethod, bool> ctorPredicate)
    	{
    		if (type == null) return null;
    		return type.Methods.Constructors.FirstOrDefault(m => !m.IsStatic && ctorPredicate(m));
    	}

    	public static IMethod FindConstructor(this IType type, int argCount)
    	{
    		return FindConstructor(type, ctor => ctor.Parameters.Count == argCount);
    	}

    	public static IMethod FindParameterlessConstructor(this IType type)
    	{
    		return FindConstructor(type, 0);
    	}

    	public static bool IsModuleType(this IType type)
    	{
    		if (type == null) return false;
    		if (!type.IsClass) return false;
    		if (type.DeclaringType != null) return false;
    		return type.FullName == "<Module>";
    	}

		//e.g. class <PrivateImplementationDetails>{C05318BA-D3C5-45BA-8FEC-725F72EE7B81}
    	public static bool IsPrivateImplementationDetails(this IType type)
    	{
    		if (type == null) return false;
    		if (!type.IsCompilerGenerated) return false;
    		if (!type.IsClass) return false;
    		if (type.DeclaringType != null) return false;
    		string name = type.FullName;
    		int n = name.Length;
    		if (n == 0) return false;
    		return name.StartsWith("<PrivateImplementationDetails>{") && name[n - 1] == '}';
    	}

    	/// <summary>
    	/// Determines whether given type is array initializer struct.
    	/// </summary>
    	/// <param name="type"></param>
    	/// <returns></returns>
    	public static bool IsArrayInitializer(this IType type)
    	{
    		if (type == null) return false;
    		if (type.TypeKind != TypeKind.Struct) return false;
    		if (type.Layout == null) return false;
    		if (type.DeclaringType.IsPrivateImplementationDetails()) return true;
    		return false;
    	}

    	/// <summary>
    	/// Determines whether the given type has only one instance constructor
    	/// </summary>
    	/// <param name="type"></param>
    	/// <returns></returns>
    	public static bool HasSingleConstructor(this IType type)
    	{
    		if (type == null) return false;
    		if (type.IsInterface) return false;
    		int n = 0;
    		foreach (var m in type.Methods.Where(m => !m.IsStatic && m.IsConstructor))
    		{
    			if (n >= 1) return false;
    			++n;
    		}
    		return true;
    	}

    	public static IType GetElementType(this IType type)
    	{
    		var compoundType = type as ICompoundType;
    		if (compoundType == null) throw new ArgumentException("type");
    		return compoundType.ElementType;
    	}

    	public static IMethod FindMethod(this IType type, string name, IType arg1)
    	{
    		return type.Methods.Find(name, arg1);
    	}

		public static IMethod FindMethod(this IType type, string name, int argc)
    	{
    		return type.Methods.Find(name, argc);
    	}

		public static IMethod FindMethod(this IType type, string name, int argc, Func<IParameterCollection, bool> args)
    	{
    		return type.Methods.Find(name, m =>
    		                               	{
    		                               		var p= m.Parameters;
    		                               		if (p.Count != argc) return false;
    		                               		return args(p);
    		                               	});
    	}

		public static IMethod FindMethodHierarchically(this IType type, string name, Func<IMethod, bool> predicate, Func<IType, bool> hierarchyFilter)
		{
			while (type != null)
			{
				if (hierarchyFilter != null && hierarchyFilter(type)) break;
				var method = type.Methods.Find(name, predicate);
				if (method != null) return method;
				type = type.BaseType;
			}
			return null;
		}

	    public static string GetSigName(this IType type)
	    {
		    return type == null ? "" : type.SigName;
	    }

	    public static void AppendSigName(this StringBuilder sb, IEnumerable<IType> types)
	    {
		    sb.Append(types, x => x.GetSigName(), "_");
		}

	    public static IEnumerable<IMethod> GetSameMethods(this IType type, IMethod method, bool compareReturnTypes)
	    {
		    bool isGeneric = method.IsGeneric;
		    var set = type.Methods.Find(method.Name);
		    foreach (var m in set)
		    {
			    if (isGeneric)
			    {
				    if (!m.IsGeneric) continue;
				    if (m.GenericParameters.Count != method.GenericParameters.Count)
					    continue;
			    }
			    else
			    {
				    if (m.IsGeneric) continue;
			    }
			    if (Signature.Equals(m, method, compareReturnTypes, false))
				    yield return m;
		    }
	    }

	    public static IMethod FindSameMethod(this IType type, IMethod method, bool compareReturnTypes)
	    {
		    IMethod result = null;
		    int curSpec = 0;
		    foreach (var candidate in type.GetSameMethods(method, compareReturnTypes))
		    {
			    if (!candidate.SignatureChanged)
				    return candidate;

			    int spec = Method.GetSpecificity(candidate);
			    if (result == null || spec > curSpec)
			    {
				    result = candidate;
				    curSpec = spec;
			    }
		    }
		    return result;
	    }

	    public static IMethod FindOverrideMethod(this IType type, IMethod method)
	    {
		    if (method.IsGenericInstance)
		    {
			    var gm = method.InstanceOf;
			    var m = type.FindSameMethod(gm, false);
			    if (m == null) return null;
			    m = GenericType.CreateMethodInstance(type, m, method.GenericArguments);
			    if (m == null)
				    throw new InvalidOperationException();
			    return m;
		    }
		    return type.FindSameMethod(method, false);
	    }

	    public static IMethod FindImplementation(this IType type, IMethod method, bool inherited)
	    {
		    if (method.IsGenericInstance)
			    method = method.InstanceOf;

		    while (type != null)
		    {
			    var impl = (from candidate in type.Methods
			                where candidate.ImplementedMethods != null &&
			                      candidate.ImplementedMethods.Any(x => x == method || x.ProxyOf == method)
			                select candidate).FirstOrDefault();
			    if (impl != null)
			    {
				    return impl;
			    }
			    if (!inherited) break;
			    type = type.BaseType;
		    }

		    return null;
	    }

	    public static IMethod FindImplementation(this IType type, IMethod method)
	    {
		    return type.FindImplementation(method, true);
	    }

	    public static IMethod GetStaticCtor(this IType type)
	    {
		    if (type == null) return null;
		    if (type.IsArray)
			    type = SystemTypes.Array;
		    return type.Methods.StaticConstructor;
	    }
    }
}