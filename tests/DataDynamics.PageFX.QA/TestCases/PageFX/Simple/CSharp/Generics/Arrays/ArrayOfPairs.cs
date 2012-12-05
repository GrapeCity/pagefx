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
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Pair<int,int>[] arr = new Pair<int, int>[5];
        for (int i = 0; i < arr.Length; ++i)
        {
            Console.WriteLine(arr[i].First);
            Console.WriteLine(arr[i].Second);
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}