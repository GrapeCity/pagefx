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

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class GenericParamTable
	{
		private readonly AssemblyLoader _loader;
		private long _id;
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
					int n = _loader.Metadata.GetRowCount(TableId.GenericParam);
					_array = new IGenericParameter[n];
				}
				return _array[index] ?? (_array[index] = Create(index));
			}
		}

		public IList<IGenericParameter> Find(SimpleIndex target)
		{
			var rows = _loader.Metadata.LookupRows(TableId.GenericParam, Schema.GenericParam.Owner, target, false);
			return rows.Select(x => this[x.Index]).ToList().AsReadOnly();
		}

		private IGenericParameter Create(int index)
		{
			var row = _loader.Metadata.GetRow(TableId.GenericParam, index);

			return new GenericParameterImpl(_loader, row, index, NextId());
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

		internal long NextId()
		{
			var corlib = _loader.CorlibLoader;
			return ++corlib.GenericParameters._id;
		}
	}
}
