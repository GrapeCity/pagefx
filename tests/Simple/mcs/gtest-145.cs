using System;

public class Test<T>
{
    private T[,] data;
    public Test(T[,] data)
    {
        this.data = data;
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Test<double> test = new Test<double>(new double[2, 2]);   
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}

