using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf
{
    public class SwfAtomList<T> : List<T>, ISwfAtom, ISupportXmlDump
        where T:ISwfAtom, ISupportXmlDump,new()
    {
	    public virtual void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
                ReadItem(reader);
        }

        protected virtual void ReadItem(SwfReader reader)
        {
            var item = new T();
            item.Read(reader);
            Add(item);
        }

        public virtual void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }

	    protected virtual string XmlElementName
        {
            get { return GetType().Name; }
        }

        public virtual void DumpXml(XmlWriter writer)
        {
            int n = Count;
            if (n > 0)
            {
                string name = XmlElementName;
                if (!string.IsNullOrEmpty(name))
                {
                    writer.WriteStartElement(name);
                    writer.WriteAttributeString("count", n.ToString());
                }

                for (int i = 0; i < n; ++i)
                    this[i].DumpXml(writer);

                if (!string.IsNullOrEmpty(name))
                    writer.WriteEndElement();
            }
        }
    }
}