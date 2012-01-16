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

    	public string Location { get; set; }

    	public byte[] HashValue { get; set; }

    	public bool ContainsMetadata { get; set; }

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