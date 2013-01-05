using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    /// <summary>
    /// Base class for SWF characters.
    /// </summary>
    public abstract class SwfCharacter : SwfTag, ISwfCharacter
    {
    	protected SwfCharacter()
        {
        }

    	protected SwfCharacter(int id)
        {
            CharacterId = checked((ushort)id);
        }

    	public ushort CharacterId { get; set; }

    	public string Name { get; set; }

    	public override sealed void ReadTagData(SwfReader reader)
        {
            CharacterId = reader.ReadUInt16();
            ReadBody(reader);
        }

        protected abstract void ReadBody(SwfReader reader);

        public override sealed void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(CharacterId);
            WriteBody(writer);
        }

        protected abstract void WriteBody(SwfWriter writer);

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteAttributeString("id", CharacterId.ToString());

            if (!string.IsNullOrEmpty(Name))
                writer.WriteAttributeString("name", Name);
        }
    }
}