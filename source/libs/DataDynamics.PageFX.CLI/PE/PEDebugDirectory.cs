using System.IO;
using System.Runtime.InteropServices;

namespace DataDynamics.PE
{
	/// <summary>
	/// Debug information headers in PE FILE.
	/// </summary>
	public class PEDebugDirectory : PEDirectory
	{
		/// <summary>
		/// Debug Directory entry
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Entry
		{
		    readonly uint  _characteristics;
		    readonly uint  _timeStamp;
		    readonly ushort _versionMajor;
		    readonly ushort _versionMinor;
		    readonly PEDebugType  _type;
		    readonly uint  _size;
		    readonly uint  _rawDataAddr;
		    readonly uint  _rawDataPtr;
		    readonly BufferedBinaryReader _rdr;

			public const int EntrySize = 28;
			/// <summary>
			/// ctor
			/// </summary>
			/// <param name="rdr">Binary reader</param>
			public Entry(BufferedBinaryReader rdr)
			{
				_characteristics = rdr.ReadUInt32();
				_timeStamp = rdr.ReadUInt32();
				_versionMajor = rdr.ReadUInt16();
				_versionMinor = rdr.ReadUInt16();
				_type = (PEDebugType)rdr.ReadUInt32();
				_size = rdr.ReadUInt32();
				_rawDataAddr = rdr.ReadUInt32();
				_rawDataPtr = rdr.ReadUInt32();
				_rdr = rdr;
			}

			/// <summary>
			/// Entry params
			/// </summary>
			public uint Characteristics
			{
				get { return _characteristics; }
			}

			/// <summary>
			/// Seconds from 1970
			/// </summary>
			public uint TimeDateStamp
			{
				get { return _timeStamp; }
			}

			/// <summary>
			/// Major version
			/// </summary>
			public ushort MajorVersion
			{
				get { return _versionMajor; }
			}

			/// <summary>
			/// Minor version
			/// </summary>
			public ushort MinorVersion
			{
				get { return _versionMinor; }
			}

			/// <summary>
			/// Debug info type
			/// </summary>
			public PEDebugType Type
			{
				get { return _type; }
			}

			/// <summary>
			/// Data size
			/// </summary>
			public uint SizeOfData
			{
				get { return _size; }
			}

			/// <summary>
			/// Address in PE file
			/// </summary>
			public uint AddressOfRawData
			{
				get { return _rawDataAddr; }
			}

			/// <summary>
			/// Pointer to the data in PE File
			/// </summary>
			public uint PointerToRawData
			{
				get { return _rawDataPtr; }
			}

			/// <summary>
			/// Returns actual info stored in the referencied PE block
			/// </summary>
			/// <returns></returns>
			public PEDebugInfo GetEntry()
			{
				long oldPos = _rdr.Position;
				_rdr.Position = _rawDataPtr;
				uint signature = _rdr.ReadUInt32();
				PEDebugInfo dbg = null;
				_rdr.Position = _rawDataPtr;
				if (_type == PEDebugType.Codeview)
				{
					if (signature == 0x53445352)
						dbg = new PEDebugInfo7(_rdr);
					else
						if (signature == 0x3031424E)
							dbg = new PEDebugInfo2(_rdr);
				}
				else
					if (_type == PEDebugType.Misc)
					{
						
					}
				_rdr.Position = oldPos;
				return dbg;
			}
		}

		private Entry[] _entries;
		/// <summary>
		/// Read directory
		/// </summary>
		/// <param name="reader">reader</param>
		/// <param name="size">Size of directory</param>
		public override void Read(BufferedBinaryReader reader, int size)
		{
			int count = size/Entry.EntrySize;
			var entries = new Entry[count];
			for (int i = 0; i < count; i++)
				entries[i] = new Entry(reader);
			_entries = entries;
		}

		/// <summary>
		/// Debug info entries located in directory
		/// </summary>
		public Entry[] Entries
		{
			get {return _entries;}
		}
	}
}
