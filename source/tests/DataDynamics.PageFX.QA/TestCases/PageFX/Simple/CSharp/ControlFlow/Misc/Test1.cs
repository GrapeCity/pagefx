using System;

class Test1
{
    static void Write(char[] buffer, int index, int count)
    {
        if (buffer == null)
            throw new ArgumentNullException("buffer");
        if (index < 0 || index > buffer.Length)
            throw new ArgumentOutOfRangeException("index");
        // re-ordered to avoid possible integer overflow
        if (count < 0 || (index > buffer.Length - count))
            throw new ArgumentOutOfRangeException("count");

        for (; count > 0; --count, ++index)
        {
            WriteChar(buffer[index]);
        }
    }

    static void WriteChar(char ch)
    {
        Console.WriteLine(ch);
    }

    static void Main()
    {
        string s = "aaabbb";
        Write(s.ToCharArray(), 1, 3);
        Console.WriteLine("<%END%>");
    }
}