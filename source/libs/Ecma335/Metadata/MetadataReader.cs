using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	//ref: Partition II, 24.2.1 Metadata root

	/// <summary>
    /// Metadata reader.
    /// </summary>
    internal sealed class MetadataReader : IDisposable
    {
	    private readonly BufferedBinaryReader _reader;
		private readonly Image _image = new Image();

        private StringHeap _strings;
		private UserStringHeap _userStrings;
		private GuidHeap _guids;
		private BlobHeap _blob;		
        
	    public MetadataReader(string path)
        {
			_reader = new BufferedBinaryReader(path);
	        Load();
        }

        public MetadataReader(Stream s)
        {
			_reader = new BufferedBinaryReader(s);
			Load();
        }

		public void Dispose()
        {
        }

		#region Public Members
        private int _resourceOrigin = -1;

        public BufferedBinaryReader SeekResourceOffset(int offset)
        {
            if (_resourceOrigin < 0)
            {
                int pos = _image.ResolveVirtualAddress(_image.Resources.VirtualAddress);
                _resourceOrigin = pos;
            }
            if (offset >= _image.Resources.Size)
                throw new BadMetadataException();
            _reader.Position = _resourceOrigin + offset;
            return _reader;
        }

        public int SizeOfResources
        {
            get { return (int)_image.Resources.Size; }
        }

	    public SimpleIndex EntryPointToken
	    {
			get { return _image.EntryPointToken; }
	    }

	    public BufferedBinaryReader SeekRVA(uint rva)
        {
			_reader.Position = _image.ResolveVirtualAddress(rva);
            return _reader;
        }

        public MetadataTable this[TableId tableId]
        {
            get
            {
                int i = (int)tableId;
                if (i < 0 || i >= _tables.Length)
                    throw new ArgumentOutOfRangeException("tableId");
                return _tables[i];
            }
        }

        public IEnumerable<MetadataRow> GetRows(TableId tableId)
        {
            int n = GetRowCount(tableId);
            for (int i = 0; i < n; ++i)
                yield return GetRow(tableId, i);
        }

        public int GetRowCount(TableId id)
        {
            var table = this[id];
            if (table == null) return 0;
            return table.RowCount;
        }

        public MetadataRow GetRow(TableId tableId, int rowIndex)
        {
            var table = this[tableId];
            if (table == null)
                return null;
            if (table.RowCount <= 0)
                return null;
            
            var row = table.Rows[rowIndex];
            if (row != null) return row;

            _reader.Position = table.Offset + rowIndex * table.RowSize;

            int n = table.Columns.Count;
            var cells = new MetadataCell[n];
            for (int i = 0; i < n; ++i)
            {
                var col = table.Columns[i];
                cells[i] = ReadCell(col);
            }

            row = new MetadataRow(rowIndex, cells);

            table.Rows[rowIndex] = row;

            return row;
        }

        public MetadataRow this[TableId tableId, int rowIndex]
        {
            get { return GetRow(tableId, rowIndex); }
        }

        private MetadataCell ReadCell(MetadataColumn column)
        {    
            switch(column.Type)
            {
                case ColumnType.Int32:
                    {
                        uint i = _reader.ReadUInt32();
                        return new MetadataCell(column, i);
                    }

                case ColumnType.Int16:
                    {
                        uint i = _reader.ReadUInt16();
                        return new MetadataCell(column, i);
                    }

                case ColumnType.StringIndex:
                    {
                        uint i = _reader.ReadIndex(_strIdxSize);
                        string s = FetchString(i);
                        return new MetadataCell(column, i, s);
                    }

                case ColumnType.BlobIndex:
                    {
                        uint i = _reader.ReadIndex(_blobIdxSize);
                        var data = GetBlob(i);
                        return new MetadataCell(column, i, data);
                    }

                case ColumnType.GuidIndex:
                    {
                        uint i = _reader.ReadIndex(_guidIdxSize);
                        return new MetadataCell(column, i, FetchGuid(i));
                    }

                case ColumnType.SimpleIndex:
                    {
                        uint i = ReadIndex(column.SimpleIndex);
                        return new MetadataCell(column, i);
                    }

                case ColumnType.CodedIndex:
                    {
                        uint i = ReadIndex(column.CodedIndex);
                        return new MetadataCell(column, i);
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SimpleIndex ReadIndex(TableId id)
        {
            int size = GetSimpleIndexSize(id);
            uint index = _reader.ReadIndex(size);
            return new SimpleIndex(id, (int)index);
        }

        private SimpleIndex ReadIndex(CodedIndex i)
        {
            int size = GetCodedIndexSize(i);
            uint index = _reader.ReadIndex(size);
            return i.Decode(index);
        }
        #endregion

        #region ColumnSize
        private static int GetIndexSize(int n)
        {
            return n >= 0x10000 ? 4 : 2;
        }

        private int GetSimpleIndexSize(TableId id)
        {
            int n = GetRowCount(id);
            return GetIndexSize(n);
        }

        private int GetMaxRowCount(IEnumerable<TableId> tables)
        {
        	return tables.Select(id => GetRowCount(id)).Concat(new[] {0}).Max();
        }

    	int[] _codedIndexSizes;
        private int GetCodedIndexSize(CodedIndex i)
        {
            return _codedIndexSizes[i.ID];
        }

        private int GetColumnSize(MetadataColumn c)
        {
            switch (c.Type)
            {
                case ColumnType.Int32: return 4;
                case ColumnType.Int16: return 2;
                case ColumnType.StringIndex: return _strIdxSize;
                case ColumnType.BlobIndex: return _blobIdxSize;
                case ColumnType.GuidIndex: return _guidIdxSize;
                case ColumnType.SimpleIndex: return GetSimpleIndexSize(c.SimpleIndex);
                case ColumnType.CodedIndex: return GetCodedIndexSize(c.CodedIndex);
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Loading
        private void Load()
        {
			_image.Load(_reader);

            uint mdbOffset = (uint)_reader.Position;

            _reader.Position = mdbOffset;

			// Metadata Header
			// Signature
			if (_reader.ReadUInt32() != 0x424A5342)
			{
				throw new BadMetadataException("Invalid metadata header.");
			}

			// MajorVersion			2
			// MinorVersion			2
			// Reserved				4
			_reader.Advance(8);

			var runtimeVersion = _reader.ReadZeroTerminatedString(_reader.ReadInt32());

			// align for dword boundary
			_reader.Align4();

			// Flags		2
			_reader.Advance(2);

	        LoadHeaps(mdbOffset);
        }

		private static IEnumerable<int> Range(int n)
		{
			for (int i = 0; i < n; i++)
				yield return i;
		}

        #region LoadHeaps
		private void LoadHeaps(uint startOffset)
        {
			// heap headers
			int n = _reader.ReadUInt16();
			var headers = Range(n)
				.Select(x =>
				        new
					        {
								Offset = startOffset + _reader.ReadUInt32(),
						        Size = _reader.ReadUInt32(),
						        Name = _reader.ReadAlignedString(16)
					        }).ToArray();

			foreach (var header in headers)
			{
				switch (header.Name)
				{
					case "#-":
					case "#~":
						if (_tables != null)
							throw new BadMetadataException("Multiple table heaps.");
						_reader.Position = header.Offset;
						LoadTables();
						break;

					case "#Strings":
						if (_strings != null)
							throw new BadMetadataException("Multiple #Strings heaps.");
						_strings = new StringHeap(_reader.Slice(header.Offset, header.Size));
						break;

					case "#US":
						if (_userStrings != null)
							throw new BadMetadataException("Multiple #US heaps.");
						_userStrings = new UserStringHeap(_reader.Slice(header.Offset, header.Size));
						break;

					case "#GUID":
						if (_guids != null)
							throw new BadMetadataException("Multiple #GUID heaps.");
						_guids = new GuidHeap(_reader.Slice(header.Offset, header.Size));
						break;

					case "#Blob":
						if (_blob != null)
							throw new BadMetadataException("Multiple #Blob heaps.");
						_blob = new BlobHeap(_reader.Slice(header.Offset, header.Size));
						break;

					default:
						throw new BadMetadataException("Unknown meta-data stream.");
				}
			}
        }
        #endregion

		#region LoadTables
		private int _strIdxSize;
		private int _blobIdxSize;
		private int _guidIdxSize;
		private MetadataTable[] _tables;
        internal const int MaxTableNum = 64;

        private void LoadTables()
        {
	        _reader.Advance(4); //reserved: 4, always 0

	        var header = new
		        {
			        MajorVersion = _reader.ReadUInt8(),
			        MinorVersion = _reader.ReadUInt8(),
			        HeapSizes = _reader.ReadUInt8(),
			        _ = _reader.Advance(1), //reserved: 1, always 1
			        Valid = _reader.ReadUInt64(),
			        Sorted = _reader.ReadUInt64(),
		        };

            _strIdxSize = (((header.HeapSizes & 1) == 0) ? 2 : 4);
            _guidIdxSize = (((header.HeapSizes & 2) == 0) ? 2 : 4);
            _blobIdxSize = (((header.HeapSizes & 4) == 0) ? 2 : 4);

            //Read table row nums
            ulong present = header.Valid;
            _tables = new MetadataTable[MaxTableNum];
            for (int i = 0; i < MaxTableNum; i++)
            {
                //NOTE: If flag set table is presented
                if (((present >> i) & 1) != 0)
                {
                    int rowNum = _reader.ReadInt32();
                    var id = (TableId)i;
                    var table = Schema.CreateTable(id);
                    table.RowCount = rowNum;
                    table.IsSorted = ((header.Sorted >> i) & 1) != 0;
                    _tables[i] = table;
                }
            }

            //Prepare coded index sizes
            int n = CodedIndex.All.Length;
            _codedIndexSizes = new int[n];
            for (int i = 0; i < n; ++i)
            {
                var ci = CodedIndex.All[i];
                Debug.Assert(ci.ID == i);
                int mn = GetMaxRowCount(ci.Tables);
                int mn2 = mn << ci.Bits;
                int size = GetIndexSize(mn2);
                _codedIndexSizes[ci.ID] = size;
            }

            //Setup tables (Offset, RowSize, Size)
            long pos = _reader.Position;
            for (int i = 0; i < MaxTableNum; i++)
            {
                var table = _tables[i];
                if (table != null)
                {
                    table.Offset = pos;
                    int rowSize = table.Columns.Sum(column => GetColumnSize(column));
                	table.RowSize = rowSize;
                    table.Size = table.RowCount * rowSize;
                    table.Rows = new MetadataRow[table.RowCount];
                    pos += table.Size;
                }
            }
        }
		#endregion
		#endregion

		#region Heaps

		private string FetchString(uint offset)
        {
            if (_strings == null)
                throw new BadMetadataException("#Strings heap not found.");
	        return _strings.Fetch(offset);
        }

		public string GetUserString(uint offset)
		{
			if (_userStrings == null)
				throw new BadMetadataException("#US heap not found.");
			return _userStrings.Fetch(offset);
		}

		private Guid FetchGuid(uint index)
		{
			if (index == 0) 
				return Guid.Empty;
			if (_guids == null)
				throw new BadMetadataException("#GUID heap not found.");
			//guid index is 1 based
			return _guids.Fetch((int)(index - 1));
		}

		private BufferedBinaryReader GetBlob(uint offset)
        {
            if (_blob == null)
                throw new BadMetadataException("#Blob heap not found.");
			return _blob.Fetch(offset);
        }

		private sealed class StringHeap
		{
			private readonly BufferedBinaryReader _heap;
			private readonly Dictionary<uint, string> _cache = new Dictionary<uint, string>();
			
			public StringHeap(BufferedBinaryReader heap)
			{
				_heap = heap;
			}

			public string Fetch(uint offset)
			{
				if (offset >= _heap.Length)
					throw new BadMetadataException("Invalid #Strings heap index.");

				string value;
				if (_cache.TryGetValue(offset, out value))
					return value;

				_heap.Seek(offset, SeekOrigin.Begin);

				value = _heap.ReadUtf8();
				_cache.Add(offset, value);

				return value;
			}
		}

		private sealed class UserStringHeap
		{
			private readonly BufferedBinaryReader _heap;

			public UserStringHeap(BufferedBinaryReader heap)
			{
				_heap = heap;
			}

			public string Fetch(uint offset)
			{
				if (offset == 0)
					throw new BadMetadataException("Invalid #US heap offset.");


				_heap.Seek(offset, SeekOrigin.Begin);

				int length = _heap.ReadPackedInt();
				var bytes = _heap.ReadBytes(length);
				
				if (bytes[length - 1] == 0 || bytes[length - 1] == 1)
					length--;

				return Encoding.Unicode.GetString(bytes, 0, length);
			}
		}

		private sealed class GuidHeap
		{
			private readonly BufferedBinaryReader _heap;
			private IReadOnlyList<Guid> _list;

			public GuidHeap(BufferedBinaryReader heap)
			{
				_heap = heap;
			}

			public Guid Fetch(int index)
			{
				return List[index];
			}

			private IReadOnlyList<Guid> List
			{
				get { return _list ?? (_list = Populate().Memoize()); }
			}

			private IEnumerable<Guid> Populate()
			{
				_heap.Seek(0, SeekOrigin.Begin);

				long size = _heap.Length;
				while (size > 0)
				{
					var guid = _heap.ReadBytes(16);
					yield return new Guid(guid);
					size -= 16;
				}
			}
		}

		private sealed class BlobHeap
		{
			private readonly BufferedBinaryReader _heap;

			public BlobHeap(BufferedBinaryReader heap)
			{
				_heap = heap;
			}

			public BufferedBinaryReader Fetch(uint offset)
			{
				if (offset >= _heap.Length)
					throw new BadMetadataException("Invalid #Blob heap offset.");

				_heap.Seek(offset, SeekOrigin.Begin);

				int length = _heap.ReadPackedInt();
				if (length <= 0)
				{
					return Zero;
				}

				return _heap.Slice(_heap.Position, length);
			}

			private static readonly BufferedBinaryReader Zero = new BufferedBinaryReader(new byte[0]);
		}

		#endregion

		#region Dump
		internal void Dump(string path)
        {
            var xws = new XmlWriterSettings { Indent = true, IndentChars = "  " };
            using (var writer = XmlWriter.Create(path, xws))
            {
                writer.WriteStartDocument();
                Dump(writer);
                writer.WriteEndDocument();
            }
        }

        internal void Dump(XmlWriter writer)
        {
            writer.WriteStartElement("mdb");
            foreach (var table in _tables)
            {
                if (table != null)
                    Dump(writer, table);
            }
            writer.WriteEndElement();
        }

        private static void Dump(XmlWriter writer, MetadataTable table)
        {
            writer.WriteStartElement("table");
            writer.WriteAttributeString("name", table.Name);
            writer.WriteAttributeString("id", string.Format("{0} (0x{0:X2})", (int)table.Id));
            writer.WriteAttributeString("offset", XmlConvert.ToString(table.Offset));
            writer.WriteAttributeString("size", XmlConvert.ToString(table.Size));
            writer.WriteAttributeString("aligned", XmlConvert.ToString((table.Size % 4) == 0));
            writer.WriteAttributeString("row-count", XmlConvert.ToString(table.RowCount));
            writer.WriteAttributeString("row-size", XmlConvert.ToString(table.RowSize));
            writer.WriteAttributeString("sorted", XmlConvert.ToString(table.IsSorted));
            writer.WriteEndElement();
        }
		#endregion

		public IEnumerable<MetadataRow> LookupRows(TableId tableId, MetadataColumn column, int target, bool simple)
		{
			var table = this[tableId];
			if (table == null)
				return Enumerable.Empty<MetadataRow>();

			var lookup = table.GetLookup(column);

			IList<int> list;
			var rowCount = GetRowCount(tableId);
			while (lookup.LastIndex < rowCount)
			{
				var row = GetRow(tableId, lookup.LastIndex);
				int key = simple ? row[column].Index - 1 : (int)((SimpleIndex)row[column].Value);

				if (!lookup.TryGetValue(key, out list))
				{
					list = new List<int> {lookup.LastIndex};
					lookup.Add(key, list);
				}

				for (lookup.LastIndex++; lookup.LastIndex < rowCount; lookup.LastIndex++)
				{
					row = GetRow(tableId, lookup.LastIndex);
					int key2 = simple ? row[column].Index - 1 : (int)((SimpleIndex)row[column].Value);
					if (key != key2) break;
					list.Add(lookup.LastIndex);
				}
			}

			if (lookup.TryGetValue(target, out list))
			{
				return GetRows(tableId, list);
			}

			return Enumerable.Empty<MetadataRow>();
		}

		private IEnumerable<MetadataRow> GetRows(TableId tableId, IEnumerable<int> rows)
		{
			return rows.Select(i => GetRow(tableId, i));
		}

		public MetadataRow LookupRow(TableId tableId, MetadataColumn column, int target, bool simple)
		{
			var table = this[tableId];
			if (table == null)
				return null;

			var lookup = table.GetLookup(column);

			IList<int> list;
			if (lookup.TryGetValue(target, out list))
			{
				return GetRow(tableId, list[0]);
			}

			var rowCount = GetRowCount(tableId);
			for (; lookup.LastIndex < rowCount; lookup.LastIndex++)
			{
				var row = GetRow(tableId, lookup.LastIndex);
				int index = simple ? row[column].Index - 1 : (int)((SimpleIndex)row[column].Value);

				if (index == target)
				{
					lookup.Add(target, new List<int> { lookup.LastIndex });
					lookup.LastIndex++;
					return row;
				}

				if (!lookup.TryGetValue(index, out list))
				{
					list = new List<int>();
					lookup.Add(index, list);
				}

				list.Add(lookup.LastIndex);
			}

			return null;
		}
    }
}