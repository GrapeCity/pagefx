using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CLI.Collections;
using DataDynamics.PageFX.CLI.Tables;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.PDB;
using MethodBody=DataDynamics.PageFX.CLI.IL.MethodBody;

namespace DataDynamics.PageFX.CLI
{
    /// <summary>
    /// Represents loader of CLI managed assemblies.
    /// Implementation of Code Model Deserializer for CLI.
    /// </summary>
    internal sealed class AssemblyLoader : IAssemblyLoader, IMethodContext, IDisposable
    {
		private AssemblyLoader _corlib;
	    private readonly TypeSpecTable _typeSpec;
		private readonly MemberRefTable _memberRef;
		private readonly MethodSpecTable _methodSpec;
		private readonly SignatureResolver _signatureResolver;

	    internal IAssembly Assembly { get; private set; }
	    internal IModule MainModule { get { return Assembly.MainModule; } }
	    internal MdbReader Mdb { get; private set; }

	    internal AssemblyRefTable AssemblyRefs { get; private set; }
		internal ModuleRefTable ModuleRefs { get; private set; }
		internal ModuleTable Modules { get; private set; }
		internal FileTable Files { get; private set; }
		internal ManifestResourceTable ManifestResources { get; private set; }
	    internal ConstantTable Const { get; private set; }
	    internal ParamTable Parameters { get; private set; }
	    internal GenericParamTable GenericParameters { get; private set; }
	    internal FieldTable Fields { get; private set; }
	    internal MethodTable Methods { get; private set; }
	    internal PropertyTable Properties { get; private set; }
	    internal EventTable Events { get; private set; }
	    internal TypeTable Types { get; private set; }
	    internal TypeRefTable TypeRefs { get; private set; }

		public static IAssembly Load(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");
			if (!Path.IsPathRooted(path))
				path = Path.Combine(Environment.CurrentDirectory, path);
			var loader = new AssemblyLoader();
			return loader.LoadFromFile(path);
		}

		public static IAssembly Load(Stream s)
		{
			var loader = new AssemblyLoader();
			return loader.LoadFromStream(s);
		}

	    private IAssembly LoadFromFile(string path)
        {
            Mdb = new MdbReader(path);
            //_mdb.Dump(@"c:\mdb.xml");
		    Assembly = new AssemblyImpl
			    {
					Loader = this,
				    Location = path
			    };
            LoadCore();
            return Assembly;
        }

		private IAssembly LoadFromStream(Stream s)
        {
            Mdb = new MdbReader(s);
			Assembly = new AssemblyImpl {Loader = this};
            LoadCore();
            return Assembly;
        }

#if PERF
        public static int TotalTime;
#endif

        private void LoadCore()
        {
            if (LoadAssemblyTable()) return;

#if PERF
            int start = Environment.TickCount;
#endif

            LoadTables();

#if PERF
            int time = Environment.TickCount - start;
            Console.WriteLine("AssemblyLoader: {0} loaded in {1}ms", _assembly.Name, time);
            TotalTime += time;
#endif
        }

		public AssemblyLoader()
		{
			_signatureResolver = new SignatureResolver(this);

			Const = new ConstantTable(this);
			Files = new FileTable(this);
			ManifestResources = new ManifestResourceTable(this);

			Modules = new ModuleTable(this);

			ModuleRefs = new ModuleRefTable(this);
			AssemblyRefs = new AssemblyRefTable(this);

			TypeRefs = new TypeRefTable(this);
			_memberRef = new MemberRefTable(this);
			_typeSpec = new TypeSpecTable(this);
			_methodSpec = new MethodSpecTable(this);

			Parameters = new ParamTable(this);
			Fields = new FieldTable(this);
			Properties = new PropertyTable(this);
			Events = new EventTable(this);
			GenericParameters = new GenericParamTable(this);

			Methods = new MethodTable(this);
			Types = new TypeTable(this);
		}

        private void LoadTables()
        {
            //To avoid circular references assembly is added to cache
            AssemblyResolver.AddToCache(Assembly);

            Loaders.Add(this);

			// load modules
			foreach (var mod in Modules)
	        {
		        Assembly.Modules.Add(mod);
	        }

	        LoadCorlib();

	        //TODO: eliminate SystemTypes
	        //Types.Load();
        }

	    #region LoadAssemblyTable

        private bool LoadAssemblyTable()
        {
	        int n = Mdb.GetRowCount(MdbTableId.Assembly);
	        if (n > 1)
		        throw new BadMetadataException("The Assembly table shall contain zero or one row");

	        var table = new AssemblyTable(this);
	        var asmref = table[0];

	        var asm = AssemblyResolver.GetFromCache(asmref);
	        if (asm != null)
	        {
		        Assembly = asm;
		        return true;
	        }

	        Assembly.Name = asmref.Name;
	        Assembly.Version = asmref.Version;
	        Assembly.Flags = asmref.Flags;
			Assembly.HashAlgorithm = asmref.HashAlgorithm;
	        Assembly.PublicKey = asmref.PublicKey;
	        Assembly.PublicKeyToken = asmref.PublicKeyToken;
	        Assembly.Culture = asmref.Culture;

			var token = MdbIndex.MakeToken(MdbTableId.Assembly, 1);
	        Assembly.MetadataToken = token;
			
	        ((CustomAttributeProvider)Assembly).CustomAttributes = new CustomAttributes(this, Assembly);

	        return false;
        }

	    #endregion

        #region Assembly Refs
        internal IAssembly ResolveAssembly(IAssemblyReference r)
        {
            return AssemblyResolver.ResolveAssembly(r, Assembly.Location);
        }

	    public void ResolveAssemblyReferences()
	    {
		    AssemblyRefs.Load();
        }

	    public IMethod ResolveEntryPoint()
	    {
			MdbIndex token = Mdb.CLIHeader.EntryPointToken;
		    if (token.Table != MdbTableId.MethodDef)
			    return null;
		    int index = token.Index - 1;
		    if (index < 0 || index >= Methods.Count)
			    return null;
		    return Methods[index];
	    }

	    private void LoadCorlib()
        {
            int n = AssemblyRefs.Count;
            if (n == 0)
            {
                Assembly.IsCorlib = true;
	            _corlib = this;
            }
            else
            {
				foreach (var asm in AssemblyRefs)
				{
					if (asm.IsCorlib)
					{
						_corlib = (AssemblyLoader)((AssemblyImpl)asm).Loader;
						break;
					}						
				}
            }
        }
        #endregion

	    internal IType ResolveDeclType(ITypeMember member)
		{
			return Types.ResolveDeclType(member);
		}

	    public MethodBody LoadMethodBody(IMethod method, uint rva)
        {
            var reader = Mdb.SeekRVA(rva);
            var body = new MethodBody(method, this, reader);
            method.Body = body;
            return body;
        }

	    internal ITypeMember GetMemberRef(int index, Context context)
        {
            return _memberRef.Get(index, context);
        }

	    internal IMethod GetMethodDefOrRef(MdbIndex i, Context context)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case MdbTableId.MethodDef:
                    return Methods[index];

                case MdbTableId.MemberRef:
                    return GetMemberRef(index, context) as IMethod;

                default:
                    throw new ArgumentOutOfRangeException("i");
            }
        }

	    private IMethod GetMethodSpec(int index, Context context)
        {
            return _methodSpec.Get(index, context);
        }

	    internal IType GetTypeSpec(int index, Context context)
        {
            return _typeSpec.Get(index, context);
        }

	    internal IType GetTypeDefOrRef(MdbIndex i, Context context)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case MdbTableId.TypeDef:
                    if (index < 0) return ResolveSystemType(SystemTypes.Object, "System.Object");
                    return Types[index];

                case MdbTableId.TypeRef:
                    return TypeRefs[index];

                case MdbTableId.TypeSpec:
                    return GetTypeSpec(index, context);

                default:
                    throw new ArgumentOutOfRangeException("i");
            }
        }

	    internal IType ResolveType(MdbTypeSignature sig, Context context)
        {
            return _signatureResolver.ResolveType(sig, context);
        }

	    #region IMethodContext Members
        public IVariableCollection ResolveLocalVariables(IMethod method, int sig, out bool hasGenericVars)
        {
            hasGenericVars = false;
            
            var list = new VariableCollection();
            if (sig == 0) return list;

	        var context = new Context(method);
            MdbIndex idx = sig;
            var row = Mdb.GetRow(MdbTableId.StandAloneSig, idx.Index - 1);
            var blob = row[MDB.StandAloneSig.Signature].Blob;
            using (var reader = new BufferedBinaryReader(blob))
            {
                int prolog = reader.ReadPackedInt();
                if (prolog != 0x07)
                    throw new BadSignatureException("Invalid local variable signature.");

                int varCount = reader.ReadPackedInt();
                for (int i = 0; i < varCount; ++i)
                {
                    var typeSig = MdbSignature.DecodeTypeSignature(reader);
                    var type = ResolveType(typeSig, context);

                    if (!hasGenericVars && GenericType.IsGenericContext(type))
                        hasGenericVars = true;

	                var v = new Variable
		                {
			                Index = i,
			                Type = type,
			                Name = string.Format("v{0}", i)
		                };
                    list.Add(v);
                }
            }
            return list;
        }

        public IType ResolveType(IMethod method, int sig)
        {
            if (method == null) return null;

            MdbIndex i = sig;
			var type = GetTypeDefOrRef(i, new Context(method));

            return type;
        }

        public object ResolveMetadataToken(IMethod method, int token)
        {
            uint msb = (uint)token >> 24;
            int index = token & 0xFFFFFF;
            if (msb == 0x70)
                return Mdb.GetUserString((uint)index);

            var tableId = (MdbTableId)msb;
	        var context = new Context(method);
	        switch (tableId)
            {
                case MdbTableId.TypeRef:
                case MdbTableId.TypeDef:
                case MdbTableId.TypeSpec:
                    return GetTypeDefOrRef(token, context);

                case MdbTableId.Field:
                    return Fields[index - 1];

                case MdbTableId.MethodDef:
                    return Methods[index - 1];

                case MdbTableId.MemberRef:
					return GetMemberRef(index - 1, context);

                case MdbTableId.MethodSpec:
					return GetMethodSpec(index - 1, context);

                case MdbTableId.StandAloneSig:
                    return null;
            }
            return null;
        }
        #endregion

        #region DebugInfo
		private bool _initDebugInfo = true;
		private PdbReader _pdbReader;

        private bool IsFrameworkLib
        {
            get 
            {
                if (Assembly.Location.ComparePath(GlobalSettings.GetCorlibPath(false)) == 0)
                    return true;
                return AssemblyResolver.IsFrameworkAssembly(Assembly);
            }
        }

		private PdbReader CreatePdbReader()
        {
            if (!GlobalSettings.EmitDebugInfo) return null;

            //NOTE: Since we provide .NET framework libs in binary form it is not needed to emit debug info for them.
            if (IsFrameworkLib) return null;

            string path = Assembly.Location;
            if (string.IsNullOrEmpty(path)) return null; //from stream?
            
            return path.GetPdbReader();
        }

        public void LinkDebugInfo(IMethodBody body)
        {
            var cilBody = body as MethodBody;
            if (cilBody == null) return;

            if (_initDebugInfo)
            {
                _initDebugInfo = false;
                _pdbReader = CreatePdbReader();
            }

            if (_pdbReader == null) return;

            var symMethod = _pdbReader.SymReader.GetSymbolMethod(body.Method);
            if (symMethod == null) return;

            cilBody.LinkSequencePoints(symMethod);
            cilBody.LocalVariables.SetNames(symMethod.RootScope);
            cilBody.LocalVariables.SetGoodNames();
        }
        #endregion

        #region IDisposable
        public static List<AssemblyLoader> Loaders = new List<AssemblyLoader>();

	    public static void Clean()
	    {
		    GenericParamTable.ResetId();
            while (Loaders.Count > 0)
            {
                var al = Loaders[0];
                Loaders.RemoveAt(0);
                al.Dispose();
            }
        }

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
                if (_pdbReader != null)
                {
                    _pdbReader.Dispose();
                    _pdbReader = null;
                }
            }
            // Free your own state (unmanaged objects).
        }

        ~AssemblyLoader()
        {
            Dispose(false);
        }
        #endregion

		private IType ResolveSystemType(IType type, string fullName)
		{
			return type ?? FindSystemType(fullName);
		}

	    public IType FindSystemType(string fullName)
	    {
		    return _corlib.Types[fullName];
	    }
    }
}