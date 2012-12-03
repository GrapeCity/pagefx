using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes
{
    /// <summary>
    /// Fill and Line styles together.
    /// </summary>
    public class SwfStyles
    {
        #region Properties
        public SwfFillStyles FillStyles
        {
            get { return _fillStyles; }
        }
        private readonly SwfFillStyles _fillStyles = new SwfFillStyles();

        public SwfLineStyles LineStyles
        {
            get { return _lineStyles; }
        }
        private readonly SwfLineStyles _lineStyles = new SwfLineStyles();

        public int FillBits
        {
            get { return _fillBits; }
        }
        private int _fillBits;

        public int LineBits
        {
            get { return _lineBits; }
        }
        private int _lineBits;
        #endregion

        #region IO
        private bool _read;

        public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            reader.Align();
            _fillStyles.Read(reader, shapeType);
            _lineStyles.Read(reader, shapeType);
            _fillBits = (int)reader.ReadUB(4);
            _lineBits = (int)reader.ReadUB(4);
            _read = true;
            reader.Styles = this;
        }

        public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.Align();
            if (!_read)
            {
                _fillBits = ((uint)_fillStyles.Count).GetMinBits();
                _lineBits = ((uint)_lineStyles.Count).GetMinBits();
            }
            _fillStyles.Write(writer, shapeType);
            _lineStyles.Write(writer, shapeType);
            writer.WriteUB((uint)_fillBits, 4);
            writer.WriteUB((uint)_lineBits, 4);
            writer.Styles = this;
        }

        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("styles");
            writer.WriteAttributeString("fill-bits", _fillBits.ToString());
            writer.WriteAttributeString("line-bits", _lineBits.ToString());
            _fillStyles.Dump(writer, shapeType);
            _lineStyles.Dump(writer, shapeType);
            writer.WriteEndElement();
        }
        #endregion

        public void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            _fillStyles.ImportDependencies(from, to);
            _lineStyles.ImportDependencies(from, to);
        }

        public void GetRefs(IIDList list)
        {
            _fillStyles.GetRefs(list);
            _lineStyles.GetRefs(list);
        }
    }
}