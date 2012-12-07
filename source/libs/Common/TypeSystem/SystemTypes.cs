using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class SystemTypes
    {
		private static readonly SystemType[] SysTypes;
		private static readonly Dictionary<string, SystemType> Lookup;

        static SystemTypes()
        {
            const BindingFlags bf = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField;
            var fields = typeof(SystemTypeCode).GetFields(bf);
            SysTypes = new SystemType[fields.Length];
            foreach (var field in fields)
            {
                var code = (SystemTypeCode)field.GetValue(null);
                int i = (int)code;
                var attr = field.GetAttribute<SystemTypeNameAttribute>(false);
                var st = new SystemType(code, attr != null ? attr.Name : code.ToString());
                var cs_attr = field.GetAttribute<CSharpAttribute>(false);
                if (cs_attr != null)
                    st.CSharpKeyword = cs_attr.Value;
                SysTypes[i] = st;
            }
	        Lookup = SysTypes.ToDictionary(x => x.Name, x => x);
        }

        public static void Reset()
        {
            foreach (var type in Types)
            {
                type.Value = null;
            }
        }

		public static SystemType Find(string name)
		{
			SystemType type;
			return Lookup.TryGetValue(name, out type) ? type : null;
		}

	    public static SystemType[] Types
        {
            get { return SysTypes; }
        }
        
        public static IType GetType(IAssembly assembly, TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Object:
                    return FindSystemType(assembly, SystemTypeCode.Object);
                case TypeCode.Boolean:
                    return FindSystemType(assembly, SystemTypeCode.Boolean);
                case TypeCode.Char:
                    return FindSystemType(assembly, SystemTypeCode.Char);
                case TypeCode.SByte:
                    return FindSystemType(assembly, SystemTypeCode.Int8);
                case TypeCode.Byte:
                    return FindSystemType(assembly, SystemTypeCode.UInt8);
                case TypeCode.Int16:
                    return FindSystemType(assembly, SystemTypeCode.Int16);
                case TypeCode.UInt16:
                    return FindSystemType(assembly, SystemTypeCode.UInt16);
                case TypeCode.Int32:
                    return FindSystemType(assembly, SystemTypeCode.Int32);
                case TypeCode.UInt32:
                    return FindSystemType(assembly, SystemTypeCode.UInt32);
                case TypeCode.Int64:
                    return FindSystemType(assembly, SystemTypeCode.Int64);
                case TypeCode.UInt64:
                    return FindSystemType(assembly, SystemTypeCode.UInt64);
                case TypeCode.Single:
                    return FindSystemType(assembly, SystemTypeCode.Single);
                case TypeCode.Double:
                    return FindSystemType(assembly, SystemTypeCode.Double);
                case TypeCode.Decimal:
                    return FindSystemType(assembly, SystemTypeCode.Decimal);
                case TypeCode.DateTime:
                    return FindSystemType(assembly, SystemTypeCode.DateTime);
                case TypeCode.String:
                    return FindSystemType(assembly, SystemTypeCode.String);
                default:
                    return null;
            }
        }

        public static IType GetType(IAssembly assembly, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var tc = Type.GetTypeCode(type);
            if (tc == TypeCode.Object)
            {
                //TODO:
                foreach (var st in SysTypes)
                {
                    if (st.Value != null && st.Value.FullName == type.FullName)
                    {
                        return st.Value;
                    }
                }
            }
            return GetType(assembly, tc);
        }

        public static IType GetType(IAssembly assembly, object obj)
        {
            if (obj == null) return FindSystemType(assembly, SystemTypeCode.Object);
            return GetType(assembly, obj.GetType());
        }

		public static TypeCode GetTypeCode(this IType type)
        {
	        if (type == null) return TypeCode.Empty;
			var st = type.SystemType();
			return st != null ? st.TypeCode : TypeCode.Empty;
        }

	    public static bool IsSystemType(this IType type)
		{
			return type.SystemType() != null;
		}

		public static SystemType SystemType(this IType type)
		{
			if (type == null) return null;
			return type.Namespace == TypeSystem.SystemType.Namespace ? Find(type.Name) : null;
		}

		public static IType FindSystemType(this IAssembly assembly, SystemTypeCode typeCode)
		{
			return assembly.Corlib().FindType(GetFullName(typeCode));
		}

		private static string GetFullName(SystemTypeCode typeCode)
		{
			var sysType = Types[(int)typeCode];
			return sysType.FullName;
		}

		public static IType FindSystemType(this IType type, SystemTypeCode typeCode)
		{
			return type.Assembly.FindSystemType(typeCode);
		}

		public static bool Is(this IType type, SystemTypeCode typeCode)
		{
			var st = type.SystemType();
			return st != null && st.Code == typeCode;
		}

		public static bool IsNumeric(this IType type)
        {
            if (type == null) return false;
            var st = type.SystemType();
            if (st == null) return false;
            return st.IsNumeric;
        }

        public static bool IsIntegral(this IType type)
        {
            if (type == null) return false;
            var st = type.SystemType();
            return st != null && st.IsIntegral;
        }

        public static bool IsSigned(this IType type)
        {
            if (type == null) return false;
            var st = type.SystemType();
            return st != null && st.IsSigned;
        }

        public static bool IsUnsigned(this IType type)
        {
            if (type == null) return false;
            var st = type.SystemType();
            return st != null && st.IsUnsigned;
        }

        public static IType ToSigned(IType type)
        {
            if (type == null) return null;
            var st = type.SystemType();
	        if (st == null) return type;
			switch (st.Code)
			{
				case SystemTypeCode.UInt8:
					return type.FindSystemType(SystemTypeCode.Int8);
				case SystemTypeCode.UInt16:
					return type.FindSystemType(SystemTypeCode.Int16);
				case SystemTypeCode.UInt32:
					return type.FindSystemType(SystemTypeCode.Int32);
				case SystemTypeCode.UInt64:
					return type.FindSystemType(SystemTypeCode.Int64);
				default:
					return type;
			}
        }

        public static IType ToUnsigned(IType type)
        {
            if (type == null) return null;
            var st = type.SystemType();
	        if (st == null) return type;
			switch (st.Code)
			{
				case SystemTypeCode.Int8:
					return type.FindSystemType(SystemTypeCode.UInt8);
				case SystemTypeCode.Int16:
					return type.FindSystemType(SystemTypeCode.UInt16);
				case SystemTypeCode.Int32:
					return type.FindSystemType(SystemTypeCode.UInt32);
				case SystemTypeCode.Int64:
					return type.FindSystemType(SystemTypeCode.UInt64);
				default:
					return type;
			}
        }

		private static IEnumerable<IType> GetDescendingOrder(IType type)
        {
            yield return type.FindSystemType(SystemTypeCode.Decimal);
			yield return type.FindSystemType(SystemTypeCode.Double);
			yield return type.FindSystemType(SystemTypeCode.Single);
            yield return type.FindSystemType(SystemTypeCode.Int64);
            yield return type.FindSystemType(SystemTypeCode.UInt64);
            yield return type.FindSystemType(SystemTypeCode.Int32);
            yield return type.FindSystemType(SystemTypeCode.UInt32);
            yield return type.FindSystemType(SystemTypeCode.Int16);
            yield return type.FindSystemType(SystemTypeCode.UInt16);
            yield return type.FindSystemType(SystemTypeCode.Int8);
            yield return type.FindSystemType(SystemTypeCode.UInt8);
        }

        public static IType GetCommonDenominator(IType a, IType b)
        {
	        if (a == null && b == null) return null;
        	return GetDescendingOrder(a ?? b).FirstOrDefault(type => ReferenceEquals(a, type) || ReferenceEquals(b, type));
        }

    	public static IType UInt32OR64(IType type)
        {
            if (type == null) return null;
            var st = type.SystemType();
            if (st == null) return null;
            switch (st.Code)
            {
                case SystemTypeCode.Int8:
                case SystemTypeCode.Int16:
                case SystemTypeCode.Int32:
                case SystemTypeCode.UInt8:
                case SystemTypeCode.UInt16:
                case SystemTypeCode.UInt32:
					return type.FindSystemType(SystemTypeCode.UInt32);
                case SystemTypeCode.Int64:
                case SystemTypeCode.UInt64:
                    return type.FindSystemType(SystemTypeCode.UInt64);
            }
            return null;
        }

	    public static IType UInt32OR64(IType a, IType b)
        {
            a = UInt32OR64(a);
            b = UInt32OR64(b);
            if (a == null) return b;
            if (b == null) return null;
            if (a.Is(SystemTypeCode.UInt64) || b.Is(SystemTypeCode.UInt64))
                return a.FindSystemType(SystemTypeCode.UInt64);
			return a.FindSystemType(SystemTypeCode.UInt32);
        }
    }
}