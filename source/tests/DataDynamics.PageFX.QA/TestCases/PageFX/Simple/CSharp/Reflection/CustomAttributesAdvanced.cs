using System;
using System.ComponentModel;

enum EInt64 : long { A = -1, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P };

class CtorTest
{
    abstract class AAAttribute : Attribute
    {
        public int[] bar;
    }

    class AAttribute : AAAttribute
    {
        public int[] arr;
    }
        
    class BBAttribute : Attribute
    {
        public string msg;
    }

    [BB(msg = "Hello, this a custom attribute test. Have a nice day! =)")]
    class BB
    {
        
    }
    
    [Description("234234")]
    [A(arr = new int[] {1, 3, 5, 7}, bar = new int[] {2, 4, 6, 8})]
    class B : BB
    {
    }

    class BAttribute : Attribute
    {
        public EInt64[] en;
    }

    [B(en = new EInt64[] { EInt64.A, EInt64.B, EInt64.C })]
    class C
    {
    }

    class D
    {
        [A(arr = new int[] { 1, 2, 3 })]
        public void Foo()
        {
            Console.WriteLine("Foo");
        }

    
    }

    static void Test1()
    {
        var b = new B();
        Console.WriteLine("----Test1");
        var attrs = b.GetType().GetCustomAttributes(false);
        foreach (var o in attrs)
        {
            var a = o as AAttribute;
            if (a != null)
            { 
                foreach (var item in a.arr)
                    Console.WriteLine(item);
            }
        }
    }

    static void Test2()
    {
        var c = new C();
        Console.WriteLine("----Test2");
        var attrs = c.GetType().GetCustomAttributes(false);
        foreach (var o in attrs)
        {
            var a = o as BAttribute;
            if (a != null)
            {
                for (int i = 0; i < a.en.Length; ++i)
                    Console.WriteLine(a.en[i]);
            }
        }
    }

    static void Test3()
    {
        var d = new D();
        d.Foo();
        Console.WriteLine("----Test3");
        var method = d.GetType().GetMethod("Foo");
        Console.WriteLine(method.Name);
        //foreach (var o in attrs)
        //{
        //    var a = o as AAttribute;
        //    if (a != null)
        //    {
        //        for (int i = 0; i < a.arr.Length; ++i)
        //            Console.WriteLine(a.arr[i]);
        //    }
        //}
    }

    static void Test4()
    {
        var b = new B();
        Console.WriteLine("----Test4");
        var attrs = b.GetType().GetCustomAttributes(true);
        foreach (var o in attrs)
        {
            var a = o as AAttribute;
            if (a != null)
            {
                foreach (var item in a.arr)
                    Console.WriteLine(item);
                foreach (var item in a.bar)
                    Console.WriteLine(item);
            }
        }
    }

    static void Test5()
    {
        var b = new B();
        Console.WriteLine("----Test5");
        bool res = b.GetType().IsDefined(typeof(BBAttribute), true);
        Console.WriteLine(res);
        if (res)
        {
            foreach (var attr in  b.GetType().GetCustomAttributes(true))
            {
                Console.WriteLine(attr);
                var bbattr = attr as BBAttribute;
                if (bbattr != null)
                    Console.WriteLine(bbattr.msg);
            }
        }
    }

    static void Test6()
    {
        var b = new B();
        Console.WriteLine("----Test6");
        bool res = b.GetType().IsDefined(typeof(BBAttribute), true);
        Console.WriteLine(res);
        if (res)
        {
            foreach (var attr in b.GetType().GetCustomAttributes(typeof(BBAttribute), true))
            {
                Console.WriteLine(attr);
                var bbattr = attr as BBAttribute;
                Console.WriteLine(bbattr.msg);
            }
        }
    }

    static void Test7()
    {
        var b = new B();
        Console.WriteLine("----Test7");
        bool res = b.GetType().IsDefined(typeof(BBAttribute), true);
        Console.WriteLine(res);
        if (res)
        {
            Console.WriteLine(b.GetType().IsDefined(typeof(BBAttribute), true));
        }
    }

    static void Main()
    {
        //Test1();
        //Test2();
        Test3();
        //Test4();
        //Test5();
        //Test6();
        //Test7();
        Console.WriteLine("<%END%>");
    }
}