using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class ManifestFile : CustomAttributeProvider, IManifestFile
    {
        #region IManifestResource Members
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        private int _offset;

        public bool IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }
        private bool _isPublic;

        public IModule Module
        {
            get { return _module; }
            set { _module = value; }
        }
        private IModule _module;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private byte[] _data;
        #endregion

        #region IManifestFile Members
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
        private string _location;

        public byte[] HashValue
        {
            get { return _hashValue; }
            set { _hashValue = value; }
        }
        private byte[] _hashValue;

        public bool ContainsMetadata
        {
            get { return _containsMetadata; }
            set { _containsMetadata = value; }
        }
        private bool _containsMetadata;
        #endregion
    }

    public sealed class ManifestFileCollection : HashedList<string, IManifestFile>, IManifestFileCollection
    {
        public ManifestFileCollection()
            : base(f => f.Name)
        {
        }
    }
}