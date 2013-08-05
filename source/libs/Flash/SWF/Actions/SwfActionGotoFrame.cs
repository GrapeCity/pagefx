using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Actions
{
    [SwfAction(SwfActionCode.GotoFrame)]
    public sealed class SwfActionGotoFrame : SwfAction
    {
	    public ushort Frame { get; set; }

	    public override SwfActionCode ActionCode
        {
            get { return SwfActionCode.GotoFrame; }
        }

        public override void ReadBody(SwfReader reader)
        {
            Frame = reader.ReadUInt16();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.WriteUInt16(Frame);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("frame", Frame.ToString());
        }
    }
}