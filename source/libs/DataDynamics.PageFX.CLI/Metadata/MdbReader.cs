using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using DataDynamics.PE;

namespace DataDynamics.PageFX.CLI.Metadata
{
    //ref: Partition II, 24.2.1 Metadata root

    #region class MdbTableHeapHeader
    //Size = (4 + 1 + 1 + 1 + 1 + 8 + 8) = 24
    public sealed class MdbTableHeapHeader
    {
        public byte MajorVersion;
        public byte MinorVersion;
        public byte HeapSizes;
        public ulong Valid;
        public ulong Sorted;

        public MdbTableHeapHeader(BufferedBinaryReader reader)
        {
            int r1 = reader.ReadInt32(); //reserved, always 0
            MajorVersion = reader.ReadUInt8();
            MinorVersion = reader.ReadUInt8();
            HeapSizes = reader.ReadUInt8();
            int r2 = reader.ReadUInt8(); //reserved, always 1
            Valid = reader.ReadUInt64();
            Sorted = reader.ReadUInt64();
        }
    }
    #endregion

    /// <summary>
    /// MetaDataBase Reader
    /// </summary>
    public sealed class MdbReader : IDisposable
    {
        #region Fields
        private BufferedBinaryReader _reader;
        //private List<string> _strings;
        private MdbStream _strings;
        private MdbStream _userStrings;
        //private List<byte[]> _blob;
        private MdbStream _blob;
        private List<Guid> _guids;
        private PEImage _image;
        #endregion

        #region Constructors
        public MdbReader(string path) 
            : this(new PEImage(path))
        {
        }

        public MdbReader(Stream s)
            : this(new PEImage(s))
        {
        }

        public MdbReader(PEImage image)
        {
            Load(image);
        }
        #endregion

        #region IDisposable Members
        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~MdbReader()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
        #endregion

        #region Public Members
        int _resourceOrigin = -1;

        public BufferedBinaryReader SeekResourceOffset(int offset)
        {
            if (_resourceOrigin < 0)
            {
                int pos = _image.TranslateRVA(_cliHeader.Resources.RVA);
                if (pos < 0)
                    throw new BadMetadataException();
                _resourceOrigin = pos;
            }
            if (offset >= _cliHeader.Resources.Size)
                throw new BadMetadataException();
            _reader.Position = _resourceOrigin + offset;
            return _reader;
        }

        public int SizeOfResources
        {
            get
            {
                if (_cliHeader != null)
                    return (int)_cliHeader.Resources.Size;
                return 0;
            }
        }

        public BufferedBinaryReader SeekRVA(uint rva)
        {
            int pos = _image.TranslateRVA(rva);
            if (pos < 0)
                throw new BadMetadataException();
            _reader.Position = pos;
            return _reader;
        }

        public MdbTable this[MdbTableId tableId]
        {
            get
            {
                int i = (int)tableId;
                if (i < 0 || i >= _tables.Length)
                    throw new ArgumentOutOfRangeException("tableId");
                return _tables[i];
            }
        }

        public IEnumerable<MdbRow> GetRows(MdbTableId tableId)
        {
            int n = GetRowCount(tableId);
            for (int i = 0; i < n; ++i)
                yield return GetRow(tableId, i);
        }

        public int GetRowCount(MdbTableId id)
        {
            var table = this[id];
            if (table == null) return 0;
            return table.RowCount;
        }

        public MdbRow GetRow(MdbTableId tableId, int rowIndex)
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
            var cells = new MdbCell[n];
            for (int i = 0; i < n; ++i)
            {
                var col = table.Columns[i];
                cells[i] = ReadCell(col);
            }

            row = new MdbRow(rowIndex, cells);
            table.Rows[rowIndex] = row;
            return row;
        }

        public MdbRow this[MdbTableId tableId, int rowIndex]
        {
            get
            {
                return GetRow(tableId, rowIndex);
            }
        }

        MdbCell ReadCell(MdbColumn column)
        {    
            switch(column.Type)
            {
                case MdbColumnType.Int32:
                    {
                        uint i = _reader.ReadUInt32();
                        return new MdbCell(column, i);
                    }

                case MdbColumnType.Int16:
                    {
                        uint i = _reader.ReadUInt16();
                        return new MdbCell(column, i);
                    }

                case MdbColumnType.StringIndex:
                    {
                        uint i = _reader.ReadIndex(_strIdxSize);
                        string s = GetString(i);
                        return new MdbCell(column, i, s);
                    }

                case MdbColumnType.BlobIndex:
                    {
                        uint i = _reader.ReadIndex(_blobIdxSize);
                        var data = GetBlob(i);
                        return new MdbCell(column, i, data);
                    }

                case MdbColumnType.GuidIndex:
                    {
                        uint i = _reader.ReadIndex(_guidIdxSize);
                        return new MdbCell(column, i, GetGuid(i));
                    }

                case MdbColumnType.SimpleIndex:
                    {
                        uint i = ReadIndex(column.SimpleIndex);
                        return new MdbCell(column, i);
                    }

                case MdbColumnType.CodedIndex:
                    {
                        uint i = ReadIndex(column.CodedIndex);
                        return new MdbCell(column, i);
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        MdbIndex ReadIndex(MdbTableId id)
        {
            int size = GetSimpleIndexSize(id);
            uint index = _reader.ReadIndex(size);
            return new MdbIndex(id, (int)index);
        }

        MdbIndex ReadIndex(MdbCodedIndex i)
        {
            int size = GetCodedIndexSize(i);
            uint index = _reader.ReadIndex(size);
            return i.Decode(index);
        }
        #endregion

        #region ColumnSize
        static int GetIndexSize(int n)
        {
            return n >= 0x10000 ? 4 : 2;
        }

        int GetSimpleIndexSize(MdbTableId id)
        {
            int n = GetRowCount(id);
            return GetIndexSize(n);
        }

        int GetMaxRowCount(IEnumerable<MdbTableId> tables)
        {
            int maxRowCount = 0;
            foreach (var id in tables)
            {
                int n = GetRowCount(id);
                if (n > maxRowCount)
                    maxRowCount = n;
            }
            return maxRowCount;
        }

        int[] _codedIndexSizes;
        int GetCodedIndexSize(MdbCodedIndex i)
        {
            return _codedIndexSizes[i.ID];
        }

        int GetColumnSize(MdbColumn c)
        {
            switch (c.Type)
            {
                case MdbColumnType.Int32: return 4;
                case MdbColumnType.Int16: return 2;
                case MdbColumnType.StringIndex: return _strIdxSize;
                case MdbColumnType.BlobIndex: return _blobIdxSize;
                case MdbColumnType.GuidIndex: return _guidIdxSize;
                case MdbColumnType.SimpleIndex: return GetSimpleIndexSize(c.SimpleIndex);
                case MdbColumnType.CodedIndex: return GetCodedIndexSize(c.CodedIndex);
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Loading
        void Load(PEImage image)
        {
            _image = image;
            _reader = image.Reader;

            //CLIHeader
            uint mdbOffset = ReadCLIHeader(image);

            _reader.Position = mdbOffset;
            var header = new MdbHeader(_reader);

            foreach (var mds in header.Streams)
            {
                mds.Offset += mdbOffset;
                LoadStream(mds);
            }
        }

        #region LoadStream
        void LoadStream(MdbStream mds)
        {
            string name = mds.Name;
            if (name == "#~" || name == "#-")
            {
                if (_tables != null)
                    throw new BadMetadataException("Multiple table heaps.");

                _reader.Position = mds.Offset;
                LoadTables();
                return;
            }

            if (name == "#Strings")
            {
                if (_strings != null)
                    throw new BadMetadataException("Multiple #Strings heaps.");
                _strings = mds;
                return;
            }

            if (name == "#US")
            {
                if (_userStrings != null)
                    throw new BadMetadataException("Multiple #US heaps.");
                _userStrings = mds;
                return;
            }

            if (name == "#GUID")
            {
                if (_guids != null)
                    throw new BadMetadataException("Multiple #GUID heaps.");
                _guids = new List<Guid>();
                _reader.Position = mds.Offset;
                long endPos = mds.Offset + mds.Size;
                while (_reader.Position < endPos)
                {
                    var guid = _reader.ReadBlock(16);
                    _guids.Add(new Guid(guid));
                }
                return;
            }

            if (name == "#Blob")
            {
                if (_blob != null)
                    throw new BadMetadataException("Multiple #Blob heaps.");
                _blob = mds;
                return;
            }

            throw new BadMetadataException("Unknown meta-data stream.");
        }
        #endregion

        #region CLI Header
        /// <summary>
        /// CLI Header
        /// </summary>
        public CLIHeader CLIHeader
        {
            get { return _cliHeader; }
        }
        CLIHeader _cliHeader;

        uint ReadCLIHeader(PEImage image)
        {
            if (!image.GotoDirectory(PEDictionaryEntry.ENTRY_COM_DESCRIPTOR))
                throw new BadImageFormatException("No COM DESCRIPTOR (CLI Header)");

            var cli = new CLIHeader(image.Reader);
            if (cli.MetaData.Size == 0)
                throw new BadImageFormatException("Metadata is empty");

            _cliHeader = cli;
            int offset = image.TranslateRVA(cli.MetaData.RVA);
            if (offset <= 0)
                throw new BadImageFormatException("Invalid metadata offset");
            return (uint)offset;
        }
        #endregion

        int _strIdxSize;
        int _blobIdxSize;
        int _guidIdxSize;
        MdbTable[] _tables;
        internal const int MaxTableNum = 64;

        void LoadTables()
        {
            var header = new MdbTableHeapHeader(_reader);
            _strIdxSize = (((header.HeapSizes & 1) == 0) ? 2 : 4);
            _guidIdxSize = (((header.HeapSizes & 2) == 0) ? 2 : 4);
            _blobIdxSize = (((header.HeapSizes & 4) == 0) ? 2 : 4);

            //Read table row nums
            ulong present = header.Valid;
            _tables = new MdbTable[MaxTableNum];
            for (int i = 0; i < MaxTableNum; i++)
            {
                //NOTE: If flag set table is presented
                if (((present >> i) & 1) != 0)
                {
                    int rowNum = _reader.ReadInt32();
                    var id = (MdbTableId)i;
                    var table = MDB.CreateTable(id);
                    table.RowCount = rowNum;
                    table.IsSorted = ((header.Sorted >> i) & 1) != 0;
                    _tables[i] = table;
                }
            }

            //Prepare coded index sizes
            int n = MdbCodedIndex.All.Length;
            _codedIndexSizes = new int[n];
            for (int i = 0; i < n; ++i)
            {
                var ci = MdbCodedIndex.All[i];
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
                    int rowSize = 0;
                    foreach (var column in table.Columns)
                    {
                        int colSize = GetColumnSize(column);
                        rowSize += colSize;
                    }
                    table.RowSize = rowSize;
                    table.Size = table.RowCount * rowSize;
                    table.Rows = new MdbRow[table.RowCount];
                    pos += table.Size;
                }
            }
        }
        #endregion

        #region Heap Access
        private string GetString(uint offset)
        {
            if (_strings == null)
                throw new BadMetadataException("#Strings heap not found.");
            if (offset >= _strings.Size)
                throw new BadMetadataException("Invalid #Strings heap index.");
            long pos = _reader.Position;
            _reader.Position = _strings.Offset + offset;
            string s = _reader.ReadUtf8();
            _reader.Position = pos;
            return s;
        }

        private byte[] GetBlob(uint offset)
        {
            if (_blob == null)
                throw new BadMetadataException("#Blob heap not found.");
            if (offset >= _blob.Size)
                throw new BadMetadataException("Invalid #Blob heap offset.");
            long pos = _reader.Position;
            _reader.Position = _blob.Offset + offset;
            int length = _reader.ReadPackedInt();
            var res = new byte[0];
            if (length > 0)
                res = _reader.ReadBlock(length);
            _reader.Position = pos;
            return res;
        }

        private Guid GetGuid(uint index)
        {
            if (index == 0) 
                return Guid.Empty;
            if (_guids == null)
                throw new BadMetadataException("#GUID heap not found.");
            //guid index is 1 based
            index = index - 1;
            if (index >= _guids.Count)
                throw new BadMetadataException("Invalid #GUID heap index.");
            return _guids[(int)index];
        }

        public string GetUserString(uint offset)
        {
            if (_userStrings == null)
                throw new BadMetadataException("#US heap not found.");
            if (offset == 0)
                throw new BadMetadataException("Invalid #US heap offset.");
            long pos = _reader.Position;
            _reader.Position = _userStrings.Offset + offset;
            int length = _reader.ReadPackedInt();
            var bytes = _reader.ReadBlock(length);
            _reader.Position = pos;
            if (bytes[length - 1] == 0 || bytes[length - 1] == 1)
                length--;
            return Encoding.Unicode.GetString(bytes, 0, length);
        }
        #endregion

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

        static void Dump(XmlWriter writer, MdbTable table)
        {
            writer.WriteStartElement("table");
            writer.WriteAttributeString("name", table.Name);
            writer.WriteAttributeString("id", string.Format("{0} (0x{0:X2})", (int)table.Id));
            writer.WriteAttributeString("offset", table.Offset.ToString());
            writer.WriteAttributeString("size", table.Size.ToString());
            writer.WriteAttributeString("aligned", XmlConvert.ToString((table.Size % 4) == 0));
            writer.WriteAttributeString("row-count", table.RowCount.ToString());
            writer.WriteAttributeString("row-size", table.RowSize.ToString());
            writer.WriteAttributeString("sorted", XmlConvert.ToString(table.IsSorted));
            writer.WriteEndElement();
        }
    }
}