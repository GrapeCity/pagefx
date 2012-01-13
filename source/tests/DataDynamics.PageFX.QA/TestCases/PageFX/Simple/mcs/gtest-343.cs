using System;

interface Interface<TImplementer>
  where TImplementer : Interface<TImplementer>
{
    void Combine<TOImplementer>()
      where TOImplementer : Interface<TOImplementer>;
}

class Implementer : Interface<Implementer>
{
    public void Combine<TOImplementer>()
      where TOImplementer : Interface<TOImplementer>
    {
    }
}

class MainClass
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Implementer i = new Implementer();
        i.Combine<Implementer>();
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
