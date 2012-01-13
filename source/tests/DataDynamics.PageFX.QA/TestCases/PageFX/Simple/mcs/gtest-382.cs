using System;

namespace gtest383
{
    class C
    {
        public static void Main()
        {
            try
            {
                int? i = 1;
                try
                {
                    i = checked(int.MaxValue + i);
                    Console.WriteLine("error");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("ok");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
            Console.WriteLine("<%END%>");
        }
    }
}
