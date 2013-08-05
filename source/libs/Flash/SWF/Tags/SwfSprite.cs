using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags
{
    /// <summary>
    /// Represents SWF sprite.
    /// </summary>
    [SwfTag(SwfTagCode.DefineSprite)]
    public sealed class SwfSprite : SwfCharacter
    {
		public override SwfMovie Swf
		{
			get
			{
				return base.Swf;
			}
			set
			{
				base.Swf = value;

				_tags.Swf = value;
			}
		}

	    /// <summary>
	    /// Gets or sets number of frames in sprite
	    /// </summary>
	    public int FrameCount { get; set; }

	    /// <summary>
        /// Gets the list of tags contained in this sprite.
        /// </summary>
        public SwfTagList Tags
        {
            get { return _tags; }
        }
        private readonly SwfTagList _tags = new SwfTagList(null);

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineSprite; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            FrameCount = reader.ReadUInt16();
            _tags.Read(reader);
        }

        protected override void WriteBody(SwfWriter writer)
        {
            writer.WriteUInt16((ushort)FrameCount);
            _tags.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            writer.WriteElementString("frame-count", FrameCount.ToString());
            _tags.DumpXml(writer);
        }

        public override void DumpShortBody(XmlWriter writer)
        {
            base.DumpShortBody(writer);
            writer.WriteElementString("frame-count", FrameCount.ToString());
        }

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            foreach (var tag in _tags)
                tag.ImportDependencies(from, to);
        }

        public override void GetRefs(SwfRefList list)
        {
            foreach (var tag in _tags)
                tag.GetRefs(list);
        }
    }
}