using System.IO;

namespace DataDynamics.ZLib
{
	/// <summary>
	/// Summary description for ZByteArray.
	/// </summary>
	internal class ZByteArray
	{
		public static byte[] Compress(byte[] data,int level)
		{
			var sourceStream = new MemoryStream(data) {Position = 0};
			var destStream = new MemoryStream();
			var zs = new ZStream();
			var result = zs.Compress(sourceStream, destStream, level, 4096);
			if (result != ReturnCodes.ZLibOk && result != ReturnCodes.ZLibStreamEnd)
			{
				destStream.Close();
				sourceStream.Close();
				return null;
			}
			data = destStream.ToArray();
			destStream.Close();
			sourceStream.Close();
			return data;
		}

		public static byte[] DeCompress(byte[] data)
		{
			var sourceStream = new MemoryStream(data) {Position = 0};
			var destStream = new MemoryStream();
			var zs = new ZStream();
			var result = zs.Decompress(sourceStream, destStream, 4096);
			if (result != ReturnCodes.ZLibOk && result != ReturnCodes.ZLibStreamEnd)
			{
				destStream.Close();
				sourceStream.Close();
				return null;
			}
			data = destStream.ToArray();
			destStream.Close();
			sourceStream.Close();
			return data;
		}
	}
}
