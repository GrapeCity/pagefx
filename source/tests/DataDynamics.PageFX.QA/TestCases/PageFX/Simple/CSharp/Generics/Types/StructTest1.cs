using System;

struct Pair<TFirst, TSecond>
{
    public TFirst First;
    public TSecond Second;

    public Pair(TFirst f, TSecond s)
    {
        First = f;
        Second = s;
    }
}

class Test
{
    static Pair<int, int> Inc(Pair<int, int> p)
    {
        ++p.First;
        ++p.Second;
        return p;
    }

    static void Test1()
    {
        Pair<int, int> p = new Pair<int, int>(10, 10);
        p = Inc(p);
        Console.WriteLine(p.First);
        Console.WriteLine(p.Second);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}