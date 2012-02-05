using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.SWF
{
    //TODO: Rewrite ReadDouble without using BinaryReader

    public sealed class SwfReader : IDisposable
    {
        #region Member Variables
        private static readonly float[] powers; // For pre-computed fixed-point powers
        private Stream _stream; // Input stream from SWF data source
        private BitReader _bitReader;
        private BinaryReader _reader;
        #endregion

        #region Constructors
        static SwfReader()
        {
            // Setup power values for later lookup
            powers = new float[32];
            for (byte power = 0; power < 32; power++)
            {
                powers[power] = (float)Math.Pow(2, power - 16);
            }
        }

        public SwfReader(Stream stream)
        {
            Stream = stream;
        }

        public SwfReader(byte[] data)
            : this(new MemoryStream(data))
        {
        }

        public SwfReader(byte[] data, SwfReader parent)
            : this(data)
        {
            FileVersion = parent.FileVersion;
            TagDecodeOptions = parent.TagDecodeOptions;
        }
        #endregion

        #region Properties
        public AbcFile ABC { get; set; }

        internal int MultinameCount { get; set; }

        public Stream Stream
        {
            get { return _stream; }
            set
            {
                _stream = value;
                _bitReader = new BitReader(value);
                _reader = new BinaryReader(_stream);
            }
        }

        public long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public long Length
        {
            get { return _stream.Length; }
        }
        #endregion

        #region Byte aligned types (SI8, SI16, SI32, UI8, UI16, UI32, FIXED)
        /// <summary>
        /// Reads an unsigned 8-bit integer
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            int res = _stream.ReadByte();
            if (res == -1)
                throw new EndOfStreamException();
            // Reset bit reader state, so that ReadBit() knows that we've used this byte already
            _bitReader.ResetState();
            return (byte)res;
        }

        /// <summary>
        /// Reads all bytes from the stream.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadAllBytes()
        {
            return ReadUInt8((int)_stream.Length);
        }

        public byte[] ReadToEnd()
        {
            int n = (int)(_stream.Length - _stream.Position);
            return ReadUInt8(n);
        }

        public byte[] ReadBlock(int size)
        {
            byte[] block = new byte[size];
            _stream.Read(block, 0, size);
            return block;
        }

        /// <summary>
        /// Reads an unsigned 8-bit integer
        /// </summary>
        /// <returns></returns>
        public byte ReadUInt8()
        {
            return ReadByte();
        }

        /// <summary>
        /// Reads an array of unsigned 8-bit integers
        /// </summary>
        /// <param name="n">array size</param>
        /// <returns></returns>
        public byte[] ReadUInt8(int n)
        {
            var res = new byte[n];
            for (int i = 0; i < n; i++)
                res[i] = ReadUInt8();
            return res;
        }

        /// <summary>
        /// Reads a signed byte
        /// </summary>
        /// <returns></returns>
        public sbyte ReadInt8()
        {
            return (sbyte)ReadByte();
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            ushort res = ReadByte();
            res |= (ushort)(ReadByte() << 8);
            return res;
        }

        /// <summary>
        /// Reads an array of unsigned 16-bit integers
        /// </summary>
        /// <param name="n">array size</param>
        /// <returns></returns>
        public ushort[] ReadUInt16(int n)
        {
            var res = new ushort[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadUInt16();
            return res;
        }

        /// <summary>
        /// Reads a signed 16-bit integer
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        /// <summary>
        /// Reads an array of signed 16-bit integers
        /// </summary>
        /// <param name="n">array size</param>
        /// <returns></returns>
        public short[] ReadInt16(int n)
        {
            var res = new short[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadInt16();
            return res;
        }

        /// <summary>
        /// Reads a 24-bit unsigned integer.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt24()
        {
            uint b1 = ReadByte();
            uint b2 = ReadByte();
            uint b3 = ReadByte();
            return (b3 << 16) | (b2 << 8) | b1;
        }

        /// <summary>
        /// Reads a 24-bit unsigned integer in big endian ordering.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt24BE()
        {
            uint b1 = ReadByte();
            uint b2 = ReadByte();
            uint b3 = ReadByte();
            return (b1 << 16) | (b2 << 8) | b3;
        }

        /// <summary>
        /// Reads a 24-bit signed integer.
        /// </summary>
        /// <returns></returns>
        public int ReadInt24()
        {
            uint b1 = ReadByte();
            uint b2 = ReadByte();
            uint b3 = ReadByte();
            uint result = (b3 << 16) | (b2 << 8) | b1;
            if ((b3 & 0x80) != 0)
                result |= 0xFF000000u;
            return (int)result;
        }

        /// <summary>
        /// Reads an array of 24-bit signed integers.
        /// </summary>
        /// <param name="n">array size</param>
        /// <returns></returns>
        public int[] ReadInt24(int n)
        {
            var res = new int[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadInt24();
            return res;
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            uint res = ReadByte();
            res |= ((uint)ReadByte() << 8);
            res |= ((uint)ReadByte() << 16);
            res |= ((uint)ReadByte() << 24);
            return res;
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer in big endian byte ordering.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32BE()
        {
            uint res = ((uint)ReadByte() << 24);
            res |= ((uint)ReadByte() << 16);
            res |= ((uint)ReadByte() << 8);
            res |= ReadByte();
            return res;
        }

        /// <summary>
        /// Reads an array of unsigned 32-bit integers
        /// </summary>
        /// <param name="n">array size</param>
        /// <returns></returns>
        public uint[] ReadUInt32(int n)
        {
            var res = new uint[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadUInt32();
            return res;
        }

        /// <summary>
        /// Reads a signed 32-bit integer
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        /// Reads an array of signed 32-bit integers
        /// </summary>
        /// <param name="n">array size</param>
        /// <returns></returns>
        public int[] ReadInt32(int n)
        {
            var res = new int[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadInt32();
            return res;
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            ulong res = ReadUInt32();
            res |= (ulong)ReadUInt32() << 32;
            return res;
        }

        /// <summary>
        /// Reads a signed 64-bit integer
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        /// <summary>
        /// Reads a 32-bit 16.16 fixed-point number
        /// </summary>
        /// <returns></returns>
        public float ReadFixed32()
        {
            float res = 0;
            res += (ReadByte() * powers[0]);
            res += (ReadByte() * powers[7]);
            res += (ReadByte() * powers[15]);
            res += (ReadByte() * powers[31]);
            return res;
        }

        /// <summary>
        /// Reads a 16-bit 8.8 fixed-point number
        /// </summary>
        /// <returns></returns>
        public float ReadFixed16()
        {
            int v = ReadUInt16();
            return v / 256f;
        }

        internal static bool CheckU30;

        public uint ReadUIntEncoded()
        {
            uint result = 0;
            for (int i = 0; i < 5; ++i)
            {
                uint b = ReadByte();
                if ((b & 0x80) == 0)
                {
                    b <<= 7 * i;
                    result |= b;
                    break;
                }
                b &= 0x7f;
                b <<= 7 * i;
                result |= b;
            }
            //if (CheckU30 && (result & 0xc0000000) != 0)
            //    Debugger.Break();
            return result;
        }

        public uint[] ReadUIntEncoded(int n)
        {
            var res = new uint[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadUIntEncoded();
            return res;
        }

        public int ReadIntEncoded()
        {
            return (int)ReadUIntEncoded();
        }

        public int[] ReadIntEncoded(int n)
        {
            var res = new int[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadIntEncoded();
            return res;
        }

        public float ReadSingle()
        {
            return _reader.ReadSingle();
        }

        public float[] ReadSingle(int n)
        {
            var res = new float[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadSingle();
            return res;
        }

        public double ReadDouble()
        {
            return _reader.ReadDouble();
        }

        public double[] ReadDouble(int n)
        {
            var res = new double[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadDouble();
            return res;
        }
        #endregion

        #region Non-byte-aligned bit types (SB[nBits], UB[nBits], FB[nBits])
        /// <summary>
        /// Reads one bit from the stream.
        /// </summary>
        /// <returns></returns>
        public bool ReadBit()
        {
            return _bitReader.ReadBit();
        }

        /// <summary>
        /// Reads an unsigned bit value
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public uint ReadUB(int bits)
        {
            return (uint)_bitReader.ReadCode(bits);
        }

        /// <summary>
        /// Reads a signed bit value
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public int ReadSB(int bits)
        {
            if (bits > 0)
            {
                uint v = ReadUB(bits);
                uint mask = (uint)(1 << (bits - 1));
                if ((v & mask) != 0)
                    v |= 0xFFFFFFFF << bits;
                return (int)v;
            }
            return 0;
        }

        /// <summary>
        /// Reads a signed fixed-point bit value
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public float ReadFB(int bits)
        {
            // Is there anything to read?
            if (bits > 0)
            {
                //TODO: Optimize code below

                float res = 0;

                // Is this a negative number (MSB will be set)?
                if (ReadBit())
                {
                    res -= powers[bits - 1];
                }

                // Calculate rest of value
                for (int index = bits - 1; index > 0; index--)
                {
                    if (ReadBit())
                    {
                        res += powers[index - 1];
                    }
                }
                return res;
            }
            return 0;
        }

        public float ReadFB(int bits, int q)
        {
            int v = ReadSB(bits);
            return FloatHelper.ToSingle(v, q);
        }

        public float ReadFB32(int bits)
        {
            return ReadFB(bits, 16);
        }

        public float ReadFB16(int bits)
        {
            return ReadFB(bits, 8);
        }

        public float ReadTwip(int bits)
        {
            int v = ReadSB(bits);
            return SwfHelper.FromTwips(v);
        }

        public float ReadTwipS16()
        {
            int v = ReadInt16();
            return SwfHelper.FromTwips(v);
        }

        public float ReadTwipU16()
        {
            int v = ReadUInt16();
            return SwfHelper.FromTwips(v);
        }
        
        public void Align()
        {
            //_bitReader.Align();
            _bitReader.ResetState();
        }
        #endregion

        #region STRING
        /// <summary>
        /// Gets or sets SWF format version
        /// </summary>
        public int FileVersion
        {
            get { return _fileVersion; }
            set { _fileVersion = value; }
        }
        private int _fileVersion = 9;

        /// <summary>
        /// Reads an string
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            if (_fileVersion <= 5)
                return ReadASCII();
            return ReadUtf8();
        }

        /// <summary>
        /// Reads null terminated ASCII string.
        /// </summary>
        /// <returns></returns>
        public string ReadASCII()
        {
            var ms = new MemoryStream(64);
            // Grab characters until we hit 0x00
            while (true)
            {
                byte ch = ReadByte();
                if (ch == 0x00) break;
                ms.WriteByte(ch);
            }
            ms.Flush();
            ms.Close();
            var b = ms.ToArray();
            return Encoding.ASCII.GetString(b);
        }

        /// <summary>
        /// Reads ASCII string of specified length.
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string  ReadASCII(int len)
        {
            var b = ReadUInt8(len);
            return Encoding.ASCII.GetString(b);
        }

        public string ReadUtf8()
        {
            return ReadUtf8(-1);
        }

        public string ReadUtf8(int len)
        {
            return Stream2.ReadUtf8(_stream, len);
        }
        #endregion

        #region Simple structured data types
        public Rectangle ReadRect()
        {
            int nBits = (int)ReadUB(5);
            int xMin = ReadSB(nBits);
            int xMax = ReadSB(nBits);
            int yMin = ReadSB(nBits);
            int yMax = ReadSB(nBits);
            //NOTE: The RECT record must be byte aligned.
            Align();
            return Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
        }

        public RectangleF ReadRectF()
        {
            int bits = (int)ReadUB(5);
            float xMin = ReadTwip(bits);
            float xMax = ReadTwip(bits);
            float yMin = ReadTwip(bits);
            float yMax = ReadTwip(bits);
            //NOTE: The RECT record must be byte aligned.
            Align();
            return RectangleF.FromLTRB(xMin, yMin, xMax, yMax);
        }

        public Color ReadRGB()
        {
            byte r = ReadByte();
            byte g = ReadByte();
            byte b = ReadByte();
            return Color.FromArgb(r, g, b);
        }

        public Color[] ReadRGB(int n)
        {
            var res = new Color[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadRGB();
            return res;
        }

        public Color ReadRGBA()
        {
            byte r = ReadByte();
            byte g = ReadByte();
            byte b = ReadByte();
            byte a = ReadByte();
            return Color.FromArgb(a, r, g, b);
        }

        public Color[] ReadRGBA(int n)
        {
            var res = new Color[n];
            for (int i = 0; i < n; ++i)
                res[i] = ReadRGBA();
            return res;
        }

        public Color ReadARGB()
        {
            byte a = ReadByte();
            byte r = ReadByte();
            byte g = ReadByte();
            byte b = ReadByte();
            return Color.FromArgb(a, r, g, b);
        }

        public Matrix ReadMatrix()
        {
            bool hasScale = ReadBit();
            float scaleX = 1;
            float scaleY = 1;
            int bits;
            if (hasScale)
            {
                bits = (int)ReadUB(5);
                scaleX = ReadFB32(bits);
                scaleY = ReadFB32(bits);
            }

            bool hasRotate = ReadBit();
            float skewX = 0;
            float skewY = 0;
            if (hasRotate)
            {
                bits = (int)ReadUB(5);
                skewX = ReadFB32(bits);
                skewY = ReadFB32(bits);
            }

            bits = (int)ReadUB(5);
            float tx = ReadTwip(bits);
            float ty = ReadTwip(bits);
            //NOTE: The MATRIX record must be byte aligned.
            Align();
            return new Matrix(scaleX, skewX, skewY, scaleY, tx, ty);
        }

        public SwfColorTransform ReadColorTransform(bool alpha)
        {
            return new SwfColorTransform(this, alpha);
        }
        #endregion

        #region IDisposable Members
        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~SwfReader()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Unzips end of stream and returns new <see cref="SwfReader"/> to read uncompressed data.
        /// </summary>
        /// <returns></returns>
        public SwfReader Unzip()
        {
            var data = ReadToEnd();
            data = Zip.Uncompress(data);
            return new SwfReader(data);
        }

        public void Round32()
        {
            long pos = Position;
            long pad = (pos % 4);
            if (pad != 0)
                Position = pos + 4 - pad;
        }

        public SwfTagDecodeOptions TagDecodeOptions
        {
            get { return _tagDecodeOptions; }
            set { _tagDecodeOptions = value; }
        }
        private SwfTagDecodeOptions _tagDecodeOptions;

        #region Shapes
        public SwfStyles Styles
        {
            get { return _styles; }
            set { _styles = value; }
        }
        private SwfStyles _styles;

        public int ReadFillStyle()
        {
            return (int)ReadUB(_styles.FillBits);
        }

        public int ReadLineStyle()
        {
            return (int)ReadUB(_styles.LineBits);
        }
        #endregion
    }
}