using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class TypeTable : MetadataTable<IType>, ITypeCollection
	{
		private readonly Dictionary<string, IType> _cache = new Dictionary<string, IType>();

		public TypeTable(AssemblyLoader loader) 
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.TypeDef; }
		}

		protected override IType ParseRow(MdbRow row, int index)
		{
			var flags = (TypeAttributes)row[MDB.TypeDef.Flags].Value;
			string name = row[MDB.TypeDef.TypeName].String;
			string ns = row[MDB.TypeDef.TypeNamespace].String;

			var token = MdbIndex.MakeToken(MdbTableId.TypeDef, index + 1);
			var genericParams = Loader.GenericParameters.Find(token);

			bool isIface = IsInterface(flags);
			var kind = isIface ? TypeKind.Interface : TypeKind.Class;
			var sysType = !isIface && ns == SystemTypes.Namespace ? SystemTypes.Find(name) : null;
			if (sysType != null) kind = sysType.Kind;

			var type = CreateType(kind, genericParams);

			type.Namespace = ns;
			type.Name = name;
			SetTypeFlags(type, flags);
			type.Module = Loader.MainModule;

			if (sysType != null)
			{
				sysType.Value = type;
				type.SystemType = sysType;
			}

			// to avoid problems with self refs in fields/methods,etc
			row.Object = type;

			type.MetadataToken = token;
			type.CustomAttributes = new CustomAttributes(Loader, type, token);

			//TODO: lazy resolving of class layout
			type.Layout = ResolveLayout(Mdb, index);

			//TODO: lazy resolving of declaring type
			var declType = ResolveDeclaringType(index);
			if (declType != null)
			{
				type.DeclaringType = declType;
				declType.Types.Add(type);
			}
			else
			{
				_cache.Add(type.FullName, type);
			}

			var nextRow = index + 1 < Count ? Mdb.GetRow(MdbTableId.TypeDef, index + 1) : null;
			SetMembers(row, nextRow, type);

			//TODO: lazy resolving of base type
			SetBaseType(row, type);

			type.Interfaces = new InterfaceImpl(Loader, type);

			LoadMethodImpl(type, index);

			return type;
		}

		private void SetBaseType(MdbRow row, IType type)
		{
			if (type.FullName == "System.Object") return;

			MdbIndex baseIndex = row[MDB.TypeDef.Extends].Value;

			var baseType = Loader.GetTypeDefOrRef(baseIndex, new Context(type));
			type.BaseType = baseType;

			var thisType = type as UserDefinedType;
			if (thisType == null || thisType.TypeKind == TypeKind.Primitive) return;

			TypeKind kind;
			if (TypeKindByBase.TryGetValue(baseType.FullName, out kind))
			{
				thisType.TypeKind = kind;
			}
		}

		private static readonly Dictionary<string, TypeKind> TypeKindByBase =
			new Dictionary<string, TypeKind>
				{
					{"System.Enum", TypeKind.Enum},
					{"System.ValueType", TypeKind.Struct},
					{"System.Delegate", TypeKind.Delegate},
					{"System.MulticastDelegate", TypeKind.Delegate},
				};

		private static ClassLayout ResolveLayout(MdbReader mdb, int typeIndex)
		{
			var row = mdb.LookupRow(MdbTableId.ClassLayout, MDB.ClassLayout.Parent, typeIndex, true);
			if (row == null)
				return null;

			var size = (int)row[MDB.ClassLayout.ClassSize].Value;
			var pack = (int)row[MDB.ClassLayout.PackingSize].Value;
			return new ClassLayout(size, pack);
		}

		private IType ResolveDeclaringType(int index)
		{
			var row = Mdb.LookupRow(MdbTableId.NestedClass, MDB.NestedClass.Class, index, true);
			if (row == null) return null;

			int enclosingIndex = row[MDB.NestedClass.EnclosingClass].Index - 1;
			return this[enclosingIndex];
		}

		private void SetMembers(MdbRow row, MdbRow nextRow, IType type)
		{
			var fields = GetFields(row, nextRow, type);
			var methods = GetMethods(row, nextRow, type);

			var members = (TypeMemberCollection)type.Members;
			members.Fields = fields;			
			members.Methods = methods;
			members.Properties = new PropertyList(type);
			members.Events = new EventList(type);
		}

		private IFieldCollection GetFields(MdbRow row, MdbRow nextRow, IType type)
		{
			var range = Loader.GetFieldRange(row, nextRow);
			if (range == null) return FieldCollection.Empty;
			return new FieldList(Loader, type, range[0], range[1]);
		}

		private IMethodCollection GetMethods(MdbRow row, MdbRow nextRow, IType type)
		{
			var range = Loader.GetMethodRange(row, nextRow);
			if (range == null) return MethodCollection.Empty;
			return new MethodList(Loader, type, range[0], range[1]);
		}

		private void LoadMethodImpl(IType type, int typeIndex)
		{
			var rows = Mdb.LookupRows(MdbTableId.MethodImpl, MDB.MethodImpl.Class, typeIndex, true);
			foreach (var row in rows)
			{
				MdbIndex bodyIdx = row[MDB.MethodImpl.MethodBody].Value;
				MdbIndex declIdx = row[MDB.MethodImpl.MethodDeclaration].Value;

				var body = Loader.GetMethodDefOrRef(bodyIdx, new Context(type));

				var decl = Loader.GetMethodDefOrRef(declIdx, new Context(type, body));

				body.ImplementedMethods = new[] { decl };
				body.IsExplicitImplementation = true;
			}
		}

		private static UserDefinedType CreateType(TypeKind kind, IList<IGenericParameter> genericParameters)
		{
			if (genericParameters != null && genericParameters.Any())
			{
				var type = new GenericType {TypeKind = kind};
				foreach (var parameter in genericParameters)
				{
					type.GenericParameters.Add(parameter);
					parameter.DeclaringType = type;
				}
				return type;
			}
			return new UserDefinedType(kind);
		}

		private static void SetTypeFlags(IType type, TypeAttributes flags)
		{
			type.Visibility = ToVisibility(flags);
			type.IsAbstract = (flags & TypeAttributes.Abstract) != 0;
			type.IsSealed = (flags & TypeAttributes.Sealed) != 0;
			type.IsBeforeFieldInit = (flags & TypeAttributes.BeforeFieldInit) != 0;
			type.IsSpecialName = (flags & TypeAttributes.SpecialName) != 0;
			type.IsRuntimeSpecialName = (flags & TypeAttributes.RTSpecialName) != 0;
		}

		private static bool IsInterface(TypeAttributes f)
		{
			return (f & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface;
		}

		private static Visibility ToVisibility(TypeAttributes f)
		{
			var v = f & TypeAttributes.VisibilityMask;
			switch (v)
			{
				case TypeAttributes.Public:
					return Visibility.Public;
				case TypeAttributes.NestedFamily:
					return Visibility.NestedProtected;
				case TypeAttributes.NestedAssembly:
					return Visibility.NestedInternal;
				case TypeAttributes.NestedFamORAssem:
				case TypeAttributes.NestedFamANDAssem:
					return Visibility.NestedProtectedInternal;
				case TypeAttributes.NestedPrivate:
					return Visibility.NestedPrivate;
			}
			return Visibility.Internal;
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Types; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IType this[string fullname]
		{
			get
			{
				IType type;
				if (_cache.TryGetValue(fullname, out type))
					return type;

				if (this.Any(item => _cache.TryGetValue(fullname, out type)))
				{
					return type;
				}

				return null;
			}
		}

		public void Add(IType type)
		{
			throw new NotSupportedException();
		}

		public bool Contains(IType type)
		{
			return type != null && this.Any(x => x == type);
		}
	}
}
