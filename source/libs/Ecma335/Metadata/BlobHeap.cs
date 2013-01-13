using System.IO;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	internal sealed class BlobHeap
	{
		private readonly BufferedBinaryReader _heap;

		public BlobHeap(BufferedBinaryReader heap)
		{
			_heap = heap;
		}

		public BufferedBinaryReader Fetch(uint offset)
		{
			if (offset >= _heap.Length)
				throw new BadMetadataException("Invalid #Blob heap offset.");

			_heap.Seek(offset, SeekOrigin.Begin);

			int length = _heap.ReadPackedInt();
			if (length <= 0)
			{
				return Zero;
			}

			return _heap.Slice(_heap.Position, length);
		}

		private static readonly BufferedBinaryReader Zero = new BufferedBinaryReader(new byte[0]);
	}
}