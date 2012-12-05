using System;

public class Moo<C>
    where C : Moo<C>.Foo
{
    public class Foo
    { }
}

public class Test : Moo<Test>.Foo
{
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Moo<Test> moo = new Moo<Test>();
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
