using System;

public interface I1
{
}

public interface I2 : I1
{
}

public interface I3
{
}

public interface I4
{
}

public class A : I1
{
}

public class B : A, I2
{
}

class C : B, I3, I4
{
}

enum E { A, B, C, D }

class X
{
    static void TestGetTypeCode()
    {
        Console.WriteLine("--- TestGetTypeCode");
        Console.WriteLine(Type.GetTypeCode(typeof(int)));
        Console.WriteLine(Type.GetTypeCode(typeof(uint)));
        Console.WriteLine(Type.GetTypeCode(typeof(string)));
        Console.WriteLine(Type.GetTypeCode(typeof(object)));
        Console.WriteLine(Type.GetTypeCode(typeof(int[])));
        Console.WriteLine(Type.GetTypeCode(typeof(E)));
    }

    static void PrintTypeInfo(Type type)
    {
        Console.WriteLine(type.FullName);
        Console.WriteLine("BaseType: {0}", type.BaseType);
        Console.WriteLine("IsClass: {0}", type.IsClass);
        Console.WriteLine("IsInterface: {0}", type.IsInterface);
        Console.WriteLine("IsValueType: {0}", type.IsValueType);
        Console.WriteLine("IsPrimitive: {0}", type.IsPrimitive);
        Console.WriteLine("IsArray: {0}", type.IsArray);
        Console.WriteLine("HasElementType: {0}", type.HasElementType);
    }

    static void PrintTypeInfo()
    {
        Console.WriteLine("--- PrintTypeInfo");
        PrintTypeInfo(typeof(int));
        PrintTypeInfo(typeof(string));
        PrintTypeInfo(typeof(object));
        PrintTypeInfo(typeof(int[]));
    }

    static void TestBooleanGetType()
    {
        Console.WriteLine("--- TestBooleanGetType");
        bool t = true, f = false;
        Console.WriteLine(Object.ReferenceEquals(t.GetType(), f.GetType()));
    }

    static void TestBaseType(Type type)
    {
        Console.WriteLine("{0} -> {1}", type, type.BaseType);
    }

    static void TestBaseType()
    {
        Console.WriteLine("--- TestBaseType");
        TestBaseType(typeof(object));
        TestBaseType(typeof(string));
        TestBaseType(typeof(int));
        TestBaseType(typeof(int[]));

        Type type = typeof(int);
        while (type != null)
        {
            Console.WriteLine(type);
            type = type.BaseType;
        }
    }

    static void TestIsAssignableFrom(Type type)
    {
        Console.WriteLine(type.FullName);
        Console.WriteLine("from object: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(object)));
        Console.WriteLine("from string: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(string)));
        Console.WriteLine("from int: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(int)));
        Console.WriteLine("from ICloneable: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(ICloneable)));
        Console.WriteLine("from IConvertible: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(IConvertible)));
        Console.WriteLine("from A: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(A)));
        Console.WriteLine("from B: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(B)));
        Console.WriteLine("from I1: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(I1)));
        Console.WriteLine("from I2: ");
        Console.WriteLine(type.IsAssignableFrom(typeof(I2)));
    }

    static void TestIsAssignableFrom()
    {
        Console.WriteLine("--- TestIsAssignableFrom");
        TestIsAssignableFrom(typeof(object));
        TestIsAssignableFrom(typeof(string));
        TestIsAssignableFrom(typeof(int));
        TestIsAssignableFrom(typeof(ICloneable));
        TestIsAssignableFrom(typeof(IConvertible));
        TestIsAssignableFrom(typeof(A));
        TestIsAssignableFrom(typeof(B));
        TestIsAssignableFrom(typeof(I1));
        TestIsAssignableFrom(typeof(I2));
    }

    static void TestGetInterfaces(Type type)
    {
        Console.WriteLine("{0} interfaces: ", type);
        foreach (Type iface in type.GetInterfaces())
        {
            Console.WriteLine(iface);
        }
    }

    static void TestGetInterfaces()
    {
        Console.WriteLine("--- TestGetInterfaces");
        TestGetInterfaces(typeof(object));
        TestGetInterfaces(typeof(A));
        TestGetInterfaces(typeof(B));
        TestGetInterfaces(typeof(C));
    }

    static void TestIsPrimitive()
    {
        Console.WriteLine("--- TestIsPrimitive");
        Console.WriteLine(typeof(int).IsPrimitive);
        Console.WriteLine(typeof(string).IsPrimitive);
        Console.WriteLine(typeof(E).IsPrimitive);
    }

    static void Main()
    {
        TestGetTypeCode();
        PrintTypeInfo();
        TestBooleanGetType();
        TestBaseType();
        TestIsAssignableFrom();
        TestGetInterfaces();
        TestIsPrimitive();
        Console.WriteLine("<%END%>");
    }
}