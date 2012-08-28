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
using System.Security.Permissions;
using System.Text;

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
        private static TextWriter stdout;
        private static TextWriter stderr;
        private static TextReader stdin;
    	private static bool _hasReadCalls;

    	static Console()
        {
            ResetColor();
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
				if (stderr == null)
				{
					stderr = Out;
				}
                return stderr;
            }
        }

        public static TextWriter Out
        {
            get
            {
				if (stdout == null)
				{
					stdout = ConsoleWriter.Instance;
				}
                return stdout;
            }
        }

#if CONSOLE_READ
		public static TextReader In
        {
            get
            {
	            return stdin ?? (stdin = CreateIn());
            }
        }

		private static TextReader CreateIn()
		{
			//TODO: fix verify ReferenceError on flash.text.textField in ConsoleReader running in avm shell
			//TODO: introduce a way (conditional classes) to stub classes like ConsoleReader in avm shell case
			if (avm.IsFlashPlayer)
				return new ConsoleReader();
			return new StringReader("");
		}
#else
		public static TextReader In
        {
            get { return stdin ?? (stdin = new StringReader("")); }
        }
#endif

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

        public static void Write(bool value)
        {
            Out.Write(value);
        }

        public static void Write(char value)
        {
			Out.Write(value);
        }

        public static void Write(char[] value)
        {
			Out.Write(value);
        }

        public static void Write(decimal value)
        {
			Out.Write(value);
        }

        public static void Write(double value)
        {
			Out.Write(value);
        }

        public static void Write(int value)
        {
			Out.Write(value);
        }

        public static void Write(long value)
        {
			Out.Write(value);
        }

        public static void Write(object value)
        {
			Out.Write(value);
        }

        public static void Write(float value)
        {
			Out.Write(value);
        }

        public static void Write(string value)
        {
			Out.Write(value);
        }

        [CLSCompliant(false)]
        public static void Write(uint value)
        {
			Out.Write(value);
        }

        [CLSCompliant(false)]
        public static void Write(ulong value)
        {
			Out.Write(value);
        }

        public static void Write(string format, object arg0)
        {
			Out.Write(format, arg0);
        }

        public static void Write(string format, params object[] arg)
        {
			Out.Write(format, arg);
        }

        public static void Write(char[] buffer, int index, int count)
        {
			Out.Write(buffer, index, count);
        }

        public static void Write(string format, object arg0, object arg1)
        {
			Out.Write(format, arg0, arg1);
        }

        public static void Write(string format, object arg0, object arg1, object arg2)
        {
			Out.Write(format, arg0, arg1, arg2);
        }

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
			Out.WriteLine();
        }

        public static void WriteLine(bool value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(char value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(char[] value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(decimal value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(double value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(int value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(long value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(object value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(float value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(string value)
        {
			Out.WriteLine(value);
        }

        [CLSCompliant(false)]
        public static void WriteLine(uint value)
        {
			Out.WriteLine(value);
        }

        [CLSCompliant(false)]
        public static void WriteLine(ulong value)
        {
			Out.WriteLine(value);
        }

        public static void WriteLine(string format, object arg0)
        {
			Out.WriteLine(format, arg0);
        }

        public static void WriteLine(string format, params object[] arg)
        {
			Out.WriteLine(format, arg);
        }

        public static void WriteLine(char[] buffer, int index, int count)
        {
			Out.WriteLine(buffer, index, count);
        }

        public static void WriteLine(string format, object arg0, object arg1)
        {
			Out.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
			Out.WriteLine(format, arg0, arg1, arg2);
        }

		public static int Read()
		{
			_hasReadCalls = true;
            return In.Read();
        }

        public static string ReadLine()
        {
        	_hasReadCalls = true;
            return In.ReadLine();
        }

		//TODO:
		public static Encoding InputEncoding { get; set; }
		public static Encoding OutputEncoding { get; set; }
		public static ConsoleColor BackgroundColor { get; set; }
		public static ConsoleColor ForegroundColor { get; set; }
		public static int BufferHeight { get; set; }
		public static int BufferWidth { get; set; }
		public static int WindowHeight { get; set; }
		public static int WindowWidth { get; set; }
		public static int LargestWindowWidth
		{
			get { return 800; }
		}
		public static int LargestWindowHeight
		{
			get { return 600; }
		}
		public static int WindowLeft { get; set; }
		public static int WindowTop { get; set; }
		public static int CursorLeft { get; set; }
		public static int CursorTop { get; set; }
		public static int CursorSize { get; set; }
		public static bool CursorVisible { get; set; }
		public static string Title { get; set; }

    	public static bool KeyAvailable
    	{
			get { return true; }
    	}

    	public static bool NumberLock
    	{
			get { return false; }
    	}
    	public static bool CapsLock
    	{
			get { return false; }
    	}

		public static bool TreatControlCAsInput { get; set; }

    	public static bool HasReadCalls
    	{
    		get { return _hasReadCalls; }
    	}

    	public static void Clear()
		{
#if CONSOLE_READ
			var console = In as ConsoleReader;
			if (console != null)
			{
				console.Clear();
			}
#endif
		}

		public static void ResetColor()
		{
			BackgroundColor = ConsoleColor.Black;
			ForegroundColor = ConsoleColor.White;
		}

    	public static event ConsoleCancelEventHandler CancelKeyPress;

		internal static void FireCancelKeyPress()
		{
			if (CancelKeyPress != null)
			{
				CancelKeyPress(In, new ConsoleCancelEventArgs(ConsoleSpecialKey.ControlBreak));
			}
		}
    }
}

