using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.CLI.CLI.Tables;
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
    sealed class AssemblyLoader : IMethodContext, IDisposable, IAssemblyReferencesResolver
    {
        #region Shared Members
        public static IAssembly Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);
            var reader = new AssemblyLoader();
            return reader.LoadFromFile(path);
        }

        public static IAssembly Load(Stream s)
        {
            var reader = new AssemblyLoader();
            return reader.LoadFromStream(s);
        }
        #endregion

		private MdbReader _mdb;
		private IAssembly _assembly;
		private FileTable _files;
		private ManifestResourceTable _manifestResources;
		private ConstantTable _const;
		private AssemblyRefTable _assemblyRef;
		private ModuleTable _module;
		private ModuleRefTable _moduleRef;
		private IType[] _typeRef;
		private TypeTable _typeDef;
		private IType[] _typeSpec;
		private IType[] _interfaceImpl;
		private FieldTable _fields;
		private MethodTable _methodDef;
		private ITypeMember[] _memberRef;
		private IMethod[] _methodSpec;
		private ParamTable _parameters;
		private GenericParamTable _genericParam;
		private PropertyTable _property;
        private EventTable _event;
		private ClassLayoutTable _classLayout;

		internal IAssembly Assembly { get { return _assembly; } }
		internal IModule MainModule { get { return _assembly.MainModule; } }
	    internal MdbReader Mdb { get { return _mdb; } }
	    internal ConstantTable Const { get { return _const; } }
	    internal ClassLayoutTable ClassLayout { get { return _classLayout; } }
	    internal FileTable Files { get { return _files; } }
		internal ManifestResourceTable ManifestResources { get { return _manifestResources; } }
		internal ParamTable Parameters { get { return _parameters; } }
		internal GenericParamTable GenericParameters { get { return _genericParam; } }
		internal FieldTable Fields { get { return _fields; } }
		internal MethodTable Methods { get { return _methodDef; } }

        #region Loading Process
        #region LoadFromFile, LoadFromStream
        IAssembly LoadFromFile(string path)
        {
            _mdb = new MdbReader(path);
            //_mdb.Dump(@"c:\mdb.xml");
            _assembly = new AssemblyImpl {Location = path};
            LoadCore();
            return _assembly;
        }

        IAssembly LoadFromStream(Stream s)
        {
            _mdb = new MdbReader(s);
            _assembly = new AssemblyImpl();
            LoadCore();
            return _assembly;
        }
        #endregion

        #region LoadCore, LoadTables
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

        private void LoadTables()
        {
            //To avoid circular references assembly is added to cache
            AssemblyResolver.AddToCache(_assembly);

            _loaders.Add(this);

			// THE ORDER IS IMPORTANT!!!
			_const = new ConstantTable(_mdb);
			_files = new FileTable(this);
	        _manifestResources = new ManifestResourceTable(this);

			// load modules
	        _module = new ModuleTable(this);
			foreach (var mod in _module)
	        {
		        _assembly.Modules.Add(mod);
	        }

			_moduleRef = new ModuleRefTable(this);
			_assemblyRef = new AssemblyRefTable(this);

	        LoadCorlib();

	        _parameters = new ParamTable(this);
	        _fields = new FieldTable(this);
			_property = new PropertyTable(this);
			_event = new EventTable(this);
			_classLayout = new ClassLayoutTable();
	        _genericParam = new GenericParamTable(_mdb);

			_methodDef = new MethodTable(this);

			//TODO: remove loading, do lazy loading
	        _methodDef.Load();
	        
	        _typeDef = new TypeTable(this);
	        _typeDef.Load();
			
	        LoadNestedClassTable();
            RegisterTypes();
            
            ResolveFieldSignatures();
            ResolveMethodSignatures();

            SetBaseType();
            LoadGenericParamConstraintTable();
	        
	        LoadMethodSemanticsTable();
            LoadMethodImplTable();
            LoadInterfaceImplTable();

            LoadCustomAttribute();
            LoadFieldRVA();

            if (SortMembers)
                Sort();
        }
        #endregion

        #region LoadAssemblyTable
        bool LoadAssemblyTable()
        {
            int n = _mdb.GetRowCount(MdbTableId.Assembly);
            if (n > 1)
                throw new BadMetadataException("The Assembly table shall contain zero or one row");

            if (n == 1)
            {
                var row = _mdb.GetRow(MdbTableId.Assembly, 0);

                var asmref = new AssemblyReference
                {
                    Name = row[MDB.Assembly.Name].String,
                    Version = GetVersion(row, 1),
                    Flags = ((AssemblyFlags)row[MDB.Assembly.Flags].Value),
                    Culture = row[MDB.Assembly.Culture].Culture
                };

                var alg = (HashAlgorithmId)row[MDB.Assembly.HashAlgId].Value;
                if ((asmref.Flags & AssemblyFlags.PublicKey) != 0)
                {
                    asmref.PublicKey = row[MDB.Assembly.PublicKey].Blob;
                    asmref.PublicKeyToken = asmref.PublicKey.ComputePublicKeyToken(alg);
                }

                var asm = AssemblyResolver.GetFromCache(asmref);
                if (asm != null)
                {
                    _assembly = asm;
                    return true;
                }

                _assembly.HashAlgorithm = alg;

                _assembly.Name = asmref.Name;
                _assembly.Version = asmref.Version;
                _assembly.Flags = asmref.Flags;

                _assembly.PublicKey = asmref.PublicKey;
                _assembly.PublicKeyToken = asmref.PublicKeyToken;
                _assembly.Culture = asmref.Culture;
            }

            return false;
        }

        public static bool SortMembers;
        #endregion

        #region LoadFieldRVA
        static int GetFieldTypeSize(IType type)
        {
            if (type.Layout != null)
                return type.Layout.Size;
            var st = type.SystemType;
            if (st == null)
                return -1;
            return st.Size;
        }

        void LoadFieldRVA()
        {
            int n = _mdb.GetRowCount(MdbTableId.FieldRVA);
            for (int i = 0; i < n; ++i)
            {
                var row = _mdb.GetRow(MdbTableId.FieldRVA, i);

                uint rva = row[MDB.FieldRVA.RVA].Value;
                int fieldIndex = row[MDB.FieldRVA.Field].Index - 1;

                var f = _fields[fieldIndex];

                var type = f.Type;
                int size = GetFieldTypeSize(type);
                if (size > 0)
                {
                    var reader = _mdb.SeekRVA(rva);
                    f.Value = reader.ReadBlock(size);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        #endregion

        #region LoadNestedClassTable
        void LoadNestedClassTable()
        {
            foreach (var row in _mdb.GetRows(MdbTableId.NestedClass))
            {
                int nestedIndex = row[MDB.NestedClass.Class].Index - 1;
                int enclosingIndex = row[MDB.NestedClass.EnclosingClass].Index - 1;
                var nestedType = _typeDef[nestedIndex];
                var enclosingType = _typeDef[enclosingIndex];
                nestedType.DeclaringType = enclosingType;
                enclosingType.Types.Add(nestedType);
            }
        }
        #endregion

        #region RegisterTypes

        void RegisterTypes()
        {
	        foreach (var type in _typeDef)
	        {
				RegisterType(type);
	        }
        }

        void RegisterType(IType type)
        {
            var mod = MainModule;
            type.Module = mod;
            mod.Types.Add(type);

            if (type.DeclaringType != null)
            {
                var ns = mod.Namespaces[type.Namespace];
                ns.Types.Add(type);
            }
        }

        #endregion

        #region Assembly Refs
        internal IAssembly ResolveAssembly(IAssemblyReference r)
        {
            return AssemblyResolver.ResolveAssembly(r, _assembly.Location);
        }

	    public void ResolveAssemblyReferences()
        {
	        foreach (var asm in _assemblyRef)
	        {
		        Trace.WriteLine(asm.Name);
	        }
        }

        private void LoadCorlib()
        {
            int n = _assemblyRef.Count;
            if (n == 0)
            {
                _assembly.IsCorlib = true;
            }
            else
            {
                for (int i = 0; i < n; ++i)
                {
                    var asm = _assemblyRef[i];
                    if (asm.IsCorlib)
                        break;
                }
            }
        }
        #endregion

	    #region LoadTypeRefTable
        ITypeContainer GetTypeContainer(MdbIndex idx)
        {
            switch (idx.Table)
            {
                case MdbTableId.Module:
                    return _module[idx.Index - 1];

                case MdbTableId.ModuleRef:
                    return _moduleRef[idx.Index - 1];

                case MdbTableId.AssemblyRef:
                    return _assemblyRef[idx.Index - 1];

                case MdbTableId.TypeRef:
                    return GetTypeRef(idx.Index - 1);

                default:
                    throw new BadMetadataException();
            }
        }

        IType FindType(MdbIndex rs, string fullname)
        {
            var c = GetTypeContainer(rs);
            if (c != null)
                return c.Types[fullname];
            return null;
        }

        static string QName(string ns, string name)
        {
            if (string.IsNullOrEmpty(ns)) return name;
            return ns + "." + name;
        }

        IType GetTypeRef(int index)
        {
            if (_typeRef == null)
            {
                int n = _mdb.GetRowCount(MdbTableId.TypeRef);
                _typeRef = new IType[n];
            }

            if (_typeRef[index] != null)
                return _typeRef[index];

            var row = _mdb.GetRow(MdbTableId.TypeRef, index);
            MdbIndex rs = row[MDB.TypeRef.ResolutionScope].Value;
            string name = row[MDB.TypeRef.TypeName].String;
            string ns = row[MDB.TypeRef.TypeNamespace].String;
            string fullname = QName(ns, name);

            var type = FindType(rs, fullname);

            _typeRef[index] = type;

            if (type == null)
            {
                //TODO: Report error
#if DEBUG
                if (DebugHooks.BreakInvalidTypeReference)
                {
                    Debugger.Break();
                    FindType(rs, fullname);
                }
#endif
                throw new BadMetadataException(string.Format("Unable to resolve type reference {0}", fullname));
            }
            return type;
        }
        #endregion

        #region SetBaseType
        void SetBaseType()
        {
			//TODO: move to TypeTable
            int n = _typeDef.Count;
            for (int i = 0; i < n; ++i)
            {
                var type = _typeDef[i];
                if (type == SystemTypes.Object) continue;
                var row = _mdb.GetRow(MdbTableId.TypeDef, i);
                MdbIndex baseIndex = row[MDB.TypeDef.Extends].Value;
                var baseType = GetTypeDefOrRef(baseIndex, type, type.DeclaringMethod);
                type.BaseType = baseType;
                var myType = type as UserDefinedType;
                if (myType != null && myType.TypeKind != TypeKind.Primitive)
                {
                    if (baseType == SystemTypes.Enum)
                        myType.TypeKind = TypeKind.Enum;
                    else if (baseType == SystemTypes.ValueType)
                        myType.TypeKind = TypeKind.Struct;
                    else if (baseType == SystemTypes.Delegate || baseType == SystemTypes.MulticastDelegate)
                        myType.TypeKind = TypeKind.Delegate;
                }
            }
        }
        #endregion

        #region LoadGenericParamConstraintTable
        void LoadGenericParamConstraintTable()
        {
			//TODO: generic parameter constraints
            /*const MdbTableId tableId = MdbTableId.GenericParamConstraint;
            int n = _mdb.GetRowCount(tableId);
            for (int i = 0; i < n; ++i)
            {
                var row = _mdb.GetRow(tableId, i);
                int index = row[MDB.GenericParamConstraint.Owner].Index - 1;
                IGenericParameter gparam = _genericParam[index];
                MdbIndex cid = row[MDB.GenericParamConstraint.Constraint].Value;

                var declType = gparam.DeclaringType;
                if (gparam.DeclaringMethod != null)
                    declType = gparam.DeclaringMethod.DeclaringType;

                var constraint = GetTypeDefOrRef(cid, declType, gparam.DeclaringMethod);
                if (constraint == null)
                    throw new BadMetadataException(string.Format("Invalid constraint index {0}", cid));

                if (constraint.TypeKind == TypeKind.Interface)
                {
                    gparam.Interfaces.Add(constraint);
                }
                else
                {
                    //if (gparam.BaseType != null)
                    //    throw new BadMetadataException(string.Format("Multiple derive types in generic param {0}", gparam));
                    //gparam.BaseType = constraint;

                    if (gparam.BaseType == null)
                        gparam.BaseType = constraint;
                }
            }*/
        }
        #endregion

        #region ResolveFieldSignatures
        void ResolveFieldSignatures()
        {
            int n = _fields.Count;
            for (int i = 0; i < n; ++i)
            {
                var field = _fields[i];
                if (field.DeclaringType == null)
                    throw new BadMetadataException(string.Format("Field {0}[{1}] has no declaring type", field.Name, i));
                var row = _mdb.GetRow(MdbTableId.Field, i);
                var sigBlob = row[MDB.Field.Signature].Blob;
                var sig = MdbSignature.DecodeFieldSignature(sigBlob);
                field.Type = ResolveTypeSignature(sig.Type, field.DeclaringType, null);
            }
        }
        #endregion

        #region ResolveMethodSignatures
        void ResolveMethodSignatures()
        {
            int n = _methodDef.Count;
            for (int i = 0; i < n; ++i)
            {
                var method = _methodDef[i];
                var declType = method.DeclaringType;
                if (declType == null)
                    throw new BadMetadataException(string.Format("Method {0}[{1}] has no declaring type", method.Name, i));

                var row = _mdb.GetRow(MdbTableId.MethodDef, i);
                var sigBlob = row[MDB.MethodDef.Signature].Blob;
                var sig = MdbSignature.DecodeMethodSignature(sigBlob);

                ResolveMethodSignature(method, sig);
            }
        }

        void ResolveMethodSignature(IMethod method, MdbMethodSignature sig)
        {
            var declType = method.DeclaringType;
            method.Type = ResolveTypeSignature(sig.Type, declType, method);

            int n = sig.Params.Length;
            for (int i = 0; i < n; ++i)
            {
                var ptype = ResolveTypeSignature(sig.Params[i], declType, method);
                if (i < method.Parameters.Count)
                {
                    var p = method.Parameters[i];
                    p.Type = ptype;
                }
                else
                {
                    var p = new Parameter(ptype, "arg" + (i + 1), i + 1);
                    method.Parameters.Add(p);
                }
            }
        }
        #endregion

        #region LoadMethodImplTable
        void LoadMethodImplTable()
        {
            int n = _mdb.GetRowCount(MdbTableId.MethodImpl);
            for (int i = 0; i < n; ++i)
            {
                var row = _mdb[MdbTableId.MethodImpl, i];
                int cid = row[MDB.MethodImpl.Class].Index - 1;

                MdbIndex bodyIdx = row[MDB.MethodImpl.MethodBody].Value;
                MdbIndex declIdx = row[MDB.MethodImpl.MethodDeclaration].Value;

                var type = _typeDef[cid];
                _currentType = type;

                var body = GetMethodDefOrRef(bodyIdx);
                _currentMethod = body;
                var decl = GetMethodDefOrRef(declIdx);

                body.ImplementedMethods = new[] { decl };
                body.IsExplicitImplementation = true;

                _currentType = null;
                _currentMethod = null;
            }
        }
        #endregion

        #region LoadInterfaceImplTable
        void LoadInterfaceImplTable()
        {
            int n = _mdb.GetRowCount(MdbTableId.InterfaceImpl);
            _interfaceImpl = new IType[n];
            var ifaces = new IType[n];
            for (int i = 0; i < n; ++i)
            {
                var row = _mdb.GetRow(MdbTableId.InterfaceImpl, i);
                int typeIndex = row[MDB.InterfaceImpl.Class].Index - 1;
                var type = _typeDef[typeIndex];
                MdbIndex ifaceIndex = row[MDB.InterfaceImpl.Interface].Value;

                var iface = GetTypeDefOrRef(ifaceIndex, type, type.DeclaringMethod);

                type.Interfaces.Add(iface);
                _interfaceImpl[i] = type;
                ifaces[i] = iface;
            }

            for (int i = 0; i < n; ++i)
            {
                var type = _interfaceImpl[i];
                var iface = ifaces[i];

                foreach (var ifaceMethod in iface.Methods)
                    AddImplementedMethod(type, ifaceMethod);
            }
        }

        static bool HasExplicitImplementation(IType type, IMethod ifaceMethod)
        {
        	return (from method in type.Methods
					where method.IsExplicitImplementation
					select method.ImplementedMethods).Any(impl => impl != null && impl.Length == 1 && impl[0] == ifaceMethod);
        }

    	static IMethod FindImpl(IType type, IMethod ifaceMethod)
        {
            string mname = ifaceMethod.Name;
            while (type != null)
            {
                var candidates = type.Methods.Find(mname);
                foreach (var method in candidates)
                {
                    if (method.IsExplicitImplementation) continue;
                    if (Signature.Equals(method, ifaceMethod, true, false))
                        return method;
                }
                type = type.BaseType;
            }
            return null;
        }

        static void AddImplementedMethod(IType type, IMethod ifaceMethod)
        {
            if (HasExplicitImplementation(type, ifaceMethod))
                return;

            var method = FindImpl(type, ifaceMethod);
            if (method == null) return;

            if (method.ImplementedMethods == null)
            {
                method.ImplementedMethods = new[] {ifaceMethod};
                return;
            }

            if (method.ImplementedMethods.Contains(ifaceMethod))
                return;

            int n = method.ImplementedMethods.Length;
            var newArr = new IMethod[n + 1];
            Array.Copy(method.ImplementedMethods, newArr, n);
            newArr[n] = ifaceMethod;
            method.ImplementedMethods = newArr;
        }
        #endregion

        #region LoadMethodSemanticsTable
        void LoadMethodSemanticsTable()
        {
            int n = _mdb.GetRowCount(MdbTableId.MethodSemantics);
            for (int i = 0; i < n; ++i)
            {
                var row = _mdb.GetRow(MdbTableId.MethodSemantics, i);

                int methodIndex = row[MDB.MethodSemantics.Method].Index - 1;
                var method = _methodDef[methodIndex];

                var sem = (MethodSemanticsAttributes)row[MDB.MethodSemantics.Semantics].Value;

                MdbIndex assoc = row[MDB.MethodSemantics.Association].Value;
                int assocRowIndex = assoc.Index - 1;
                switch (assoc.Table)
                {
                    case MdbTableId.Property:
                        {
                            var property = _property[assocRowIndex];

                            method.Association = property;
                            switch (sem)
                            {
                            	case MethodSemanticsAttributes.Getter:
                            		property.Getter = method;
                            		break;
                            	case MethodSemanticsAttributes.Setter:
                            		property.Setter = method;
                            		break;
                            	default:
                            		throw new ArgumentOutOfRangeException();
                            }

                            property.ResolveTypeAndParameters();

                            if (property.DeclaringType == null)
                            {
                                method.DeclaringType.Members.Add(property);
                            }
                        }
                        break;

                    case MdbTableId.Event:
                        {
                            var e = _event[assocRowIndex];
                            method.Association = e;

							switch (sem)
							{
								case MethodSemanticsAttributes.AddOn:
									e.Adder = method;
									break;
								case MethodSemanticsAttributes.RemoveOn:
									e.Remover = method;
									break;
								case MethodSemanticsAttributes.Fire:
									e.Raiser = method;
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}

                        	e.ResolveType();

                            if (e.DeclaringType == null)
                            {
                                method.DeclaringType.Members.Add(e);
                            }
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

        #region LoadCustomAttribute
		private void LoadCustomAttribute()
        {
            int n = _mdb.GetRowCount(MdbTableId.CustomAttribute);
            for (int i = 0; i < n; ++i)
            {
                var row = _mdb.GetRow(MdbTableId.CustomAttribute, i);
                MdbIndex parent = row[MDB.CustomAttribute.Parent].Value;
                MdbIndex ctorIndex = row[MDB.CustomAttribute.Type].Value;
                var ctor = GetCustomAttributeConstructor(ctorIndex);
                if (ctor != null)
                {
                    var value = row[MDB.CustomAttribute.Value].Blob;
                    var provider = GetCustomAttributeProvider(parent);
                    if (provider != null)
                    {
                        var attrType = ctor.DeclaringType;
                        var attr = new CustomAttribute
                        {
                            Constructor = ctor,
                            Type = attrType,
                            Owner = provider
                        };
                        if (value != null && value.Length > 0) //non null
                        {
                            ReadArguments(attr, value);
                        }
                        provider.CustomAttributes.Add(attr);
                        ReviewAttribute(attr);
                    }
                    else
                    {
                        //TODO:
                    }
                }
                else
                {
                    //TODO: warning
                }
            }
        }

        IType FindType(string name)
        {
            //Parse assembly qualified type name
            var parts = name.Split(',');
            if (parts.Length > 1) // fully qualified name
            {
                string asmName = string.Empty;
                for (int i = 1; i < parts.Length; i++)
                {
                    if (i != 1) asmName += ", ";
                    asmName += parts[i];
                }
                var r = new AssemblyReference(asmName);
                var asm = ResolveAssembly(r);
                return asm.FindType(parts[0]);
            }

            var type = _assembly.FindType(name);
            if (type != null)
                return type;

            int n = _mdb.GetRowCount(MdbTableId.AssemblyRef);
            for (int i = 0; i < n; ++i)
            {
                var asm = _assemblyRef[i];
                type = asm.FindType(name);
                if (type != null)
                    return type;
            }

            return null;
        }

		private IType ReadType(BufferedBinaryReader reader)
        {
            string s = reader.ReadCountedUtf8();
            return FindType(s);
        }

		private object ReadArray(BufferedBinaryReader reader, ElementType elemType)
        {
            Array arr = null;
            int n = reader.ReadInt32();
            for (int i = 0; i < n; ++i)
            {
                var val = ReadValue(reader, elemType);
                if (arr == null)
                    arr = Array.CreateInstance(val.GetType(), n);
                arr.SetValue(val, i);
            }
            return arr;
        }

		private object ReadArray(BufferedBinaryReader reader, IType elemType)
        {
            Array arr = null;
            int n = reader.ReadInt32();
            for (int i = 0; i < n; ++i)
            {
                var val = ReadValue(reader, elemType);
                if (arr == null)
                    arr = Array.CreateInstance(val.GetType(), n);
                arr.SetValue(val, i);
            }
            return arr;
        }

        private object ReadValue(BufferedBinaryReader reader, ElementType e)
        {
            switch (e)
            {
                case ElementType.Boolean:
                    return reader.ReadBoolean();

                case ElementType.Char:
                    return reader.ReadChar();

                case ElementType.Int8:
                    return reader.ReadSByte();

                case ElementType.UInt8:
                    return reader.ReadByte();

                case ElementType.Int16:
                    return reader.ReadInt16();

                case ElementType.UInt16:
                    return reader.ReadUInt16();

                case ElementType.Int32:
                    return reader.ReadInt32();

                case ElementType.UInt32:
                    return reader.ReadUInt32();

                case ElementType.Int64:
                    return reader.ReadInt64();

                case ElementType.UInt64:
                    return reader.ReadUInt64();

                case ElementType.Single:
                    return reader.ReadSingle();

                case ElementType.Double:
                    return reader.ReadDouble();

                case ElementType.String:
                    return reader.ReadCountedUtf8();

                case ElementType.Object:
                case ElementType.CustomArgsBoxedObject:
                    {
                        var elem = (ElementType)reader.ReadInt8();
                        return ReadValue(reader, elem);
                    }

                case ElementType.CustomArgsEnum:
                    {
                        string enumTypeName = reader.ReadCountedUtf8();
                        var enumType = FindType(enumTypeName);
                        if (enumType == null)
                        {
                            //TODO:
                            throw new BadMetadataException();
                        }
                        return ReadValue(reader, enumType);
                    }

                case ElementType.CustomArgsType:
                    return ReadType(reader);

                case ElementType.ArraySz:
                    {
                        var arrElemType = (ElementType)reader.ReadInt8();
                        return ReadArray(reader, arrElemType);
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //static object EnumToObject(object val, IType type)
        //{
        //    foreach (IField field in type.Fields)
        //    {
        //        if (field.IsStatic)
        //        {
        //            if (Equals(val, field.Value))
        //                return field;
        //        }
        //    }
        //    return val;
        //}

        object ReadValue(BufferedBinaryReader reader, IType type)
        {
            if (type == SystemTypes.Boolean)
                return reader.ReadBoolean();
            if (type == SystemTypes.Char)
                return reader.ReadChar();
            if (type == SystemTypes.Int8)
                return reader.ReadInt8();
            if (type == SystemTypes.UInt8)
                return reader.ReadUInt8();
            if (type == SystemTypes.Int16)
                return reader.ReadInt16();
            if (type == SystemTypes.UInt16)
                return reader.ReadUInt16();
            if (type == SystemTypes.Int32)
                return reader.ReadInt32();
            if (type == SystemTypes.UInt32)
                return reader.ReadUInt32();
            if (type == SystemTypes.Int64)
                return reader.ReadInt64();
            if (type == SystemTypes.UInt64)
                return reader.ReadUInt64();
            if (type == SystemTypes.Single)
                return reader.ReadSingle();
            if (type == SystemTypes.Double)
                return reader.ReadDouble();
            if (type == SystemTypes.String)
                return reader.ReadCountedUtf8();
            if (type == SystemTypes.Type)
                return ReadType(reader);

            if (type.TypeKind == TypeKind.Enum)
            {
                var val = ReadValue(reader, type.ValueType);
                //Type enumType = Type.GetType(type.FullName);
                //return Enum.ToObject(enumType, val);
                return val;
            }

            var arrType = type as IArrayType;
            if (arrType != null)
            {
                int numElem = reader.ReadInt32();
                Array arr = null;
                for (int i = 0; i < numElem; ++i)
                {
                    var val = ReadValue(reader, arrType.ElementType);
                    if (arr == null)
                        arr = Array.CreateInstance(val.GetType(), numElem);
                    arr.SetValue(val, i);
                }
                return arr;
            }

            //boxed value type
            if (type == SystemTypes.Object)
            {
                var e = (ElementType)reader.ReadInt8();
                return ReadValue(reader, e);
            }

            return null;
        }

        void ReadArguments(ICustomAttribute attr, byte[] blob)
        {
            var reader = new BufferedBinaryReader(blob);
            ushort prolog = reader.ReadUInt16();
            if (prolog != 0x01)
                throw new BadSignatureException("Invalid prolog in custom attribute value");

            var ctor = attr.Constructor;
            int numFixed = ctor.Parameters.Count;
            for (int i = 0; i < numFixed; ++i)
            {
                var p = ctor.Parameters[i];
                var arg = new Argument
                              {
                                  Type = p.Type,
                                  Kind = ArgumentKind.Fixed,
                                  Name = p.Name,
                                  Value = ReadValue(reader, p.Type)
                              };
                attr.Arguments.Add(arg);
            }

            int numNamed = reader.ReadUInt16();
            for (int i = 0; i < numNamed; ++i)
            {
                var arg = new Argument 
                {
                    Kind = ((ArgumentKind)reader.ReadUInt8())
                };
                var elemType = (ElementType)reader.ReadUInt8();
                if (elemType == ElementType.CustomArgsEnum)
                {
                    var enumType = ReadEnumType(reader);
                    arg.Type = enumType;
                    arg.Name = reader.ReadCountedUtf8();
                    arg.Value = ReadValue(reader, enumType);
                }
                else if (elemType == ElementType.ArraySz)
                {
                    elemType = (ElementType)reader.ReadUInt8();
                    if (elemType == ElementType.CustomArgsEnum)
                    {
                        var enumType = ReadEnumType(reader);
                        arg.Name = reader.ReadCountedUtf8();
                        ResolveNamedArgType(arg, attr.Type);
                        arg.Value = ReadArray(reader, enumType);
                    }
                    else
                    {
                        arg.Name = reader.ReadCountedUtf8();
                        ResolveNamedArgType(arg, attr.Type);
                        arg.Value = ReadArray(reader, elemType);
                    }
                }
                else
                {
                    arg.Name = reader.ReadCountedUtf8();
                    ResolveNamedArgType(arg, attr.Type);
                    arg.Value = ReadValue(reader, elemType);
                }
                attr.Arguments.Add(arg);
            }
        }

        IType ReadEnumType(BufferedBinaryReader reader)
        {
            string enumTypeName = reader.ReadCountedUtf8();
            return FindType(enumTypeName);
        }

        static void ResolveNamedArgType(IArgument arg, IType declType)
        {
            if (arg.Kind == ArgumentKind.Field)
            {
                var f = declType.FindField(arg.Name, true);
                if (f == null)
                    throw new InvalidOperationException();
                arg.Type = f.Type;
                arg.Member = f;
                return;
            }
            var p = declType.FindProperty(arg.Name, true);
            if (p == null)
                throw new InvalidOperationException();
            arg.Type = p.Type;
            arg.Member = p;
        }

        IMethod GetCustomAttributeConstructor(MdbIndex i)
        {
            try
            {
                switch (i.Table)
                {
                    case MdbTableId.MethodDef:
                        return _methodDef[i.Index - 1];

                    case MdbTableId.MemberRef:
                        return GetMemberRef(i.Index - 1) as IMethod;

                    default:
                        throw new BadMetadataException(string.Format("Invalid custom attribute type index {0}", i));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        ICustomAttributeProvider GetCustomAttributeProvider(MdbIndex i)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case MdbTableId.MethodDef:
                    return _methodDef[index];

                case MdbTableId.Field:
                    return _fields[index];

                case MdbTableId.TypeRef:
                    return GetTypeRef(index);

                case MdbTableId.TypeDef:
                    return _typeDef[index];

                case MdbTableId.Param:
                    return _parameters[index];

                case MdbTableId.Property:
                    return _property[index];

                case MdbTableId.Event:
                    return _event[index];

                case MdbTableId.Module:
                    return _module[index];

                case MdbTableId.ModuleRef:
                    return _moduleRef[index];

                case MdbTableId.TypeSpec:
                    return GetTypeSpec(index, CurrentType, CurrentMethod);

                case MdbTableId.AssemblyRef:
                    return _assemblyRef[index];

                case MdbTableId.Assembly:
                    return _assembly;

                case MdbTableId.InterfaceImpl:
                    return _interfaceImpl[index];

                case MdbTableId.MemberRef:
                    return GetMemberRef(index);

                case MdbTableId.File:
                    return _files[index];

                case MdbTableId.DeclSecurity:
                    return null;

                case MdbTableId.StandAloneSig:
                    return null;

                case MdbTableId.ExportedType:
                    return null;

                case MdbTableId.ManifestResource:
                    return _manifestResources[index];

                case MdbTableId.GenericParam:
                    return _genericParam[index];

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static void ReviewAttribute(ICustomAttribute attr)
        {
            var type = attr.Owner as IType;
            if (type != null)
            {
                if (attr.Type.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute")
                {
                    type.IsCompilerGenerated = true;
                    return;
                }
                return;
            }

            var param = attr.Owner as IParameter;
            if (param != null)
            {
                if (attr.Type.FullName == "System.ParamArrayAttribute")
                {
                    param.CustomAttributes.Remove(attr);
                    param.HasParams = true;
                    return;
                }
                return;
            }
        }
        #endregion

		#region LoadMethodBody
		public MethodBody LoadBody(IMethod method, uint rva)
        {
            var reader = _mdb.SeekRVA(rva);
            _currentMethod = method;
            var body = new MethodBody(this, reader);
            method.Body = body;
            if (CommonLanguageInfrastructure.EnableDecompiler)
            {
                //Decompiler dc = new Decompiler();
                //dc.Decompile(body);
            }
            return body;
        }
        #endregion

        #region Sort
        void Sort()
        {
            _assembly.Modules.Sort();
        }
        #endregion

        #region GetMemberRef
        IType[] ResolveMethodSignature(MdbMethodSignature sig, IType type, IMethod method)
        {
            int n = sig.Params.Length;
            var types = new IType[n + 1];
            types[0] = ResolveTypeSignature(sig.Type, type, method);
            for (int i = 0; i < n; ++i)
            {
                types[i + 1] = ResolveTypeSignature(sig.Params[i], type, method);
            }
            return types;
        }

        IType[] ResolveMethodSignature(MdbMethodSignature sig, IType type)
        {
            return ResolveMethodSignature(sig, type, CurrentMethod);
        }

        static ITypeMember FindMember(IType type, MemberType kind, string name, IType[] types)
        {
            switch (kind)
            {
                case MemberType.Field:
                    {
                        var field = type.Fields[name];
                        if (field != null)
                            return field;
                    }
                    break;

                case MemberType.Property:
                    {
                        foreach (var property in type.Properties)
                        {
                            if (Signature.CheckSignature(property, name, types))
                                return property;
                        }
                    }
                    break;
            }
            return null;
        }

        IType[] ResolveArrayMethodParams(IType contextType, MdbMethodSignature sig)
        {
            int n = sig.Params.Length;
            var types = new IType[n];
            for (int i = 0; i < n; ++i)
            {
                var ptype = ResolveTypeSignature(sig.Params[i], contextType, CurrentMethod);
                types[i] = ptype;
            }
            return types;
        }

        static void CreateParams(IMethod m, IType[] types, string prefix)
        {
            int n = types.Length;
            for (int i = 0; i < n; ++i)
            {
                var p = new Parameter(types[i], prefix + i, i + 1);
                m.Parameters.Add(p);
            }
        }

        IMethod CreateArrayCtor(IType type, MdbMethodSignature sig)
        {
            var types = ResolveArrayMethodParams(type, sig);
            
            var arrType = (ArrayType)type;
            var ctor = arrType.FindConstructor(types);
            if (ctor != null) return ctor;

            var m = new Method
            {
                Name = CLRNames.Constructor,
                Type = SystemTypes.Void,
                DeclaringType = type
            };

            CreateParams(m, types, "n");
            m.IsSpecialName = true;
            m.IsInternalCall = true;
            arrType.Constructors.Add(m);
            return m;
        }

        IMethod GetArrayGetter(IType type, MdbMethodSignature sig)
        {
            var arrType = (ArrayType)type;
            if (arrType.Getter != null)
                return arrType.Getter;

            var m = new Method
                        {
                            Name = CLRNames.Array.Getter,
                            Type = arrType.ElementType,
                            IsInternalCall = true,
                            DeclaringType = type,
                        };
            arrType.Getter = m;

            var contextType = FixContextType(arrType.ElementType);

            var types = ResolveArrayMethodParams(contextType, sig);
            CreateParams(m, types, "i");

            return m;
        }

        IMethod GetArrayAddress(IType type, MdbMethodSignature sig)
        {
            var arrType = (ArrayType)type;
            if (arrType.Address != null)
                return arrType.Address;

            var m = new Method
                        {
                            Name = CLRNames.Array.Address,
                            Type = ResolveTypeSignature(sig.Type, type, null),
                            IsInternalCall = true,
                            DeclaringType = type
                        };

            arrType.Address = m;

            var contextType = FixContextType(arrType.ElementType);

            var types = ResolveArrayMethodParams(contextType, sig);
            CreateParams(m, types, "i");

            return m;
        }

        IMethod GetArraySetter(IType type, MdbMethodSignature sig)
        {
            var arrType = (ArrayType)type;

            if (arrType.Setter != null)
                return arrType.Setter;

            var m = new Method
                        {
                            Name = CLRNames.Array.Setter,
                            Type = SystemTypes.Void,
                            IsInternalCall = true,
                            DeclaringType = type
                        };

            arrType.Setter = m;

            var contextType = FixContextType(arrType.ElementType);

            var types = ResolveArrayMethodParams(contextType, sig);
            int n = types.Length;
            for (int i = 0; i < n - 1; ++i)
            {
                m.Parameters.Add(new Parameter(types[i], "i" + i, i + 1));
            }
            m.Parameters.Add(new Parameter(types[n - 1], "value", n));


            return m;
        }

        static IType FixContextType(IType type)
        {
            var gp = type as IGenericParameter;
            if (gp != null)
            {
                if (gp.DeclaringMethod != null)
                    return gp.DeclaringMethod.DeclaringType;
                return gp.DeclaringType;
            }
            return type;
        }

        IMethod _currentMethodSpec;

        IEnumerable<IMethod> GetMatchedMethods(IType type, string name, MdbMethodSignature sig)
        {
            int paramNum = sig.Params.Length;
            var set = type.Methods.Find(name);
            foreach (var method in set)
            {
                if (method.Parameters.Count != paramNum) continue;
                
                if (_resolvingMethodSpec)
                {
                    if (!method.IsGeneric)
                        continue;
                    if (method.GenericParameters.Count != sig.GenericParamCount)
                        continue;
                    _currentMethodSpec = method;
                }

                if (CheckMethodSig(type, method, sig))
                {
                    _currentMethodSpec = null;
                    yield return method;
                }
            }
        }

        IMethod FindMethod(IType type, string name, MdbMethodSignature sig)
        {
            IMethod result = null;
            while (type != null)
            {
                int curSpec = 0;
                foreach (var method in GetMatchedMethods(type, name, sig))
                {
                    if (!method.SignatureChanged)
                        return method;

                    int spec = Method.GetSpecificity(method);
                    if (result == null || spec > curSpec)
                    {
                        result = method;
                        curSpec = spec;
                    }
                }
                if (result != null) break;
                type = type.BaseType;
            }

            return result;
        }

        bool CheckMethodSig(IType type, IMethod method, MdbMethodSignature sig)
        {
            var t = ResolveTypeSig(sig.Type, type);
            if (!Signature.TypeEquals(method.Type, t))
                return false;

            int n = sig.Params.Length;
            for (int i = 0; i < n; ++i)
            {
                var p = method.Parameters[i];
                var psig = sig.Params[i];
                t = ResolveTypeSig(psig, type);

                if (!Signature.TypeEquals(p.Type, t))
                    return false;
            }

            return true;
        }

        private IType ResolveTypeSig(MdbTypeSignature sig, IType contextType)
        {
        	if (_resolvingMethodSpec)
                return ResolveTypeSignature(sig, contextType, CurrentMethod);
        	return sig.ResolvedType ?? (sig.ResolvedType = ResolveTypeSignature(sig, contextType, CurrentMethod));
        }

    	private ITypeMember GetMemberRef(IType type, string name, MdbSignature sig)
        {
            if (type == null) return null;

            //special multidimensional array methods
            if (type.IsArray)
            {
                if (name == CLRNames.Constructor)
                    return CreateArrayCtor(type, (MdbMethodSignature)sig);

                if (name == CLRNames.Array.Getter)
                    return GetArrayGetter(type, (MdbMethodSignature)sig);

                if (name == CLRNames.Array.Address)
                    return GetArrayAddress(type, (MdbMethodSignature)sig);

                if (name == CLRNames.Array.Setter)
                    return GetArraySetter(type, (MdbMethodSignature)sig);
            }

            IType[] types = null;
            MemberType kind;
            switch (sig.Kind)
            {
                case MdbSignatureKind.Field:
                    kind = MemberType.Field;
                    break;

                case MdbSignatureKind.Method:
                    return FindMethod(type, name, (MdbMethodSignature)sig);

                case MdbSignatureKind.Property:
                    types = ResolveMethodSignature((MdbMethodSignature)sig, type);
                    kind = MemberType.Property;
                    break;

                default:
                    throw new BadSignatureException(string.Format("Invalid member signature {0}", sig));
            }

            while (type != null)
            {
                var member = FindMember(type, kind, name, types);
                if (member != null)
                    return member;
                type = type.BaseType;
            }
            return null;
        }

        IType GetMemberOwner(MdbIndex owner)
        {
            int index = owner.Index - 1;
            switch (owner.Table)
            {
	            case MdbTableId.TypeDef:
		            return _typeDef[index];

	            case MdbTableId.TypeRef:
		            return GetTypeRef(index);

	            case MdbTableId.ModuleRef:
		            throw new NotImplementedException();

	            case MdbTableId.TypeSpec:
		            return GetTypeSpec(index, CurrentType, CurrentMethod);

	            case MdbTableId.MethodDef:
		            throw new NotImplementedException();
            }
	        return null;
        }

        ITypeMember GetMemberRef(int index)
        {
            if (_memberRef == null)
            {
                int n = _mdb.GetRowCount(MdbTableId.MemberRef);
                _memberRef = new ITypeMember[n];
            }

            var member = _memberRef[index];
            if (member != null)
                return _memberRef[index];

            var row = _mdb.GetRow(MdbTableId.MemberRef, index);

            string name = row[MDB.MemberRef.Name].String;
            var sigBlob = row[MDB.MemberRef.Signature].Blob;
            var sig = MdbSignature.DecodeSignature(sigBlob);

            MdbIndex ownerIndex = row[MDB.MemberRef.Class].Value;
            var owner = GetMemberOwner(ownerIndex);

            member = GetMemberRef(owner, name, sig);

            if (member == null)
            {
                //TODO: Report warning
#if DEBUG
                if (DebugHooks.BreakInvalidMemberReference)
                {
                    Debugger.Break();
                    GetMemberRef(owner, name, sig);
                }
#endif
                throw new BadMetadataException(string.Format("Unable to resolve member ref {0}", ownerIndex));
            }
            _memberRef[index] = member;
            return member;
        }
        #endregion

        #region GetMethodDefOrRef
        IMethod GetMethodDefOrRef(MdbIndex i)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case MdbTableId.MethodDef:
                    return _methodDef[index];

                case MdbTableId.MemberRef:
                    return GetMemberRef(index) as IMethod;

                default:
                    throw new ArgumentOutOfRangeException("i");
            }
        }
        #endregion

        #region GetMethodSpec
        bool _resolvingMethodSpec;

        IMethod GetMethodSpec(int index)
        {
            if (_methodSpec == null)
            {
                int n = _mdb.GetRowCount(MdbTableId.MethodSpec);
                _methodSpec = new IMethod[n];
            }

            var spec = _methodSpec[index];
            if (spec != null) return spec;

            _resolvingMethodSpec = true;

            var row = _mdb.GetRow(MdbTableId.MethodSpec, index);
            MdbIndex idx = row[MDB.MethodSpec.Method].Value;
            var method = GetMethodDefOrRef(idx);

            if (method == null)
                throw new BadTokenException(idx);

            var blob = row[MDB.MethodSpec.Instantiation].Blob;
            var args = ReadMethodSpecArgs(blob);

            spec = GenericType.CreateMethodInstance(method.DeclaringType, method, args);
            spec.MetadataToken = MdbIndex.MakeToken(MdbTableId.MethodSpec, index);

            //spec = new GenericMethodInstance(method.DeclaringType, method, args)
            //           {
            //               MetadataToken = MdbIndex.MakeToken(MdbTableId.MethodSpec, index)
            //           };
            _methodSpec[index] = spec;

            _resolvingMethodSpec = false;
            return spec;
        }

        IType[] ReadMethodSpecArgs(byte[] blob)
        {
            var reader = new BufferedBinaryReader(blob);
            if (reader.ReadByte() != 0x0A)
                throw new BadSignatureException("Invalid MethodSpec signature");

            int n = reader.ReadPackedInt();
            var args = new IType[n];
            for (int i = 0; i < n; ++i)
            {
                var sig = MdbSignature.DecodeTypeSignature(reader);
                args[i] = ResolveTypeSignature(sig);
            }
            return args;
        }
        #endregion

        #region GetTypeSpec
        IType GetTypeSpec(int index, IType contextType, IMethod contextMethod)
        {
            if (_typeSpec == null)
            {
                int n = _mdb.GetRowCount(MdbTableId.TypeSpec);
                _typeSpec = new IType[n];
            }

            var type = _typeSpec[index];
            if (type != null) return type;

            var row = _mdb.GetRow(MdbTableId.TypeSpec, index);
            var blob = row[MDB.TypeSpec.Signature].Blob;
            var sig = MdbSignature.DecodeTypeSignature(blob);

            type = ResolveTypeSignature(sig, contextType, contextMethod);
            if (type == null)
                throw new BadMetadataException(string.Format("Unable to resolve signature {0}", sig));
            _typeSpec[index] = type;
            return type;
        }
        #endregion

        #region GetTypeDefOrRef
        IType GetTypeDefOrRef(MdbIndex i, IType contextType, IMethod contextMethod)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case MdbTableId.TypeDef:
                    if (index < 0)
                        return SystemTypes.Object;
                    return _typeDef[index];

                case MdbTableId.TypeRef:
                    return GetTypeRef(index);

                case MdbTableId.TypeSpec:
                    return GetTypeSpec(index, contextType, contextMethod);

                default:
                    throw new ArgumentOutOfRangeException("i");
            }
        }
        #endregion

        #region ResolveTypeSignature
        static Exception BadTypeSig(MdbTypeSignature sig)
        {
            return new BadSignatureException(string.Format("Unable to resolve type signature {0}", sig));            
        }

        IEnumerable<IType> ResolveGenericArgs(MdbTypeSignature sig, IType contextType, IMethod contextMethod)
        {
            int n = sig.GenericParams.Length;
            var args = new IType[n];
            for (int i = 0; i < n; ++i)
            {
                var arg = ResolveTypeSignature(sig.GenericParams[i], contextType, contextMethod);
                if (arg == null)
                    throw BadTypeSig(sig);
                args[i] = arg;
            }
            return args;
        }

        IType ResolveTypeSignature(MdbTypeSignature sig)
        {
            return ResolveTypeSignature(sig, CurrentType, CurrentMethod);
        }

        IType ResolveTypeSignature(MdbTypeSignature sig, IType contextType, IMethod contextMethod)
        {
            switch (sig.Element)
            {
                case ElementType.Void: return SystemTypes.Void;
                case ElementType.Boolean: return SystemTypes.Boolean;
                case ElementType.Char: return SystemTypes.Char;
                case ElementType.Int8: return SystemTypes.Int8;
                case ElementType.UInt8: return SystemTypes.UInt8;
                case ElementType.Int16: return SystemTypes.Int16;
                case ElementType.UInt16: return SystemTypes.UInt16;
                case ElementType.Int32: return SystemTypes.Int32;
                case ElementType.UInt32: return SystemTypes.UInt32;
                case ElementType.Int64: return SystemTypes.Int64;
                case ElementType.UInt64: return SystemTypes.UInt64;
                case ElementType.Single: return SystemTypes.Single;
                case ElementType.Double: return SystemTypes.Double;
                case ElementType.String: return SystemTypes.String;
                case ElementType.TypedReference: return SystemTypes.TypedReference;
                case ElementType.IntPtr: return SystemTypes.IntPtr;
                case ElementType.UIntPtr: return SystemTypes.UIntPtr;
                case ElementType.Object: return SystemTypes.Object;

                case ElementType.Ptr:
                    {
                        var type = ResolveTypeSignature(sig.Type, contextType, contextMethod);
                        if (type == null) return null;
                        return TypeFactory.MakePointerType(type);
                    }

                case ElementType.ByRef:
                    {
                        var type = ResolveTypeSignature(sig.Type, contextType, contextMethod);
                        if (type == null) return null;
                        return TypeFactory.MakeReferenceType(type);
                    }

                case ElementType.ValueType:
                case ElementType.Class:
                case ElementType.CustomArgsEnum:
                    {
                        var type = GetTypeDefOrRef(sig.TypeIndex, contextType, contextMethod);
                        return type;
                    }

                case ElementType.Array:
                case ElementType.ArraySz:
                    {
                        var type = ResolveTypeSignature(sig.Type, contextType, contextMethod);
                        if (type == null) return null;
                        var dim = sig.ArrayShape.ToDimension();
                        return TypeFactory.MakeArray(type, dim);
                    }

                case ElementType.GenericInstantiation:
                    {
                        var type = ResolveTypeSignature(sig.Type, contextType, contextMethod) as IGenericType;
                        if (type == null)
                            throw BadTypeSig(sig);

                        var args = ResolveGenericArgs(sig, contextType, contextMethod);
                        return TypeFactory.MakeGenericType(type, args);
                    }

                case ElementType.MethodPtr:
                    {
                        //TODO:
                        //MdbMethodSignature msig = sig.Method;
                    }
                    break;

                case ElementType.Var:
                    {
                        int index = sig.GenericParamNumber;
                        var gt = contextType as IGenericType;
                        if (gt != null)
                            return gt.GenericParameters[index];
                        var gi = contextType as IGenericInstance;
                        if (gi != null)
                            return gi.GenericArguments[index];
                        throw BadTypeSig(sig);
                    }

                case ElementType.MethodVar:
                    {
                        int index = sig.GenericParamNumber;
                        if (_currentMethodSpec != null)
                            contextMethod = _currentMethodSpec;
                        if (contextMethod == null)
                            throw new BadMetadataException("Invalid method context");
                        if (contextMethod.IsGenericInstance)
                            return contextMethod.GenericArguments[index];
                        return contextMethod.GenericParameters[index];
                    }

                case ElementType.RequiredModifier:
                case ElementType.OptionalModifier:
                    {
                        return ResolveTypeSignature(sig.Type, contextType, contextMethod);
                    }

                case ElementType.Sentinel:
                case ElementType.Pinned:
                    {
                        return ResolveTypeSignature(sig.Type, contextType, contextMethod);
                    }

                case ElementType.CustomArgsType:
                    return SystemTypes.Type;

                case ElementType.CustomArgsBoxedObject:
                    return SystemTypes.Object;

                case ElementType.CustomArgsField:
                    break;

                case ElementType.CustomArgsProperty:
                    break;
            }
            return null;
        }
        #endregion
        #endregion

	    #region IMethodContext Members
        private IType CurrentType
        {
            get
            {
                if (_currentType != null)
                    return _currentType;
                if (_currentMethod != null)
                    return _currentMethod.DeclaringType;
                return null;
            }
        }
		private IType _currentType;

        public IMethod CurrentMethod
        {
            get { return _currentMethod; }
        }
        private IMethod _currentMethod;

        public IVariableCollection ResolveLocalVariables(int sig, out bool hasGenericVars)
        {
            hasGenericVars = false;
            if (_currentMethod == null) 
                return null;

            var list = new VariableCollection();
            if (sig == 0) return list;

            MdbIndex idx = sig;
            var row = _mdb.GetRow(MdbTableId.StandAloneSig, idx.Index - 1);
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
                    var type = ResolveTypeSignature(typeSig, _currentMethod.DeclaringType, _currentMethod);

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

        public IType ResolveType(int sig)
        {
            if (_currentMethod == null) return null;
            MdbIndex i = sig;
            var type = GetTypeDefOrRef(i, _currentMethod.DeclaringType, _currentMethod);
            return type;
        }

        public object ResolveMetadataToken(IMethod method, int token)
        {
            _currentMethod = method;
            return ResolveMetadataToken(token);
        }

        public object ResolveMetadataToken(int token)
        {
            if (_currentMethod == null) return null;

            uint msb = (uint)token >> 24;
            int index = token & 0xFFFFFF;
            if (msb == 0x70)
                return _mdb.GetUserString((uint)index);

            var tableId = (MdbTableId)msb;
            switch (tableId)
            {
                case MdbTableId.TypeRef:
                case MdbTableId.TypeDef:
                case MdbTableId.TypeSpec:
                    return GetTypeDefOrRef(token, _currentMethod.DeclaringType, _currentMethod);

                case MdbTableId.Field:
                    return _fields[index - 1];

                case MdbTableId.MethodDef:
                    return _methodDef[index - 1];

                case MdbTableId.MemberRef:
                    return GetMemberRef(index - 1);

                case MdbTableId.MethodSpec:
                    return GetMethodSpec(index - 1);

                case MdbTableId.StandAloneSig:
                    return null;
            }
            return null;
        }
        #endregion

        #region DebugInfo
        bool _initDebugInfo = true;
        PdbReader _pdbReader;

        bool IsFrameworkLib
        {
            get 
            {
                if (_assembly.Location.ComparePath(GlobalSettings.GetCorlibPath(false)) == 0)
                    return true;
                return AssemblyResolver.IsFrameworkAssembly(_assembly);
            }
        }

        PdbReader CreatePdbReader()
        {
            if (!GlobalSettings.EmitDebugInfo) return null;

            //NOTE: Since we provide .NET framework libs in binary form it is not needed to emit debug info for them.
            if (IsFrameworkLib) return null;

            string path = _assembly.Location;
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
        public static List<AssemblyLoader> _loaders = new List<AssemblyLoader>();
	    
	    public static void Clean()
	    {
		    GenericParamTable.ResetId();
            while (_loaders.Count > 0)
            {
                var al = _loaders[0];
                _loaders.RemoveAt(0);
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

		private static Version GetVersion(MdbRow row, int i)
		{
			return new Version((int)row[i].Value,
							   (int)row[i + 1].Value,
							   (int)row[i + 2].Value,
							   (int)row[i + 3].Value);
		}
    }
}