using System;

using T = System.Single;

class X
{
    private static readonly decimal[] nums =
        {
            0m,
            1m,
            100m,
            decimal.MinusOne,
            decimal.MinValue,
            decimal.MaxValue,
            1.2m,
            3.14m,
        };

    static void Main()
    {
        for (int i = 0; i < nums.Length; ++i)
        {
            decimal d = nums[i];
            Console.WriteLine(d);
            try
            {
                T v = (T)d;
                Console.WriteLine(v);
            }
            catch (OverflowException)
            {
                Console.WriteLine("overflow");
            }
        }
        Console.WriteLine("<%END%>");
    }
}