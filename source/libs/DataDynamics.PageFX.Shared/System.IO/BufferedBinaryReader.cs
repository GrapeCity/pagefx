using System.Text;

namespace System.IO
{
    /// <summary>
    /// Reader on bytes stream
    /// </summary>
    public sealed class BufferedBinaryReader : Stream
    {
        #region Fields
        private readonly byte[] _buffer;
        private long _pos;
        #endregion

        #region Constructors
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        public BufferedBinaryReader(string fileName)
        {
            using (Stream val = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                _pos = 0;
                _buffer = new byte[val.Length];
                val.Read(_buffer, 0, (int)val.Length);
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="stream">Stream with data</param>
        public BufferedBinaryReader(Stream stream)
        {
            _pos = 0;
            _buffer = new byte[stream.Length];
            int total = 0;
            while (total != stream.Length)
            {
                int readed = stream.Read(_buffer, total, (int)stream.Length - total);
                total += readed;
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="buffer">Source array</param>
        public BufferedBinaryReader(byte[] buffer)
        {
            _buffer = buffer;
        }
        #endregion

        #region Stream Members
        /// <summary>
        ///  Current cursor position
        /// </summary>
        public override long Position
        {
            set { _pos = value; }
            get { return _pos; }
        }

        /// <summary>
        /// Buffer length
        /// </summary>
        public override long Length
        {
            get { return _buffer.Length; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_pos + count > Length)
                count = (int)(Length - _pos);

            Array.Copy(_buffer, _pos, buffer, offset, count);
            _pos += count;
            return count;
        }

        public override int ReadByte()
        {
            return ReadUInt8();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Read bool value from stream
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            return _buffer[_pos++] != 0;
        }

        /// <summary>
        /// Read char value from stream
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return (char)ReadUInt16();
        }

        /// <summary>
        /// Read float value from stream
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            var val = new MemoryStream(ReadBlock(4));
            using (var reader = new BinaryReader(val))
            {
                return reader.ReadSingle();
            }
        }

        /// <summary>
        /// Read double value from stream
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            var val = new MemoryStream(ReadBlock(8));
            using (var reader = new BinaryReader(val))
            {
                return reader.ReadDouble();
            }
        }

        /// <summary>
        ///  Read signed byte
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return (sbyte)_buffer[_pos++];
        }

        /// <summary>
        ///  Read signed byte
        /// </summary>
        /// <returns></returns>
        public sbyte ReadInt8()
        {
            return (sbyte)_buffer[_pos++];
        }

        /// <summary>
        /// Read byte
        /// </summary>
        /// <returns></returns>
        public byte ReadUInt8()
        {
            return _buffer[_pos++];
        }

        /// <summary>
        /// Read ushort value
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            int b1 = _buffer[_pos++];
            int b2 = _buffer[_pos++];
            //return (ushort)(b2 | (b1 << 8));
            return (ushort)(b1 | (b2 << 8));
        }

        /// <summary>
        /// Read short value from stream
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        /// <summary>
        /// Read uint
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            uint b1 = _buffer[_pos++];
            uint b2 = _buffer[_pos++];
            uint b3 = _buffer[_pos++];
            uint b4 = _buffer[_pos++];
            //return b4 | (b3 << 8) | (b2 << 16) | (b1 << 24);
            return b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
        }

        /// <summary>
        /// Read int value
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        /// Read long
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        /// <summary>
        /// Read ulong
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            ulong v1 = ReadUInt32();
            ulong v2 = ReadUInt32();
            return (v1 | (v2 << 32));
        }

        public int ReadPackedInt(int b0)
        {
            //1 byte
            if ((b0 & 0x80) == 0)
                return b0;

            int b1 = ReadUInt8();

            //2 bytes
            if ((b0 & 0xC0) == 0x80)
                return (((b0 & 0x3F) << 8) | b1);

            //4 bytes
            int b2 = ReadUInt8();
            int b3 = ReadUInt8();
            return ((b0 & 0x3F) << 24) | (b1 << 16) | (b2 << 8) | b3;
        }

        public int ReadPackedInt()
        {
            int b0 = ReadUInt8();
            return ReadPackedInt(b0);
        }

        /// <summary>
        /// Read array of bytes
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ReadBlock(int count)
        {
            var buf = new byte[count];
            Array.Copy(_buffer, _pos, buf, 0, count);
            _pos += count;
            return buf;
        }

        public uint ReadIndex(int size)
        {
            if (size == 2) return ReadUInt16();
            return ReadUInt32();
        }

        public string ReadUtf8()
        {
            return ReadUtf8(-1);
        }

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
        public string ReadUtf8(int bytesToRead) // bytesToRead==-1 reads string till \0
        {
            long bufferPos = _pos;

            // calc string length and allocate char _buffer
            int count = 0;
            int byteCount = 0;
            while (bytesToRead == -1 || byteCount < bytesToRead)
            {
                byte ch0 = _buffer[bufferPos++];
                byteCount++;
                if (ch0 == 0) // end of string
                    break;

                if ((ch0 & 0x80) == 0) // check for lead byte
                {
                    count++; // single byte character
                    continue;
                }

                byte ch1 = _buffer[bufferPos++]; // trail
                byteCount++;

                if (ch1 == 0)
                    break;
                if ((ch0 & 0x20) != 0)
                {
                    byte ch2 = _buffer[bufferPos++];
                    byteCount++;

                    if (ch2 == 0)
                        break;
                    uint ch32;
                    if ((ch0 & 0x10) == 0)
                    {
                        ch32 = (uint)(((ch0 & 0x0F) << 12)
                                      | ((ch1 & 0x3F) << 6)
                                      | (ch2 & 0x3F));
                    }
                    else
                    {
                        byte ch3 = _buffer[bufferPos++];
                        byteCount++;

                        if (ch3 == 0)
                        {
                            count++;
                            break;
                        }
                        ch32 = (uint)(((ch0 & 0x07) << 18)
                                      | ((ch1 & 0x3F) << 12)
                                      | ((ch2 & 0x3F) << 6)
                                      | (ch3 & 0x3F));
                    }

                    if ((ch32 & 0xFFFF0000) != 0)
                        count++;
                }
                count++;
            }

            // Copy _buffer
            int index = 0;
            var result = new char[count];
            byteCount = 0;
            while (bytesToRead == -1 || byteCount < bytesToRead)
            {
                byte ch0 = _buffer[_pos++];
                byteCount++;
                if (ch0 == 0)
                    break;
                if ((ch0 & 0x80) == 0)
                {
                    result[index++] = (char)ch0;
                    continue;
                }
                char ch;
                byte ch1 = _buffer[_pos++];
                byteCount++;
                if (ch1 == 0)
                {
                    //Dangling lead byte, do not decompose
                    result[index++] = (char)ch0;
                    break;
                }
                if ((ch0 & 0x20) == 0)
                {
                    ch = (char)(((ch0 & 0x1F) << 6) | (ch1 & 0x3F));
                }
                else
                {
                    byte ch2 = _buffer[_pos++];
                    byteCount++;
                    if (ch2 == 0)
                    {
                        //Dangling lead bytes, do not decompose
                        result[index++] = (char)((ch0 << 8) | ch1);
                        break;
                    }

                    uint ch32;
                    if ((ch0 & 0x10) == 0)
                        ch32 = (uint)(((ch0 & 0x0F) << 12)
                                      | ((ch1 & 0x3F) << 6)
                                      | (ch2 & 0x3F));
                    else
                    {
                        byte ch3 = _buffer[_pos++];
                        byteCount++;
                        if (ch3 == 0)
                        {
                            result[index++] = (char)((ch0 << 8) | ch1);
                            result[index++] = (char)ch2;
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
                        result[index++] = (char)((ch32 >> 10) | 0xD800);
                        ch = (char)((ch32 & 0x3FF) | 0xDC00);
                    }
                }

                result[index++] = ch;
            }

            return new string(result);
        }

        public string ReadNullTerminatedAsciiString()
        {
            var s = new StringBuilder();
            while (true)
            {
                byte c = ReadUInt8();
                if (c == 0) break;
                s.Append((char)c);
            }
            return s.ToString();
        }

        public string ReadAscii(int len, bool trimZero)
        {
            var buf = ReadBlock(len);
            string s = Encoding.ASCII.GetString(buf);
            if (trimZero)
                return s.Trim('\0');
            return s;
        }

        public void Align4()
        {
            _pos = ((_pos + 3) / 4) * 4;
        }
        #endregion
    }
}