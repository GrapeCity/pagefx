using System;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;

namespace DataDynamics
{
    /// <summary>
    /// Facade to zip lib
    /// </summary>
    public static class Zip
    {
        public static Stream Uncompress(Stream input)
        {
			return new ZlibStream(input, CompressionMode.Decompress);
        }

        public static byte[] Uncompress(byte[] data)
        {
			return ZlibStream.UncompressBuffer(data);
        }

	    public static byte[] Compress(byte[] data)
	    {
		    return ZlibStream.CompressBuffer(data);
	    }

	    public static Stream ExtractEntry(this ZipFile zip, Func<ZipEntry,bool> selector)
        {
			var e = zip.FirstOrDefault(selector);
            return e != null ? e.OpenReader().ToMemoryStream() : null;
        }

        public static Stream ExtractEntry(this ZipFile zip, string name)
        {
            return zip.ExtractEntry(e => e.FileName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}