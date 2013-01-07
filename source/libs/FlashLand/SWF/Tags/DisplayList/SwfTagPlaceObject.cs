using System.Drawing.Drawing2D;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.DisplayList
{
    //NOTE: PlaceObject is rarely used in SWF 3 and later versions; it is superseded by PlaceObject2 and PlaceObject3.

    /// <summary>
    /// This tag will be used to specify where and how to place an object in the next frame.
    /// </summary>
    [SwfTag(SwfTagCode.PlaceObject)]
    public class SwfTagPlaceObject : SwfTag
    {
        #region ctors
        public SwfTagPlaceObject()
        {
        }

        public SwfTagPlaceObject(ushort cid, ushort depth, Matrix matrix)
        {
            _cid = cid;
            _depth = depth;
            _matrix = matrix;
        }

        public SwfTagPlaceObject(ushort cid, ushort depth, float x, float y)
        {
            _cid = cid;
            _depth = depth;
            _matrix = new Matrix();
            _matrix.Translate(x, y);
        }
        #endregion

        #region Properties
        public ushort CharID
        {
            get { return _cid; }
            set { _cid = value; }
        }
        private ushort _cid;

        public ushort Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private ushort _depth;

        public Matrix Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }
        private Matrix _matrix;

        public SwfColorTransform ColorTransform
        {
            get { return _colorTransform; }
            set { _colorTransform = value; }
        }
        private SwfColorTransform _colorTransform;
        #endregion

        #region IO
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.PlaceObject; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _cid = reader.ReadUInt16();
            _depth = reader.ReadUInt16();
            _matrix = reader.ReadMatrix();

            //NOTE:
            //If the size of the PlaceObject tag exceeds the end of the transformation matrix, it is assumed that a
            //ColorTransform field is appended to the record.
            if (reader.Position < reader.Length)
            {
                _colorTransform = reader.ReadColorTransform(false);
            }
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_cid);
            writer.WriteUInt16(_depth);
            writer.WriteMatrix(_matrix);
            if (_colorTransform != null)
                _colorTransform.Write(writer, false);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                writer.WriteElementString("cid", _cid.ToString());
                writer.WriteElementString("depth", _depth.ToString());
                writer.WriteElementString("matrix", _matrix.GetMatrixString());
                if (_colorTransform != null)
                    _colorTransform.Dump(writer, false);
            }
        }
        #endregion

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            to.ImportCharacter(from, ref _cid);
        }

        public override void GetRefs(SwfRefList list)
        {
            list.Add(_cid);
        }
    }
}