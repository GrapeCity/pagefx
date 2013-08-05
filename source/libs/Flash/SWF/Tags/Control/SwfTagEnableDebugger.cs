using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.EnableDebugger)]
    public sealed class SwfTagEnableDebugger : SwfTag
    {
        public SwfTagEnableDebugger()
        {
        }

        public SwfTagEnableDebugger(string password)
        {
            Password = password;
        }

	    public string Password { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.EnableDebugger; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Password = reader.ReadString();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(Password);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("password", Password);
        }
    }
}