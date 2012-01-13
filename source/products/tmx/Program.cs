using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics;

namespace tmx
{
    class Program
    {
        static void Logo()
        {
            CommandLine.Logo(typeof(Program).Assembly);
        }

        static void Usage(bool nologo)
        {
            if (!nologo)
                Logo();

            Console.WriteLine("Usage: tmx.exe [options] <input-mxml-files>");
            Console.WriteLine("\twhere:");
            Console.WriteLine("\t<input-mxml-files> - MXML files to translate");
            Console.WriteLine("\t[options]:");
            Options.Usage();
        }

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Usage(false);
                return;
            }

            var cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Usage(false);
                Errors.InvalidCommandLine();
                return;
            }

            Run(cl);
        }

        static void Run(CommandLine cl)
        {
            Options.Init(cl);
            
            var files = cl.GetInputFiles();
            if (files == null || files.Length == 0)
            {
                Errors.NoInputFiles();
                return;
            }

            var t = new Translator();

            foreach (var path in files)
            {
                try
                {
                    if (IsPattern(path))
                    {
                        var f = ResolvePattern(path);
                        t.Translate(f);
                    }
                    else
                    {
                        t.Translate(path);
                    }
                }
                catch (Exception e)
                {
                    Errors.UnableToTranslate(e, path);
                }
            }
        }

        static string[] ResolvePattern(string pattern)
        {
            //TODO: Add support for NAnt path patterns
            string cd = Environment.CurrentDirectory;
            return Directory.GetFiles(cd, pattern);
        }

        static bool IsPattern(IEnumerable<char> path)
        {
            foreach (var c in path)
            {
                if (c == '*')
                    return true;
                if (c == '?')
                    return true;
            }
            return false;
        }
    }
}
