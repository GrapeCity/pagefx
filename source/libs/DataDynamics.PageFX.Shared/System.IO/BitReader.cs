namespace System.IO
{
    /// <summary>
    /// Represents reader that cna read bit-by-bit in bigendian byte ordering.
    /// </summary>
    public class BitReader
    {
        #region Member Variables
        private readonly Stream _stream;
        private int _curbyte = -1;
        private int _bitpos = 7;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="BitReader"/> with specified stream and bits per code.
        /// </summary>
        /// <param name="s">The stream to read.</param>
        public BitReader(Stream s)
        {
            _stream = s;
        }

        /// <summary>
        /// Initializes new instance of <see cref="BitReader"/> with specified data and bits per code.
        /// </summary>
        /// <param name="data">The buffer to read.</param>
        public BitReader(byte[] data)
            : this(new MemoryStream(data))
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

        /// <summary>
        /// Gets or sets the stream position.
        /// </summary>
        public long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        /// <summary>
        /// Gets or sets bit position in the current byte.
        /// </summary>
        public int BitPosition
        {
            get { return _bitpos; }
            set
            {
                if (value < 0 || value > 7)
                    throw new ArgumentOutOfRangeException("value");
                _bitpos = value;
            }
        }

        public void ResetState()
        {
            _curbyte = -1;
            _bitpos = 7;
        }

        /// <summary>
        /// Reads next bit from the stream.
        /// </summary>
        /// <returns>The delivered bit.</returns>
        public bool ReadBit()
        {
            if (_curbyte < 0)
            {
                _curbyte = _stream.ReadByte();
                if (_curbyte == -1)
                    throw new EndOfStreamException();
                _bitpos = 6;
                return (_curbyte & 0x80) != 0;
            }
            int mask = 1 << _bitpos;
            int value = _curbyte & mask;
            --_bitpos;
            if (_bitpos < 0)
            {
                _curbyte = -1;
                _bitpos = 7;
            }
            return value != 0;
        }

        /// <summary>
        /// Reads code with specified length.
        /// </summary>
        /// <param name="len">The length of code to read.</param>
        /// <returns>The delivered code.</returns>
        public int ReadCode(int len)
        {
            if (len <= 0) return 0;
            if (_curbyte < 0)
            {
                _curbyte = _stream.ReadByte();
                if (_curbyte == -1)
                    throw new EndOfStreamException();
                _bitpos = 7;
            }
            int res = 0;
            while (true)
            {
                int v = _curbyte;
                int rem = _bitpos + 1;					// Number of remaining bits in the current byte
                if (len >= rem)
                {
                    // We have already bits at the begin of current byte.
                    // Therefore no need to make shift operation.
                    int mask = 0xff >> (8 - rem);
                    v &= mask;				            // Clears unnecessary bits.
                    len -= rem;							// Decreases number of bits to read.
                    v <<= len;							// Shifts bits to the end of free bits in result code.
                    res |= v;							// Writes bits to result code.
                    _bitpos = 7;						// Set bit cursor to the start of next byte.

                    if (len == 0)
                    {
                        _curbyte = -1;
                        return res;
                    }

                    _curbyte = _stream.ReadByte();
                    if (_curbyte == -1)
                        throw new EndOfStreamException();
                }
                else
                {
                    v >>= rem - len;					// Shifts bits to read to the begin of byte.
                    v &= 0xff >> (8 - len);				// Clears unnecessary bits.
                    res |= v;							// Writes bits to result code.
                    _bitpos -= len;						// Moves bit cursor to the next bit to read.
                    break;
                }
            }
            if (_bitpos < 0)
            {
                _curbyte = -1;
                _bitpos = 7;
            }
            return res;
        }

        /// <summary>
        /// If bit position is not equal to 7 this method reads next byte and set bit position to 7.
        /// </summary>
        public void Align()
        {
            if (_bitpos != 7)
            {
                _curbyte = _stream.ReadByte();
                _bitpos = 7;
            }
        }
        #endregion
    }
}