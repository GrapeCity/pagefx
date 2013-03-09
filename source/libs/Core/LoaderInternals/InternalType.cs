using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalType : MemberBase, IType
	{
		private static readonly Dictionary<string, TypeKind> TypeKindByBase =
			new Dictionary<string, TypeKind>
				{
					{"System.Enum", TypeKind.Enum},
					{"System.ValueType", TypeKind.Struct},
					{"System.Delegate", TypeKind.Delegate},
					{"System.MulticastDelegate", TypeKind.Delegate},
				};

		private string _namespace;
		private readonly TypeAttributes _flags;
		private readonly TypeMemberCollection _members;
		private IType _baseType;
		private IType _declaringType;
		private IGenericParameterCollection _genericParams;
		private ITypeCollection _interfaces;
		private ClassLayout _layout;
		private ITypeCollection _nestedTypes;
		private bool _baseTypeResolved;
		
		public InternalType(AssemblyLoader loader, MetadataRow row, int index)
			: base(loader, TableId.TypeDef, index)
		{
			_flags = (TypeAttributes)row[Schema.TypeDef.Flags].Value;

			Namespace = row[Schema.TypeDef.TypeNamespace].String;
			Name = row[Schema.TypeDef.TypeName].String;

			var typeTable = Loader.Types;
			var nextRow = index + 1 < typeTable.Count ? Loader.Metadata.GetRow(TableId.TypeDef, index + 1) : null;
			var fields = typeTable.GetFields(row, nextRow, this);
			var methods = typeTable.GetMethods(row, nextRow, this);

			_members = new TypeMemberCollection(fields, methods, new PropertyList(this), new EventList(this));
		}

		public MemberType MemberType
		{
			get { return MemberType.Type; }
		}

		public string Namespace
		{
			get
			{
				if (DeclaringType != null)
					return DeclaringType.Namespace;
				return _namespace;
			}
			private set { _namespace = value; }
		}

		public string FullName
		{
			get { return this.BuildFullName(); }
		}

		public override string DisplayName
		{
			get { return this.BuildDisplayName(); }
		}

		public string Key
		{
			get { return FullName; }
		}

		public string SigName
		{
			get { return this.BuildSigName(); }
		}

		public string NestedName
		{
			get { return this.BuildNestedName(); }
		}

		public IType DeclaringType
		{
			get { return _declaringType ?? (_declaringType = ResolveDeclaringType()); }
		}

		public IType Type
		{
			get { return null; }
		}

		public Visibility Visibility
		{
			get
			{
				var v = _flags & TypeAttributes.VisibilityMask;
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
		}

		public TypeKind TypeKind
		{
			get
			{
				if ((_flags & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface)
				{
					return TypeKind.Interface;
				}

				var sysType = this.SystemType();
				if (sysType != null)
					return sysType.Kind;

				var baseType = BaseType;
				if (baseType != null)
				{
					TypeKind kind;
					if (TypeKindByBase.TryGetValue(baseType.FullName, out kind))
					{
						return kind;
					}
				}

				return TypeKind.Class;
			}
		}

		public bool IsStatic
		{
			get { return false; }
		}

		public bool IsAbstract
		{
			get { return (_flags & TypeAttributes.Abstract) != 0; }
		}

		public bool IsSealed
		{
			get { return (_flags & TypeAttributes.Sealed) != 0; }
		}

		public bool IsBeforeFieldInit
		{
			get { return (_flags & TypeAttributes.BeforeFieldInit) != 0; }
		}

		public bool IsSpecialName
		{
			get { return (_flags & TypeAttributes.SpecialName) != 0; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & TypeAttributes.RTSpecialName) != 0; }
		}

		public bool IsPartial
		{
			get { return false; }
		}

		public bool IsInterface
		{
			get { return TypeKind == TypeKind.Interface; }
		}

		public bool IsClass
		{
			get { return TypeKind == TypeKind.Class; }
		}

		public bool IsArray
		{
			get { return false; }
		}

		public bool IsEnum
		{
			get { return TypeKind == TypeKind.Enum; }
		}

		public IMethod DeclaringMethod
		{
			get { return null; }
		}

		public IType BaseType
		{
			get { return _baseType ?? (_baseType = ResolveBaseType()); }
		}

		public ITypeCollection Interfaces
		{
			get { return _interfaces ?? (_interfaces = new InterfaceImpl(Loader, this)); }
		}

		public IType ValueType
		{
			get { return this.ResolveValueType(); }
		}

		public IGenericParameterCollection GenericParameters
		{
			get { return _genericParams ?? (_genericParams = new GenericParamList(Loader, this)); }
		}

		public IFieldCollection Fields
		{
			get { return _members.Fields; }
		}

		public IMethodCollection Methods
		{
			get { return _members.Methods; }
		}

		public IPropertyCollection Properties
		{
			get { return _members.Properties; }
		}

		public IEventCollection Events
		{
			get { return _members.Events; }
		}

		public ITypeMemberCollection Members
		{
			get { return _members; }
		}

		public ClassLayout Layout
		{
			get { return _layout ?? (_layout = ResolveLayout()); }
		}

		public string Documentation { get; set; }

		public ITypeCollection Types
		{
			get { return _nestedTypes ?? (_nestedTypes = new NestedTypeList(Loader, this)); }
		}

		private IType ResolveBaseType()
		{
			if (_baseTypeResolved) return null;

			_baseTypeResolved = true;

			if (this.Is(SystemTypeCode.Object)) return null;

			var row = Loader.Metadata.GetRow(TableId.TypeDef, this.RowIndex());

			SimpleIndex baseIndex = row[Schema.TypeDef.Extends].Value;

			return Loader.GetTypeDefOrRef(baseIndex, new Context(this));
		}

		private IType ResolveDeclaringType()
		{
			var row = Loader.Metadata.LookupRow(TableId.NestedClass, Schema.NestedClass.Class, this.RowIndex(), true);
			if (row == null) return null;

			int enclosingIndex = row[Schema.NestedClass.EnclosingClass].Index - 1;
			return Loader.Types[enclosingIndex];
		}

		private ClassLayout ResolveLayout()
		{
			if ((_flags & TypeAttributes.LayoutMask) != TypeAttributes.ExplicitLayout)
				return null;

			var row = Loader.Metadata.LookupRow(TableId.ClassLayout, Schema.ClassLayout.Parent, this.RowIndex(), true);
			if (row == null) return null;

			var size = (int)row[Schema.ClassLayout.ClassSize].Value;
			var pack = (int)row[Schema.ClassLayout.PackingSize].Value;
			return new ClassLayout(size, pack);
		}
	}
}