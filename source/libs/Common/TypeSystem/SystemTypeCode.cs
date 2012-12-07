using System;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public enum SystemTypeCode : byte
	{
		None,

		[CSharp("void")]
		Void,

		[CSharp("bool")]
		Boolean,

		[CSharp("sbyte")]
		[SystemTypeName("SByte")]
		Int8,

		[CSharp("byte")]
		[SystemTypeName("Byte")]
		UInt8,

		[CSharp("short")]
		Int16,

		[CSharp("ushort")]
		UInt16,

		[CSharp("int")]
		Int32,

		[CSharp("uint")]
		UInt32,

		[CSharp("long")]
		Int64,

		[CSharp("ulong")]
		UInt64,

		[CSharp("float")]
		Single,

		[CSharp("double")]
		Double,

		[CSharp("decimal")]
		Decimal,

		DateTime,

		UIntPtr,

		IntPtr,

		[CSharp("char")]
		Char,

		[CSharp("string")]
		String,

		[CSharp("object")]
		Object,

		ValueType,

		Enum,

		Array,

		Type,

		TypedReference,

		Delegate,

		MulticastDelegate,

		Exception,

		Attribute,

		Max
	}

	public sealed class SystemTypeNameAttribute : Attribute
	{
		public SystemTypeNameAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}

	public static class SystemTypeCodeExtensions
	{
		public static SystemTypeCode ToSystemTypeCode(this TypeCode type)
		{
			switch (type)
			{
				case TypeCode.Object:
					return SystemTypeCode.Object;
				case TypeCode.Boolean:
					return SystemTypeCode.Boolean;
				case TypeCode.Char:
					return SystemTypeCode.Char;
				case TypeCode.SByte:
					return SystemTypeCode.Int8;
				case TypeCode.Byte:
					return SystemTypeCode.UInt8;
				case TypeCode.Int16:
					return SystemTypeCode.Int16;
				case TypeCode.UInt16:
					return SystemTypeCode.UInt16;
				case TypeCode.Int32:
					return SystemTypeCode.Int32;
				case TypeCode.UInt32:
					return SystemTypeCode.UInt32;
				case TypeCode.Int64:
					return SystemTypeCode.Int64;
				case TypeCode.UInt64:
					return SystemTypeCode.UInt64;
				case TypeCode.Single:
					return SystemTypeCode.Single;
				case TypeCode.Double:
					return SystemTypeCode.Double;
				case TypeCode.Decimal:
					return SystemTypeCode.Decimal;
				case TypeCode.DateTime:
					return SystemTypeCode.DateTime;
				case TypeCode.String:
					return SystemTypeCode.String;
				default:
					throw new ArgumentOutOfRangeException("type");
			}
		}
	}
}