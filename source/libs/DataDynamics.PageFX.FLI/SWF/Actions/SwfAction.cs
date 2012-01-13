using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    public abstract class SwfAction : ISupportXmlDump
    {
        #region Shared Members
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
                _mapver = SwfHelper.GetEnumAttributeMap<SwfActionCode, int, SwfVersionAttribute>(
                    delegate(SwfVersionAttribute attr) { return attr.Version; });
            }
            int result;
            if (_mapver.TryGetValue(code, out result))
                return result;
            return -1;
        }
        #endregion

        #region Public Members
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
        #endregion

        #region ISupportXmlDump Members
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
        #endregion
    }

    public class SwfActionList : List<SwfAction>, ISupportXmlDump
    {
        #region Read & Write
        private const byte ActionsWithData = 0x80;

        public void Read(SwfReader reader)
        {
            while (true)
            {
                byte code = reader.ReadUInt8();
                if (code == 0) break;
                if (code >= ActionsWithData)
                {
                    int len = reader.ReadUInt16();
                    var data = reader.ReadUInt8(len);
                    var action = SwfActionFactory.Create((SwfActionCode)code);
                    if (action == null)
                    {
                        action = new SwfActionUnknown((SwfActionCode)code, data);
                    }
                    else
                    {
                        action.ReadBody(new SwfReader(data));
                    }
                    Add(action);
                }
                else
                {
                    Add(new SwfActionSimple((SwfActionCode)code));
                }
            }
        }

        public void Write(SwfWriter writer)
        {
            foreach (var action in this)
            {
                byte code = (byte)action.ActionCode;
                writer.WriteUInt8(code);
                if (code >= ActionsWithData)
                {
                    var data = action.GetData();
                    writer.WriteUInt16((ushort)data.Length);
                    writer.Write(data);
                }
            }
            if (Count <= 0 || this[Count - 1].ActionCode != SwfActionCode.End)
                writer.WriteUInt8(0);
        }
        #endregion

        #region ISupportXmlDump Members
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("actions");
            foreach (var action in this)
                action.DumpXml(writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}