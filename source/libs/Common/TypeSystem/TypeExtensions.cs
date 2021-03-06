using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public static class TypeExtensions
    {
		public static IType ResolveValueType(this IType type)
		{
			if (type == null) return null;
			if (type.IsEnum)
			{
				return type.Fields.Where(field => field.IsSpecialName).Select(x => x.Type).FirstOrDefault();
			}
			return null;
		}

		/// <summary>
		/// Indicates whether the type is generic parameter.
		/// </summary>
		public static bool IsGenericParameter(this IType type)
		{
			return type != null && type.TypeKind == TypeKind.GenericParameter;
		}

		/// <summary>
		/// Indicates whether the type has generic parameters.
		/// </summary>
		/// <param name="type">The type to check.</param>
		public static bool IsGeneric(this IType type)
		{
			return type != null && type.GenericParameters.Count > 0;
		}

		/// <summary>
		/// Indicates whether the type is generic instance of generic type.
		/// </summary>
		/// <param name="type">The type to check.</param>
		public static bool IsGenericInstance(this IType type)
		{
			return type != null && type.GenericArguments.Count > 0;
		}

		/// <summary>
		/// Determines wheher the type is generated by compiler.
		/// </summary>
		public static bool IsCompilerGenerated(this IType type)
		{
			return type.HasAttribute("System.Runtime.CompilerServices.CompilerGeneratedAttribute");
		}

		/// <summary>
		/// Gets c# keyword used for this type
		/// </summary>
		public static string CSharpKeyword(this IType type)
		{
			var systemType = type.SystemType();
			return systemType != null ? systemType.CSharpKeyword : "";
		}

		public static IType GetArrayType(this IType elementType)
		{
			return elementType != null ? elementType.Assembly.TypeFactory.MakeArray(elementType) : null;
		}

		public static int ArrayRank(this IType type)
		{
			return type.IsArray ? type.ArrayDimensions.Count + 1 : -1;
		}

		public static IType GetPointerType(this IType type)
		{
			return type != null ? type.Assembly.TypeFactory.MakePointerType(type) : null;
		}

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
	        return type.FindMember(inherit,
	                               t =>
		                               {
			                               var set = t.Properties.Find(name) ?? Enumerable.Empty<IProperty>();
			                               return set.FirstOrDefault(p => !p.IsIndexer());
		                               });
        }

        public static IType GetCommonBaseType(this IType a, IType b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (ReferenceEquals(a, b)) return a;

            //A : B
            if (a.IsSubclassOf(b))
                return b;

            //B : A
            if (b.IsSubclassOf(a))
                return a;

            //A : C
            //B : C
            var c = a.BaseType;
            while (c != null)
            {
                if (b.IsSubclassOf(c))
                    return c;
                c = c.BaseType;
            }

            return a.SystemType(SystemTypeCode.Object);
        }

        private static IType GetCommonInterface(this IType a, IType b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (ReferenceEquals(a, b)) return a;

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

            return a.SystemType(SystemTypeCode.Object);
        }

        public static IType GetCommonAncestor(this IType a, IType b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (ReferenceEquals(a, b)) return a;

            if (a.IsEnum)
            {
                if (b.IsEnum)
                    return a.ValueType.GetCommonAncestor(b.ValueType);
                if (b.IsSystemType())
                    return a.ValueType.GetCommonAncestor(b);
            }

            if (b.IsEnum)
            {
                if (a.IsSystemType())
                    return a.GetCommonAncestor(b.ValueType);
            }

        	if (a.IsSystemType())
            {
            	if (!b.IsSystemType()) return a;

                var cd = NumericType.GetCommonType(a, b);
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
            return type.IsGenericInstance() && type.Type.FullName == fullname;
        }

        public static bool IsIEnumerableInstance(this IType type)
        {
            return type.IsGenericInstance(CLRNames.Types.IEnumerableT);
        }

        public static bool IsIEnumerableInstance(this IType type, IType arg)
        {
	        return type.IsIEnumerableInstance()
	               && ReferenceEquals(type.GenericArguments[0], arg);
        }

	    public static bool IsIEnumerableChar(this IType type)
        {
            return type.IsIEnumerableInstance(type.SystemType(SystemTypeCode.Char));
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
            if (!type.IsGenericInstance()) return false;
            string name = type.Type.FullName;
            return Array.IndexOf(GenericArrayInterfaces, name) >= 0;
        }

        public static bool IsGenericArrayInterface(this IType type, IType arg)
        {
	        return type.IsGenericArrayInterface()
	               && ReferenceEquals(type.GenericArguments[0], arg);
        }

	    public static IType GetTypeArgument(this IType type, int arg)
        {
            if (!type.IsGenericInstance())
			{
				throw new InvalidOperationException(
					String.Format("given type {0} is not generic instance", type.FullName));
			}
        	return type.GenericArguments[arg];
        }

		public static bool IsImplicitCast(this IType source, IType target)
        {
            if (ReferenceEquals(source, target)) return true;
            if (source == null) return true;

            if (target.Is(SystemTypeCode.Object))
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
	                        if (source.ArrayRank() != target.ArrayRank())
                                return false;
                            if (source.ElementType.IsImplicitCast(target.ElementType))
                                return true;
                        }
                    }
                    break;

                case TypeKind.Primitive:
                    {
                        var t = target.SystemType();
                        if (t == null) return false;
                        var s = source.SystemType();
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
	            if (iface.IsGenericArrayInterface())
                {
                    var arg = iface.GetTypeArgument(0);
                    return type.IsImplicitCast(arg);
                }
            }

            if (type.IsInterface)
                return type.Interfaces.Any(i => ReferenceEquals(i, iface));

            while (type != null)
            {
                if (type.Is(SystemTypeCode.Object)) break;
                if (type.Interfaces.Any(i => ReferenceEquals(i, iface)))
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
				if (type == typeof(object)) break;
				if (type.GetInterfaces().Any(i => i.FullName == iface.FullName))
					return true;
				type = type.BaseType;
			}

			return false;
		}

        public static bool IsSubclassOf(this IType type, IType baseType)
        {
            if (type == null || baseType == null) return false;

            type = type.BaseType;
            while (type != null)
            {
                if (ReferenceEquals(type, baseType)) return true;
                type = type.BaseType;
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

		public static IEnumerable<IType> BaseTypes(this IType type)
        {
            for (var t = type.BaseType; t != null; t = t.BaseType)
                yield return t;
        }

        public static bool IsDecimalOrInt64(IType type)
        {
            if (type == null) return false;
            var st = type.SystemType();
            if (st == null) return false;
            return st.IsDecimalOrInt64;
        }

        #region Conversion Utils
        private static readonly bool[,] ImplicitNumericConversions;
        private static readonly bool[,] ExplicitNumericConversions;

        static TypeExtensions()
        {
            const int count = (int)SystemTypeCode.Max;
	        ImplicitNumericConversions = new bool[count,count];
	        ExplicitNumericConversions = new bool[count,count];

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

	    public static bool HasImplicitConversion(IType from, IType to)
        {
            if (ReferenceEquals(from, to)) return true;
            var sf = from.SystemType();
            if (sf == null) return false;
            var st = to.SystemType();
            if (st == null) return false;
            return ImplicitNumericConversions[(int)sf.Code, (int)st.Code];
        }

        #endregion

        public static bool IsThisMethod(IMethod thisMethod, IMethod method)
        {
			if (thisMethod.IsStatic || method.IsStatic) return false;
            
            var type = method.DeclaringType;
            var t = thisMethod.DeclaringType;
            while (t != null)
            {
                if (ReferenceEquals(type, t)) return true;
                t = t.BaseType;
            }

            return false;
        }

        public static bool IsBaseMethod(this IMethod thisMethod, IMethod baseMethod)
        {
            if (thisMethod.IsStatic || baseMethod.IsStatic) return false;
            
            var type = baseMethod.DeclaringType;
            var t = thisMethod.DeclaringType.BaseType;
            while (t != null)
            {
                if (ReferenceEquals(type, t)) return true;
                t = t.BaseType;
            }

            return false;
        }

        public static bool IsBoxableType(this IType type)
        {
            //NOTE: enums is also boxable types
            if (type == null) return false;
            if (type.IsEnum) return true;
            var st = type.SystemType();
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
	        while (type != null && type.TypeKind == TypeKind.Reference)
		        type = type.ElementType;
	        return type;
        }

        public static bool IsVoid(this IMethod method)
        {
	        return method.Type == null
	               || method.IsConstructor
	               || method.Type.Is(SystemTypeCode.Void);
        }

	    public static bool IsEqual(this IType a, IType b)
    	{
    		if (a == null) return b == null;
    		if (b == null) return false;
    		if (ReferenceEquals(a, b)) return true;
		    return a.TypeKind == b.TypeKind
		           && a.DeclaringType.IsEqual(b.DeclaringType)
		           && a.DeclaringMethod == b.DeclaringMethod
		           && a.FullName == b.FullName;
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
    		if (!type.IsCompilerGenerated()) return false;
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

		public static bool IsArrayInitializer(this IField field)
		{
			if (field == null || field.Type == null) return false;
			if (field.Type.IsArrayInitializer()) return true;
			return field.Type.TypeKind == TypeKind.Primitive
			       && field.DeclaringType.IsPrivateImplementationDetails()
			       && field.Value != null;
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
    		var ctors = type.Methods.Where(m => !m.IsStatic && m.IsConstructor);
    		return ctors.Take(2).Count() == 1;
    	}

    	public static IType GetElementType(this IType type)
    	{
    		return type != null ? type.ElementType : null;
    	}

	    public static IMethod FindMethod(this IType type, string name, int argc, Func<IParameterCollection, bool> args)
    	{
    		return type.Methods.Find(name, m =>
    		                               	{
    		                               		var p = m.Parameters;
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

	    public static void AppendSigName(this StringBuilder sb, IEnumerable<IType> types, Runtime runtime)
	    {
		    sb.Append(types, x => x.GetSigName(runtime), "_");
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
			    if (Signature.Equals(m, method, compareReturnTypes))
				    yield return m;
		    }
	    }

	    public static IMethod FindSameMethod(this IType type, IMethod method, bool compareReturnTypes)
	    {
		    IMethod result = null;
		    int curSpec = 0;
		    foreach (var candidate in type.GetSameMethods(method, compareReturnTypes))
		    {
			    if (!candidate.SignatureChanged())
				    return candidate;

				int spec = candidate.GetSpecificity();
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

	    public static IMethod FindImplementation(this IType type, IMethod ifaceMethod, bool inherit, bool onlyImpls)
	    {
		    if (ifaceMethod.IsGenericInstance)
			    ifaceMethod = ifaceMethod.InstanceOf;

		    while (type != null)
		    {
			    var impl = FindImpl(type, ifaceMethod, onlyImpls);
			    if (impl != null)
			    {
				    return impl;
			    }
			    if (!inherit) break;
			    type = type.BaseType;
		    }

		    return null;
	    }

	    private static IMethod FindImpl(this IType type, IMethod ifaceMethod, bool onlyImpls)
	    {
		    var impl = (from candidate in type.Methods
		                where candidate.Implements != null &&
		                      candidate.Implements.Any(x => x == ifaceMethod || x.ProxyOf == ifaceMethod)
		                select candidate).FirstOrDefault();
		    if (impl != null)
		    {
			    return impl;
		    }

			if (onlyImpls)
			{
				return null;
			}

		    var methods = type.Methods.Find(ifaceMethod.Name);
		    return (from candidate in methods
		            where Signature.Equals(candidate, ifaceMethod, true)
		            select candidate).FirstOrDefault();
	    }

	    public static IMethod GetStaticCtor(this IType type)
	    {
		    if (type == null) return null;
		    if (type.IsArray)
			    type = type.SystemType(SystemTypeCode.Array);
		    return type.Methods.StaticConstructor;
	    }

	    public static bool IsInt64(this IType type)
	    {
		    var t = type.SystemType();
		    return t != null && (t.Code == SystemTypeCode.Int64 || t.Code == SystemTypeCode.UInt64);
	    }

	    public static bool IsInt64Based(this IType type)
	    {
		    if (type.IsEnum)
			    type = type.ValueType;
		    return type.IsInt64();
	    }

	    public static int GetCorlibKind(this IType type)
	    {
		    const int typeCodeOffset = 100;
		    //set type kind
		    switch (type.TypeKind)
		    {
			    case TypeKind.Class:
				    {
					    if (type.Is(SystemTypeCode.String))
						    return typeCodeOffset + (int)TypeCode.String;
					    return 0;
				    }

			    case TypeKind.Delegate:
				    return 0;

			    case TypeKind.Struct:
				    {
					    if (type.FullName == "System.DBNull")
						    return typeCodeOffset + (int)TypeCode.DBNull;

					    if (type.Is(SystemTypeCode.DateTime))
						    return typeCodeOffset + (int)TypeCode.DateTime;

					    return (int)type.TypeKind;
				    }

			    case TypeKind.Array:
			    case TypeKind.Interface:
			    case TypeKind.Pointer:
			    case TypeKind.Reference:
			    case TypeKind.Enum:
				    return (int)type.TypeKind;

			    case TypeKind.GenericParameter:
				    throw new NotSupportedException();

			    case TypeKind.Primitive:
				    {
					    var st = type.SystemType();
					    if (st == null)
						    throw new InvalidOperationException();
					    return typeCodeOffset + (int)st.TypeCode;
				    }
		    }
		    return -1;
	    }

	    public static IField[] GetEnumFields(this IType type)
	    {
		    if (type == null)
			    throw new ArgumentNullException("type");
		    if (type.TypeKind != TypeKind.Enum)
			    throw new ArgumentException("type is not enum");
		    return type.Fields.Where(f => f.IsStatic).ToArray();
	    }
    }
}