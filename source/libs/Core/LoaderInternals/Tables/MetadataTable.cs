using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal abstract class MetadataTable<T> : IMetadataTable<T> where T:class 
	{
		protected readonly AssemblyLoader Loader;
		private T[] _rows;
		
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
				int count = Count;
				if (index < 0 || index >= count)
					throw new ArgumentOutOfRangeException("index");

				if (_rows == null)
				{
					_rows = new T[count];
				}

				var row = Loader.Metadata.GetRow(Id, index);

				if (_rows[index] == null)
				{
					var item = ParseRow(row, index);

					_rows[index] = item;

					OnLoaded(item);
				}

				return _rows[index];
			}
			protected set
			{
				_rows[index] = value;
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