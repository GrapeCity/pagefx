using System;
using System.Text;


class Test7
{
    private static bool HasDecimal(string digits, int decPointPos)
    {
        return digits.Length > decPointPos;
    }

    private static void append(char c, int n)
    {
        for (; n > 0; --n)
            Console.WriteLine(c);
    }

    public static void f(int precision, string digits, int decPointPos)
    {
        if (!HasDecimal(digits, decPointPos))
        {
            append('0', precision);
            return;
        }

        int i = decPointPos;
        for (; i < digits.Length && i < precision + decPointPos; i++)
        {
            if (i >= 0)
            {
                Console.WriteLine(digits[i]);
            }
            else
            {
                Console.WriteLine('0');
            }
        }

        i -= decPointPos;
        if (i < precision)
        {
            append('0', precision - i);
        }
    }

    static void Main()
    {
        f(10, "3.14", 1);
        f(10, "314", 3);
        Console.WriteLine("<%END%>");
    }
}