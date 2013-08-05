namespace DataDynamics.PageFX.Flash.Swf
{
	public interface ISwfAtom
	{
		void Read(SwfReader reader);
		void Write(SwfWriter writer);
	}

	public interface ISwfIndexedAtom : ISwfAtom
	{
		int Index { get; set; }
	}
}