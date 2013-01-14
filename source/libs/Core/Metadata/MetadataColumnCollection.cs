using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
	/// List of <see cref="MetadataColumn"/>s.
	/// </summary>
	internal sealed class MetadataColumnCollection : IReadOnlyList<MetadataColumn>
	{
		private readonly List<MetadataColumn> _list = new List<MetadataColumn>();

		/// <summary>
		/// Gets the number of columns.
		/// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

		public MetadataColumn this[int index]
		{
			get { return _list[index]; }
		}

		internal void Add(MetadataColumn col)
		{
			_list.Add(col);
		}

		public override string ToString()
		{
			return _list.Join("(", ")", ", ", x => x.Name);
		}

		public IEnumerator<MetadataColumn> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
	}
}