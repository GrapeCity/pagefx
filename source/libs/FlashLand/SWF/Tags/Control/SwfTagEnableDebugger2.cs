using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.EnableDebugger2)]
    public class SwfTagEnableDebugger2 : SwfTag
    {
        public SwfTagEnableDebugger2()
        {
        }

        public SwfTagEnableDebugger2(ushort flags, string password)
        {
            _flags = flags;
            _password = password;
        }

        public ushort Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private ushort _flags;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _password;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.EnableDebugger2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _flags = reader.ReadUInt16();
            _password = reader.ReadString();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_flags);
            writer.WriteString(_password);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("flags", _flags.ToString());
            writer.WriteElementString("password", _password);
        }
    }
}