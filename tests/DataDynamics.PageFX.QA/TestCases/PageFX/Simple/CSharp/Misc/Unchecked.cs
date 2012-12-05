using System;

class TestClass
{
    const int x = 2147483647;   // Max int 
    const int y = 2;

    static void Main()
    {
        int z;
        unchecked
        {
            z = x * y;
        }
        Console.WriteLine("unchecked({0} * {1}) = {2}", x, y, z);
        Console.WriteLine("<%END%>");
    }
}