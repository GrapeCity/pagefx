using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.Tools
{
    static class RV
    {
        static readonly string[] NotVars = { "out", "output", "noprefix", "?" };

        #region Usage
        public static void Usage()
        {
            Console.WriteLine("rv (/Var1:Value1 ... /VarN:ValueN) [Options] template");
            Console.WriteLine();
        }
        #endregion

        public static int Run(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return 1;
                //args = new[] { "/F:file(As.cs) t.txt" };
            }

            var cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Console.WriteLine("error RV0001: Invalid command line.");
                return -1;
            }

            return Run(cl);
        }

        public static int Run(CommandLine cl)
        {
            if (cl.HasOption("?"))
            {
                Usage();
                return 0;
            }

            string[] inputs = cl.GetInputFiles();
            if (inputs == null || inputs.Length != 1)
            {
                Console.WriteLine("error RV0002: No template file.");
                return -1;
            }

            string input = AbsPath(inputs[0]);
            string output = cl.GetOption(null, NotVars);
            bool noprefix = cl.HasOption("noprefix");
            RVScheme scheme = RVScheme.DollarID;
            if (noprefix)
            {
                scheme = RVScheme.ID;
            }
            else
            {
                string s = cl.GetOption(null, "scheme");
                if (!string.IsNullOrEmpty(s))
                {
                    if (string.Compare(s, "ant", true) == 0)
                        scheme = RVScheme.Ant;
                    else if (string.Compare(s, "id", true) == 0)
                        scheme = RVScheme.ID;
                }
            }

            if (string.IsNullOrEmpty(output))
                output = Path.ChangeExtension(input, ".cs");
            output = AbsPath(output);

            try
            {
                var vars = GetVars(cl);

                string template = File.ReadAllText(input);

                string src = template.ReplaceVars(scheme, vars.ToArray());
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                File.WriteAllText(output, src);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return 0;
        }

        static List<string> GetVars(CommandLine cl)
        {
            var vars = new List<string>();
            foreach (var item in cl.Items)
            {
                if (item.Type == CommandLine.ItemType.Option)
                {
                    string name = item.Name;
					if (NotVars.Any(s => string.Equals(s, name, StringComparison.CurrentCultureIgnoreCase)))
                        continue;

                    vars.Add(name);
                    vars.Add(Eval(item.Value));
                }
            }
            return vars;
        }

        #region Eval
        static string Eval(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            int i = value.IndexOf('(');
            if (i >= 0) //func?
            {
                //TODO: Add support for quoted strings
                int j = value.IndexOf(')', i + 1);
                if (j >= 0)
                {
                    string func = value.Substring(0, i);
                    string args = value.Substring(i + 1, j - i - 1);
                    return EvalFunc(func, args);
                }
            }
            return value;
        }

        static string EvalFunc(string func, string args)
        {
            if (string.Compare(func, "file", true) == 0)
            {
                string path = AbsPath(args);
                return File.ReadAllText(path);
            }
            if (string.Compare(func, "env", true) == 0)
                return GetEnv(args);
            if (string.Compare(func, "envp", true) == 0)
            {
                string path = GetEnv(args);
                if (!string.IsNullOrEmpty(path))
                    return path.Replace("\\", "\\\\");
                return "";
            }
            if (string.Compare(func, "newguid", true) == 0)
                return Guid.NewGuid().ToString("D");
            throw new NotImplementedException("The function is not implemented: " + func + "(" + args + ")");
        }
        #endregion

        #region Utils
        static string GetEnv(string name, params EnvironmentVariableTarget[] targets)
        {
            int n = targets.Length;
            for (int i = 0; i < n; ++i)
            {
                string v = Environment.GetEnvironmentVariable(name, targets[i]);
                if (!string.IsNullOrEmpty(v))
                    return v;
            }
            return null;
        }

        static string GetEnv(string name)
        {
            return GetEnv(name, EnvironmentVariableTarget.Process,
                          EnvironmentVariableTarget.User,
                          EnvironmentVariableTarget.Machine);
        }

        static string AbsPath(string path)
        {
            if (!Path.IsPathRooted(path))
                return Path.Combine(Environment.CurrentDirectory, path);
            return path;
        }
        #endregion
    }
}