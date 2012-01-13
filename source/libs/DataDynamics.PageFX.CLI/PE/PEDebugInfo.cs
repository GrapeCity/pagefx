using System;
using System.IO;
using System.Text;

namespace DataDynamics.PE
{
	/// <summary>
	/// Base information about debug entry
	/// </summary>
	public class PEDebugInfo
	{
		protected uint	_signature;
		protected int	_offset;
		protected uint	_age;
		protected string	_pdbFile;
		
		/// <summary>
		/// Do not create base class instance
		/// </summary>
		protected PEDebugInfo(){}

		public uint Signature
		{
			get { return _signature; }
		}

		public int Offset
		{
			get { return _offset; }
		}

		public uint Age
		{
			get { return _age; }
		}

		public string PdbFile
		{
			get { return _pdbFile; }
		}

		/// <summary>
		/// Read block at least desired size and until 0 byte readed
		/// </summary>
		/// <param name="desiredSize"></param>
		/// <returns></returns>
		protected byte[] ReadBlock(Stream stm, int desiredSize)
		{
			long position = stm.Position;
			stm.Position += desiredSize;
			int size = desiredSize;
			while (stm.ReadByte() != 0)
				size++;
			stm.Position = position;
			var block = new byte[size+1];
			stm.Read(block, 0, block.Length);
			return block;
		}
	}

	/// <summary>
	/// PDB 2.0 debugging info
	/// </summary>
	public sealed class PEDebugInfo2 : PEDebugInfo
	{
		private readonly uint	_timestamp;
		private const	int s_Size = 16;
		
		public PEDebugInfo2(Stream stm)
		{
			var rdr = new BufferedBinaryReader(ReadBlock(stm, s_Size));
			_signature = rdr.ReadUInt32();
			_offset = rdr.ReadInt32();
			_timestamp = rdr.ReadUInt32();
			_age = rdr.ReadUInt32();
			_pdbFile = new ASCIIEncoding().GetString(rdr.ReadBlock((int)(rdr.Length-rdr.Position))).Trim('\0');
		}

		public DateTime Timestamp
		{
			get
			{
				return (new DateTime(1970, 0, 0, 0, 0, 0)).AddSeconds(_timestamp);
			}
		}
	}

	/// <summary>
	/// PDB 7.0 Debugging info
	/// </summary>
	public sealed class PEDebugInfo7 : PEDebugInfo
	{
		private readonly Guid	_stamp;
		private const	int s_Size = 24;
		
		public PEDebugInfo7(Stream stm)
		{
			var rdr = new BufferedBinaryReader(base.ReadBlock(stm, s_Size));
			_signature = rdr.ReadUInt32();
			_offset = 0;
			_stamp = new Guid(rdr.ReadBlock(16));
			_age = rdr.ReadUInt32();
			_pdbFile = new ASCIIEncoding().GetString(rdr.ReadBlock((int)(rdr.Length-rdr.Position))).Trim('\0');
		}

		public Guid Stamp
		{
			get { return _stamp; }
		}
	}

	public sealed class PEMiskDebugInfo : PEDebugInfo
	{
//		DWORD       DataType;               // type of misc data, see defines
//		DWORD       Length;                 // total length of record, rounded to four
//											// byte multiple.
//		BOOLEAN     Unicode;                // TRUE if data is unicode string
//		BYTE        Reserved[ 3 ];
//		BYTE        Data[ 1 ];              // Actual data
		
		public PEMiskDebugInfo(Stream stm)
		{
			var rdr = new BufferedBinaryReader(base.ReadBlock(stm, 16));
			_signature = rdr.ReadUInt32();
			_offset = rdr.ReadInt32();
			uint isUnicode = rdr.ReadUInt32();
			// reserved
			rdr.ReadBlock(3);
			_age = 0;
			if (_signature == 1)
				_pdbFile = new ASCIIEncoding().GetString(rdr.ReadBlock((int)(rdr.Length-rdr.Position))).Trim('\0');
		}

	}
}
