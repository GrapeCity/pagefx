using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// The DefineBinaryData tag is used to save any arbitrary user defined binary data in an SWF movie. The Flash player itself ignores that data. The size of the data is not specifically limited.
    /// </summary>
    [SwfTag(SwfTagCode.DefineBinaryData)]
    public class SwfTagDefineBinaryData : SwfCharacter
    {
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private byte[] _data;

        public override int GetDataSize()
        {
            return _data != null ? 6 + _data.Length : 6;
        }

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineBinaryData; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            reader.ReadUInt32(); //reserved
            _data = reader.ReadToEnd();
        }

        protected override void WriteBody(SwfWriter writer)
        {
            writer.WriteUInt32(0); //reserved
            writer.Write(_data);
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            if (_data != null)
                writer.WriteElementString("datasize", _data.Length.ToString());
        }
    }
}