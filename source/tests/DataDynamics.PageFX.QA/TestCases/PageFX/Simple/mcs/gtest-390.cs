using System;

namespace gtest390
{
    class Program
    {
        static void Main()
        {
            Error error = Error.FILE_NOT_FOUND;
            Console.WriteLine((error == null) ? 1 : 0);
            Console.WriteLine("<%END%>");
        }
    }

    enum Error
    {
        FILE_NOT_FOUND
    }


}
