using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DataDynamics.PageFX.FLI.SWF
{
    public static class Extensions
    {
        public static int ToTwips(this float value)
        {
            double v = Math.Round(value * 20.0);
            return (int)v;
        }

        public static float FromTwips(this int value)
        {
            return (float)(value / 20.0);
        }

        public static string GetMatrixString(this Matrix m)
        {
            if (m == null) return "";
            return m.Elements.Join();
        }

        public static string ToHtmlHex(this Color c, bool alpha)
        {
            if (alpha)
                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);
            return string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
        }

        public static string ToHtmlHex(this Color c)
        {
            return c.ToHtmlHex(true);
        }

        public static string ToHtmlHex(this IEnumerable<Color> colors, bool alpha)
        {
            return colors.Join("", "", ", ", c => c.ToHtmlHex(alpha));
        }
    }
}