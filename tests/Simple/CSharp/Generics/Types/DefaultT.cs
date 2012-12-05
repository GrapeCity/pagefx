using System;

struct Point
{
    public int X;
    public int Y;

    public override string ToString()
    {
        return string.Format("{{X = {0}, Y = {1}}}", X, Y);
    }
}

struct Pair<TFirst, TSecond>
{
    public TFirst First;
    public TSecond Second;

    public override string ToString()
    {
        return string.Format("<{0}, {1}>", First, Second);
    }
}

class A<T>
{
    public static T GetDefault()
    {
        return default(T);
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine(A<int>.GetDefault());
        Console.WriteLine(A<bool>.GetDefault());
        Console.WriteLine(A<string>.GetDefault());
        Console.WriteLine(A<Point>.GetDefault());
        Console.WriteLine(A<A<int>>.GetDefault());
        Console.WriteLine(A<Pair<int, int>>.GetDefault());
        Console.WriteLine(A<Pair<bool, bool>>.GetDefault());
        Console.WriteLine("<%END%>");
    }
}