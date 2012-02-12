using System;
using System.IO;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    /// <summary>
    /// Facade to Flash Language Infrastructure
    /// </summary>
    public static class Infrastructure
    {
        class Impl : ILanguageInfrastructure
        {
            public Impl()
            {
                LanguageInfrastructure.Register(this);
            }

            #region ILanguageInfrastructure Members
            public void Init()
            {    
            }

            public string Name
            {
                get { return "FLI"; }
            }

            static OutputFormat GetFormat(CommandLine cl)
            {
                string f = cl.GetOption("", "f", "format");
                if (string.IsNullOrEmpty(f) || string.Compare(f, "abc", true) == 0)
                    return OutputFormat.ABC;
                if (string.Compare(f, "swf", true) == 0)
                    return OutputFormat.SWF;
                if (string.Compare(f, "swc", true) == 0)
                    return OutputFormat.SWC;
                CompilerReport.Add(Warnings.UnsupportedFormat, f);
                return OutputFormat.SWF;
            }

            public IAssembly Deserialize(string path, string format)
            {
                var cl = CommandLine.Parse(format);
                var f = GetFormat(cl);
                if (f == OutputFormat.ABC)
                {
                    return AssemblyBuilder.Build(path, cl);
                }
                throw new NotImplementedException();
            }

            public IAssembly Deserialize(Stream input, string format)
            {
                var cl = CommandLine.Parse(format) ?? new CommandLine();

            	var f = GetFormat(cl);
                if (f == OutputFormat.ABC)
                {
                    return AssemblyBuilder.Build(input, cl);
                }
                throw new NotImplementedException();
            }

            public void Serialize(IAssembly assembly, string path, string format)
            {
                var cl = CommandLine.Parse(format) ?? new CommandLine();

                GlobalSettings.SetGlobalOptions(cl);

                var f = GetFormat(cl);
                switch (f)
                {
                    case OutputFormat.ABC:
                        AbcGenerator.Save(assembly, path);
                        break;

                    case OutputFormat.SWF:
                    case OutputFormat.SWC:
                        {
                            var options = new SwfCompilerOptions(cl) {OutputFormat = f};
                            SwfCompiler.Save(assembly, path, options);
                        }
                        break;
                }
            }

            public void Serialize(IAssembly assembly, Stream output, string format)
            {
                var cl = CommandLine.Parse(format) ?? new CommandLine();

                var f = GetFormat(cl);
                switch (f)
                {
                    case OutputFormat.ABC:
                        AbcGenerator.Save(assembly, output);
                        break;

                    case OutputFormat.SWF:
                    case OutputFormat.SWC:
                        {
                            var options = new SwfCompilerOptions(cl) {OutputFormat = f};
                            SwfCompiler.Save(assembly, output, options);
                        }
                        break;
                }
            }
            #endregion
        }

        public static ILanguageInfrastructure Instance
        {
            get { return _instance; }
        }

        static readonly ILanguageInfrastructure _instance = new Impl();

        public static IAssembly Deserialize(string path, string format)
        {
            return _instance.Deserialize(path, format);
        }

        public static IAssembly Deserialize(Stream stream, string format)
        {
            return _instance.Deserialize(stream, format);
        }

        public static void Serialize(IAssembly assembly, string path, string format)
        {
            _instance.Serialize(assembly, path, format);
        }

        public static void Serialize(string assemblyPath, string path, string format)
        {
            var asm = LanguageInfrastructure.CLI.Deserialize(assemblyPath, null);
            Serialize(asm, path, format);
        }

        public static void Serialize(IAssembly assembly, Stream stream, string format)
        {
            _instance.Serialize(assembly, stream, format);
        }

        public static void PerformCodeCaching()
        {
            CodeCachingService.Run();
        }

#if PERF
        public static void DumpPerfStat()
        {
            Console.WriteLine("--- FLI Perf Stat");
            Console.WriteLine("AbcMultinamePool Stat:");
            Console.WriteLine("  CallCount: {0}", AbcMultinamePool.CallCount);
            Console.WriteLine("  Time: {0}", AbcMultinamePool.Time);
        }
#endif
    }
}