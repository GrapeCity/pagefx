using System.Xml;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Actions
{
    [SwfTag(SwfTagCode.DoAbc2)]
    public class SwfTagDoAbc2 : SwfTagDoAbc
    {
        public SwfTagDoAbc2()
        {
        }

        public SwfTagDoAbc2(string frameName, uint flags, AbcFile abc)
        {
            _frameName = frameName;
            _flags = flags;
            ByteCode = abc;
        }

        public uint Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private uint _flags;

        public string FrameName
        {
            get { return _frameName; }
            set { _frameName = value; }
        }
        private string _frameName;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DoAbc2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _flags = reader.ReadUInt32();
            _frameName = reader.ReadString();
            ByteCode = new AbcFile(reader) {Name = _frameName};
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt32(Flags);
            writer.WriteString(FrameName);
            ByteCode.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("frame-name", _frameName);
            writer.WriteElementString("action-flags", _flags.ToString());
            base.DumpBody(writer);
        }

        public override void DumpShortBody(XmlWriter writer)
        {
            writer.WriteElementString("frame-name", _frameName);
            writer.WriteElementString("action-flags", _flags.ToString());
        }
    }
}