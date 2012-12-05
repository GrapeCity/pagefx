using System;

interface a<t> { void x(); }

interface b<t> : a<t> { }

class kv<k, v> { } // type t

interface c<k, v> : b<kv<k, v>>,  // b <t>
                   a<kv<k, v>>    // a <t>
{ }

class m<k, v> : c<k, v>,
                b<kv<k, v>> // b <t>
{
    void a<kv<k, v>>.x()
    {
        Console.WriteLine("ok");
    } // a<t>.x ()
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");   
        m<double, string> m = new m<double, string>();
        (m as a<kv<double, string>>).x();
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
