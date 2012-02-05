using System;
using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    public class SwfAsset : ISupportXmlDump
    {
        #region ctors
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
            _id = obj.CharacterID;
            Name = name;
        }

        public SwfAsset(SwfReader reader)
        {
            Read(reader);
        }
        #endregion

        #region Properties
        public ushort Id
        {
            get
            {
                if (Character != null)
                    return Character.CharacterID;
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
        #endregion

        #region IO
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
        #endregion

        #region Dump
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
        #endregion

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Name);
        }
    }

    public class SwfAssetCollection : List<SwfAsset>, ISupportXmlDump
    {
        public void Add(ISwfCharacter obj, string name)
        {
            Add(new SwfAsset(obj, name));
        }

        public void Add(ushort id, string name)
        {
            Add(new SwfAsset(id, name));
        }

        #region IO
        public void Read(SwfReader reader, SwfAssetFlags flags)
        {
            int n = reader.ReadUInt16();
            for (int i = 0; i < n; ++i)
            {
                var asset = new SwfAsset(reader);
                asset.Flags = flags;
                Add(asset);
            }
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt16((ushort)Count);
            foreach (var obj in this)
                obj.Write(writer);
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer, string name, string assetName)
        {
            writer.WriteStartElement(name);
            foreach (var obj in this)
                obj.DumpXml(writer, assetName);
            writer.WriteEndElement();
        }

        public void DumpXml(XmlWriter writer)
        {
            DumpXml(writer, "assets", "asset");
        }
        #endregion
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