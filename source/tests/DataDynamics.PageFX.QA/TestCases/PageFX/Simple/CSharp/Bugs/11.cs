using System;
using System.IO;
using System.Text;

class Test
{
    static void Main()
    {
        Console.WriteLine("Hello!");

        byte[] buffer = new byte[] { 67, 0, 111, 0, 117, 0, 110, 0, 116, 0, 114, 0, 121, 0 };

        Console.WriteLine("Memory length: " + buffer.Length);
        MemoryStream stream = new MemoryStream(buffer);

        BinaryReader reader = new BinaryReader(stream, Encoding.Unicode);
        Console.WriteLine("Position before reading " + reader.BaseStream.Position);
        char[] chars = reader.ReadChars(7);
        Console.WriteLine("Position after reading " + reader.BaseStream.Position);

        if (reader.BaseStream.Position < buffer.Length)
        {
            reader.ReadByte();
            Console.WriteLine("We can read a byte more. Pos=" + reader.BaseStream.Position);
        }

        Console.WriteLine("<%END%>");
    }
}