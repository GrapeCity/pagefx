using System;
using System.ComponentModel;

class CtorTest
{
    [Description("Hello, this is a test for custom attributes.")]
    class A
    {
        
    }

    static void Test1()
    {
        var a = new A();
        Console.WriteLine("----Test1");
        var attrs = a.GetType().GetCustomAttributes(false);
        foreach (var o in attrs)
        {
            var desc = o as DescriptionAttribute;
            if (desc != null)
                Console.WriteLine(desc.Description);
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}