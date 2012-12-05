using System;
using System.Collections;
using System.Collections.Generic;

interface IItem
{
    string Name { get; set; }
}

class Item : IItem
{
    public string Name { get; set; }
}

interface IItemCollection : ICollection
{
    IItem this[int index] { get; }
}

class MyList : List<IItem>, IItemCollection
{
}

class X
{
    static void Test1()
    {
        var list = new MyList();
        list.Add(new Item { Name = "A" });
        list.Add(new Item { Name = "B" });
        Console.WriteLine(list[0].Name);
        Console.WriteLine(list[1].Name);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}