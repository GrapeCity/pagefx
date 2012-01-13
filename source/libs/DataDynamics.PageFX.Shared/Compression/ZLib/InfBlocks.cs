namespace DataDynamics.ZLib
{
	using System;
	using System.Diagnostics;

	internal class InfBlocks
	{
		private const int MANY = 1440;

		// And'ing with mask[n] masks the lower n bits
		static private int[] m_anInflateMask = 
		{
			0x00000000, 
			0x00000001, 
			0x00000003, 
			0x00000007, 
			0x0000000f,
			0x0000001f, 
			0x0000003f, 
			0x0000007f, 
			0x000000ff, 
			0x000001ff,
			0x000003ff, 
			0x000007ff, 
			0x00000fff, 
			0x00001fff, 
			0x00003fff,
			0x00007fff, 
			0x0000ffff
		};

		// Table for deflate from PKZIP's appnote.txt.
		static int[] anBorder = 
		{ 
			// Order of the bit length code lengths
			16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15
		};

		private const int TYPE = 0;  // get type bits (3, including end bit)
		private const int LENS = 1;  // get lengths for stored
		private const int STORED = 2;// processing stored block
		private const int TABLE = 3; // get table lengths
		private const int BTREE = 4; // get bit lengths tree for a dynamic block
		private const int DTREE = 5; // get length, distance trees for a dynamic block
		private const int CODES = 6; // processing fixed or dynamic block
		private const int DRY = 7;   // output remaining window bytes
		private const int DONE = 8;  // finished last block, done
		private const int BAD = 9;   // ot a data error--stuck here

		public int m_nInflateBlockMode; // current inflate_block mode 
		public int m_nBytesLeftToCopy; // if STORED, bytes left to copy 
		public int m_nTable; // table lengths (14 bits) 
		public int m_nIndex; // index into blens (or border) 
		public int[] m_anBitLength; // bit lengths of codes 
		public int[] m_anBitLengthTreeDepth = new int[1]; // bit length tree depth 
		public int[] m_anBitLengthDecodingTree = new int[1]; // bit length decoding tree 

		public InfCodes m_theCodes; // if CODES, current state 

		public bool m_bLastBlock; // true if this block is the last block 

		// mode independent information 
		public int m_nBitInBitBuffer; // bits in bit buffer 
		public int m_nBitBuffer; // bit buffer 
		public int[] m_anHufts; // single malloc for tree space 
		public byte[] m_bSlidingWindow; // sliding window 
		public int m_nEnd; // one byte after sliding window 
		public int m_nRead; // window read pointer 
		public int m_nWrite; // window write pointer 
		public object m_objCheckFn; // check function TODO this need to be fixed
		public long m_nCheck; // check on output 

		internal InfBlocks(ZStream zstream, Object objCheckFn, int windowSize)
		{
			m_anHufts = new int[MANY * 3];
			m_bSlidingWindow = new byte[windowSize];
			m_nEnd = windowSize;
			m_objCheckFn = objCheckFn;
			m_nInflateBlockMode = TYPE;
			Reset(zstream, null);
		}

		internal void Reset(ZStream zstream, long[] ancheck)
		{
			if (ancheck != null) 
				ancheck[0] = m_nCheck;

			if (m_nInflateBlockMode == BTREE || m_nInflateBlockMode == DTREE)
				m_anBitLength = null;

			if (m_nInflateBlockMode == CODES)
				m_theCodes.Free(zstream);

			m_nInflateBlockMode = TYPE;
			m_nBitInBitBuffer = 0;
			m_nBitBuffer = 0;
			m_nRead = m_nWrite = 0;
			if (m_objCheckFn != null)
				zstream.m_nAdler = m_nCheck = zstream.m_theAdler.Calculate(0L, null, 0, 0);
		}

		internal ReturnCodes Process(ZStream zstream, ReturnCodes result)
		{
			int t = 0; // temporary storage
			int b = 0; // bit buffer
			int k = 0; // bits in bit buffer
			int p = 0; // input data pointer
			int n = 0; // bytes available there
			int q = 0; // output window write pointer
			int m = 0; // bytes to end of window or m_nRead pointer

			// copy input/output information to locals (UPDATE macro restores)
			p = zstream.m_nNextInIndex;
			n = zstream.m_nAvailIn;
			b = m_nBitBuffer;
			k = m_nBitInBitBuffer;
			q = m_nWrite;
			m = (int)(q < m_nRead ? m_nRead - q - 1 : m_nEnd - q);

			// process input based on current state
			while (true)
			{
				switch (m_nInflateBlockMode)
				{
					case TYPE:
						while (k < 3)
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{
								m_nBitBuffer = b; 
								m_nBitInBitBuffer = k; 
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex >>= p;
								m_nWrite = q;
								return InflateFlush(zstream, result);
							}
							n--;
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k += 8;
						}
						t = (int)(b & 7);
						m_bLastBlock = (0 != (t & 1)) ? true : false;

						switch (t >> 1)
						{
						case 0: // stored 
							b >>= 3;
							k -= 3;
							t = k & 7; // go to byte boundary
							b >>= t;
							k -= t;
							m_nInflateBlockMode = LENS;// get length of stored block
							break;

						case 1: // fixed
							{
								int[] bl = new int[1];
								int[] bd = new int[1];
								int[][] tl = new int[1][];
								int[][] td=new int[1][];

								InfTree.InflateTreesFixed(bl, bd, tl, td, zstream);
								m_theCodes = new InfCodes(bl[0], bd[0], tl[0], td[0]);
							}
							b >>= 3;
							k -= 3;
							m_nInflateBlockMode = CODES;
							break;

						case 2: // dynamic
							b >>= 3;
							k -= 3;
							m_nInflateBlockMode = TABLE;
							break;

						case 3: // illegal
							b >>= 3;
							k -= 3;
							m_nInflateBlockMode = BAD;
							zstream.ErrorInfo("Invalid Block Type", "InfBlocks", 183);
							result = ReturnCodes.ZLibDataError;
							m_nBitBuffer = b; 
							m_nBitInBitBuffer = k; 
							zstream.m_nAvailIn =n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							m_nRead = q;
							return InflateFlush(zstream, result);
						}
						break;

					case LENS:
						while(k < 32)
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{
								m_nBitBuffer = b; 
								m_nBitInBitBuffer = k; 
								zstream.m_nAvailIn=n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex = p;
								m_nWrite = q;
								return InflateFlush(zstream, result);
							}
							n--;
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k += 8;
						}

						if ((((~b) >> 16) & 0xffff) != (b & 0xffff))
						{
							m_nInflateBlockMode = BAD;
							zstream.ErrorInfo("Invalid Stored Block Lengths", "InfBlocks", 218);
							result = ReturnCodes.ZLibDataError;
							m_nBitBuffer = b; 
							m_nBitInBitBuffer = k; 
							zstream.m_nAvailIn >>= n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							m_nWrite = q;
							return InflateFlush(zstream, result);
						}
						m_nBytesLeftToCopy = (b & 0xffff);
						b = k = 0;                       // dump bits
						m_nInflateBlockMode = m_nBytesLeftToCopy != 0 ? STORED : (m_bLastBlock ? DRY : TYPE);
						break;

					case STORED:
						if (n == 0)
						{
							m_nBitBuffer = b; 
							m_nBitInBitBuffer = k; 
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							m_nWrite = q;
							return InflateFlush(zstream, result);
						}

						if (m == 0)
						{
							if (q == m_nEnd && m_nRead != 0)
							{
								q = 0; 
								m = (int)(q < m_nRead ? m_nRead - q - 1 : m_nEnd - q);
							}
							if (m == 0)
							{
								m_nWrite = q; 
								result = InflateFlush(zstream, result);
								q = m_nWrite;
								m = (int)( q < m_nRead ? m_nRead - q - 1 : m_nEnd - q);

								if (q == m_nEnd && m_nRead != 0)
								{
									q = 0; 
									m = (int)(q < m_nRead ? m_nRead - q - 1 : m_nEnd - q);
								}
								if (m == 0)
								{
									m_nBitBuffer = b; 
									m_nBitInBitBuffer = k; 
									zstream.m_nAvailIn = n;
									zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
									zstream.m_nNextInIndex = p;
									m_nWrite = q;
									return InflateFlush(zstream, result);
								}
							}
						}
						result = ReturnCodes.ZLibOk;
						t = m_nBytesLeftToCopy;
						if (t > n) 
							t = n;
						if (t > m) 
							t = m;
						Array.Copy(zstream.m_abNextIn, p, m_bSlidingWindow, q, t);
						p += t;  
						n -= t;
						q += t;  
						m -= t;
						if ((m_nBytesLeftToCopy -= t) != 0)
							break;
						m_nInflateBlockMode = m_bLastBlock ? DRY : TYPE;
						break;

					case TABLE:
						while(k < 14)
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{
								m_nBitBuffer = b; 
								m_nBitInBitBuffer = k; 
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex = p;
								m_nWrite = q;
								return InflateFlush(zstream, result);
							};
							n--;
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k += 8;
						}
						m_nTable = t = (b & 0x3fff);
						if ((t & 0x1f) > 29 || ((t >> 5) & 0x1f) > 29)
						{
							m_nInflateBlockMode = BAD;
							zstream.ErrorInfo("Too many length or distance symbols", "InfBlocks", 315);
							result = ReturnCodes.ZLibDataError;
							m_nBitBuffer = b; 
							m_nBitInBitBuffer = k; 
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							m_nWrite = q;
							return InflateFlush(zstream, result);
						}
						t = 258 + (t & 0x1f) + ((t >> 5) & 0x1f);
						m_anBitLength = new int[t];
						{
							b >>= 14;
							k -= 14;
						}
						m_nIndex = 0;
						m_nInflateBlockMode = BTREE;
						break;

					case BTREE:
						while (m_nIndex < 4 + (m_nTable >> 10))
						{
							while (k < 3)
							{
								if (n != 0)
									result = ReturnCodes.ZLibOk;
								else
								{
									m_nBitBuffer = b; 
									m_nBitInBitBuffer = k; 
									zstream.m_nAvailIn = n;
									zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
									zstream.m_nNextInIndex = p;
									m_nWrite = q;
									return InflateFlush(zstream, result);
								}
								n--;
								b |= (zstream.m_abNextIn[p++] & 0xff) << k;
								k += 8;
							}
						
							m_anBitLength[anBorder[m_nIndex++]] = b & 7;
							b >>= 3;
							k -= 3;
						}

						while (m_nIndex < 19)
							m_anBitLength[anBorder[m_nIndex++]] = 0;
						
						m_anBitLengthTreeDepth[0] = 7;
						ReturnCodes aReturnCode = InfTree.InflateTreesBits(m_anBitLength, m_anBitLengthTreeDepth, m_anBitLengthDecodingTree, m_anHufts, zstream);
						if (aReturnCode != ReturnCodes.ZLibOk)
						{
							m_anBitLength = null;
							result = aReturnCode;
							if (result == ReturnCodes.ZLibDataError)
								m_nInflateBlockMode = BAD;
							m_nBitBuffer = b; 
							m_nBitInBitBuffer = k; 
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							m_nWrite = q;
							return InflateFlush(zstream, result);
						}
						m_nIndex = 0;
						m_nInflateBlockMode = DTREE;
						break;

					case DTREE:
						while (true)
						{
							t = m_nTable;
							if (!(m_nIndex < 258 + (t & 0x1f) + ((t >> 5) & 0x1f)))
								break;

							int i, j, c;
							t = m_anBitLengthTreeDepth[0];
							while (k < t)
							{
								if (n != 0)
									result = ReturnCodes.ZLibOk;
								else
								{
									m_nBitBuffer = b; 
									m_nBitInBitBuffer = k; 
									zstream.m_nAvailIn = n;
									zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
									zstream.m_nNextInIndex = p;
									m_nWrite = q;
									return InflateFlush(zstream, result);
								};
								n--;
								b |= (zstream.m_abNextIn[p++] & 0xff) << k;
								k += 8;
							}
//#if DEBUG
//							if (m_anBitLengthDecodingTree[0] == -1)
//								Debug.WriteLine("null...");
//#endif
							t = m_anHufts[(m_anBitLengthDecodingTree[0] + (b & m_anInflateMask[t])) * 3 + 1];
							c = m_anHufts[(m_anBitLengthDecodingTree[0] + (b & m_anInflateMask[t])) * 3 + 2];

							if (c < 16)
							{
								b >>= t;
								k -= t;
								m_anBitLength[m_nIndex++] = c;
							}
							else 
							{ 
								// c == 16..18
								i = c == 18 ? 7 : c - 14;
								j = c == 18 ? 11 : 3;
								while( k < (t+i))
								{
									if (n != 0)
										result = ReturnCodes.ZLibOk;
									else
									{
										m_nBitBuffer = b; 
										m_nBitInBitBuffer = k; 
										zstream.m_nAvailIn = n;
										zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
										zstream.m_nNextInIndex = p;
										m_nWrite = q;
										return InflateFlush(zstream, result);
									}
									n--;
									b |= (zstream.m_abNextIn[p++] & 0xff) << k;
									k += 8;
								}
								b >>= t;
								k -= t;
								j += (b & m_anInflateMask[i]);
								b >>= i;
								k -= i;
								i = m_nIndex;
								t = m_nTable;
								if (i + j > 258 + (t & 0x1f) + ((t >> 5) & 0x1f) || (c == 16 && i < 1))
								{
									m_anBitLength = null;
									m_nInflateBlockMode = BAD;
									zstream.ErrorInfo("Invalid bit length repeat", "InfBlocks", 457);
									result = ReturnCodes.ZLibDataError;
									m_nBitBuffer = b; 
									m_nBitInBitBuffer = k; 
									zstream.m_nAvailIn = n;
									zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
									zstream.m_nNextInIndex = p;
									m_nWrite = q;
									return InflateFlush(zstream, result);
								}
								c = c == 16 ? m_anBitLength[i - 1] : 0;
								do
								{
									m_anBitLength[i++] = c;
								}
								while (--j != 0);
								m_nIndex = i;
							}
						}
						m_anBitLengthDecodingTree[0]=-1;
						{
							int[] bl = new int[1];
							int[] bd = new int[1];
							int[] tl = new int[1];
							int[] td = new int[1];
							bl[0] = 9;         // must be <= 9 for lookahead assumptions
							bd[0] = 6;         // must be <= 9 for lookahead assumptions
							t = m_nTable;
							aReturnCode = InfTree.InflateTreesDynamic(257 + (t & 0x1f), 1 + ((t >> 5) & 0x1f), m_anBitLength, bl, bd, tl, td, m_anHufts, zstream);
							m_anBitLength = null;
							if (aReturnCode != ReturnCodes.ZLibOk)
							{
								if (aReturnCode == ReturnCodes.ZLibDataError)
									m_nInflateBlockMode = BAD;
								result = aReturnCode;
								m_nBitBuffer = b; 
								m_nBitInBitBuffer = k; 
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex = p;
								m_nWrite = q;
								return InflateFlush(zstream, result);
							}

							m_theCodes = new InfCodes(bl[0], bd[0], m_anHufts, tl[0], m_anHufts, td[0]);
						}
						m_nInflateBlockMode = CODES;
						break;

					case CODES:
						m_nBitBuffer = b; 
						m_nBitInBitBuffer = k;
						zstream.m_nAvailIn = n; 
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						m_nWrite = q;
						if ((result = m_theCodes.Process(this, zstream, result)) != ReturnCodes.ZLibStreamEnd)
							return InflateFlush(zstream, result);
						result = ReturnCodes.ZLibOk;
						m_theCodes.Free(zstream);
						p = zstream.m_nNextInIndex; 
						n = zstream.m_nAvailIn;
						b = m_nBitBuffer;
						k = m_nBitInBitBuffer;
						q = m_nWrite;
						m = (int)(q < m_nRead ? m_nRead - q - 1 : m_nEnd - q);
						if (!m_bLastBlock)
						{
							m_nInflateBlockMode = TYPE;
							break;
						}
						m_nInflateBlockMode = DRY;
						break;

					case DRY:
						m_nWrite = q; 
						result = InflateFlush(zstream, result); 
						q = m_nWrite; 
						m = (int)(q < m_nRead ? m_nRead - q - 1 : m_nEnd - q);
						if (m_nRead != m_nWrite)
						{
							m_nBitBuffer = b; 
							m_nBitInBitBuffer = k; 
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							m_nWrite = q;
							return InflateFlush(zstream, result);
						}
						m_nInflateBlockMode = DONE;
						break;

					case DONE:
						result = ReturnCodes.ZLibStreamEnd;
						m_nBitBuffer = b; 
						m_nBitInBitBuffer = k; 
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						m_nWrite = q;
						return InflateFlush(zstream, result);

					case BAD:
						result = ReturnCodes.ZLibDataError;
						m_nBitBuffer = b; 
						m_nBitInBitBuffer = k; 
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						m_nWrite = q;
						return InflateFlush(zstream, result);

					default:
						result = ReturnCodes.ZLibStreamError;
						m_nBitBuffer = b; 
						m_nBitInBitBuffer = k; 
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						m_nWrite = q;
						return InflateFlush(zstream, result);
				}
			}
		}

		public void Free(ZStream zstream)
		{
			Reset(zstream, null);
			m_bSlidingWindow = null;
			m_anHufts = null;
		}

		public void SetDictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, m_bSlidingWindow, 0, n);
			m_nRead = m_nWrite = n;
		}

		// Returns true if inflate is currently at the end of a block generated
		// by Z_SYNC_FLUSH or Z_FULL_FLUSH. 
		public bool SyncPoint()
		{
			return (m_nInflateBlockMode == LENS);
		}

		// copy as much as possible from the sliding window to the output area
		public ReturnCodes InflateFlush(ZStream zstream, ReturnCodes result)
		{
			int n;
			int p;
			int q;

			// local copies of source and destination pointers
			p = zstream.m_nNextOutIndex;
			q = m_nRead;

			// compute number of bytes to copy as far as end of window
			n = (int)((q <= m_nWrite ? m_nWrite : m_nEnd) - q);
			if (n > zstream.m_nAvailOut) 
				n = zstream.m_nAvailOut;
			if (n != 0 && result == ReturnCodes.ZLibBufferError) 
				result = ReturnCodes.ZLibOk;

			// update counters
			zstream.m_nAvailOut -= n;
			zstream.m_nTotalOut += n;

			// update check information
			if (m_objCheckFn != null)
				zstream.m_nAdler = m_nCheck = zstream.m_theAdler.Calculate(m_nCheck, m_bSlidingWindow, q, n);

			// copy as far as end of window
			Array.Copy(m_bSlidingWindow, q, zstream.m_abNextOut, p, n);
			p += n;
			q += n;

			// see if more to copy at beginning of window
			if (q == m_nEnd)
			{
				// wrap pointers
				q = 0;
				if (m_nWrite == m_nEnd)
					m_nWrite = 0;

				// compute bytes to copy
				n = m_nWrite - q;
				if (n > zstream.m_nAvailOut) 
					n = zstream.m_nAvailOut;

				if (n != 0 && result == ReturnCodes.ZLibBufferError) 
					result = ReturnCodes.ZLibOk;

				// update counters
				zstream.m_nAvailOut -= n;
				zstream.m_nTotalOut += n;

				// update check information
				if (m_objCheckFn != null)
					zstream.m_nAdler = m_nCheck = zstream.m_theAdler.Calculate(m_nCheck, m_bSlidingWindow, q, n);

				// copy
				Array.Copy(m_bSlidingWindow, q, zstream.m_abNextOut, p, n);
				p += n;
				q += n;
			}

			// update pointers
			zstream.m_nNextOutIndex = p;
			m_nRead = q;

			// done
			return result;
		}
	}
}