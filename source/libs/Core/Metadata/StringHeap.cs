using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Core.Metadata
{
	internal sealed class StringHeap
	{
		private readonly BufferedBinaryReader _heap;
		private readonly Dictionary<uint, string> _cache = new Dictionary<uint, string>();

		public StringHeap(BufferedBinaryReader heap)
		{
			_heap = heap;
		}

		public string Fetch(uint offset)
		{
			if (offset >= _heap.Length)
				throw new BadMetadataException("Invalid #Strings heap index.");

			string value;
			if (_cache.TryGetValue(offset, out value))
				return value;

			_heap.Seek(offset, SeekOrigin.Begin);

			value = _heap.ReadUtf8();
			_cache.Add(offset, value);

			return value;
		}
	}
}