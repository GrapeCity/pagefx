using System;
using System.IO;

namespace DataDynamics.PE
{
    /// <summary>
    /// PE file image
    /// </summary>
    public class PEImage
    {
        #region Member Variables
        private DOSHeader _dosHeader;
        private COFFHeader _coffHeader;
        private PEHeader _peHeader;
        private NTHeader _ntHeader;
        private PEDataDirectory[] _dirs;
        private PESectionList _sections;
        private readonly BufferedBinaryReader _reader;
        #endregion

        #region Constructors
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="path">path to PE file image</param>
        public PEImage(string path) : this(new BufferedBinaryReader(path))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="imageStream">PE file native image stream</param>
        public PEImage(Stream imageStream) : this(new BufferedBinaryReader(imageStream))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="image">PE file native image bytes</param>
        public PEImage(byte[] image) : this(new BufferedBinaryReader(image))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="reader">PE file native image reader</param>
        public PEImage(BufferedBinaryReader reader)
        {
            _reader = reader;
            Read(reader);
        }

        private void Read(BufferedBinaryReader reader)
        {
            _dosHeader = new DOSHeader(reader);
            reader.Position = _dosHeader.NewHeaderOffset;

            _coffHeader = new COFFHeader(reader);
            _peHeader = new PEHeader(reader);
            _ntHeader = new NTHeader(reader);

            var dirs = new PEDataDirectory[_ntHeader.NumberOfDataDirectories];
            for (int i = 0; i < dirs.Length; i++)
                dirs[i] = new PEDataDirectory(reader);
            _dirs = dirs;

            _sections = new PESectionList();
            for (int i = 0; i < _coffHeader.NumberOfSections; i++)
            {
                var section = new PESection(reader);
                _sections.Add(section);
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// PE File DOS stub header
        /// </summary>
        public DOSHeader DOSheader
        {
            get { return _dosHeader; }
        }

        /// <summary>
        /// PE File COFF header
        /// </summary>
        public COFFHeader COFFheader
        {
            get { return _coffHeader; }
        }

        /// <summary>
        /// PE File PE header
        /// </summary>
        public PEHeader PEHeader
        {
            get { return _peHeader; }
        }

        /// <summary>
        /// PE File Optional NT header
        /// </summary>
        public NTHeader NTHeader
        {
            get { return _ntHeader; }
        }

        /// <summary>
        /// PE File directory infos
        /// </summary>
        public PEDataDirectory[] Entries
        {
            get { return _dirs; }
        }

        /// <summary>
        /// PE file sections dictionary
        /// </summary>
        public PESectionList Sections
        {
            get { return _sections; }
        }

        /// <summary>
        /// PE file Binary data
        /// </summary>
        public Stream RawBytes
        {
            get { return _reader; }
        }

        /// <summary>
        /// For internal usage only - access to the BinaryReader
        /// </summary>
        internal BufferedBinaryReader Reader
        {
            get { return _reader; }
        }
        #endregion

        #region RVA Utils
        /// <summary>
        /// Translate Relative Virtual Address (RVA) to the actual Virtual Address (VA)
        /// </summary>
        /// <param name="rva">RVA to translate</param>
        /// <returns></returns>
        public int TranslateRVA(uint rva)
        {
            foreach (var section in _sections)
            {
                if (rva >= section.VirtualAddress && rva < section.VirtualAddress + section.SizeOfRawData)
                {
                    return (int)(section.PointerToRawData + rva - section.VirtualAddress);
                }
            }
            //throw new BadImageException(string.Format("Invalid RVA address '0x{0:x8}'", rva));
            return -1;
        }

        public bool GotoDirectory(int id)
        {
            uint rva = _dirs[id].RVA;
            int pos = TranslateRVA(rva);
            if (pos < 0) return false;
            _reader.Position = pos;
            return true;
        }
        #endregion

        #region Dir Utils
        /// <summary>
        /// Read directory from PE File
        /// </summary>
        /// <param name="directoryId"></param>
        /// <returns></returns>
        public PEDirectory GetDirectory(int directoryId)
        {
            if (directoryId >= _dirs.Length || directoryId < 0)
                throw new ArgumentOutOfRangeException("directoryId", directoryId,
                                                      string.Concat("Must be in range 0 - ", _dirs.Length.ToString()));

            if (_dirs[directoryId].Size == 0)
                return null;

            int offset = TranslateRVA(_dirs[directoryId].RVA);
            long oldPos = _reader.Position;
            _reader.Position = offset;

            uint size = _dirs[directoryId].Size;
            var dir = CreateDir(directoryId, size);

            if (dir != null)
                dir.Read(_reader, (int)size);
            _reader.Position = oldPos;
            return dir;
        }

        /// <summary>
        /// Create PE directory according its Index
        /// </summary>
        /// <param name="directoryId">Directory index</param>
        /// <param name="dirSize">Size of directory</param>
        /// <returns>Craeted object or null if:
        /// <list type="bullet">
        ///	<item>
        ///		<code>Directory type not implemented yet</code>
        ///	</item>
        ///	<item>
        ///		<code>Passed Size of directory less then required</code>
        ///	</item>
        ///	<item>
        ///		<code>Unknown directory index passsed</code>
        ///	</item>
        /// </list>
        /// </returns>
        private static PEDirectory CreateDir(int directoryId, uint dirSize)
        {
            switch (directoryId)
            {
                case PEDictionaryEntry.ENTRY_DEBUG:
                    if (dirSize < PEDebugDirectory.Entry.EntrySize)
                        return null;
                    return new PEDebugDirectory();
                default:
                    return null;
            }
        }
        #endregion
    }
}