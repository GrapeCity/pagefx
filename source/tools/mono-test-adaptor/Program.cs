using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics;

namespace mono_test_adaptor
{
    class Program
    {
        static CommandLine cl;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return;
            }

            cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Usage();
                Error(1, "Invalid command line");
            }

            if (cl.HasOption("vb"))
            {
                AdaptVB();
            }
        }

        static void Usage()
        {
        }

        static void Error(int code, string format, params object[] args)
        {
            Console.WriteLine("error MTA{0:G4}: {1}", code, string.Format(format, args));
            Environment.Exit(-1);
        }

        static void Warn(int code, string format, params object[] args)
        {
            Console.WriteLine("warning MTA{0:G4}: {1}", code, string.Format(format, args));
        }

        static void AdaptVB()
        {
            string[] input = cl.GetInputFiles();
            if (input.Length == 0)
            {
                Error(2, "No input files");
            }

            foreach (var file in input)
            {
                string path = file;
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);
                if (!File.Exists(path))
                {
                    Warn(1, "File {0} does not exist", path);
                    continue;
                }
                var lines = File.ReadAllLines(path);
                AdaptVB(lines, path);
            }
        }

        static void AdaptVB(string[] lines, string path)
        {
            int modBegin = Array.FindIndex(lines, 0, VB.IsModuleBegin);
            if (modBegin < 0)
            {
                Warn(2, "No Module in file {0}", path);
                return;
            }
            int modEnd = Array.FindIndex(lines, modBegin + 1, VB.IsModuleEnd);
            if (modEnd < 0)
            {
                Warn(3, "No End Module in file {0}", path);
                return;
            }

            bool mainSub = false;
            int mainBegin = Array.FindIndex(lines, modBegin + 1, modEnd - modBegin, VB.IsMainSub);
            int mainEnd;
            if (mainBegin >= 0)
            {
                mainSub = true;
                mainEnd = Array.FindIndex(lines, mainBegin + 1, modEnd - mainBegin, VB.IsEndSub);
            }
            else
            {
                mainBegin = Array.FindIndex(lines, modBegin + 1, modEnd - modBegin, VB.IsMainFunction);
                if (mainBegin < 0)
                {
                    Warn(4, "No Main in file {0}", path);
                    return;
                }
                
                mainEnd = Array.FindIndex(lines, mainBegin + 1, modEnd - mainBegin, VB.IsEndFunction);
            }

            if (mainEnd < 0)
            {
                Warn(5, "No End Sub For Main in file {0}", path);
                return;
            }

            var newLines = new List<string>();

            newLines.AddRange(Slice(lines, 0, mainBegin));

            if (mainSub)
            {
                newLines.Add("\tSub _Main()");
            }
            else
            {
                newLines.Add("\tFunction _Main() As Integer");
            }

            newLines.AddRange(Slice(lines, mainBegin + 1, mainEnd - mainBegin));

            newLines.Add("");
            newLines.Add("\tSub Main()");
            newLines.Add("\t\t_Main()");
            newLines.Add("\t\tSystem.Console.WriteLine(\"<%END%>\")");
            newLines.Add("\tEnd Sub");
            newLines.Add("");

            newLines.AddRange(Slice(lines, mainEnd + 1));

            //string newPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".pfx.vb");
            string newPath = path;
            File.WriteAllLines(newPath, newLines.ToArray());
        }

        static T[] Slice<T>(T[] arr, int startIndex, int count)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; ++i)
                result[i] = arr[startIndex + i];
            return result;
        }

        static T[] Slice<T>(T[] arr, int startIndex)
        {
            int n = arr.Length - startIndex;
            T[] result = new T[n];
            for (int i = 0; i < n; ++i)
                result[i] = arr[startIndex + i];
            return result;
        }
    }
}
