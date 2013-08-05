using System.Drawing;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Bitmaps
{
	[SwfTag(SwfTagCode.DefineBitsLossless2)]
	public sealed class SwfTagDefineBitsLossless2 : SwfTagDefineBitsLossless
	{
		public SwfTagDefineBitsLossless2()
		{
		}

		public SwfTagDefineBitsLossless2(Image image)
			: base(image)
		{
		}

		public SwfTagDefineBitsLossless2(int id, Image image)
			: base(id, image)
		{
		}

		public override SwfTagCode TagCode
		{
			get { return SwfTagCode.DefineBitsLossless2; }
		}

		protected override void ReadBody(SwfReader reader)
		{
			ReadBody(reader, true);
		}

		protected override void WriteBody(SwfWriter writer)
		{
			WriteBody(writer, true);
		}
	}
}