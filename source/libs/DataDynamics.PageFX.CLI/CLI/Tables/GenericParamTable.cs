﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class GenericParamTable
	{
		private readonly AssemblyLoader _loader;
		private static long _id;
		private IGenericParameter[] _array;

		public GenericParamTable(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public IGenericParameter this[int index]
		{
			get
			{
				if (_array == null)
				{
					int n = _loader.Mdb.GetRowCount(MdbTableId.GenericParam);
					_array = new IGenericParameter[n];
				}
				return _array[index] ?? (_array[index] = Create(index));
			}
		}

		public IList<IGenericParameter> Find(MdbIndex target)
		{
			var rows = _loader.Mdb.LookupRows(MdbTableId.GenericParam, MDB.GenericParam.Owner, target, false);
			return rows.Select(x => this[x.Index]).ToList().AsReadOnly();
		}

		private IGenericParameter Create(int index)
		{
			var row = _loader.Mdb.GetRow(MdbTableId.GenericParam, index);

			var pos = (int)row[MDB.GenericParam.Number].Value;
			var flags = (GenericParamAttributes)row[MDB.GenericParam.Flags].Value;
			var token = MdbIndex.MakeToken(MdbTableId.GenericParam, index + 1);

			var param = new GenericParameter
				{
					Position = pos,
					Variance = ToVariance(flags),
					SpecialConstraints = ToSpecConstraints(flags),
					Name = row[MDB.GenericParam.Name].String,
					MetadataToken = token,
					ID = ++_id
				};

			param.Constraints = new Constraints(_loader, param);
			param.CustomAttributes = new CustomAttributes(_loader, param);

			return param;
		}

		private static GenericParameterVariance ToVariance(GenericParamAttributes flags)
		{
			var variance = flags & GenericParamAttributes.VarianceMask;
			switch (variance)
			{
				case GenericParamAttributes.Covariant:
					return GenericParameterVariance.Covariant;

				case GenericParamAttributes.Contravariant:
					return GenericParameterVariance.Contravariant;
			}
			return GenericParameterVariance.NonVariant;
		}

		private static GenericParameterSpecialConstraints ToSpecConstraints(GenericParamAttributes flags)
		{
			var v = GenericParameterSpecialConstraints.None;
			var sc = flags & GenericParamAttributes.SpecialConstraintMask;
			if ((sc & GenericParamAttributes.DefaultConstructorConstraint) != 0)
				v |= GenericParameterSpecialConstraints.DefaultConstructor;
			if ((sc & GenericParamAttributes.ReferenceTypeConstraint) != 0)
				v |= GenericParameterSpecialConstraints.ReferenceType;
			if ((sc & GenericParamAttributes.NotNullableValueTypeConstraint) != 0)
				v |= GenericParameterSpecialConstraints.ValueType;
			return v;
		}

		public static void ResetId()
		{
			_id = 0;
		}

		private sealed class Constraints : IGenericParameterConstraints
		{
			private readonly AssemblyLoader _loader;
			private readonly IGenericParameter _owner;
			private IReadOnlyList<IType> _list;
			private IType _baseType;
			private bool _resolveBaseType = true;

			public Constraints(AssemblyLoader loader, IGenericParameter owner)
			{
				_loader = loader;
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
				get { return this.FirstOrDefault(t => t.FullName == fullname); }
			}

			public void Add(IType type)
			{
				throw new NotSupportedException();
			}

			public bool Contains(IType type)
			{
				return type != null && this.Any(x => x == type);
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
				var mdb = _loader.Mdb;
				var rows = mdb.LookupRows(MdbTableId.GenericParamConstraint, MDB.GenericParamConstraint.Owner, OwnerIndex, true);

				foreach (var row in rows)
				{
					MdbIndex cid = row[MDB.GenericParamConstraint.Constraint].Value;

					var constraint = _loader.GetTypeDefOrRef(cid, new Context(_owner));
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
		}
	}
}
