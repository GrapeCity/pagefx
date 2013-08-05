using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Actions
{
    public abstract class SwfAction : ISupportXmlDump
    {
	    private static Dictionary<SwfActionCode, int> _mapver;

        /// <summary>
        /// Returns minimum SWF version where action with specified code was defined.
        /// </summary>
        /// <param name="code">action code</param>
        /// <returns></returns>
        public static int GetActionVersion(SwfActionCode code)
        {
            if (_mapver == null)
            {
                _mapver = EnumReflector.GetAttributeMap<SwfActionCode, int, SwfVersionAttribute>(attr => attr.Version);
            }
            int result;
            if (_mapver.TryGetValue(code, out result))
                return result;
            return -1;
        }

	    /// <summary>
        /// Gets the action code.
        /// </summary>
        public abstract SwfActionCode ActionCode { get; }

        /// <summary>
        /// Gets the action version.
        /// </summary>
        public int ActionVersion
        {
            get { return GetActionVersion(ActionCode); }
        }
        
        /// <summary>
        /// Reads the action data.
        /// </summary>
        /// <param name="reader"><see cref="SwfReader"/> used to read action data.</param>
        public abstract void ReadBody(SwfReader reader);

        /// <summary>
        /// Writes the action data.
        /// </summary>
        /// <param name="writer"><see cref="SwfWriter"/> used to write action data.</param>
        public abstract void WriteBody(SwfWriter writer);

        public virtual byte[] GetData()
        {
            var writer = new SwfWriter();
            WriteBody(writer);
            return writer.ToByteArray();
        }

        public int GetDataSize()
        {
            var data = GetData();
            if (data != null)
                return data.Length;
            return 0;
        }

	    public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("action");
            var code = ActionCode;
            writer.WriteAttributeString("name", code.ToString());
            writer.WriteAttributeString("code", ((int)code).ToString());
            writer.WriteAttributeString("codex", string.Format("0x{0:X2}", (int)code));
            writer.WriteAttributeString("size", GetDataSize().ToString());
            writer.WriteAttributeString("version", ActionVersion.ToString());
            DumpBody(writer);
            writer.WriteEndElement();
        }

        public virtual void DumpBody(XmlWriter writer)
        {
        }
    }
}