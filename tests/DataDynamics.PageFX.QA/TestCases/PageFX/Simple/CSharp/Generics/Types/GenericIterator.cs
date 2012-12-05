using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    public class Stack<T> : IEnumerable<T>
    {
        private T[] values = new T[100];
        private int top = 0;

        public void Push(T t) { values[top++] = t; }
        public T Pop() { return values[--top]; }

        // These make Stack<T> implement IEnumerable<T> allowing
        // a stack to be used in a foreach statement.
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = top; --i >= 0; )
            {
                yield return values[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Iterate from top to bottom.
        public IEnumerable<T> TopToBottom
        {
            get
            {
                // Since we implement IEnumerable<T>
                // and the default iteration is top to bottom,
                // just return the object.
                return this;
            }
        }

        // Iterate from bottom to top.
        public IEnumerable<T> BottomToTop
        {
            get
            {
                for (int i = 0; i < top; i++)
                {
                    yield return values[i];
                }
            }
        }

        //A parameterized iterator that return n items from the top
        public IEnumerable<T> TopN(int n)
        {
            // in this example we return less than N if necessary 
            int j = n >= top ? 0 : top - n;

            for (int i = top; --i >= j; )
            {
                yield return values[i];
            }
        }
    }


    static void Test1()
    {
        Console.WriteLine("------Test1");
        Stack<string> s = new Stack<string>();
        for (int i = 0; i < 10; i++)
        {
            s.Push(i.ToString());
        }

        // Prints: 9 8 7 6 5 4 3 2 1 0
        // Foreach legal since s implements IEnumerable<int>
        foreach (string n in s)
        {
            System.Console.WriteLine(n);
        }
        System.Console.WriteLine("----------------------");

        // Prints: 9 8 7 6 5 4 3 2 1 0
        // Foreach legal since s.TopToBottom returns IEnumerable<int>
        foreach (string n in s.TopToBottom)
        {
            System.Console.WriteLine(n);
        }
        System.Console.WriteLine("----------------------");

        // Prints: 0 1 2 3 4 5 6 7 8 9
        // Foreach legal since s.BottomToTop returns IEnumerable<int>
        foreach (string n in s.BottomToTop)
        {
            System.Console.WriteLine(n);
        }
        System.Console.WriteLine("----------------------");

        // Prints: 9 8 7 6 5 4 3
        // Foreach legal since s.TopN returns IEnumerable<int>
        foreach (string n in s.TopN(7))
        {
            System.Console.WriteLine(n);
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
