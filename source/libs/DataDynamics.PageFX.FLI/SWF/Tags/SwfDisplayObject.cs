namespace DataDynamics.PageFX.FLI.SWF
{
    public abstract class SwfDisplayObject : SwfCharacter, ISwfDisplayObject
    {
    	public ushort Depth { get; set; }
    }
}