using System;
using System.Collections.Generic;

namespace gtest399
{
	class Base
	{
	}

	class Derived : Base
	{
	}

	class Program
	{
        static object CreateDerivedArray()
        {
            return (new Base[] { });
        }

        static void Main ()
		{
            try
            {
                try
                {
                    IEnumerable<Derived> e1 = (IEnumerable<Derived>)(CreateDerivedArray());
                    Console.WriteLine("error");
                }
                catch (InvalidCastException)
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
