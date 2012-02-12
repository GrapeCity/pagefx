using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class ManifestFile : CustomAttributeProvider, IManifestFile
    {
        #region IManifestResource Members

    	public string Name { get; set; }

    	public int Offset { get; set; }

    	public bool IsPublic { get; set; }

    	public IModule Module { get; set; }

    	public byte[] Data { get; set; }

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