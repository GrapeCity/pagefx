using System;

public class Bar<U> where U : EventArgs
{
    internal void OnWorldDestroyed()
    {
    }
}

public class Baz<U> where U : Bar<EventArgs>
{
    public void DestroyWorld(U bar)
    {
        bar.OnWorldDestroyed();
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Baz<Bar<EventArgs>> baz = new Baz<Bar<EventArgs>>();
        baz.DestroyWorld(new Bar<EventArgs>());
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
