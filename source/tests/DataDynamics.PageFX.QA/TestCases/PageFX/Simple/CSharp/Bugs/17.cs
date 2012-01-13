using System;
using System.Collections;
using System.Collections.Generic;

struct Length
{
    public float Value;
    public bool IsUndefined;

    public float ToTwips()
    {
        return Value;
    }

    public Length(bool undef)
    {
        Value = 0;
        IsUndefined = true;
    }
}

interface IReportItem
{
    Length Width { get; }
    Length Height { get; }
}

interface IRect : IReportItem
{
    IRoundingRadius RoundingRadius { get; }
}

class ReportItem : IReportItem
{
    Length _width;
    Length _height;

    public Length Width { get { return _width; } }
    public Length Height { get { return _height; } }
}

class Rect : ReportItem, IRect, IRoundingRadius
{
    //RR _rr = new RR();
    //public IRoundingRadius RoundingRadius { get { return _rr; } }

        Length _default;
    Length _topLeft = new Length(true);
    Length _topRight = new Length(true);
    Length _bottomLeft = new Length(true);
    Length _bottomRight = new Length(true);

    Length IRoundingRadius.Default { get { return _default; } }
    Length IRoundingRadius.TopLeft { get { return _topLeft; } }
    Length IRoundingRadius.TopRight { get { return _topRight; } }
    Length IRoundingRadius.BottomLeft { get { return _bottomLeft; } }
    Length IRoundingRadius.BottomRight { get { return _bottomRight; } }

    public IRoundingRadius RoundingRadius { get { return this; } }
}

interface IRoundingRadius
{
    Length Default { get; }
    Length TopLeft { get; }
    Length TopRight { get; }
    Length BottomLeft { get; }
    Length BottomRight { get; }
}

//class RR : IRoundingRadius
//{
//    Length _default;
//    Length _topLeft = new Length(true);
//    Length _topRight = new Length(true);
//    Length _bottomLeft = new Length(true);
//    Length _bottomRight = new Length(true);

//    public Length Default { get { return _default; } }
//    public Length TopLeft { get { return _topLeft; } }
//    public Length TopRight { get { return _topRight; } }
//    public Length BottomLeft { get { return _bottomLeft; } }
//    public Length BottomRight { get { return _bottomRight; } }
//}

internal enum Corner
{
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft
}

class X
{
    static float GetCornerRounding(IRect rectangle, Corner corner, float defaultRadius)
    {
        Length radiusLength;
        switch (corner)
        {
            case Corner.TopLeft:
                radiusLength = rectangle.RoundingRadius.TopLeft;
                break;
            case Corner.TopRight:
                radiusLength = rectangle.RoundingRadius.TopRight;
                break;
            case Corner.BottomRight:
                radiusLength = rectangle.RoundingRadius.BottomRight;
                break;
            case Corner.BottomLeft:
                radiusLength = rectangle.RoundingRadius.BottomLeft;
                break;
            default:
                return 0;
        }
        float result = (radiusLength.IsUndefined ? defaultRadius : radiusLength.ToTwips()) * 2; //rendering uses diameter
        result = Math.Max(0, result);
        float width = rectangle.Width.ToTwips();
        if (width > 0)
        {
            result = Math.Min(width, result);
        }
        float height = rectangle.Height.ToTwips();
        if (height > 0)
        {
            result = Math.Min(height, result);
        }
        return result;
    }

    static void Test1()
    {
        var rect = new Rect();
        var dr = rect.RoundingRadius.Default.ToTwips();
        Console.WriteLine(GetCornerRounding(rect, Corner.TopLeft, dr));
        Console.WriteLine(GetCornerRounding(rect, Corner.TopRight, dr));
        Console.WriteLine(GetCornerRounding(rect, Corner.BottomLeft, dr));
        Console.WriteLine(GetCornerRounding(rect, Corner.BottomRight, dr));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}