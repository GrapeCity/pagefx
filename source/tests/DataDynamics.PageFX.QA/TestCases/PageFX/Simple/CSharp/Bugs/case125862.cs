using System;
using System.Drawing;

class X
{
    static PointF[] gdip_closed_curve_tangents(int terms, PointF[] points, float tension)
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

            tangents[i].X += (coefficient * (points[r].X - points[s].X));
            tangents[i].Y += (coefficient * (points[r].Y - points[s].Y));
        }

        return tangents;
    }

    static void Main()
    {
        var tans = gdip_closed_curve_tangents(
            0,
            new[]
                {
                    new PointF(100, 100),
                    new PointF(200, 200),
                    new PointF(200, 100),
                },
            0.5f);

        for (int i = 0; i < tans.Length; ++i)
        {
            Console.WriteLine("[{0}] = {1}, {2}", i, tans[i].X, tans[i].Y);
        }

        Console.WriteLine("<%END%>");
    }
}