using System;

struct S1
{
    public int X;

    public override string ToString()
    {
        return string.Format("{{{0}}}", X);
    }
}

struct S2
{
    public int X;
    public S1 A;
    public S1 B;

    public override string ToString()
    {
        return string.Format("X: {0}, A: {1}, B: {2}", X, A, B);
    }
}

internal class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        S2 s = new S2 {X = 10, A = new S1 {X = 20}, B = new S1 {X = 30}};
        S2 s2 = s;
        s2.X += 10;
        s2.A.X += 10;
        s2.B.X += 10;
        Console.WriteLine(s);
        Console.WriteLine(s2);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}