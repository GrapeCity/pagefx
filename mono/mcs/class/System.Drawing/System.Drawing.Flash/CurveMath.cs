using System.Collections.Generic;

namespace System.Drawing
{
    static class CurveMath
    {
        public const int CURVE_MIN_TERMS = 1;
        public const int CURVE_MAX_TERMS = 7;

        public static PointF[] CalcOpenCurveTangents(int terms, PointF[] points, float tension)
        {
            int n = points.Length;
            float coefficient = tension / 3.0f;
            var tangents = new PointF[n];

            if (n <= 2)
                return tangents;

            for (int i = 0; i < n; i++)
            {
                int r = i + 1;
                int s = i - 1;

                if (r >= n) r = n - 1;
                if (s < 0) s = 0;

                var pr = points[r];
                var ps = points[s];
                tangents[i].X += (coefficient * (pr.X - ps.X));
                tangents[i].Y += (coefficient * (pr.Y - ps.Y));
            }

            return tangents;
        }

        public static PointF[] CalcClosedCurveTangents(int terms, PointF[] points, float tension)
        {
            int n = points.Length;
            float coefficient = tension / 3.0f;
            var tangents = new PointF[n];

            if (n <= 2)
                return tangents;

            for (int i = 0; i < n; i++)
            {
                int r = i + 1;
                int s = i - 1;

                if (r >= n) r -= n;
                if (s < 0) s += n;

                var pr = points[r];
                var ps = points[s];
                tangents[i].X += (coefficient * (pr.X - ps.X));
                tangents[i].Y += (coefficient * (pr.Y - ps.Y));
            }

            return tangents;
        }

        /* Recursion depth of the flattening algorithm */
        private const int FLATTEN_RECURSION_LIMIT = 10;

        /* nr_curve_flatten comes from Sodipodi's libnr (public domain) available from http://www.sodipodi.com/ */
        /* Mono changes: converted to float (from double), added recursion limit, use GArray */
        public static bool nr_curve_flatten(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3, float flatness, int level, ICollection<PointF> points)
        {
            float dx1_0, dy1_0, dx2_0, dy2_0, dx3_0, dy3_0, dx2_3, dy2_3, d3_0_2;
            float s1_q, t1_q, s2_q, t2_q, v2_q;
            float f2, f2_q;
            float x00t, y00t, x0tt, y0tt, xttt, yttt, x1tt, y1tt, x11t, y11t;

            dx1_0 = x1 - x0;
            dy1_0 = y1 - y0;
            dx2_0 = x2 - x0;
            dy2_0 = y2 - y0;
            dx3_0 = x3 - x0;
            dy3_0 = y3 - y0;
            dx2_3 = x3 - x2;
            dy2_3 = y3 - y2;
            f2 = flatness;
            d3_0_2 = dx3_0 * dx3_0 + dy3_0 * dy3_0;
            if (d3_0_2 < f2)
            {
                float d1_0_2, d2_0_2;
                d1_0_2 = dx1_0 * dx1_0 + dy1_0 * dy1_0;
                d2_0_2 = dx2_0 * dx2_0 + dy2_0 * dy2_0;
                if ((d1_0_2 < f2) && (d2_0_2 < f2))
                {
                    goto nosubdivide;
                }
                else
                {
                    goto subdivide;
                }
            }
            f2_q = f2 * d3_0_2;
            s1_q = dx1_0 * dx3_0 + dy1_0 * dy3_0;
            t1_q = dy1_0 * dx3_0 - dx1_0 * dy3_0;
            s2_q = dx2_0 * dx3_0 + dy2_0 * dy3_0;
            t2_q = dy2_0 * dx3_0 - dx2_0 * dy3_0;
            v2_q = dx2_3 * dx3_0 + dy2_3 * dy3_0;
            if ((t1_q * t1_q) > f2_q) goto subdivide;
            if ((t2_q * t2_q) > f2_q) goto subdivide;
            if ((s1_q < 0.0) && ((s1_q * s1_q) > f2_q)) goto subdivide;
            if ((v2_q < 0.0) && ((v2_q * v2_q) > f2_q)) goto subdivide;
            if (s1_q >= s2_q) goto subdivide;

        nosubdivide:
            {
                points.Add(new PointF(x3, y3));
                return true;
            }
        subdivide:
            /* things gets *VERY* memory intensive without a limit */
            if (level >= FLATTEN_RECURSION_LIMIT)
                return false;

            x00t = (x0 + x1) * 0.5f;
            y00t = (y0 + y1) * 0.5f;
            x0tt = (x0 + 2 * x1 + x2) * 0.25f;
            y0tt = (y0 + 2 * y1 + y2) * 0.25f;
            x1tt = (x1 + 2 * x2 + x3) * 0.25f;
            y1tt = (y1 + 2 * y2 + y3) * 0.25f;
            x11t = (x2 + x3) * 0.5f;
            y11t = (y2 + y3) * 0.5f;
            xttt = (x0tt + x1tt) * 0.5f;
            yttt = (y0tt + y1tt) * 0.5f;

            if (!nr_curve_flatten(x0, y0, x00t, y00t, x0tt, y0tt, xttt, yttt, flatness, level + 1, points)) return false;
            if (!nr_curve_flatten(xttt, yttt, x1tt, y1tt, x11t, y11t, x3, y3, flatness, level + 1, points)) return false;
            return true;
        }
    }
}