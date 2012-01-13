using System;
using System.ComponentModel;

class CtorTest
{
    [Description("Hello, this is a test for custom attributes.")]
    class A
    {
        
    }

    class AAttribute : Attribute
    {
        public string FirstName;
        public string SecondName { get; set; }
        //AAttribute(string name)
        //{
        //    Name = name;
        //}
    }
    
    [A(FirstName="John", SecondName="Smith")]
    class B
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

    static void Test2()
    {
        var b = new B();
        Console.WriteLine("----Test2");
        var attrs = b.GetType().GetCustomAttributes(false);
        foreach (var o in attrs)
        {
            var a = o as AAttribute;
            if (a != null)
                Console.WriteLine(a.FirstName + " " + a.SecondName);
        }
    }
    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}