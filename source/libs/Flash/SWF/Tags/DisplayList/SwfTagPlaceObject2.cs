using System;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.DisplayList
{
    [SwfTag(SwfTagCode.PlaceObject2)]
    public class SwfTagPlaceObject2 : SwfTag
    {
	    public SwfTagPlaceObject2()
        {
        }

        public SwfTagPlaceObject2(ISwfDisplayObject obj, Matrix matrix, SwfPlaceMode mode)
        {
            Depth = obj.Depth;
            _cid = obj.CharacterId;
            _matrix = matrix;

            if (mode == SwfPlaceMode.Modify || mode == SwfPlaceMode.Renew)
                Flags |= SwfPlaceFlags.HasMove;

            if (mode != SwfPlaceMode.Modify)
                Flags |= SwfPlaceFlags.HasCharacter;

            if (matrix != null)
                Flags |= SwfPlaceFlags.HasMatrix;
        }

	    public SwfPlaceFlags Flags { get; set; }

	    public bool HasCharacter
        {
            get { return (Flags & SwfPlaceFlags.HasCharacter) != 0; }
        }

	    public ushort Depth { get; set; }

	    public ushort CharID
        {
            get { return _cid; }
            set
            {
                _cid = value;
                Flags |= SwfPlaceFlags.HasCharacter;
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
                    Flags |= SwfPlaceFlags.HasMatrix;
                else
                    Flags &= ~SwfPlaceFlags.HasMatrix;
            }
        }
        private Matrix _matrix;

	    public SwfColorTransform ColorTransform { get; set; }

	    public ushort Ratio { get; set; }

	    public string Name { get; set; }

	    public ushort ClipDepth
        {
            get { return _clipDepth; }
            set
            {
                _clipDepth = value;
                Flags |= SwfPlaceFlags.HasClipDepth;
            }
        }
        private ushort _clipDepth;

        public SwfEventList ClipActions
        {
            get { return _clipActions; }
        }
        private readonly SwfEventList _clipActions = new SwfEventList();

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.PlaceObject2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            var tc = TagCode;
            if (tc == SwfTagCode.PlaceObject2)
            {
                Flags = (SwfPlaceFlags)reader.ReadUInt8();
            }
            else
            {
                Flags = (SwfPlaceFlags)reader.ReadUInt8();
                Flags |= (SwfPlaceFlags)(reader.ReadUInt8() << 8);
            }

            Depth = reader.ReadUInt16();

            if ((Flags & SwfPlaceFlags.HasCharacter) != 0)
                _cid = reader.ReadUInt16();
            if ((Flags & SwfPlaceFlags.HasMatrix) != 0)
                _matrix = reader.ReadMatrix();
            if ((Flags & SwfPlaceFlags.HasColorTransform) != 0)
                ColorTransform = reader.ReadColorTransform(true);
            if ((Flags & SwfPlaceFlags.HasRatio) != 0)
                Ratio = reader.ReadUInt16();
            if ((Flags & SwfPlaceFlags.HasName) != 0)
                Name = reader.ReadString();
            if ((Flags & SwfPlaceFlags.HasClipDepth) != 0)
                _clipDepth = reader.ReadUInt16();

            ReadBeforeClipActions(reader);

            if ((Flags & SwfPlaceFlags.HasClipActions) != 0)
                _clipActions.Read(reader);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            var tc = TagCode;
            if (tc == SwfTagCode.PlaceObject2)
            {
                byte f = (byte)Flags;
                writer.WriteUInt8(f);
            }
            else
            {
                writer.WriteUInt8((byte)Flags);
                writer.WriteUInt8((byte)((int)Flags >> 8));
            }

            writer.WriteUInt16(Depth);

            if ((Flags & SwfPlaceFlags.HasCharacter) != 0)
                writer.WriteUInt16(_cid);
            if ((Flags & SwfPlaceFlags.HasMatrix) != 0)
                writer.WriteMatrix(_matrix);
            if ((Flags & SwfPlaceFlags.HasColorTransform) != 0)
                ColorTransform.Write(writer, true);
            if ((Flags & SwfPlaceFlags.HasRatio) != 0)
                writer.WriteUInt16(Ratio);
            if ((Flags & SwfPlaceFlags.HasName) != 0)
                writer.WriteString(Name);
            if ((Flags & SwfPlaceFlags.HasClipDepth) != 0)
                writer.WriteUInt16(_clipDepth);

            WriteBeforeClipActions(writer);

            if ((Flags & SwfPlaceFlags.HasClipActions) != 0)
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
            if ((Flags & SwfPlaceFlags.HasCharacter) != 0)
                writer.WriteAttributeString("cid", _cid.ToString());

            writer.WriteAttributeString("depth", Depth.ToString());
            writer.WriteAttributeString("flags", Flags.ToString());

            if (SwfDumpService.DumpDisplayListTags)
            {
                if ((Flags & SwfPlaceFlags.HasMatrix) != 0)
                    writer.WriteElementString("matrix", _matrix.GetMatrixString());
                if ((Flags & SwfPlaceFlags.HasColorTransform) != 0)
                    ColorTransform.Dump(writer, true);
                if ((Flags & SwfPlaceFlags.HasRatio) != 0)
                    writer.WriteElementString("ratio", Ratio.ToString());
                //if ((_flags & SwfPlaceFlags.HasName) != 0)
                //    writer.WriteElementString("name", XmlHelper.EntifyString(_name));
                if ((Flags & SwfPlaceFlags.HasClipDepth) != 0)
                    writer.WriteElementString("clip-depth", _clipDepth.ToString());
            }
        }

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