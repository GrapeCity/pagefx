using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Metadata;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.LoaderInternals.Collections;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal sealed class TypeTable : MetadataTable<IType>, ITypeCollection
	{
		private readonly Dictionary<string, IType> _cache = new Dictionary<string, IType>();
		private readonly Dictionary<int, int> _methodDeclTypeLookup = new Dictionary<int, int>();
		private readonly Dictionary<int, int> _fieldDeclTypeLookup = new Dictionary<int, int>();
		private int _lastIndex;

		public TypeTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.TypeDef; }
		}

		#region ResolveDeclType

		internal IType ResolveDeclType(ITypeMember member)
		{
			if (!(member is IMethod || member is IField))
				throw new InvalidOperationException();

			int index = member.RowIndex();

			bool isMethod = member is IMethod;
			var lookup = isMethod ? _methodDeclTypeLookup : _fieldDeclTypeLookup;
			int typeIndex;
			if (lookup.TryGetValue(index, out typeIndex))
			{
				return this[typeIndex];
			}

			var mdb = Metadata;
			int typeCount = mdb.GetRowCount(TableId.TypeDef);
			for (; _lastIndex < typeCount; _lastIndex++)
			{
				var row = mdb.GetRow(TableId.TypeDef, _lastIndex);
				var nextRow = _lastIndex + 1 < typeCount ? mdb.GetRow(TableId.TypeDef, _lastIndex + 1) : null;

				var methodRange = GetMethodRange(row, nextRow);
				var fieldRange = GetFieldRange(row, nextRow);

				if (methodRange != null)
				{
					PutMethodRange(methodRange, _lastIndex);
				}

				if (fieldRange != null)
				{
					PutFieldRange(fieldRange, _lastIndex);
				}

				if (isMethod && methodRange != null && index >= methodRange[0] && index < methodRange[1])
				{
					return this[_lastIndex++];
				}
				if (fieldRange != null && index >= fieldRange[0] && index < fieldRange[1])
				{
					return this[_lastIndex++];
				}
			}

			return null;
		}

		private void PutMethodRange(int[] range, int typeIndex)
		{
			bool first = true;
			foreach (var methodIndex in GetRange(range, TableId.MethodDef))
			{
				if (first && _methodDeclTypeLookup.ContainsKey(methodIndex))
				{
					// break since all range should be cached at once
					break;
				}
				first = false;
				_methodDeclTypeLookup.Add(methodIndex, typeIndex);
			}
		}

		private void PutFieldRange(int[] range, int typeIndex)
		{
			bool first = true;
			foreach (var fieldIndex in GetRange(range, TableId.Field))
			{
				if (first && _fieldDeclTypeLookup.ContainsKey(fieldIndex))
				{
					// break since all range should be cached at once
					break;
				}
				first = false;
				_fieldDeclTypeLookup.Add(fieldIndex, typeIndex);
			}
		}

		private IEnumerable<int> GetRange(int[] range, TableId tableId)
		{
			var n = Metadata.GetRowCount(tableId);
			for (int i = range[0]; i < n && i < range[1]; i++)
			{
				yield return i;
			}
		}

		#endregion

		protected override void OnLoaded(IType item)
		{
			Loader.FireTypeLoaded(item);
		}

		protected override IType ParseRow(MetadataRow row, int index)
		{
			var flags = (TypeAttributes)row[Schema.TypeDef.Flags].Value;
			string name = row[Schema.TypeDef.TypeName].String;
			string ns = row[Schema.TypeDef.TypeNamespace].String;

			var token = SimpleIndex.MakeToken(TableId.TypeDef, index + 1);
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
			}

			// to avoid problems with self refs in fields/methods,etc
			row.Object = type;

			type.MetadataToken = token;
			type.CustomAttributes = new CustomAttributes(Loader, type);

			//TODO: lazy resolving of class layout
			type.Layout = ResolveLayout(Metadata, index);

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

			var nextRow = index + 1 < Count ? Metadata.GetRow(TableId.TypeDef, index + 1) : null;
			SetMembers(row, nextRow, type);

			//TODO: lazy resolving of base type
			SetBaseType(row, type);

			type.Interfaces = new InterfaceImpl(Loader, type);

			LoadMethodImpl(type, index);

			return type;
		}

		private void SetBaseType(MetadataRow row, IType type)
		{
			if (type.FullName == "System.Object") return;

			SimpleIndex baseIndex = row[Schema.TypeDef.Extends].Value;

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

		private static ClassLayout ResolveLayout(MetadataReader metadata, int typeIndex)
		{
			var row = metadata.LookupRow(TableId.ClassLayout, Schema.ClassLayout.Parent, typeIndex, true);
			if (row == null)
				return null;

			var size = (int)row[Schema.ClassLayout.ClassSize].Value;
			var pack = (int)row[Schema.ClassLayout.PackingSize].Value;
			return new ClassLayout(size, pack);
		}

		private IType ResolveDeclaringType(int index)
		{
			var row = Metadata.LookupRow(TableId.NestedClass, Schema.NestedClass.Class, index, true);
			if (row == null) return null;

			int enclosingIndex = row[Schema.NestedClass.EnclosingClass].Index - 1;
			return this[enclosingIndex];
		}

		private void SetMembers(MetadataRow row, MetadataRow nextRow, IType type)
		{
			var fields = GetFields(row, nextRow, type);
			var methods = GetMethods(row, nextRow, type);

			var members = (TypeMemberCollection)type.Members;
			members.Fields = fields;			
			members.Methods = methods;
			members.Properties = new PropertyList(type);
			members.Events = new EventList(type);
		}

		private IFieldCollection GetFields(MetadataRow row, MetadataRow nextRow, IType type)
		{
			var range = GetFieldRange(row, nextRow);
			if (range == null) return FieldCollection.Empty;

			PutFieldRange(range, type.RowIndex());

			return new FieldList(Loader, type, range[0], range[1]);
		}

		private IMethodCollection GetMethods(MetadataRow row, MetadataRow nextRow, IType type)
		{
			var range = GetMethodRange(row, nextRow);
			if (range == null) return MethodCollection.Empty;

			PutMethodRange(range, type.RowIndex());

			return new MethodList(Loader, type, range[0], range[1]);
		}

		private int[] GetMethodRange(MetadataRow row, MetadataRow nextRow)
		{
			int from = row[Schema.TypeDef.MethodList].Index - 1;
			if (from < 0) return null;

			int to = Metadata.GetRowCount(TableId.MethodDef);
			if (nextRow != null)
			{
				to = nextRow[Schema.TypeDef.MethodList].Index - 1;
			}

			return from == to ? null : new[] { @from, to };
		}

		private int[] GetFieldRange(MetadataRow row, MetadataRow nextRow)
		{
			int from = row[Schema.TypeDef.FieldList].Index - 1;
			if (from < 0) return null;

			int to = Metadata.GetRowCount(TableId.Field);
			if (nextRow != null)
			{
				to = nextRow[Schema.TypeDef.FieldList].Index - 1;
			}

			return from == to ? null : new[] { @from, to };
		}

		private void LoadMethodImpl(IType type, int typeIndex)
		{
			//TODO: try to do lazy loading
			var rows = Metadata.LookupRows(TableId.MethodImpl, Schema.MethodImpl.Class, typeIndex, true);
			foreach (var row in rows)
			{
				SimpleIndex bodyIdx = row[Schema.MethodImpl.MethodBody].Value;
				SimpleIndex declIdx = row[Schema.MethodImpl.MethodDeclaration].Value;

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
			return type != null && this.Any(x => ReferenceEquals(x, type));
		}
	}
}
