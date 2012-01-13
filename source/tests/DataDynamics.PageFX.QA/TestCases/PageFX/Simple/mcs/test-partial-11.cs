using System;
using System.Reflection;

static partial class StaticClass
{
    public static string Name ()
    {
        return "OK";
    }
}

partial class StaticClass2 {}
static partial class StaticClass2 {}

	
public class MainClass
{
	static bool IsStatic (Type t)
	{
		Type type = typeof (StaticClass);
		if (!type.IsAbstract || !type.IsSealed) {
			Console.WriteLine ("Is not abstract sealed");
			return false;
		}
        
		if (type.GetConstructors ().Length > 0) {
			Console.WriteLine ("Has constructor");
			return false;
		}
		return true;
	}

    public static void Main ()
    {
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
    }

    private static int Test1()
    {
        Console.WriteLine("----Test1");
        if (!IsStatic (typeof (StaticClass)))
            return 1;

        if (!IsStatic (typeof (StaticClass2)))
            return 2;
        
        Console.WriteLine ("OK");
        return 0;
    }
}