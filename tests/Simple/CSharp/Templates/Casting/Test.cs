static void TestAs(object a)
{
    Console.WriteLine("-- as");
    try
    {
        var b = a as T;
        Console.WriteLine(b != null);
        Console.WriteLine(ReferenceEquals(a, b));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestCast(object a)
{
    Console.WriteLine("-- cast");
    try
    {
        var b = (T)a;
        Console.WriteLine(ReferenceEquals(a, b));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestIs(object a)
{
    Console.WriteLine("-- is");
    try
    {
        Console.WriteLine(a is T);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestCore(object a)
{
    TestAs(a);
    TestCast(a);
    TestIs(a);
}