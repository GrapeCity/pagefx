using System.IO;

namespace DataDynamics.PE
{
    /// <summary>
    /// PE section information record
    /// </summary>
    public sealed class PEDataDirectory
    {
        private uint _rva;
        private uint _size;

        public PEDataDirectory(BufferedBinaryReader reader)
        {
            _rva = reader.ReadUInt32();
            _size = reader.ReadUInt32();
        }

        /// <summary>
        /// Data RVA
        /// </summary>
        public uint RVA
        {
            get { return _rva; }
            set { _rva = value; }
        }

        /// <summary>
        /// Data size
        /// </summary>
        public uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var dir = obj as PEDataDirectory;
            if (dir == null) return false;
            if (dir._rva != _rva) return false;
            if (dir._size != _size) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return (int)_rva ^ (int)(_size << 1);
        }

        public override string ToString()
        {
            return string.Format("RVA = {0}, Size = {1}", _rva, _size);
        }
    }
}