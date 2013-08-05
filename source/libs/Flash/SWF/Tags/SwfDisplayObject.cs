namespace DataDynamics.PageFX.Flash.Swf.Tags
{
    public abstract class SwfDisplayObject : SwfCharacter, ISwfDisplayObject
    {
    	public ushort Depth { get; set; }
    }
}