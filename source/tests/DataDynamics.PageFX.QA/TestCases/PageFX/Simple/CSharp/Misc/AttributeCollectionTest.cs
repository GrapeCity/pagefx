using System;
using System.Collections;
using System.ComponentModel;

class X
{
    static void Main()
    {
        AttributeCollection ac = new AttributeCollection(null);
        ICollection ic = (ICollection)ac;
        Console.WriteLine(ac.Count);
        Console.WriteLine(ic.Count);
        Console.WriteLine("<%END%>");
    }
}