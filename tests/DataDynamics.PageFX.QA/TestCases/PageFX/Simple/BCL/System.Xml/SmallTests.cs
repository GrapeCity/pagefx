using System;
using System.Xml;

class X
{
    class Reader
    {
        private bool allowMultipleRoot;
        private XmlNodeType currentState;

        void VerifyXmlDeclaration()
        {
            if (!allowMultipleRoot && currentState != XmlNodeType.None)
            {
                Console.WriteLine(currentState);
                //throw NotWFError("XML declaration cannot appear in this state.");
            }
        }

        public void Read()
        {
            allowMultipleRoot = false;
            currentState = XmlNodeType.None;

            VerifyXmlDeclaration();
        }
    }

    static void Test1()
    {
        Reader reader = new Reader();
        reader.Read();
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}