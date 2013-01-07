using System;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.DisplayList
{
    [SwfTag(SwfTagCode.PlaceObject2)]
    public class SwfTagPlaceObject2 : SwfTag
    {
        #region ctors
        public SwfTagPlaceObject2()
        {
        }

        public SwfTagPlaceObject2(ISwfDisplayObject obj, Matrix matrix, SwfPlaceMode mode)
        {
            _depth = obj.Depth;
            _cid = obj.CharacterId;
            _matrix = matrix;

            if (mode == SwfPlaceMode.Modify || mode == SwfPlaceMode.Renew)
                _flags |= SwfPlaceFlags.HasMove;

            if (mode != SwfPlaceMode.Modify)
                _flags |= SwfPlaceFlags.HasCharacter;

            if (matrix != null)
                _flags |= SwfPlaceFlags.HasMatrix;
        }
        #endregion

        #region Properties
        public SwfPlaceFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private SwfPlaceFlags _flags;

        public bool HasCharacter
        {
            get { return (_flags & SwfPlaceFlags.HasCharacter) != 0; }
        }

        public ushort Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private ushort _depth;

        public ushort CharID
        {
            get { return _cid; }
            set
            {
                _cid = value;
                _flags |= SwfPlaceFlags.HasCharacter;
            }
        }
        private ushort _cid;

        public Matrix Matrix
        {
            get { return _matrix; }
            set
            {
                _matrix = value;
                if (_matrix != null)
                    _flags |= SwfPlaceFlags.HasMatrix;
                else
                    _flags &= ~SwfPlaceFlags.HasMatrix;
            }
        }
        private Matrix _matrix;

        public SwfColorTransform ColorTransform
        {
            get { return _colorTransform; }
            set { _colorTransform = value; }
        }
        private SwfColorTransform _colorTransform;

        public ushort Ratio
        {
            get { return _ratio; }
            set { _ratio = value; }
        }
        private ushort _ratio;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        public ushort ClipDepth
        {
            get { return _clipDepth; }
            set
            {
                _clipDepth = value;
                _flags |= SwfPlaceFlags.HasClipDepth;
            }
        }
        private ushort _clipDepth;

        public SwfEventList ClipActions
        {
            get { return _clipActions; }
        }
        private readonly SwfEventList _clipActions = new SwfEventList();
        #endregion

        #region IO
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.PlaceObject2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            var tc = TagCode;
            if (tc == SwfTagCode.PlaceObject2)
            {
                _flags = (SwfPlaceFlags)reader.ReadUInt8();
            }
            else
            {
                _flags = (SwfPlaceFlags)reader.ReadUInt8();
                _flags |= (SwfPlaceFlags)(reader.ReadUInt8() << 8);
            }

            _depth = reader.ReadUInt16();

            if ((_flags & SwfPlaceFlags.HasCharacter) != 0)
                _cid = reader.ReadUInt16();
            if ((_flags & SwfPlaceFlags.HasMatrix) != 0)
                _matrix = reader.ReadMatrix();
            if ((_flags & SwfPlaceFlags.HasColorTransform) != 0)
                _colorTransform = reader.ReadColorTransform(true);
            if ((_flags & SwfPlaceFlags.HasRatio) != 0)
                _ratio = reader.ReadUInt16();
            if ((_flags & SwfPlaceFlags.HasName) != 0)
                _name = reader.ReadString();
            if ((_flags & SwfPlaceFlags.HasClipDepth) != 0)
                _clipDepth = reader.ReadUInt16();

            ReadBeforeClipActions(reader);

            if ((_flags & SwfPlaceFlags.HasClipActions) != 0)
                _clipActions.Read(reader);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            var tc = TagCode;
            if (tc == SwfTagCode.PlaceObject2)
            {
                byte f = (byte)_flags;
                writer.WriteUInt8(f);
            }
            else
            {
                writer.WriteUInt8((byte)_flags);
                writer.WriteUInt8((byte)((int)_flags >> 8));
            }

            writer.WriteUInt16(_depth);

            if ((_flags & SwfPlaceFlags.HasCharacter) != 0)
                writer.WriteUInt16(_cid);
            if ((_flags & SwfPlaceFlags.HasMatrix) != 0)
                writer.WriteMatrix(_matrix);
            if ((_flags & SwfPlaceFlags.HasColorTransform) != 0)
                _colorTransform.Write(writer, true);
            if ((_flags & SwfPlaceFlags.HasRatio) != 0)
                writer.WriteUInt16(_ratio);
            if ((_flags & SwfPlaceFlags.HasName) != 0)
                writer.WriteString(_name);
            if ((_flags & SwfPlaceFlags.HasClipDepth) != 0)
                writer.WriteUInt16(_clipDepth);

            WriteBeforeClipActions(writer);

            if ((_flags & SwfPlaceFlags.HasClipActions) != 0)
                _clipActions.Write(writer);
        }

        protected virtual void ReadBeforeClipActions(SwfReader reader)
        {
        }

        protected virtual void WriteBeforeClipActions(SwfWriter writer)
        {
        }

        public override void DumpBody(XmlWriter writer)
        {
            if ((_flags & SwfPlaceFlags.HasCharacter) != 0)
                writer.WriteAttributeString("cid", _cid.ToString());

            writer.WriteAttributeString("depth", _depth.ToString());
            writer.WriteAttributeString("flags", _flags.ToString());

            if (SwfDumpService.DumpDisplayListTags)
            {
                if ((_flags & SwfPlaceFlags.HasMatrix) != 0)
                    writer.WriteElementString("matrix", _matrix.GetMatrixString());
                if ((_flags & SwfPlaceFlags.HasColorTransform) != 0)
                    _colorTransform.Dump(writer, true);
                if ((_flags & SwfPlaceFlags.HasRatio) != 0)
                    writer.WriteElementString("ratio", _ratio.ToString());
                //if ((_flags & SwfPlaceFlags.HasName) != 0)
                //    writer.WriteElementString("name", XmlHelper.EntifyString(_name));
                if ((_flags & SwfPlaceFlags.HasClipDepth) != 0)
                    writer.WriteElementString("clip-depth", _clipDepth.ToString());
            }
        }
        #endregion

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            if (HasCharacter)
            {
                to.ImportCharacter(from, ref _cid);
            }
            else
            {
            }
        }

        public override void GetRefs(SwfRefList list)
        {
            if (HasCharacter)
            {
                list.Add(_cid);
            }
        }
    }

    [Flags]
    public enum SwfPlaceFlags
    {
        HasMove = 0x0001,
        HasCharacter = 0x0002,
        HasMatrix = 0x0004,
        HasColorTransform = 0x0008,
        HasRatio = 0x0010,
        HasName = 0x0020,
        HasClipDepth = 0x0040,
        HasClipActions = 0x0080,
        //5 - reserved
        HasCacheAsBitmap = 0x0400,
        HasBlendMode = 0x0200,
        HasFilterList = 0x0100,
    }

    public enum SwfPlaceMode
    {
        New,
        Modify,
        Renew
    }
}