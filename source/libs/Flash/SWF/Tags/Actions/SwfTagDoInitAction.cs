using System.Xml;
using DataDynamics.PageFX.Flash.Swf.Actions;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Actions
{
    [SwfTag(SwfTagCode.DoInitAction)]
    public class SwfTagDoInitAction : SwfTag
    {
        /// <summary>
        /// Gets or sets sprite id to which actions will apply.
        /// </summary>
        public ushort SpriteID
        {
            get { return _spriteID; }
            set { _spriteID = value; }
        }
        private ushort _spriteID;

        public SwfActionList Actions
        {
            get { return _actions; }
        }
        private readonly SwfActionList _actions = new SwfActionList();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DoInitAction; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _spriteID = reader.ReadUInt16();
            _actions.Read(reader);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_spriteID);
            _actions.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("sprite-id", _spriteID.ToString());
            _actions.DumpXml(writer);
        }
    }
}