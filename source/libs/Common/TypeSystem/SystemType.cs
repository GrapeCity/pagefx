using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class SystemType
	{
		public SystemType(SystemTypeCode code, string name)
		{
			Code = code;
			Name = name;
		}

		public string Name { get; private set; }

		public SystemTypeCode Code { get; private set; }

		public string CSharpKeyword { get; internal set; }

		public string ShortName
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Int8: return "i8";
					case SystemTypeCode.UInt8: return "u8";
					case SystemTypeCode.Int16: return "i16";
					case SystemTypeCode.UInt16: return "u16";
					case SystemTypeCode.Int32: return "i32";
					case SystemTypeCode.UInt32: return "u32";
					case SystemTypeCode.Int64: return "i64";
					case SystemTypeCode.UInt64: return "u64";
					case SystemTypeCode.Single: return "f32";
					case SystemTypeCode.Double: return "f64";
					case SystemTypeCode.Boolean: return "b";
					case SystemTypeCode.Char: return "c";
				}
				return CSharpKeyword;
			}
		}

		public TypeCode TypeCode
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Boolean: return TypeCode.Boolean;
					case SystemTypeCode.Int8: return TypeCode.SByte;
					case SystemTypeCode.UInt8: return TypeCode.Byte;
					case SystemTypeCode.Int16: return TypeCode.Int16;
					case SystemTypeCode.UInt16: return TypeCode.UInt16;
					case SystemTypeCode.Int32: return TypeCode.Int32;
					case SystemTypeCode.UInt32: return TypeCode.UInt32;
					case SystemTypeCode.Int64: return TypeCode.Int64;
					case SystemTypeCode.UInt64: return TypeCode.UInt64;
					case SystemTypeCode.Single: return TypeCode.Single;
					case SystemTypeCode.Double: return TypeCode.Double;
					case SystemTypeCode.Decimal: return TypeCode.Decimal;
					case SystemTypeCode.DateTime: return TypeCode.DateTime;
					case SystemTypeCode.Char: return TypeCode.Char;
					case SystemTypeCode.String: return TypeCode.String;
					case SystemTypeCode.Object: return TypeCode.Object;
					default: return TypeCode.Empty;
				}
			}
		}

		public TypeKind Kind
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.None:
					case SystemTypeCode.Void:
					case SystemTypeCode.Boolean:
					case SystemTypeCode.Int8:
					case SystemTypeCode.UInt8:
					case SystemTypeCode.Int16:
					case SystemTypeCode.UInt16:
					case SystemTypeCode.Int32:
					case SystemTypeCode.UInt32:
					case SystemTypeCode.Int64:
					case SystemTypeCode.UInt64:
					case SystemTypeCode.Single:
					case SystemTypeCode.Double:
					case SystemTypeCode.Decimal:
					case SystemTypeCode.UIntPtr:
					case SystemTypeCode.IntPtr:
					case SystemTypeCode.Char:
						return TypeKind.Primitive;

					case SystemTypeCode.String:
					case SystemTypeCode.Object:
					case SystemTypeCode.Array:
					case SystemTypeCode.Type:
					case SystemTypeCode.TypedReference:
						return TypeKind.Class;

					case SystemTypeCode.ValueType:
					case SystemTypeCode.Enum:
						return TypeKind.Struct;

					case SystemTypeCode.Delegate:
					case SystemTypeCode.MulticastDelegate:
						return TypeKind.Delegate;

					default:
						return TypeKind.Class;
				}
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public static SystemTypeCode ParseCode(string name)
		{
			name = name.ToLowerInvariant();
			switch (name)
			{
				case "sbyte":
				case "int8":
					return SystemTypeCode.Int8;
				case "byte":
				case "uint8":
					return SystemTypeCode.UInt8;
				case "short":
				case "int16":
					return SystemTypeCode.Int16;
				case "ushort":
				case "uint16":
					return SystemTypeCode.UInt16;
				case "int":
				case "int32":
					return SystemTypeCode.Int32;
				case "uint":
				case "uint32":
					return SystemTypeCode.UInt32;
				case "long":
				case "int64":
					return SystemTypeCode.Int64;
				case "ulong":
				case "uint64":
					return SystemTypeCode.UInt64;
				case "char":
					return SystemTypeCode.Char;
				case "float":
				case "single":
				case "float32":
					return SystemTypeCode.Single;
				case "double":
				case "float64":
					return SystemTypeCode.Double;
				case "decimal":
					return SystemTypeCode.Decimal;
				case "string":
					return SystemTypeCode.String;
				default:
					return SystemTypeCode.None;
			}
		}

		public int Size
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Boolean:
					case SystemTypeCode.Int8:
					case SystemTypeCode.UInt8:
						return 1;

					case SystemTypeCode.Char:
					case SystemTypeCode.Int16:
					case SystemTypeCode.UInt16:
						return 2;

					case SystemTypeCode.Int32:
					case SystemTypeCode.UInt32:
					case SystemTypeCode.Single:
						return 4;

					case SystemTypeCode.Int64:
					case SystemTypeCode.UInt64:
					case SystemTypeCode.Double:
						return 8;

					case SystemTypeCode.Decimal:
						return 16;
				}
				return 0;
			}
		}

		public int Bits
		{
			get { return Size * 8; }
		}

		public bool IsNumeric
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Boolean:
					case SystemTypeCode.Int8:
					case SystemTypeCode.UInt8:
					case SystemTypeCode.Char:
					case SystemTypeCode.Int16:
					case SystemTypeCode.UInt16:
					case SystemTypeCode.Int32:
					case SystemTypeCode.UInt32:
					case SystemTypeCode.Int64:
					case SystemTypeCode.UInt64:
					case SystemTypeCode.Single:
					case SystemTypeCode.Double:
					case SystemTypeCode.Decimal:
						return true;
				}
				return false;
			}
		}

		public bool IsSigned
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Int8:
					case SystemTypeCode.Int16:
					case SystemTypeCode.Int32:
					case SystemTypeCode.Int64:
						return true;
				}
				return false;
			}
		}

		public bool IsUnsigned
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.UInt8:
					case SystemTypeCode.UInt16:
					case SystemTypeCode.UInt32:
					case SystemTypeCode.UInt64:
						return true;
				}
				return false;
			}
		}

		

		public bool IsIntegral
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Int8:
					case SystemTypeCode.Int16:
					case SystemTypeCode.Int32:
					case SystemTypeCode.Int64:
					case SystemTypeCode.UInt8:
					case SystemTypeCode.UInt16:
					case SystemTypeCode.UInt32:
					case SystemTypeCode.UInt64:
						return true;
				}
				return false;
			}
		}

		public bool LessThenInt32
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Boolean:
					case SystemTypeCode.Int8:
					case SystemTypeCode.Int16:
					case SystemTypeCode.UInt8:
					case SystemTypeCode.UInt16:
					case SystemTypeCode.Char:
						return true;
				}
				return false;
			}
		}

		public bool IsIntegral32
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Int32:
					case SystemTypeCode.UInt32:
					case SystemTypeCode.Int8:
					case SystemTypeCode.UInt8:
					case SystemTypeCode.Int16:
					case SystemTypeCode.UInt16:
					case SystemTypeCode.Boolean:
					case SystemTypeCode.Char:
						return true;
				}
				return false;
			}
		}

		public bool IsDecimal
		{
			get { return Code == SystemTypeCode.Decimal; }
		}

		public bool IsSingle
		{
			get { return Code == SystemTypeCode.Single; }
		}

		public bool IsDouble
		{
			get { return Code == SystemTypeCode.Double; }
		}

		public bool IsInt32
		{
			get { return Code == SystemTypeCode.Int32; }
		}

		public bool IsUInt32
		{
			get { return Code == SystemTypeCode.UInt32; }
		}

		public bool IsInt64
		{
			get { return Code == SystemTypeCode.Int64; }
		}

		public bool IsUInt64
		{
			get { return Code == SystemTypeCode.UInt64; }
		}

		public bool IsDecimalOrInt64
		{
			get
			{
				switch (Code)
				{
					case SystemTypeCode.Decimal:
					case SystemTypeCode.Int64:
					case SystemTypeCode.UInt64:
						return true;
				}
				return false;
			}
		}

		public const string Namespace = "System";

		public string FullName
		{
			get { return Namespace + "." + Name; }
		}
	}
}