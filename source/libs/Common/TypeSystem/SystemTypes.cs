using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class SystemTypes
    {
		private static readonly SystemType[] Types;
		private static readonly Dictionary<string, SystemType> Lookup;
		private static readonly Dictionary<string, SystemType> LookupByFullName;

		private readonly IAssembly _assembly;
		private readonly IType[] _types;

		internal SystemTypes(IAssembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			_assembly = assembly.Corlib();
			_types = new IType[Types.Length];
		}

        static SystemTypes()
        {
            const BindingFlags bf = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField;
            var fields = typeof(SystemTypeCode).GetFields(bf);
            Types = new SystemType[fields.Length];
            foreach (var field in fields)
            {
                var code = (SystemTypeCode)field.GetValue(null);
                int index = (int)code;
                var attr = field.GetAttribute<SystemTypeNameAttribute>(false);
                var systemType = new SystemType(code, attr != null ? attr.Name : code.ToString());
                var cs_attr = field.GetAttribute<CSharpAttribute>(false);
                if (cs_attr != null)
                    systemType.CSharpKeyword = cs_attr.Value;
                Types[index] = systemType;
            }
	        Lookup = Types.ToDictionary(x => x.Name, x => x);
	        LookupByFullName = Types.ToDictionary(x => x.FullName, x => x);
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

		public IType this[SystemTypeCode typeCode]
		{
			get
			{
				int index = (int)typeCode;
				return _types[index] ?? (_types[index] = Resolve(typeCode));
			}
		}

		public IType this[TypeCode code]
		{
			get
			{
				if (code == TypeCode.Empty || code == TypeCode.DBNull)
					return null;
				return this[code.ToSystemTypeCode()];
			}
		}

		private IType Resolve(SystemTypeCode typeCode)
		{
			var fullName = GetFullName(typeCode);
			return _assembly.FindType(fullName);
		}

		private static string GetFullName(SystemTypeCode typeCode)
		{
			var sysType = Types[(int)typeCode];
			return sysType.FullName;
		}

		public IType Boolean
		{
			get { return this[SystemTypeCode.Boolean]; }
		}

		public IType Int8
		{
			get { return this[SystemTypeCode.Int8]; }
		}

		public IType UInt8
		{
			get { return this[SystemTypeCode.UInt8]; }
		}

		public IType Byte
		{
			get { return UInt8; }
		}

		public IType SByte
		{
			get { return Int8; }
		}

		public IType Char
		{
			get { return this[SystemTypeCode.Char]; }
		}

		public IType Int16
		{
			get { return this[SystemTypeCode.Int16]; }
		}

		public IType UInt16
		{
			get { return this[SystemTypeCode.UInt16]; }
		}

		public IType Int32
		{
			get { return this[SystemTypeCode.Int32]; }
		}

		public IType UInt32
		{
			get { return this[SystemTypeCode.UInt32]; }
		}

		public IType Int64
		{
			get { return this[SystemTypeCode.Int64]; }
		}

		public IType UInt64
		{
			get { return this[SystemTypeCode.UInt64]; }
		}

		public IType Single
		{
			get { return this[SystemTypeCode.Single]; }
		}

		public IType Double
		{
			get { return this[SystemTypeCode.Double]; }
		}

		public IType Decimal
		{
			get { return this[SystemTypeCode.Decimal]; }
		}

		public IType String
		{
			get { return this[SystemTypeCode.String]; }
		}

		public IType Object
		{
			get { return this[SystemTypeCode.Object]; }
		}

		public IType Array
		{
			get { return this[SystemTypeCode.Array]; }
		}

		public IType DateTime
		{
			get { return this[SystemTypeCode.DateTime]; }
		}

		public IType Type
		{
			get { return this[SystemTypeCode.Type]; }
		}

		public IType Delegate
		{
			get { return this[SystemTypeCode.Delegate]; }
		}

		public IType MulticastDelegate
		{
			get { return this[SystemTypeCode.MulticastDelegate]; }
		}

		public IType Exception
		{
			get { return this[SystemTypeCode.Exception]; }
		}

		public IType IntPtr
		{
			get { return this[SystemTypeCode.IntPtr]; }
		}

		public IType Void
		{
			get { return this[SystemTypeCode.Void]; }
		}

		public IType ResolveType(object value)
		{
			return value == null ? null : this[System.Type.GetTypeCode(value.GetType())];
		}
    }
}