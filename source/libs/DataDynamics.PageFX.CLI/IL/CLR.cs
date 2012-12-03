using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CLI.LoaderInternals;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.IL
{
    internal static class CLR
    {
        #region InitializeArray

	    public static bool IsInitializeArray(this IMethod method)
        {
            if (!method.IsStatic) return false;
            if (method.Name != "InitializeArray") return false;
            if (method.DeclaringType.FullName != "System.Runtime.CompilerServices.RuntimeHelpers")
                return false;
            return true;
        }

	    public static List<object> ReadArrayValues(IField f, TypeCode type)
		{
			return ReadArrayValues(f, type.ToSystemTypeCode());
		}

		public static List<object> ReadArrayValues(IField f, SystemTypeCode type)
		{
			var blob = f.GetBlob();
			if (blob == null)
				throw new ArgumentException("Invalid value of field. Value must be blob.", "f");

			var vals = new List<object>();
			var reader = new BufferedBinaryReader(blob);
			while (reader.Position < reader.Length)
			{
				var value = ReadValue(reader, type);
				vals.Add(value);
			}

			return vals;
		}

		public static byte[] GetBlob(this IField field)
		{
			var value = field.Value;
			var blob = value as byte[];
			if (blob == null)
			{
				switch (Type.GetTypeCode(value.GetType()))
				{
					case TypeCode.Boolean:
						blob = new[] { (bool)value ? (byte)1 : (byte)0 };
						break;
					case TypeCode.Char:
						blob = BitConverter.GetBytes((char)value);
						break;
					case TypeCode.SByte:
						blob = new[] { (byte)(sbyte)value };
						break;
					case TypeCode.Byte:
						blob = new[] { (byte)value };
						break;
					case TypeCode.Int16:
						blob = BitConverter.GetBytes((Int16)value);
						break;
					case TypeCode.UInt16:
						blob = BitConverter.GetBytes((UInt16)value);
						break;
					case TypeCode.Int32:
						blob = BitConverter.GetBytes((Int32)value);
						break;
					case TypeCode.UInt32:
						blob = BitConverter.GetBytes((UInt32)value);
						break;
					case TypeCode.Int64:
						blob = BitConverter.GetBytes((Int64)value);
						break;
					case TypeCode.UInt64:
						blob = BitConverter.GetBytes((UInt64)value);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			return blob;
		}

        private static object ReadValue(BufferedBinaryReader reader, SystemTypeCode type)
        {
            switch (type)
            {
                case SystemTypeCode.Boolean:
                    return reader.ReadBoolean();
                case SystemTypeCode.Int8:
                    return reader.ReadInt8();
                case SystemTypeCode.UInt8:
                    return reader.ReadUInt8();
                case SystemTypeCode.Int16:
                    return reader.ReadInt16();
                case SystemTypeCode.UInt16:
                    return reader.ReadUInt16();
                case SystemTypeCode.Int32:
                    return reader.ReadInt32();
                case SystemTypeCode.UInt32:
                    return reader.ReadUInt32();
                case SystemTypeCode.Int64:
                    return reader.ReadInt64();
                case SystemTypeCode.UInt64:
                    return reader.ReadUInt64();
                case SystemTypeCode.Single:
                    return reader.ReadSingle();
                case SystemTypeCode.Double:
                    return reader.ReadDouble();
                case SystemTypeCode.Decimal:
                    throw new NotImplementedException();
                case SystemTypeCode.DateTime:
                    throw new NotImplementedException();
                case SystemTypeCode.Char:
                    return reader.ReadChar();
                case SystemTypeCode.String:
                    return reader.ReadCountedUtf8();
                case SystemTypeCode.Enum:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region TypeOf
        public static bool IsGetTypeFromHandle(this IMethod method)
        {
            if (!method.IsStatic) return false;
            if (method.Name != "GetTypeFromHandle") return false;
            if (method.DeclaringType.FullName != "System.Type") return false;
            return true;
        }

        public static IExpression TypeOf(ICallExpression call)
        {
            if (call.Method.Method.IsGetTypeFromHandle())
            {
                if (call.Arguments.Count != 1)
                    throw new InvalidOperationException();

                var typeRef = call.Arguments[0] as ITypeReferenceExpression;
                if (typeRef == null)
                    throw new InvalidOperationException();

                return new TypeOfExpression(typeRef.Type);
            }
            return null;
        }
        #endregion

        public static bool IsSpecialMethod(IMethod method)
        {
            if (method.IsInitializeArray()) return true;
            if (method.IsGetTypeFromHandle()) return true;
            return false;
        }
    }
}