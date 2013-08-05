using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf
{
	public sealed class SwfFocalGradient : SwfGradient
	{
		public float FocalPoint { get; set; }

		public override void Read(SwfReader reader, bool alpha)
		{
			base.Read(reader, alpha);
			FocalPoint = reader.ReadFixed16();
		}

		public override void Write(SwfWriter writer, bool alpha)
		{
			base.Write(writer, alpha);
			writer.WriteFixed16(FocalPoint);
		}

		public override void DumpBody(XmlWriter writer, bool alpha)
		{
			base.DumpBody(writer, alpha);
			writer.WriteElementString("focal-point", FocalPoint.ToString());
		}
	}
}