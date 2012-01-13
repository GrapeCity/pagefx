using System;

namespace DataDynamics.ZLib
{
	public enum ReturnCodes
	{
		ZLibOk = 0,
		ZLibStreamEnd = 1,
		ZLibNeedDict = 2,
		ZLibNumberError = -1,
		ZLibStreamError = -2,
		ZLibDataError = -3,
		ZLibMemoryError = -4,
		ZLibBufferError = -5,
		ZLibVersionError = -6
	}

	internal class ErrorStrings
	{
		static private String[] m_astrErrorMsgs = 
		{
			"Need Dictionary",     // Z_NEED_DICT       2
			"Stream End",          // Z_STREAM_END      1
			"Ok",                  // Z_OK              0
			"File Error",          // Z_ERRNO         (-1)
			"Stream Error",        // Z_STREAM_ERROR  (-2)
			"Data Error",          // Z_DATA_ERROR    (-3)
			"Insufficient Memory", // Z_MEM_ERROR     (-4)
			"Buffer Error",        // Z_BUF_ERROR     (-5)
			"Incompatible Version",// Z_VERSION_ERROR (-6)
			""
		};

		public static string ErrorMsg(ReturnCodes acode)
		{
			return m_astrErrorMsgs[ReturnCodes.ZLibNeedDict - acode];
		}
	}

	internal enum CompressionLevels
	{
		Z_NO_COMPRESSION = 0,
		Z_BEST_SPEED = 1,
		Z_BEST_COMPRESSION = 9,
		Z_DEFAULT_COMPRESSION = (-1)
	}

	internal enum CompressionStrategy
	{
		Z_FILTERED = 1,
		Z_HUFFMAN_ONLY = 2,
		Z_DEFAULT_STRATEGY = 0
	}

	internal enum FlushTypes
	{
		Z_LAST_FLUSH = -1,
		Z_NO_FLUSH = 0,
		Z_PARTIAL_FLUSH = 1,
		Z_SYNC_FLUSH = 2,
		Z_FULL_FLUSH = 3,
		Z_FINISH = 4
	}
}
