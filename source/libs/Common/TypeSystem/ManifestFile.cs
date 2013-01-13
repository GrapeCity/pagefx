using System.IO;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class ManifestFile : CustomAttributeProvider, IManifestFile
    {
	    private Stream _data;

	    public string Name { get; set; }

    	public int Offset { get; set; }

    	public bool IsPublic { get; set; }

    	public IModule Module { get; set; }

		public Stream Data
		{
			get
			{
				if (_data != null)
				{
					_data.Seek(0, SeekOrigin.Begin);
				}
				return _data;
			}
			set { _data = value; }
		}

	    public string Location { get; set; }

    	public byte[] HashValue { get; set; }

    	public bool ContainsMetadata { get; set; }
    }

    public sealed class ManifestFileCollection : HashList<string, IManifestFile>, IManifestFileCollection
    {
        public ManifestFileCollection()
            : base(f => f.Name)
        {
        }
    }
}