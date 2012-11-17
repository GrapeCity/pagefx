using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal abstract class MetadataTable<T> : IMetadataTable<T> where T:class 
	{
		protected readonly AssemblyLoader Loader;
		
		protected MetadataTable(AssemblyLoader loader)
		{
			Loader = loader;
		}

		public abstract MdbTableId Id { get; }
		
		public int Count
		{
			get { return Loader.Mdb.GetRowCount(Id); }
		}

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException("index");

				var row = Loader.Mdb.GetRow(Id, index);
				
				if (row.Object == null)
					row.Object = ParseRow(row, index);
				
				return (T)row.Object;
			}
		}

		public int Load()
		{
			return this.Count();
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected MdbReader Mdb
		{
			get { return Loader.Mdb; }
		}

		protected abstract T ParseRow(MdbRow row, int index);
	}
}