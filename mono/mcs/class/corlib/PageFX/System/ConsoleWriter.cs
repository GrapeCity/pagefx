using System.IO;
using System.Text;
//using flash.external;

namespace System
{
    internal class ConsoleWriter : TextWriter
    {
        public static readonly ConsoleWriter Instance = new ConsoleWriter();

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        public override void Write(char value)
        {
            write(value.ToString());
        }

        static void write(string s)
        {
            if (avm.IsFlashPlayer)
            {
                //NOTE: It was used in flashell that hosted of FlashPlayer ActiveX control
                //if (ExternalInterface.available)
                //{
                //    ExternalInterface.call("log", s);
                //    return;
                //}

                avm.trace(s);

#if CONSOLE_READ
				if (Console.HasReadCalls)
				{
					var terminal = Console.In as ConsoleReader;
					if (terminal != null)
					{
						terminal.WriteLine(s);
					}
				}
#endif
            }
            else
            {
                avm.Console_Write(s);
            }
        }

        internal override bool UseEOL
        {
            get 
            {
                if (avm.IsFlashPlayer)
                {
                    //return ExternalInterface.available;
                    return false;
                }
                return true;
            }
        }

        public override void Write(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            write(s);
        }

        public override void WriteLine()
        {
            if (UseEOL)
                write(PfxIO.NewLine);
            else
                write("");
        }

        public override void WriteLine(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteLine();
            }
            else
            {
                write(value);
                if (UseEOL)
                    WriteLine();
            }
        }

        public override void WriteLine(object value)
        {
            if (value != null)
                WriteLine(value.ToString());
            else
                WriteLine();
        }
    }
}