using System;

public delegate V Mapper<T, V>(T item);

public interface ITree<T>
{
    void Map<V>(Mapper<T, V> mapper);
}

public class Tree<T> : ITree<T>
{
    T item;

    public Tree(T item)
    {
        this.item = item;
    }

    public void Map<V>(Mapper<T, V> mapper)
    {
        V new_item = mapper(item);
    }
}

class X
{
    private string themap(int i)
    {
        return String.Format("AA {0,4} BB", i);
    }

    void Test()
    {
        Tree<int> tree = new Tree<int>(3);
        tree.Map(new Mapper<int, string>(themap));
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        X x = new X();
        x.Test();
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
