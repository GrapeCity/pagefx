using System;
using System.IO;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI
{
    public static class Infrastructure
    {
        private class Impl : ILanguageInfrastructure
        {
            public Impl()
            {
                LanguageInfrastructure.Register(this);
            }

            #region ILanguageInfrastructure Members
            public string Name
            {
                get { return "CLI"; }
            }

            public void Init()
            {
                ClearCache();
            }

            public IAssembly Deserialize(string path, string format)
            {
                return AssemblyLoader.Load(path);
            }

            public IAssembly Deserialize(Stream input, string format)
            {
                return AssemblyLoader.Load(input);
            }

            public void Serialize(IAssembly assembly, string path, string format)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void Serialize(IAssembly assembly, Stream output, string format)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            #endregion
        }

        public static ILanguageInfrastructure Instance
        {
            get { return _instance; }
        }
        private static readonly ILanguageInfrastructure _instance = new Impl();

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

        public static void Serialize(IAssembly assembly, Stream stream, string format)
        {
            _instance.Serialize(assembly, stream, format);
        }

#if DEBUG
        public static bool Debug;
        public static string TestCaseDirectory;
#endif

        public static bool SubstituteFrameworkAssemblies = true;
        public static bool ResolveLabels;
        public static bool EnableDecompiler;

        public static void ClearCache()
        {
            SystemTypes.Reset();
            TypeFactory.ClearCache();
            AssemblyResolver.ClearCache();
            AssemblyLoader.Clean();
#if PERF
            ILTranslator.CallCount = 0;
#endif
        }

#if PERF
        public static void DumpPerfStat()
        {
            Console.WriteLine("--- CLI Perf Stat");
            Console.WriteLine("AssemblyLoader Time: {0}", AssemblyLoader.TotalTime);
            Console.WriteLine("CIL Translator Stat:");
            Console.WriteLine("  CallCount = {0}", ILTranslator.CallCount);
        }
#endif
    }
}