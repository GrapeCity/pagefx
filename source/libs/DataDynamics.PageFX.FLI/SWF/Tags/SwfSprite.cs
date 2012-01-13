using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Represents SWF sprite.
    /// </summary>
    [SwfTag(SwfTagCode.DefineSprite)]
    public class SwfSprite : SwfCharacter
    {
        /// <summary>
        /// Gets or sets number of frames in sprite
        /// </summary>
        public int FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }
        private int _frameCount;

        /// <summary>
        /// Gets the list of tags contained in this sprite.
        /// </summary>
        public SwfTagList Tags
        {
            get { return _tags; }
        }
        private readonly SwfTagList _tags = new SwfTagList();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineSprite; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            _frameCount = reader.ReadUInt16();
            _tags.Read(reader);
        }

        protected override void WriteBody(SwfWriter writer)
        {
            writer.WriteUInt16((ushort)_frameCount);
            _tags.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            writer.WriteElementString("frame-count", _frameCount.ToString());
            _tags.DumpXml(writer);
        }

        public override void DumpShortBody(XmlWriter writer)
        {
            base.DumpShortBody(writer);
            writer.WriteElementString("frame-count", _frameCount.ToString());
        }

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            foreach (var tag in _tags)
                tag.ImportDependencies(from, to);
        }

        public override void GetRefs(IIDList list)
        {
            foreach (var tag in _tags)
                tag.GetRefs(list);
        }
    }
}