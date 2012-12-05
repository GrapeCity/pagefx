static void print(T?[] set)
{
    foreach (var item in set)
        Console.WriteLine(item);
}

static void TestAs(object a)
{
    Console.WriteLine("-- as");
    try
    {
        var b = a as T?[];
        print(b);
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
        Console.WriteLine(a is T?[]);
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
        var b = (T?[])a;
        print(b);
        Console.WriteLine(ReferenceEquals(a, b));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestCore(object a)
{
    TestAs(a);
    TestIs(a);
    TestCast(a);
}