﻿using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class GenericParamTable
	{
		private readonly AssemblyLoader _loader;
		private long _id;
		private IType[] _array;

		public GenericParamTable(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public IType this[int index]
		{
			get
			{
				if (_array == null)
				{
					int n = _loader.Metadata.GetRowCount(TableId.GenericParam);
					_array = new IType[n];
				}
				return _array[index] ?? (_array[index] = Create(index));
			}
		}

		public IList<IType> Find(SimpleIndex target)
		{
			var rows = _loader.Metadata.LookupRows(TableId.GenericParam, Schema.GenericParam.Owner, target, false);
			return rows.Select(x => this[x.Index]).ToList().AsReadOnly();
		}

		private IType Create(int index)
		{
			var row = _loader.Metadata.GetRow(TableId.GenericParam, index);

			return new GenericParameterImpl(_loader, row, index, NextId());
		}

		internal long NextId()
		{
			var corlib = _loader.CorlibLoader;
			return ++corlib.GenericParameters._id;
		}
	}
}
