using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    #region class SystemTypeNameAttribute
    public class SystemTypeNameAttribute : Attribute
    {
        public SystemTypeNameAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
        readonly string _name;
    }
    #endregion

    #region enum SystemTypeCode
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
    #endregion

    #region class SystemType
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

        public IType Value { get; set; }

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

        public IType Unsigned
        {
            get
            {
                switch (Code)
                {
                    case SystemTypeCode.Int8:
                        return SystemTypes.UInt8;
                    case SystemTypeCode.Int16:
                        return SystemTypes.UInt16;
                    case SystemTypeCode.Int32:
                        return SystemTypes.UInt32;
                    case SystemTypeCode.Int64:
                        return SystemTypes.UInt64;
                }
                return null;
            }
        }

        public IType Signed
        {
            get
            {
                switch (Code)
                {
                    case SystemTypeCode.UInt8:
                        return SystemTypes.Int8;
                    case SystemTypeCode.UInt16:
                        return SystemTypes.Int16;
                    case SystemTypeCode.UInt32:
                        return SystemTypes.Int32;
                    case SystemTypeCode.UInt64:
                        return SystemTypes.Int64;
                }
                return null;
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
    }
    #endregion

    #region class SystemTypes
    public static class SystemTypes
    {
        public const string Namespace = "System";

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
        private static readonly SystemType[] SysTypes;
	    private static readonly Dictionary<string, SystemType> Lookup;

        public static IType GetType(TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Object:
                    return Object;
                case TypeCode.Boolean:
                    return Boolean;
                case TypeCode.Char:
                    return Char;
                case TypeCode.SByte:
                    return SByte;
                case TypeCode.Byte:
                    return Byte;
                case TypeCode.Int16:
                    return Int16;
                case TypeCode.UInt16:
                    return UInt16;
                case TypeCode.Int32:
                    return Int32;
                case TypeCode.UInt32:
                    return UInt32;
                case TypeCode.Int64:
                    return Int64;
                case TypeCode.UInt64:
                    return UInt64;
                case TypeCode.Single:
                    return Single;
                case TypeCode.Double:
                    return Double;
                case TypeCode.Decimal:
                    return Decimal;
                case TypeCode.DateTime:
                    return DateTime;
                case TypeCode.String:
                    return String;
                default:
                    return null;
            }
        }

        public static IType GetType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var tc = System.Type.GetTypeCode(type);
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
            return GetType(tc);
        }

        public static IType GetType(object obj)
        {
            if (obj == null) return Object;
            return GetType(obj.GetType());
        }

        public static IType Get(SystemTypeCode typeCode)
        {
            int i = (int)typeCode;
            if (i < 0 || i >= Types.Length)
                throw new ArgumentOutOfRangeException("typeCode");
            return Types[i].Value;
        }

        public static TypeCode GetTypeCode(this IType type)
        {
	        if (type == null) return TypeCode.Empty;
			var st = type.SystemType();
			return st != null ? st.TypeCode : TypeCode.Empty;
        }

	    #region Type Properties
        public static IType Void
        {
            get { return Get(SystemTypeCode.Void); }
        }

        public static IType Boolean
        {
            get { return Get(SystemTypeCode.Boolean); }
        }

        public static IType Byte
        {
            get { return Get(SystemTypeCode.UInt8); }
        }

        public static IType UInt8
        {
            get { return Byte; }
        }

        public static IType SByte
        {
            get { return Get(SystemTypeCode.Int8); }
        }

        public static IType Int8
        {
            get { return SByte; }
        }

        public static IType Char
        {
            get { return Get(SystemTypeCode.Char); }
        }

        public static IType Int16
        {
            get { return Get(SystemTypeCode.Int16); }
        }

        public static IType UInt16
        {
            get { return Get(SystemTypeCode.UInt16); }
        }

        public static IType Int32
        {
            get { return Get(SystemTypeCode.Int32); }
        }

        public static IType UInt32
        {
            get { return Get(SystemTypeCode.UInt32); }
        }

        public static IType Int64
        {
            get { return Get(SystemTypeCode.Int64); }
        }

        public static IType UInt64
        {
            get { return Get(SystemTypeCode.UInt64); }
        }

        public static IType Single
        {
            get { return Get(SystemTypeCode.Single); }
        }

        public static IType Float32
        {
            get { return Single; }
        }

        public static IType Double
        {
            get { return Get(SystemTypeCode.Double); }
        }

        public static IType Float64
        {
            get { return Double; }
        }

        public static IType Decimal
        {
            get { return Get(SystemTypeCode.Decimal); }
        }

        public static IType DateTime
        {
            get { return Get(SystemTypeCode.DateTime); }
        }

        public static IType IntPtr
        {
            get { return Get(SystemTypeCode.IntPtr); }
        }

        public static IType UIntPtr
        {
            get { return Get(SystemTypeCode.UIntPtr); }
        }

        public static IType String
        {
            get { return Get(SystemTypeCode.String); }
        }

        public static IType ValueType
        {
            get { return Get(SystemTypeCode.ValueType); }
        }

        public static IType Enum
        {
            get { return Get(SystemTypeCode.Enum); }
        }

        public static IType Object
        {
            get { return Get(SystemTypeCode.Object); }
        }

        public static IType Array
        {
            get { return Get(SystemTypeCode.Array); }
        }

        public static IType Type
        {
            get { return Get(SystemTypeCode.Type); }
        }

        public static IType TypedReference
        {
            get { return Get(SystemTypeCode.TypedReference); }
        }

        public static IType Delegate
        {
            get { return Get(SystemTypeCode.Delegate); }
        }

        public static IType MulticastDelegate
        {
            get { return Get(SystemTypeCode.MulticastDelegate); }
        }

        public static IType Exception
        {
            get { return Get(SystemTypeCode.Exception); }
        }

        public static IType Attribute
        {
            get { return Get(SystemTypeCode.Attribute); }
        }

		public static IType NativeUInt
		{
			get { return UInt32; }
		}

		public static IType NativeInt
		{
			get { return Int32; }
		}
        #endregion

		public static bool IsSystemType(this IType type)
		{
			return type.SystemType() != null;
		}

		public static SystemType SystemType(this IType type)
		{
			if (type == null) return null;
			return type.Namespace == Namespace ? Find(type.Name) : null;
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
            return st == null ? null : st.Signed;
        }

        public static IType ToUnsigned(IType type)
        {
            if (type == null) return null;
            var st = type.SystemType();
            return st == null ? null : st.Unsigned;
        }

        private static IEnumerable<IType> GetDescendingOrder()
        {
            yield return Decimal;
            yield return Double;
            yield return Single;
            yield return Int64;
            yield return UInt64;
            yield return Int32;
            yield return UInt32;
            yield return Int16;
            yield return UInt16;
            yield return Int8;
            yield return UInt8;
        }

        public static IType GetCommonDenominator(IType a, IType b)
        {
        	return GetDescendingOrder().FirstOrDefault(type => a == type || b == type);
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
                    return UInt32;
                case SystemTypeCode.Int64:
                case SystemTypeCode.UInt64:
                    return UInt64;
            }
            return null;
        }

        static bool IsUInt64(IType type)
        {
            return type == UInt64;
        }

        public static IType UInt32OR64(IType a, IType b)
        {
            a = UInt32OR64(a);
            b = UInt32OR64(b);
            if (a == null) return b;
            if (b == null) return null;
            if (IsUInt64(a) || IsUInt64(b))
                return UInt64;
            return UInt32;
        }

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
    #endregion
}