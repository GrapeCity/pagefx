using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.NUnit;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Pointers;
using DataDynamics.PageFX.FlashLand.Core.SwfGeneration;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    /// <summary>
	/// Implements <see cref="AbcFile"/> generation.
    /// </summary>
    internal sealed class AbcGenerator : IDisposable
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

		#region Properties

		internal IMethod EntryPoint { get; private set; }
		internal AbcCode NewApi { get; private set; }

		internal AbcFile Abc { get; private set; }
        
        //If not null indicates that we genearate swiff file.
        internal SwfCompiler SwfCompiler;

        public AbcGenMode Mode;
		
	    public IAssembly AppAssembly { get; private set; }

	    internal CorlibFacade Corlib
	    {
			get { return _corlib ?? (_corlib = new CorlibFacade(this)); }
	    }
		private CorlibFacade _corlib;

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

		private IType MainType
		{
			get
			{
				if (EntryPoint != null)
					return EntryPoint.DeclaringType;

				if (IsSwf)
				{
					if (IsFlexApplication)
						return SwfCompiler.TypeFlexApp;
					var root = RootSprite.Instance;
					if (root != null)
						return root.Type;
				}

				return null;
			}
		}

		public AbcInstance MainInstance
		{
			get
			{
				if (RootSprite.IsGenerated)
					return RootSprite.Instance;
				var type = MainType;
				return type != null ? TypeBuilder.BuildInstance(type) : null;
			}
		}

		/// <summary>
		/// Returns true if application assembly has nunit tests and does not define custom root sprite.
		/// </summary>
		public bool IsNUnit
		{
			get
			{
				if (IsSwf)
				{
					if (IsFlexApplication) return false;
					if (!RootSprite.IsGenerated)
						return false;
				}
				return NUnit.FixtureCount > 0;
			}
		}

		#endregion

		#region Parts, Builders

	    internal TypeBuilder TypeBuilder
	    {
			get { return _typeBuilder ?? (_typeBuilder = new TypeBuilder(this)); }
	    }
	    private TypeBuilder _typeBuilder;

		internal MethodBuilder MethodBuilder
		{
			get { return _methodBuilder ?? (_methodBuilder = new MethodBuilder(this)); }
		}
	    private MethodBuilder _methodBuilder;

		internal FieldBuilder FieldBuilder
	    {
			get { return _fieldBuilder ?? (_fieldBuilder = new FieldBuilder(this)); }
	    }
	    private FieldBuilder _fieldBuilder;

	    internal ArrayImpl ArrayImpl
	    {
			get { return _arrayImpl ?? (_arrayImpl = new ArrayImpl(this)); }
	    }
	    private ArrayImpl _arrayImpl;

		internal NUnitRunner NUnit
	    {
			get { return _nunit ?? (_nunit = new NUnitRunner(this)); }
	    }
		private NUnitRunner _nunit;

		internal PtrManager Pointers
		{
			get { return _pointers ?? (_pointers = new PtrManager(this)); }
		}
		private PtrManager _pointers;

		internal DelegatesImpl Delegates
		{
			get { return _delegates ?? (_delegates = new DelegatesImpl(this)); }
		}
		private DelegatesImpl _delegates;

	    internal RuntimeImpl RuntimeImpl
	    {
			get { return _runtimeImpl ?? (_runtimeImpl = new RuntimeImpl(this)); }
	    }
	    private RuntimeImpl _runtimeImpl;

	    internal StaticCtorsImpl StaticCtors
	    {
			get { return _staticCtors ?? (_staticCtors = new StaticCtorsImpl(this)); }
	    }
	    private StaticCtorsImpl _staticCtors;

	    internal RootSpriteImpl RootSprite
	    {
			get { return _rootSprite ?? (_rootSprite = new RootSpriteImpl(this)); }
	    }
	    private RootSpriteImpl _rootSprite;

	    internal ScriptBuilder Scripts
	    {
			get { return _scripts ?? (_scripts = new ScriptBuilder(this)); }
	    }
	    private ScriptBuilder _scripts;

	    internal OperatorBuilder Operators
	    {
			get { return _operators ?? (_operators = new OperatorBuilder(this)); }
	    }
		private OperatorBuilder _operators;

	    internal BoxingImpl Boxing
	    {
			get { return _boxingImpl ?? (_boxingImpl = new BoxingImpl(this)); }
	    }
	    private BoxingImpl _boxingImpl;

	    internal EmbeddedAssetBuilder EmbeddedAssets
	    {
		    get { return _embeddedAssets ?? (_embeddedAssets = new EmbeddedAssetBuilder(this)); }
	    }
		private EmbeddedAssetBuilder _embeddedAssets;

	    internal ReflectionImpl Reflection
	    {
			get { return _reflectionImpl ?? (_reflectionImpl = new ReflectionImpl(this)); }
	    }
	    private ReflectionImpl _reflectionImpl;

	    internal ObjectPrototypeImpl ObjectPrototypes
	    {
			get { return _objectPrototypes ?? (_objectPrototypes = new ObjectPrototypeImpl(this)); }
	    }
	    private ObjectPrototypeImpl _objectPrototypes;

	    internal StringPrototypeImpl StringPrototypes
	    {
			get { return _stringPrototypes ?? (_stringPrototypes = new StringPrototypeImpl(this)); }
	    }
	    private StringPrototypeImpl _stringPrototypes;

	    internal FlexAppBuilder FlexAppBuilder
	    {
			get { return _flexAppBuilder ?? (_flexAppBuilder = new FlexAppBuilder(this)); }
	    }
		private FlexAppBuilder _flexAppBuilder;

	    internal CallResolver CallResolver
	    {
			get { return _callResolver ?? (_callResolver = new CallResolver(this)); }
	    }
		private CallResolver _callResolver;

		#endregion

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

            NewApi = new AbcCode(Abc);
            ObjectPrototypes.Init();

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

            RootSprite.BuildTimeline();

            #region Finish Types
#if DEBUG
            DebugService.DoCancel();
#endif

			TypeBuilder.FinishTypes();
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
            Scripts.BuildScripts();
            #endregion

            Scripts.FinishMainScript();

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
            EntryPoint = AppAssembly.EntryPoint;
            if (EntryPoint != null)
				MethodBuilder.Build(EntryPoint);
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
                    NUnit.AddFixture(type);
				TypeBuilder.Build(type);
            }
        }

	    #region BuildLibrary
		internal bool IsRootSprite(IType type)
		{
			if (SwfCompiler != null && !string.IsNullOrEmpty(SwfCompiler.RootSprite))
				return type.FullName == SwfCompiler.RootSprite;
			return type.IsRootSprite();
		}

        private void BuildLibrary()
        {
            if (IsFlexApplication)
            {
                var type = SwfCompiler.TypeFlexApp;
                if (type == null)
                    throw new InvalidOperationException();
				TypeBuilder.Build(type);
                return;
            }

            if (IsSwf)
            {
				var type = AppAssembly.Types.FirstOrDefault(IsRootSprite);
                if (type != null)
                {
					TypeBuilder.Build(type);
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
				TypeBuilder.Build(type);
            }
        }
        #endregion

        #region Late Methods

        private readonly AbcLateMethodCollection _lateMethods = new AbcLateMethodCollection();

    	internal void AddLateMethod(AbcMethod method, AbcCoder coder)
        {
            _lateMethods.Add(method, coder);
        }

        #endregion

        #region Utils

        public IType FindTypeDefOrRef(string fullname)
        {
            return AssemblyIndex.FindType(AppAssembly, fullname);
        }

        public AbcInstance FindInstanceDefOrRef(string fullname)
        {
            var type = FindTypeDefOrRef(fullname);
            if (type == null) return null;
			return TypeBuilder.BuildInstance(type);
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

	    #endregion

	    internal object SetData(ITypeMember member, object data)
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

		public void CheckEmbedAsset(IField field)
		{
			Debug.Assert(field.HasEmbedAttribute());

			if (!field.IsStatic)
				throw Errors.EmbedAsset.FieldIsNotStatic.CreateException(field.Name);

			if (!IsSwf)
				throw Errors.EmbedAsset.NotFlashRuntime.CreateException();
		}

		private int PlayerVersion
		{
			get
			{
				if (SwfCompiler != null)
					return SwfCompiler.PlayerVersion;
				return -1;
			}
		}

		public void CheckApiCompatibility(ITypeMember m)
		{
			if (m == null) return;
			if (!IsSwf) return;

			int v = m.GetPlayerVersion();
			if (v < 0) return;

			if (v > PlayerVersion)
			{
				var method = m as IMethod;
				if (method != null)
				{
					CompilerReport.Add(Errors.ABC.IncompatibleCall, method.GetFullName(), v);
					return;
				}

				var f = m as IField;
				if (f != null)
				{
					CompilerReport.Add(Errors.ABC.IncompatibleField, f.GetFullName(), v);
					return;
				}
			}
		}
    }
}