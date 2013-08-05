using System;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    public sealed class SwfAsset : ISupportXmlDump
    {
	    public SwfAsset()
        {
        }

        public SwfAsset(ushort id, string name)
        {
            _id = id;
            Name = name;
        }

        public SwfAsset(ISwfCharacter obj, string name)
        {
            Character = obj;
            _id = obj.CharacterId;
            Name = name;
        }

        public SwfAsset(SwfReader reader)
        {
            Read(reader);
        }

	    public ushort Id
        {
            get
            {
                if (Character != null)
                    return Character.CharacterId;
                return _id;
            }
            set { _id = value; }
        }
        private ushort _id;

    	public ISwfCharacter Character { get; set; }

    	public string Name { get; set; }

    	public SwfAssetFlags Flags { get; set; }

    	public bool IsExported
        {
            get { return (Flags & SwfAssetFlags.Exported) != 0; }
            set
            {
                if (value) Flags |= SwfAssetFlags.Exported;
                else Flags &= ~SwfAssetFlags.Exported;
            }
        }

        public bool IsImported
        {
            get { return (Flags & SwfAssetFlags.Imported) != 0; }
            set
            {
                if (value) Flags |= SwfAssetFlags.Imported;
                else Flags &= ~SwfAssetFlags.Imported;
            }
        }

        public bool IsSymbol
        {
            get { return (Flags & SwfAssetFlags.Symbol) != 0; }
            set
            {
                if (value) Flags |= SwfAssetFlags.Symbol;
                else Flags &= ~SwfAssetFlags.Symbol;
            }
        }

	    public void Read(SwfReader reader)
        {
            _id = reader.ReadUInt16();
            Name = reader.ReadString();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt16(Id);
            writer.WriteString(Name);
        }

	    public void DumpXml(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("id", _id.ToString());
            writer.WriteAttributeString("name", Name);
            writer.WriteEndElement();
        }

        public void DumpXml(XmlWriter writer)
        {
            DumpXml(writer, "asset");
        }

	    public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Name);
        }
    }

	[Flags]
    public enum SwfAssetFlags
    {
        None = 0,
        Exported = 1,
        Imported = 2,
        Symbol = 4,
    }
}