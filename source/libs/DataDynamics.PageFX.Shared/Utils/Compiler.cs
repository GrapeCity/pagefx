using System.CodeDom.Compiler;

namespace DataDynamics
{
    public class Compiler
    {
        private readonly ICodeCompiler compiler;
        private readonly CompilerParameters options;

        public Compiler() 
            : this(null, null, "c#")
        {
        }

        public Compiler(string[] assemblyNames) 
            : this(assemblyNames, null, "c#")
        {
        }

        public Compiler(string[] assemblyNames, string outputName, string lang)
        {
            var provider = CodeDomProvider.CreateProvider(lang);
            compiler = provider.CreateCompiler();
            options = new CompilerParameters();

            if (assemblyNames != null && assemblyNames.Length > 0)
                options.ReferencedAssemblies.AddRange(assemblyNames);
            if (!string.IsNullOrEmpty(outputName))
                options.OutputAssembly = outputName;

            options.IncludeDebugInformation = false;
            options.TempFiles = new TempFileCollection(".", false);
            options.GenerateInMemory = false;
        }

        public CompilerParameters Options
        {
            get { return options; }
        }

        public CompilerResults Compile(string code)
        {
            return compiler.CompileAssemblyFromSource(options, code);
        }
    }
}