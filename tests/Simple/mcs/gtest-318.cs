using System;

class Foo<K>
{
}

class Bar<Q> : Goo<Q>
{
    public override string ToString()
    {
        return string.Format("Bar<{0}>", typeof(Q));
    }
    public class Baz
    {
    }
}

class Goo<Q> : Foo<Bar<Q>.Baz>
{
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Bar<int> bar = new Bar<int>();
        Console.WriteLine(bar);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}

