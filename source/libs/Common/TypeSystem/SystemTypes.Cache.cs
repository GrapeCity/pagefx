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

		private IType Get(SystemTypeCode typeCode)
		{
			int index = (int)typeCode;
			return _types[index] ?? (_types[index] = _assembly.FindSystemType(typeCode));
		}

		public IType Boolean
		{
			get { return Get(SystemTypeCode.Boolean); }
		}

		public IType Int8
		{
			get { return Get(SystemTypeCode.Int8); }
		}

		public IType UInt8
		{
			get { return Get(SystemTypeCode.UInt8); }
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
			get { return Get(SystemTypeCode.Char); }
		}

		public IType Int16
		{
			get { return Get(SystemTypeCode.Int16); }
		}

		public IType UInt16
		{
			get { return Get(SystemTypeCode.UInt16); }
		}

		public IType Int32
		{
			get { return Get(SystemTypeCode.Int32); }
		}

		public IType UInt32
		{
			get { return Get(SystemTypeCode.UInt32); }
		}

		public IType Int64
		{
			get { return Get(SystemTypeCode.Int64); }
		}

		public IType UInt64
		{
			get { return Get(SystemTypeCode.UInt64); }
		}

		public IType Single
		{
			get { return Get(SystemTypeCode.Single); }
		}

		public IType Double
		{
			get { return Get(SystemTypeCode.Double); }
		}

		public IType Decimal
		{
			get { return Get(SystemTypeCode.Decimal); }
		}

		public IType String
		{
			get { return Get(SystemTypeCode.String); }
		}

		public IType Object
		{
			get { return Get(SystemTypeCode.Object); }
		}

		public IType Array
		{
			get { return Get(SystemTypeCode.Array); }
		}

		public IType DateTime
		{
			get { return Get(SystemTypeCode.DateTime); }
		}

		public IType Type
		{
			get { return Get(SystemTypeCode.Type); }
		}

		public IType Delegate
		{
			get { return Get(SystemTypeCode.Delegate); }
		}

		public IType MulticastDelegate
		{
			get { return Get(SystemTypeCode.MulticastDelegate); }
		}

		public IType Exception
		{
			get { return Get(SystemTypeCode.Exception); }
		}

		public IType IntPtr
		{
			get { return Get(SystemTypeCode.IntPtr); }
		}
	}
}