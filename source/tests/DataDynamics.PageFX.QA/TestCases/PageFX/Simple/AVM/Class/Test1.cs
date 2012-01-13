using System;

namespace Utils
{
    public class Msg
    {
        public Msg()
        {
        }

        public string value;
    }
}

class Test
{
    static void Test1()
    {
        Avm.Class klass = Avm.Class.Find("", "Object");
        Avm.Object obj = (Avm.Object)klass.CreateInstance();
        obj.SetProperty("a", "1");
        Console.WriteLine(obj.GetProperty("a"));
    }

    static void Test2()
    {
        Avm.Class klass = Avm.Class.Find("Utils", "Msg");
        Utils.Msg msg = (Utils.Msg)klass.CreateInstance();
        msg.value = "aaa";
        Console.WriteLine(msg.value);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}