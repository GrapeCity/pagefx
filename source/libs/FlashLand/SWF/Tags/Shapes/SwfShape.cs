using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using DataDynamics.PageFX.Common.Graphics;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes
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
        #region ctors
        public SwfShapeSetupRecord()
        {
        }

        public SwfShapeSetupRecord(PathPainting paint, float dx, float dy)
        {
            if ((paint & PathPainting.Stroke) != 0)
            {
                _lineStyle = 1;
                _state |= SwfStyleState.HasLineStyle;
            }

            if ((paint & PathPainting.Fill) != 0)
            {
                _fillStyle0 = 1;
                _fillStyle1 = 0;
                _state |= SwfStyleState.HasFillStyle0;
                _state |= SwfStyleState.HasFillStyle1;
            }

            if (dx != 0 || dy != 0)
            {
                _state |= SwfStyleState.HasMoveTo;
                _deltaX = dx;
                _deltaY = dy;
            }
        }
        #endregion

        #region Properties
        public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.StyleChange; }
        }

        public SwfStyleState State
        {
            get { return _state; }
            set { _state = value; }
        }
        private SwfStyleState _state;

        public float DeltaX
        {
            get { return _deltaX; }
            set { _deltaX = value; }
        }
        private float _deltaX;

        public float DeltaY
        {
            get { return _deltaY; }
            set { _deltaY = value; }
        }
        private float _deltaY;

        public int FillStyle0
        {
            get { return _fillStyle0; }
            set { _fillStyle0 = value; }
        }
        private int _fillStyle0;

        public int FillStyle1
        {
            get { return _fillStyle1; }
            set { _fillStyle1 = value; }
        }
        private int _fillStyle1;

        public int LineStyle
        {
            get { return _lineStyle; }
            set { _lineStyle = value; }
        }
        private int _lineStyle;

        public SwfStyles Styles
        {
            get { return _styles; }
        }
        private SwfStyles _styles;
        #endregion

        #region Read
        private int _bits;
        private bool _read;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            if ((_state & SwfStyleState.HasMoveTo) != 0)
            {
                int bits = (int)reader.ReadUB(5);
                _deltaX = reader.ReadTwip(bits);
                _deltaY = reader.ReadTwip(bits);
                _bits = bits;
                _read = true;
            }

            if ((_state & SwfStyleState.HasFillStyle0) != 0)
                _fillStyle0 = reader.ReadFillStyle();

            if ((_state & SwfStyleState.HasFillStyle1) != 0)
                _fillStyle1 = reader.ReadFillStyle();

            if ((_state & SwfStyleState.HasLineStyle) != 0)
                _lineStyle = reader.ReadLineStyle();

            if ((_state & SwfStyleState.HasNewStyles) != 0)
            {
                _styles = new SwfStyles();
                _styles.Read(reader, shapeType);
            }
        }
        #endregion

        #region Write
        public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            //auto detect state
            if (_styles != null)
            {
                //TODO: check shape type
                _state |= SwfStyleState.HasNewStyles;
            }

            writer.WriteBit(false); //edge flag
            //writer.WriteBit((_state & SwfStyleState.HasNewStyles) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasLineStyle) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasFillStyle1) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasFillStyle0) != 0);
            //writer.WriteBit((_state & SwfStyleState.HasMoveTo) != 0);
            writer.WriteUB((uint)_state, 5);

            if ((_state & SwfStyleState.HasMoveTo) != 0)
            {
                if (_read)
                {
                    writer.WriteUB((uint)_bits, 5);
                    writer.WriteTwip(_deltaX, _bits);
                    writer.WriteTwip(_deltaY, _bits);
                }
                else
                {
                    writer.WriteBitwiseTwipPoint(_deltaX, _deltaY, false);
                }
            }

            if ((_state & SwfStyleState.HasFillStyle0) != 0)
                writer.WriteFillStyle(_fillStyle0);

            if ((_state & SwfStyleState.HasFillStyle1) != 0)
                writer.WriteFillStyle(_fillStyle1);

            if ((_state & SwfStyleState.HasLineStyle) != 0)
                writer.WriteLineStyle(_lineStyle);

            if (_styles != null && (_state & SwfStyleState.HasNewStyles) != 0)
            {
                _styles.Write(writer, shapeType);
            }
        }
        #endregion

        #region Dump
        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteAttributeString("state", _state.ToString());

            if ((_state & SwfStyleState.HasMoveTo) != 0)
            {
                writer.WriteAttributeString("dx", _deltaX.ToString());
                writer.WriteAttributeString("dy", _deltaY.ToString());
            }

            if ((_state & SwfStyleState.HasFillStyle0) != 0)
                writer.WriteAttributeString("fs0", _fillStyle0.ToString());

            if ((_state & SwfStyleState.HasFillStyle1) != 0)
                writer.WriteAttributeString("fs1", _fillStyle1.ToString());

            if ((_state & SwfStyleState.HasLineStyle) != 0)
                writer.WriteAttributeString("ls", _lineStyle.ToString());

            if ((_state & SwfStyleState.HasNewStyles) != 0)
                _styles.Dump(writer, shapeType);
        }
        #endregion
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
    internal class SwfShapeMoveToRecord : SwfShapeRecord
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
    public class SwfStraitEdge : SwfShapeRecord
    {
        public SwfStraitEdge()
        {
        }

        public SwfStraitEdge(float dx, float dy)
        {
            _dx = dx;
            _dy = dy;
        }

        public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.StraitEdge; }
        }

        public float DeltaX
        {
            get { return _dx; }
            set { _dx = value; }
        }
        private float _dx;

        public float DeltaY
        {
            get { return _dy; }
            set { _dy = value; }
        }
        private float _dy;

        private int _bits;
        private bool _read;
        
        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            int bits = (int)reader.ReadUB(4) + 2;
            bool gl = reader.ReadBit(); //general line, x and y
            if (gl)
            {
                _dx = reader.ReadTwip(bits);
                _dy = reader.ReadTwip(bits);
            }
            else
            {
                bool vl = reader.ReadBit(); //vertical line
                if (vl) _dy = reader.ReadTwip(bits);
                else _dx = reader.ReadTwip(bits);
            }
            _bits = bits;
            _read = true;
        }

        public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            if (_dx == 0 && _dy == 0) return;

            writer.WriteBit(true); //edge flag
            writer.WriteBit(true); //strait flag

            if (_dx == 0) //vert
            {
                WriteCoord(writer, _dy, true);
            }
            else if (_dy == 0) //horz
            {
                WriteCoord(writer, _dx, false);
            }
            else
            {
                int x = _dx.ToTwips();
                int y = _dy.ToTwips();
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
            writer.WriteAttributeString("dx", _dx.ToString());
            writer.WriteAttributeString("dy", _dy.ToString());
        }

        public override string ToString()
        {
            if (_dx == 0)
                return string.Format("v {0}", _dy);
            if (_dy == 0)
                return string.Format("h {0}", _dx);
            return string.Format("l {0} {1}", _dx, _dy);
        }
    }
    #endregion

    #region SwfCurveEdge
    public class SwfCurveEdge : SwfShapeRecord
    {
        public SwfCurveEdge()
        {
        }

        public SwfCurveEdge(float cdx, float cdy, float adx, float ady)
        {
            _cdx = cdx;
            _cdy = cdy;
            _adx = adx;
            _ady = ady;
        }

        public override SwfShapeRecordType Type
        {
            get { return SwfShapeRecordType.CurveEdge; }
        }

        public float ControlDeltaX
        {
            get { return _cdx; }
            set { _cdx = value; }
        }
        private float _cdx;

        public float ControlDeltaY
        {
            get { return _cdy; }
            set { _cdy = value; }
        }
        private float _cdy;

        public float AnchorDeltaX
        {
            get { return _adx; }
            set { _adx = value; }
        }
        private float _adx;

        public float AnchorDeltaY
        {
            get { return _ady; }
            set { _ady = value; }
        }
        private float _ady;

        private int _bits;
        private bool _read;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            int bits = (int)reader.ReadUB(4) + 2;
            _cdx = reader.ReadTwip(bits);
            _cdy = reader.ReadTwip(bits);
            _adx = reader.ReadTwip(bits);
            _ady = reader.ReadTwip(bits);
            _bits = bits;
            _read = true;
        }

        public override void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteBit(true); //edge flag
            writer.WriteBit(false); //strait flag

            int cx = _cdx.ToTwips();
            int cy = _cdy.ToTwips();
            int ax = _adx.ToTwips();
            int ay = _ady.ToTwips();

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
            writer.WriteAttributeString("cdx", _cdx.ToString());
            writer.WriteAttributeString("cdy", _cdy.ToString());
            writer.WriteAttributeString("adx", _adx.ToString());
            writer.WriteAttributeString("ady", _ady.ToString());
        }
    }
    #endregion
}