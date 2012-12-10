using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	//TODO: introduce IAssembly.SystemTypes property to have a single instance
	public partial class SystemTypes
	{
		private readonly IAssembly _assembly;
		private readonly IType[] _types;

		public SystemTypes(IAssembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			_assembly = assembly.Corlib();
			_types = new IType[SysTypes.Length];
		}

		private IType this[SystemTypeCode typeCode]
		{
			get
			{
				int index = (int)typeCode;
				return _types[index] ?? (_types[index] = _assembly.FindSystemType(typeCode));
			}
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

		public IType this[TypeCode code]
		{
			get
			{
				if (code == TypeCode.Empty || code == TypeCode.DBNull)
					return null;
				return this[code.ToSystemTypeCode()];
			}
		}

		public IType ResolveType(object value)
		{
			return value == null ? null : this[System.Type.GetTypeCode(value.GetType())];
		}
	}
}