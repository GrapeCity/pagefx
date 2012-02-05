using System;
using System.IO;
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
            Stream unzip = Uncompress(new MemoryStream(data));
            byte[] buf = new byte[4096];
            MemoryStream ms = new MemoryStream();
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
            MemoryStream ms = new MemoryStream(data.Length);
            Stream zip = Compress(ms);
            zip.Write(data, 0, data.Length);
            zip.Flush();
            zip.Close();
            return ms.ToArray();
        }

        public static ZipEntry FindEntry(ZipFile zip, Predicate<ZipEntry> p)
        {
            foreach (ZipEntry e in zip)
            {
                if (p(e)) 
                    return e;
            }
            return null;
        }

        public static Stream Extract(ZipFile zip, Predicate<ZipEntry> p)
        {
            ZipEntry e = FindEntry(zip, p);
            if (e != null)
                return ToMemoryStream(e.Data);
            return null;
        }

        public static Stream Extract(ZipFile zip, string name)
        {
            return Extract(zip,
                           delegate(ZipEntry e)
                               {
                                   return e.Name == name;
                               });
        }

        private static MemoryStream ToMemoryStream(Stream s)
        {
            MemoryStream ms = new MemoryStream();
            CopyTo(s, ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        private static void CopyTo(Stream input, Stream output)
        {
            int len;
            byte[] buf = new byte[512];
            while ((len = input.Read(buf, 0, buf.Length)) > 0)
                output.Write(buf, 0, len);
        }
    }
}