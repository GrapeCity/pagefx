using System.IO;
using System.Text;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	internal sealed class UserStringHeap
	{
		private readonly BufferedBinaryReader _heap;

		public UserStringHeap(BufferedBinaryReader heap)
		{
			_heap = heap;
		}

		public string Fetch(uint offset)
		{
			if (offset == 0)
				throw new BadMetadataException("Invalid #US heap offset.");


			_heap.Seek(offset, SeekOrigin.Begin);

			int length = _heap.ReadPackedInt();
			var bytes = _heap.ReadBytes(length);

			if (bytes[length - 1] == 0 || bytes[length - 1] == 1)
				length--;

			return Encoding.Unicode.GetString(bytes, 0, length);
		}
	}
}