using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal abstract class MetadataTable<T> : IMetadataTable<T> where T:class 
	{
		protected readonly AssemblyLoader Loader;
		
		protected MetadataTable(AssemblyLoader loader)
		{
			Loader = loader;
		}

		public abstract TableId Id { get; }
		
		public int Count
		{
			get { return Loader.Metadata.GetRowCount(Id); }
		}

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException("index");

				var row = Loader.Metadata.GetRow(Id, index);

				if (row.Object == null)
				{
					var item = ParseRow(row, index);

					row.Object = item;

					OnLoaded(item);
				}

				return (T)row.Object;
			}
		}

		protected virtual void OnLoaded(T item)
		{
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

		protected MetadataReader Metadata
		{
			get { return Loader.Metadata; }
		}

		protected abstract T ParseRow(MetadataRow row, int index);
	}
}