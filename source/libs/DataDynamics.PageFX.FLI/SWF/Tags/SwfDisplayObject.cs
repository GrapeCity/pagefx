namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    public abstract class SwfDisplayObject : SwfCharacter, ISwfDisplayObject
    {
    	public ushort Depth { get; set; }
    }
}