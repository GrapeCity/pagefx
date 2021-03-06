namespace DataDynamics.PageFX.Flash.Swf
{
	/// <summary>
	/// Represents SWF character.
	/// </summary>
	public interface ISwfCharacter : ISwfTag
	{
		ushort CharacterId { get; set; }

		string Name { get; set; }
	}
}