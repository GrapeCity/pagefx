namespace DataDynamics.ZLib
{
	using System;

	internal class InfCodes
	{
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

		// waiting for "i:"=input,
		//             "o:"=output,
		//             "x:"=nothing
		private const int START = 0;  // x: set up for LEN
		private const int LEN = 1;    // i: get length/literal/eob next
		private const int LENEXT = 2; // i: getting length extra (have base)
		private const int DIST = 3;   // i: get distance next
		private const int DISTEXT = 4;// i: getting distance extra
		private const int COPY = 5;   // o: copying bytes in window, waiting for space
		private const int LIT = 6;    // o: got literal, waiting for output space
		private const int WASH = 7;   // o: got eob, possibly still output waiting
		private const int END = 8;    // x: got eob and all data flushed
		private const int BADCODE = 9;// x: got error

		int m_nMode; // current inflate_codes mode

		// mode dependent information
		int m_nLength;

		int[] m_anTree; // pointer into tree
		int m_nTreeIndex = 0;
		int m_nNeed;   // bits needed
		int m_nLiteral;

		// if EXT or COPY, where and how much
		int m_nBitsToGetForExtra; // bits to get for extra
		int m_nDistanceBackToCopyFrom; // distance back to copy from

		byte m_bLiteralBits; // ltree bits decoded per branch
		byte m_bDistanceBits; // dtree bits decoder per branch
		int[] m_anLiteralTree; // literal/length/eob tree
		int m_nLiteralTreeIndex; // literal/length/eob tree
		int[] m_anDistanceTree; // distance tree
		int m_nDistanceTreeIndex; // distance tree

		public InfCodes(int literalBits, int distanceBits, int[] aliteralTree, int literalTreeIndex, int[] adistanceTree, int distanceTreeIndex)
		{
			m_nMode = START;
			m_bLiteralBits = (byte)literalBits;
			m_bDistanceBits = (byte)distanceBits;
			m_anLiteralTree = aliteralTree;
			m_nLiteralTreeIndex = literalTreeIndex;
			m_anDistanceTree = adistanceTree;
			m_nDistanceTreeIndex = distanceTreeIndex;
		}

		public InfCodes(int literalBits, int distanceBits, int[] aliteralTree, int[] adistanceTree)
		{
			m_nMode = START;
			m_bLiteralBits = (byte)literalBits;
			m_bDistanceBits = (byte)distanceBits;
			m_anLiteralTree = aliteralTree;
			m_nLiteralTreeIndex = 0;
			m_anDistanceTree = adistanceTree;
			m_nDistanceTreeIndex = 0;
		}

		internal ReturnCodes Process(InfBlocks infBlocks, ZStream zstream, ReturnCodes result)
		{ 
			int j; // temporary storage
			int tindex; // temporary pointer
			int e; // extra bits or operation
			int b = 0; // bit buffer
			int k = 0; // bits in bit buffer
			int p = 0; // input data pointer
			int n; // bytes available there
			int q; // output window write pointer
			int m; // bytes to end of window or read pointer
			int f; // pointer to copy strings from

			// Copy input/output information to locals (UPDATE macro restores)
			p = zstream.m_nNextInIndex;
			n = zstream.m_nAvailIn;
			b = infBlocks.m_nBitBuffer;
			k = infBlocks.m_nBitInBitBuffer;
			q = infBlocks.m_nWrite;
			m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;

			// process input and output based on current state
			while (true)
			{
				switch (m_nMode)
				{
					// waiting for "i:"=input, "o:"=output, "x:"=nothing
					case START:         // x: set up for LEN
						if (m >= 258 && n >= 10)
						{
							infBlocks.m_nBitBuffer = b;
							infBlocks.m_nBitInBitBuffer = k;
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							infBlocks.m_nWrite = q;
							result = InflateFast(m_bLiteralBits, m_bDistanceBits, m_anLiteralTree, m_nLiteralTreeIndex, m_anDistanceTree, m_nDistanceTreeIndex, infBlocks, zstream);
							p = zstream.m_nNextInIndex;
							n = zstream.m_nAvailIn;
							b = infBlocks.m_nBitBuffer;
							k = infBlocks.m_nBitInBitBuffer;
							q = infBlocks.m_nWrite;
							m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;

							if (result != ReturnCodes.ZLibOk)
							{
								m_nMode = (result == ReturnCodes.ZLibStreamEnd) ? WASH : BADCODE;
								break;
							}
						}
						m_nNeed = m_bLiteralBits;
						m_anTree = m_anLiteralTree;
						m_nTreeIndex = m_nLiteralTreeIndex;
						m_nMode = LEN;
						break;

					case LEN: // i: get length/literal/eob next
						j = m_nNeed;
						while (k < j)
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{
								infBlocks.m_nBitBuffer = b;
								infBlocks.m_nBitInBitBuffer = k;
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex = p;
								infBlocks.m_nWrite = q;
								return infBlocks.InflateFlush(zstream, result);
							}
							n--;
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k+=8;
						}
						tindex = (m_nTreeIndex + (b & m_anInflateMask[j])) * 3;
						b >>= (m_anTree[tindex+1]);
						k -= (m_anTree[tindex+1]);
						e = m_anTree[tindex];
						if (e == 0)
						{  
							// literal
							m_nLiteral = m_anTree[tindex+2];
							m_nMode = LIT;
							break;
						}
						if ((e & 16) != 0)
						{
							// length
							m_nBitsToGetForExtra = e & 15;
							m_nLength = m_anTree[tindex + 2];
							m_nMode = LENEXT;
							break;
						}
						if ((e & 64) == 0)
						{
							// next table
							m_nNeed = e;
							m_nTreeIndex = tindex / 3 + m_anTree[tindex + 2];
							break;
						}
						if ((e & 32)!=0)
						{
							// end of block
							m_nMode = WASH;
							break;
						}
						m_nMode = BADCODE;        // invalid code
						zstream.ErrorInfo("Invalid literal/length code", "InfCodes", 196);
						result = ReturnCodes.ZLibDataError;
						infBlocks.m_nBitBuffer = b;
						infBlocks.m_nBitInBitBuffer = k;
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						infBlocks.m_nWrite = q;
						return infBlocks.InflateFlush(zstream, result);

					case LENEXT: // i: getting length extra (have base)
						j = m_nBitsToGetForExtra;

						while(k<(j))
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{

								infBlocks.m_nBitBuffer = b;
								infBlocks.m_nBitInBitBuffer = k;
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex = p;
								infBlocks.m_nWrite = q;
								return infBlocks.InflateFlush(zstream, result);
							}
							n--; 
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k += 8;
						}
						m_nLength += (b & m_anInflateMask[j]);
						b >>= j;
						k -= j;
						m_nNeed = m_bDistanceBits;
						m_anTree = m_anDistanceTree;
						m_nTreeIndex = m_nDistanceTreeIndex;
						m_nMode = DIST;
						break;

					case DIST:          // i: get distance next
						j = m_nNeed;
						while (k < j)
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{
								infBlocks.m_nBitBuffer = b;
								infBlocks.m_nBitInBitBuffer = k;
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex=p;
								infBlocks.m_nWrite = q;
								return infBlocks.InflateFlush(zstream, result);
							}
							n--; 
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k += 8;
						}
						tindex = (m_nTreeIndex + (b & m_anInflateMask[j])) * 3;

						b >>= m_anTree[tindex+1];
						k -= m_anTree[tindex+1];

						e = (m_anTree[tindex]);
						if ((e & 16) != 0)
						{   
							// distance
							m_nBitsToGetForExtra = e & 15;
							m_nDistanceBackToCopyFrom = m_anTree[tindex+2];
							m_nMode = DISTEXT;
							break;
						}
						if ((e & 64) == 0)
						{   
							// next table
							m_nNeed = e;
							m_nTreeIndex = tindex / 3 + m_anTree[tindex+2];
							break;
						}
						m_nMode = BADCODE;        // invalid code
						zstream.ErrorInfo("Invalid distance code", "InfCodes", 279);
						result = ReturnCodes.ZLibDataError;
						infBlocks.m_nBitBuffer = b;
						infBlocks.m_nBitInBitBuffer = k;
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						infBlocks.m_nWrite = q;
						return infBlocks.InflateFlush(zstream, result);

					case DISTEXT:// i: getting distance extra
						j = m_nBitsToGetForExtra;
						while (k < j)
						{
							if (n != 0)
								result = ReturnCodes.ZLibOk;
							else
							{
								infBlocks.m_nBitBuffer = b;
								infBlocks.m_nBitInBitBuffer = k;
								zstream.m_nAvailIn = n;
								zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
								zstream.m_nNextInIndex = p;
								infBlocks.m_nWrite=q;
								return infBlocks.InflateFlush(zstream, result);
							}
							n--; 
							b |= (zstream.m_abNextIn[p++] & 0xff) << k;
							k+=8;
						}
						m_nDistanceBackToCopyFrom += (b & m_anInflateMask[j]);
						b >>= j;
						k -= j;
						m_nMode = COPY;
						break;

					case COPY: // o: copying bytes in window, waiting for space
						f = (q < m_nDistanceBackToCopyFrom) ? infBlocks.m_nEnd - (m_nDistanceBackToCopyFrom - q) : q - m_nDistanceBackToCopyFrom;
						while (m_nLength != 0)
						{
							if (m == 0)
							{
								if (q == infBlocks.m_nEnd && infBlocks.m_nRead != 0)
								{
									q = 0;
									m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
								}
								if (m == 0)
								{
									infBlocks.m_nWrite = q; 
									result = infBlocks.InflateFlush(zstream, result);
									q = infBlocks.m_nWrite;
									m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
									if (q == infBlocks.m_nEnd && infBlocks.m_nRead != 0)
									{
										q = 0;
										m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
									}
									if (m == 0)
									{
										infBlocks.m_nBitBuffer = b;
										infBlocks.m_nBitInBitBuffer = k;
										zstream.m_nAvailIn = n;
										zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
										zstream.m_nNextInIndex = p;
										infBlocks.m_nWrite = q;
										return infBlocks.InflateFlush(zstream, result);
									}  
								}
							}
							infBlocks.m_bSlidingWindow[q++] = infBlocks.m_bSlidingWindow[f++]; 
							m--;
							if (f == infBlocks.m_nEnd)
								f = 0;
							m_nLength--;
						}
						m_nMode = START;
						break;

					case LIT:           // o: got literal, waiting for output space
						if (m == 0)
						{
							if(q == infBlocks.m_nEnd && infBlocks.m_nRead != 0)
							{
								q = 0;
								m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
							}
							if (m == 0)
							{
								infBlocks.m_nWrite = q; 
								result = infBlocks.InflateFlush(zstream, result);
								q = infBlocks.m_nWrite;
								m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
								if (q == infBlocks.m_nEnd && infBlocks.m_nRead != 0)
								{
									q = 0;
									m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
								}
								if (m == 0)
								{
									infBlocks.m_nBitBuffer = b;
									infBlocks.m_nBitInBitBuffer = k;
									zstream.m_nAvailIn = n;
									zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
									zstream.m_nNextInIndex = p;
									infBlocks.m_nWrite = q;
									return infBlocks.InflateFlush(zstream, result);
								}
							}
						}
						result = ReturnCodes.ZLibOk;
						infBlocks.m_bSlidingWindow[q++] = (byte)m_nLiteral; 
						m--;
						m_nMode = START;
						break;

					case WASH:           // o: got eob, possibly more output
						if (k > 7)
						{   
							// return unused byte, if any
							k -= 8;
							n++;
							p--; // can always return one
						}
						infBlocks.m_nWrite = q; 
						result = infBlocks.InflateFlush(zstream, result);
						q = infBlocks.m_nWrite;
						m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;
						if (infBlocks.m_nRead != infBlocks.m_nWrite)
						{
							infBlocks.m_nBitBuffer = b;
							infBlocks.m_nBitInBitBuffer= k;
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							infBlocks.m_nWrite = q;
							return infBlocks.InflateFlush(zstream, result);
						}
						m_nMode = END;
						break;

					case END:
						result = ReturnCodes.ZLibStreamEnd;
						infBlocks.m_nBitBuffer = b;
						infBlocks.m_nBitInBitBuffer = k;
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						infBlocks.m_nWrite = q;
						return infBlocks.InflateFlush(zstream, result);

					case BADCODE:       // x: got error
						result = ReturnCodes.ZLibDataError;
						infBlocks.m_nBitBuffer = b;
						infBlocks.m_nBitInBitBuffer = k;
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						infBlocks.m_nWrite = q;
						return infBlocks.InflateFlush(zstream, result);

					default:
						result = ReturnCodes.ZLibStreamError;
						infBlocks.m_nBitBuffer = b;
						infBlocks.m_nBitInBitBuffer = k;
						zstream.m_nAvailIn = n;
						zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
						zstream.m_nNextInIndex = p;
						infBlocks.m_nWrite = q;
						return infBlocks.InflateFlush(zstream, result);
				}
			}
		}

		public void Free(ZStream z)
		{
		}

		// Called with number of bytes left to write in window at least 258
		// (the maximum string length) and number of input bytes available
		// at least ten.  The ten bytes are six bytes for the longest length/
		// distance pair plus four bytes for overloading the bit buffer.

		ReturnCodes InflateFast(int bl, int bd, int[] tl, int tlIndex, int[] td, int tdIndex, InfBlocks infBlocks, ZStream zstream)
		{
			try
			{
				int t; // temporary pointer
				int[] tp; // temporary pointer
				int tp_index; // temporary pointer
				int e; // extra bits or operation
				int b; // bit buffer
				int k; // bits in bit buffer
				int p; // input data pointer
				int n; // bytes available there
				int q; // output window write pointer
				int m; // bytes to end of window or read pointer
				int ml; // mask for literal/length tree
				int md; // mask for distance tree
				int c; // bytes to copy
				int d; // distance back to copy from
				int r; // copy source pointer

				// load input, output, bit values
				p = zstream.m_nNextInIndex;
				n = zstream.m_nAvailIn;
				b = infBlocks.m_nBitBuffer;
				k = infBlocks.m_nBitInBitBuffer;
				q = infBlocks.m_nWrite;
				m = q < infBlocks.m_nRead ? infBlocks.m_nRead - q - 1 : infBlocks.m_nEnd - q;

				// initialize masks
				ml = m_anInflateMask[bl];
				md = m_anInflateMask[bd];

				// do until not enough input or output space for fast loop
				do 
				{                          // assume called with m >= 258 && n >= 10
					// get literal/length code
				while (k < 20)
				{   
					// max bits for literal/length code
					n--;
					b |= (zstream.m_abNextIn[p++] & 0xff) << k;
					k += 8;
				}
					t = b & ml;
					tp = tl; 
					tp_index = tlIndex;
					if ((e = tp[(tp_index + t) * 3]) == 0)
					{
						b >>= (tp[(tp_index + t) * 3 + 1]); 
						k -= (tp[(tp_index + t) * 3 + 1]);
						infBlocks.m_bSlidingWindow[q++] = (byte)tp[(tp_index + t) * 3 + 2];
						m--;
						continue;
					}
					do 
					{
						b >>= (tp[(tp_index + t) * 3 + 1]); 
						k -= (tp[(tp_index + t) * 3 + 1]);
						if ((e&16) != 0)
						{
							e &= 15;
							c = tp[(tp_index+t)*3+2] + ((int)b & m_anInflateMask[e]);
							b >>= e; 
							k -= e;
							// decode distance base of block to copy
							while (k < 15)
							{           
								// max bits for distance code
								n--;
								b |= (zstream.m_abNextIn[p++] & 0xff) << k;
								k += 8;
							}
							t = b & md;
							tp = td;
							tp_index = tdIndex;
							e = tp[(tp_index + t) * 3];

							do 
							{
								b >>= (tp[(tp_index + t) * 3 + 1]); 
								k -= (tp[(tp_index + t) * 3 + 1]);
								if ((e & 16) != 0)
								{
									// get extra bits to add to distance base
									e &= 15;
									while(k < e)
									{
										// get extra bits (up to 13)
										n--;
										b |= (zstream.m_abNextIn[p++] & 0xff) << k;
										k += 8;
									}
									d = tp[(tp_index + t) * 3 + 2] + (b & m_anInflateMask[e]);
									b >>= (e); 
									k -= (e);
									// do the copy
									m -= c;
									if (q >= d)
									{
										// offset before dest just copy
										r = q - d;
										if (q - r > 0 && 2 > (q - r))
										{
											infBlocks.m_bSlidingWindow[q++] = infBlocks.m_bSlidingWindow[r++]; 
											c--; // minimum count is three,
											infBlocks.m_bSlidingWindow[q++] = infBlocks.m_bSlidingWindow[r++]; 
											c--; // so unroll loop a little
										}
										else
										{
											Array.Copy(infBlocks.m_bSlidingWindow, r, infBlocks.m_bSlidingWindow, q, 2);
											q += 2; 
											r += 2; 
											c -= 2;
										}
									}
									else
									{
										// else offset after destination
										e = d - q;
										// bytes from offset to end
										r = infBlocks.m_nEnd - e;
										// pointer to offset
										if (c > e)
										{
											// if source crosses,
											c -= e;
											// copy to end of window
											if (q - r > 0 && e > (q-r))
											{           
												do
												{
													infBlocks.m_bSlidingWindow[q++] = infBlocks.m_bSlidingWindow[r++];
												}
												while(--e != 0);
											}
											else
											{
												Array.Copy(infBlocks.m_bSlidingWindow, r, infBlocks.m_bSlidingWindow, q, e);
												q+=e; 
												r+=e; 
												e=0;
											}
											r = 0;
											// copy rest from start of window
										}
									}	
									// copy all or what's left
									if (q - r > 0 && c > (q - r))
									{           
										do
										{
											infBlocks.m_bSlidingWindow[q++] = infBlocks.m_bSlidingWindow[r++];
										}
										while (--c != 0);
									}
									else
									{
										Array.Copy(infBlocks.m_bSlidingWindow, r, infBlocks.m_bSlidingWindow, q, c);
										q += c; 
										r += c; 
										c = 0;
									}
									break;
								}
								else if ((e&64) == 0)
								{
									t += tp[(tp_index + t) * 3 + 2];
									t += (b & m_anInflateMask[e]);
									e = tp[(tp_index + t) * 3];
								}
								else
								{
									zstream.ErrorInfo("Invalid distance code", "InfCodes", 636);
									c = zstream.m_nAvailIn - n;
									c = (k >> 3) < c ? k >> 3 : c;
									n += c;
									p -= c;
									k -= c << 3;
									infBlocks.m_nBitBuffer = b;
									infBlocks.m_nBitInBitBuffer = k;
									zstream.m_nAvailIn = n;
									zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
									zstream.m_nNextInIndex = p;
									infBlocks.m_nWrite = q;
									return ReturnCodes.ZLibDataError;
								}
							}
							while(true);
							break;
						}
						if ((e & 64) == 0)
						{
							t += tp[(tp_index + t) * 3 + 2];
							t += (b & m_anInflateMask[e]);
							if ((e = tp[(tp_index + t) * 3]) == 0)
							{
								b >>= (tp[(tp_index + t) * 3 + 1]); 
								k -= (tp[(tp_index + t) * 3 + 1]);
								infBlocks.m_bSlidingWindow[q++] = (byte)tp[(tp_index + t) * 3 + 2];
								m--;
								break;
							}
						}
						else if ((e&32) != 0)
						{
							c = zstream.m_nAvailIn - n;
							c = (k >> 3) < c ? k >> 3 : c;
							n += c;
							p -= c;
							k -= c << 3;
							infBlocks.m_nBitBuffer = b;
							infBlocks.m_nBitInBitBuffer = k;
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							infBlocks.m_nWrite = q;
							return ReturnCodes.ZLibStreamEnd;
						}
						else
						{
							zstream.ErrorInfo("Invalid literal/length code", "InfCodes", 683);
							c = zstream.m_nAvailIn - n;
							c =(k >> 3) < c ? k >> 3 : c;
							n += c;
							p -= c;
							k -= c << 3;
							infBlocks.m_nBitBuffer = b;
							infBlocks.m_nBitInBitBuffer = k;
							zstream.m_nAvailIn = n;
							zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
							zstream.m_nNextInIndex = p;
							infBlocks.m_nWrite = q;
							return ReturnCodes.ZLibDataError;
						}
					} 
					while (true);
				} 
				while (m >= 258 && n >= 10);

				// not enough input or output--restore pointers and return
				c = zstream.m_nAvailIn - n;
				c = (k >> 3) < c ? k >> 3 : c;
				n += c;
				p -= c;
				k -= c << 3;
				infBlocks.m_nBitBuffer = b;
				infBlocks.m_nBitInBitBuffer = k;
				zstream.m_nAvailIn = n;
				zstream.m_nTotalIn += p - zstream.m_nNextInIndex;
				zstream.m_nNextInIndex = p;
				infBlocks.m_nWrite = q;
				return ReturnCodes.ZLibOk;
			}
			catch (Exception)
			{
				return ReturnCodes.ZLibDataError;
			}
		}
	}
}