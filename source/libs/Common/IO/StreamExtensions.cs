using System.IO;
using System.Text;

namespace DataDynamics.PageFX.Common.IO
{
    public static class StreamExtensions
    {
        public static int ReadByteSafe(this Stream s)
        {
            int res = s.ReadByte();
            if (res < 0) throw new EndOfStreamException();
            return res;
        }

        #region int16
        public static short ReadInt16BE(this Stream s)
        {
            return (short)((s.ReadByteSafe() << 8) | s.ReadByteSafe());
        }

        public static short ReadInt16LE(this Stream s)
        {
            return (short)(s.ReadByteSafe() | (s.ReadByteSafe() << 8));
        }

        public static short ReadInt16(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadInt16BE() : s.ReadInt16LE();
        }

        public static ushort ReadUInt16BE(this Stream s)
        {
            return (ushort)((s.ReadByteSafe() << 8) | s.ReadByteSafe());
        }

        public static ushort ReadUInt16LE(this Stream s)
        {
            return (ushort)(s.ReadByteSafe() | (s.ReadByteSafe() << 8));
        }

        public static ushort ReadUInt16(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadUInt16BE() : s.ReadUInt16LE();
        }
        #endregion

        #region int32
        public static uint ReadUInt32BE(this Stream s)
        {
            uint v0 = (uint)(s.ReadByteSafe() << 24);
            uint v1 = (uint)(s.ReadByteSafe() << 16);
            uint v2 = (uint)(s.ReadByteSafe() << 8);
            uint v3 = (uint)s.ReadByteSafe();
            return v0 | v1 | v2 | v3;
        }

        public static int ReadInt32BE(this Stream s)
        {
            return (int)s.ReadUInt32BE();
        }

        public static uint ReadUInt32LE(this Stream s)
        {
            uint v0 = (uint)s.ReadByteSafe();
            uint v1 = (uint)(s.ReadByteSafe() << 8);
            uint v2 = (uint)(s.ReadByteSafe() << 16);
            uint v3 = (uint)(s.ReadByteSafe() << 24);
            return v0 | v1 | v2 | v3;
        }

        public static int ReadInt32LE(this Stream s)
        {
            return (int)s.ReadUInt32LE();
        }

        public static int ReadInt32(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadInt32BE() : s.ReadInt32LE();
        }

        public static uint ReadUInt32(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadUInt32BE() : s.ReadUInt32LE();
        }
        #endregion

        #region int64
        public static ulong ReadUInt64BE(this Stream s)
        {
            ulong v0 = (ulong)s.ReadByteSafe() << 56;
            ulong v1 = (ulong)s.ReadByteSafe() << 48;
            ulong v2 = (ulong)s.ReadByteSafe() << 40;
            ulong v3 = (ulong)s.ReadByteSafe() << 32;
            ulong v4 = (ulong)s.ReadByteSafe() << 24;
            ulong v5 = (ulong)s.ReadByteSafe() << 16;
            ulong v6 = (ulong)s.ReadByteSafe() << 8;
            ulong v7 = (ulong)s.ReadByteSafe();
            return v0 | v1 | v2 | v3 | v4 | v5 | v6 | v7;
        }

        public static long ReadInt64BE(this Stream s)
        {
            return (long)s.ReadUInt64BE();
        }

        public static ulong ReadUInt64LE(this Stream s)
        {
            ulong v0 = (ulong)s.ReadByteSafe();
            ulong v1 = (ulong)s.ReadByteSafe() << 8;
            ulong v2 = (ulong)s.ReadByteSafe() << 16;
            ulong v3 = (ulong)s.ReadByteSafe() << 24;
            ulong v4 = (ulong)s.ReadByteSafe() << 32;
            ulong v5 = (ulong)s.ReadByteSafe() << 40;
            ulong v6 = (ulong)s.ReadByteSafe() << 48;
            ulong v7 = (ulong)s.ReadByteSafe() << 56;
            return  v0 | v1 | v2 | v3 | v4 | v5 | v6 | v7;
        }

        public static long ReadInt64LE(this Stream s)
        {
            return (long)s.ReadUInt64LE();
        }

        public static long ReadInt64(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadInt64BE() : s.ReadInt64LE();
        }

        public static ulong ReadUInt64(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadUInt64BE() : s.ReadUInt64LE();
        }
        #endregion

        #region float
        public static unsafe float ReadSingleBE(this Stream s)
        {
            uint v = s.ReadUInt32BE();
            return *((float*)&v);
        }

        public static unsafe float ReadSingleLE(this Stream s)
        {
            uint v = s.ReadUInt32LE();
            return *((float*)&v);
        }

        public static float ReadSingle(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadSingleBE() : s.ReadSingleLE();
        }
        #endregion

        #region double
        public static unsafe double ReadDoubleBE(this Stream s)
        {
            ulong v = s.ReadUInt64BE();
            return *((double*)&v);
        }

        public static unsafe double ReadDoubleLE(this Stream s)
        {
            ulong v = s.ReadUInt64LE();
            return *((double*)&v);
        }

        public static double ReadDouble(this Stream s, bool bigEndian)
        {
            return bigEndian ? s.ReadDoubleBE() : s.ReadDoubleLE();
        }

        public static double[] ReadDoubleArray(this Stream s, bool bigEndian, int length)
        {
            var array = new double[length];
            for (int i = 0; i < length; ++i)
                array[i] = s.ReadDouble(bigEndian);
            return array;
        }
        #endregion

        #region utf8
        /// <summary>
        /// UTF8 to unicode algorithm:
        /// Let's take a UTF-8 byte sequence. The first byte in a new sequence will tell us how long the sequence is. 
        /// Let's call the subsequent decimal bytes z y x w v u.
        /// 
        /// If z is between and including 0 - 127, then there is 1 byte z. The decimal Unicode value ud = the value of z.
        /// If z is between and including 192 - 223, then there are 2 bytes z y; ud = (z-192)*64 + (y-128)
        /// If z is between and including 224 - 239, then there are 3 bytes z y x; ud = (z-224)*4096 + (y-128)*64 + (x-128)
        /// If z is between and including 240 - 247, then there are 4 bytes z y x w; ud = (z-240)*262144 + (y-128)*4096 + (x-128)*64 + (w-128)
        ///	If z is between and including 248 - 251, then there are 5 bytes z y x w v; ud = (z-248)*16777216 + (y-128)*262144 + (x-128)*4096 + (w-128)*64 + (v-128)
        ///	If z is 252 or 253, then there are 6 bytes z y x w v u; ud = (z-252)*1073741824 + (y-128)*16777216 + (x-128)*262144 + (w-128)*4096 + (v-128)*64 + (u-128)
        /// If z = 254 or 255 then there is something wrong!
        /// 
        ///	Example: take the decimal Unicode designation 8482 (decimal), which is for the trademark sign. 
        ///	This number is larger than 2048, so we get three numbers.																																					The first number is 224 + (8482 div 4096) = 224 + 2 = 226.
        ///	The second number is 128 + (8482 div 64) mod 64) = 128 + (132 mod 64) = 128 + 4 = 132.
        ///	The third number is 128 + (8482 mod 64) = 128 + 34 = 162.
        ///	Now the other way round. We see the numbers 226, 132 and 162. What is the decimal Unicode value?
        ///	In this case: (226-224)*4096+(132-128)*64+(162-128) = 8482.
        ///	And the conversion between hexadecimal and decimal? Come on, this is not a math tutorial! In case you don't know, use a calculator.
        /// </summary>
        /// <returns></returns>
        public static string ReadStringUtf8(this Stream stream, int length) // len==-1 reads string till \0
        {
            var s = new StringBuilder();
            int byteCount = 0;
            while (length < 0 || byteCount < length)
            {
                int ch0 = stream.ReadByteSafe();
                byteCount++;
                if (ch0 == 0 && length < 0)
                    break;

                if ((ch0 & 0x80) == 0)
                {
                    s.Append((char)ch0);
                    continue;
                }

                char ch;
                int ch1 = stream.ReadByteSafe();
                byteCount++;

                if (ch1 == 0)
                {
                    //Dangling lead byte, do not decompose
                    s.Append((char)ch0);
                    break;
                }

                if ((ch0 & 0x20) == 0)
                {
                    ch = (char)(((ch0 & 0x1F) << 6) | (ch1 & 0x3F));
                }
                else
                {
                    int ch2 = stream.ReadByteSafe();
                    byteCount++;
                    if (ch2 == 0)
                    {
                        //Dangling lead bytes, do not decompose
                        s.Append((char)((ch0 << 8) | ch1));
                        break;
                    }

                    uint ch32;
                    if ((ch0 & 0x10) == 0)
                    {
                        ch32 = (uint)(((ch0 & 0x0F) << 12)
                                      | ((ch1 & 0x3F) << 6)
                                      | (ch2 & 0x3F));
                    }
                    else
                    {
                        int ch3 = stream.ReadByteSafe();
                        byteCount++;
                        if (ch3 == 0)
                        {
                            s.Append((char)((ch0 << 8) | ch1));
                            s.Append((char)ch2);
                            break;
                        }

                        ch32 = (uint)(((ch0 & 0x07) << 0x18) // combine 6 bit parts
                                      | ((ch1 & 0x3F) << 12)
                                      | ((ch2 & 0x3F) << 6)
                                      | (ch3 & 0x3F));
                    }

                    if ((ch32 & 0xFFFF0000) == 0)
                    {
                        ch = (char)ch32;
                    }
                    else
                    {
                        //break up into UTF16 surrogate pair
                        s.Append((char)((ch32 >> 10) | 0xD800));
                        ch = (char)((ch32 & 0x3FF) | 0xDC00);
                    }
                }

                s.Append(ch);
            }

            return s.ToString();
        }
        #endregion

	    private const int DefaultBufferLength = 4096;

    	public static void CopyTo(this Stream input, Stream output)
        {
            int len;
            var buf = new byte[DefaultBufferLength];
            while ((len = input.Read(buf, 0, buf.Length)) > 0)
                output.Write(buf, 0, len);
        }

        /// <summary>
        /// Saves specified stream to file.
        /// </summary>
        /// <param name="stream">The stream to save.</param>
        /// <param name="path">The path to file in which should be saved the stream.</param>
        public static void Save(this Stream stream, string path)
        {
            stream.Position = 0;
            using (var fs = File.Create(path))
            {
                stream.CopyTo(fs);
                fs.Flush();
                stream.Position = 0;
            }
        }

        /// <summary>
        /// Creates memory stream filled by byte array.
        /// </summary>
        /// <param name="data">The byte array to fill stream.</param>
        /// <returns>The stream filled by the data.</returns>
        public static MemoryStream ToMemoryStream(this byte[] data)
        {
        	return new MemoryStream(data, false) {Position = 0};
        }

        public static MemoryStream ToMemoryStream(this Stream s)
        {
            var ms = new MemoryStream();
            s.CopyTo(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        public static byte[] ToByteArray(this Stream s)
        {
            var ms = s as MemoryStream;
            if (ms != null)
            {
                ms.Flush();
                return ms.ToArray();
            }
        	ms = s.ToMemoryStream();
        	ms.Flush();
        	ms.Close();
        	return ms.ToArray();
        }

	    public static string ReadAllText(this Stream s)
        {
            using (var reader = new StreamReader(s))
                return reader.ReadToEnd();
        }
    }
}