using System;

class Ver
{
    public int Major, Minor, Build, Revision;

    public Ver()
    {
    }

    public Ver(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }

    public Ver(int major, int minor, int build, int revision)
    {
        Major = major;
        Minor = minor;
        Build = build;
        Revision = revision;
    }

    public bool Equals(Ver x)
    {
        return ((x != null) &&
            (x.Major == Major) &&
            (x.Minor == Minor) &&
            (x.Build == Build) &&
            (x.Revision == Revision));
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Ver);
    }
}

class Test2
{
    static void f(Ver x, Ver y)
    {
        bool res = x.Equals(y);
        Console.WriteLine(res);
    }

    static void Main()
    {
        f(new Ver(1, 0), new Ver(2, 1));
        f(new Ver(1, 0, 2, 3), new Ver(1, 0, 2, 4));
        Console.WriteLine("<%END%>");
    }
}