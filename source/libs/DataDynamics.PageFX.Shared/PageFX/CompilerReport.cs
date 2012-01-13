using System;
using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX
{
    public static class CompilerReport
    {
        static List<string> _errors;
        static int _errCount;

        public static bool CollectErrors;

        public static void Add(Error err, params object[] args)
        {
            if (CollectErrors)
            {
                if (_errors == null)
                    _errors = new List<string>();
                string msg = err.ToString(args);
                if (!_errors.Contains(msg))
                    _errors.Add(msg);
                if (!err.IsWarning)
                    ++_errCount;
                return;
            }
            if (err.IsWarning)
            {
                string msg = err.ToString(args);
                Console.WriteLine(msg);
                return;
            }
            throw err.CreateException(args);
        }

        public static bool HasErrors
        {
            get { return _errCount > 0; }
        }

        public static void Log(TextWriter writer)
        {
            if (_errors == null) return;
            foreach (var s in _errors)
                writer.WriteLine(s);
            _errors.Clear();
        }

        public static void Log()
        {
            Log(Console.Out);
        }
    }
}