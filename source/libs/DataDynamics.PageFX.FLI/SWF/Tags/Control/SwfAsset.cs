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
            _name = name;
        }

        public SwfAsset(ISwfCharacter obj, string name)
        {
            _obj = obj;
            _id = obj.CharacterID;
            _name = name;
        }

        public SwfAsset(SwfReader reader)
        {
            Read(reader);
        }
        #endregion

        #region Properties
        public ushort ID
        {
            get
            {
                if (_obj != null)
                    return _obj.CharacterID;
                return _id;
            }
            set { _id = value; }
        }
        private ushort _id;

        public ISwfCharacter Character
        {
            get { return _obj; }
            set { _obj = value; }
        }
        private ISwfCharacter _obj;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        public SwfAssetFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private SwfAssetFlags _flags;

        public bool IsExported
        {
            get { return (_flags & SwfAssetFlags.Exported) != 0; }
            set
            {
                if (value) _flags |= SwfAssetFlags.Exported;
                else _flags &= ~SwfAssetFlags.Exported;
            }
        }

        public bool IsImported
        {
            get { return (_flags & SwfAssetFlags.Imported) != 0; }
            set
            {
                if (value) _flags |= SwfAssetFlags.Imported;
                else _flags &= ~SwfAssetFlags.Imported;
            }
        }

        public bool IsSymbol
        {
            get { return (_flags & SwfAssetFlags.Symbol) != 0; }
            set
            {
                if (value) _flags |= SwfAssetFlags.Symbol;
                else _flags &= ~SwfAssetFlags.Symbol;
            }
        }
        #endregion

        #region IO
        public void Read(SwfReader reader)
        {
            _id = reader.ReadUInt16();
            _name = reader.ReadString();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt16(ID);
            writer.WriteString(_name);
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("id", _id.ToString());
            writer.WriteAttributeString("name", _name);
            writer.WriteEndElement();
        }

        public void DumpXml(XmlWriter writer)
        {
            DumpXml(writer, "asset");
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0} - {1}", ID, _name);
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