using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.EnableDebugger2)]
    public sealed class SwfTagEnableDebugger2 : SwfTag
    {
        public SwfTagEnableDebugger2()
        {
        }

        public SwfTagEnableDebugger2(ushort flags, string password)
        {
            Flags = flags;
            Password = password;
        }

	    public ushort Flags { get; set; }

	    public string Password { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.EnableDebugger2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Flags = reader.ReadUInt16();
            Password = reader.ReadString();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(Flags);
            writer.WriteString(Password);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("flags", Flags.ToString());
            writer.WriteElementString("password", Password);
        }
    }
}