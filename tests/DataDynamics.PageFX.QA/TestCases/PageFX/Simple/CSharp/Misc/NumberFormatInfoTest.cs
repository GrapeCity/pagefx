using System;
using System.Globalization;

class X
{
    static void Main()
    {
        Console.WriteLine("Hello!");
        Console.WriteLine(NumberFormatInfo.InvariantInfo.CurrencyNegativePattern);
        Console.WriteLine("<%END%>");
    }
}