using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Collections
{
	internal sealed class ArgumentList : IArgumentCollection
	{
		private readonly ICustomAttribute _owner;
		private readonly BufferedBinaryReader _reader;
		private readonly AssemblyLoader _loader;
		private IReadOnlyList<IArgument> _list;

		public ArgumentList(ICustomAttribute owner, BufferedBinaryReader reader, AssemblyLoader loader)
		{
			_owner = owner;
			_reader = reader;
			_loader = loader;
		}

		private IReadOnlyList<IArgument> List
		{
			get { return _list ?? (_list = ReadArguments().AsReadOnlyList()); }
		}

		public IEnumerator<IArgument> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IArgument this[int index]
		{
			get { return List[index]; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IArgument this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		public void Add(IArgument argument)
		{
			throw new NotSupportedException();
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}

		private IList<IArgument> ReadArguments()
		{
			var reader = _reader;

			reader.Seek(0, SeekOrigin.Begin);

			ushort prolog = reader.ReadUInt16();
			if (prolog != 0x01)
				throw new BadSignatureException("Invalid prolog in custom attribute value");

			var list = new List<IArgument>();
			var ctor = _owner.Constructor;
			foreach (var p in ctor.Parameters)
			{
				var arg = new Argument
					{
						Type = p.Type,
						Kind = ArgumentKind.Fixed,
						Name = p.Name,
						Value = ReadValue(reader, p.Type)
					};
				list.Add(arg);
			}

			int numNamed = reader.ReadUInt16();
			for (int i = 0; i < numNamed; ++i)
			{
				list.Add(ReadNamedArgument(reader));
			}

			return list;
		}

		private IArgument ReadNamedArgument(BufferedBinaryReader reader)
		{
			var arg = new Argument
				{
					Kind = ((ArgumentKind)reader.ReadUInt8())
				};

			if (!(arg.Kind == ArgumentKind.Field || arg.Kind == ArgumentKind.Property))
				throw new BadMetadataException();

			var elemType = (ElementType)reader.ReadUInt8();
			IType enumType;

			switch (elemType)
			{
				case ElementType.CustomArgsEnum:
					enumType = ReadEnumType(reader);
					arg.Type = enumType;
					arg.Name = reader.ReadCountedUtf8();
					arg.Value = ReadValue(reader, enumType);
					break;

				case ElementType.ArraySz:
					elemType = (ElementType)reader.ReadUInt8();
					if (elemType == ElementType.CustomArgsEnum)
					{
						enumType = ReadEnumType(reader);
						arg.Name = reader.ReadCountedUtf8();
						ResolveNamedArgType(arg, _owner.Type);
						arg.Value = ReadArray(reader, enumType);
					}
					else
					{
						arg.Name = reader.ReadCountedUtf8();
						ResolveNamedArgType(arg, _owner.Type);
						arg.Value = ReadArray(reader, elemType);
					}
					break;

				default:
					arg.Name = reader.ReadCountedUtf8();
					ResolveNamedArgType(arg, _owner.Type);
					arg.Value = ReadValue(reader, elemType);
					break;
			}

			return arg;
		}

		private IType FindType(string name)
		{
			//Parse assembly qualified type name
			var parts = name.Split(',');
			if (parts.Length > 1) // fully qualified name
			{
				string asmName = String.Empty;
				for (int i = 1; i < parts.Length; i++)
				{
					if (i != 1) asmName += ", ";
					asmName += parts[i];
				}
				var r = new AssemblyReference(asmName);
				var asm = _loader.ResolveAssembly(r);
				return asm.FindType(parts[0]);
			}

			var type = _loader.Assembly.FindType(name);
			if (type != null)
				return type;

			foreach (var asm in _loader.AssemblyRefs)
			{
				type = asm.FindType(name);
				if (type != null)
					return type;
			}

			return null;
		}

		private IType ReadType(BufferedBinaryReader reader)
		{
			string s = reader.ReadCountedUtf8();
			return FindType(s);
		}

		private object ReadArray(BufferedBinaryReader reader, ElementType elemType)
		{
			Array arr = null;
			int n = reader.ReadInt32();
			for (int i = 0; i < n; ++i)
			{
				var val = ReadValue(reader, elemType);
				if (arr == null)
					arr = Array.CreateInstance(val.GetType(), n);
				arr.SetValue(val, i);
			}
			return arr;
		}

		private object ReadArray(BufferedBinaryReader reader, IType elemType)
		{
			Array arr = null;
			int n = reader.ReadInt32();
			for (int i = 0; i < n; ++i)
			{
				var val = ReadValue(reader, elemType);
				if (arr == null)
					arr = Array.CreateInstance(val.GetType(), n);
				arr.SetValue(val, i);
			}
			return arr;
		}

		private object ReadValue(BufferedBinaryReader reader, ElementType e)
		{
			switch (e)
			{
				case ElementType.Boolean:
					return reader.ReadBoolean();

				case ElementType.Char:
					return reader.ReadChar();

				case ElementType.Int8:
					return reader.ReadSByte();

				case ElementType.UInt8:
					return reader.ReadByte();

				case ElementType.Int16:
					return reader.ReadInt16();

				case ElementType.UInt16:
					return reader.ReadUInt16();

				case ElementType.Int32:
					return reader.ReadInt32();

				case ElementType.UInt32:
					return reader.ReadUInt32();

				case ElementType.Int64:
					return reader.ReadInt64();

				case ElementType.UInt64:
					return reader.ReadUInt64();

				case ElementType.Single:
					return reader.ReadSingle();

				case ElementType.Double:
					return reader.ReadDouble();

				case ElementType.String:
					return reader.ReadCountedUtf8();

				case ElementType.Object:
				case ElementType.CustomArgsBoxedObject:
					{
						var elem = (ElementType)reader.ReadInt8();
						return ReadValue(reader, elem);
					}

				case ElementType.CustomArgsEnum:
					{
						string enumTypeName = reader.ReadCountedUtf8();
						var enumType = FindType(enumTypeName);
						if (enumType == null)
						{
							//TODO:
							throw new BadMetadataException();
						}
						return ReadValue(reader, enumType);
					}

				case ElementType.CustomArgsType:
					return ReadType(reader);

				case ElementType.ArraySz:
					{
						var arrElemType = (ElementType)reader.ReadInt8();
						return ReadArray(reader, arrElemType);
					}

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private object ReadValue(BufferedBinaryReader reader, IType type)
		{
			var st = type.SystemType();
			if (st != null)
			{
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
					case SystemTypeCode.Char:
						return reader.ReadChar();
					case SystemTypeCode.String:
						return reader.ReadCountedUtf8();
					case SystemTypeCode.Object:
						//boxed value type
						var e = (ElementType)reader.ReadInt8();
						return ReadValue(reader, e);
					case SystemTypeCode.Type:
						return ReadType(reader);
				}
			}

			if (type.TypeKind == TypeKind.Enum)
			{
				return ReadValue(reader, type.ValueType);
			}

			var arrType = type as IArrayType;
			if (arrType != null)
			{
				int numElem = reader.ReadInt32();
				Array arr = null;
				for (int i = 0; i < numElem; ++i)
				{
					var val = ReadValue(reader, arrType.ElementType);
					if (arr == null)
						arr = Array.CreateInstance(val.GetType(), numElem);
					arr.SetValue(val, i);
				}
				return arr;
			}

			return null;
		}

		private IType ReadEnumType(BufferedBinaryReader reader)
		{
			string enumTypeName = reader.ReadCountedUtf8();
			return FindType(enumTypeName);
		}

		private static void ResolveNamedArgType(IArgument arg, IType declType)
		{
			if (arg.Kind == ArgumentKind.Field)
			{
				var f = declType.FindField(arg.Name, true);
				if (f == null)
					throw new InvalidOperationException();
				arg.Type = f.Type;
				arg.Member = f;
				return;
			}
			var p = declType.FindProperty(arg.Name, true);
			if (p == null)
				throw new InvalidOperationException();
			arg.Type = p.Type;
			arg.Member = p;
		}
	}
}