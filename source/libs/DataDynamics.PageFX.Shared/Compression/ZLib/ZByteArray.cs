using System;
using System.IO;


namespace DataDynamics.ZLib
{
	/// <summary>
	/// Summary description for ZByteArray.
	/// </summary>
	internal class ZByteArray
	{
		public ZByteArray()
		{
		}

		public static byte[] Compress(byte[] data,int level)
		{
			MemoryStream sourceStream=new MemoryStream(data);
			sourceStream.Position=0;
			MemoryStream destStream=new MemoryStream();
			ZLib.ZStream zs=new ZLib.ZStream();
			ZLib.ReturnCodes result=zs.Compress(sourceStream,destStream,level,4096);
			if (result!=ZLib.ReturnCodes.ZLibOk && result!=ZLib.ReturnCodes.ZLibStreamEnd)
			{
				destStream.Close();
				sourceStream.Close();
				return null;
			}
			data=destStream.ToArray();
			destStream.Close();
			sourceStream.Close();
			return data;
		}

		public static byte[] DeCompress(byte[] data)
		{
			MemoryStream sourceStream=new MemoryStream(data);
			sourceStream.Position=0;
			MemoryStream destStream=new MemoryStream();
			ZLib.ZStream zs=new ZLib.ZStream();
			ZLib.ReturnCodes result=zs.Decompress(sourceStream,destStream,4096);
			if (result!=ZLib.ReturnCodes.ZLibOk && result!=ZLib.ReturnCodes.ZLibStreamEnd)
			{
				destStream.Close();
				sourceStream.Close();
				return null;
			}
			data=destStream.ToArray();
			destStream.Close();
			sourceStream.Close();
			return data;
		}
	}
}
