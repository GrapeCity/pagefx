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
    	public string Name { get; set; }

    	public int Offset { get; set; }

    	public bool IsPublic { get; set; }

    	public IModule Module { get; set; }

    	public byte[] Data { get; set; }

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
                return Find(r => r.Name == name);
            }
        }

        #endregion
    }
}