using System;

/// <summary>
/// When an interface inherits from another and a method of the base interface is called
/// The verifier inside AVM2 doesn't recognize it and the call result becomes untyped (*) which
/// causes verification failures. This is a test that reproduces it. 
/// IL translator was fixed to coerce to inherited interface before making the call.
/// </summary>
class InterfaceInheritance
{
    static void Main()
    {
        ISpecializedInterface interfacePtr = GetInterface();
        bool val; // initialize local so type mismatch is caught (*,int)
        val = interfacePtr.GetValue1();
        //if (val != false)
        //    throw new Exception("Interface call2 failed");
        //val = ((IBaseInterface)interfacePtr).GetValue1();
        //if (val != false)
        //    throw new Exception("Interface call failed");
        Console.WriteLine(val);
        Console.WriteLine("<%END%>");
    }

    static ISpecializedInterface GetInterface()
    {
        Impl instance = new Impl();
        return instance;
    }

}

interface IBaseInterface
{
    bool GetValue1();
}

interface ISpecializedInterface : IBaseInterface
{
    bool GetValue2();
}

class Impl : ISpecializedInterface
{
    bool ISpecializedInterface.GetValue2()
    {
        return true;
    }

    bool IBaseInterface.GetValue1()
    {
        return false;
    }
}