using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.SetTabIndex)]
    public class SwfTagSetTabIndex : SwfTag
    {
        public ushort Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private ushort _depth;

        public ushort TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }
        private ushort _tabIndex;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SetTabIndex; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _depth = reader.ReadUInt16();
            _tabIndex = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_depth);
            writer.WriteUInt16(_tabIndex);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("depth", _depth.ToString());
            writer.WriteElementString("tab-index", _tabIndex.ToString());
        }
    }
}