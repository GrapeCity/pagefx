using System;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;
using DataDynamics.PageFX.Core.LoaderInternals;
using DataDynamics.PageFX.Core.LoaderInternals.Tables;
using DataDynamics.PageFX.Core.Metadata;
using DataDynamics.PageFX.Core.Pdb;
using MethodBody = DataDynamics.PageFX.Core.IL.MethodBody;

namespace DataDynamics.PageFX.Core
{
    /// <summary>
    /// Represents loader of CLI managed assemblies.
    /// Implementation of Code Model Deserializer for CLI.
    /// </summary>
    internal sealed class AssemblyLoader : IAssemblyLoader, IMethodContext, IDisposable
    {
	    private readonly TypeSpecTable _typeSpec;
	    private readonly MethodSpecTable _methodSpec;
	    private readonly AssemblyTable _assemblyTable;
		private readonly SignatureResolver _signatureResolver;

		internal string Location { get; private set; }

	    internal IAssembly Assembly
	    {
		    get
		    {
				if (_assemblyTable.Count != 1)
					throw new BadMetadataException("The Assembly table shall contain zero or one row");

			    return _assemblyTable[0];
		    }
	    }

	    internal IModule MainModule { get { return Assembly.MainModule; } }

	    internal MetadataReader Metadata { get; private set; }

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
	    internal MemberRefTable MemberRefs { get; private set; }

	    internal AssemblyLoader CorlibLoader
	    {
		    get { return _corlibLoader ?? (_corlibLoader = ResolveCorlibLoader()); }
	    }
		private AssemblyLoader _corlibLoader;

	    internal IAssembly Corlib
	    {
			get { return CorlibLoader.Assembly; }
	    }

	    internal SystemTypes SystemTypes
	    {
			get { return Corlib.SystemTypes; }
	    }

	    internal TypeFactory TypeFactory
	    {
		    get { return Corlib.TypeFactory; }
	    }

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
            Metadata = new MetadataReader(path);
		    Location = path;

            LoadCore();

            return Assembly;
        }

		private IAssembly LoadFromStream(Stream s)
        {
            Metadata = new MetadataReader(s);

			LoadCore();

            return Assembly;
        }

        private void LoadCore()
        {
	        //To avoid circular references assembly is added to cache
	        AssemblyResolver.AddToCache(Assembly);
        }

		public AssemblyLoader()
		{
			_signatureResolver = new SignatureResolver(this);

			_assemblyTable = new AssemblyTable(this);

			Const = new ConstantTable(this);
			Files = new FileTable(this);
			ManifestResources = new ManifestResourceTable(this);

			Modules = new ModuleTable(this);

			ModuleRefs = new ModuleRefTable(this);
			AssemblyRefs = new AssemblyRefTable(this);

			TypeRefs = new TypeRefTable(this);
			MemberRefs = new MemberRefTable(this);
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

		internal IAssembly ResolveAssembly(IAssemblyReference r)
        {
            return AssemblyResolver.ResolveAssembly(r, Assembly.Location);
        }

	    public IMethod ResolveEntryPoint()
	    {
			SimpleIndex token = Metadata.EntryPointToken;
		    if (token.Table != TableId.MethodDef)
			    return null;
		    int index = token.Index - 1;
		    if (index < 0 || index >= Methods.Count)
			    return null;
		    return Methods[index];
	    }

	    public event EventHandler<TypeEventArgs> TypeLoaded;

		internal void FireTypeLoaded(IType type)
		{
			if (TypeLoaded != null)
				TypeLoaded(this, new TypeEventArgs(type));
		}

	    private AssemblyLoader ResolveCorlibLoader()
        {
            int n = AssemblyRefs.Count;
            if (n == 0)
            {
	            return this;
            }

		    var assembly = AssemblyRefs.FirstOrDefault(x => x.IsCorlib());
		    if (assembly == null)
			    throw new InvalidOperationException();
		    return (AssemblyLoader)assembly.Loader;
        }

	    internal IType ResolveDeclType(ITypeMember member)
		{
			return Types.ResolveDeclType(member);
		}

	    public IClrMethodBody LoadMethodBody(IMethod method, uint rva)
        {
            var reader = Metadata.MoveToVirtualAddress(rva);
            return new MethodBody(method, this, reader);
        }

	    internal IMethod GetMethodDefOrRef(SimpleIndex i, Context context)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case TableId.MethodDef:
                    return Methods[index];

                case TableId.MemberRef:
                    return MemberRefs.Get(index, context) as IMethod;

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

	    internal IType GetTypeDefOrRef(SimpleIndex i, Context context)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case TableId.TypeDef:
                    if (index < 0) return SystemTypes.Object;
                    return Types[index];

                case TableId.TypeRef:
                    return TypeRefs[index];

                case TableId.TypeSpec:
                    return GetTypeSpec(index, context);

                default:
                    throw new ArgumentOutOfRangeException("i");
            }
        }

	    internal IType ResolveType(TypeSignature sig, Context context)
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
	        SimpleIndex idx = sig;
	        var row = Metadata.GetRow(TableId.StandAloneSig, idx.Index - 1);

			var reader = row[Schema.StandAloneSig.Signature].Blob;
	        int prolog = reader.ReadPackedInt();
	        if (prolog != 0x07)
		        throw new BadSignatureException("Invalid local variable signature.");

	        int varCount = reader.ReadPackedInt();
	        for (int i = 0; i < varCount; ++i)
	        {
		        var typeSig = TypeSignature.Decode(reader);
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

	        return list;
        }

	    public IType ResolveType(IMethod method, int sig)
        {
            if (method == null) return null;

            SimpleIndex i = sig;
			var type = GetTypeDefOrRef(i, new Context(method));

            return type;
        }

        public object ResolveMetadataToken(IMethod method, int token)
        {
            uint msb = (uint)token >> 24;
            int index = token & 0xFFFFFF;
            if (msb == 0x70)
                return Metadata.GetUserString((uint)index);

            var tableId = (TableId)msb;
	        var context = new Context(method);
	        switch (tableId)
            {
                case TableId.TypeRef:
                case TableId.TypeDef:
                case TableId.TypeSpec:
                    return GetTypeDefOrRef(token, context);

                case TableId.Field:
                    return Fields[index - 1];

                case TableId.MethodDef:
                    return Methods[index - 1];

                case TableId.MemberRef:
					return MemberRefs.Get(index - 1, context);

                case TableId.MethodSpec:
					return GetMethodSpec(index - 1, context);

                case TableId.StandAloneSig:
                    return null;
            }
            return null;
        }
        #endregion

        #region DebugInfo
		private bool _initDebugInfo = true;
		private ISymbolLoader _pdbReader;

        private bool IsFrameworkLib
        {
            get 
            {
                if (Assembly.Location.ComparePath(GlobalSettings.GetCorlibPath(false)) == 0)
                    return true;
                return AssemblyResolver.IsFrameworkAssembly(Assembly);
            }
        }

	    private ISymbolLoader CreatePdbReader()
        {
            if (!GlobalSettings.EmitDebugInfo) return null;

			//TODO: add option to enable emit debug info for framework libs
            //NOTE: Since we provide .NET framework libs in binary form it is not needed to emit debug info for them.
            if (IsFrameworkLib) return null;

            string path = Assembly.Location;
            if (string.IsNullOrEmpty(path)) return null; //from stream?
            
            return SymbolLoader.Create(path);
        }

        public void LinkDebugInfo(IMethodBody body)
        {
            var clrBody = body as IClrMethodBody;
            if (clrBody == null) return;

            if (_initDebugInfo)
            {
                _initDebugInfo = false;
                _pdbReader = CreatePdbReader();
            }

            if (_pdbReader == null) return;

			if (_pdbReader.LoadSymbols(clrBody))
			{
				clrBody.LocalVariables.UnifyAndNormalizeNames();
			}
        }
        #endregion

	    public void Dispose()
        {
			if (_pdbReader != null)
			{
				_pdbReader.Dispose();
				_pdbReader = null;
			}

			if (Metadata != null)
			{
				Metadata.Dispose();
				Metadata = null;
			}
        }

	    public IReadOnlyList<IType> GetExposedTypes()
	    {
		    return Types.GetExposedTypes();
	    }
    }
}