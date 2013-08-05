using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using DataDynamics.PageFX.Common.Graphics;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Shapes
{
    //NOTE: All shape records are byte aligned and begin with a TypeFlag.
    //If the TypeFlag is zero, the shape record is a non-edge record, and a further five bits of flag information follow.

    #region SwfShape
    public class SwfShape : List<SwfShapeRecord>, IPathRender
    {
        #region IO
        public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            while (true)
            {
                var r = CreateRecord(reader);
                if (r == null) break;
                r.Read(reader, shapeType);
                Add(r);
            }
            reader.Align();
        }

        private static SwfShapeRecord CreateRecord(SwfReader reader)
        {
            bool edgeFlag = reader.ReadBit();
            if (edgeFlag)
            {
                bool strait = reader.ReadBit();
                if (strait) return new SwfStraitEdge();
                return new SwfCurveEdge();
            }

            uint flags = reader.ReadUB(5);
            if (flags == 0) //end of shape
                return null;

        	return new SwfShapeSetupRecord {State = (SwfStyleState)flags};
        }

        public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var r = this[i];
                r.Write(writer, shapeType);
            }
            writer.WriteUB(0, 6); //EndShapeRecord
        }

        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("shape");
            int n = Count;
            writer.WriteAttributeString("count", n.ToString());
            for (int i = 0; i < n; ++i)
                this[i].Dump(writer, shapeType);
            writer.WriteEndElement();
        }
        #endregion

        #region IPathRender Members
        private PathPainting _paint;
        internal float X1 = float.MaxValue;
        internal float Y1 = float.MaxValue;
        internal float X2 = float.MinValue;
        internal float Y2 = float.MinValue;

        public void AddPath(GraphicsPath path, PathPainting paint)
        {
            _paint = paint;
            var b = path.GetBounds();
            if (b.X < X1) X1 = b.X;
            if (b.Right > X2) X2 = b.Right;
            if (b.Y < Y1) Y1 = b.Y;
            if (b.Bottom > Y2) Y2 = b.Bottom;
            PathRendering.Render(path, this, PathRenderFeatures.Quad);
        }

        private bool _first = true;
        void IPathRender.Move(PointF cur, PointF pt)
        {
            if (_first)
            {
                _first = false;
                float dx = pt.X;
                float dy = pt.Y;
                Add(new SwfShapeSetupRecord(_paint, dx, dy));
            }
            else
            {
                float dx = pt.X - cur.X;
                float dy = pt.Y - cur.Y;
                if (dx == 0 && dy == 0) return;
                Add(new SwfShapeMoveToRecord(dx, dy));
            }
        }

        void IPathRender.Line(PointF cur, PointF pt)
        {
            float dx = pt.X - cur.X;
            float dy = pt.Y - cur.Y;
            if (dx == 0 && dy == 0) return;
            Add(new SwfStraitEdge(dx, dy));
        }

        void IPathRender.Quad(PointF cur, PointF c, PointF pt)
        {
            float ctrlDeltaX = c.X - cur.X;
            float ctrlDeltaY = c.Y - cur.Y;
            float anchorX = pt.X - c.X;
            float anchorY = pt.Y - c.Y;
            if (ctrlDeltaX == 0 && ctrlDeltaY == 0 && anchorX == 0 && anchorY == 0) return;
            Add(new SwfCurveEdge(ctrlDeltaX, ctrlDeltaY, anchorX, anchorY));
        }

        void IPathRender.Cubic(PointF cur, PointF c1, PointF c2, PointF pt)
        {
        }

        void IPathRender.Close(PointF cur)
        {
        }
        #endregion

        #region Utils
        internal static bool IsMorph(SwfTagCode shapeType)
        {
            switch (shapeType)
            {
                case SwfTagCode.DefineMorphShape:
                case SwfTagCode.DefineMorphShape2:
                    return true;
            }
            return false;
        }

        internal static bool HasAlpha(SwfTagCode shapeType)
        {
            switch (shapeType)
            {
                case SwfTagCode.DefineShape3:
                case SwfTagCode.DefineMorphShape:
                case SwfTagCode.DefineMorphShape2:
                    return true;
            }
            return false;
        }
        #endregion

        public void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            foreach (var r in this)
            {
                var s = r as SwfShapeSetupRecord;
                if (s != null && s.Styles != null)
                    s.Styles.ImportDependencies(from, to);
            }
        }

        public void GetRefs(SwfRefList list)
        {
            foreach (var r in this)
            {
                var s = r as SwfShapeSetupRecord;
                if (s != null && s.Styles != null)
                    s.Styles.GetRefs(list);
            }
        }
    }
    #endregion

    public enum SwfShapeRecordType
    {
        StyleChange,
        StraitEdge,
        CurveEdge,
        End
    }

    #region SwfShapeRecord
    public abstract class SwfShapeRecord
    {
        public abstract SwfShapeRecordType Type { get; }

        public abstract void Read(SwfReader reader, SwfTagCode shapeType);

        public abstract void Write(SwfWriter writer, SwfTagCode shapeType);

        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("record");
            writer.WriteAttributeString("type", Type.ToString());
            DumpBody(writer, shapeType);
            writer.WriteEndElement();
        }

        protected virtual void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
        }
    }
    #endregion

    #region SwfShapeSetupRecord
    public class SwfShapeSetupRecord : SwfShapeRecord
    {
	    public SwfShapeSetupRecord()
        {
        }

        public SwfShapeSetupRecord(PathPainting paint, float dx, float dy)
        {
            if ((paint & PathPainting.Stroke) != 0)
            {
                LineStyle = 1;
                State |= SwfStyleState.HasLineStyle;
            }

            if ((paint & PathPainting.Fill) != 0)
            {
                FillStyle0 = 1;
                FillStyle1 = 0;
                State |= SwfStyleState.HasFillStyle0;
                State |= SwfStyleState.HasFillStyle1;
            }

            if (dx != 0 || dy != 0)
            {
                State |= SwfStyleState.HasMoveTo;
                DeltaX = dx;
                DeltaY = dy;
            }
        }

	    public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.StyleChange; }
        }

	    public SwfStyleState State { get; set; }

	    public float DeltaX { get; set; }

	    public float DeltaY { get; set; }

	    public int FillStyle0 { get; set; }

	    public int FillStyle1 { get; set; }

	    public int LineStyle { get; set; }

	    public SwfStyles Styles { get; private set; }

	    private int _bits;
        private bool _read;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            if ((State & SwfStyleState.HasMoveTo) != 0)
            {
                int bits = (int)reader.ReadUB(5);
                DeltaX = reader.ReadTwip(bits);
                DeltaY = reader.ReadTwip(bits);
                _bits = bits;
                _read = true;
            }

            if ((State & SwfStyleState.HasFillStyle0) != 0)
                FillStyle0 = reader.ReadFillStyle();

            if ((State & SwfStyleState.HasFillStyle1) != 0)
                FillStyle1 = reader.ReadFillStyle();

            if ((State & SwfStyleState.HasLineStyle) != 0)
                LineStyle = reader.ReadLineStyle();

            if ((State & SwfStyleState.HasNewStyles) != 0)
            {
                Styles = new SwfStyles();
                Styles.Read(reader, shapeType);
            }
        }

	    public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            //auto detect state
            if (Styles != null)
            {
                //TODO: check shape type
                State |= SwfStyleState.HasNewStyles;
            }

            writer.WriteBit(false); //edge flag
            //writer.WriteBit((_state & SwfStyleState.HasNewStyles) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasLineStyle) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasFillStyle1) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasFillStyle0) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasMoveTo) != 0);
            writer.WriteUB((uint)State, 5);

            if ((State & SwfStyleState.HasMoveTo) != 0)
            {
                if (_read)
                {
                    writer.WriteUB((uint)_bits, 5);
                    writer.WriteTwip(DeltaX, _bits);
                    writer.WriteTwip(DeltaY, _bits);
                }
                else
                {
                    writer.WriteBitwiseTwipPoint(DeltaX, DeltaY, false);
                }
            }

            if ((State & SwfStyleState.HasFillStyle0) != 0)
                writer.WriteFillStyle(FillStyle0);

            if ((State & SwfStyleState.HasFillStyle1) != 0)
                writer.WriteFillStyle(FillStyle1);

            if ((State & SwfStyleState.HasLineStyle) != 0)
                writer.WriteLineStyle(LineStyle);

            if (Styles != null && (State & SwfStyleState.HasNewStyles) != 0)
            {
                Styles.Write(writer, shapeType);
            }
        }

	    protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteAttributeString("state", State.ToString());

            if ((State & SwfStyleState.HasMoveTo) != 0)
            {
                writer.WriteAttributeString("dx", DeltaX.ToString());
                writer.WriteAttributeString("dy", DeltaY.ToString());
            }

            if ((State & SwfStyleState.HasFillStyle0) != 0)
                writer.WriteAttributeString("fs0", FillStyle0.ToString());

            if ((State & SwfStyleState.HasFillStyle1) != 0)
                writer.WriteAttributeString("fs1", FillStyle1.ToString());

            if ((State & SwfStyleState.HasLineStyle) != 0)
                writer.WriteAttributeString("ls", LineStyle.ToString());

            if ((State & SwfStyleState.HasNewStyles) != 0)
                Styles.Dump(writer, shapeType);
        }
    }

    [Flags]
    public enum SwfStyleState
    {
        HasMoveTo = 0x01,
        HasFillStyle0 = 0x02,
        HasFillStyle1 = 0x04,
        HasLineStyle = 0x08,
        HasNewStyles = 0x10,
    }
    #endregion

    #region SwfShapeMoveToRecord
    internal sealed class SwfShapeMoveToRecord : SwfShapeRecord
    {
        public SwfShapeMoveToRecord(float dx, float dy)
        {
            _dx = dx;
            _dy = dy;
        }

        public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.StyleChange; }
        }

        private readonly float _dx;
        private readonly float _dy;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteBit(false); //TypeFlag
            writer.WriteBit(false); //StateNewStyles
            writer.WriteBit(false); //StateLineStyle
            writer.WriteBit(false); //StateFillStyle1
            writer.WriteBit(false); //StateFillStyle0
            writer.WriteBit(true); //StateMoveTo
            int dx = _dx.ToTwips();
            int dy = _dy.ToTwips();
            int bits = dx.GetMinBits(dy);
            writer.WriteUB((uint)bits, 5);
            writer.WriteSB(dx, bits);
            writer.WriteSB(dy, bits);
        }
    }
    #endregion

    #region SwfStraitEdge
    public sealed class SwfStraitEdge : SwfShapeRecord
    {
        public SwfStraitEdge()
        {
        }

        public SwfStraitEdge(float dx, float dy)
        {
            DeltaX = dx;
            DeltaY = dy;
        }

        public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.StraitEdge; }
        }

	    public float DeltaX { get; set; }

	    public float DeltaY { get; set; }

	    private int _bits;
        private bool _read;
        
        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            int bits = (int)reader.ReadUB(4) + 2;
            bool gl = reader.ReadBit(); //general line, x and y
            if (gl)
            {
                DeltaX = reader.ReadTwip(bits);
                DeltaY = reader.ReadTwip(bits);
            }
            else
            {
                bool vl = reader.ReadBit(); //vertical line
                if (vl) DeltaY = reader.ReadTwip(bits);
                else DeltaX = reader.ReadTwip(bits);
            }
            _bits = bits;
            _read = true;
        }

        public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            if (DeltaX == 0 && DeltaY == 0) return;

            writer.WriteBit(true); //edge flag
            writer.WriteBit(true); //strait flag

            if (DeltaX == 0) //vert
            {
                WriteCoord(writer, DeltaY, true);
            }
            else if (DeltaY == 0) //horz
            {
                WriteCoord(writer, DeltaX, false);
            }
            else
            {
                int x = DeltaX.ToTwips();
                int y = DeltaY.ToTwips();
                int bits = _bits;
                if (!_read)
                    bits = Math.Max(x.GetMinBits(y), 2);
                writer.WriteUB((uint)(bits - 2), 4);
                writer.WriteBit(true); //gl
                writer.WriteSB(x, bits);
                writer.WriteSB(y, bits);
            }
        }

        private void WriteCoord(SwfWriter writer, float value, bool vl)
        {
            int v = value.ToTwips();
            int bits = _bits;
            if (!_read)
                bits = Math.Max(v.GetMinBits(), 2);
            writer.WriteUB((uint)(bits - 2), 4);
            writer.WriteBit(false); //gl
            writer.WriteBit(vl); //vl
            writer.WriteSB(v, bits);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteAttributeString("dx", DeltaX.ToString());
            writer.WriteAttributeString("dy", DeltaY.ToString());
        }

        public override string ToString()
        {
            if (DeltaX == 0)
                return string.Format("v {0}", DeltaY);
            if (DeltaY == 0)
                return string.Format("h {0}", DeltaX);
            return string.Format("l {0} {1}", DeltaX, DeltaY);
        }
    }
    #endregion

    #region SwfCurveEdge
    public sealed class SwfCurveEdge : SwfShapeRecord
    {
        public SwfCurveEdge()
        {
        }

        public SwfCurveEdge(float cdx, float cdy, float adx, float ady)
        {
            ControlDeltaX = cdx;
            ControlDeltaY = cdy;
            AnchorDeltaX = adx;
            AnchorDeltaY = ady;
        }

        public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.CurveEdge; }
        }

	    public float ControlDeltaX { get; set; }

	    public float ControlDeltaY { get; set; }

	    public float AnchorDeltaX { get; set; }

	    public float AnchorDeltaY { get; set; }

	    private int _bits;
        private bool _read;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            int bits = (int)reader.ReadUB(4) + 2;
            ControlDeltaX = reader.ReadTwip(bits);
            ControlDeltaY = reader.ReadTwip(bits);
            AnchorDeltaX = reader.ReadTwip(bits);
            AnchorDeltaY = reader.ReadTwip(bits);
            _bits = bits;
            _read = true;
        }

        public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteBit(true); //edge flag
            writer.WriteBit(false); //strait flag

            int cx = ControlDeltaX.ToTwips();
            int cy = ControlDeltaY.ToTwips();
            int ax = AnchorDeltaX.ToTwips();
            int ay = AnchorDeltaY.ToTwips();

            int bits = _bits;
            if (!_read)
                bits = Math.Max(cx.GetMinBits(cy, ax, ay), 2);

            writer.WriteUB((uint)(bits - 2), 4);
            writer.WriteSB(cx, bits);
            writer.WriteSB(cy, bits);
            writer.WriteSB(ax, bits);
            writer.WriteSB(ay, bits);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteAttributeString("cdx", ControlDeltaX.ToString());
            writer.WriteAttributeString("cdy", ControlDeltaY.ToString());
            writer.WriteAttributeString("adx", AnchorDeltaX.ToString());
            writer.WriteAttributeString("ady", AnchorDeltaY.ToString());
        }
    }
    #endregion
}