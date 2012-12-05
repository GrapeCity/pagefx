using System;

class A
{
    private readonly int[] arr = new int[3] { 10, 20, 30 };

    public int this[int index]
    {
        get { return arr[index]; }
        set { arr[index] = value; }
    }
}

class Indexer1
{
    static void Main()
    {
        A obj = new A();
        Console.WriteLine(obj[0]);
        Console.WriteLine(obj[1]);
        Console.WriteLine(obj[2]);
        Console.WriteLine("<%END%>");
    }
}