using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
    [SwfAction(SwfActionCode.GotoLabel)]
    public sealed class SwfActionGotoLabel : SwfAction
    {
	    public string Label { get; set; }

	    public override SwfActionCode ActionCode
        {
            get { return SwfActionCode.GotoLabel; }
        }

        public override void ReadBody(SwfReader reader)
        {
            Label = reader.ReadString();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.WriteString(Label);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("label", Label);
        }
    }
}