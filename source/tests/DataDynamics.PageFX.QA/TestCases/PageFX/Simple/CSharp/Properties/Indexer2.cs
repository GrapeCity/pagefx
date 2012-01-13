using System;

class A
{
    private readonly int[] _arr1 = new int[10];
    
    public int this[int index]
    {
        get { return _arr1[index]; }
        set { _arr1[index] = value; }
    }

    public int this[string name]
    {
        get { return _arr1[0]; }
    }
}

class Indexer2
{
    static void Main()
    {
        A obj = new A();
        Console.WriteLine(obj[0]);
        Console.WriteLine(obj["aaa"]);
        Console.WriteLine("<%END%>");
    }
}