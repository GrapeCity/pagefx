using System;

class BigSwitch1
{
    static void f(int n)
    {
        Console.WriteLine("f begin");
        switch (n)
        {
            case 1:
            case 2:
            case 3:
                Console.WriteLine("1 - 3");
                break;

            case 10:
            case 11:
            case 12:
                Console.WriteLine("10 - 12");
                break;

            case 20:
            case 21:
            case 22:
                Console.WriteLine("20 - 22");
                break;

            case 50:
            case 51:
            case 52:
                Console.WriteLine("50 - 52");
                break;

            case 100:
            case 101:
            case 102:
                Console.WriteLine("100 - 102");
                break;

            case 120:
            case 121:
            case 122:
                Console.WriteLine("120 - 122");
                break;

            case 150:
            case 151:
            case 152:
                Console.WriteLine("150 - 152");
                break;

            case 1000:
            case 1001:
            case 1002:
                Console.WriteLine("1000 - 1002");
                break;

            default:
                Console.WriteLine("default");
                break;
        }
        Console.WriteLine("f end");
    }

    static void Main()
    {
        for (int i = 0; i < 1005; i += 2)
            f(i);
        Console.WriteLine("<%END%>");
    }
}