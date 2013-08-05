using System.Drawing.Drawing2D;
using System.Xml;
using DataDynamics.PageFX.Flash.Swf.Filters;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Buttons
{
    public sealed class SwfButton
    {
	    public SwfButton()
        {
        }

        public SwfButton(SwfButtonState state)
        {
            State = state;
        }

	    public SwfButtonState State { get; set; }

	    /// <summary>
	    /// Gets or sets id of character to place
	    /// </summary>
	    public ushort CharId { get; set; }

	    public ushort PlaceDepth { get; set; }

	    public Matrix PlaceMatrix { get; set; }

	    public SwfColorTransform ColorTransform { get; set; }

	    public SwfFilterList Filters
        {
            get { return _filters; }
        }
        private readonly SwfFilterList _filters = new SwfFilterList();

	    public SwfBlendMode BlendMode { get; set; }

	    private static bool HasAlpha(SwfTagCode tagCode)
        {
            return tagCode == SwfTagCode.DefineButton2;
        }

        public void Read(SwfReader reader, SwfTagCode tagCode)
        {
            //state is already read
            CharId = reader.ReadUInt16();
            PlaceDepth = reader.ReadUInt16();
            PlaceMatrix = reader.ReadMatrix();
            ColorTransform = reader.ReadColorTransform(HasAlpha(tagCode));
            if ((State & SwfButtonState.HasFilterList) != 0)
                _filters.Read(reader);
            if ((State & SwfButtonState.HasBlendMode) != 0)
                BlendMode = (SwfBlendMode)reader.ReadUInt8();
        }

        public void Write(SwfWriter writer, SwfTagCode tagCode)
        {
            writer.WriteUInt8((byte)State);
            writer.WriteUInt16(CharId);
            writer.WriteUInt16(PlaceDepth);
            writer.WriteMatrix(PlaceMatrix);
            ColorTransform.Write(writer, HasAlpha(tagCode));
            if ((State & SwfButtonState.HasFilterList) != 0)
                _filters.Write(writer);
            if ((State & SwfButtonState.HasBlendMode) != 0)
                writer.WriteUInt8((byte)BlendMode);
        }

        public void Dump(XmlWriter writer, SwfTagCode tagCode)
        {
            writer.WriteStartElement("button");
            writer.WriteAttributeString("id", CharId.ToString());
            writer.WriteAttributeString("depth", PlaceDepth.ToString());
            writer.WriteElementString("matrix", PlaceMatrix.GetMatrixString());
            ColorTransform.Dump(writer, HasAlpha(tagCode));
            if (_filters.Count > 0)
                _filters.Dump(writer);
            if ((State & SwfButtonState.HasBlendMode) != 0)
                writer.WriteElementString("blend-mode", BlendMode.ToString());
            writer.WriteEndElement();
        }
    }
}