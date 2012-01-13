using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;

namespace DataDynamics.PageFX.FLI.SWF
{
    public static class SwfHelper
    {
        public static int ToTwips(float value)
        {
            double v = Math.Round(value * 20.0);
            return (int)v;
        }

        public static float FromTwips(int value)
        {
            return (float)(value / 20.0);
        }

        public static string ToString(Matrix m)
        {
            if (m == null) return "";
            return Str.ToString(m.Elements);
        }

        public static string ToHtmlHex(Color c, bool alpha)
        {
            if (alpha)
                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);
            return string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
        }

        public static string ToHtmlHex(Color c)
        {
            return ToHtmlHex(c, true);
        }

        public static string ToString(Color[] colors, bool alpha)
        {
            if (colors == null) return "[]";
            var sb = new StringBuilder();
            int n = colors.Length;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(ToHtmlHex(colors[i], alpha));
            }
            return sb.ToString();
        }

        internal static Dictionary<TEnum, TValue> GetEnumAttributeMap<TEnum, TValue, TAttr>(Converter<TAttr, TValue> cnv)
            where TAttr : Attribute
        {
            var fields =
                typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField);
            var map = new Dictionary<TEnum, TValue>();
            foreach (var fi in fields)
            {
                var value = (TEnum)fi.GetValue(null);
                var attr = ReflectionHelper.GetAttribute<TAttr>(fi, false);
                if (attr != null)
                {
                    map.Add(value, cnv(attr));
                }
            }
            return map;
        }
    }
}