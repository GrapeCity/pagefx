using System.Drawing;

namespace DataDynamics.PageFX.Flash.Swf
{
	public interface ISwfImageCharacter : ISwfCharacter
	{
		Image Image { get; }
	}
}