using System;
using System.Diagnostics;
using System.Text;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    public static class AbcIO
    {
        public static int SizeOf(AbcFile file, ISwfAtom atom)
        {
            using (var writer = new SwfWriter())
            {
                writer.ABC = file;
                atom.Write(writer);
                return writer.ToByteArray().Length;
            }
        }

        public static AbcMethod ReadMethod(SwfReader reader)
        {
            int index = (int)reader.ReadUIntEncoded();
            return reader.ABC.Methods[index];
        }

        public static AbcConst<string> ReadString(SwfReader reader)
        {
            int index = (int)reader.ReadUIntEncoded();
            return reader.ABC.StringPool[index];
        }

        public static AbcNamespace ReadNamespace(SwfReader reader)
        {
            int index = (int)reader.ReadUIntEncoded();
            return reader.ABC.Namespaces[index];
        }

        public static AbcMultiname ReadMultiname(SwfReader reader)
        {
            int index = (int)reader.ReadUIntEncoded();
            return reader.ABC.Multinames[index];
        }

        public static T ReadConst<T>(SwfReader reader, AbcConstKind kind)
        {
            switch (kind)
            {
                case AbcConstKind.Int:
                    return (T)(object)reader.ReadIntEncoded();

                case AbcConstKind.UInt:
                    return (T)(object)reader.ReadUIntEncoded();

                case AbcConstKind.Double:
                    return (T)(object)reader.ReadDouble();

                case AbcConstKind.String:
                    {
                        string s = string.Empty;
                        int len = (int)reader.ReadUIntEncoded();
                        if (len > 0)
                        {
                            var data = reader.ReadUInt8(len);
                            s = Encoding.UTF8.GetString(data);
                        }
                        return (T)(object)s;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public static void WriteConst<T>(SwfWriter writer, T value)
        {
            var typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Int32:
                    writer.WriteIntEncoded((int)(object)value);
                    break;

                case TypeCode.UInt32:
                    writer.WriteUIntEncoded((uint)(object)value);
                    break;

                case TypeCode.Double:
                    writer.WriteDouble((double)(object)value);
                    break;

                case TypeCode.String:
                    {
                        string s = value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            writer.WriteUIntEncoded(0);
                        }
                        else
                        {
                            var data = Encoding.UTF8.GetBytes(s);
                            writer.WriteUIntEncoded((uint)data.Length);
                            writer.Write(data);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static AbcConstKind GetConstantKind(object value)
        {
            if (value == null) 
                return AbcConstKind.Null;
            if (AbcFile.IsUndefined(value)) 
                return AbcConstKind.Undefined;

            var c = value as IAbcConst;
            if (c != null)
                return c.Kind;

            var typeCode = Type.GetTypeCode(value.GetType());
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return (bool)value ? AbcConstKind.True : AbcConstKind.False;

                case TypeCode.Int32:
                    return AbcConstKind.Int;

                case TypeCode.UInt32:
                    return AbcConstKind.UInt;

                case TypeCode.Double:
                    return AbcConstKind.Double;

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsStandardConstant(AbcConstKind kind)
        {
            switch (kind)
            {
                case AbcConstKind.True:
                case AbcConstKind.False:
                case AbcConstKind.Null:
                case AbcConstKind.Undefined:
                    return true;
            }
            return false;
        }

        public static void WriteConstIndex(SwfWriter writer, object value)
        {
            var c = value as IAbcConst;
            if (c != null)
            {
                writer.WriteUIntEncoded((uint)c.Index);
                writer.WriteUInt8((byte)c.Kind);
                return;
            }

            var kind = GetConstantKind(value);
            switch (kind)
            {
                case AbcConstKind.True:
                case AbcConstKind.False:
                case AbcConstKind.Null:
                case AbcConstKind.Undefined:
                    writer.WriteUInt8((byte)kind);
                    writer.WriteUInt8((byte)kind);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}