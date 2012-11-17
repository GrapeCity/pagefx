using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			var token = MdbIndex.MakeToken(MdbTableId.GenericParam, index);

			var param = new GenericParameter
				{
					Position = pos,
					Variance = ToVariance(flags),
					SpecialConstraints = ToSpecConstraints(flags),
					Name = row[MDB.GenericParam.Name].String,
					MetadataToken = token,
					ID = ++_id
				};

			param.Constraints = new Constraints(_loader, param, index);
			param.CustomAttributes = new CustomAttributes(_loader, param, token);

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
			private readonly int _ownerIndex;
			private readonly List<IType> _list = new List<IType>();
			private IType _baseType;
			private bool _resolveBaseType = true;
			private int _startIndex = -1;

			public Constraints(AssemblyLoader loader, IGenericParameter owner, int ownerIndex)
			{
				_loader = loader;
				_owner = owner;
				_ownerIndex = ownerIndex;
			}

			public IType BaseType
			{
				get
				{
					if (_resolveBaseType)
					{
						_baseType = Load(-1, true);
						_resolveBaseType = false;
					}
					return _baseType;
				}
			}

			public IEnumerator<IType> GetEnumerator()
			{
				for (int i = 0; i < Count; i++)
				{
					yield return this[i];
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get
				{
					Load(-1, false);
					return _list.Count;
				}
			}

			public IType this[int index]
			{
				get
				{
					if (index < 0) throw new ArgumentOutOfRangeException("index");
					if (index < _list.Count)
						return _list[index];
					return Load(index, false);
				}
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

			private IType Load(int index, bool baseType)
			{
				//TODO: possible subject for PERF

				var mdb = _loader.Mdb;
				int n = mdb.GetRowCount(MdbTableId.GenericParamConstraint);
				for (int i = _startIndex >= 0 ? _startIndex : 0; i < n; ++i)
				{
					var row = mdb.GetRow(MdbTableId.GenericParamConstraint, i);
					int owner = row[MDB.GenericParamConstraint.Owner].Index - 1;
					if (owner != _ownerIndex) continue;

					if (_startIndex < 0) _startIndex = i;

					MdbIndex cid = row[MDB.GenericParamConstraint.Constraint].Value;

					var constraint = _loader.GetTypeDefOrRef(cid, new Context(_owner));
					if (constraint == null)
						throw new BadMetadataException(string.Format("Invalid constraint index {0}", cid));

					if (constraint.TypeKind == TypeKind.Interface)
					{
						_list.Add(constraint);

						if (index >= 0 && index < _list.Count)
							return constraint;
					}
					else
					{
						if (baseType)
							return constraint;
					}
				}

				if (index >= 0)
					throw new ArgumentOutOfRangeException("index");

				return null;
			}
		}
	}
}
