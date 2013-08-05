using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    /// <summary>
    /// Names a frame or anchor. This frame can later be referenced using this name.
    /// </summary>
    [SwfTag(SwfTagCode.FrameLabel)]
    public sealed class SwfTagFrameLabel : SwfTag
    {
        public SwfTagFrameLabel()
        {    
        }

        public SwfTagFrameLabel(string label)
        {
            Label = label;
        }

	    public string Label { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.FrameLabel; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Label = reader.ReadString();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(Label);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("label", Label);
        }
    }
}