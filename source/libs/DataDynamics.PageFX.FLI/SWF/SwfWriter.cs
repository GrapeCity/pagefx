using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Represents SWF writer that provides functionality to write basic data types defined in SWF format.
    /// </summary>
    public sealed class SwfWriter : IDisposable
    {
    	private Stream _stream;
        private readonly BitWriter _writer;
        private readonly bool _disposeStream;
        private readonly BinaryWriter _binWriter;

    	#region Constructors
        public SwfWriter(Stream stream, bool disposeStream)
        {
            _stream = stream;
            _writer = new BitWriter(stream);
            _disposeStream = disposeStream;
            _binWriter = new BinaryWriter(stream);
        }

        public SwfWriter(Stream stream)
            : this(stream, false)
        {
        }

        public SwfWriter()
            : this(new MemoryStream())
        {
        }

        public SwfWriter(string path)
            : this(File.OpenWrite(path), true)
        {
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
                if (_disposeStream)
                {
                    _stream.Dispose();
                }
                _stream = null;
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~SwfWriter()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
        #endregion

        public AbcFile ABC { get; set; }

        public int FileVersion
        {
            get { return _fileVersion; }
            set { _fileVersion = value; }
        }
        private int _fileVersion = 9;

        /// <summary>
        /// Gets all bytes in output stream.
        /// </summary>
        /// <returns>array of bytes in output stream.</returns>
        public byte[] ToByteArray()
        {
            Align();
            return _stream.ToByteArray();
        }

        /// <summary>
        /// Flushes the output stream.
        /// </summary>
        public void Flush()
        {
            _writer.Flush();
        }

        public long Position
        {
            get
            {
                return _stream.Position;
            }
        }

        #region Non byte aligned bit types
        /// <summary>
        /// Writes code to output stream with specified bits per code.
        /// </summary>
        /// <param name="code">The code to write.</param>
        /// <param name="len">The number of bits per code.</param>
        public void WriteSB(int code, int len)
        {
            _writer.WriteCode((uint)code, len);    
        }

        /// <summary>
        /// Writes code to output stream with specified bits per code.
        /// </summary>
        /// <param name="code">The code to write.</param>
        /// <param name="len">The number of bits per code.</param>
        public void WriteUB(uint code, int len)
        {
            _writer.WriteCode(code, len);
        }

        /// <summary>
        /// Writes single bit to output stream.
        /// </summary>
        /// <param name="value">The value of bit to write.</param>
        public void WriteBit(bool value)
        {
            _writer.WriteBit(value);
        }

        /// <summary>
        /// Adds padding bits to align byte boundary.
        /// </summary>
        public void Align()
        {
            _writer.Align();
        }

        public void WriteFB16(float value, int q, int bits)
        {
            short v = value.FixedToInt16(q);
            WriteUB((uint)v, bits);
        }
        #endregion

        #region Byte aligned types
        /// <summary>
        /// Writes array of bytes to output stream.
        /// </summary>
        /// <param name="buf">array of bytes to write.</param>
        public void Write(byte[] buf)
        {
            _stream.Write(buf, 0, buf.Length);
        }

        public void Pad(int len)
        {
            for (int i = 0; i < len; ++i)
                _stream.WriteByte(0);
        }

        #region Integer Numbers
        /// <summary>
        /// Writes 8-bits unsigned integer to output stream.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        public void WriteUInt8(byte value)
        {
            Align();
            _stream.WriteByte(value);
        }

        /// <summary>
        /// Writes 8-bits unsigned integer to output stream.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        public void WriteByte(byte value)
        {
            Align();
            _stream.WriteByte(value);
        }

        /// <summary>
        /// Writes 8-bits signed integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteInt8(sbyte value)
        {
            WriteUInt8((byte)value);
        }

        /// <summary>
        /// Writes 16-bits unsigned integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteUInt16(ushort value)
        {
            Align();
            _stream.WriteByte((byte)value);
            _stream.WriteByte((byte)(value >> 8));
        }

        /// <summary>
        /// Writes 16-bits signed integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        /// <summary>
        /// Writes 32-bits unsigned integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteUInt32(uint value)
        {
            Align();
            _stream.WriteByte((byte)value);
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)(value >> 16));
            _stream.WriteByte((byte)(value >> 24));
        }

        /// <summary>
        /// Writes 32-bits unsigned integer to output stream in big endian order.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteUInt32BE(uint value)
        {
            Align();
            _stream.WriteByte((byte)(value >> 24));
            _stream.WriteByte((byte)(value >> 16));
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)value);
            
        }

        /// <summary>
        /// Writes 32-bits signed integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteInt32(int value)
        {
            WriteUInt32((uint)value);
        }

        /// <summary>
        /// Writes 64-bits unsigned integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteUInt64(ulong value)
        {
            WriteUInt32((uint)value);
            WriteUInt32((uint)(value >> 32));
        }

        /// <summary>
        /// Writes 64-bits signed integer to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteInt64(long value)
        {
            WriteUInt64((ulong)value);
        }
        #endregion

        #region Fixed Numbers
        /// <summary>
        /// Writes 8.8 fixed number to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteUFloat16(float value)
        {
            WriteUInt16((ushort)(value * 256));
        }

        /// <summary>
        /// Writes 16.16 fixed number to output stream.
        /// </summary>
        /// <param name="value">The number to write.</param>
        public void WriteUFloat32(float value)
        {
            WriteUInt32((uint)(value * 65536));
        }

        public void WriteFixed16(float value)
        {
            WriteUInt16((ushort)(value * 256));
        }

        public void WriteFixed32(float value)
        {
            WriteUInt32((uint)(value * 65536));
        }
        #endregion

        #region Strings
        /// <summary>
        /// Writes string to output stream.
        /// </summary>
        /// <param name="value"><see cref="string"/> to write.</param>
        public void WriteUtf8(string value, bool nullTerminated)
        {
            var b = Encoding.UTF8.GetBytes(value);
            if (nullTerminated)
            {
                _stream.Write(b, 0, b.Length);
                _stream.WriteByte(0);
            }
            else
            {
                //TODO:
                WriteUInt32((uint)b.Length);
                _stream.Write(b, 0, b.Length);
            }
        }
        #endregion

        public void WriteUInt24(uint value)
        {
            Align();
            _stream.WriteByte((byte)value);
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)(value >> 16));
        }

        public void WriteUInt24BE(uint value)
        {
            Align();
            _stream.WriteByte((byte)(value >> 16));
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)value);
        }

        public void WriteUInt24(uint[] value)
        {
            int n = value.Length;
            for (int i = 0; i < n; ++i)
                WriteUInt24(value[i]);
        }

        public void WriteInt24(int value)
        {
            WriteUInt24((uint)value);
        }

        public void WriteInt24(int[] value)
        {
            int n = value.Length;
            for (int i = 0; i < n; ++i)
                WriteInt24(value[i]);
        }

        public void WriteSingle(float value)
        {
            _binWriter.Write(value);
        }

        public void WriteSingle(float[] value)
        {
            int n = value.Length;
            for (int i = 0; i < n; ++i)
                WriteSingle(value[i]);
        }

        public void WriteDouble(double value)
        {
            _binWriter.Write(value);
        }

        public void WriteDouble(double[] value)
        {
            int n = value.Length;
            for (int i = 0; i < n; ++i)
                WriteDouble(value[i]);
        }

        public static int SizeOfUIntEncoded(uint value)
        {
            if (value < 128) return 1;
            if (value < 16384) return 2;
            if (value < 2097152) return 3;
            if (value < 268435456) return 4;
            return 5;
        }

        public static int SizeOfIntEncoded(int value)
        {
            return SizeOfUIntEncoded((uint)value);
        }

        public void WriteUIntEncoded(uint value)
        {
            if (value == 0)
            {
                WriteByte(0);
                return;
            }

            Align();
            if (value < 128)
            {
                _stream.WriteByte((byte)value);
            }
            else if (value < 16384)
            {
                byte first = (byte)((value & 0x7F) | 0x80);
                byte second = (byte)(value >> 7);
                _stream.WriteByte(first);
                _stream.WriteByte(second);
            }
            else if (value < 2097152)
            {
                byte first = (byte)((value & 0x7F) | 0x80);
                byte second = (byte)((value >> 7) | 0x80);
                byte third = (byte)(value >> 14);
                _stream.WriteByte(first);
                _stream.WriteByte(second);
                _stream.WriteByte(third);
            }
            else if (value < 268435456)
            {
                byte first = (byte)((value & 0x7F) | 0x80);
                byte second = (byte)((value >> 7) | 0x80);
                byte third = (byte)((value >> 14) | 0x80);
                byte fourth = (byte)(value >> 21);
                _stream.WriteByte(first);
                _stream.WriteByte(second);
                _stream.WriteByte(third);
                _stream.WriteByte(fourth);
            }
            else
            {
                byte first = (byte)((value & 0x7F) | 0x80);
                byte second = (byte)((value >> 7) | 0x80);
                byte third = (byte)((value >> 14) | 0x80);
                byte fourth = (byte)((value >> 21) | 0x80);
                byte fifth = (byte)(value >> 28);
                _stream.WriteByte(first);
                _stream.WriteByte(second);
                _stream.WriteByte(third);
                _stream.WriteByte(fourth);
                _stream.WriteByte(fifth);
            }
        }

        public void WriteUIntEncoded(int value)
        {
            WriteUIntEncoded((uint)value);
        }

        public void WriteIntEncoded(int value)
        {
            WriteUIntEncoded((uint)value);
        }
        #endregion

        #region String
        public void WriteString(string s)
        {
            var e = _fileVersion <= 5 ? Encoding.ASCII : Encoding.UTF8;
            Write(e.GetBytes(s));
            WriteUInt8(0);
        }
        #endregion

        #region Tag
        /// <summary>
        /// Writes tag header.
        /// </summary>
        /// <param name="code">The code of tag.</param>
        /// <param name="length">The length of tag.</param>
        public void WriteTagHeader(int code, int length)
        {
            if (length <= 62)
            {
                WriteUInt16((ushort)((code << 6) | length));
            }
            else
            {
                WriteUInt16((ushort)((code << 6) | 0x3F));
                WriteUInt32((uint)length);
            }
        }

        /// <summary>
        /// Writes tag to output stream.
        /// </summary>
        /// <param name="code">The code of tag to write.</param>
        /// <param name="data">The tag data to write.</param>
        public void WriteTag(int code, byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                WriteTagHeader(code, data.Length);
                Write(data);
            }
            else
            {
                WriteTagHeader(code, 0);
            }
        }
        #endregion

        #region Simple structured data types
        #region Color
        /// <summary>
        /// Writes only RGB field from <see cref="Color"/> structure to output stream
        /// </summary>
        /// <param name="value"><see cref="Color"/> structure to write.</param>
        public void WriteRGB(Color value)
        {
            Align();
            _stream.WriteByte(value.R);
            _stream.WriteByte(value.G);
            _stream.WriteByte(value.B);
        }

        /// <summary>
        /// Writes <see cref="Color"/> structure to output stream
        /// </summary>
        /// <param name="value"><see cref="Color"/> structure to write.</param>
        public void WriteRGBA(Color value)
        {
            Align();
            _stream.WriteByte(value.R);
            _stream.WriteByte(value.G);
            _stream.WriteByte(value.B);
            _stream.WriteByte(value.A);
        }

        /// <summary>
        /// Writes <see cref="Color"/> structure to output stream
        /// </summary>
        /// <param name="value"><see cref="Color"/> structure to write.</param>
        public void WriteARGB(Color value)
        {
            Align();
            _stream.WriteByte(value.A);
            _stream.WriteByte(value.R);
            _stream.WriteByte(value.G);
            _stream.WriteByte(value.B);
        }

        public void WriteColor(Color value, bool alpha)
        {
            if (alpha) WriteRGBA(value);
            else WriteRGB(value);
        }
        #endregion

        #region Rectangle
        /// <summary>
        /// Writes rectangle defined by <i>x1</i>, <i>y1</i>, <i>x2</i> and <i>y2</i> parameters in twips.
        /// </summary>
        /// <param name="x1">The left coordinate of rectangle in twips.</param>
        /// <param name="y1">The top coordinate of rectangle in twips.</param>
        /// <param name="x2">The right coordinate of rectangle in twips.</param>
        /// <param name="y2">The bottom coordinate of rectangle in twips.</param>
        public void WriteRect(int x1, int y1, int x2, int y2)
        {
            int len = x1.GetMinBits(x2, y1, y2);
            WriteUB((uint)len, 5);
            WriteSB(x1, len);
            WriteSB(x2, len);
            WriteSB(y1, len);
            WriteSB(y2, len);
            //NOTE: The RECT record must be byte aligned.
            Align();
        }

        /// <summary>
        /// Writes rectangle defined by <see cref="Rectangle"/> structure. All coordinates are measured in twips.
        /// </summary>
        /// <param name="rect"><see cref="Rectangle"/> structure to write.</param>
        public void WriteRect(Rectangle rect)
        {
            WriteRect(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Writes rectangle defined by <i>x1</i>, <i>y1</i>, <i>x2</i> and <i>y2</i> parameters in app units.
        /// </summary>
        /// <param name="x1">The left coordinate of rectangle in app units.</param>
        /// <param name="y1">The top coordinate of rectangle in app units.</param>
        /// <param name="x2">The right coordinate of rectangle in app units.</param>
        /// <param name="y2">The bottom coordinate of rectangle in app units.</param>
        public void WriteRectTwip(float x1, float y1, float x2, float y2)
        {
            WriteRect(SwfHelper.ToTwips(x1),
                      SwfHelper.ToTwips(y1),
                      SwfHelper.ToTwips(x2),
                      SwfHelper.ToTwips(y2));
        }

        /// <summary>
        /// Writes rectangle defined by <see cref="Rectangle"/> structure. All coordinates are measured in app units.
        /// </summary>
        /// <param name="rect"><see cref="RectangleF"/> structure to write.</param>
        public void WriteRectTwip(RectangleF rect)
        {
            WriteRectTwip(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        #endregion

        #region Matrix
        /// <summary>
        /// Writes 2D transformation matrix.
        /// </summary>
        /// <param name="value"><see cref="Matrix"/> object to write.</param>
        public void WriteMatrix(Matrix value)
        {
            if (value == null) //identity matrix
                value = new Matrix();

            const int SX = 0;
            const int SY = 3;

            var e = value.Elements;
            bool hasScale = e[SX] != 1 || e[SY] != 1;
            WriteBit(hasScale);
            if (hasScale)
            {
                int scaleX = e[SX].FixedToInt32();
                int scaleY = e[SY].FixedToInt32();
                int scaleBits = scaleX.GetMinBits(scaleY);
                WriteUB((uint)scaleBits, 5);
                WriteSB(scaleX, scaleBits);
                WriteSB(scaleY, scaleBits);
            }

            const int RX = 1;
            const int RY = 2;

            bool hasRotate = e[RX] != 0 || e[RY] != 0;
            WriteBit(hasRotate);
            if (hasRotate)
            {
                int rotateX = e[RX].FixedToInt32();
                int rotateY = e[RY].FixedToInt32();
                int rotateBits = rotateX.GetMinBits(rotateY);
                WriteUB((uint)rotateBits, 5);
                WriteSB(rotateX, rotateBits);
                WriteSB(rotateY, rotateBits);
            }

            WriteBitwiseTwipPoint(e[4], e[5], true);
            //NOTE: The MATRIX record must be byte aligned.
            Align();
        }
        #endregion
        #endregion

        public void WriteTwipU16(float value)
        {
            ushort v = (ushort)SwfHelper.ToTwips(value);
            WriteUInt16(v);
        }

        public void WriteTwipS16(float value)
        {
            short v = (short)SwfHelper.ToTwips(value);
            WriteInt16(v);
        }

        public void WriteTwip(float value, int bits)
        {
            int t = SwfHelper.ToTwips(value);
            WriteSB(t, bits);
        }

        public void WriteBitwiseTwipPoint(float x, float y, bool checkZero)
        {
            int tx = SwfHelper.ToTwips(x);
            int ty = SwfHelper.ToTwips(y);
            if (checkZero && tx == 0 && ty == 0)
            {
                WriteUB(0, 5);
                return;
            }
            int bits = tx.GetMinBits(ty);
            WriteUB((uint)bits, 5);
            WriteSB(tx, bits);
            WriteSB(ty, bits);
        }

        #region Shapes
        public SwfStyles Styles
        {
            get { return _styles; }
            set { _styles = value; }
        }
        private SwfStyles _styles;

        public void WriteFillStyle(int index)
        {
            WriteUB((uint)index, _styles.FillBits);
        }

        public void WriteLineStyle(int index)
        {
            WriteUB((uint)index, _styles.LineBits);
        }
        #endregion
    }
}