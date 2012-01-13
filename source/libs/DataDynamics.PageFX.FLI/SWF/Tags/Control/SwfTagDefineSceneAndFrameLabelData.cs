using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// The DefineSceneAndFrameLabelData tag contains scene and frame label data for a
    /// MovieClip. Scenes are supported for the main timeline only, for all other movie clips a single
    /// scene is exported.
    /// </summary>
    [SwfTag(SwfTagCode.DefineSceneAndFrameLabelData)]
    public class SwfTagDefineSceneAndFrameLabelData : SwfTag
    {
        #region Properties
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
        #endregion

        #region IO
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
        #endregion

        #region XmlDump
        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            _scenes.DumpXml(writer);
            _frameLabels.DumpXml(writer);
        }
        #endregion
    }

    #region class SwfScene
    public class SwfScene : ISwfAtom, ISupportXmlDump
    {
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        public int FrameOffset
        {
            get { return _frameOffset; }
            set { _frameOffset = value; }
        }
        private int _frameOffset;

        public void Read(SwfReader reader)
        {
            _frameOffset = (int)reader.ReadUIntEncoded();
            _name = reader.ReadString();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded(_frameOffset);
            writer.WriteString(_name);
        }

        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("scene");
            writer.WriteAttributeString("name", _name);
            writer.WriteAttributeString("frame-offset", _frameOffset.ToString());
            writer.WriteEndElement();
        }
    }
    #endregion

    #region class SwfSceneList
    public class SwfSceneList : SwfAtomList<SwfScene>
    {
        protected override string XmlElementName
        {
            get { return "scenes"; }
        }
    }
    #endregion

    #region class SwfFrameLabel
    public class SwfFrameLabel : ISwfAtom, ISupportXmlDump
    {
        public int Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }
        private int _frame;

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private string _label;

        public void Read(SwfReader reader)
        {
            _frame = (int)reader.ReadUIntEncoded();
            _label = reader.ReadString();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded(_frame);
            writer.WriteString(_label);
        }

        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("frame-label");
            writer.WriteAttributeString("frame", _frame.ToString());
            writer.WriteAttributeString("label", _label);
            writer.WriteEndElement();
        }
    }
    #endregion

    #region class SwfFrameLabelList
    public class SwfFrameLabelList : SwfAtomList<SwfFrameLabel>
    {
        protected override string XmlElementName
        {
            get { return "frame-labels"; }
        }
    }
    #endregion
}