using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.DefineScalingGrid)]
    public class SwfTagDefineScalingGrid : SwfTag
    {
        /// <summary>
        /// ID of sprite or button character upon which the scaling grid will be applied. 
        /// </summary>
        public ushort ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private ushort _id;

        /// <summary>
        /// Center region of 9-slice grid
        /// </summary>
        public RectangleF Splitter
        {
            get { return _splitter; }
            set { _splitter = value; }
        }
        private RectangleF _splitter;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineScalingGrid; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _id = reader.ReadUInt16();
            _splitter = reader.ReadRectF();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_id);
            writer.WriteRectTwip(_splitter);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("idref", _id.ToString());
            writer.WriteElementString("splitter", _splitter.ToString());
        }
    }
}