using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.NUnit;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.SwfGeneration;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    //main part of generator - contains entry point to the generator.
    internal partial class AbcGenerator : IDisposable
    {
        #region Shared Members
        public static AbcFile ToAbcFile(IAssembly assembly)
        {
            using (var g = new AbcGenerator())
            {
                return g.Generate(assembly);
            }
        }

        public static void Save(IAssembly assembly, string path)
        {
            var f = ToAbcFile(assembly);
            f.Save(path);
        }

        public static void Save(IAssembly assembly, Stream output)
        {
            var f = ToAbcFile(assembly);
            f.Save(output);
        }
        #endregion

        #region IDisposable Members
        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~AbcGenerator()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
        #endregion

	    private IMethod _entryPoint;
		private AbcCode _newAPI;

		internal AbcFile Abc { get; private set; }
        
        //If not null indicates that we genearate swiff file.
        internal SwfCompiler SwfCompiler;

        public AbcGenMode Mode;
		public CorlibTypes CorlibTypes;

	    public IAssembly AppAssembly { get; private set; }

	    /// <summary>
        /// Indicates whether we are compiling swf file.
        /// </summary>
        public bool IsSwf
        {
            get { return SwfCompiler != null; }
        }

        /// <summary>
        /// Indicates whether we are compiling swc file.
        /// </summary>
        public bool IsSwc
        {
            get { return SwfCompiler != null && SwfCompiler.IsSwc; }
        }

        public bool IsFlexApplication
        {
            get { return SwfCompiler != null && SwfCompiler.IsFlexApplication; }
        }

        public string RootNamespace
        {
            get 
            {
                if (SwfCompiler != null)
                {
                    string ns = SwfCompiler.RootNamespace;
                    if (ns != null) return ns;
                }
                return "";
            }
        }

        public AbcNamespace RootAbcNamespace
        {
            get { return _nsroot ?? (_nsroot = Abc.DefinePackage(RootNamespace)); }
        }
        private AbcNamespace _nsroot;

	    internal IType SysType(SystemTypeCode typeCode)
		{
			return SystemTypes[typeCode];
		}

	    internal SystemTypes SystemTypes
	    {
			get { return AppAssembly.SystemTypes; }
	    }

	    internal TypeFactory TypeFactory
	    {
			get { return AppAssembly.TypeFactory; }
	    }

	    #region Generate - Entry Point
        public AbcFile Generate(IAssembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

#if PERF
            int start = Environment.TickCount;
#endif

#if DEBUG
            DebugService.LogInfo("ABC Generator started");
            DebugService.LogSeparator();
            DebugService.DoCancel();
#endif
            AppAssembly = assembly;

			CorlibTypes = new CorlibTypes(assembly);

			AssemblyIndex.Setup(assembly);

            Abc = new AbcFile
                       {
                           AutoComplete = true,
                           ReduceSize = true,
                           Generator = this,
                           SwfCompiler = SwfCompiler,
                           Assembly = assembly
                       };

            AppAssembly.CustomData().AddAbc(Abc);

            if (SwfCompiler != null)
            {
                SwfCompiler.AppFrame = Abc;
                if (SwfCompiler.IsSwc)
                    Mode = AbcGenMode.Full;
            }

            _newAPI = new AbcCode(Abc);
            RegisterObjectFunctions();

            BuildApp();

            #region Finish Application
#if DEBUG
            DebugService.DoCancel();
#endif
			if (SwfCompiler != null)
			{
				SwfCompiler.FinishApplication();
			}

        	#endregion

            BuildRootTimeline();

            #region Finish Types
#if DEBUG
            DebugService.DoCancel();
#endif

            FinishTypes();
            #endregion

            #region Late Methods
#if DEBUG
            DebugService.DoCancel();
#endif

            _lateMethods.Finish();
            #endregion

            #region Scripts
#if DEBUG
            DebugService.DoCancel();
#endif
            BuildScripts();
            #endregion

            FinishMainScript();

            // Finish ABC
#if DEBUG
            DebugService.DoCancel();
#endif

            Abc.Finish();

#if PERF
            Console.WriteLine("ABC.MultinameCount: {0}", _abc.Multinames.Count);
            Console.WriteLine("AbcGenerator Elapsed Time: {0}", Environment.TickCount - start);
#endif

			// reset tags to allow recompilation
			if (!IsSwf && !IsSwc)
				ResetMembersData();

            return Abc;
        }
        #endregion

        #region BuildApp
        private void BuildApp()
        {
#if PERF
            int start = Environment.TickCount;
#endif
            switch (Mode)
            {
                case AbcGenMode.Default:
                    BuildAppDefault();
                    break;

                case AbcGenMode.Full:
                    BuildAssemblyTypes();
                    break;
            }

#if PERF
            Console.WriteLine("AbcGen.Build: {0}", Environment.TickCount - start);
#endif
        }

        private void BuildAppDefault()
        {
            _entryPoint = AppAssembly.EntryPoint;
            if (_entryPoint != null)
                DefineMethod(_entryPoint);
            else
                BuildLibrary();

            BuildExposedTypes();
        }
        #endregion

	    private void BuildExposedTypes()
        {
			// TODO: add command like option (/expose) to build exposed types
			// load only exposed types
			foreach (var type in AppAssembly.GetReferences(false).SelectMany(asm => asm.GetExposedTypes()))
            {
                if (type.IsTestFixture())
                    _testFixtures.Add(type);
                DefineType(type);
            }
        }

	    #region BuildLibrary
        private void BuildLibrary()
        {
            if (IsFlexApplication)
            {
                var type = SwfCompiler.TypeFlexApp;
                if (type == null)
                    throw new InvalidOperationException();
                DefineType(type);
                return;
            }

            if (IsSwf)
            {
				var type = AppAssembly.Types.FirstOrDefault(IsRootSprite);
                if (type != null)
                {
                    DefineType(type);
                    return;
                }
            }

            BuildAssemblyTypes();
        }

        private void BuildAssemblyTypes()
        {
            var list = new List<IType>(AppAssembly.Types);
            foreach (var type in list)
            {
                if (GenericType.HasGenericParams(type)) continue;
                DefineType(type);
            }
        }
        #endregion

        #region Late Methods

        private readonly AbcLateMethodCollection _lateMethods = new AbcLateMethodCollection();

    	private void AddLateMethod(AbcMethod method, AbcCoder coder)
        {
            _lateMethods.Add(method, coder);
        }

        #endregion

        #region Utils

        IType FindTypeDefOrRef(string fullname)
        {
            return AssemblyIndex.FindType(AppAssembly, fullname);
        }

        AbcInstance FindInstanceDefOrRef(string fullname)
        {
            var type = FindTypeDefOrRef(fullname);
            if (type == null) return null;
            return DefineAbcInstance(type);
        }

        public AbcInstance FindInstanceRef(AbcMultiname name)
        {
            return AssemblyIndex.FindInstance(AppAssembly, name);
        }

		public AbcInstance ImportType(string fullname)
		{
			return ImportType(fullname, false);
		}

        public AbcInstance ImportType(string fullname, bool safe)
        {
        	try
        	{
				return Abc.ImportType(AppAssembly, fullname);
        	}
        	catch (Exception)
        	{
				if (safe)
				{
					CompilerReport.Add(Warnings.UnableImportType, fullname);
					return null;
				}
        		throw;
        	}            
        }

		public AbcInstance ImportType(string fullname, ref AbcInstance field)
		{
			return ImportType(fullname, ref field, false);
		}

		public AbcInstance ImportType(string fullname, ref AbcInstance field, bool safe)
		{
			return field ?? (field = ImportType(fullname));
		}

    	void AddMethod(AbcMethod method)
        {
            Abc.AddMethod(method);
        }

        public AbcParameter CreateParam(AbcMultiname type, string name)
        {
            return Abc.DefineParam(type, name);
        }

        public AbcParameter CreateParam(AbcInstance type, string name)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return CreateParam(type.Name, name);
        }

        public AbcParameter CreateParam(IType type, string name)
        {
            var typeName = DefineMemberType(type);
            return CreateParam(typeName, name);
        }

        public AbcParameter CreateParam(AvmTypeCode type, string name)
        {
            return Abc.DefineParam(type, name);
        }

    	#endregion

	    private object SetData(ITypeMember member, object data)
	    {
		    if (ReferenceEquals(member.Data, data)) return data;

		    member.Data = data;

			_taggedMembers.Add(member);

		    return data;
	    }

		internal void ResetMembersData()
		{
			foreach (var member in _taggedMembers)
			{
				member.Data = null;
			}
		}

	    private readonly List<ITypeMember> _taggedMembers = new List<ITypeMember>();
    }
}