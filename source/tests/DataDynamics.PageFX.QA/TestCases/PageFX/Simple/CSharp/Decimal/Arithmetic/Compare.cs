using System;

class X
{
    static void Compare(decimal x, decimal y)
    {
        Console.WriteLine("Compare({0}, {1}) = {2}", x, y, x.CompareTo(y));
    }

    static void TestCompareTo()
    {
        Compare(decimal.MinValue, decimal.MaxValue);
        Compare(decimal.MaxValue, decimal.MinValue);
        Compare(decimal.MinValue, 0);
        Compare(0, decimal.MinValue);
        Compare(1.1m, 1.11m);
    }

    static void Main()
    {
        TestCompareTo();
        Console.WriteLine("<%END%>");
    }
}