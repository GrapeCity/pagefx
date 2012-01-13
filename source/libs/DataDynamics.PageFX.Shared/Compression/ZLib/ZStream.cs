using System;
using System.IO;
using System.Diagnostics;

namespace DataDynamics.ZLib
{
	public class ProgressEventArgs : EventArgs
	{
		public ProgressEventArgs()
		{
		}

		public ProgressEventArgs(long percent)
		{
			m_nPercent = percent;
		}

		public long Percent
		{
			get
			{
				return m_nPercent;
			}
		}

		private long m_nPercent;
	}

	public delegate int ProgressEventHandler(object sender,  ProgressEventArgs eventArgs);

	public class ZStream
	{
		private const int MAX_WBITS = 15;        // 32K LZ77 window
		private const int DEF_WBITS = MAX_WBITS;
		private const int MAX_MEM_LEVEL = 9;

		internal byte[] m_abNextIn;     // next input byte
		internal int m_nNextInIndex;
		internal int m_nAvailIn;       // number of bytes available at next_in
		internal long m_nTotalIn;      // total nb of input bytes read so far

		internal byte[] m_abNextOut;    // next output byte should be put there
		internal int m_nNextOutIndex;
		internal int m_nAvailOut;      // remaining free space at next_out
		internal long m_nTotalOut;     // total nb of bytes output so far

		private string m_strErrorMsg;
		private string m_strFileName;
		private int m_nLineNumber;

		public ProgressEventHandler Progress = null;

		internal Deflate m_theDeflateState; 
		internal Inflate m_theInflateState; 

		// best guess about the data type: ascii or binary
		internal Deflate.BlockTypes m_nDataType; 

		internal long m_nAdler;
		internal Adler32 m_theAdler = new Adler32();

		private float m_fInterval;

		public float TimeInterval
		{
			get
			{
				return m_fInterval;
			}
		}

		public string ErrorMsg
		{
			get
			{
				return m_strErrorMsg;
			}
		}	

		public void ErrorInfo(string strErrorMsg, string strFile, int lineNumber)
		{
			m_strErrorMsg = strErrorMsg;
			m_strFileName = strFile;
			m_nLineNumber = lineNumber;
		}

		public void ErrorInfo(ReturnCodes error, string strFile, int lineNumber)
		{
			m_strErrorMsg = ErrorStrings.ErrorMsg(error);
			m_strFileName = strFile;
			m_nLineNumber = lineNumber;
		}

		public void ErrorInfoClear()
		{
			m_strErrorMsg = null;
			m_strFileName = null;
			m_nLineNumber = -1;
		}

		private long Percent(long totalBytes)
		{
			if (totalBytes == 0)
				return 100;
			else if (totalBytes > 10000000L)
				return (m_nTotalIn / (totalBytes / 100));
			else
				return (m_nTotalIn * 100 / totalBytes);
		}

		private ReturnCodes InflateInit()
		{
			m_theInflateState = new Inflate();
			return m_theInflateState.Init(this, DEF_WBITS);
		}

		private ReturnCodes Inflate(FlushTypes flush)
		{
			if (m_theInflateState == null) 
				return ReturnCodes.ZLibStreamError;
			return m_theInflateState.Process(this, flush);
		}

		private ReturnCodes InflateEnd()
		{
			if (m_theInflateState == null) 
				return ReturnCodes.ZLibStreamError;
			ReturnCodes ret = m_theInflateState.End(this);
			m_theInflateState = null;
			return ret;
		}

		private ReturnCodes InflateSync()
		{
			if (m_theInflateState == null)
				return ReturnCodes.ZLibStreamError;
			return m_theInflateState.Sync(this);
		}

		private ReturnCodes InflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (m_theInflateState == null)
				return ReturnCodes.ZLibStreamError;
			return m_theInflateState.SetDictionary(this, dictionary, dictLength);
		}

		private ReturnCodes DeflateInit(int level)
		{
			m_theDeflateState = new Deflate();
			return m_theDeflateState.DeflateInit(this, level, MAX_WBITS);
		}

		internal ReturnCodes DeflateInit(int level, int bits)
		{
			m_theDeflateState = new Deflate();
			return m_theDeflateState.DeflateInit(this, level, bits);
		}

		internal ReturnCodes Deflate(FlushTypes flush)
		{
			if (m_theDeflateState == null)
				return ReturnCodes.ZLibStreamError;
			return m_theDeflateState.Process(this, flush);
		}

		private ReturnCodes DeflateEnd()
		{
			if (m_theDeflateState == null) 
				return ReturnCodes.ZLibStreamError;
			ReturnCodes ret = m_theDeflateState.End();
			m_theDeflateState = null;
			return ret;
		}

		private ReturnCodes DeflateParams(int level, int strategy)
		{
			if (m_theDeflateState == null) 
				return ReturnCodes.ZLibStreamError;
			return m_theDeflateState.Params(this, level, strategy);
		}

		private ReturnCodes DeflateSetDictionary (byte[] dictionary, int dictLength)
		{
			if (m_theDeflateState == null)
				return ReturnCodes.ZLibStreamError;
			return m_theDeflateState.SetDictionary(this, dictionary, dictLength);
		}

		// Flush as much pending output as possible. All deflate() output goes
		// through this function so some applications may wish to modify it
		// to avoid allocating a large strm->next_out buffer and copying into it.
		// (See also read_buf()).
		internal void FlushPending()
		{
			int nLength = m_theDeflateState.m_nPending;

			if (nLength > m_nAvailOut) 
				nLength = m_nAvailOut;

			if (nLength == 0) 
				return;

			if (m_theDeflateState.m_bPendingBuffer.Length <= m_theDeflateState.m_nPendingOut ||
				m_abNextOut.Length <= m_nNextOutIndex ||
				m_theDeflateState.m_bPendingBuffer.Length < (m_theDeflateState.m_nPendingOut + nLength) ||
				m_abNextOut.Length < (m_nNextOutIndex+nLength))
			{
				Debug.WriteLine(m_theDeflateState.m_bPendingBuffer.Length + ", " + m_theDeflateState.m_nPendingOut+ ", " + m_abNextOut.Length + ", " + m_nNextOutIndex + ", " + nLength);
				Debug.WriteLine("avail_out=" + m_nAvailOut);
			}

			Array.Copy(m_theDeflateState.m_bPendingBuffer, 
					   m_theDeflateState.m_nPendingOut, 
					   m_abNextOut, 
					   m_nNextOutIndex, 
					   nLength);

			m_nNextOutIndex += nLength;
			m_theDeflateState.m_nPendingOut += nLength;
			m_nTotalOut += nLength;
			m_nAvailOut -= nLength;
			m_theDeflateState.m_nPending -= nLength;

			if (0 == m_theDeflateState.m_nPending)
				m_theDeflateState.m_nPendingOut = 0;
		}

		// Read a new buffer from the current input stream, update the adler32
		// and total number of bytes read.  All deflate() input goes through
		// this function so some applications may wish to modify it to avoid
		// allocating a large strm->next_in buffer and copying from it.
		// (See also flush_pending()).
		internal int ReadBuffer(byte[] buffer, int start, int size) 
		{
			int nLength = m_nAvailIn;
			if (nLength > size)
				nLength = size;
			
			if (0 == nLength)
				return 0;
			
			m_nAvailIn -= nLength;
			
			if (0 == m_theDeflateState.m_nNoHeader)
				m_nAdler = m_theAdler.Calculate(m_nAdler, m_abNextIn, m_nNextInIndex, nLength);

			Array.Copy(m_abNextIn, m_nNextInIndex, buffer, start, nLength);

			m_nNextInIndex += nLength;
			m_nTotalIn += nLength;
			return nLength;
		}

		private void Free()
		{
			m_abNextIn = null;
			m_abNextOut = null;
			m_strErrorMsg = null;
		}

		public ReturnCodes Compress(Stream stmInput, Stream stmOutput, int level, int bufferSize)
		{
			int nStart = Environment.TickCount;
			m_nAvailOut = bufferSize;
			m_abNextOut = new byte[m_nAvailOut];
			m_nAvailIn = 0;
			m_abNextIn = new byte[bufferSize];

			ReturnCodes nResult = ReturnCodes.ZLibOk;
			int nCount;
			long nTotalBytes = stmInput.Length;
			DeflateInit(level);
			while (true)
			{
				if (m_nAvailIn == 0) 
				{
					m_nAvailIn = stmInput.Read(m_abNextIn, 0, bufferSize);
					m_nNextInIndex = 0;
				}
				if (0 == m_nAvailIn)
					break;
				nResult = Deflate(FlushTypes.Z_NO_FLUSH);

				nCount = bufferSize - m_nAvailOut;
				if (0 != nCount)
				{
					stmOutput.Write(m_abNextOut, 0, nCount);
					m_nAvailOut = bufferSize;
					m_nNextOutIndex = 0;
				}

				if (ReturnCodes.ZLibOk != nResult)
					break;

				if (null != Progress)
					Progress(this, new ProgressEventArgs(Percent(nTotalBytes)));
			}

			while (true && (ReturnCodes.ZLibOk == nResult || ReturnCodes.ZLibStreamEnd == nResult))
			{
				nResult = Deflate(FlushTypes.Z_FINISH);

				nCount = bufferSize - m_nAvailOut;
				if (0 != nCount)
				{
					stmOutput.Write(m_abNextOut, 0, nCount);
					m_nAvailOut = bufferSize;
					m_nNextOutIndex = 0;
				}

				if (ReturnCodes.ZLibOk != nResult)
					break;
			}
			if (null != Progress)
				Progress(this, new ProgressEventArgs(Percent(nTotalBytes)));
			DeflateEnd();
			m_abNextOut = null;
			m_abNextIn = null;
			m_fInterval = (float)(Environment.TickCount - nStart) / 1000;
			return nResult;
		}

		public ReturnCodes Decompress(Stream stmInput, Stream stmOutput, int bufferSize)
		{
			int nStart = Environment.TickCount;
			ReturnCodes nResult = ReturnCodes.ZLibOk;
			m_nAvailIn = 0;
			m_abNextIn = new byte[bufferSize];

			m_nAvailOut = bufferSize;
			m_abNextOut = new byte[m_nAvailOut];

			int nCount;
			long nTotalBytes = stmInput.Length;
			InflateInit();
			while (true) 
			{
				if (m_nAvailIn == 0) 
				{
					m_nAvailIn = stmInput.Read(m_abNextIn, 0, bufferSize);
					m_nNextInIndex = 0;
				}
        
				if (0 == m_nAvailIn)
					break;

				nResult = Inflate(FlushTypes.Z_NO_FLUSH);

				nCount = bufferSize - m_nAvailOut;
				if (0 != nCount)
				{
					stmOutput.Write(m_abNextOut, 0, nCount);
					m_nAvailOut = bufferSize;
					m_nNextOutIndex = 0;
				}

				if (nResult != ReturnCodes.ZLibOk)
					break;

				if (null != Progress)
					Progress(this, new ProgressEventArgs(Percent(nTotalBytes)));
			}
			while (true && (ReturnCodes.ZLibOk == nResult || ReturnCodes.ZLibStreamEnd == nResult)) 
			{
				nResult = Inflate(FlushTypes.Z_FINISH);

				nCount = bufferSize - m_nAvailOut;
				if (0 != nCount)
				{
					stmOutput.Write(m_abNextOut, 0, nCount);
					m_nAvailOut = bufferSize;
					m_nNextOutIndex = 0;
				}
				
				if (nResult != ReturnCodes.ZLibOk)
					break;
			}
			if (null != Progress)
				Progress(this, new ProgressEventArgs(Percent(nTotalBytes)));
			InflateEnd();
			m_abNextOut = null;
			m_abNextIn = null;
			m_fInterval = (float)(Environment.TickCount - nStart) / 1000;
			return nResult;
		}
	}
}