namespace DataDynamics.ZLib
{
	using System;

	internal class Tree
	{
		private const int MAX_BITS = 15;
		private const int BL_CODES = 19;
		private const int D_CODES = 30;
		private const int LITERALS = 256;
		private const int LENGTH_CODES = 29;
		private const int L_CODES = (LITERALS + LENGTH_CODES + 1);
		private const int HEAP_SIZE = (2 * L_CODES + 1);

		// Bit length codes must not exceed MAX_BL_BITS bits
		const int MAX_BL_BITS = 7; 

		// end of block literal code
		const int END_BLOCK = 256; 

		// repeat previous bit length 3-6 times (2 bits of repeat count)
		const int REP_3_6 = 16; 

		// repeat a zero length 3-10 times  (3 bits of repeat count)
		const int REPZ_3_10 = 17; 

		// repeat a zero length 11-138 times  (7 bits of repeat count)
		const int REPZ_11_138=18; 

		// extra bits for each length code
		public static int[] m_anExtraLBits =
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0
		};

		// extra bits for each distance code
		public static int[] m_anExtraDistanceBits=
		{
			0,0,0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13
		};

		// extra bits for each bit length code
		public static int[] m_anExtraBLBits =
		{
			0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,7
		};

		public static byte[] m_abBLOrder=
		{
			16,17,18,0,8,7,9,6,10,5,11,4,12,3,13,2,14,1,15
		};


		// The lengths of the bit length codes are sent in order of decreasing
		// probability, to avoid transmitting the lengths for unused bit
		// length codes.

		public static int m_nBufferSize = 8 * 2;

		public static byte[] m_abDistCode = 
		{
			0,  1,  2,  3,  4,  4,  5,  5,  6,  6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  8,
			8,  8,  8,  8,  9,  9,  9,  9,  9,  9,  9,  9, 10, 10, 10, 10, 10, 10, 10, 10,
			10, 10, 10, 10, 10, 10, 10, 10, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
			11, 11, 11, 11, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
			12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 13, 13, 13, 13,
			13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
			13, 13, 13, 13, 13, 13, 13, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,  0,  0, 16, 17,
			18, 18, 19, 19, 20, 20, 20, 20, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22, 22, 22,
			23, 23, 23, 23, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29
		};

		public static byte[] m_abLengthCode =
		{
			0,  1,  2,  3,  4,  5,  6,  7,  8,  8,  9,  9, 10, 10, 11, 11, 12, 12, 12, 12,
			13, 13, 13, 13, 14, 14, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 16, 16, 16, 16,
			17, 17, 17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18, 18, 19, 19, 19, 19,
			19, 19, 19, 19, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
			21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 22, 22, 22, 22,
			22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 23, 23, 23, 23, 23, 23, 23, 23,
			23, 23, 23, 23, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 28
		};

		public static int[] m_anBaseLength = 
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 14, 16, 20, 24, 28, 32, 40, 48, 56,
			64, 80, 96, 112, 128, 160, 192, 224, 0
		};

		public static int[] m_anBaseDist = 
		{
			0,   1,      2,     3,     4,    6,     8,    12,    16,     24,
			32,  48,     64,    96,   128,  192,   256,   384,   512,    768,
			1024, 1536,  2048,  3072,  4096,  6144,  8192, 12288, 16384, 24576
		};

		// Mapping from a distance to a distance code. dist is the distance - 1 and
		// must not have side effects. _dist_code[256] and _dist_code[257] are never
		// used.
		public static int DistanceCode(int dist)
		{
			return ((dist) < 256 ? m_abDistCode[dist] : m_abDistCode[256 + ((dist) >> 7)]);
		}

		public short[] m_anDynTree;      // the dynamic tree
		public int m_nMaxCode;      // largest code with non zero frequency
		public StaticTree m_stDesc;  // the corresponding static tree

		// Compute the optimal bit lengths for a tree and update the total bit length
		// for the current block.
		// IN assertion: the fields freq and dad are set, heap[heap_max] and
		//    above are the tree nodes sorted by increasing frequency.
		// OUT assertions: the field len is set to the optimal bit length, the
		//     array bl_count contains the frequencies for each bit length.
		//     The length opt_len is updated; static_len is also updated if stree is
		//     not null.
		void GenBitLen(Deflate deflate)
		{
			short[] anTree = m_anDynTree;
			short[] anSTree = m_stDesc.m_anStaticTree;
			int[] extra = m_stDesc.m_anExtraBits;
			int nBase = m_stDesc.m_nExtraBase;
			int nMaxLength = m_stDesc.m_nMaxLength;
			int nHeapIndex; // heap index
			int n, m; // iterate over the tree elements
			int nBitLength; // bit length
			int nExtraBits; // extra bits
			short nFreq;       // frequency
			int nOverFlow = 0;   // number of elements with bit length too large

			for (nBitLength = 0; nBitLength <= MAX_BITS; nBitLength++) 
				deflate.m_anBLCount[nBitLength] = 0;

			// In a first pass, compute the optimal bit lengths (which may
			// overflow in the case of the bit length tree).
			anTree[deflate.m_anHeap[deflate.m_nHeapMax] * 2 + 1] = 0; // root of the heap

			for (nHeapIndex = deflate.m_nHeapMax + 1; nHeapIndex < HEAP_SIZE; nHeapIndex++)
			{
				n = deflate.m_anHeap[nHeapIndex];
				nBitLength = anTree[anTree[n * 2 + 1] * 2 + 1] + 1;
				if (nBitLength > nMaxLength)
				{ 
					nBitLength = nMaxLength; 
					nOverFlow++; 
				}
				anTree[n * 2 + 1] = (short)nBitLength;
				// We overwrite tree[n*2+1] which is no longer needed

				if (n > m_nMaxCode) 
					continue;  // not a leaf node

				deflate.m_anBLCount[nBitLength]++;
				nExtraBits = 0;
				if (n >= nBase) 
					nExtraBits = extra[n - nBase];
				nFreq = anTree[n * 2];
				deflate.m_nOptBitLen += nFreq * (nBitLength + nExtraBits);
				if (anSTree != null) 
					deflate.m_StaticTreeLen += nFreq * (anSTree[n * 2 + 1] + nExtraBits);
			}
			if (nOverFlow == 0) 
				return;

			// This happens for example on obj2 and pic of the Calgary corpus
			// Find the first bit length which could increase:
			do 
			{
				nBitLength = nMaxLength - 1;
				while (0 == deflate.m_anBLCount[nBitLength])
					nBitLength--;
				deflate.m_anBLCount[nBitLength]--;      // move one leaf down the tree
				deflate.m_anBLCount[nBitLength+1] += 2;   // move one overflow item as its brother
				deflate.m_anBLCount[nMaxLength]--;
				// The brother of the overflow item also moves one step up,
				// but this does not affect bl_count[max_length]
				nOverFlow -= 2;
			}
			while (nOverFlow > 0);

			for (nBitLength = nMaxLength; nBitLength != 0; nBitLength--) 
			{
				n = deflate.m_anBLCount[nBitLength];
				while (n != 0) 
				{
					m = deflate.m_anHeap[--nHeapIndex];
					if (m > m_nMaxCode) 
						continue;

					if (anTree[m * 2 + 1] != nBitLength) 
					{
						deflate.m_nOptBitLen += (nBitLength - anTree[m * 2 + 1]) * anTree[m * 2];
						anTree[m * 2 + 1] = (short)nBitLength;
					}
					n--;
				}
			}
		}

		// Construct one Huffman tree and assigns the code bit strings and lengths.
		// Update the total bit length for the current block.
		// IN assertion: the field freq is set for all tree elements.
		// OUT assertions: the fields len and code are set to the optimal bit length
		//     and corresponding code. The length opt_len is updated; static_len is
		//     also updated if stree is not null. The field max_code is set.
		internal void BuildTree(Deflate deflate)
		{
			short[] anTree = m_anDynTree;
			short[] anSTtree = m_stDesc.m_anStaticTree;
			int nElems = m_stDesc.m_nElems;
			int nNodeOfLeastFreq, nNodeOfNextLeastFreq; // iterate over heap elements
			int nMaxCode = -1; // largest code with non zero frequency
			int nNode; // new node being created

			// Construct the initial heap, with least frequent element in
			// heap[1]. The sons of heap[n] are heap[2*n] and heap[2*n+1].
			// heap[0] is not used.
			deflate.m_nHeapLen = 0;
			deflate.m_nHeapMax = HEAP_SIZE;

			for (int nIndex = 0; nIndex < nElems; nIndex++) 
			{
				if (anTree[nIndex * 2] != 0) 
				{
					deflate.m_anHeap[++deflate.m_nHeapLen] = nMaxCode = nIndex;
					deflate.m_abDepth[nIndex] = 0;
				}
				else
					anTree[nIndex * 2 + 1] = 0;
			}

			// The pkzip format requires that at least one distance code exists,
			// and that at least one bit should be sent even if there is only one
			// possible code. So to avoid special checks later on we force at least
			// two codes of non zero frequency.
			while (deflate.m_nHeapLen < 2) 
			{
				nNode = deflate.m_anHeap[++deflate.m_nHeapLen] = (nMaxCode < 2 ? ++nMaxCode : 0);
				anTree[nNode * 2] = 1;
				deflate.m_abDepth[nNode] = 0;
				deflate.m_nOptBitLen--; 
				if (anSTtree != null) 
					deflate.m_StaticTreeLen -= anSTtree[nNode * 2 + 1];
				// node is 0 or 1 so it does not have extra bits
			}
			m_nMaxCode = nMaxCode;

			// The elements heap[heap_len/2+1 .. heap_len] are leaves of the tree,
			// establish sub-heaps of increasing lengths:

			for (int nIndex = deflate.m_nHeapLen / 2; nIndex >= 1; nIndex--)
				deflate.PQDownHeap(anTree, nIndex);

			// Construct the Huffman tree by repeatedly combining the least two
			// frequent nodes.

			nNode = nElems; // next internal node of the tree
			do
			{
				// n = node of least frequency
				nNodeOfLeastFreq = deflate.m_anHeap[1];
				deflate.m_anHeap[1] = deflate.m_anHeap[deflate.m_nHeapLen--];
				deflate.PQDownHeap(anTree, 1);
				nNodeOfNextLeastFreq = deflate.m_anHeap[1]; // m = node of next least frequency

				deflate.m_anHeap[--deflate.m_nHeapMax] = nNodeOfLeastFreq; // keep the nodes sorted by frequency
				deflate.m_anHeap[--deflate.m_nHeapMax] = nNodeOfNextLeastFreq;

				// Create a new node father of n and m
				anTree[nNode * 2] = (short)(anTree[nNodeOfLeastFreq * 2] + anTree[nNodeOfNextLeastFreq * 2]);
				deflate.m_abDepth[nNode] = (byte)(Math.Max(deflate.m_abDepth[nNodeOfLeastFreq], deflate.m_abDepth[nNodeOfNextLeastFreq]) + 1);
				anTree[nNodeOfLeastFreq * 2 + 1] = anTree[nNodeOfNextLeastFreq * 2 + 1] = (short)nNode;

				// and insert the new node in the heap
				deflate.m_anHeap[1] = nNode++;
				deflate.PQDownHeap(anTree, 1);
			}
			while (deflate.m_nHeapLen >= 2);

			deflate.m_anHeap[--deflate.m_nHeapMax] = deflate.m_anHeap[1];

			// At this point, the fields freq and dad are set. We can now
			// generate the bit lengths.

			GenBitLen(deflate);

			// The field len is now set, we can generate the bit codes
			GenCodes(anTree, nMaxCode, deflate.m_anBLCount);
		}

		// Generate the codes for a given tree and bit counts (which need not be
		// optimal).
		// IN assertion: the array bl_count contains the bit length statistics for
		// the given tree and the field len is set for all tree elements.
		// OUT assertion: the field code is set for all tree elements of non
		//     zero code length.
		// short[] tree, the tree to decorate
		// int max_code, largest code with non zero frequency
		// short[] bl_count number of codes at each bit length

		static void GenCodes(short[] atree, int maxCode, short[] ablcount)
		{
			short[] anNextCode = new short[MAX_BITS+1]; // next code value for each bit length
			short nCode = 0; // running code value
			int nBits; // bit index
			int nCodeIndex; // code index

			// The distribution counts are first used to generate the code values
			// without bit reversal.
			for (nBits = 1; nBits <= MAX_BITS; nBits++) 
				anNextCode[nBits] = nCode = (short)((nCode + ablcount[nBits-1]) << 1);

			// Check that the bit counts in bl_count are consistent. The last code
			// must be all ones.

			int nLen;
			for (nCodeIndex = 0;  nCodeIndex <= maxCode; nCodeIndex++) 
			{
				nLen = atree[nCodeIndex * 2 + 1];
				if (nLen == 0) 
					continue;
				// Now reverse the bits
				atree[nCodeIndex * 2] = (short)(BIReverse(anNextCode[nLen]++, nLen));
			}
		}

		// Reverse the first len bits of a code, using straightforward code (a faster
		// method would use a table)
		// IN assertion: 1 <= len <= 15
		// code the value to invert
		// len its bit length
		static int BIReverse(int code, int len)
		{
			int nResult = 0;
			do
			{
				nResult |= code & 1;
				code >>= 1;
				nResult <<= 1;
			} 
			while (--len > 0);
			return nResult >> 1;
		}
	}
}