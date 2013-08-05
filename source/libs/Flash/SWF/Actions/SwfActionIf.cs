using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Actions
{
    [SwfAction(SwfActionCode.If)]
    public sealed class SwfActionIf : SwfAction
    {
	    public short Offset { get; set; }

	    public override SwfActionCode ActionCode
        {
            get { return SwfActionCode.If; }
        }

        public override void ReadBody(SwfReader reader)
        {
            Offset = reader.ReadInt16();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.WriteInt16(Offset);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("offset", Offset.ToString());
        }
    }
}