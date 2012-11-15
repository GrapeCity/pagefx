using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal abstract class MetadataTable<T> : IMetadataTable<T> where T:class 
	{
		protected readonly AssemblyLoader Loader;
		private readonly T[] _rows;

		protected MetadataTable(AssemblyLoader loader, MdbTableId tableId)
		{
			Loader = loader;

			int rowCount = loader.Mdb.GetRowCount(tableId);

			_rows = new T[rowCount];
		}

		public abstract MdbTableId Id { get; }
		
		public int Count
		{
			get { return _rows.Length; }
		}

		public T this[int index]
		{
			get { return _rows[index] ?? (_rows[index] = ParseRow(index)); }
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

		protected abstract T ParseRow(int index);
	}
}