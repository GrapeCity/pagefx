using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	internal sealed class GuidHeap
	{
		private readonly BufferedBinaryReader _heap;
		private IReadOnlyList<Guid> _list;

		public GuidHeap(BufferedBinaryReader heap)
		{
			_heap = heap;
		}

		public Guid Fetch(int index)
		{
			return List[index];
		}

		private IReadOnlyList<Guid> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<Guid> Populate()
		{
			_heap.Seek(0, SeekOrigin.Begin);

			long size = _heap.Length;
			while (size > 0)
			{
				var guid = _heap.ReadBytes(16);
				yield return new Guid(guid);
				size -= 16;
			}
		}
	}
}