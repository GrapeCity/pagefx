using System;

namespace DataDynamics.ZLib
{
	internal class Deflate
	{
		private const int MAX_MEM_LEVEL = 9;
		private const int MAX_WBITS = 15; // 32K LZ77 window
		private const int DEF_MEM_LEVEL = 8;

		internal delegate int CompressDelegate(FlushTypes flush);

		internal class Config
		{
			public int m_nGoodLength; // reduce lazy search above this match length
			public int m_nMaxLazy;    // do not perform lazy search above this match length
			public int m_nNiceLength; // quit search above this match length
			public int m_nMaxChain;
			public CompressDelegate m_Compress;

			internal Config(int goodLength, int maxLazy, int niceLength, int maxChain, CompressDelegate cdDelegate)
			{
				m_nGoodLength = goodLength;
				m_nMaxLazy = maxLazy;
				m_nNiceLength = niceLength;
				m_nMaxChain = maxChain;
				m_Compress = cdDelegate;
			}
		}
	  
		private Config[] m_aConfigTable;

		// block not completed, need more input or more output
		private const int m_nNeedMore = 0; 

		// block flush performed
		private const int m_nBlockDone = 1; 

		// finish started, need only more output at next deflate
		private const int m_nFinishStarted=2;

		// finish done, accept no more input or output
		private const int m_nFinishDone=3;

		// preset dictionary flag in zlib header
		private const int PRESET_DICT = 0x20;

		internal enum States
		{
			INIT=42,
			BUSY=113,
			FINISH=666
		}

		// The deflate compression method
		private const int Z_DEFLATED=8;

		private const int STORED_BLOCK = 0;
		private const int STATIC_TREES = 1;
		private const int DYN_TREES = 2;

		// The three kinds of block type
		internal enum BlockTypes
		{
			Z_BINARY=0,
			Z_ASCII=1,
			Z_UNKNOWN=2
		}

		private const int Buf_size=8*2;

		// repeat previous bit length 3-6 times (2 bits of repeat count)
		private const int REP_3_6=16; 

		// repeat a zero length 3-10 times  (3 bits of repeat count)
		private const int REPZ_3_10=17; 

		// repeat a zero length 11-138 times  (7 bits of repeat count)
		private const int REPZ_11_138 = 18; 

		private const int MIN_MATCH = 3;
		private const int MAX_MATCH = 258;
		private const int MIN_LOOKAHEAD = (MAX_MATCH+MIN_MATCH+1);

		private const int MAX_BITS = 15;
		private const int D_CODES = 30;
		private const int BL_CODES = 19;
		private const int LENGTH_CODES = 29;
		private const int LITERALS = 256;
		private const int L_CODES = (LITERALS+1+LENGTH_CODES);
		private const int HEAP_SIZE = (2*L_CODES+1);

		private const int END_BLOCK = 256;

		public ZStream m_strm; // pointer back to this zlib stream
		public States m_nStatus; // as the name implies
		public byte[] m_bPendingBuffer; // output still pending
		public int m_nPendingBufferSize; // size of pending_buf
		public int m_nPendingOut;      // next pending byte to output to the stream
		public int m_nPending;          // nb of bytes in the pending buffer
		public int m_nNoHeader;         // suppress zlib header and adler32
		public byte m_bDataType;       // UNKNOWN, BINARY or ASCII
		public byte m_bMethod;          // STORED (for zip only) or DEFLATED
		public FlushTypes m_nLastFlush;       // value of flush param for previous deflate call

		public int m_nWindowSize;           // LZ77 window size (32K by default)
		public int m_nWindowBits;           // log2(w_size)  (8..16)
		public int m_nWindowMask;    // w_size - 1

		public byte[] m_abWindow;
		// Sliding window. Input bytes are read into the second half of the window,
		// and move to the first half later to keep a dictionary of at least wSize
		// bytes. With this organization, matches are limited to a distance of
		// wSize-MAX_MATCH bytes, but this ensures that IO is always
		// performed with a length multiple of the block size. Also, it limits
		// the window size to 64K, which is quite useful on MSDOS.
		// To do: use the user input buffer as sliding window.

		public int m_nActualWindowSize;
		// Actual size of window: 2*wSize, except when the user input buffer
		// is directly used as sliding window.

		public short[] m_anPrev;
		// Link to older string with same hash index. To limit the size of this
		// array to 64K, this link is maintained only for the last 32K strings.
		// An index in this array is thus a window index modulo 32K.

		public short[] m_anHeadOfHash; // Heads of the hash chains or NIL.

		public int m_nHashIndexStringToInsert;          // hash index of string to be inserted
		public int m_nHashSize;      // number of elements in hash table
		public int m_nHashBits;      // log2(hash_size)
		public int m_nHashMask;      // hash_size-1

		// Number of bits by which ins_h must be shifted at each input
		// step. It must be such that after MIN_MATCH steps, the oldest
		// byte no longer takes part in the hash key, that is:
		// hash_shift * MIN_MATCH >= hash_bits
		public int m_nHashShift;

		// Window position at the beginning of the current output block. Gets
		// negative when the window is moved backwards.

		public int m_nBlockStart;

		public int m_nMatchLength; // length of best match
		public int m_nPrevMatch; // previous match
		public int m_nMatchAvailable; // set if previous match exists
		public int m_nStringStartInsert; // start of string to insert
		public int m_nMatchStringStart; // start of matching string
		public int m_nLookAheadBytes; // number of valid bytes ahead in window

		// Length of the best match at previous step. Matches not greater than this
		// are discarded. This is used in the lazy match evaluation.
		public int m_nPrevLength;

		// To speed up deflation, hash chains are never searched beyond this
		// length.  A higher limit improves compression ratio but degrades the speed.
		public int m_nMaxChainLength;

		// Attempt to find a better match only when the current match is strictly
		// smaller than this value. This mechanism is used only for compression
		// levels >= 4.
		public int m_nMaxLazyMatch;

		// Insert new strings in the hash table only if the match length is not
		// greater than this length. This saves time but degrades compression.
		// max_insert_length is used only for compression levels <= 3.

		public int m_nLevel; // compression level (1..9)
		public int m_nStrategy; // favor or force Huffman coding

		// Use a faster search when the previous match is longer than this
		public int m_nGoodMatch;

		// Stop searching when current match exceeds this
		public int m_nNiceMatch;

		public short[] m_anDynLiteralTree; // literal and length tree
		public short[] m_anDynDistanceTree; // distance tree
		public short[] m_anBLTree; // Huffman tree for bit lengths

		public Tree m_tLiteralDesc = new Tree();  // desc for literal tree
		public Tree m_tDistanceDesc = new Tree();  // desc for distance tree
		public Tree m_tBLDesc = new Tree(); // desc for bit length tree

		// number of codes at each bit length for an optimal tree
		public short[] m_anBLCount = new short[MAX_BITS+1];

		// heap used to build the Huffman trees
		public int[] m_anHeap = new int[2*L_CODES+1];

		public int m_nHeapLen; // number of elements in the heap
		public int m_nHeapMax; // element of largest frequency
		// The sons of heap[n] are heap[2*n] and heap[2*n+1]. heap[0] is not used.
		// The same heap array is used to build all trees.

		// Depth of each subtree used as tie breaker for trees of equal frequency
		public byte[] m_abDepth = new byte[2 * L_CODES + 1];

		public int m_nLiteralIndex; // index for literals or lengths */

		// Size of match buffer for literals/lengths.  There are 4 reasons for
		// limiting lit_bufsize to 64K:
		//   - frequencies can be kept in 16 bit counters
		//   - if compression is not successful for the first block, all input
		//     data is still in the window so we can still emit a stored block even
		//     when input comes from standard input.  (This can also be done for
		//     all blocks if lit_bufsize is not greater than 32K.)
		//   - if compression is not successful for a file smaller than 64K, we can
		//     even emit a stored file instead of a stored block (saving 5 bytes).
		//     This is applicable only for zip (not gzip or zlib).
		//   - creating new Huffman trees less frequently may not provide fast
		//     adaptation to changes in the input data statistics. (Take for
		//     example a binary file with poorly compressible code followed by
		//     a highly compressible string table.) Smaller buffer sizes give
		//     fast adaptation but have of course the overhead of transmitting
		//     trees more frequently.
		//   - I can't count above 4
		public int m_nLiteralBufferSize;

		public int m_nLastLiteralIndex;      // running index in l_buf

		// Buffer for distances. To simplify the code, d_buf and l_buf have
		// the same number of elements. To use different lengths, an extra flag
		// array would be necessary.

		public int m_nDistanceIndex; // index of pendig_buf

		public int m_nOptBitLen;        // bit length of current block with optimal trees
		public int m_StaticTreeLen;     // bit length of current block with static trees
		public int m_nMatches;        // number of string matches in current block
		public int m_nLastEOBLen;   // bit length of EOB code for last block

		// Output buffer. bits are inserted starting at the bottom (least
		// significant bits).
		public ushort m_bBufferOutput;

		// Number of valid bits in bi_buf.  All bits above the last valid bit
		// are always zero.
		public int m_nValidBitsInBufferOut;

		public Deflate()
		{
			m_anDynLiteralTree = new short[HEAP_SIZE*2];
			m_anDynDistanceTree = new short[(2*D_CODES+1)*2]; // distance tree
			m_anBLTree = new short[(2*BL_CODES+1)*2];  // Huffman tree for bit lengths
			m_aConfigTable = new[]
			                 	{
			                 		new Config(0, 0, 0, 0, DeflateStored),
			                 		new Config(4, 4, 8, 4, DeflateFast),
			                 		new Config(4, 5, 16, 8, DeflateFast),
			                 		new Config(4, 6, 32, 32, DeflateFast),
			                 		new Config(4, 4, 16, 16, DeflateSlow),
			                 		new Config(8, 16, 32, 32, DeflateSlow),
			                 		new Config(8, 16, 128, 128, DeflateSlow),
			                 		new Config(8, 32, 128, 256, DeflateSlow),
			                 		new Config(32, 128, 258, 1024, DeflateSlow),
			                 		new Config(32, 258, 258, 4096, DeflateSlow)
			                 	};
		}

		void LiteralMatchInit() 
		{
			m_nActualWindowSize = 2 * m_nWindowSize;
			m_anHeadOfHash[m_nHashSize - 1] = 0;
			for (int i = 0; i < m_nHashSize - 1; i++)
				m_anHeadOfHash[i] = 0;

			// Set the default configuration parameters:
			m_nMaxLazyMatch = m_aConfigTable[m_nLevel].m_nMaxLazy;
			m_nGoodMatch = m_aConfigTable[m_nLevel].m_nGoodLength;
			m_nNiceMatch = m_aConfigTable[m_nLevel].m_nNiceLength;
			m_nMaxChainLength = m_aConfigTable[m_nLevel].m_nMaxChain;

			m_nStringStartInsert = 0;
			m_nBlockStart = 0;
			m_nLookAheadBytes = 0;
			m_nMatchLength = m_nPrevLength = MIN_MATCH - 1;
			m_nMatchAvailable = 0;
			m_nHashIndexStringToInsert = 0;
		}

		// Initialize the tree data structures for a new zlib stream.
		void TreeInit()
		{
			m_tLiteralDesc.m_anDynTree = m_anDynLiteralTree;
			m_tLiteralDesc.m_stDesc = StaticTree.m_stLDesc;

			m_tDistanceDesc.m_anDynTree = m_anDynDistanceTree;
			m_tDistanceDesc.m_stDesc = StaticTree.m_stDDesc;

			m_tBLDesc.m_anDynTree = m_anBLTree;
			m_tBLDesc.m_stDesc = StaticTree.m_stBLDesc;

			m_bBufferOutput = 0;
			m_nValidBitsInBufferOut = 0;
			m_nLastEOBLen = 8; // enough lookahead for inflate

			// Initialize the first block of the first file:
			InitBlock();
		}

		void InitBlock()
		{
			// Initialize the trees.
			for (int i = 0; i < L_CODES; i++) 
				m_anDynLiteralTree[i * 2] = 0;
			for (int i = 0; i < D_CODES; i++) 
				m_anDynDistanceTree[i * 2] = 0;
			for (int i = 0; i < BL_CODES; i++) 
				m_anBLTree[i*2] = 0;
			m_anDynLiteralTree[END_BLOCK * 2] = 1;
			m_nOptBitLen = m_StaticTreeLen = 0;
			m_nLastLiteralIndex = m_nMatches = 0;
		}

		// Restore the heap property by moving down the tree starting at node k,
		// exchanging a node with the smallest of its two sons if necessary, stopping
		// when the heap property is re-established (each father smaller than its
		// two sons).
		// short[] tree the tree to restore
		// int k node to move down
		public void PQDownHeap(short[] tree, int k)
		{
			int v = m_anHeap[k];
			int j = k << 1;  // left son of k
			while (j <= m_nHeapLen) 
			{
				// Set j to the smallest of the two sons:
				if (j < m_nHeapLen && Smaller(tree, m_anHeap[j+1], m_anHeap[j], m_abDepth))
				{
					j++;
				}
				// Exit if v is smaller than both sons
				if (Smaller(tree, v, m_anHeap[j], m_abDepth)) 
					break;
				// Exchange v with the smallest son
				m_anHeap[k]=m_anHeap[j];  
				k = j;
				// And continue down the tree, setting j to the left son of k
				j <<= 1;
			}
			m_anHeap[k] = v;
		}

		static bool Smaller(short[] tree, int n, int m, byte[] depth)
		{
			return (tree[n*2] < tree[m*2] || (tree[n*2] == tree[m*2] && depth[n] <= depth[m]));
		}

		// Scan a literal or distance tree to determine the frequencies of the codes
		// in the bit length tree.
		// short[] tree the tree to be scanned
		// int max_code and its largest code of non zero frequency
		void ScanTree (short[] tree, int maxCode)
		{
			int n;                     // iterates over all tree elements
			int prevlen = -1;          // last emitted length
			int curlen;                // length of current code
			int nextlen = tree[0*2+1]; // length of next code
			int count = 0;             // repeat count of the current code
			int max_count = 7;         // max repeat count
			int min_count = 4;         // min repeat count

			if (nextlen == 0)
			{ 
				max_count = 138; 
				min_count = 3; 
			}
			tree[(maxCode+1)*2+1] = 0xff; // guard

			for(n = 0; n <= maxCode; n++) 
			{
				curlen = nextlen; 
				nextlen = tree[(n+1)*2+1];
				if (++count < max_count && curlen == nextlen) 
					continue;
				else if (count < min_count) 
				{
					m_anBLTree[curlen*2] += (short)count;
				}
				else if (curlen != 0) 
				{
					if (curlen != prevlen) 
						m_anBLTree[curlen*2]++;
					m_anBLTree[REP_3_6*2]++;
				}
				else if(count <= 10) 
				{
					m_anBLTree[REPZ_3_10*2]++;
				}
				else
				{
					m_anBLTree[REPZ_11_138*2]++;
				}
				count = 0; 
				prevlen = curlen;
				if(nextlen == 0) 
				{
					max_count = 138; 
					min_count = 3;
				}
				else if(curlen == nextlen) 
				{
					max_count = 6; 
					min_count = 3;
				}
				else
				{
					max_count = 7; 
					min_count = 4;
				}
			}
		}

		// Construct the Huffman tree for the bit lengths and return the index in
		// bl_order of the last bit length code to send.
		int BuildBitLengthTree()
		{
			int max_blindex;  // index of last bit length code of non zero freq

			// Determine the bit length frequencies for literal and distance trees
			ScanTree(m_anDynLiteralTree, m_tLiteralDesc.m_nMaxCode);
			ScanTree(m_anDynDistanceTree, m_tDistanceDesc.m_nMaxCode);

			// Build the bit length tree:
			m_tBLDesc.BuildTree(this);
			// opt_len now includes the length of the tree representations, except
			// the lengths of the bit lengths codes and the 5+5+4 bits for the counts.

			// Determine the number of bit length codes to send. The pkzip format
			// requires that at least 4 bit length codes be sent. (appnote.txt says
			// 3 but the actual value used is 4.)
			for (max_blindex = BL_CODES - 1; max_blindex >= 3; max_blindex--) 
			{
				if (m_anBLTree[Tree.m_abBLOrder[max_blindex] * 2 + 1] != 0) 
					break;
			}
			// Update opt_len to include the bit length tree and counts
			m_nOptBitLen += 3 * (max_blindex + 1) + 5 + 5 + 4;

			return max_blindex;
		}


		// Send the header for a block using dynamic Huffman trees: the counts, the
		// lengths of the bit length codes, the literal tree and the distance tree.
		// IN assertion: lcodes >= 257, dcodes >= 1, blcodes >= 4.
		void SendAllTrees(int lcodes, int dcodes, int blcodes)
		{
			int rank; // index in bl_order
			SendBits(lcodes - 257, 5); // not +255 as stated in appnote.txt
			SendBits(dcodes - 1, 5);
			SendBits(blcodes - 4, 4); // not -3 as stated in appnote.txt
			for (rank = 0; rank < blcodes; rank++) 
				SendBits(m_anBLTree[Tree.m_abBLOrder[rank] * 2 + 1], 3);
			SendTree(m_anDynLiteralTree, lcodes - 1); // literal tree
			SendTree(m_anDynDistanceTree, dcodes - 1); // distance tree
		}

		// Send a literal or distance tree in compressed form, using the codes in
		// bl_tree.
		// short[] tree the tree to be sent
		// int max_code and its largest code of non zero frequency
		public void SendTree (short[] antree, int maxCode)
		{
			int n; // iterates over all tree elements
			int prevlen = -1; // last emitted length
			int nCurLen; // length of current code
			int nNextLen = antree[0 * 2 + 1]; // length of next code
			int nCount = 0; // repeat count of the current code
			int max_count = 7; // max repeat count
			int min_count = 4; // min repeat count

			if (nNextLen == 0)
			{ 
				max_count = 138; 
				min_count = 3; 
			}

			for (n = 0; n <= maxCode; n++) 
			{
				nCurLen = nNextLen; 
				nNextLen = antree[(n+1)*2+1];
				if (++nCount < max_count && nCurLen == nNextLen) 
				{
					continue;
				}
				else if (nCount < min_count) 
				{
					do 
					{ 
						SendCode(nCurLen, m_anBLTree); 
					} 
					while (--nCount != 0);
				}
				else if (nCurLen != 0)
				{
					if (nCurLen != prevlen)
					{
						SendCode(nCurLen, m_anBLTree); 
						nCount--;
					}
					SendCode(REP_3_6, m_anBLTree); 
					SendBits(nCount - 3, 2);
				}
				else if (nCount <= 10)
				{
					SendCode(REPZ_3_10, m_anBLTree); 
					SendBits(nCount - 3, 3);
				}
				else
				{
					SendCode(REPZ_11_138, m_anBLTree);
					SendBits(nCount - 11, 7);
				}
				nCount = 0; 
				prevlen = nCurLen;
				if (nNextLen == 0)
				{
					max_count = 138; 
					min_count = 3;
				}
				else if (nCurLen == nNextLen)
				{
					max_count = 6; 
					min_count = 3;
				}
				else
				{
					max_count = 7; 
					min_count = 4;
				}
			}
		}

		// Output a byte on the stream.
		// IN assertion: there is enough room in pending_buf.
		public void PutByte(byte[] abbytes, int start, int length)
		{
			Array.Copy(abbytes, start, m_bPendingBuffer, m_nPending, length);
			m_nPending += length;
		}

		public void PutByte(byte abyte)
		{
			m_bPendingBuffer[m_nPending++] = abyte;
		}

		public void PutShort(ushort value) 
		{
			PutByte((byte)(value & 0XFF));
			PutByte((byte)((value >> 8)));
		}

		public void PutShortMSB(uint b)
		{
			PutByte((byte)(b >> 8));
			PutByte((byte)(b & 0XFF));
		}   

		void SendCode(int code, short[] antree)
		{
			SendBits((antree[code * 2] & 0xffff), (antree[code * 2 + 1] & 0xffff));
		} 

		public void SendBits(int value, int length)
		{
			int len = length;
			if (m_nValidBitsInBufferOut > (int)Buf_size - len) 
			{
				int val = value;
				m_bBufferOutput |= (ushort)((val << m_nValidBitsInBufferOut) & 0xffff);
				PutShort(m_bBufferOutput);
				m_bBufferOutput = (ushort)(val >> (Buf_size - m_nValidBitsInBufferOut));
				m_nValidBitsInBufferOut += len - Buf_size;
			} 
			else 
			{
				m_bBufferOutput |= (ushort)(((value) << m_nValidBitsInBufferOut)&0xffff);
				m_nValidBitsInBufferOut += len;
			}
		}

		// Send one empty static block to give enough lookahead for inflate.
		// This takes 10 bits, of which 7 may remain in the bit buffer.
		// The current inflate code requires 9 bits of lookahead. If the
		// last two codes for the previous block (real code plus EOB) were coded
		// on 5 bits or less, inflate may have only 5+3 bits of lookahead to decode
		// the last real code. In this case we send two empty static blocks instead
		// of one. (There are no problems if the previous block is stored or fixed.)
		// To simplify the code, we assume the worst case of last real code encoded
		// on one bit only.
		public void TreeAlign()
		{
			SendBits(STATIC_TREES << 1, 3);
			SendCode(END_BLOCK, StaticTree.m_anStaticLTree);

			BIFlush();

			// Of the 10 bits for the empty block, we have already sent
			// (10 - bi_valid) bits. The lookahead for the last real code (before
			// the EOB of the previous block) was thus at least one plus the length
			// of the EOB plus what we have just sent of the empty static block.
			if (1 + m_nLastEOBLen + 10 - m_nValidBitsInBufferOut < 9) 
			{
				SendBits(STATIC_TREES << 1, 3);
				SendCode(END_BLOCK, StaticTree.m_anStaticLTree);
				BIFlush();
			}
			m_nLastEOBLen = 7;
		}

		// Save the match info and tally the frequency counts. Return true if
		// the current block must be flushed.
		// int dist distance of matched string
		// int lc match length-MIN_MATCH or unmatched char (if dist==0)
		public bool TreeTally (int dist, int nlc)
		{
			m_bPendingBuffer[m_nDistanceIndex + m_nLastLiteralIndex * 2] = (byte)(dist >> 8);
			m_bPendingBuffer[m_nDistanceIndex + m_nLastLiteralIndex * 2 + 1] = (byte)dist;

			m_bPendingBuffer[m_nLiteralIndex + m_nLastLiteralIndex] = (byte)nlc; 
			m_nLastLiteralIndex++;

			if (dist == 0) 
			{
				// lc is the unmatched char
				m_anDynLiteralTree[nlc * 2]++;
			} 
			else 
			{
				m_nMatches++;
				// Here, lc is the match length - MIN_MATCH
				dist--; // dist = match distance - 1
				m_anDynLiteralTree[(Tree.m_abLengthCode[nlc] + LITERALS + 1) * 2]++;
				m_anDynDistanceTree[Tree.DistanceCode(dist) * 2]++;
			}

			if ((m_nLastLiteralIndex & 0xfff) == 0 && m_nLevel > 2) 
			{
				// Compute an upper bound for the compressed length
				int nOutLength = m_nLastLiteralIndex * 8;
				int nInLength = m_nStringStartInsert - m_nBlockStart;
				for (int nDCode = 0; nDCode < D_CODES; nDCode++) 
					nOutLength += (int)(m_anDynDistanceTree[nDCode * 2] * (5L + Tree.m_anExtraDistanceBits[nDCode]));
				nOutLength >>= 3;
				if ((m_nMatches < (m_nLastLiteralIndex / 2)) && nOutLength < nInLength / 2) 
					return true;
			}
			return (m_nLastLiteralIndex == m_nLiteralBufferSize - 1);
			// We avoid equality with lit_bufsize because of wraparound at 64K
			// on 16 bit machines and because stored blocks are restricted to
			// 64K-1 bytes.
		}

		// Send the block data compressed using the given Huffman trees
		public void CompressBlock(short[] anltree, short[] dtree)
		{
			int nDist; // distance of matched string
			int nLC; // match length or unmatched char (if dist == 0)
			int lx = 0; // running index in l_buf
			int nCode; // the code to send
			int nExtra; // number of extra bits to send

			if (m_nLastLiteralIndex != 0)
			{
				do
				{
					nDist = ((m_bPendingBuffer[m_nDistanceIndex + lx * 2] << 8) & 0xff00) | (m_bPendingBuffer[m_nDistanceIndex + lx * 2 + 1] & 0xff);
					nLC = (m_bPendingBuffer[m_nLiteralIndex + lx]) & 0xff;
					lx++;
					if (nDist == 0)
						SendCode((int)nLC, anltree); // send a literal byte
					else
					{
						// Here, lc is the match length - MIN_MATCH
						nCode = Tree.m_abLengthCode[nLC];

						SendCode(nCode + LITERALS + 1, anltree); // send the length code
						nExtra = Tree.m_anExtraLBits[nCode];
						if (nExtra != 0)
						{
							nLC -= Tree.m_anBaseLength[nCode];
							SendBits(nLC, nExtra);       // send the extra length bits
						}
						nDist--; // dist is now the match distance - 1
						nCode = Tree.DistanceCode(nDist);

						SendCode(nCode, dtree);       // send the distance code
						nExtra = Tree.m_anExtraDistanceBits[nCode];
						if (nExtra != 0) 
						{
							nDist -= Tree.m_anBaseDist[nCode];
							SendBits(nDist, nExtra);   // send the extra distance bits
						}
					} // literal or match pair ?
					// Check that the overlay between pending_buf and d_buf+l_buf is ok:
				}
				while (lx < m_nLastLiteralIndex);
			}
			SendCode(END_BLOCK, anltree);
			m_nLastEOBLen = anltree[END_BLOCK * 2 + 1];
		}

		// Set the data type to ASCII or BINARY, using a crude approximation:
		// binary if more than 20% of the bytes are <= 6 or >= 128, ascii otherwise.
		// IN assertion: the fields freq of dyn_ltree are set and the total of all
		// frequencies does not exceed 64K (to fit in an int on 16 bit machines).

		public void SetDataType()
		{
			int n = 0;
			int nASCIIFreq = 0;
			int nBinaryFreq = 0;
			while (n < 7)
			{ 
				nBinaryFreq += m_anDynLiteralTree[n*2]; 
				n++;
			}
			while (n < 128)
			{ 
				nASCIIFreq += m_anDynLiteralTree[n*2]; 
				n++;
			}
			while (n < LITERALS)
			{ 
				nBinaryFreq += m_anDynLiteralTree[n*2]; 
				n++;
			}
			m_bDataType = (byte)(nBinaryFreq > (nASCIIFreq >> 2) ? BlockTypes.Z_BINARY : BlockTypes.Z_ASCII);
		}

		// Flush the bit buffer, keeping at most 7 bits in it.
		public void BIFlush()
		{
			if (m_nValidBitsInBufferOut == 16) 
			{
				PutShort((byte)m_bBufferOutput);
				m_bBufferOutput = 0;
				m_nValidBitsInBufferOut = 0;
			}
			else if (m_nValidBitsInBufferOut >= 8) 
			{
				PutByte((byte)m_bBufferOutput);
				m_bBufferOutput >>= 8;
				m_nValidBitsInBufferOut -= 8;
			}
		}

		// Flush the bit buffer and align the output on a byte boundary
		public void BitBufferWinDup()
		{
			if (m_nValidBitsInBufferOut > 8) 
				PutShort(m_bBufferOutput);
			else if (m_nValidBitsInBufferOut > 0) 
				PutByte((byte)m_bBufferOutput);
			m_bBufferOutput = 0;
			m_nValidBitsInBufferOut = 0;
		}

		// Copy a stored block, storing first the length and its
		// one's complement if requested.
		// int buf the input data
		// int len its length
		// bool header true if block header must be written
		public void CopyBlock(int buffer, int length, bool header)   
		{
			BitBufferWinDup();      // align on byte boundary
			m_nLastEOBLen = 8; // enough lookahead for inflate

			if (header) 
			{
				PutShort((ushort)length);   
				PutShort((ushort)~length);
			}
			PutByte(m_abWindow, buffer, length);
		}

		public void FlushBlockOnly(bool eof)
		{
			TreeFlushBlock(m_nBlockStart >= 0 ? m_nBlockStart : -1, m_nStringStartInsert - m_nBlockStart, eof);
			m_nBlockStart = m_nStringStartInsert;
			m_strm.FlushPending();
		}

		// Copy without compression as much as possible from the input stream, return
		// the current block state.
		// This function does not insert new strings in the dictionary since
		// uncompressible data is probably not useful. This function is used
		// only for the level=0 compression option.
		// NOTE: this function should be optimized to avoid extra copying from
		// window to pending_buf.
		public int DeflateStored(FlushTypes ftflush)
		{
			// Stored blocks are limited to 0xffff bytes, pending_buf is limited
			// to pending_buf_size, and each stored block has a 5 byte header:

			int nMaxBlockSize = 0xffff;
			int nMaxStart;

			if (nMaxBlockSize > m_nPendingBufferSize - 5) 
				nMaxBlockSize = m_nPendingBufferSize - 5;

			// Copy as much as possible from input to output:
			while (true)
			{
				// Fill the window as much as possible:
				if (m_nLookAheadBytes <= 1)
				{
					FillWindow();
					if (m_nLookAheadBytes == 0 && ftflush == FlushTypes.Z_NO_FLUSH) 
						return m_nNeedMore;
					if (m_nLookAheadBytes == 0) 
						break; // flush the current block
				}

				m_nStringStartInsert += m_nLookAheadBytes;
				m_nLookAheadBytes = 0;

				// Emit a stored block if pending_buf will be full:
				nMaxStart = m_nBlockStart + nMaxBlockSize;
				if (m_nStringStartInsert == 0 || m_nStringStartInsert >= nMaxStart) 
				{
					// strstart == 0 is possible when wraparound on 16-bit machine
					m_nLookAheadBytes = (int)(m_nStringStartInsert - nMaxStart);
					m_nStringStartInsert = (int)nMaxStart;
				    
					FlushBlockOnly(false);
					if (m_strm.m_nAvailOut == 0) 
						return m_nNeedMore;
				}

				// Flush if we may have to slide, otherwise block_start may become
				// negative and the data will be gone:
				if (m_nStringStartInsert - m_nBlockStart >= m_nWindowSize - MIN_LOOKAHEAD) 
				{
					FlushBlockOnly(false);
					if (m_strm.m_nAvailOut == 0) 
						return m_nNeedMore;
				}
			}

			FlushBlockOnly(ftflush == FlushTypes.Z_FINISH);
			if (m_strm.m_nAvailOut == 0)
				return (ftflush == FlushTypes.Z_FINISH) ? m_nFinishStarted : m_nNeedMore;

			return ftflush == FlushTypes.Z_FINISH ? m_nFinishDone : m_nBlockDone;
		}

		// Send a stored block
		public void TreeStoredBlock(int inputBlock, int inputBlockLength, bool eob)
		{
			SendBits((STORED_BLOCK << 1) + (eob ? 1 : 0), 3); // send block type
			CopyBlock(inputBlock, inputBlockLength, true); // with header
		}

		// Determine the best encoding for the current block: dynamic trees, static
		// trees or store, and output the encoded block to the zip file.
		// int nBuf input block, or NULL if too old
		// int nStoredLen length of input block
		// bool bEof true if this is the last block for a file
		public void TreeFlushBlock(int buf, int storedLen, bool eof)
		{
			int nOptLenB, nStaticLenB;// opt_len and static_len in bytes
			int nMaxBLIndex = 0;      // index of last bit length code of non zero freq

			// Build the Huffman trees unless a stored block is forced
			if (m_nLevel > 0) 
			{
				// Check if the file is ascii or binary
				if (m_bDataType == (byte)BlockTypes.Z_UNKNOWN) 
					SetDataType();

				// Construct the literal and distance trees
				m_tLiteralDesc.BuildTree(this);

				m_tDistanceDesc.BuildTree(this);

				// At this point, opt_len and static_len are the total bit lengths of
				// the compressed block data, excluding the tree representations.

				// Build the bit length tree for the above two trees, and get the index
				// in bl_order of the last bit length code to send.
				nMaxBLIndex = BuildBitLengthTree();

				// Determine the best encoding. Compute first the block length in bytes
				nOptLenB = (m_nOptBitLen + 3 + 7) >> 3;
				nStaticLenB =(m_StaticTreeLen + 3 + 7) >> 3;

				if (nStaticLenB <= nOptLenB) 
					nOptLenB = nStaticLenB;
			}
			else 
				nOptLenB = nStaticLenB = storedLen + 5; // force a stored block

			if (storedLen + 4 <= nOptLenB && buf != -1)
			{
				// 4: two words for the lengths
				// The test buf != NULL is only necessary if LIT_BUFSIZE > WSIZE.
				// Otherwise we can't have processed more than WSIZE input bytes since
				// the last block flush, because compression would have been
				// successful. If LIT_BUFSIZE <= WSIZE, it is never too late to
				// transform a block into a stored block.
				TreeStoredBlock(buf, storedLen, eof);
			}
			else if (nStaticLenB == nOptLenB)
			{
				SendBits((STATIC_TREES << 1) + (eof ? 1 : 0), 3);
				CompressBlock(StaticTree.m_anStaticLTree, StaticTree.m_anStaticDTree);
			}
			else
			{
				SendBits((DYN_TREES<<1) + (eof? 1 : 0), 3);
				SendAllTrees(m_tLiteralDesc.m_nMaxCode + 1, m_tDistanceDesc.m_nMaxCode + 1, nMaxBLIndex + 1);
				CompressBlock(m_anDynLiteralTree, m_anDynDistanceTree);
			}

			// The above check is made mod 2^32, for files larger than 512 MB
			// and uLong implemented on 32 bits.

			InitBlock();

			if (eof)
				BitBufferWinDup();
		}

		// Fill the window when the lookahead becomes insufficient.
		// Updates strstart and lookahead.
		//
		// IN assertion: lookahead < MIN_LOOKAHEAD
		// OUT assertions: strstart <= window_size-MIN_LOOKAHEAD
		//    At least one byte has been read, or avail_in == 0; reads are
		//    performed for at least two bytes (required for the zip translate_eol
		//    option -- not supported here).
		public void FillWindow()
		{
			int n, m;
			int p;
			int nMore;    // Amount of free space at the end of the window.

			do
			{
				nMore = (m_nActualWindowSize-m_nLookAheadBytes-m_nStringStartInsert);

				// Deal with !@#$% 64K limit:
				if (nMore==0 && m_nStringStartInsert==0 && m_nLookAheadBytes==0)
				{
					nMore = m_nWindowSize;
				} 
				else if (nMore == -1) 
				{
					// Very unlikely, but possible on 16 bit machine if strstart == 0
					// and lookahead == 1 (input done one byte at time)
					nMore--;

					// If the window is almost full and there is insufficient lookahead,
					// move the upper half to the lower one to make room in the upper half.
				}
				else if (m_nStringStartInsert >= m_nWindowSize + m_nWindowSize - MIN_LOOKAHEAD) 
				{
					Array.Copy(m_abWindow, m_nWindowSize, m_abWindow, 0, m_nWindowSize);
					m_nMatchStringStart -= m_nWindowSize;
					m_nStringStartInsert -= m_nWindowSize; // we now have strstart >= MAX_DIST
					m_nBlockStart -= m_nWindowSize;

					// Slide the hash table (could be avoided with 32 bit values
					// at the expense of memory usage). We slide even when level == 0
					// to keep the hash table consistent if we switch back to level > 0
					// later. (Using level 0 permanently is not an optimal usage of
					// zlib, so we don't care about this pathological case.)

					n = m_nHashSize;
					p=n;
					do 
					{
						m = (m_anHeadOfHash[--p] & 0xffff);
						m_anHeadOfHash[p] = (short)(m >= m_nWindowSize ? (m - m_nWindowSize) : 0);
					}
					while (--n != 0);

					n = m_nWindowSize;
					p = n;
					do 
					{
						m = (m_anPrev[--p]&0xffff);
						m_anPrev[p] = (short)(m >= m_nWindowSize ? (m - m_nWindowSize) : 0);
						// If n is not on any hash chain, prev[n] is garbage but
						// its value will never be used.
					}
					while (--n != 0);
					nMore += m_nWindowSize;
				}

				if (m_strm.m_nAvailIn == 0) 
					return;

				// If there was no sliding:
				//    strstart <= WSIZE+MAX_DIST-1 && lookahead <= MIN_LOOKAHEAD - 1 &&
				//    more == window_size - lookahead - strstart
				// => more >= window_size - (MIN_LOOKAHEAD-1 + WSIZE + MAX_DIST-1)
				// => more >= window_size - 2*WSIZE + 2
				// In the BIG_MEM or MMAP case (not yet supported),
				//   window_size == input_size + MIN_LOOKAHEAD  &&
				//   strstart + s->lookahead <= input_size => more >= MIN_LOOKAHEAD.
				// Otherwise, window_size == 2*WSIZE so more >= 2.
				// If there was sliding, more >= WSIZE. So in all cases, more >= 2.

				n = m_strm.ReadBuffer(m_abWindow, m_nStringStartInsert + m_nLookAheadBytes, nMore);
				m_nLookAheadBytes += n;

				// Initialize the hash value now that we have some input:
				if(m_nLookAheadBytes >= MIN_MATCH) 
				{
					m_nHashIndexStringToInsert = m_abWindow[m_nStringStartInsert] & 0xff;
					m_nHashIndexStringToInsert=(((m_nHashIndexStringToInsert) << m_nHashShift) ^ (m_abWindow[m_nStringStartInsert + 1] & 0xff)) & m_nHashMask;
				}
				// If the whole input has less than MIN_MATCH bytes, ins_h is garbage,
				// but this is not important since only literal bytes will be emitted.
			}
			while (m_nLookAheadBytes < MIN_LOOKAHEAD && m_strm.m_nAvailIn != 0);
		}

		// Compress as much as possible from the input stream, return the current
		// block state.
		// This function does not perform lazy evaluation of matches and inserts
		// new strings in the dictionary only for unmatched strings or for short
		// matches. It is used only for the fast compression options.
		public int DeflateFast(FlushTypes ftflush)
		{
			int hash_head = 0; // head of the hash chain
			bool bflush;      // set if current block must be flushed

			while (true)
			{
				// Make sure that we always have enough lookahead, except
				// at the end of the input file. We need MAX_MATCH bytes
				// for the next match, plus MIN_MATCH bytes to insert the
				// string following the next match.
				if (m_nLookAheadBytes < MIN_LOOKAHEAD)
				{
					FillWindow();
					if(m_nLookAheadBytes < MIN_LOOKAHEAD && ftflush == FlushTypes.Z_NO_FLUSH)
						return m_nNeedMore;
					if (m_nLookAheadBytes == 0) 
						break; // flush the current block
				}

				// Insert the string window[strstart .. strstart+2] in the
				// dictionary, and set hash_head to the head of the hash chain:
				if (m_nLookAheadBytes >= MIN_MATCH)
				{
					m_nHashIndexStringToInsert = (((m_nHashIndexStringToInsert)<<m_nHashShift)^(m_abWindow[(m_nStringStartInsert)+(MIN_MATCH-1)]&0xff))&m_nHashMask;
					hash_head = (m_anHeadOfHash[m_nHashIndexStringToInsert]&0xffff);
					m_anPrev[m_nStringStartInsert & m_nWindowMask] = m_anHeadOfHash[m_nHashIndexStringToInsert];
					m_anHeadOfHash[m_nHashIndexStringToInsert] = (short)m_nStringStartInsert;
				}

				// Find the longest match, discarding those <= prev_length.
				// At this point we have always match_length < MIN_MATCH

				if (hash_head != 0L && m_nStringStartInsert-hash_head <= m_nWindowSize-MIN_LOOKAHEAD) 
				{
					// To simplify the code, we prevent matches with the string
					// of window index 0 (in particular we have to avoid a match
					// of the string with itself at the start of the input file).
					if (m_nStrategy != (int)CompressionStrategy.Z_HUFFMAN_ONLY)
					{
						m_nMatchLength = LongestMatch (hash_head);
					}
					// longest_match() sets match_start
				}
				if (m_nMatchLength >= MIN_MATCH)
				{
					bflush=TreeTally(m_nStringStartInsert-m_nMatchStringStart, m_nMatchLength-MIN_MATCH);
					m_nLookAheadBytes -= m_nMatchLength;
					// Insert new strings in the hash table only if the match length
					// is not too large. This saves time but degrades compression.
					if (m_nMatchLength <= m_nMaxLazyMatch && m_nLookAheadBytes >= MIN_MATCH) 
					{
						m_nMatchLength--; // string at strstart already in hash table
						do
						{
							m_nStringStartInsert++;
							m_nHashIndexStringToInsert = ((m_nHashIndexStringToInsert << m_nHashShift) ^ (m_abWindow[(m_nStringStartInsert) + (MIN_MATCH - 1)] & 0xff))& m_nHashMask;
							hash_head=(m_anHeadOfHash[m_nHashIndexStringToInsert] & 0xffff);
							m_anPrev[m_nStringStartInsert & m_nWindowMask] = m_anHeadOfHash[m_nHashIndexStringToInsert];
							m_anHeadOfHash[m_nHashIndexStringToInsert]=(short)m_nStringStartInsert;

							// strstart never exceeds WSIZE-MAX_MATCH, so there are
							// always MIN_MATCH bytes ahead.
						}
						while (--m_nMatchLength != 0);
						m_nStringStartInsert++; 
					}
					else
					{
						m_nStringStartInsert += m_nMatchLength;
						m_nMatchLength = 0;
						m_nHashIndexStringToInsert = m_abWindow[m_nStringStartInsert] & 0xff;
						m_nHashIndexStringToInsert = (((m_nHashIndexStringToInsert) << m_nHashShift) ^ (m_abWindow[m_nStringStartInsert + 1] & 0xff)) & m_nHashMask;
						// If lookahead < MIN_MATCH, ins_h is garbage, but it does not
						// matter since it will be recomputed at next deflate call.
					}
				}
				else 
				{
					// No match, output a literal byte
					bflush=TreeTally(0, m_abWindow[m_nStringStartInsert]&0xff);
					m_nLookAheadBytes--;
					m_nStringStartInsert++; 
				}
				if (bflush)
				{
					FlushBlockOnly(false);
					if (m_strm.m_nAvailOut==0) 
						return m_nNeedMore;
				}
			}
			FlushBlockOnly(ftflush == FlushTypes.Z_FINISH);
			if (m_strm.m_nAvailOut==0)
			{
				if (ftflush == FlushTypes.Z_FINISH) 
					return m_nFinishStarted;
				else 
					return m_nNeedMore;
			}
			return ftflush==FlushTypes.Z_FINISH ? m_nFinishDone : m_nBlockDone;
		}

		// Same as above, but achieves better compression. We use a lazy
		// evaluation for matches: a match is ly adopted only if there is
		// no better match at the next window position.
		public int DeflateSlow(FlushTypes ftflush)
		{
			int nHashHead = 0;    // head of hash chain
			bool bFlush;         // set if current block must be flushed

			// Process the input block.
			while (true)
			{
				// Make sure that we always have enough lookahead, except
				// at the end of the input file. We need MAX_MATCH bytes
				// for the next match, plus MIN_MATCH bytes to insert the
				// string following the next match.

				if (m_nLookAheadBytes < MIN_LOOKAHEAD) 
				{
					FillWindow();
					if (m_nLookAheadBytes < MIN_LOOKAHEAD && ftflush == FlushTypes.Z_NO_FLUSH) 
						return m_nNeedMore;

					if (m_nLookAheadBytes == 0) 
						break; // flush the current block
				}

				// Insert the string window[strstart .. strstart+2] in the
				// dictionary, and set hash_head to the head of the hash chain:

				if (m_nLookAheadBytes >= MIN_MATCH) 
				{
					m_nHashIndexStringToInsert = (((m_nHashIndexStringToInsert) << m_nHashShift) ^ (m_abWindow[(m_nStringStartInsert) + (MIN_MATCH-1)] & 0xff)) & m_nHashMask;
					nHashHead = (m_anHeadOfHash[m_nHashIndexStringToInsert] & 0xffff);
					m_anPrev[m_nStringStartInsert & m_nWindowMask] = m_anHeadOfHash[m_nHashIndexStringToInsert];
					m_anHeadOfHash[m_nHashIndexStringToInsert] = (short)m_nStringStartInsert;
				}
				// Find the longest match, discarding those <= prev_length.
				m_nPrevLength = m_nMatchLength; 
				m_nPrevMatch = m_nMatchStringStart;
				m_nMatchLength = MIN_MATCH - 1;

				if (nHashHead != 0 && 
					m_nPrevLength < m_nMaxLazyMatch && 
					m_nStringStartInsert - nHashHead <= m_nWindowSize - MIN_LOOKAHEAD) 
				{
					// To simplify the code, we prevent matches with the string
					// of window index 0 (in particular we have to avoid a match
					// of the string with itself at the start of the input file).

					if (m_nStrategy != (int)CompressionStrategy.Z_HUFFMAN_ONLY) 
						m_nMatchLength = LongestMatch(nHashHead);

					if (m_nMatchLength <= 5 && 
						(m_nStrategy == (int)CompressionStrategy.Z_FILTERED || (m_nMatchLength == MIN_MATCH && m_nStringStartInsert - m_nMatchStringStart > 4096))) 
					{
						// If prev_match is also MIN_MATCH, match_start is garbage
						// but we will ignore the current match anyway.
						m_nMatchLength = MIN_MATCH-1;
					}
				}

				// If there was a match at the previous step and the current
				// match is not better, output the previous match:
				if (m_nPrevLength >= MIN_MATCH && m_nMatchLength <= m_nPrevLength) 
				{
					int nMaxInsert = m_nStringStartInsert + m_nLookAheadBytes - MIN_MATCH;
					// Do not insert strings in hash table beyond this.
					//	check_match(strstart-1, prev_match, prev_length);

					bFlush = TreeTally(m_nStringStartInsert - 1 - m_nPrevMatch, m_nPrevLength - MIN_MATCH);

					// Insert in hash table all strings up to the end of the match.
					// strstart-1 and strstart are already inserted. If there is not
					// enough lookahead, the last two strings are not inserted in
					// the hash table.
					m_nLookAheadBytes -= m_nPrevLength - 1;
					m_nPrevLength -= 2;
					do
					{
						if (++m_nStringStartInsert <= nMaxInsert) 
						{
							m_nHashIndexStringToInsert = (((m_nHashIndexStringToInsert) << m_nHashShift) ^ (m_abWindow[(m_nStringStartInsert) + (MIN_MATCH - 1)] & 0xff)) & m_nHashMask;
							nHashHead=(m_anHeadOfHash[m_nHashIndexStringToInsert] & 0xffff);
							m_anPrev[m_nStringStartInsert & m_nWindowMask] = m_anHeadOfHash[m_nHashIndexStringToInsert];
							m_anHeadOfHash[m_nHashIndexStringToInsert] = (short)m_nStringStartInsert;
						}
					}
					while(--m_nPrevLength != 0);
					
					m_nMatchAvailable = 0;
					m_nMatchLength = MIN_MATCH-1;
					m_nStringStartInsert++;
					
					if (bFlush)
					{
						FlushBlockOnly(false);
						if (0 == m_strm.m_nAvailOut)
							return m_nNeedMore;
					}
				} 
				else if (0 != m_nMatchAvailable) 
				{
					// If there was no match at the previous position, output a
					// single literal. If there was a match but the current match
					// is longer, truncate the previous match to a single literal.

					bFlush = TreeTally(0, m_abWindow[m_nStringStartInsert-1] & 0xff);

					if (bFlush) 
						FlushBlockOnly(false);
					m_nStringStartInsert++;
					m_nLookAheadBytes--;
					if ((int)(m_strm.m_nAvailOut) == 0) 
						return m_nNeedMore;
				} 
				else 
				{
					// There is no previous match to compare with, wait for
					// the next step to decide.
					m_nMatchAvailable = 1;
					m_nStringStartInsert++;
					m_nLookAheadBytes--;
				}
			}
			if (m_nMatchAvailable != 0) 
			{
				bFlush = TreeTally(0, m_abWindow[m_nStringStartInsert-1]&0xff);
				m_nMatchAvailable = 0;
			}
			FlushBlockOnly(ftflush == FlushTypes.Z_FINISH);

			if (0 == m_strm.m_nAvailOut)
			{
				if (ftflush == FlushTypes.Z_FINISH) 
					return m_nFinishStarted;
				else 
					return m_nNeedMore;
			}
			return ftflush == FlushTypes.Z_FINISH ? m_nFinishDone : m_nBlockDone;
		}

		public int LongestMatch(int currentMatch)
		{
			int nChainLength = m_nMaxChainLength; // max hash chain length
			int nScan = m_nStringStartInsert;                 // current string
			int nMatch;                           // matched string
			int len;                             // length of current match
			int nBestLen = m_nPrevLength;          // best match length so far
			int limit = m_nStringStartInsert > (m_nWindowSize - MIN_LOOKAHEAD) ? m_nStringStartInsert - (m_nWindowSize - MIN_LOOKAHEAD) : 0;
			int nice_match = m_nNiceMatch;

			// Stop when cur_match becomes <= limit. To simplify the code,
			// we prevent matches with the string of window index 0.

			int nWindowMask = m_nWindowMask;

			int strend = m_nStringStartInsert + MAX_MATCH;
			byte scan_end1 = m_abWindow[nScan+nBestLen-1];
			byte scan_end = m_abWindow[nScan+nBestLen];

			// The code is optimized for HASH_BITS >= 8 and MAX_MATCH-2 multiple of 16.
			// It is easy to get rid of this optimization if necessary.

			// Do not waste too much time if we already have a good match:
			if (m_nPrevLength >= m_nGoodMatch) 
				nChainLength >>= 2;

			// Do not look for matches beyond the end of the input. This is necessary
			// to make deflate deterministic.
			if (nice_match > m_nLookAheadBytes) 
				nice_match = m_nLookAheadBytes;
			do 
			{
				nMatch = currentMatch;

				// Skip to next match if the match length cannot increase
				// or if the match length is less than 2:

				if (m_abWindow[nMatch + nBestLen]     != scan_end  ||
					m_abWindow[nMatch + nBestLen - 1] != scan_end1 ||
					m_abWindow[nMatch]				  != m_abWindow[nScan] ||
					m_abWindow[++nMatch]			  != m_abWindow[nScan + 1])      
				{
					continue;
				}

				// The check at best_len-1 can be removed because it will be made
				// again later. (This heuristic is not always a win.)
				// It is not necessary to compare scan[2] and match[2] since they
				// are always equal when the other bytes match, given that
				// the hash keys are equal and that HASH_BITS >= 8.

				nScan += 2; 
				nMatch++;

				// We check for insufficient lookahead only every 8th comparison;
				// the 256th check will be made at strstart+258.

				do 
				{
				} 
				while (m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   m_abWindow[++nScan] == m_abWindow[++nMatch] &&
					   nScan < strend);

				len = MAX_MATCH - (int)(strend - nScan);
				nScan = strend - MAX_MATCH;
				if (len > nBestLen) 
				{
					m_nMatchStringStart = currentMatch;
					nBestLen = len;
					if (len >= nice_match) 
						break;
					scan_end1  = m_abWindow[nScan+nBestLen-1];
					scan_end   = m_abWindow[nScan+nBestLen];
				}
			} 
			while ((currentMatch = (m_anPrev[currentMatch & nWindowMask]&0xffff)) > limit && --nChainLength != 0);

			if (nBestLen <= m_nLookAheadBytes) 
				return nBestLen;
			return m_nLookAheadBytes;
		}

		public ReturnCodes DeflateInit(ZStream strm, int level, int bits)
		{
			return DeflateInit2(strm, level, Z_DEFLATED, bits, DEF_MEM_LEVEL, (int)CompressionStrategy.Z_DEFAULT_STRATEGY);
		}

		public ReturnCodes DeflateInit(ZStream strm, int level)
		{
			return DeflateInit(strm, level, MAX_WBITS);
		}

		public ReturnCodes DeflateInit2(ZStream strm, int level, int method,  int windowBits, int memLevel, int strategy)
		{
			int noheader = 0;
			//    byte[] my_version=ZLIB_VERSION;

			//
			//  if (version == null || version[0] != my_version[0]
			//  || stream_size != sizeof(z_stream)) {
			//  return Z_VERSION_ERROR;
			//  }

			strm.ErrorInfoClear();

			if (level == (int)CompressionLevels.Z_DEFAULT_COMPRESSION) 
				level = 6;

			if (windowBits < 0) 
			{ 
				// undocumented feature: suppress zlib header
				noheader = 1;
				windowBits = -windowBits;
			}

			if (memLevel < 1 || memLevel > MAX_MEM_LEVEL || 
				method != Z_DEFLATED ||
				windowBits < 8 || 
				windowBits > 15 || 
				level < (int)CompressionLevels.Z_NO_COMPRESSION ||
				level > (int)CompressionLevels.Z_BEST_COMPRESSION || 
				strategy < (int)CompressionStrategy.Z_DEFAULT_STRATEGY || 
				strategy > (int)CompressionStrategy.Z_HUFFMAN_ONLY) 
			{
				return ReturnCodes.ZLibStreamError;
			}

			strm.m_theDeflateState = (Deflate)this;

			m_nNoHeader = noheader;
			m_nWindowBits = windowBits;
			m_nWindowSize = 1 << m_nWindowBits;
			m_nWindowMask = m_nWindowSize - 1;

			m_nHashBits = memLevel + 7;
			m_nHashSize = 1 << m_nHashBits;
			m_nHashMask = m_nHashSize - 1;
			m_nHashShift = ((m_nHashBits + MIN_MATCH -1 ) / MIN_MATCH);

			m_abWindow = new byte[m_nWindowSize * 2];
			m_anPrev = new short[m_nWindowSize];
			m_anHeadOfHash = new short[m_nHashSize];

			m_nLiteralBufferSize = 1 << (memLevel + 6); // 16K elements by default

			// We overlay pending_buf and d_buf+l_buf. This works since the average
			// output size for (length,distance) codes is <= 24 bits.
			m_bPendingBuffer = new byte[m_nLiteralBufferSize * 4];
			m_nPendingBufferSize = m_nLiteralBufferSize * 4;

			m_nDistanceIndex = m_nLiteralBufferSize / 2;
			m_nLiteralIndex = (1 + 2) * m_nLiteralBufferSize;

			m_nLevel = level;

			m_nStrategy = strategy;
			m_bMethod = (byte)method;
			return Reset(strm);
		}

		public ReturnCodes Reset(ZStream strm)
		{
			strm.m_nTotalIn = strm.m_nTotalOut = 0;
			strm.ErrorInfoClear();
			strm.m_nDataType = BlockTypes.Z_UNKNOWN;

			m_nPending = 0;
			m_nPendingOut = 0;

			if (m_nNoHeader < 0) 
				m_nNoHeader = 0; // was set to -1 by deflate(..., Z_FINISH);

			m_nStatus = (m_nNoHeader != 0) ? States.BUSY : States.INIT;
			strm.m_nAdler = strm.m_theAdler.Calculate(0, null, 0, 0);
			m_nLastFlush = FlushTypes.Z_NO_FLUSH;
			TreeInit();
			LiteralMatchInit();
			return ReturnCodes.ZLibOk;
		}

		public ReturnCodes End()
		{
			if (m_nStatus != States.INIT && m_nStatus != States.BUSY && m_nStatus != States.FINISH)
				return ReturnCodes.ZLibStreamError;
			// Deallocate in reverse order of allocations:
			m_bPendingBuffer = null;
			m_anHeadOfHash = null;
			m_anPrev = null;
			m_abWindow = null;
			return m_nStatus == States.BUSY ? ReturnCodes.ZLibDataError : ReturnCodes.ZLibOk;
		}

		public ReturnCodes Params(ZStream strm, int level, int strategy)
		{
			ReturnCodes rcReturn = ReturnCodes.ZLibOk;

			if (level == (int)CompressionLevels.Z_DEFAULT_COMPRESSION)
				level = 6;

			if (level < (int)CompressionLevels.Z_NO_COMPRESSION || level > (int)CompressionLevels.Z_BEST_COMPRESSION || 
				strategy < (int)CompressionStrategy.Z_DEFAULT_STRATEGY || strategy > (int)CompressionStrategy.Z_HUFFMAN_ONLY) 
			{
				return ReturnCodes.ZLibStreamError;
			}

			if (m_aConfigTable[level].m_Compress != m_aConfigTable[level].m_Compress && strm.m_nTotalIn != 0) 
			{
				// Flush the last buffer:
				rcReturn = strm.Deflate(FlushTypes.Z_PARTIAL_FLUSH);
			}

			if (m_nLevel != level) 
			{
				m_nLevel = level;
				m_nMaxLazyMatch   = m_aConfigTable[m_nLevel].m_nMaxLazy;
				m_nGoodMatch       = m_aConfigTable[m_nLevel].m_nGoodLength;
				m_nNiceMatch       = m_aConfigTable[m_nLevel].m_nNiceLength;
				m_nMaxChainLength = m_aConfigTable[m_nLevel].m_nMaxChain;
			}
			m_nStrategy = strategy;
			return rcReturn;
		}

		public ReturnCodes SetDictionary (ZStream strm, byte[] abdictionary, int dictLength)
		{
			int nLength = dictLength;
			int nIndex = 0;

			if (abdictionary == null || m_nStatus != States.INIT)
				return ReturnCodes.ZLibStreamError;

			strm.m_nAdler = strm.m_theAdler.Calculate(strm.m_nAdler, abdictionary, 0, dictLength);

			if (nLength < MIN_MATCH) 
				return ReturnCodes.ZLibOk;

			if (nLength > m_nWindowSize - MIN_LOOKAHEAD)
			{
				nLength = m_nWindowSize - MIN_LOOKAHEAD;
				nIndex = dictLength - nLength; // use the tail of the dictionary
			}
			Array.Copy(abdictionary, nIndex, m_abWindow, 0, nLength);
			m_nStringStartInsert = nLength;
			m_nBlockStart = nLength;

			// Insert all strings in the hash table (except for the last two bytes).
			// s->lookahead stays null, so s->ins_h will be recomputed at the next
			// call of fill_window.

			m_nHashIndexStringToInsert = m_abWindow[0] & 0xff;
			m_nHashIndexStringToInsert = (((m_nHashIndexStringToInsert) << m_nHashShift) ^ (m_abWindow[1] & 0xff))& m_nHashMask;

			for (int n = 0; n <= nLength - MIN_MATCH; n++)
			{
				m_nHashIndexStringToInsert = (((m_nHashIndexStringToInsert) << m_nHashShift) ^ (m_abWindow[(n) + (MIN_MATCH-1)] & 0xff)) & m_nHashMask;
				m_anPrev[n & m_nWindowMask] = m_anHeadOfHash[m_nHashIndexStringToInsert];
				m_anHeadOfHash[m_nHashIndexStringToInsert] = (short)n;
			}
			return ReturnCodes.ZLibOk;
		}

		public ReturnCodes Process(ZStream strm, FlushTypes flush)
		{
			FlushTypes nFlushOld;
			if (flush > FlushTypes.Z_FINISH || flush < 0)
				return ReturnCodes.ZLibStreamError;

			if (strm.m_abNextOut == null ||
				(strm.m_abNextIn == null && strm.m_nAvailIn != 0) ||
				(m_nStatus == States.FINISH && flush != FlushTypes.Z_FINISH)) 
			{
				strm.ErrorInfo(ReturnCodes.ZLibStreamError, "Deflate", 1557);
				return ReturnCodes.ZLibStreamError;
			}

			if (strm.m_nAvailOut == 0)
			{
				strm.ErrorInfo(ReturnCodes.ZLibStreamError, "Deflate", 1563);
				return ReturnCodes.ZLibBufferError;
			}

			m_strm = strm; // just in case
			nFlushOld = m_nLastFlush;
			m_nLastFlush = flush;

			// Write the zlib header
			if (m_nStatus == States.INIT) 
			{
				int nHeader = (Z_DEFLATED + ((m_nWindowBits - 8) << 4)) << 8;
				int nLevelFlags = ((m_nLevel - 1) & 0xff) >> 1;

				if (nLevelFlags > 3) 
					nLevelFlags = 3;

				nHeader |= (nLevelFlags << 6);
				
				if (m_nStringStartInsert != 0) 
					nHeader |= PRESET_DICT;
				
				nHeader += 31 - (nHeader % 31);

				m_nStatus = States.BUSY;
				
				PutShortMSB((uint)nHeader);

				// Save the adler32 of the preset dictionary:
				if (0 != m_nStringStartInsert)
				{
					PutShortMSB((uint)(strm.m_nAdler >> 16));
					PutShortMSB((uint)(strm.m_nAdler & 0xffff));
				}
				strm.m_nAdler = strm.m_theAdler.Calculate(0, null, 0, 0);
			}

			// Flush as much pending output as possible
			if (0 != m_nPending) 
			{
				strm.FlushPending();
				if ((int)(strm.m_nAvailOut) == 0)
				{
					m_nLastFlush = FlushTypes.Z_LAST_FLUSH;
					return ReturnCodes.ZLibOk;
				}
			}
			else if (0 == strm.m_nAvailIn && flush <= nFlushOld && flush != FlushTypes.Z_FINISH) 
			{
				strm.ErrorInfo(ReturnCodes.ZLibBufferError, "Deflate", 1612);
				return ReturnCodes.ZLibBufferError;
			}

			// User must not provide more input after the first FINISH:
			if (m_nStatus == States.FINISH && strm.m_nAvailIn != 0) 
			{
				strm.ErrorInfo(ReturnCodes.ZLibBufferError, "Deflate", 1620);
				return ReturnCodes.ZLibBufferError;
			}

			// Start a new block or continue the current one.
			if (strm.m_nAvailIn != 0 || m_nLookAheadBytes != 0 || (flush != FlushTypes.Z_NO_FLUSH && m_nStatus != States.FINISH)) 
			{
				if (flush == FlushTypes.Z_FINISH) 
					m_nStatus = States.FINISH;
				int bstate = m_aConfigTable[m_nLevel].m_Compress(flush);
				if (bstate == m_nFinishStarted || bstate == m_nFinishDone) 
					m_nStatus = States.FINISH;

				if (bstate == m_nNeedMore || bstate == m_nFinishStarted) 
				{
					if (strm.m_nAvailOut == 0) 
						m_nLastFlush = FlushTypes.Z_LAST_FLUSH; // avoid BUF_ERROR next call, see above

					return ReturnCodes.ZLibOk;
					// If flush != Z_NO_FLUSH && m_nAvailOut == 0, the next call
					// of deflate should use the same flush parameter to make sure
					// that the flush is complete. So we don't have to output an
					// empty block here, this will be done at next call. This also
					// ensures that for a very small output buffer, we emit at most
					// one empty block.
				}

				if (bstate == m_nBlockDone) 
				{
					if (flush == FlushTypes.Z_PARTIAL_FLUSH) 
						TreeAlign();
					else 
					{ 
						// FULL_FLUSH or SYNC_FLUSH
						TreeStoredBlock(0, 0, false);
						// For a full flush, this empty block will be recognized
						// as a special marker by inflate_sync().
						if (flush == FlushTypes.Z_FULL_FLUSH) 
						{
							for (int i = 0; i < m_nHashSize; i++)  // forget history
								m_anHeadOfHash[i] = 0;
						}
					}
					strm.FlushPending();
					if (strm.m_nAvailOut == 0) 
					{
						m_nLastFlush = FlushTypes.Z_LAST_FLUSH; // avoid BUF_ERROR at next call, see above
						return ReturnCodes.ZLibOk;
					}
				}
			}

			if (flush != FlushTypes.Z_FINISH) 
				return ReturnCodes.ZLibOk;

			if (m_nNoHeader != 0) 
				return ReturnCodes.ZLibStreamEnd;

			// Write the zlib trailer (adler32)
			PutShortMSB((uint)(strm.m_nAdler >> 16));
			PutShortMSB((uint)(strm.m_nAdler & 0xffff));
			strm.FlushPending();

			// If  is zero, the application will call deflate again
			// to flush the rest.
			m_nNoHeader = -1; // write the trailer only once!
			return m_nPending != 0 ? ReturnCodes.ZLibOk : ReturnCodes.ZLibStreamEnd;
		}
	}
}