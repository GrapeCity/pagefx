namespace DataDynamics.ZLib
{
	using System;

	internal class Inflate
	{
		private const int MAX_WBITS=15; // 32K LZ77 window

		// preset dictionary flag in zlib header
		private const int PRESET_DICT = 0x20;

		private const int Z_DEFLATED=8;

		private const int METHOD = 0;   // waiting for method byte
		private const int FLAG = 1;     // waiting for flag byte
		private const int DICT4 = 2;    // four dictionary check bytes to go
		private const int DICT3 = 3;    // three dictionary check bytes to go
		private const int DICT2 = 4;    // two dictionary check bytes to go
		private const int DICT1 = 5;    // one dictionary check byte to go
		private const int DICT0 = 6;    // waiting for inflateSetDictionary
		private const int BLOCKS = 7;   // decompressing blocks
		private const int CHECK4 = 8;   // four check bytes to go
		private const int CHECK3 = 9;   // three check bytes to go
		private const int CHECK2 = 10;  // two check bytes to go
		private const int CHECK1 = 11;  // one check byte to go
		private const int DONE = 12;    // finished check, done
		private const int BAD = 13;     // got an error--stay here

		int m_nMode; // current inflate mode

		// mode dependent information
		int m_nMethod; // if FLAGS, method byte

		// if CHECK, check values to compare
		long[] m_nWas = new long[1] ; // computed check value
		long m_nNeed;               // stream check value

		// if BAD, inflateSync's marker bytes count
		int m_nMarker;

		// mode independent information
		int m_nNoWrap; // flag for no wrapper
		int m_nWindowBits; // log2(window size)  (8..15, defaults to 15)

		InfBlocks m_theInfBlocks; // current inflate_blocks state

		public ReturnCodes Reset(ZStream zstream)
		{
			if (zstream == null || zstream.m_theInflateState == null) 
				return ReturnCodes.ZLibStreamError;
			zstream.m_nTotalIn = zstream.m_nTotalOut = 0;
			zstream.ErrorInfoClear();
			zstream.m_theInflateState.m_nMode = zstream.m_theInflateState.m_nNoWrap != 0 ? BLOCKS : METHOD;
			zstream.m_theInflateState.m_theInfBlocks.Reset(zstream, null);
			return ReturnCodes.ZLibOk;
		}

		public ReturnCodes End(ZStream zstream)
		{
			if (m_theInfBlocks != null)
				m_theInfBlocks.Free(zstream);
			m_theInfBlocks = null;
			return ReturnCodes.ZLibOk;
		}

		public ReturnCodes Init(ZStream zstream, int w)
		{
			zstream.ErrorInfoClear();
			m_theInfBlocks = null;

			// handle undocumented nowrap option (no zlib header or check)
			m_nNoWrap = 0;
			if (w < 0)
			{
				w = - w;
				m_nNoWrap = 1;
			}

			// set window size
			if (w < 8 ||w > 15)
			{
				End(zstream);
				return ReturnCodes.ZLibStreamError;
			}
			m_nWindowBits = w;
			zstream.m_theInflateState.m_theInfBlocks = new InfBlocks(zstream, zstream.m_theInflateState.m_nNoWrap != 0 ? null : this, 1 << w);
			// reset state
			Reset(zstream);
			return ReturnCodes.ZLibOk;
		}

		public ReturnCodes Process(ZStream zstream, FlushTypes flush)
		{
			ReturnCodes nResult;
			int b;

			if (zstream == null || zstream.m_theInflateState == null || zstream.m_abNextIn == null)
				return ReturnCodes.ZLibStreamError;

			ReturnCodes nErrorCode = (flush == FlushTypes.Z_FINISH) ? ReturnCodes.ZLibBufferError : ReturnCodes.ZLibOk;
			nResult = ReturnCodes.ZLibBufferError;

			while (true)
			{
				switch (zstream.m_theInflateState.m_nMode)
				{
					case METHOD:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						if (((zstream.m_theInflateState.m_nMethod = zstream.m_abNextIn[zstream.m_nNextInIndex++]) & 0xf) != Z_DEFLATED)
						{
							zstream.m_theInflateState.m_nMode = BAD;
							zstream.ErrorInfo("Unknown compression method", "Inflate", 116);
							zstream.m_theInflateState.m_nMarker = 5;       // can't try inflateSync
							break;
						}
						if ((zstream.m_theInflateState.m_nMethod >> 4) + 8 > zstream.m_theInflateState.m_nWindowBits)
						{
							zstream.m_theInflateState.m_nMode = BAD;
							zstream.ErrorInfo("Invalid window size", "Inflate", 123);
							zstream.m_theInflateState.m_nMarker = 5;       // can't try inflateSync
							break;
						}
						zstream.m_theInflateState.m_nMode = FLAG;
						break;

					case FLAG:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						b = (zstream.m_abNextIn[zstream.m_nNextInIndex++]) & 0xff;
						if ((((zstream.m_theInflateState.m_nMethod << 8) + b) % 31) != 0)
						{
							zstream.m_theInflateState.m_nMode = BAD;
							zstream.ErrorInfo("Incorrect header check", "Inflate", 140);
							zstream.m_theInflateState.m_nMarker = 5; // Can't try InflateSync
							break;
						}
						if ((b & PRESET_DICT) == 0)
						{
							zstream.m_theInflateState.m_nMode = BLOCKS;
							break;
						}
						zstream.m_theInflateState.m_nMode = DICT4;
						break;

					case DICT4:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed = (zstream.m_abNextIn[zstream.m_nNextInIndex++]&0xff) << 24;
						zstream.m_theInflateState.m_nMode = DICT3;
						break;

					case DICT3:
						if (0 == zstream.m_nAvailIn)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed += (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff) << 16;
						zstream.m_theInflateState.m_nMode = DICT2;
						break;

					case DICT2:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed += (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff) << 8;
						zstream.m_theInflateState.m_nMode = DICT1;
						break;

					case DICT1:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed += (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff);
						zstream.m_nAdler = zstream.m_theInflateState.m_nNeed;
						zstream.m_theInflateState.m_nMode = DICT0;
						return ReturnCodes.ZLibNeedDict;

					case DICT0:
						zstream.m_theInflateState.m_nMode = BAD;
						zstream.ErrorInfo("Need dictionary", "Inflate", 195);
						zstream.m_theInflateState.m_nMarker = 0;       // can try inflateSync
						return ReturnCodes.ZLibStreamError;

					case BLOCKS:
						nResult = zstream.m_theInflateState.m_theInfBlocks.Process(zstream, nResult);
						if (nResult == ReturnCodes.ZLibDataError)
						{
							zstream.m_theInflateState.m_nMode = BAD;
							zstream.m_theInflateState.m_nMarker = 0;     // can try inflateSync
							break;
						}
						if (nResult == ReturnCodes.ZLibOk)
							nResult = nErrorCode;
						if (nResult != ReturnCodes.ZLibStreamEnd)
							return nResult;
						nResult = nErrorCode;
						zstream.m_theInflateState.m_theInfBlocks.Reset(zstream, zstream.m_theInflateState.m_nWas);
						if (zstream.m_theInflateState.m_nNoWrap != 0)
						{
							zstream.m_theInflateState.m_nMode = DONE;
							break;
						}
						zstream.m_theInflateState.m_nMode = CHECK4;
						break;

					case CHECK4:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed = (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff) << 24;
						zstream.m_theInflateState.m_nMode = CHECK3;
						break;

					case CHECK3:
						if (zstream.m_nAvailIn==0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed += (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff) << 16;
						zstream.m_theInflateState.m_nMode = CHECK2;
						break;

					case CHECK2:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed += (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff) << 8;
						zstream.m_theInflateState.m_nMode = CHECK1;
						break;

					case CHECK1:
						if (zstream.m_nAvailIn == 0)
							return nResult;
						nResult = nErrorCode;
						zstream.m_nAvailIn--; 
						zstream.m_nTotalIn++;
						zstream.m_theInflateState.m_nNeed += (zstream.m_abNextIn[zstream.m_nNextInIndex++] & 0xff);

						if (((int)(zstream.m_theInflateState.m_nWas[0])) != ((int)(zstream.m_theInflateState.m_nNeed)))
						{
							zstream.m_theInflateState.m_nMode = BAD;
							zstream.ErrorInfo("Incorrect data check", "Inflate", 453);
							zstream.m_theInflateState.m_nMarker = 5; // can't try inflateSync
							break;
						}
						zstream.m_theInflateState.m_nMode = DONE;
						break;

					case DONE:
						return ReturnCodes.ZLibStreamEnd;

					case BAD:
						return ReturnCodes.ZLibDataError;

					default:
						return ReturnCodes.ZLibStreamError;
				}
			}
		}

		public ReturnCodes SetDictionary(ZStream zstream, byte[] adictionary, int dictLength)
		{
			int nIndex = 0;
			int nLength = dictLength;
			if (zstream == null || zstream.m_theInflateState == null || zstream.m_theInflateState.m_nMode != DICT0)
				return ReturnCodes.ZLibStreamError;

			if (zstream.m_theAdler.Calculate(1L, adictionary, 0, dictLength) != zstream.m_nAdler)
				return ReturnCodes.ZLibDataError;

			zstream.m_nAdler = zstream.m_theAdler.Calculate(0, null, 0, 0);

			if (nLength >= (1 << zstream.m_theInflateState.m_nWindowBits))
			{
				nLength = (1 << zstream.m_theInflateState.m_nWindowBits) - 1;
				nIndex = dictLength - nLength;
			}
			zstream.m_theInflateState.m_theInfBlocks.SetDictionary(adictionary, nIndex, nLength);
			zstream.m_theInflateState.m_nMode = BLOCKS;
			return ReturnCodes.ZLibOk;
		}

		private byte[] mark = new[]{(byte)0, (byte)0, (byte)0xff, (byte)0xff};

		public ReturnCodes Sync(ZStream z)
		{
			int n;       // number of bytes to look at
			int p;       // pointer to bytes
			int m;       // number of marker bytes found in a row
			long r, w;   // temporaries to save m_nTotalIn and total_out

			// set up
			if (z == null || z.m_theInflateState == null)
				return ReturnCodes.ZLibStreamError;

			if(z.m_theInflateState.m_nMode != BAD)
			{
				z.m_theInflateState.m_nMode = BAD;
				z.m_theInflateState.m_nMarker = 0;
			}
			if ((n = z.m_nAvailIn) == 0)
				return ReturnCodes.ZLibBufferError;
			p = z.m_nNextInIndex;
			m = z.m_theInflateState.m_nMarker;

			// search
			while (n != 0 && m < 4)
			{
				if(z.m_abNextIn[p] == mark[m])
					m++;
				else if(z.m_abNextIn[p]!=0)
					m = 0;
				else
					m = 4 - m;
				p++; 
				n--;
			}

			// restore
			z.m_nTotalIn += p - z.m_nNextInIndex;
			z.m_nNextInIndex = p;
			z.m_nAvailIn = n;
			z.m_theInflateState.m_nMarker = m;

			// return no joy or set up to restart on a new block
			if (m != 4)
				return ReturnCodes.ZLibDataError;
			r = z.m_nTotalIn;  
			w = z.m_nTotalOut;
			Reset(z);
			z.m_nTotalIn = r;  
			z.m_nTotalOut = w;
			z.m_theInflateState.m_nMode = BLOCKS;
			return ReturnCodes.ZLibOk;
		}

		// Returns true if inflate is currently at the end of a block generated
		// by Z_SYNC_FLUSH or Z_FULL_FLUSH. This function is used by one PPP
		// implementation to provide an additional safety check. PPP uses Z_SYNC_FLUSH
		// but removes the length bytes of the resulting empty stored block. When
		// decompressing, PPP checks that at the end of input packet, inflate is
		// waiting for these length bytes.
		public bool SyncPoint(ZStream z)
		{
			if (z == null || z.m_theInflateState == null || z.m_theInflateState.m_theInfBlocks == null)
				return false;
			return z.m_theInflateState.m_theInfBlocks.SyncPoint();
		}
	}
}