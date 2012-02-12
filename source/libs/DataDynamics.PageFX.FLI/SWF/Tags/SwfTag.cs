using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    public abstract class SwfTag : ISwfTag, ICloneable, ISupportXmlDump
    {
        #region Shared Members
        private static Dictionary<SwfTagCode, int> _mapver;

        /// <summary>
        /// Returns minimum SWF version where tag with specified code was defined.
        /// </summary>
        /// <param name="code">tag code</param>
        /// <returns></returns>
        public static int GetTagVersion(SwfTagCode code)
        {
            if (_mapver == null)
            {
                _mapver = SwfHelper.GetEnumAttributeMap<SwfTagCode, int, SwfVersionAttribute>(attr => attr.Version);
            }
            int result;
        	return _mapver.TryGetValue(code, out result) ? result : -1;
        }

        private static Dictionary<SwfTagCode, SwfTagCategory> _mapcat;

        public static SwfTagCategory GetTagCategory(SwfTagCode code)
        {
            if (_mapcat == null)
            {
                _mapcat = SwfHelper.GetEnumAttributeMap<SwfTagCode, SwfTagCategory, SwfTagCategoryAttribute>(attr => attr.Category);
            }
            SwfTagCategory result;
            return _mapcat.TryGetValue(code, out result) ? result : SwfTagCategory.Unknown;
        }

        public static bool IsCharacter(SwfTagCode code)
        {
            return GetTagCategory(code) == SwfTagCategory.Define;
        }

        public static bool IsImage(SwfTagCode code)
        {
            switch (code)
            {
                case SwfTagCode.DefineBitsJPEG:
                case SwfTagCode.DefineBitsJPEG2:
                case SwfTagCode.DefineBitsJPEG3:
                case SwfTagCode.DefineBitsLossless:
                case SwfTagCode.DefineBitsLossless2:
                    return true;
            }
            return false;
        }

        public static bool IsShape(SwfTagCode code)
        {
            switch (code)
            {
                case SwfTagCode.DefineShape:
                case SwfTagCode.DefineShape2:
                case SwfTagCode.DefineShape3:
                case SwfTagCode.DefineShape4:
                    return true;
            }
            return false;
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Gets the tag code.
        /// </summary>
        public abstract SwfTagCode TagCode { get; }

        public int Index { get; set; }

        public bool Imported { get; set; }

        /// <summary>
        /// Gets the tag version.
        /// </summary>
        public int TagVersion
        {
            get { return GetTagVersion(TagCode); }
        }

        public virtual SwfTag Clone()
        {
            var tc = TagCode;
            var data = GetData();
            var res = SwfTagFactory.Create(tc);
            if (res != null)
            {
                using (var reader = new SwfReader(data))
                    res.ReadTagData(reader);
                return res;
            }
            if (IsCharacter(tc))
            {
                using (var reader = new SwfReader(data))
                {
                    var c = new SwfCharacterAny(tc);
                    c.ReadTagData(reader);
                    return c;
                }
            }
            return new SwfTagAny(tc, data);
        }

        public SwfMovie Swf
        {
            get { return _swf; }
            set
            {
                _swf = value;
                var sprite = this as SwfSprite;
                if (sprite != null)
                {
                    sprite.Tags.Swf = value;
                }
            }
        }
        private SwfMovie _swf;
        #endregion

        #region IO
        public void ReadTagData(byte[] data, SwfReader parent)
        {
            using (var tagReader = new SwfReader(data, parent))
                ReadTagData(tagReader);
        }

        public void ReadTagData(byte[] data)
        {
            using (var tagReader = new SwfReader(data))
                ReadTagData(tagReader);
        }

        /// <summary>
        /// Reads tag data.
        /// </summary>
        /// <param name="reader"><see cref="SwfReader"/> used to read tag data.</param>
        public abstract void ReadTagData(SwfReader reader);

        /// <summary>
        /// Writes tag data.
        /// </summary>
        /// <param name="writer"><see cref="SwfWriter"/> used to write tag data.</param>
        public abstract void WriteTagData(SwfWriter writer);

        /// <summary>
        /// Gets tag data.
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GetData()
        {
            var body = new SwfWriter();
            WriteTagData(body);
            return body.ToByteArray();
        }

        /// <summary>
        /// Gets tag size.
        /// </summary>
        /// <returns></returns>
        public virtual int GetDataSize()
        {
            var data = GetData();
            if (data != null)
                return data.Length;
            return 0;
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            Dump(writer, true);
        }

        public void Dump(XmlWriter writer, bool fullbody)
        {
            writer.WriteStartElement("tag");
            var code = TagCode;
            writer.WriteAttributeString("code", code.ToString());

            if (SwfDumpService.Verbose)
            {
                writer.WriteAttributeString("codex", string.Format("{0} - 0x{0:X2}", (int)code));
                writer.WriteAttributeString("size", GetDataSize().ToString());
                writer.WriteAttributeString("version", TagVersion.ToString());
            }

            if (fullbody) DumpBody(writer);
            else DumpShortBody(writer);

            if (SwfDumpService.DumpRefs)
                DumpRefs(writer);

            writer.WriteEndElement();
        }

        private void DumpRefs(XmlWriter writer)
        {
            var refs = GetRefs();
            if (refs != null && refs.Length > 0)
            {
                writer.WriteStartElement("refs");
                foreach (int id in refs)
                	DumpRef(writer, id);
            	writer.WriteEndElement();
            }
        }

        private void DumpRef(XmlWriter writer, int id)
        {
            writer.WriteStartElement("ref");
            writer.WriteAttributeString("id", id.ToString());

            if (_swf != null)
            {
                var rc = _swf.GetCharacter(id);
                if (rc != null)
                {
                    if (!string.IsNullOrEmpty(rc.Name))
                        writer.WriteAttributeString("name", rc.Name);
                    writer.WriteAttributeString("code", rc.TagCode.ToString());
                }
            }

            writer.WriteEndElement();
        }

        public virtual void DumpShortBody(XmlWriter writer)
        {
            DumpBody(writer);
        }

        public virtual void DumpBody(XmlWriter writer)
        {
        }
        #endregion

        #region Import
        public virtual void ImportDependencies(SwfMovie from, SwfMovie to)
        {
        }

        internal SwfTag ImportedTag { get; set; }
        #endregion

        #region Refs
        public int[] GetRefs()
        {
            var list = new IDList();
            GetRefs(list);
            return list.ToArray();
        }

        public virtual void GetRefs(IIDList list)
        {
        }
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            var sb = new StringBuilder();
            var tc = TagCode;
            sb.Append(tc.ToString());
            if (IsCharacter(tc))
            {
                var c = this as ISwfCharacter;
                if (c != null)
                {
                    sb.AppendFormat("({0})", c.CharacterID);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region ICloneable Members
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion
    }
}