//CHANGED
//
// System.Console.cs
//
// Author:
// 	Dietmar Maurer (dietmar@ximian.com)
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// (C) 2004,2005 Novell, Inc. (http://www.novell.com)
//

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.IO;
#if NOT_PFX
using System.Security.Permissions;
#endif

namespace System
{
    public
#if NET_2_0
	static
#else
    sealed
#endif
    class Console
    {
        internal static TextWriter stdout;
        private static TextWriter stderr;
        //private static TextReader stdin;

        static Console()
        {
            stdout = ConsoleWriter.Instance;
            stderr = stdout;
        }

#if !NET_2_0
        private Console()
        {
        }
#endif

        public static TextWriter Error
        {
            get
            {
                return stderr;
            }
        }

        public static TextWriter Out
        {
            get
            {
                return stdout;
            }
        }

#if NOT_PFX
        public static TextReader In
        {
            get
            {
                return stdin;
            }
        }
#endif

#if NOT_PFX
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public static void SetError(TextWriter newError)
        {
            if (newError == null)
                throw new ArgumentNullException("newError");

            stderr = newError;
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public static void SetIn(TextReader newIn)
        {
            if (newIn == null)
                throw new ArgumentNullException("newIn");

            stdin = newIn;
        }

        //[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public static void SetOut(TextWriter newOut)
        {
            if (newOut == null)
                throw new ArgumentNullException("newOut");

            stdout = newOut;
        }
#endif

#if NOT_PFX
        public static void Write(bool value)
        {
            stdout.Write(value);
        }

        public static void Write(char value)
        {
            stdout.Write(value);
        }

        public static void Write(char[] value)
        {
            stdout.Write(value);
        }

        public static void Write(decimal value)
        {
            stdout.Write(value);
        }

        public static void Write(double value)
        {
            stdout.Write(value);
        }

        public static void Write(int value)
        {
            stdout.Write(value);
        }

        public static void Write(long value)
        {
            stdout.Write(value);
        }

        public static void Write(object value)
        {
            stdout.Write(value);
        }

        public static void Write(float value)
        {
            stdout.Write(value);
        }

        public static void Write(string value)
        {
            stdout.Write(value);
        }

        [CLSCompliant(false)]
        public static void Write(uint value)
        {
            stdout.Write(value);
        }

        [CLSCompliant(false)]
        public static void Write(ulong value)
        {
            stdout.Write(value);
        }

        public static void Write(string format, object arg0)
        {
            stdout.Write(format, arg0);
        }

        public static void Write(string format, params object[] arg)
        {
            stdout.Write(format, arg);
        }

        public static void Write(char[] buffer, int index, int count)
        {
            stdout.Write(buffer, index, count);
        }

        public static void Write(string format, object arg0, object arg1)
        {
            stdout.Write(format, arg0, arg1);
        }

        public static void Write(string format, object arg0, object arg1, object arg2)
        {
            stdout.Write(format, arg0, arg1, arg2);
        }
#endif

        internal static void OpenSW()
        {
            stdout = new StringWriter();
        }

        internal static string CloseSW()
        {
            string str = stdout.ToString();
            stdout = ConsoleWriter.Instance;
            return str;
        }

        public static void WriteLine()
        {
            stdout.WriteLine();
        }

        public static void WriteLine(bool value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(char value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(char[] value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(decimal value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(double value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(int value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(long value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(object value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(float value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(string value)
        {
            stdout.WriteLine(value);
        }

        [CLSCompliant(false)]
        public static void WriteLine(uint value)
        {
            stdout.WriteLine(value);
        }

        [CLSCompliant(false)]
        public static void WriteLine(ulong value)
        {
            stdout.WriteLine(value);
        }

        public static void WriteLine(string format, object arg0)
        {
            stdout.WriteLine(format, arg0);
        }

        public static void WriteLine(string format, params object[] arg)
        {
            stdout.WriteLine(format, arg);
        }

        public static void WriteLine(char[] buffer, int index, int count)
        {
            stdout.WriteLine(buffer, index, count);
        }

        public static void WriteLine(string format, object arg0, object arg1)
        {
            stdout.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            stdout.WriteLine(format, arg0, arg1, arg2);
        }

#if NOT_PFX
#if NET_2_0
		public static int Read ()
		{
			if ((stdin is CStreamReader) && ConsoleDriver.IsConsole) {
				return ConsoleDriver.Read ();
			} else {
				return stdin.Read ();
			}
		}

		public static string ReadLine ()
		{
			if ((stdin is CStreamReader) && ConsoleDriver.IsConsole) {
				return ConsoleDriver.ReadLine ();
			} else {
				return stdin.ReadLine ();
			}
		}
#else
        public static int Read()
        {
            return stdin.Read();
        }

        public static string ReadLine()
        {
            return stdin.ReadLine();
        }
#endif

#endif

    }
}

