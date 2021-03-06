using System.Xml;
using DataDynamics.PageFX.Flash.Swf.Actions;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Actions
{
    [SwfTag(SwfTagCode.DoAction)]
    public class SwfTagDoAction : SwfTag
    {
        public SwfActionList Actions
        {
            get { return _actions; }
        }
        private readonly SwfActionList _actions = new SwfActionList();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DoAction; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _actions.Read(reader);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            _actions.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            _actions.DumpXml(writer);
        }
    }
}