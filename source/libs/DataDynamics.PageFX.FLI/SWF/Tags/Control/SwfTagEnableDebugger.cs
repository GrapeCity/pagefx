using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.EnableDebugger)]
    public class SwfTagEnableDebugger : SwfTag
    {
        public SwfTagEnableDebugger()
        {
        }

        public SwfTagEnableDebugger(string password)
        {
            _password = password;
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _password;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.EnableDebugger; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _password = reader.ReadString();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(_password);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("password", _password);
        }
    }
}