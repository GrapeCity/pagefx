using System.Diagnostics;

namespace System.IO
{
    /// <summary>
    /// Represents writer that can write bit-by-bit in bigendian byte ordering.
    /// </summary>
    public class BitWriter
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="BitWriter"/> with specified stream and code length.
        /// </summary>
        /// <param name="s">The output stream.</param>
        /// <param name="bpc">The number of bits per code.</param>
        public BitWriter(Stream s, int bpc)
        {
            _stream = s;
            _bpc = bpc;
        }

        /// <summary>
        /// Initializes new instance of <see cref="BitWriter"/> with specified stream and code length 32.
        /// </summary>
        /// <param name="s">The output stream.</param>
        public BitWriter(Stream s)
            : this(s, 32)
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="BitWriter"/> with memory stream and specified code length.
        /// </summary>
        /// <param name="bpc">The number of bits per code.</param>
        public BitWriter(int bpc)
            : this(new MemoryStream(), bpc)
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="BitWriter"/> with memory stream and default code length 32.
        /// </summary>
        public BitWriter()
            : this(new MemoryStream(), 32)
        {
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Gets the underlied stream.
        /// </summary>
        public Stream BaseStream
        {
            get { return _stream; }
        }
        private readonly Stream _stream;

        /// <summary>
        /// Gets or sets the stream position.
        /// </summary>
        public long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        /// <summary>
        /// Gets or sets the number of bits per code.
        /// </summary>
        public int BitsPerCode
        {
            get { return _bpc; }
            set { _bpc = value; }
        }
        private int _bpc = 32;
        private int _bitpos = 7;
        private int _curbyte;

        /// <summary>
        /// Closes the memory stream and returns bytes.
        /// </summary>
        public byte[] ToByteArray()
        {
            Align();
            return Stream2.ToByteArray(_stream);
        }

        /// <summary>
        /// Aligns and flushes underlined stream.
        /// </summary>
        public void Flush()
        {
            Align();
            _stream.Flush();
        }

        /// <summary>
        /// Writes an array of bits.
        /// </summary>
        /// <param name="bits">Array of bits to write.</param>
        public void WriteBits(bool[] bits)
        {
            foreach (var v in bits)
                WriteBit(v);
        }

        /// <summary>
        /// Writes bit-string (e.g. "0111011").
        /// </summary>
        /// <param name="s">The bit-string to write.</param>
        public void WriteBits(string s)
        {
            foreach (var c in s)
            {
                if (c == '0') WriteBit(false);
                else if (c == '1') WriteBit(true);
                else throw new ArgumentException("Invalid character in string", "s");
            }
        }

        /// <summary>
        /// Writes code to output with predefined bits per code.
        /// </summary>
        /// <param name="code">The code to write.</param>
        public void WriteCode(uint code)
        {
            WriteCode(code, _bpc);
        }

        /// <summary>
        /// Writes 32-bit integer number to output.
        /// </summary>
        /// <param name="code">The code to write.</param>
        public void Write32(uint code)
        {
            if (_bitpos == 7)
            {
                _stream.WriteByte((byte)((code >> 24) & 0xff));
                _stream.WriteByte((byte)((code >> 16) & 0xff));
                _stream.WriteByte((byte)((code >> 8) & 0xff));
                _stream.WriteByte((byte)(code & 0xff));
            }
            else
            {
                WriteCode(code, 32);
            }
        }

        /// <summary>
        /// Writes 24-bit code to output.
        /// </summary>
        /// <param name="code">The code to write.</param>
        public void Write24(uint code)
        {
            if (_bitpos == 7)
            {
                _stream.WriteByte((byte)((code >> 16) & 0xff));
                _stream.WriteByte((byte)((code >> 8) & 0xff));
                _stream.WriteByte((byte)(code & 0xff));
            }
            else
            {
                WriteCode(code, 24);
            }
        }

        /// <summary>
        /// Writes 16-bit code to output.
        /// </summary>
        /// <param name="code">The code to write.</param>
        public void Write16(ushort code)
        {
            if (_bitpos == 7)
            {
                _stream.WriteByte((byte)((code >> 8) & 0xff));
                _stream.WriteByte((byte)(code & 0xff));
            }
            else
            {
                WriteCode(code, 16);
            }
        }

        /// <summary>
        /// Writes 8-bit code to output.
        /// </summary>
        /// <param name="code">The code to write.</param>
        public void Write8(byte code)
        {
            if (_bitpos == 7)
            {
                _stream.WriteByte(code);
            }
            else
            {
                WriteCode(code, 8);
            }
        }

        /// <summary>
        /// Writes an array of 8-bit codes.
        /// </summary>
        /// <param name="codes">An array to write.</param>
        public void Write8(byte[] codes)
        {
            int n = codes.Length;
            for (int i = 0; i < n; ++i)
                Write8(codes[i]);
        }

        /// <summary>
        /// Writes single bit to output.
        /// </summary>
        /// <param name="value">The value of bit to write.</param>
        public void WriteBit(bool value)
        {
            Debug.Assert(_bitpos >= 0);
            int mask = (1 << _bitpos);
            if (value) _curbyte |= mask;
            else _curbyte &= ~mask;
            --_bitpos;
            if (_bitpos < 0)
            {
                _stream.WriteByte((byte)_curbyte);
                _curbyte = 0;
                _bitpos = 7;
            }
        }

        /// <summary>
        /// Writes bit n times to output.
        /// </summary>
        /// <param name="value">The value of bit to write.</param>
        /// <param name="n">The number of repeats.</param>
        public void WriteBit(bool value, int n)
        {
            uint code = value ? 0xffffffff : 0;
            while (true)
            {
                if (n >= 32)
                {
                    WriteCode(code, 32);
                    n -= 32;
                }
                else
                {
                    WriteCode(code, n);
                    return;
                }
            }
        }

        /// <summary>
        /// Writes code to output with specified bits per code.
        /// </summary>
        /// <param name="code">The code to write.</param>
        /// <param name="len">The number of bits per code.</param>
        public void WriteCode(uint code, int len)
        {
            while (len > 0)
            {
                int v = (int)code;
                int rem = _bitpos + 1;
                if (len >= rem)
                {
                    len -= rem;
                    v >>= len;
                    v &= 0xff >> (8 - rem);
                    _curbyte |= v;
                    _stream.WriteByte((byte)_curbyte);
                    _curbyte = 0;
                    _bitpos = 7;
                }
                else
                {
                    v &= 0xff >> (8 - len);
                    v <<= rem - len;
                    _curbyte |= v;
                    _bitpos -= len;
                    return;
                }
            }
        }

        /// <summary>
        /// Writes signed code.
        /// </summary>
        /// <param name="code">The code to write.</param>
        /// <param name="len">The code length.</param>
        public void WriteCode(int code, int len)
        {
            WriteCode((uint)code, len);
        }

        /// <summary>
        /// Adds padding bits to align byte boundary.
        /// </summary>
        public void Align()
        {
            if (_bitpos != 7)
            {
                _stream.WriteByte((byte)_curbyte);
                _curbyte = 0;
                _bitpos = 7;
            }
        }

        /// <summary>
        /// Closes the underlying output stream.
        /// </summary>
        public void Close()
        {
            _stream.Close();
        }
        #endregion
    }
}