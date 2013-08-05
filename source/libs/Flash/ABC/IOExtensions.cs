using System;
using System.Text;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
    public static class IOExtensions
    {
    	public static AbcMethod ReadAbcMethod(this SwfReader reader)
        {
            var index = (int)reader.ReadUIntEncoded();
            return reader.ABC.Methods[index];
        }

        public static AbcConst<string> ReadAbcString(this SwfReader reader)
        {
            var index = (int)reader.ReadUIntEncoded();
            return reader.ABC.StringPool[index];
        }

        public static AbcNamespace ReadAbcNamespace(this SwfReader reader)
        {
            var index = (int)reader.ReadUIntEncoded();
            return reader.ABC.Namespaces[index];
        }

        public static AbcMultiname ReadMultiname(this SwfReader reader)
        {
            var index = (int)reader.ReadUIntEncoded();
            return reader.ABC.Multinames[index];
        }

        public static T ReadAbcConst<T>(this SwfReader reader, AbcConstKind kind)
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
                        var len = (int)reader.ReadUIntEncoded();
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

        public static void WriteAbcConst<T>(this SwfWriter writer, T value)
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
                        var s = value as string;
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

        private static AbcConstKind GetConstantKind(object value)
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

    	public static void WriteConstIndex(this SwfWriter writer, object value)
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