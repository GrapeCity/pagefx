using System;
using System.Collections;

interface I1
{
}

interface I2 : I1
{
}

class A : I1
{
}

class B : A, I2
{
}

class X
{
    static void Test1()
    {
        Console.WriteLine("--- Test1: ArrayList.ToArray");
        try
        {
            ArrayList list = new ArrayList();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            string[] arr = (string[])list.ToArray(typeof(string));
            for (int i = 0; i < arr.Length; ++i)
                Console.WriteLine(arr[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2: string[] to object[]");
        try
        {
            string[] arr1 = new string[] { "a", "b", "c" };
            object[] arr2 = ToObjectArray(arr1);
            for (int i = 0; i < arr2.Length; ++i)
                Console.WriteLine(arr2[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static object[] ToObjectArray(Array arr)
    {
        return (object[])arr;
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3: int[] to object[]");
        try
        {
            int[] arr1 = new int[] { 10, 20, 30 };
            object[] arr2 = ToObjectArray(arr1);
            for (int i = 0; i < arr2.Length; ++i)
                Console.WriteLine(arr2[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4: B[] to A[]");
        try
        {
            B[] arr1 = new B[] { new B(), new B(), new B() };
            A[] arr2 = (A[])arr1;
            for (int i = 0; i < arr2.Length; ++i)
                Console.WriteLine(arr2[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test5()
    {
        Console.WriteLine("--- Test5: B[] to I1[]");
        try
        {
            B[] arr1 = new B[] { new B(), new B(), new B() };
            I1[] arr2 = (I1[])arr1;
            for (int i = 0; i < arr2.Length; ++i)
                Console.WriteLine(arr2[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test6()
    {
        Console.WriteLine("--- Test5: I2[] to I1[]");
        try
        {
            I2[] arr1 = new I2[] { new B(), new B(), new B() };
            I1[] arr2 = (I1[])arr1;
            for (int i = 0; i < arr2.Length; ++i)
                Console.WriteLine(arr2[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Console.WriteLine("<%END%>");
    }
}