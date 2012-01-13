using System;

enum E1 { A, B, C, D }

class Test
{
    static void UnboxInt(object obj)
    {
        try
        {
            int v = (int)obj;
            Console.WriteLine(v);
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().FullName);
        }
    }
        
    static void Test1()
    {
        UnboxInt(10);
        UnboxInt(null);
        UnboxInt(new Test());
        UnboxInt("10");
        UnboxInt(E1.D);
    }

    public static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}