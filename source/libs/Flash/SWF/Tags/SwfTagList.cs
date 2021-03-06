using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Flash.Swf.Tags
{
    public sealed class SwfTagList : IReadOnlyList<SwfTag>, ISupportXmlDump
    {
		private readonly List<SwfTag> _list = new List<SwfTag>();
	    private SwfMovie _swf;

	    internal SwfTagList(SwfMovie swf)
		{
			_swf = swf;
		}

	    public int Count
	    {
			get { return _list.Count; }
	    }

	    public SwfTag this[int index]
	    {
		    get { return _list[index]; }
	    }

	    public SwfMovie Swf
        {
            get { return _swf; }
            set
            {
				if (_swf == value) return;

	            _swf = value;

                foreach (var tag in this)
                    tag.Swf = value;
            }
        }

		public void Add(SwfTag tag)
		{
			if (_swf != null)
			{
				tag.Swf = _swf;
			}

			tag.Index = Count;

			_list.Add(tag);
		}

		public void RemoveAt(int index)
		{
			//TODO: update SwfTag.Index for each tag

			_list.RemoveAt(index);
		}

	    #region Read
        public void Read(SwfReader reader)
        {
            while (ReadTag(reader))
            {
            }
        }

        bool ReadTag(SwfReader reader)
        {
            ushort tagHeader = reader.ReadUInt16();
            ushort tagCode = (ushort)(tagHeader >> 6);
            if (tagCode == 0) return false;

            int length = (tagHeader & 0x3f);
            // Is this a long data block?
            if (length == 0x3f)
                length = reader.ReadInt32();

            var tag = DecodeTag(reader, length, (SwfTagCode)tagCode);
            Add(tag);
            return true;
        }

        public void Read(SwfReader reader, int n)
        {
            for (int i = 0; i < n; ++i)
                ReadTag(reader);
        }

        private static SwfTag DecodeTag(SwfReader reader, int length, SwfTagCode tagCode)
        {
        	if (length == 0)
                return new SwfTagEmpty(tagCode);

            var opts = reader.TagDecodeOptions;
            var tagData = reader.ReadUInt8(length);

            if (tagCode == SwfTagCode.DefineSprite 
                && (opts & SwfTagDecodeOptions.DonotDecodeSprites) == 0)
            {
                var sprite = new SwfSprite();
                sprite.ReadTagData(tagData, reader);
                return sprite;
            }

            if ((opts & SwfTagDecodeOptions.DonotDecodeTags) != 0)
                return new SwfTagAny(tagCode, tagData);

            bool isChar = SwfTag.IsCharacter(tagCode);
            if (isChar && (opts & SwfTagDecodeOptions.DonotDecodeCharacters) != 0)
                return ReadChar(reader, tagCode, tagData);

            bool isImage = SwfTag.IsImage(tagCode);
            if (isImage && (opts & SwfTagDecodeOptions.DonotDecodeImages) != 0)
                return ReadChar(reader, tagCode, tagData);

            var tag = SwfTagFactory.Create(tagCode);
            if (tag != null)
            {
                tag.ReadTagData(tagData, reader);
                return tag;
            }

            if (isChar)
                return ReadChar(reader, tagCode, tagData);

            return new SwfTagAny(tagCode, tagData);
        }

        private static SwfTag ReadChar(SwfReader reader, SwfTagCode tagCode, byte[] tagData)
        {
            var ch = new SwfCharacterAny(tagCode);
            ch.ReadTagData(tagData, reader);
            return ch;
        }
        #endregion

        #region Write
        public void Write(SwfWriter writer, bool addEnd)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var tag = this[i];
                var data = tag.GetData();
                writer.WriteTag((int)tag.TagCode, data);
            }

            if (addEnd)
            {
                SwfTag lastTag = null;
                if (n > 0)
                    lastTag = this[n - 1];
                if (lastTag == null || lastTag.TagCode != SwfTagCode.End)
                    writer.WriteTagHeader((int)SwfTagCode.End, 0);
            }
        }

        public void Write(SwfWriter writer)
        {
            Write(writer, true);
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            Dump(writer, true);
        }

        public void Dump(XmlWriter writer, bool fullbody)
        {
            writer.WriteStartElement("tags");
            writer.WriteAttributeString("count", Count.ToString());
            foreach (var tag in this)
            {
                tag.Dump(writer, fullbody);
            }
            writer.WriteEndElement();
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Adds empty tag with specified code.
        /// </summary>
        /// <param name="code"></param>
        public void Add(SwfTagCode code)
        {
            Add(new SwfTagEmpty(code));
        }

        /// <summary>
        /// Adds ShowFrame tag.
        /// </summary>
        public void ShowFrame()
        {
            Add(SwfTagCode.ShowFrame);
        }

        public ISwfCharacter GetCharacter(int id)
        {
			//TODO: optimize searching characters
			return _list.OfType<ISwfCharacter>().FirstOrDefault(x => x.CharacterId == id);
        }

        #endregion

	    public IEnumerator<SwfTag> GetEnumerator()
	    {
		    return _list.GetEnumerator();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }
    }
}