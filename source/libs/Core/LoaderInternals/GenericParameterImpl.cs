using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class GenericParameterImpl : IType
	{
		private readonly AssemblyLoader _loader;
		private readonly GenericParamAttributes _flags;
		private ICustomAttributeCollection _customAttributes;
		private readonly ConstraintsImpl _constraints;
		private readonly SimpleIndex _owner;
		private readonly GenericParameterInfo _info;

		public GenericParameterImpl(AssemblyLoader loader, MetadataRow row, int index, long id)
		{
			_loader = loader;

			MetadataToken = SimpleIndex.MakeToken(TableId.GenericParam, index + 1);

			Name = row[Schema.GenericParam.Name].String;
			Id = id;
			var position = (int)row[Schema.GenericParam.Number].Value;

			_flags = (GenericParamAttributes)row[Schema.GenericParam.Flags].Value;
			_owner = row[Schema.GenericParam.Owner].Value;
			_constraints = new ConstraintsImpl(this);

			_info = new GenericParameterInfo(position, Variance, SpecialConstraints);
		}

		public int MetadataToken { get; private set; }

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _customAttributes ?? (_customAttributes = new CustomAttributes(_loader, this)); }
		}
		
		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Enumerable.Empty<ICodeNode>(); }
		}

		public object Data { get; set; }

		public string Documentation { get; set; }

		public IAssembly Assembly
		{
			get { return _loader.Assembly; }
		}

		public IModule Module
		{
			get { return Assembly.MainModule; }
		}

		public MemberType MemberType
		{
			get { return MemberType.Type; }
		}

		public string Name { get; private set; }

		public string FullName
		{
			get { return Name; }
		}

		public string DisplayName
		{
			get { return Name; }
		}

		public IType Type
		{
			get { return null; }
		}

		public Visibility Visibility
		{
			get { return Visibility.Public; }
		}

		public bool IsStatic
		{
			get { return false; }
		}

		public bool IsSpecialName
		{
			get { return false; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return false; }
		}

		public ITypeCollection Types
		{
			get { return TypeCollection.Empty; }
		}

		public string Namespace
		{
			get { return string.Empty; }
		}

		public TypeKind TypeKind
		{
			get { return TypeKind.GenericParameter; }
		}

		public bool IsAbstract { get { return false; } }
		public bool IsSealed { get { return false; } }
		public bool IsPartial { get { return false; } }
		public bool IsBeforeFieldInit { get { return false; } }
		public bool IsInterface { get { return false; } }
		public bool IsClass { get { return false; } }
		public bool IsArray { get { return false; } }
		public bool IsEnum { get { return false; } }

		public ITypeMember Owner
		{
			get { return _loader.GetTypeOrMethodDef(_owner); }
		}

		public IType DeclaringType
		{
			get { return Owner as IType; }
		}

		public IMethod DeclaringMethod
		{
			get { return Owner as IMethod; }
		}

		public IType BaseType
		{
			get { return _constraints.BaseType; }
		}

		public ITypeCollection Interfaces
		{
			get { return _constraints; }
		}

		public IType ElementType
		{
			get { return null; }
		}

		public IType ValueType
		{
			get { return null; }
		}

		public ITypeCollection GenericParameters
		{
			get { return TypeCollection.Empty; }
		}

		public ITypeCollection GenericArguments
		{
			get { return TypeCollection.Empty; }
		}

		public IFieldCollection Fields
		{
			get { return FieldCollection.Empty; }
		}

		public IMethodCollection Methods
		{
			get { return MethodCollection.Empty; }
		}

		public IPropertyCollection Properties
		{
			get { return PropertyCollection.Empty; }
		}

		public IEventCollection Events
		{
			get { return EventCollection.Empty; }
		}

		public ITypeMemberCollection Members
		{
			get { return TypeMemberCollection.Empty; }
		}

		public ClassLayout Layout
		{
			get { return null; }
		}

		public string Key
		{
			get { return Name + Id; }
		}

		public string SigName
		{
			get { return Name; }
		}

		public string NestedName
		{
			get { return Name; }
		}

		public ArrayDimensionCollection ArrayDimensions
		{
			get { return null; }
		}

		public GenericParameterInfo GetGenericParameterInfo()
		{
			return _info;
		}

		public long Id { get; private set; }

		public GenericParameterVariance Variance
		{
			get
			{
				var variance = _flags & GenericParamAttributes.VarianceMask;
				switch (variance)
				{
					case GenericParamAttributes.Covariant:
						return GenericParameterVariance.Covariant;

					case GenericParamAttributes.Contravariant:
						return GenericParameterVariance.Contravariant;
				}
				return GenericParameterVariance.NonVariant;
			}
		}

		public GenericParameterSpecialConstraints SpecialConstraints
		{
			get
			{
				var v = GenericParameterSpecialConstraints.None;
				var sc = _flags & GenericParamAttributes.SpecialConstraintMask;
				if ((sc & GenericParamAttributes.DefaultConstructorConstraint) != 0)
					v |= GenericParameterSpecialConstraints.DefaultConstructor;
				if ((sc & GenericParamAttributes.ReferenceTypeConstraint) != 0)
					v |= GenericParameterSpecialConstraints.ReferenceType;
				if ((sc & GenericParamAttributes.NotNullableValueTypeConstraint) != 0)
					v |= GenericParameterSpecialConstraints.ValueType;
				return v;
			}
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Name;
		}

		private sealed class ConstraintsImpl : IGenericParameterConstraints
		{
			private readonly GenericParameterImpl _owner;
			private IReadOnlyList<IType> _list;
			private IType _baseType;
			private bool _resolveBaseType = true;

			public ConstraintsImpl(GenericParameterImpl owner)
			{
				_owner = owner;
			}

			private int OwnerIndex
			{
				get { return _owner.RowIndex(); }
			}

			public IType BaseType
			{
				get
				{
					if (_resolveBaseType && _baseType == null)
					{
						_resolveBaseType = false;
						foreach (var type in List)
						{
							if (_baseType != null)
								break;
						}
					}
					return _baseType;
				}
			}

			public IEnumerator<IType> GetEnumerator()
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

			public IType this[int index]
			{
				get { return List[index]; }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return this.Cast<ICodeNode>(); }
			}

			public object Data { get; set; }

			public IType FindType(string fullname)
			{
				return this.FirstOrDefault(t => t.FullName == fullname);
			}

			public void Add(IType type)
			{
				throw new NotSupportedException();
			}

			public bool Contains(IType type)
			{
				return type != null && this.Any(x => ReferenceEquals(x, type));
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return SyntaxFormatter.Format(this, format, formatProvider);
			}

			private IReadOnlyList<IType> List
			{
				get { return _list ?? (_list = Populate().Memoize()); }
			}

			private IEnumerable<IType> Populate()
			{
				var mdb = _owner._loader.Metadata;
				var rows = mdb.LookupRows(TableId.GenericParamConstraint, Schema.GenericParamConstraint.Owner, OwnerIndex, true);

				foreach (var row in rows)
				{
					SimpleIndex cid = row[Schema.GenericParamConstraint.Constraint].Value;

					var constraint = _owner._loader.GetTypeDefOrRef(cid, CreateContext());
					if (constraint == null)
						throw new BadMetadataException(string.Format("Invalid constraint index {0}", cid));

					if (constraint.TypeKind == TypeKind.Interface)
					{
						yield return constraint;
					}
					else
					{
						if (_baseType == null)
							_baseType = constraint;
					}
				}
			}

			private Context CreateContext()
			{
				var type = _owner.DeclaringType;
				if (type != null)
				{
					return new Context(type);
				}

				var method = _owner.DeclaringMethod;
				if (method == null || method.DeclaringType == null)
				{
					throw new InvalidOperationException("Invalid context!");
				}

				return new Context(method.DeclaringType, method);
			}
		}
	}
}
