using System.IO;
using System.Linq;
using DataDynamics.Compression.Zip;
using DataDynamics.Compression.Zip.Compression.Streams;

namespace DataDynamics
{
    /// <summary>
    /// Facade to zip lib
    /// </summary>
    public static class Zip
    {
        public static Stream Uncompress(Stream input)
        {
            return new InflaterInputStream(input);
        }

        public static byte[] Uncompress(byte[] data)
        {
            var unzip = Uncompress(new MemoryStream(data));
            var buf = new byte[4096];
            var ms = new MemoryStream();
            int size;
            while ((size = unzip.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, size);
            }
            ms.Flush();
            ms.Close();
            return ms.ToArray();
        }

        public static Stream Compress(Stream input)
        {
            return new DeflaterOutputStream(input);
        }

        public static byte[] Compress(byte[] data)
        {
            var ms = new MemoryStream(data.Length);
            var zip = Compress(ms);
            zip.Write(data, 0, data.Length);
            zip.Flush();
            zip.Close();
            return ms.ToArray();
        }

        public static ZipEntry FindEntry(ZipFile zip, System.Func<ZipEntry,bool> p)
        {
        	return zip.Cast<ZipEntry>().FirstOrDefault(p);
        }

    	public static Stream Extract(ZipFile zip, System.Func<ZipEntry,bool> p)
        {
            var e = FindEntry(zip, p);
            return e != null ? ToMemoryStream(e.Data) : null;
        }

        public static Stream Extract(ZipFile zip, string name)
        {
            return Extract(zip, e => e.Name == name);
        }

        private static MemoryStream ToMemoryStream(Stream s)
        {
            var ms = new MemoryStream();
            CopyTo(s, ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        private static void CopyTo(Stream input, Stream output)
        {
            int len;
            var buf = new byte[512];
            while ((len = input.Read(buf, 0, buf.Length)) > 0)
                output.Write(buf, 0, len);
        }
    }
}