using System;

class A
{
}

class B : A
{
}

class Test
{
    static void Main()
    {
        object obj = new A();
        if (obj is B)
            Console.WriteLine("B");
        else if (obj is A)
            Console.WriteLine("A");
        obj = "str";
        obj = obj as A;
        Console.WriteLine(obj != null ? "A" : "null");
        Console.WriteLine("<%END%>");
    }
}