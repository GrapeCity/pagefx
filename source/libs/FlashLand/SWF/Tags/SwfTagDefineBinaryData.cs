using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    /// <summary>
    /// The DefineBinaryData tag is used to save any arbitrary user defined binary data in an SWF movie. The Flash player itself ignores that data. The size of the data is not specifically limited.
    /// </summary>
    [SwfTag(SwfTagCode.DefineBinaryData)]
    public sealed class SwfTagDefineBinaryData : SwfCharacter
    {
	    public byte[] Data { get; set; }

	    public override int GetDataSize()
        {
            return Data != null ? 6 + Data.Length : 6;
        }

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineBinaryData; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            reader.ReadUInt32(); //reserved
            Data = reader.ReadToEnd();
        }

        protected override void WriteBody(SwfWriter writer)
        {
            writer.WriteUInt32(0); //reserved
            writer.Write(Data);
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            if (Data != null)
                writer.WriteElementString("datasize", Data.Length.ToString());
        }
    }
}