using System.Drawing;

namespace DataDynamics.PageFX.FlashLand.Swf
{
	public interface ISwfImageCharacter : ISwfCharacter
	{
		Image Image { get; }
	}
}