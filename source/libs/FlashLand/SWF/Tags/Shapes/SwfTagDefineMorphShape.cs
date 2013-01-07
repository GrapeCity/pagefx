using System;
using System.Drawing;
using System.Xml;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes
{
    /// <summary>
    /// The DefineMorphShape tag defines the start and end states of a morph sequence.
    /// </summary>
    [TODO]
    [SwfTag(SwfTagCode.DefineMorphShape)]
    public class SwfTagDefineMorphShape : SwfCharacter
    {
        #region Properties
        public RectangleF StartBounds
        {
            get { return _startBounds; }
            set { _startBounds = value; }
        }
        private RectangleF _startBounds;

        public RectangleF EndBounds
        {
            get { return _endBounds; }
            set { _endBounds = value; }
        }
        private RectangleF _endBounds;

        public SwfStyles Styles
        {
            get { return _styles; }
        }
        private readonly SwfStyles _styles = new SwfStyles();

        public SwfShape Shape
        {
            get { return _shape; }
        }
        private readonly SwfShape _shape = new SwfShape();
        #endregion

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineMorphShape; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            _startBounds = reader.ReadRectF();
            _endBounds = reader.ReadRectF();
        }

        protected override void WriteBody(SwfWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            if (SwfDumpService.DumpShapes)
            {
                //TODO: Dump tag body
            }
        }

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            throw new NotImplementedException();
        }

        public override void GetRefs(SwfRefList list)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The DefineMorphShape2 tag extends the capabilities of DefineMorphShape by using a new morph line style record in the morph shape.
    /// </summary>
    [SwfTag(SwfTagCode.DefineMorphShape2)]
    public class SwfTagDefineMorphShape2 : SwfTagDefineMorphShape
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineMorphShape2; }
        }
    }
}