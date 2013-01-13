using System.IO;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents manifest resource.
    /// </summary>
    public sealed class ManifestResource : CustomAttributeProvider, IManifestResource
    {
	    private Stream _data;

	    /// <summary>
    	/// Gets the resource name.
    	/// </summary>
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

	    public override string ToString()
        {
            return Name;
        }
    }
}