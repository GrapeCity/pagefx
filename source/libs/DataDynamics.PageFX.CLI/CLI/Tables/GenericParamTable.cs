using System.Collections.Generic;
using System.Collections.ObjectModel;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal sealed class GenericParamTable
	{
		private readonly MdbReader _mdb;
		private static readonly ReadOnlyCollection<IGenericParameter> Empty = new List<IGenericParameter>().AsReadOnly();
		private readonly Dictionary<MdbIndex, IList<IGenericParameter>> _cache = new Dictionary<MdbIndex, IList<IGenericParameter>>();
		private int _lastIndex;
		private static long _id;
		private readonly IGenericParameter[] _array;

		public GenericParamTable(MdbReader mdb)
		{
			_mdb = mdb;

			int n = mdb.GetRowCount(MdbTableId.GenericParam);
			_array = new IGenericParameter[n];
		}

		public IGenericParameter this[int index]
		{
			get { return _array[index] ?? (_array[index] = Create(index)); }
		}

		public IList<IGenericParameter> Find(MdbIndex target)
		{
			IList<IGenericParameter> result;
			if (_cache.TryGetValue(target, out result))
				return result;

			var list = new List<IGenericParameter>();

			int n = _mdb.GetRowCount(MdbTableId.GenericParam);
			for (; _lastIndex < n; _lastIndex++)
			{
				var row = _mdb.GetRow(MdbTableId.GenericParam, _lastIndex);
				MdbIndex currentOwner = row[MDB.GenericParam.Owner].Value;

				list.Add(this[_lastIndex]);

				for (_lastIndex++; _lastIndex < n; _lastIndex++)
				{
					row = _mdb.GetRow(MdbTableId.GenericParam, _lastIndex);
					MdbIndex owner = row[MDB.GenericParam.Owner].Value;
					if (owner != currentOwner)
					{
						break;
					}

					list.Add(this[_lastIndex]);
				}

				result = list.AsReadOnly();
				_cache.Add(currentOwner, result);

				if (currentOwner == target)
				{
					return result;
				}

				_lastIndex--;
				list = new List<IGenericParameter>();
			}
			
			return Empty;
		}

		private IGenericParameter Create(int index)
		{
			var row = _mdb.GetRow(MdbTableId.GenericParam, index);

			var pos = (int)row[MDB.GenericParam.Number].Value;
			var flags = (GenericParamAttributes)row[MDB.GenericParam.Flags].Value;

			return new GenericParameter
				{
					Position = pos,
					Variance = ToVariance(flags),
					SpecialConstraints = ToSpecConstraints(flags),
					Name = row[MDB.GenericParam.Name].String,
					MetadataToken = MdbIndex.MakeToken(MdbTableId.GenericParam, index),
					ID = ++_id
				};
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
	}
}
