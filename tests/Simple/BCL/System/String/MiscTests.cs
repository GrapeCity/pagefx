using System;

namespace PageFX.Tests
{
    class StringTest
    {
        static void Main()
        {
            TestMisc1();
            TestMisc2();
            TestIndexOf();
            TestCopyTo();
            TestSplit();
            TestReplace();
            TestFormat();
            TestConcat();
            Console.WriteLine("<%END%>");
        }

        static void TestConcat()
        {
            Console.WriteLine("kala " + 10);
            Console.WriteLine("kala " + 11);
            Console.WriteLine("kala " + 12);
        }

        static void TestMisc1()
        {
            string s = "aaabbb";
            s += s.Substring(0, 3);
            Console.WriteLine(s);
        }

        static void TestMisc2()
        {
            string s = "aaa";
            int len = s.Length;
            Console.WriteLine(len);
            Console.WriteLine(s[0]);

            string s1 = "aaa";
            string s2 = "bbb";
            string s3 = s1 + s2;
            Console.WriteLine(s3);

            Console.WriteLine(100.ToString("X"));
            Console.WriteLine(100.ToString("X4"));

            Console.WriteLine(s1 == s2);
            Console.WriteLine(s1.Equals(s2));

            char[] arr = new char[3] { 'A', 'B', 'C' };
            Console.WriteLine(new String(arr, 1, 2));

            Console.WriteLine((int)'A');
            Console.WriteLine((int)'a');
            Console.WriteLine(string.CompareOrdinal("A", "a"));
            Console.WriteLine(string.CompareOrdinal("a", "A"));
            Console.WriteLine(string.Compare("A", "a"));
            Console.WriteLine(string.Compare("a", "A"));

            s = "original";
            Console.WriteLine(s.IndexOf('a'));
            Console.WriteLine(s.IndexOf('b'));
            Console.WriteLine(s.IndexOf("in"));
            Console.WriteLine(s.IndexOf('l', s.Length));
        }

        static void TestIndexOf()
        {
            string s1 = "original";

            Console.WriteLine(s1.IndexOf('r')); //1
            Console.WriteLine(s1.IndexOf('i')); //2
            Console.WriteLine(s1.IndexOf('q')); //3

            Console.WriteLine(s1.IndexOf("rig")); //4
            Console.WriteLine(s1.IndexOf("i")); //5
            Console.WriteLine("".IndexOf("")); //6
            Console.WriteLine("ABC".IndexOf("")); //7
            Console.WriteLine(s1.IndexOf("rag")); //8

            Console.WriteLine(s1.IndexOf('r', 1)); //9
            Console.WriteLine(s1.IndexOf('i', 1)); //10
            Console.WriteLine(s1.IndexOf('i', 3)); //11
            Console.WriteLine(s1.IndexOf('i', 5)); //12
            Console.WriteLine(s1.IndexOf('l', s1.Length)); //13

            Console.WriteLine(s1.IndexOf('r', 1, 1)); //14
            Console.WriteLine(s1.IndexOf('r', 0, 1)); //15
            Console.WriteLine(s1.IndexOf('i', 1, 3)); //16
            Console.WriteLine(s1.IndexOf('i', 3, 3)); //17
            Console.WriteLine(s1.IndexOf('i', 5, 3)); //18

            s1 = "original original";
            Console.WriteLine(s1.IndexOf("original", 0)); //19
            Console.WriteLine(s1.IndexOf("original", 1)); //20
            Console.WriteLine(s1.IndexOf("original", 10)); //21
            Console.WriteLine(s1.IndexOf("", 3)); //22
            Console.WriteLine(s1.IndexOf("rig", 0, 5)); //23
            Console.WriteLine(s1.IndexOf("rig", 0, 3)); //24
            Console.WriteLine(s1.IndexOf("rig", 2, 15)); //25
            Console.WriteLine(s1.IndexOf("rig", 2, 3)); //26
            Console.WriteLine(s1.IndexOf("", 2, 3)); //27

            string s2 = "QBitArray::bitarr_data";
            Console.WriteLine(s2.IndexOf("::")); //28
        }

        static void TestCopyTo()
        {
            string s1 = "original";

            bool errorThrown = false;
            try
            {
                s1.CopyTo(0, (char[])null, 0, s1.Length);
            }
            catch (ArgumentNullException)
            {
                errorThrown = true;
            }

            int n = s1.Length;
            char[] c1 = new char[n];
            string s2 = new String(c1);
            Console.WriteLine(!s1.Equals(s2));
            for (int i = 0; i < n; i++)
            {
                s1.CopyTo(i, c1, i, 1);
            }
            s2 = new String(c1);
            Console.WriteLine(s1.Equals(s2));
        }

        static void TestSplit()
        {
            string s1 = "abcdefghijklm";
            char[] c1 = { 'q', 'r' };
            Console.WriteLine((s1.Split(c1))[0]); //1

            char[] c2 = { 'a', 'e', 'i', 'o', 'u' };
            string[] chunks = s1.Split(c2);
            Console.WriteLine(chunks[0]); //2
            Console.WriteLine(chunks[1]); //3
            Console.WriteLine(chunks[2]); //4
            Console.WriteLine(chunks[3]); //5

            {
                bool errorThrown = false;
                try
                {
                    chunks = s1.Split(c2, -1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    errorThrown = true;
                }
            }

            chunks = s1.Split(c2, 2);
            Console.WriteLine(chunks.Length); //6
            Console.WriteLine(chunks[0]); //7
            Console.WriteLine(chunks[1]); //8

            string s3 = "1.0";
            char[] c3 = { '.' };
            chunks = s3.Split(c3, 2);
            Console.WriteLine(chunks.Length); //9
            Console.WriteLine(chunks[0]); //10
            Console.WriteLine(chunks[1]); //11

            string s4 = "1.0.0";
            char[] c4 = { '.' };
            chunks = s4.Split(c4, 2);
            Console.WriteLine(chunks.Length); //12
            Console.WriteLine(chunks[0]); //13
            Console.WriteLine(chunks[1]); //14

            string s5 = ".0.0";
            char[] c5 = { '.' };
            chunks = s5.Split(c5, 2);
            Console.WriteLine(chunks.Length); //15
            Console.WriteLine(chunks[0]); //16
            Console.WriteLine(chunks[1]); //17

            string s6 = ".0";
            char[] c6 = { '.' };
            chunks = s6.Split(c6, 2);
            Console.WriteLine(chunks.Length); //18
            Console.WriteLine(chunks[0]); //19
            Console.WriteLine(chunks[1]); //20

            string s7 = "0.";
            char[] c7 = { '.' };
            chunks = s7.Split(c7, 2);
            Console.WriteLine(chunks.Length); //21
            Console.WriteLine(chunks[0]); //22
            Console.WriteLine(chunks[1]); //23

            string s8 = "0.0000";
            char[] c8 = { '.' };
            chunks = s8.Split(c8, 2);
            Console.WriteLine(chunks.Length); //24
            Console.WriteLine(chunks[0]); //25
            Console.WriteLine(chunks[1]); //26

            chunks = s8.Split(c8, 3);
            Console.WriteLine(chunks.Length); //27
            Console.WriteLine(chunks[0]); //28
            Console.WriteLine(chunks[1]); //29

            chunks = s8.Split(c8, 1);
            Console.WriteLine(chunks.Length); //30
            Console.WriteLine(chunks[0]); //31

            chunks = s1.Split(c2, 1);
            Console.WriteLine(chunks.Length); //32
            Console.WriteLine(chunks[0]); //33

            chunks = s1.Split(c2, 0);
            Console.WriteLine(chunks.Length); //34
        }

        static void TestReplace()
        {
            string s1 = "original";

            Console.WriteLine(s1.Replace('q', 's')); //1
            Console.WriteLine(s1.Replace('r', 'x')); //2
            Console.WriteLine(s1.Replace('i', 'x')); //3

            bool errorThrown = false;
            try
            {
                string s = s1.Replace(null, "feh");
            }
            catch (ArgumentNullException)
            {
                errorThrown = true;
            }

            Console.WriteLine(s1.Replace("igi", null)); //4
            Console.WriteLine(s1.Replace("spam", "eggs")); //5
            Console.WriteLine(s1.Replace("gin", "rum")); //6
            Console.WriteLine(s1.Replace("i", "ei")); //7

            Console.WriteLine("::".Replace("::", ":!:")); //8

            // Test overlapping matches (bug #54988)
            string s2 = "...aaaaaaa.bbbbbbbbb,............ccccccc.u...";
            Console.WriteLine(s2.Replace("..", "."));

            // Test replacing null characters (bug #67395)
            Console.WriteLine("is \0 ok ?".Replace("\0", "this"));
        }

        static void TestFormat()
        {
            Console.WriteLine(String.Format("", 0)); //1
            Console.WriteLine(String.Format("{0}", 100)); //2
            Console.WriteLine(String.Format("X{0,5}X", 37)); //3
            Console.WriteLine(String.Format("X{0,-5}X", 37)); //4
            Console.WriteLine(String.Format("{0, 4:x}", 125)); //5
            Console.WriteLine(String.Format("The {0} wise {1}.", 3, "men")); //6
            Console.WriteLine(String.Format("{0} re {1} fa {2}.", "do", "me", "so")); //7
            Console.WriteLine(String.Format("###{0:x8}#", 0xc0ffee)); //8
            Console.WriteLine(String.Format("#{0,5:x3}#", 0x33)); //9
            Console.WriteLine(String.Format("#{0,-5:x3}#", 0x33)); //10
            Console.WriteLine(String.Format("typedef struct _{0} {{ ... }} MonoObject;", "MonoObject")); //11
            Console.WriteLine(String.Format("Could not find file \"{0}\"", "a/b")); //12
            Console.WriteLine(String.Format("Could not find file \"{0}\"", "a\\b")); //13
        }
    }
}