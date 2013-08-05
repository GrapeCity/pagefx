using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes
{
    /// <summary>
    /// Defines a shape for later use by control tags such as PlaceObject.
    /// </summary>
    [SwfTag(SwfTagCode.DefineShape)]
    public class SwfTagDefineShape : SwfDisplayObject
    {
	    public RectangleF Bounds { get; set; }

	    public SwfStyles Styles
        {
            get { return _styles; }
        }
        private readonly SwfStyles _styles = new SwfStyles();

        public SwfFillStyles FillStyles
        {
            get { return _styles.FillStyles; }
        }

        public SwfLineStyles LineStyles
        {
            get { return _styles.LineStyles; }
        }

        public SwfShape Shape
        {
            get { return _shape; }
        }
        private readonly SwfShape _shape = new SwfShape();

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineShape; }
        }

        private bool _read;

        protected override void ReadBody(SwfReader reader)
        {
            var shapeType = TagCode;
            Bounds = reader.ReadRectF();
            _styles.Read(reader, shapeType);
            _shape.Read(reader, shapeType);
            _read = true;
        }

        protected override void WriteBody(SwfWriter writer)
        {
            var shapeType = TagCode;
            if (!_read)
                Bounds = RectangleF.FromLTRB(_shape.X1, _shape.Y1, _shape.X2, _shape.Y2);
            writer.WriteRectTwip(Bounds);
            _styles.Write(writer, shapeType);
            _shape.Write(writer, shapeType);
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            if (SwfDumpService.DumpShapes)
            {
                writer.WriteElementString("bounds", Bounds.ToString());
                _styles.Dump(writer, TagCode);
                _shape.Dump(writer, TagCode);
            }
        }

	    public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            _styles.ImportDependencies(from, to);
            _shape.ImportDependencies(from, to);
        }

        public override void GetRefs(SwfRefList list)
        {
            _styles.GetRefs(list);
            _shape.GetRefs(list);
        }
    }

    /// <summary>
    /// DefineShape2 extends the capabilities of DefineShape with the ability to support more than
    /// 255 styles in the style list and multiple style lists in a single shape.
    /// </summary>
    [SwfTag(SwfTagCode.DefineShape2)]
    public class SwfTagDefineShape2 : SwfTagDefineShape
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineShape2; }
        }
    }

    /// <summary>
    /// DefineShape3 extends the capabilities of DefineShape2 by extending all of the RGB color
    /// fields to support RGBA with alpha transparency.
    /// </summary>
    [SwfTag(SwfTagCode.DefineShape3)]
    public class SwfTagDefineShape3 : SwfTagDefineShape2
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineShape3; }
        }
    }

    /// <summary>
    /// DefineShape4 extends the capabilities of DefineShape3 by using a new line style record in the shape.
    /// </summary>
    [SwfTag(SwfTagCode.DefineShape4)]
    public class SwfTagDefineShape4 : SwfTagDefineShape3
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineShape4; }
        }
    }
}