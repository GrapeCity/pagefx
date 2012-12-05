using System;

enum EInt8 : sbyte { A = -1, B, C, D }
enum EUInt8 : byte { A, B, C, D }
enum EInt16 : short { A = -1, B, C, D }
enum EUInt16 : ushort { A, B, C, D }
enum EInt32 : int { A = -1, B, C, D }
enum EUInt32 : uint { A, B, C, D }
enum EInt64 : long { A = -1, B, C, D }
enum EUInt64 : ulong { A, B, C, D }

namespace PageFX
{
    using T = System.Int16;

    class X
    {
        private delegate void Code();

        static void Safe(Code code)
        {
            try
            {
                code();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                return;
            }
            Console.WriteLine("ok");
        }

        static T FromInt8(sbyte v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromUInt8(byte v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromInt16(short v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromUInt16(ushort v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromInt32(int v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromUInt32(uint v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromInt64(long v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromUInt64(ulong v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromSingle(float v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromDouble(double v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }
        
        static T FromEInt8(EInt8 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromEUInt8(EUInt8 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }
        
        static T FromEInt16(EInt16 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromEUInt16(EUInt16 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromEInt32(EInt32 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromEUInt32(EUInt32 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromEInt64(EInt64 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static T FromEUInt64(EUInt64 v)
        {
            Console.WriteLine(v);
            return checked((T)v);
        }

        static void Main()
        {
            Console.WriteLine("Int8");
            Safe(delegate { FromInt8(sbyte.MinValue); });
            Safe(delegate { FromInt8(sbyte.MaxValue); });

            Console.WriteLine("UInt8");
            Safe(delegate { FromUInt8(byte.MinValue); });
            Safe(delegate { FromUInt8(byte.MaxValue); });

            Console.WriteLine("Int16");
            Safe(delegate { FromInt16(short.MinValue); });
            Safe(delegate { FromInt16(short.MaxValue); });

            Console.WriteLine("UInt16");
            Safe(delegate { FromUInt16(ushort.MinValue); });
            Safe(delegate { FromUInt16(ushort.MaxValue); });

            Console.WriteLine("Int32");
            Safe(delegate { FromInt32(int.MinValue); });
            Safe(delegate { FromInt32(int.MaxValue); });

            Console.WriteLine("UInt32");
            Safe(delegate { FromUInt32(uint.MinValue); });
            Safe(delegate { FromUInt32(uint.MaxValue); });

            Console.WriteLine("Int64");
            Safe(delegate { FromInt64(long.MinValue); });
            Safe(delegate { FromInt64(long.MaxValue); });

            Console.WriteLine("UInt64");
            Safe(delegate { FromUInt64(ulong.MinValue); });
            Safe(delegate { FromUInt64(ulong.MaxValue); });

            Console.WriteLine("Single");
            Safe(delegate { FromSingle(float.MinValue); });
            Safe(delegate { FromSingle(float.MaxValue); });

            Console.WriteLine("Double");
            Safe(delegate { FromDouble(double.MinValue); });
            Safe(delegate { FromDouble(double.MaxValue); });
            
            Console.WriteLine("EInt8");
            Safe(delegate { FromEInt8(EInt8.A); });
            Safe(delegate { FromEInt8(EInt8.B); });

            Console.WriteLine("EUInt8");
            Safe(delegate { FromEUInt8(EUInt8.A); });
            Safe(delegate { FromEUInt8(EUInt8.B); });

            Console.WriteLine("EInt16");
            Safe(delegate { FromEInt16(EInt16.A); });
            Safe(delegate { FromEInt16(EInt16.B); });

            Console.WriteLine("EUInt16");
            Safe(delegate { FromEUInt16(EUInt16.A); });
            Safe(delegate { FromEUInt16(EUInt16.B); });

            Console.WriteLine("EInt32");
            Safe(delegate { FromEInt32(EInt32.A); });
            Safe(delegate { FromEInt32(EInt32.B); });
            
            Console.WriteLine("EUInt32");
            Safe(delegate { FromEUInt32(EUInt32.A); });
            Safe(delegate { FromEUInt32(EUInt32.B); });

            Console.WriteLine("EInt64");
            Safe(delegate { FromEInt64(EInt64.A); });
            Safe(delegate { FromEInt64(EInt64.B); });

            Console.WriteLine("EUInt64");
            Safe(delegate { FromEUInt64(EUInt64.A); });
            Safe(delegate { FromEUInt64(EUInt64.B); });
            
            Console.WriteLine("<%END%>");
        }
    }
}