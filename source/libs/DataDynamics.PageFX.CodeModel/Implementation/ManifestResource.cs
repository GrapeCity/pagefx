namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents manifest resource.
    /// </summary>
    public class ManifestResource : CustomAttributeProvider, IManifestResource
    {
	    /// <summary>
    	/// Gets the resource name.
    	/// </summary>
    	public string Name { get; set; }

    	public int Offset { get; set; }

    	public bool IsPublic { get; set; }

    	public IModule Module { get; set; }

    	public byte[] Data { get; set; }

	    public override string ToString()
        {
            return Name;
        }
    }
}