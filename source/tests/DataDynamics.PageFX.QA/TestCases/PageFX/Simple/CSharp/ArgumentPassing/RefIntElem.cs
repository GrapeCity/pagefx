using System;

class X
{
    static void f(ref int a)
    {
        Console.WriteLine(a);
        a = a + 1;
        Console.WriteLine(a);
    }

    //Note: A property or indexer may not be passed as an out or ref parameter
    //static int Value
    //{
    //    get { return _value; }
    //    set { _value = value; }
    //}
    //private static int _value;

    static void Main()
    {
        //f(ref Value);
        //f(ref Value);

        int[] arr = new int[] { 10, 20, 30 };
        f(ref arr[0]);
        f(ref arr[1]);
        f(ref arr[2]);
        Console.WriteLine("<%END%>");
    }
}