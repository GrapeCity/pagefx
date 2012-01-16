using System;
using System.IO;
using System.Reflection;
using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Metadata
{
    #region Enums
    public enum MdbSignatureKind
    {
        Method = 0x00,
        Field = 0x06,
        LocalVars = 0x07,
        Property = 0x08,
    }

    [Flags]
    internal enum SIGTYPE : byte
    {
        GENERIC = 0x10,
        HASTHIS = 0x20,
        EXPLICITTHIS = 0x40,

        //One of this values identifies method signature
        DEFAULT = 0x00,
        C = 0x01,
        STDCALL = 0x02,
        THISCALL = 0x03,
        FASTCALL = 0x04,
        VARARG = 0x05,

        FIELD = 0x06,
        LOCAL = 0x07,
        PROPERTY = 0x08,
        TYPEMASK = 0xF,
    }
    #endregion

    #region class ArrayShape
    public class ArrayShape
    {
        public int Rank;
        public int[] Sizes;
        public int[] LoBounds;

        public static ArrayShape Single
        {
            get
            {
            	return _single ?? (_single = new ArrayShape
            	                             	{
            	                             		LoBounds = new int[0],
            	                             		Sizes = new int[0]
            	                             	});
            }
        }
        private static ArrayShape _single;

        public int GetLowBound(int index)
        {
            if (LoBounds != null && index >= 0 && index < LoBounds.Length)
                return LoBounds[index];
            return -1;
        }

        public int GetUpperBound(int index)
        {
            if (LoBounds != null && index >= 0 && index < LoBounds.Length
                && Sizes != null && index < Sizes.Length)
                return LoBounds[index] + Sizes[index] - 1;
            return -1;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("[");
            for (int i = 0; i < Rank; i++)
            {
                if (i > 0) s.Append(", ");
                int l = GetLowBound(i);
                int u = GetUpperBound(i);
                s.Append(ArrayDimension.Format(l, u, false));
            }
            s.Append("]");
            return s.ToString();
        }

        public ArrayDimensionCollection ToDimension()
        {
            int n = Rank;
            if (n <= 0) return ArrayDimensionCollection.Single;

            var dim = new ArrayDimensionCollection();
            for (int i = 0; i < n; ++i)
            {
                var d = new ArrayDimension
                {
                    LowerBound = GetLowBound(i),
                    UpperBound = GetUpperBound(i)
                };
                dim.Add(d);
            }
            return dim;
        }
    }
    #endregion

    #region class MdbTypeSignature
    public class MdbTypeSignature
    {
        public ElementType Element;
        public MdbIndex TypeIndex;
        public MdbTypeSignature Type;
        public MdbTypeSignature[] GenericParams;
        public ArrayShape ArrayShape;
        public int GenericParamNumber = -1;
        public MdbMethodSignature Method;
        public IType ResolvedType;

        public MdbTypeSignature(ElementType e)
        {
            Element = e;
        }

        public MdbTypeSignature(ElementType e, MdbTypeSignature type)
        {
            Element = e;
            Type = type;
        }

        public MdbTypeSignature(ElementType e, MdbTypeSignature type, ArrayShape info)
        {
            Element = e;
            Type = type;
            ArrayShape = info;
        }

        public MdbTypeSignature(ElementType e, MdbTypeSignature type, MdbTypeSignature[] genericParams)
        {
            Element = e;
            Type = type;
            GenericParams = genericParams;
        }

        public MdbTypeSignature(ElementType e, MdbIndex typeIndex)
        {
            Element = e;
            TypeIndex = typeIndex;
        }

        public MdbTypeSignature(ElementType e, MdbIndex typeIndex, MdbTypeSignature type)
        {
            Element = e;
            TypeIndex = typeIndex;
            Type = type;
        }

        public MdbTypeSignature(ElementType e, int genericParam)
        {
            Element = e;
            GenericParamNumber = genericParam;
        }

        public MdbTypeSignature(ElementType e, MdbMethodSignature sig)
        {
            Element = e;
            Method = sig;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append(Element.ToString());

            switch (Element)
            {
                case ElementType.ValueType:
                case ElementType.Class:
                case ElementType.CustomArgsEnum:
                    {
                        s.Append(" ");
                        s.Append(TypeIndex.ToString());
                    }
                    break;

                case ElementType.ByRef:
                case ElementType.Ptr:
                    {
                        s.Append("[");
                        s.Append(Type.ToString());
                        s.Append("]");
                    }
                    break;

                case ElementType.Var:
                case ElementType.MethodVar:
                    {
                        s.Append(" ");
                        s.Append(GenericParamNumber);
                    }
                    break;

                case ElementType.Array:
                    {
                        s.Append(" ");
                        s.Append(Type.ToString());
                        s.Append(ArrayShape.ToString());
                    }
                    break;

                case ElementType.ArraySz:
                    {
                        s.Append(" ");
                        s.Append(Type.ToString());
                    }
                    break;

                case ElementType.GenericInstantiation:
                    {
                        s.Append(" ");
                        s.Append(Type.ToString());
                        s.Append("<");
                        for (int i = 0; i < GenericParams.Length; ++i)
                        {
                            if (i > 0) s.Append(", ");
                            s.Append(GenericParams[i].ToString());
                        }
                        s.Append(">");
                    }
                    break;

                case ElementType.MethodPtr:
                    {
                        s.Append(" ");
                        s.Append(Method.ToString());
                    }
                    break;

                case ElementType.RequiredModifier:
                case ElementType.OptionalModifier:
                    {
                        s.Append(" ");
                        s.Append(TypeIndex.ToString());
                        s.Append(" ");
                        s.Append(Type.ToString());
                    }
                    break;

                case ElementType.Sentinel:
                case ElementType.Pinned:
                    {
                        s.Append(" ");
                        s.Append(Type.ToString());
                    }
                    break;

                case ElementType.CustomArgsType:
                    break;
                case ElementType.CustomArgsBoxedObject:
                    break;
                case ElementType.CustomArgsField:
                    break;
                case ElementType.CustomArgsProperty:
                    break;
            }
            return s.ToString();
        }
    }
    #endregion

    #region class MdbFieldSignature
    public class MdbFieldSignature : MdbSignature
    {
        public override MdbSignatureKind Kind
        {
            get { return MdbSignatureKind.Field; }
        }

        public MdbTypeSignature Type;

        public override string ToString()
        {
            return "FIELD " + Type;
        }
    }
    #endregion

    #region class MdbMethodSignature
    public class MdbMethodSignature : MdbSignature
    {
        public bool IsProperty;
        public CallingConventions CallingConventions = CallingConventions.Standard;
        public int GenericParamCount;
        public MdbTypeSignature Type;
        public MdbTypeSignature[] Params;

        public override MdbSignatureKind Kind
        {
            get
            {
                if (IsProperty) return MdbSignatureKind.Property;
                return MdbSignatureKind.Method;
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            if (IsProperty) s.Append("PROPERTY ");
            else s.Append("METHOD ");

            if (GenericParamCount > 0)
            {
                s.Append("GENERIC<");
                s.Append(GenericParamCount);
                s.Append('>');
            }

            if (Type != null)
            {
                s.Append(" ");
                s.Append(Type.ToString());
                s.Append(" ");
            }

            if (Params != null)
            {
                s.Append("(");
                for (int i = 0; i < Params.Length; ++i)
                {
                    if (i > 0) s.Append(", ");
                    s.Append(Params[i].ToString());
                }
                s.Append(")");
            }

            return s.ToString();
        }
    }
    #endregion

    #region abstract class MdbSignature
    public abstract class MdbSignature
    {
        public abstract MdbSignatureKind Kind { get; }

        public static MdbSignature DecodeSignature(byte[] blob)
        {
            var reader = new BufferedBinaryReader(blob);
            var h = (SIGTYPE)reader.ReadUInt8();
            var type = h & SIGTYPE.TYPEMASK;
            if (type == SIGTYPE.FIELD)
            {
                var f = new MdbFieldSignature
                {
                    Type = DecodeTypeSignature(reader)
                };
                return f;
            }
            if (type == SIGTYPE.LOCAL)
                throw new NotImplementedException();
            return DecodeMethodSignature(reader, h);
        }

        public static MdbFieldSignature DecodeFieldSignature(byte[] blob)
        {
            return DecodeFieldSignature(new BufferedBinaryReader(blob));
        }

        public static MdbFieldSignature DecodeFieldSignature(BufferedBinaryReader reader)
        {
            var h = (SIGTYPE)reader.ReadUInt8();
            if ((h & SIGTYPE.TYPEMASK) != SIGTYPE.FIELD)
                throw new BadMetadataException("Incorrect signature for field.");
            var f = new MdbFieldSignature
            {
                Type = DecodeTypeSignature(reader)
            };
            return f;
        }

        public static MdbMethodSignature DecodeMethodSignature(byte[] blob)
        {
            return DecodeMethodSignature(new BufferedBinaryReader(blob));
        }

        static MdbMethodSignature DecodeMethodSignature(BufferedBinaryReader reader, SIGTYPE h)
        {
            var type = (h & SIGTYPE.TYPEMASK);
            if (!(type == SIGTYPE.PROPERTY || (type >= SIGTYPE.DEFAULT && type <= SIGTYPE.VARARG)))
                throw new BadMetadataException("Incorrect signature for method.");

            var sig = new MdbMethodSignature();
            if (type == SIGTYPE.PROPERTY)
                sig.IsProperty = true;

            if ((h & SIGTYPE.HASTHIS) != 0)
                sig.CallingConventions |= CallingConventions.HasThis;
            if ((h & SIGTYPE.EXPLICITTHIS) != 0)
                sig.CallingConventions |= CallingConventions.ExplicitThis;
            if (type == SIGTYPE.VARARG)
            {
                sig.CallingConventions &= CallingConventions.Standard;
                sig.CallingConventions |= CallingConventions.VarArgs;
            }

            if ((h & SIGTYPE.GENERIC) != 0)
                sig.GenericParamCount = reader.ReadPackedInt();

            int paramCount = reader.ReadPackedInt();

            sig.Type = DecodeTypeSignature(reader);
            sig.Params = new MdbTypeSignature[paramCount];
            for (int i = 0; i < paramCount; ++i)
            {
                sig.Params[i] = DecodeTypeSignature(reader);
            }
            return sig;
        }

        public static MdbMethodSignature DecodeMethodSignature(BufferedBinaryReader reader)
        {
            var h = (SIGTYPE)reader.ReadUInt8();
            return DecodeMethodSignature(reader, h);
        }

        static MdbIndex DecodeTypeDefOrRef(BufferedBinaryReader reader)
        {
            uint token = (uint)reader.ReadPackedInt();
            return MdbCodedIndex.TypeDefOrRef.Decode(token);
        }

        public static MdbTypeSignature DecodeTypeSignature(byte[] blob)
        {
            return DecodeTypeSignature(new BufferedBinaryReader(blob));
        }

        public static MdbTypeSignature DecodeTypeSignature(BufferedBinaryReader reader)
        {
            var e = (ElementType)reader.ReadPackedInt();
            switch (e)
            {
                case ElementType.End:
                case ElementType.Void:
                case ElementType.Boolean:
                case ElementType.Char:
                case ElementType.Int8:
                case ElementType.UInt8:
                case ElementType.Int16:
                case ElementType.UInt16:
                case ElementType.Int32:
                case ElementType.UInt32:
                case ElementType.Int64:
                case ElementType.UInt64:
                case ElementType.Single:
                case ElementType.Double:
                case ElementType.String:
                case ElementType.TypedReference:
                case ElementType.IntPtr:
                case ElementType.UIntPtr:
                case ElementType.Object:
                    return new MdbTypeSignature(e);

                case ElementType.Ptr:
                case ElementType.ByRef:
                    {
                        var type = DecodeTypeSignature(reader);
                        if (type == null)
                            throw new BadSignatureException(string.Format("Unable to decode type of {0} signature.", e));
                        return new MdbTypeSignature(e, type);
                    }

                case ElementType.ValueType:
                case ElementType.Class:
                case ElementType.CustomArgsEnum:
                    {
                        var index = DecodeTypeDefOrRef(reader);
                        return new MdbTypeSignature(e, index);
                    }

                case ElementType.Array:
                    {
                        var type = DecodeTypeSignature(reader);
                        if (type == null)
                            throw new BadSignatureException("Unable to decode type of array.");

                        var info = new ArrayShape();
                        info.Rank = reader.ReadPackedInt();
                        int numSizes = reader.ReadPackedInt();
                        info.Sizes = new int[numSizes];
                        for (int i = 0; i < numSizes; i++)
                            info.Sizes[i] = reader.ReadPackedInt();

                        int numLoBounds = reader.ReadPackedInt();
                        info.LoBounds = new int[numLoBounds];
                        for (int i = 0; i < numLoBounds; i++)
                            info.LoBounds[i] = reader.ReadPackedInt();

                        return new MdbTypeSignature(e, type, info);
                    }

                case ElementType.ArraySz:
                    {
                        var type = DecodeTypeSignature(reader);
                        if (type == null)
                            throw new BadSignatureException("Unable to decode type of single-dimensional array signature.");
                        return new MdbTypeSignature(e, type, ArrayShape.Single);
                    }

                case ElementType.MethodPtr:
                    {
                        var msig = DecodeMethodSignature(reader);
                        return new MdbTypeSignature(e, msig);
                    }

                case ElementType.RequiredModifier:
                case ElementType.OptionalModifier:
                    {
                        var i = DecodeTypeDefOrRef(reader);
                        var type = DecodeTypeSignature(reader);
                        return new MdbTypeSignature(e, i, type);
                    }

                case ElementType.Sentinel:
                case ElementType.Pinned:
                    {
                        var type = DecodeTypeSignature(reader);
                        return new MdbTypeSignature(e, type);
                    }

                case ElementType.GenericInstantiation:
                    {
                        var type = DecodeTypeSignature(reader);
                        int n = reader.ReadPackedInt();
                        var genericParams = new MdbTypeSignature[n];
                        for (int i = 0; i < n; i++)
                            genericParams[i] = DecodeTypeSignature(reader);
                        return new MdbTypeSignature(e, type, genericParams);
                    }

                case ElementType.Var:
                case ElementType.MethodVar:
                    {
                        int num = reader.ReadPackedInt();
                        return new MdbTypeSignature(e, num);
                    }
            }

            throw new BadSignatureException("Unknown element type.");
        }
    }
    #endregion
}