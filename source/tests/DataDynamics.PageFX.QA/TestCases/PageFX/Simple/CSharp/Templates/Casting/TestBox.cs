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

static void TestUnbox(object a)
{
    Console.WriteLine("-- unbox");
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

static void TestCore(object a)
{
    TestUnbox(a);
    TestIs(a);
}