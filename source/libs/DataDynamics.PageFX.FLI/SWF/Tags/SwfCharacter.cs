using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Base class for SWF characters.
    /// </summary>
    public abstract class SwfCharacter : SwfTag, ISwfCharacter
    {
        #region ctors
        public SwfCharacter()
        {
        }

        public SwfCharacter(int id)
        {
            _id = checked((ushort)id);
        }
        #endregion

        #region ISwfCharacter Members
        public ushort CharacterID
        {
            get { return _id; }
            set { _id = value; }
        }
        private ushort _id;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;
        #endregion

        public override sealed void ReadTagData(SwfReader reader)
        {
            _id = reader.ReadUInt16();
            ReadBody(reader);
        }

        protected abstract void ReadBody(SwfReader reader);

        public override sealed void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_id);
            WriteBody(writer);
        }

        protected abstract void WriteBody(SwfWriter writer);

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteAttributeString("id", _id.ToString());

            if (!string.IsNullOrEmpty(_name))
                writer.WriteAttributeString("name", _name);
        }
    }
}