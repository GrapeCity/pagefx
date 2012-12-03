using System.Drawing.Drawing2D;
using flash.display;
using GraphicsPath=System.Drawing.Drawing2D.GraphicsPath;

namespace System.Drawing.Flash
{
    using Graphics = flash.display.Graphics;

    public static class FlashDrawingAdapter
    {
        #region SetBrush
        public static void SetBrush(Graphics g, Brush brush)
        {
            if (brush == null) return;

            var sb = brush as SolidBrush;
            if (sb != null)
            {
                SetFillColor(g, sb.Color);
                return;
            }

            var lg = brush as LinearGradientBrush;
            if (lg != null)
            {
                BeginGradientFill(g, lg);
                return;
            }

            var tb = brush as TextureBrush;
            if (tb != null)
            {
                BeginTextureBrush(g, tb);
                return;
            }

            throw new NotImplementedException();
        }
        #endregion

        public static void SetFillColor(Graphics g, Color color)
        {
            g.beginFill(ColorHelper.ToFlash(color), color.A / 255.0);
        }

        #region BeginGradientFill
        static void BeginGradientFill(Graphics g, LinearGradientBrush brush)
        {
            Avm.Array colors, alphas, ratios;
            InitGradientArrays(brush, out colors, out alphas, out ratios);

            g.beginGradientFill(
                GradientType.LINEAR,
                colors,
                alphas,
                ratios,
                brush.Transform.native);
        }

        static void AddColor(Avm.Array colors, Avm.Array alphas, Color c)
        {
            colors.push(ColorHelper.ToFlash(c));
            alphas.push(c.A / 255.0);
        }

        static void InitGradientArrays(
            LinearGradientBrush brush,
            out Avm.Array colors,
            out Avm.Array alphas,
            out Avm.Array ratios)
        {
            colors = new Avm.Array();
            alphas = new Avm.Array();
            ratios = new Avm.Array();
            
            var cb = brush.InterpolationColors;
            if (cb != null)
            {
                int n = cb.Colors.Length;
                for (int i = 0; i < n; ++i)
                {
                    var c = cb.Colors[i];
                    AddColor(colors, alphas, c);
                    ratios.push((uint)(cb.Positions[i] * 255));
                }
            }
            else
            {
                var c1 = brush.LinearColors[0];
                var c2 = brush.LinearColors[1];
                AddColor(colors, alphas, c1);
                AddColor(colors, alphas, c2);
                ratios.push(0);
                ratios.push(255);
            }
        }
        #endregion

        public static void BeginTextureBrush(Graphics g, TextureBrush brush)
        {
            g.beginBitmapFill(brush.Image.Data, brush.Transform.native, brush.Repeat);
        }

        #region SetPen
        public static void SetPen(Graphics g, Pen pen)
        {
            if (pen == null) return;

            switch (pen.PenType)
            {
                case PenType.SolidColor:
                    SetLineStyle(g, pen);
                    break;

                case PenType.LinearGradient:
                    {
                        SetLineStyle(g, pen);
                        var lg = pen.Brush as LinearGradientBrush;
                        if (lg != null)
                        {
                            Avm.Array colors, alphas, ratios;
                            InitGradientArrays(lg, out colors, out alphas, out ratios);
                            g.lineGradientStyle(
                                GradientType.LINEAR,
                                colors,
                                alphas,
                                ratios,
                                lg.Transform.native);
                        }
                    }
                    break;

                case PenType.TextureFill:
                    {
                        SetLineStyle(g, pen);
                        var tb = pen.Brush as TextureBrush;
                        if (tb != null)
                        {
                            g.lineBitmapStyle(tb.Image.Data, tb.Transform.native, tb.Repeat);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region SetLineStyle
        static void SetLineStyle(Graphics g, Pen pen)
        {
            var c = pen.Color;

            if (pen.DashStyle != DashStyle.Solid)
            {
                Color bc = Color.FromArgb(0, 0, 0, 0);
                DashedLine.SetLineStyle(g, pen.DashStyle, pen.Width, c, bc);
            }
            else
            {
                g.lineStyle(
                    pen.Width, //thickness
                    ColorHelper.ToFlash(c), //color
                    c.A / 255.0, //alpha
                    false, //pixelHinting
                    LineScaleMode.NORMAL, //scaleMode
                    null, //caps
                    ToFlash(pen.LineJoin), //jointStyle
                    pen.MiterLimit //miterLimit
                    );
            }
            
        }
        #endregion

        public static Avm.String ToFlash(LineJoin value)
        {
            switch (value)
            {
                case LineJoin.Bevel:
                    return JointStyle.BEVEL;
                case LineJoin.Miter:
                case LineJoin.MiterClipped:
                    return JointStyle.MITER;
                case LineJoin.Round:
                    return JointStyle.ROUND;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void ResetPen(Graphics g)
        {
            g.lineStyle();
        }

        public static void DrawPath(Graphics g, GraphicsPath path)
        {
            var vec = new Avm.Vector<IGraphicsData>();
            vec.push(path.NativeObject);
            g.drawGraphicsData(vec);
        }
    }
}