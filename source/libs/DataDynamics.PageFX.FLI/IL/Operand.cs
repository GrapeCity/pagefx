using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.IL
{
    #region enum OperandType
    /// <summary>
    /// Enumerates operand types.
    /// </summary>
    public enum OperandType
    {
        /// <summary>
        /// Unsigned encoded int.
        /// </summary>
        U30,

        /// <summary>
        /// Signed encoded int.
        /// </summary>
        S30,

        /// <summary>
        /// 8-bit unsigned integer.
        /// </summary>
        U8,

        /// <summary>
        /// 16-bit unsigned integer.
        /// </summary>
        U16,

        /// <summary>
        /// 24-bit unsigned integer.
        /// </summary>
        U24,

        /// <summary>
        /// 32-bit unsigned integer.
        /// </summary>
        U32,

        /// <summary>
        /// 24-bit signed integer.
        /// </summary>
        S24,

        /// <summary>
        /// Index in int constant pool (actually U30)
        /// </summary>
        ConstInt,

        /// <summary>
        /// Index in uint constant pool (actually U30)
        /// </summary>
        ConstUInt,

        /// <summary>
        /// Index in double constant pool (actually U30)
        /// </summary>
        ConstDouble,

        /// <summary>
        /// Index in string constant pool (actually U30)
        /// </summary>
        ConstString,

        /// <summary>
        /// Index in multiname constant pool (actually U30)
        /// </summary>
        ConstMultiname,

        /// <summary>
        /// Index in namespace constant pool (actually U30)
        /// </summary>
        ConstNamespace,

        /// <summary>
        /// Index in ABC method_info array (actually U30)
        /// </summary>
        MethodIndex,

        /// <summary>
        /// Index in ABC class_info array (actually U30)
        /// </summary>
        ClassIndex,

        /// <summary>
        /// An index of an exception_info structure for current method (actually U30)
        /// </summary>
        ExceptionIndex,

        /// <summary>
        /// The number of bytes to jump
        /// </summary>
        BranchTarget,

        /// <summary>
        /// Array of branch targets
        /// </summary>
        BranchTargets,
    }
    #endregion

    /// <summary>
    /// Represents operand of instruction.
    /// </summary>
    public class Operand : ICloneable, ISupportXmlDump
    {
        #region Constructors
        public Operand()
        {    
        }

        public Operand(OperandType type, object value)
        {
            Type = type;
            Value = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of this operand.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of this operand.
        /// </summary>
        public OperandType Type { get; set; }

        /// <summary>
        /// Gets the size of operand in bytes.
        /// </summary>
        public int Size
        {
            get
            {
                switch (Type)
                {
                    case OperandType.U8:
                        return 1;

                    case OperandType.U24:
                    case OperandType.S24:
                    case OperandType.BranchTarget:
                        return 3;

                    case OperandType.U30:
                    case OperandType.ConstInt:
                    case OperandType.ConstUInt:
                    case OperandType.ConstDouble:
                    case OperandType.ConstString:
                    case OperandType.ConstMultiname:
                    case OperandType.ConstNamespace:
                    case OperandType.MethodIndex:
                    case OperandType.ClassIndex:
                    case OperandType.ExceptionIndex:
                        if (Value != null)
                        {
                            int v = ToInt32(Value);
                            return SwfWriter.SizeOfUIntEncoded((uint)v);
                        }
                        break;

                    case OperandType.S30:
                        if (Value != null)
                        {
                            int v = ToInt32(Value);
                            return SwfWriter.SizeOfIntEncoded(v);
                        }
                        break;

                    case OperandType.BranchTargets:
                        if (Value != null)
                        {
                            var offsets = (int[])Value;
                            return offsets.Length * 3;
                        }
                        break;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the value of this operand.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets description of this operand.
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region ICloneable Members
        public Operand Clone()
        {
            var op = new Operand
                         {
                             Description = Description,
                             Name = Name,
                             Type = Type
                         };
            if (Value != null)
            {
            	var c = Value as ICloneable;
            	op.Value = c != null ? c.Clone() : Value;
            }
        	return op;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Read
        public static object Read(AbcMethodBody body, SwfReader reader, OperandType type)
        {
            switch (type)
            {
                case OperandType.U30:
                    return (int)reader.ReadUIntEncoded();

                case OperandType.S30:
                    return reader.ReadIntEncoded();

                case OperandType.U8:
                    return reader.ReadUInt8();

                case OperandType.U16:
                    return reader.ReadUInt16();

                case OperandType.U24:
                    return reader.ReadUInt24();

                case OperandType.U32:
                    return reader.ReadUInt32();

                case OperandType.S24:
                case OperandType.BranchTarget:
                    return reader.ReadInt24();

                case OperandType.ConstInt:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.IntPool[index];
                    }

                case OperandType.ConstUInt:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.UIntPool[index];
                    }

                case OperandType.ConstDouble:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.DoublePool[index];
                    }

                case OperandType.ConstString:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.StringPool[index];
                    }

                case OperandType.ConstMultiname:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.Multinames[index];
                    }

                case OperandType.ConstNamespace:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.Namespaces[index];
                    }

                case OperandType.MethodIndex:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.Methods[index];
                    }

                case OperandType.ClassIndex:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return reader.ABC.Classes[index];
                    }

                case OperandType.ExceptionIndex:
                    {
                        int index = (int)reader.ReadUIntEncoded();
                        return body.Exceptions[index];
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Read(AbcMethodBody body, SwfReader reader)
        {
            Value = Read(body, reader, Type);
        }
        #endregion

        #region Write
        public static void Write(SwfWriter writer, OperandType type, object value)
        {
            var idx = value as ISwfIndexedAtom;
            if (idx != null)
            {
                int index = idx.Index;
                if (index <= 0 && idx is IAbcConst)
                    throw new InvalidOperationException();
                writer.WriteUIntEncoded((uint)index);
                return;
            }

            switch (type)
            {
                case OperandType.U30:
                    {
                        int v = ToInt32C(value);
                        writer.WriteUIntEncoded((uint)v);
                    }
                    break;

                case OperandType.S30:
                    {
                        int v = ToInt32C(value);
                        writer.WriteIntEncoded(v);
                    }
                    break;

                case OperandType.U8:
                    {
                        int v = ToInt32C(value);
                        writer.WriteUInt8((byte)v);
                    }
                    break;

                case OperandType.U16:
                    {
                        int v = ToInt32C(value);
                        writer.WriteUInt16((ushort)v);
                    }
                    break;

                case OperandType.U24:
                    {
                        uint v = ToUInt32C(value);
                        writer.WriteUInt24(v);
                    }
                    break;

                case OperandType.U32:
                    {
                        uint v = ToUInt32C(value);
                        writer.WriteUInt32(v);
                    }
                    break;

                case OperandType.S24:
                case OperandType.BranchTarget:
                    {
                        var c = value as IConvertible;
                        if (c == null)
                            throw new InvalidOperationException();
                        int v = c.ToInt32(null);
                        writer.WriteInt24(v);
                    }
                    break;

                case OperandType.BranchTargets:
                    {
                        var offsets = (int[])value;
                        int n = offsets.Length;
                        for (int i = 0; i < n; ++i)
                            writer.WriteInt24(offsets[i]);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Write(SwfWriter writer)
        {
            Write(writer, Type, Value);
        }
        #endregion

        #region Xml Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("op");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("type", Type.ToString());
            writer.WriteAttributeString("desc", Description);
            writer.WriteEndElement();
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool valueOnly)
        {
            var sb = new StringBuilder();
            if (valueOnly || string.IsNullOrEmpty(Name))
            {
                sb.Append(ToString(Value));
            }
            else
            {
                sb.Append(Name);
                sb.Append(" = ");
                sb.Append(ToString(Value));
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var op = obj as Operand;
            if (op == null) return false;
            if (op.Type != Type) return false;
            if (!Equals(op.Value, Value)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = (int)Type;
            if (Value != null)
                h ^= Value.GetHashCode();
            return h;
        }
        #endregion

        #region Utils
        public int ToInt32()
        {
            return ToInt32(Value);
        }

        public static int ToInt32(object value)
        {
            var i = value as ISwfIndexedAtom;
            if (i != null)
                return i.Index;

            //if (value is uint)
            //    return (int)(uint)value;

            //if (value is int)
            //    return (int)value;

            var c = value as IConvertible;
            if (c != null)
                return c.ToInt32(null);

            return -1;
        }

        public static int ToInt32C(object value)
        {
            var c = value as IConvertible;
            if (c == null)
                throw new InvalidOperationException();
            return c.ToInt32(null);
        }

        public static uint ToUInt32C(object value)
        {
            var c = value as IConvertible;
            if (c == null)
                throw new InvalidOperationException();
            return c.ToUInt32(null);
        }

        internal static string ToString(object value)
        {
            if (value == null)
                return "null";

            var tc = System.Type.GetTypeCode(value.GetType());
            switch (tc)
            {
                case TypeCode.Object:
                    {
                        var c = value as IAbcConst;
                        if (c != null)
                        {
                            if (c.Kind == AbcConstKind.String)
                            {
                                string s = (string)c.Value;
                                if (s != null)
                                    return Escaper.Escape(s);
                                return "null";
                            }
                            return c.ToString();
                        }
                        var arr = value as int[];
                        if (arr != null)
                            return ToStringIntArray(arr);
                        return value.ToString();
                    }

                case TypeCode.Boolean:
                    return (bool)value ? "true" : "false";

                case TypeCode.Char:
                    return Escaper.Escape(((char)value).ToString(), false);

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                    return value.ToString();

                case TypeCode.String:
                    return Escaper.Escape((string)value);
            }
            return string.Empty;
        }

        private static string ToStringIntArray(int[] arr)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < arr.Length; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(arr[i]);
            }
            sb.Append("}");
            return sb.ToString();
        }
        #endregion
    }
}