using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class TypeTable : MetadataTable<IType>, ITypeCollection
	{
		private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
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

		private sealed class TypeInfo
		{
			public int[] MethodRange;
			public int[] FieldRange;
			public string FullName;
		}

		internal IType ResolveDeclType(ITypeMember member)
		{
			if (!(member is IMethod || member is IField))
				throw new InvalidOperationException();

			int memberIndex = member.RowIndex();

			var typeIndex = ResolveDeclTypeIndex(memberIndex, member is IMethod);

			return typeIndex >= 0 ? this[typeIndex] : null;
		}

		internal int ResolveDeclTypeIndex(int memberIndex, bool isMethod)
		{
			var lookup = isMethod ? _methodDeclTypeLookup : _fieldDeclTypeLookup;
			int typeIndex;
			if (lookup.TryGetValue(memberIndex, out typeIndex))
			{
				return typeIndex;
			}

			int typeCount = Metadata.GetRowCount(TableId.TypeDef);
			for (; _lastIndex < typeCount; _lastIndex++)
			{
				var info = GetTypeInfo(_lastIndex);

				var range = isMethod ? info.MethodRange : info.FieldRange;
				if (range != null && memberIndex >= range[0] && memberIndex < range[1])
				{
					return _lastIndex++;
				}
			}

			return -1;
		}

		private TypeInfo GetTypeInfo(int index)
		{
			int typeCount = Metadata.GetRowCount(TableId.TypeDef);
			var row = Metadata.GetRow(TableId.TypeDef, index);
			var nextRow = index + 1 < typeCount ? Metadata.GetRow(TableId.TypeDef, index + 1) : null;

			var methodRange = GetMethodRange(row, nextRow);
			var fieldRange = GetFieldRange(row, nextRow);

			if (methodRange != null)
			{
				PutMethodRange(methodRange, index);
			}

			if (fieldRange != null)
			{
				PutFieldRange(fieldRange, index);
			}

			var fullName = GetFullName(index);

			if (!_cache.ContainsKey(fullName))
			{
				_cache.Add(fullName, index);
			}

			return new TypeInfo
				{
					MethodRange = methodRange,
					FieldRange = fieldRange,
					FullName = fullName
				};
		}
		
		private void PutMethodRange(int[] range, int typeIndex)
		{
			bool first = true;
			foreach (var methodIndex in GetRange(TableId.MethodDef, range))
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
			foreach (var fieldIndex in GetRange(TableId.Field, range))
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

		private IEnumerable<int> GetRange(TableId tableId, int[] range)
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
			var sysType = !isIface && ns == SystemType.Namespace ? SystemTypes.Find(name) : null;
			if (sysType != null) kind = sysType.Kind;

			var type = CreateType(kind, genericParams);

			type.Namespace = ns;
			type.Name = name;
			SetTypeFlags(type, flags);
			type.Module = Loader.MainModule;

			// to avoid problems with self refs in fields/methods,etc
			this[index] = type;

			type.MetadataToken = token;
			type.CustomAttributes = new CustomAttributes(Loader, type);

			//TODO: lazy resolving of class layout
			type.Layout = ResolveLayout(Metadata, index);

			//TODO: lazy resolving of declaring type
			var declType = ResolveDeclaringType(index);
			if (declType != null)
			{
				type.DeclaringType = declType;
			}
			else
			{
				_cache[type.FullName] = type;
			}

			var nextRow = index + 1 < Count ? Metadata.GetRow(TableId.TypeDef, index + 1) : null;
			SetMembers(row, nextRow, type);

			//TODO: lazy resolving of base type
			SetBaseType(row, type);

			type.Interfaces = new InterfaceImpl(Loader, type);
			type.Types = new NestedTypeList(Loader, type);

			return type;
		}

		private void SetBaseType(MetadataRow row, IType type)
		{
			if (type.FullName == "System.Object") return;

			SimpleIndex baseIndex = row[Schema.TypeDef.Extends].Value;

			var baseType = Loader.GetTypeDefOrRef(baseIndex, new Context(type));
			type.BaseType = baseType;

			var thisType = type as TypeImpl;
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

		public string GetFullName(int index)
		{
			var sb = new StringBuilder();
			GetFullName(sb, index);
			return sb.ToString();
		}

		private void GetFullName(StringBuilder sb, int index)
		{
			bool isNested = false;
			var row = Metadata.LookupRow(TableId.NestedClass, Schema.NestedClass.Class, index, true);
			if (row != null)
			{
				isNested = true;
				int enclosingIndex = row[Schema.NestedClass.EnclosingClass].Index - 1;
				GetFullName(sb, enclosingIndex);
				sb.Append("+");
			}

			row = Metadata.GetRow(TableId.TypeDef, index);
			if (!isNested)
			{
				string ns = row[Schema.TypeDef.TypeNamespace].String;
				if (!string.IsNullOrEmpty(ns))
				{
					sb.Append(ns);
					sb.Append(".");
				}
			}
			
			string name = row[Schema.TypeDef.TypeName].String;
			sb.Append(name);
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

			return from == to ? null : new[] { from, to };
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

			return from == to ? null : new[] { from, to };
		}

		private static TypeImpl CreateType(TypeKind kind, IList<IGenericParameter> genericParameters)
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
			return new TypeImpl(kind);
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

		public object Data { get; set; }

		public IType FindType(string fullname)
		{
			if (string.IsNullOrEmpty(fullname))
				return null;

			var type = FindInCache(fullname);
			if (type != null)
				return type;

			int count = Count;
			for (; _lastIndex < count; _lastIndex++)
			{
				var info = GetTypeInfo(_lastIndex);
				if (info.FullName == fullname)
				{
					return this[_lastIndex++];
				}
			}

			return null;
		}

		private IType FindInCache(string fullname)
		{
			object value;
			if (_cache.TryGetValue(fullname, out value))
			{
				var type = value as IType;
				if (type != null)
				{
					return type;
				}

				var index = (int)value;
				type = this[index];
				_cache[fullname] = type;

				return type;
			}

			return null;
		}

		public void Add(IType type)
		{
			throw new NotSupportedException();
		}

		public bool Contains(IType type)
		{
			return type != null && this.Any(x => ReferenceEquals(x, type));
		}

		private IReadOnlyList<IType> _exposedTypes;

		public IReadOnlyList<IType> GetExposedTypes()
		{
			return _exposedTypes ?? (_exposedTypes = PopulateExposedTypes().Memoize());
		}

		private IEnumerable<IType> PopulateExposedTypes()
		{
			var count = Count;
			for (int i = 0; i < count; i++)
			{
				if (IsExposed(i))
					yield return this[i];
			}
		}

		private static readonly string[] ExposeAttributes =
			{
				"ExposeAttribute..ctor",
				"NUnit.Framework.TestFixtureAttribute..ctor"
			};

		private bool IsExposed(int index)
		{
			//TODO: return true when base type is NUnit.Framework.Assertion or NUnit.Framework.TestCase

			var token = SimpleIndex.MakeToken(TableId.TypeDef, index + 1);
			var rows = Metadata.LookupRows(TableId.CustomAttribute, Schema.CustomAttribute.Parent, token, false);

			return rows.Select(x => CustomAttributes.GetFullName(Loader, x)).Any(name => ExposeAttributes.Contains(name));
		}
	}
}
