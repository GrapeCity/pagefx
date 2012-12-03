using System;
using System.Diagnostics;
using System.IO;

namespace DataDynamics.PageFX
{
    public sealed class Error
    {
        #region ctors
        public Error()
        {
        }

        public Error(string code, string format, bool warn)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("Invalid error code");
            if (string.IsNullOrEmpty(format))
                throw new ArgumentException("Format can not be null or empty", "format");
            Code = code;
            Format = format;
            _formatArgCount = CountOfFormatArgs(format);
            IsWarning = warn;
        }

        public Error(string code, string format)
            : this(code, format, false)
        {
        }

        internal Error(int id, string format, bool warn)
            : this(FormatCode(id), format, warn)
        {
        }

        internal Error(int id, string format)
            : this(id, format, false)
        {
        }

        static string FormatCode(int id)
        {
            return "PFC" + id.ToString("X4");
        }

        static int CountOfFormatArgs(string format)
        {
            int count = 0;
            int n = format.Length;
            bool br = false;
            for (int i = 0; i < n; ++i)
            {
                char c = format[i];
                if (br)
                {
                    if (c == '{')
                    {
                        br = false;
                        continue;
                    }
                    ++count;

                    for (; i < n; ++i)
                    {
                        if (format[i] == '}')
                        {
                            br = false;
                            break;
                        }
                    }

                    if (br)
                        throw new FormatException("Bad format string");

                    continue;
                }

                if (c == '{')
                {
                    br = true;
                    continue;
                }
            }

            return count;
        }
        #endregion

        #region Properties
        public string Code { get; set; }

        public bool IsWarning { get; set; }

        public string Format { get; set; }

        readonly int _formatArgCount;
        #endregion

        public string ToString(params object[] args)
        {
            string msg = FormatMessage(args);
            string name = IsWarning ? "warning" : "error";
            return string.Format("{0} {1}: {2}", name, Code, msg);
        }

        public string FormatMessage(params object[] args)
        {
            Debug.Assert(!string.IsNullOrEmpty(Format));
            Debug.Assert(_formatArgCount == args.Length);
            if (_formatArgCount == 0)
                return Format;
            return string.Format(Format, args);
        }

        public Exception CreateException(params object[] args)
        {
            string msg = FormatMessage(args);
            return new CompilerException(this, msg);
        }

        public Exception CreateInnerException(Exception e, params object[] args)
        {
            string msg = FormatMessage(args);
            return new CompilerException(this, msg);
        }

        public void Log(TextWriter writer, params object[] args)
        {
            writer.WriteLine(ToString(args));
        }

        public void LogConsole(params object[] args)
        {
            Log(Console.Out, args);
        }
    }
}