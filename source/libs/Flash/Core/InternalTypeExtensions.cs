using System;
using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;

namespace DataDynamics.PageFX.Flash.Core
{
    /// <summary>
    /// Contains various type utils.
    /// </summary>
    internal static class InternalTypeExtensions
    {
        /// <summary>
        /// Returns true if given type should not be compiled.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CanExclude(this IType type)
        {
            if (type.IsModuleType())
                return true;

            if (type.IsSpecialName || type.IsRuntimeSpecialName)
                return true;

            if (type.IsArrayInitializer())
                return true;

            if (type.IsPrivateImplementationDetails())
            {
            	return type.Fields.All(field => field.IsArrayInitializer());
            }

        	return false;
        }

    	/// <summary>
        /// Finds type in given assembly.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static IType FindType(IAssembly asm, string fullname)
        {
            var t = asm.FindType(fullname);
            if (t == null)
                throw new InvalidOperationException(
                    String.Format("Unable to find type {0}.", fullname));
            return t;
        }

        /// <summary>
        /// Determines whether we should use native AVM Object type when given type is used for member type
        /// </summary>
        /// <param name="type">given type to inspect</param>
        /// <returns></returns>
        public static bool UseNativeObject(this IType type)
        {
	        if (type == null) return false;

	        return type.TypeKind == TypeKind.Reference
	               || type.Is(SystemTypeCode.Object)
	               || type.IsStringInterface()
	               || type.IsGenericArrayInterface();
        }

	    public static bool Is(this IType type, AvmTypeCode typeCode)
        {
            if (type == null) return false;
            var instance = type.AbcInstance();
            if (instance == null) return false;
            if (!instance.IsNative) return false;
            return instance.FullName == typeCode.FullName();
        }

	    public static AbcMultiname GetMultiname(this IType type)
        {
            if (type == null) throw new ArgumentNullException("type");

	        object tag = type.Data;
	        if (tag == null) return null;

	        var data = tag as ITypeData;
			if (data != null)
			{
				return data.Name;
			}

	        return tag as AbcMultiname;
        }

        public static bool IsStringInterface(this IType type)
        {
	        var stringType = type.SystemType(SystemTypeCode.String);
	        return stringType.Implements(type);
        }

	    public static bool IsInternalType(this IType type)
        {
            if (String.IsNullOrEmpty(type.Namespace) && type.Name == "avm")
                return true;
            return false;
        }

        public static bool HasGlobalFunctions(this IType type)
        {
            return type.HasAttribute(Attrs.GlobalFunctions);
        }

        #region name utils

	    public static string GetNamespaceForMembers(string ns, string name)
        {
            if (String.IsNullOrEmpty(ns))
                return name;
            return ns + ":" + name;
        }

        public static string GetNamespaceForMembers(IType type)
        {
            string name = type.NestedName;
            string ns = type.GetTypeNamespace("");
            return GetNamespaceForMembers(ns, name);
        }

        public static string GetNamespaceForMembers(AbcInstance instance)
        {
            return GetNamespaceForMembers(instance.NamespaceString, instance.NameString);
        }
        #endregion

        #region ctor utils

    	public static IMethod FindParameterlessConstructor(this AbcInstance instance)
        {
            var type = instance.Type;
            return type == null ? null : type.FindParameterlessConstructor();
        }

	    public static IMethod FindInitializer(AbcInstance instance)
        {
            if (instance == null) return null;
            var type = instance.Type;
            if (type == null) return null;
            return FindInitializer(type);
        }

        public static IMethod FindInitializer(IType type)
        {
            if (type == null) return null;
            IMethod ctor = null;
            int n = 0;
            foreach (var m in type.Methods.Constructors)
            {
                if (m.IsStatic) continue;
                if (n >= 1) return null;
                if (m.Parameters.Count > 0 && !false)
                    return null;
                ctor = m;
                ++n;
            }
            return ctor;
        }

	    #endregion

        #region numeric types

	    public static bool IsDecimalOrInt64(this IType type)
        {
            return type.Is(SystemTypeCode.Decimal) || type.IsInt64();
        }

        public static bool IsDecimalOrInt64(IType type1, IType type2)
        {
            return IsDecimalOrInt64(type1) || IsDecimalOrInt64(type2);
        }

        public static IType SelectDecimalOrInt64(IType type1, IType type2)
        {
            if (type1.Is(SystemTypeCode.Decimal))
                return type1;
            if (type2.Is(SystemTypeCode.Decimal))
                return type2;
            if (type1.IsInt64())
                return type1;
            if (type2.IsInt64())
                return type2;
            return null;
        }

        public static NumberType GetNumberType(this IType type)
        {
            var st = type.SystemType();
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Int8:
                    case SystemTypeCode.Int16:
                    case SystemTypeCode.Int32:
                        return NumberType.Int32;

                    case SystemTypeCode.UInt8:
                    case SystemTypeCode.UInt16:
                    case SystemTypeCode.UInt32:
                        return NumberType.UInt32;

                    case SystemTypeCode.Int64:
                        return NumberType.Int64;

                    case SystemTypeCode.UInt64:
                        return NumberType.UInt64;
                }
            }
            return NumberType.Float;
        }
        #endregion

        public static bool IsBoxableOrInt64Based(this IType type)
        {
            return type.IsBoxableType() || type.IsInt64Based();
        }

	    public static bool SupportsCopyMethods(this IType type)
        {
            if (type == null) return false;
            if (type.Is(SystemTypeCode.ValueType)) return false;
            if (type.Is(SystemTypeCode.Enum)) return false;
            if (type.TypeKind == TypeKind.Struct) return true;
            return IsDecimalOrInt64(type);
        }

        public static bool MustInitValueTypeFields(IType type)
        {
            if (type == null) return false;
            if (type.IsBoxableType()) return false;
            if (type.IsEnum) return false;
            if (type.IsInterface) return false;
            if (type.IsArray) return false;
            var st = type.SystemType();
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Array:
                    case SystemTypeCode.DateTime:
                    case SystemTypeCode.Delegate:
                        return false;

                    case SystemTypeCode.Int64:
                    case SystemTypeCode.UInt64:
                        return false;
                }
            }
            return true;
        }

        public static bool IsInitArray(IType type)
        {
            if (type == null) return false;
            switch (type.TypeKind)
            {
                case TypeKind.Primitive:
                case TypeKind.Struct:
                case TypeKind.Enum:
                    return true;
            }
            return IsInitRequired(type);
        }

        public static bool IsInitRequired(IType type)
        {
            if (type.TypeKind == TypeKind.Struct)
            {
                return !type.IsArrayInitializer();
            }

            if (type.IsEnum)
                type = type.ValueType;

            var st = type.SystemType();
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Int64:
                    case SystemTypeCode.UInt64:
                    case SystemTypeCode.Decimal:
                    case SystemTypeCode.DateTime:
                        return true;
                }
            }

            return false;
        }

        public static bool IsInitRequiredField(IType type)
        {
            if (IsInitRequired(type)) return true;
			//TODO: write a comment why double and single types requires initialization
            return type.Is(SystemTypeCode.Double) || type.Is(SystemTypeCode.Single);
        }

        public static bool HasInitFields(this IType type, bool isStatic)
        {
            if (!MustInitValueTypeFields(type)) return false;
        	return type.Fields.Any(f => !f.IsConstant && f.IsStatic == isStatic && IsInitRequiredField(f.Type));
        }

    	public static bool IsFrom(IType type, string fullname)
        {
            if (type == null) return false;
            var bt = type.BaseType;
            while (bt != null)
            {
                if (bt.FullName == fullname)
                    return true;
                bt = bt.BaseType;
            }
            return false;
        }

        public static bool IsAvmObject(this IType type)
        {
            if (type == null) return false;
            var instance = type.AbcInstance();
            return instance != null && instance.IsObject;
        }

        public static bool IsFromAvmObject(this IType type)
        {
            if (type == null) return false;
            var bt = type.BaseType;
            while (bt != null)
            {
                if (bt.IsAvmObject())
                    return true;
                bt = bt.BaseType;
            }
            return false;
        }

        public static bool IsRootSprite(this IType type)
        {
            if (type == null) return false;

            if (type.CustomAttributes.Any(attr => attr.TypeName == Attrs.RootSprite))
            {
                return true;
            }

            return false;
        }

        public static bool IsValueType(this IType type)
        {
            if (type == null) return false;
	        switch (type.TypeKind)
	        {
				case TypeKind.Struct:
				case TypeKind.Enum:
				case TypeKind.Primitive:
			        return true;
				default:
			        return false;
	        }
        }

        public static bool MustUseCastToMethod(IType type, bool asop)
        {
            if (type == null) return false;
            if (type.IsArray) return true;

            if (AbcGenConfig.UseCastToValueType && asop && type.IsValueType()) return true;

            if (type.IsStringInterface())
                return true;

            if (type.IsGenericArrayInterface())
                return true;

            if (type.IsNullableInstance())
                return true;
            
            return false;
        }

	    public static bool HasProtectedNamespace(this IType type)
    	{
    		switch (type.TypeKind)
    		{
    			case TypeKind.Interface:
    			case TypeKind.Enum:
    				return false;
    		}
    		return true;
    	}

	    public static string GetStaticCtorName(this IType type)
	    {
		    return CLRNames.StaticConstructor;
	    }

	    public static string GetMethodName(this IType type, string name, int argcount)
	    {
		    var m = type.Methods.Find(name, argcount);
		    if (m == null)
			    throw new ArgumentException(String.Format("Unable to find method {0} in type {1}", name, type.FullName));
		    return m.GetSigName(Runtime.Avm);
	    }

		/// <summary>
		/// Gets generated <see cref="AbcInstance"/> associated with the type.
		/// </summary>
		/// <param name="type">The type to get linked instance of.</param>
		public static AbcInstance AbcInstance(this IType type)
		{
			return type != null ? type.Data as AbcInstance : null;
		}
    }
}