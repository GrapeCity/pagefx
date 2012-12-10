using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed partial class SystemTypes
    {
		private static readonly SystemType[] SysTypes;
		private static readonly Dictionary<string, SystemType> Lookup;
		private static readonly Dictionary<string, SystemType> LookupByFullName;

        static SystemTypes()
        {
            const BindingFlags bf = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField;
            var fields = typeof(SystemTypeCode).GetFields(bf);
            SysTypes = new SystemType[fields.Length];
            foreach (var field in fields)
            {
                var code = (SystemTypeCode)field.GetValue(null);
                int index = (int)code;
                var attr = field.GetAttribute<SystemTypeNameAttribute>(false);
                var systemType = new SystemType(code, attr != null ? attr.Name : code.ToString());
                var cs_attr = field.GetAttribute<CSharpAttribute>(false);
                if (cs_attr != null)
                    systemType.CSharpKeyword = cs_attr.Value;
                SysTypes[index] = systemType;
            }
	        Lookup = SysTypes.ToDictionary(x => x.Name, x => x);
	        LookupByFullName = SysTypes.ToDictionary(x => x.FullName, x => x);
        }

        public static SystemType Find(string name)
		{
			SystemType type;
			return Lookup.TryGetValue(name, out type) ? type : null;
		}

		public static SystemType FindByFullName(string fullName)
		{
			SystemType type;
			return LookupByFullName.TryGetValue(fullName, out type) ? type : null;
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
                    return assembly.FindSystemType(SystemTypeCode.Object);
                case TypeCode.Boolean:
                    return assembly.FindSystemType(SystemTypeCode.Boolean);
                case TypeCode.Char:
                    return assembly.FindSystemType(SystemTypeCode.Char);
                case TypeCode.SByte:
                    return assembly.FindSystemType(SystemTypeCode.Int8);
                case TypeCode.Byte:
                    return assembly.FindSystemType(SystemTypeCode.UInt8);
                case TypeCode.Int16:
                    return assembly.FindSystemType(SystemTypeCode.Int16);
                case TypeCode.UInt16:
                    return assembly.FindSystemType(SystemTypeCode.UInt16);
                case TypeCode.Int32:
                    return assembly.FindSystemType(SystemTypeCode.Int32);
                case TypeCode.UInt32:
                    return assembly.FindSystemType(SystemTypeCode.UInt32);
                case TypeCode.Int64:
                    return assembly.FindSystemType(SystemTypeCode.Int64);
                case TypeCode.UInt64:
                    return assembly.FindSystemType(SystemTypeCode.UInt64);
                case TypeCode.Single:
                    return assembly.FindSystemType(SystemTypeCode.Single);
                case TypeCode.Double:
                    return assembly.FindSystemType(SystemTypeCode.Double);
                case TypeCode.Decimal:
                    return assembly.FindSystemType(SystemTypeCode.Decimal);
                case TypeCode.DateTime:
                    return assembly.FindSystemType(SystemTypeCode.DateTime);
                case TypeCode.String:
                    return assembly.FindSystemType(SystemTypeCode.String);
                default:
                    return null;
            }
        }

        public static IType GetType(IAssembly assembly, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var tc = System.Type.GetTypeCode(type);
            if (tc == TypeCode.Object)
            {
	            return assembly.GetReferences(false).Select(x => x.FindType(type.FullName)).FirstOrDefault(x => x != null);
            }
            return GetType(assembly, tc);
        }

        public static IType GetType(IAssembly assembly, object obj)
        {
            if (obj == null) return assembly.FindSystemType(SystemTypeCode.Object);
            return GetType(assembly, obj.GetType());
        }

		public static string GetFullName(SystemTypeCode typeCode)
		{
			var sysType = Types[(int)typeCode];
			return sysType.FullName;
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