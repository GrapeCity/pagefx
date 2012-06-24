using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal static class CLR
    {
        #region InitializeArray
        public static bool InitializeArray(IStatement st)
        {
            ICallExpression call;
            if (IsInitializeArray(st, out call))
            {
                InitializeArray(call);
                return true;
            }
            return false;
        }

        private static ICallExpression ToCallExpression(IStatement s)
        {
            var es = s as IExpressionStatement;
            if (es == null) return null;
            return es.Expression as ICallExpression;
        }

        public static bool IsInitializeArray(this IMethod method)
        {
            if (!method.IsStatic) return false;
            if (method.Name != "InitializeArray") return false;
            if (method.DeclaringType.FullName != "System.Runtime.CompilerServices.RuntimeHelpers")
                return false;
            return true;
        }

        private static bool IsInitializeArray(IStatement s, out ICallExpression call)
        {
            call = ToCallExpression(s);
            if (call == null) return false;
            return call.Method.Method.IsInitializeArray();
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

		public static List<object> ReadArrayValues(IField f, TypeCode type)
		{
			return ReadArrayValues(f, type.ToSystemTypeCode());
		}

        public static List<object> ReadArrayValues(IField f, SystemTypeCode type)
        {
            var blob = f.Value as byte[];
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

        private static void InitializeArray(ICallExpression call)
        {
            var arr = call.Arguments[0] as INewArrayExpression;
            if (arr == null)
                throw new DecompileException();

            var arrType = call.Arguments[0].ResultType as IArrayType;
            if (arrType == null)
                throw new DecompileException();

            var fe = call.Arguments[1] as IFieldReferenceExpression;
            if (fe == null)
                throw new DecompileException();

            var f = fe.Field;
            var vals = ReadArrayValues(f, arrType.ElementType.SystemType.Code);
            
            int n = vals.Count;
            for (int i = 0; i < n; ++i)
            {
                arr.Initializers.Add(new ConstExpression(vals[i]));
            }
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