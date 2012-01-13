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

        public static bool IsInitializeArray(IMethod method)
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
            return IsInitializeArray(call.Method.Method);
        }

        public static List<object> ReadArrayValues(IField f, IType elemType)
        {
            var blob = f.Value as byte[];
            if (blob == null)
                throw new ArgumentException("Invalid value of field. Value must be blob.", "f");

            var vals = new List<object>();
            var reader = new BufferedBinaryReader(blob);
            while (reader.Position < reader.Length)
            {
                var value = ReadValue(reader, elemType);
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
            var vals = ReadArrayValues(f, arrType.ElementType);
            
            int n = vals.Count;
            for (int i = 0; i < n; ++i)
            {
                arr.Initializers.Add(new ConstExpression(vals[i]));
            }
        }

        private static object ReadValue(BufferedBinaryReader reader, IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var st = type.SystemType;
            if (st == null)
                throw new InvalidOperationException();

            switch (st.Code)
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
                    return IOUtils.ReadCountedUtf8(reader);
                case SystemTypeCode.Enum:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region TypeOf
        public static bool IsGetTypeFromHandle(IMethod method)
        {
            if (!method.IsStatic) return false;
            if (method.Name != "GetTypeFromHandle") return false;
            if (method.DeclaringType.FullName != "System.Type") return false;
            return true;
        }

        public static IExpression TypeOf(ICallExpression call)
        {
            if (IsGetTypeFromHandle(call.Method.Method))
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
            if (IsInitializeArray(method)) return true;
            if (IsGetTypeFromHandle(method)) return true;
            return false;
        }
    }
}