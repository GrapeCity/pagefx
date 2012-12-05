using System;

class A
{
    public string name;
    public int id;
}

class Program
{
    static void Main()
    {
        A obj = new A();
        obj.name = "aaa";
        obj.id = 10;
        Console.WriteLine(obj.name);
        Console.WriteLine(obj.id);
        Console.WriteLine("<%END%>");
    }
}