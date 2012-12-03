using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
    [SwfAction(SwfActionCode.GotoLabel)]
    public class SwfActionGotoLabel : SwfAction
    {
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private string _label;

        public override SwfActionCode ActionCode
        {
            get { return SwfActionCode.GotoLabel; }
        }

        public override void ReadBody(SwfReader reader)
        {
            _label = reader.ReadString();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.WriteString(_label);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("label", _label);
        }
    }
}