using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    /// <summary>
    /// The DefineSceneAndFrameLabelData tag contains scene and frame label data for a
    /// MovieClip. Scenes are supported for the main timeline only, for all other movie clips a single
    /// scene is exported.
    /// </summary>
    [SwfTag(SwfTagCode.DefineSceneAndFrameLabelData)]
    public sealed class SwfTagDefineSceneAndFrameLabelData : SwfTag
    {
	    public SwfSceneList Scenes
        {
            get { return _scenes; }
        }
        private readonly SwfSceneList _scenes = new SwfSceneList();

        public SwfFrameLabelList FrameLabels
        {
            get { return _frameLabels; }
        }
        private readonly SwfFrameLabelList _frameLabels = new SwfFrameLabelList();

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineSceneAndFrameLabelData; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _scenes.Read(reader);
            _frameLabels.Read(reader);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            _scenes.Write(writer);
            _frameLabels.Write(writer);
        }

	    public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            _scenes.DumpXml(writer);
            _frameLabels.DumpXml(writer);
        }
    }

	public sealed class SwfScene : ISwfAtom, ISupportXmlDump
    {
	    public string Name { get; set; }

	    public int FrameOffset { get; set; }

	    public void Read(SwfReader reader)
        {
            FrameOffset = (int)reader.ReadUIntEncoded();
            Name = reader.ReadString();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded(FrameOffset);
            writer.WriteString(Name);
        }

        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("scene");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("frame-offset", FrameOffset.ToString());
            writer.WriteEndElement();
        }
    }

	public sealed class SwfSceneList : SwfAtomList<SwfScene>
    {
        protected override string XmlElementName
        {
            get { return "scenes"; }
        }
    }

	public sealed class SwfFrameLabel : ISwfAtom, ISupportXmlDump
    {
		public int Frame { get; set; }

		public string Label { get; set; }

		public void Read(SwfReader reader)
        {
            Frame = (int)reader.ReadUIntEncoded();
            Label = reader.ReadString();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded(Frame);
            writer.WriteString(Label);
        }

        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("frame-label");
            writer.WriteAttributeString("frame", Frame.ToString());
            writer.WriteAttributeString("label", Label);
            writer.WriteEndElement();
        }
    }

	public sealed class SwfFrameLabelList : SwfAtomList<SwfFrameLabel>
    {
        protected override string XmlElementName
        {
            get { return "frame-labels"; }
        }
    }
}