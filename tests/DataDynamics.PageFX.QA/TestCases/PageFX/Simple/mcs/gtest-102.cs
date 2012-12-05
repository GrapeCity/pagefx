using System;

class A<T>
    where T : IComparable
{
}

class B<U, V>
    where U : IComparable
    where V : A<U>
{
}

class Driver
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        A<int> a_int;
        B<int, A<int>> b_stuff;
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
