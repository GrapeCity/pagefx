using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents manifest resource.
    /// </summary>
    public class ManifestResource : CustomAttributeProvider, IManifestResource
    {
        #region IManifestResource Members
        /// <summary>
        /// Gets the resource name.
        /// </summary>
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

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// List of <see cref="ManifestResource"/>s.
    /// </summary>
    public class ManifestResourceCollection : List<IManifestResource>, IManifestResourceCollection
    {
        #region IManifestResourceCollection Members
        public IManifestResource this[string name]
        {
            get
            {
                return Find(delegate(IManifestResource r)
                                {
                                    return r.Name == name;
                                });
            }
        }
        #endregion
    }
}