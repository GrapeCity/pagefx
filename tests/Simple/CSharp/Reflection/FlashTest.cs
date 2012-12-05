using System;

class X
{
    static void Main()
    {
        Avm.Object obj = new Avm.Object();
        string s = flash.utils.Global.getQualifiedClassName(obj);
        Console.WriteLine(s);
    }
}