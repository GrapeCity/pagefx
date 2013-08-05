using System.Drawing.Drawing2D;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.DisplayList
{
    //NOTE: PlaceObject is rarely used in SWF 3 and later versions; it is superseded by PlaceObject2 and PlaceObject3.

    /// <summary>
    /// This tag will be used to specify where and how to place an object in the next frame.
    /// </summary>
    [SwfTag(SwfTagCode.PlaceObject)]
    public sealed class SwfTagPlaceObject : SwfTag
    {
	    public SwfTagPlaceObject()
        {
        }

        public SwfTagPlaceObject(ushort cid, ushort depth, Matrix matrix)
        {
            CharId = cid;
            Depth = depth;
            Matrix = matrix;
        }

        public SwfTagPlaceObject(ushort cid, ushort depth, float x, float y)
        {
            CharId = cid;
            Depth = depth;
            Matrix = new Matrix();
            Matrix.Translate(x, y);
        }

	    public ushort CharId { get; set; }

	    public ushort Depth { get; set; }

	    public Matrix Matrix { get; set; }

	    public SwfColorTransform ColorTransform { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.PlaceObject; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            CharId = reader.ReadUInt16();
            Depth = reader.ReadUInt16();
            Matrix = reader.ReadMatrix();

            //NOTE:
            //If the size of the PlaceObject tag exceeds the end of the transformation matrix, it is assumed that a
            //ColorTransform field is appended to the record.
            if (reader.Position < reader.Length)
            {
                ColorTransform = reader.ReadColorTransform(false);
            }
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(CharId);
            writer.WriteUInt16(Depth);
            writer.WriteMatrix(Matrix);
            if (ColorTransform != null)
                ColorTransform.Write(writer, false);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                writer.WriteElementString("cid", CharId.ToString());
                writer.WriteElementString("depth", Depth.ToString());
                writer.WriteElementString("matrix", Matrix.GetMatrixString());
                if (ColorTransform != null)
                    ColorTransform.Dump(writer, false);
            }
        }

	    public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
	        ushort cid = CharId;
            to.ImportCharacter(from, ref cid);
	        CharId = cid;
        }

        public override void GetRefs(SwfRefList list)
        {
            list.Add(CharId);
        }
    }
}