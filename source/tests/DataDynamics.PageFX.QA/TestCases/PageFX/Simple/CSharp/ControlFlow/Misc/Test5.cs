using System;

class Test5
{
    static void f(int precision, int DefaultPrecision, bool IsDecimalSource, int IntegerDigits, int DecimalPointPosition)
    {
        precision = precision > 0 ? precision : DefaultPrecision;

        int exponent = 0;
        bool expMode = (IsDecimalSource && precision == DefaultPrecision 
            ? false : (IntegerDigits > precision || DecimalPointPosition <= -4));

        if (expMode)
        {
            Console.WriteLine("1");
        }
        else
        {
            Console.WriteLine("0");
        }

        Console.WriteLine(exponent);
    }

    static void Main()
    {
        f(-1, 10, false, 2, 2);
        Console.WriteLine("<%END%>");
    }
}