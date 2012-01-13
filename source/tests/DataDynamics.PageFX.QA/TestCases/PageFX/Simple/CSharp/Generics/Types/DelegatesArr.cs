using System;

class Test
{
    public delegate T Function<T1, T>(T1 arg);

    public enum Op
    {
        Add, Subtract, Multiply, Divide
    }

    public static Function<double, double>[] CreateCalculator()
    {
        double value = 0;
        return new Function<double, double>[] {
                delegate (double arg) { value += arg; return value; },
                delegate (double arg) { value -= arg; return value; },
                delegate (double arg) { value *= arg; return value; },
                delegate (double arg) { value /= arg; return value; },
            };
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        Function<double, double>[] calc = CreateCalculator();
        calc[(int)Op.Add](3);
        calc[(int)Op.Multiply](7);
        calc[(int)Op.Subtract](1);
        double result = calc[(int)Op.Divide](4);
        Console.WriteLine(result);
    }

    static void Main(string[] args)
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}