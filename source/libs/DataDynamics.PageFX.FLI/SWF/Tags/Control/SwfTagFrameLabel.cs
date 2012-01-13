using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Names a frame or anchor. This frame can later be referenced using this name.
    /// </summary>
    [SwfTag(SwfTagCode.FrameLabel)]
    public class SwfTagFrameLabel : SwfTag
    {
        public SwfTagFrameLabel()
        {    
        }

        public SwfTagFrameLabel(string label)
        {
            _label = label;
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private string _label;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.FrameLabel; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _label = reader.ReadString();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(_label);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("label", _label);
        }
    }
}