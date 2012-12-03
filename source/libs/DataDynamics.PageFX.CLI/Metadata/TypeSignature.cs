using System;
using System.Text;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	internal sealed class TypeSignature
	{
		public ElementType Element;
		public SimpleIndex TypeIndex;
		public TypeSignature Type;
		public TypeSignature[] GenericParams;
		public ArrayShape ArrayShape;
		public int GenericParamNumber = -1;
		public MethodSignature Method;

		public TypeSignature(ElementType e)
		{
			Element = e;
		}

		public TypeSignature(ElementType e, TypeSignature type)
		{
			Element = e;
			Type = type;
		}

		public TypeSignature(ElementType e, TypeSignature type, ArrayShape info)
		{
			Element = e;
			Type = type;
			ArrayShape = info;
		}

		public TypeSignature(ElementType e, TypeSignature type, TypeSignature[] genericParams)
		{
			Element = e;
			Type = type;
			GenericParams = genericParams;
		}

		public TypeSignature(ElementType e, SimpleIndex typeIndex)
		{
			Element = e;
			TypeIndex = typeIndex;
		}

		public TypeSignature(ElementType e, SimpleIndex typeIndex, TypeSignature type)
		{
			Element = e;
			TypeIndex = typeIndex;
			Type = type;
		}

		public TypeSignature(ElementType e, int genericParam)
		{
			Element = e;
			GenericParamNumber = genericParam;
		}

		public TypeSignature(ElementType e, MethodSignature sig)
		{
			Element = e;
			Method = sig;
		}

		private static SimpleIndex DecodeTypeDefOrRef(BufferedBinaryReader reader)
		{
			var token = (uint)reader.ReadPackedInt();
			return CodedIndex.TypeDefOrRef.Decode(token);
		}

		public static TypeSignature Decode(BufferedBinaryReader reader)
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
					return new TypeSignature(e);

				case ElementType.Ptr:
				case ElementType.ByRef:
					{
						var type = Decode(reader);
						if (type == null)
							throw new BadSignatureException(String.Format("Unable to decode type of {0} signature.", e));
						return new TypeSignature(e, type);
					}

				case ElementType.ValueType:
				case ElementType.Class:
				case ElementType.CustomArgsEnum:
					{
						var index = DecodeTypeDefOrRef(reader);
						return new TypeSignature(e, index);
					}

				case ElementType.Array:
					{
						var type = Decode(reader);
						if (type == null)
							throw new BadSignatureException("Unable to decode type of array.");

						int rank = reader.ReadPackedInt();
						int numSizes = reader.ReadPackedInt();
						var info = new ArrayShape {Rank = rank, Sizes = new int[numSizes]};
						for (int i = 0; i < numSizes; i++)
							info.Sizes[i] = reader.ReadPackedInt();

						int numLoBounds = reader.ReadPackedInt();
						info.LoBounds = new int[numLoBounds];
						for (int i = 0; i < numLoBounds; i++)
							info.LoBounds[i] = reader.ReadPackedInt();

						return new TypeSignature(e, type, info);
					}

				case ElementType.ArraySz:
					{
						var type = Decode(reader);
						if (type == null)
							throw new BadSignatureException("Unable to decode type of single-dimensional array signature.");
						return new TypeSignature(e, type, ArrayShape.Single);
					}

				case ElementType.MethodPtr:
					{
						var msig = MethodSignature.Decode(reader);
						return new TypeSignature(e, msig);
					}

				case ElementType.RequiredModifier:
				case ElementType.OptionalModifier:
					{
						var i = DecodeTypeDefOrRef(reader);
						var type = Decode(reader);
						return new TypeSignature(e, i, type);
					}

				case ElementType.Sentinel:
				case ElementType.Pinned:
					{
						var type = Decode(reader);
						return new TypeSignature(e, type);
					}

				case ElementType.GenericInstantiation:
					{
						var type = Decode(reader);
						int n = reader.ReadPackedInt();
						var genericParams = new TypeSignature[n];
						for (int i = 0; i < n; i++)
							genericParams[i] = Decode(reader);
						return new TypeSignature(e, type, genericParams);
					}

				case ElementType.Var:
				case ElementType.MethodVar:
					{
						int num = reader.ReadPackedInt();
						return new TypeSignature(e, num);
					}
			}

			throw new BadSignatureException("Unknown element type.");
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
						s.Append(Type);
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
						s.Append(Type);
						s.Append(ArrayShape);
					}
					break;

				case ElementType.ArraySz:
					{
						s.Append(" ");
						s.Append(Type);
					}
					break;

				case ElementType.GenericInstantiation:
					{
						s.Append(" ");
						s.Append(Type);
						s.Append("<");
						for (int i = 0; i < GenericParams.Length; ++i)
						{
							if (i > 0) s.Append(", ");
							s.Append(GenericParams[i]);
						}
						s.Append(">");
					}
					break;

				case ElementType.MethodPtr:
					{
						s.Append(" ");
						s.Append(Method);
					}
					break;

				case ElementType.RequiredModifier:
				case ElementType.OptionalModifier:
					{
						s.Append(" ");
						s.Append(TypeIndex.ToString());
						s.Append(" ");
						s.Append(Type);
					}
					break;

				case ElementType.Sentinel:
				case ElementType.Pinned:
					{
						s.Append(" ");
						s.Append(Type);
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
}