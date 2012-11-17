using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    internal sealed class AssemblyLoader : IMethodContext, IDisposable, IAssemblyReferencesResolver
    {
	    private TypeSpecTable _typeSpec;
		private IType[] _interfaceImpl;
	    private MemberRefTable _memberRef;
		private MethodSpecTable _methodSpec;
		private SignatureResolver _signatureResolver;

	    internal IAssembly Assembly { get; private set; }
	    internal IModule MainModule { get { return Assembly.MainModule; } }
	    internal MdbReader Mdb { get; private set; }

	    internal AssemblyRefTable AssemblyRefs { get; private set; }
		internal ModuleRefTable ModuleRefs { get; private set; }
		internal ModuleTable Modules { get; private set; }
		internal FileTable Files { get; private set; }
		internal ManifestResourceTable ManifestResources { get; private set; }
	    internal ConstantTable Const { get; private set; }
	    internal ClassLayoutTable ClassLayout { get; private set; }
	    internal ParamTable Parameters { get; private set; }
	    internal GenericParamTable GenericParameters { get; private set; }
	    internal FieldTable Fields { get; private set; }
	    internal MethodTable Methods { get; private set; }
	    internal PropertyTable Properties { get; private set; }
	    internal EventTable Events { get; private set; }
	    internal TypeTable Types { get; private set; }
	    internal TypeRefTable TypeRefs { get; private set; }

		public static bool SortMembers;

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

	    private IAssembly LoadFromFile(string path)
        {
            Mdb = new MdbReader(path);
            //_mdb.Dump(@"c:\mdb.xml");
            Assembly = new AssemblyImpl {Location = path};
            LoadCore();
            return Assembly;
        }

		private IAssembly LoadFromStream(Stream s)
        {
            Mdb = new MdbReader(s);
            Assembly = new AssemblyImpl();
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

        private void LoadTables()
        {
            //To avoid circular references assembly is added to cache
            AssemblyResolver.AddToCache(Assembly);

            _loaders.Add(this);

			_signatureResolver = new SignatureResolver(this);

			// THE ORDER IS IMPORTANT!!!
			Const = new ConstantTable(Mdb);
			Files = new FileTable(this);
	        ManifestResources = new ManifestResourceTable(this);

			// load modules
	        Modules = new ModuleTable(this);
			foreach (var mod in Modules)
	        {
		        Assembly.Modules.Add(mod);
	        }

			ModuleRefs = new ModuleRefTable(this);
			AssemblyRefs = new AssemblyRefTable(this);

			TypeRefs = new TypeRefTable(this);
			_memberRef = new MemberRefTable(this);
			_typeSpec = new TypeSpecTable(this);
			_methodSpec = new MethodSpecTable(this);

	        LoadCorlib();

	        Parameters = new ParamTable(this);
	        Fields = new FieldTable(this);
			Properties = new PropertyTable(this);
			Events = new EventTable(this);
			ClassLayout = new ClassLayoutTable();
	        GenericParameters = new GenericParamTable(this);

			Methods = new MethodTable(this);
			Types = new TypeTable(this);

			//TODO: remove loading, do lazy loading
	        Methods.Load();
	        
			//TODO: remove loading, do lazy loading
	        Types.Load();
			
            ResolveFieldSignatures();
            ResolveMethodSignatures();

            LoadMethodSemanticsTable();
            LoadMethodImplTable();
            LoadInterfaceImplTable();

            LoadCustomAttribute();
            LoadFieldRVA();

            if (SortMembers)
                Sort();
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

	        return false;
        }

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
            int n = Mdb.GetRowCount(MdbTableId.FieldRVA);
            for (int i = 0; i < n; ++i)
            {
                var row = Mdb.GetRow(MdbTableId.FieldRVA, i);

                uint rva = row[MDB.FieldRVA.RVA].Value;
                int fieldIndex = row[MDB.FieldRVA.Field].Index - 1;

                var f = Fields[fieldIndex];

                var type = f.Type;
                int size = GetFieldTypeSize(type);
                if (size > 0)
                {
                    var reader = Mdb.SeekRVA(rva);
                    f.Value = reader.ReadBlock(size);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
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

        private void LoadCorlib()
        {
            int n = AssemblyRefs.Count;
            if (n == 0)
            {
                Assembly.IsCorlib = true;
            }
            else
            {
                for (int i = 0; i < n; ++i)
                {
                    var asm = AssemblyRefs[i];
                    if (asm.IsCorlib)
                        break;
                }
            }
        }
        #endregion
        
        #region ResolveFieldSignatures
        void ResolveFieldSignatures()
        {
            int n = Fields.Count;
            for (int i = 0; i < n; ++i)
            {
                var field = Fields[i];
                if (field.DeclaringType == null)
                    throw new BadMetadataException(string.Format("Field {0}[{1}] has no declaring type", field.Name, i));

                var row = Mdb.GetRow(MdbTableId.Field, i);
                var sigBlob = row[MDB.Field.Signature].Blob;
                var sig = MdbSignature.DecodeFieldSignature(sigBlob);

                field.Type = ResolveType(sig.Type, new Context(field.DeclaringType));
            }
        }
        #endregion

        #region ResolveMethodSignatures
        private void ResolveMethodSignatures()
        {
            int n = Methods.Count;
            for (int i = 0; i < n; ++i)
            {
                var method = Methods[i];
                var declType = method.DeclaringType;
                if (declType == null)
                    throw new BadMetadataException(string.Format("Method {0}[{1}] has no declaring type", method.Name, i));

                var row = Mdb.GetRow(MdbTableId.MethodDef, i);
                var sigBlob = row[MDB.MethodDef.Signature].Blob;
                var sig = MdbSignature.DecodeMethodSignature(sigBlob);

                ResolveMethodSignature(method, sig);
            }
        }

        private void ResolveMethodSignature(IMethod method, MdbMethodSignature sig)
        {
	        var context = new Context(method);
            method.Type = ResolveType(sig.Type, context);

            int n = sig.Params.Length;
            for (int i = 0; i < n; ++i)
            {
                var ptype = ResolveType(sig.Params[i], context);
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
        private void LoadMethodImplTable()
        {
            int n = Mdb.GetRowCount(MdbTableId.MethodImpl);
            for (int i = 0; i < n; ++i)
            {
                var row = Mdb[MdbTableId.MethodImpl, i];
                int typeIndex = row[MDB.MethodImpl.Class].Index - 1;

                MdbIndex bodyIdx = row[MDB.MethodImpl.MethodBody].Value;
                MdbIndex declIdx = row[MDB.MethodImpl.MethodDeclaration].Value;

                var type = Types[typeIndex];
                var body = GetMethodDefOrRef(bodyIdx, new Context(type));

                var decl = GetMethodDefOrRef(declIdx, new Context(body));

                body.ImplementedMethods = new[] { decl };
                body.IsExplicitImplementation = true;
            }
        }
        #endregion

        #region LoadInterfaceImplTable
        private void LoadInterfaceImplTable()
        {
            int n = Mdb.GetRowCount(MdbTableId.InterfaceImpl);
            _interfaceImpl = new IType[n];
            var ifaces = new IType[n];
            for (int i = 0; i < n; ++i)
            {
                var row = Mdb.GetRow(MdbTableId.InterfaceImpl, i);
                int typeIndex = row[MDB.InterfaceImpl.Class].Index - 1;
                var type = Types[typeIndex];
                MdbIndex ifaceIndex = row[MDB.InterfaceImpl.Interface].Value;

                var iface = GetTypeDefOrRef(ifaceIndex, new Context(type));

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

        private static bool HasExplicitImplementation(IType type, IMethod ifaceMethod)
        {
        	return (from method in type.Methods
					where method.IsExplicitImplementation
					select method.ImplementedMethods).Any(impl => impl != null && impl.Length == 1 && impl[0] == ifaceMethod);
        }

	    private static void AddImplementedMethod(IType type, IMethod ifaceMethod)
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

	    private static IMethod FindImpl(IType type, IMethod ifaceMethod)
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

	    #endregion

        #region LoadMethodSemanticsTable
        void LoadMethodSemanticsTable()
        {
            int n = Mdb.GetRowCount(MdbTableId.MethodSemantics);
            for (int i = 0; i < n; ++i)
            {
                var row = Mdb.GetRow(MdbTableId.MethodSemantics, i);

                int methodIndex = row[MDB.MethodSemantics.Method].Index - 1;
                var method = Methods[methodIndex];

                var sem = (MethodSemanticsAttributes)row[MDB.MethodSemantics.Semantics].Value;

                MdbIndex assoc = row[MDB.MethodSemantics.Association].Value;
                int assocRowIndex = assoc.Index - 1;
                switch (assoc.Table)
                {
                    case MdbTableId.Property:
                        {
                            var property = Properties[assocRowIndex];

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
                            var e = Events[assocRowIndex];
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
            int n = Mdb.GetRowCount(MdbTableId.CustomAttribute);
            for (int i = 0; i < n; ++i)
            {
	            var row = Mdb.GetRow(MdbTableId.CustomAttribute, i);
	            MdbIndex parent = row[MDB.CustomAttribute.Parent].Value;
	            var provider = GetCustomAttributeProvider(parent, null);
	            if (provider == null)
	            {
		            //TODO: warning
		            continue;
	            }

	            MdbIndex ctorIndex = row[MDB.CustomAttribute.Type].Value;
	            var ctor = GetCustomAttributeConstructor(ctorIndex, ResolveAttributeContext(provider));
	            if (ctor == null)
	            {
		            //TODO: warning
		            continue;
	            }

	            var value = row[MDB.CustomAttribute.Value].Blob;
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
        }

		private static Context ResolveAttributeContext(ICustomAttributeProvider provider)
		{
			var type = provider as IType;
			if (type != null)
			{
				return new Context(type);
			}

			var method = provider as IMethod;
			if (method != null)
			{
				return new Context(method);
			}

			var member = provider as ITypeMember;
			if (member != null)
			{
				return new Context(member.DeclaringType);
			}

			return null;
		}

        private IType FindType(string name)
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

            var type = Assembly.FindType(name);
            if (type != null)
                return type;

            int n = Mdb.GetRowCount(MdbTableId.AssemblyRef);
            for (int i = 0; i < n; ++i)
            {
                var asm = AssemblyRefs[i];
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

        private object ReadValue(BufferedBinaryReader reader, IType type)
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
                return ReadValue(reader, type.ValueType);
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

        private void ReadArguments(ICustomAttribute attr, byte[] blob)
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

        private IType ReadEnumType(BufferedBinaryReader reader)
        {
            string enumTypeName = reader.ReadCountedUtf8();
            return FindType(enumTypeName);
        }

        private static void ResolveNamedArgType(IArgument arg, IType declType)
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

        private IMethod GetCustomAttributeConstructor(MdbIndex i, Context context)
        {
            try
            {
                switch (i.Table)
                {
                    case MdbTableId.MethodDef:
                        return Methods[i.Index - 1];

                    case MdbTableId.MemberRef:
						return GetMemberRef(i.Index - 1, context) as IMethod;

                    default:
                        throw new BadMetadataException(string.Format("Invalid custom attribute type index {0}", i));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ICustomAttributeProvider GetCustomAttributeProvider(MdbIndex i, Context context)
        {
            int index = i.Index - 1;
            switch (i.Table)
            {
                case MdbTableId.MethodDef:
                    return Methods[index];

                case MdbTableId.Field:
                    return Fields[index];

                case MdbTableId.TypeRef:
                    return TypeRefs[index];

                case MdbTableId.TypeDef:
                    return Types[index];

                case MdbTableId.Param:
                    return Parameters[index];

                case MdbTableId.Property:
                    return Properties[index];

                case MdbTableId.Event:
                    return Events[index];

                case MdbTableId.Module:
                    return Modules[index];

                case MdbTableId.ModuleRef:
                    return ModuleRefs[index];

                case MdbTableId.TypeSpec:
                    return GetTypeSpec(index, context);

                case MdbTableId.AssemblyRef:
                    return AssemblyRefs[index];

                case MdbTableId.Assembly:
                    return Assembly;

                case MdbTableId.InterfaceImpl:
                    return _interfaceImpl[index];

                case MdbTableId.MemberRef:
                    return GetMemberRef(index, context);

                case MdbTableId.File:
                    return Files[index];

                case MdbTableId.DeclSecurity:
                    return null;

                case MdbTableId.StandAloneSig:
                    return null;

                case MdbTableId.ExportedType:
                    return null;

                case MdbTableId.ManifestResource:
                    return ManifestResources[index];

                case MdbTableId.GenericParam:
                    return GenericParameters[index];

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ReviewAttribute(ICustomAttribute attr)
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
            var reader = Mdb.SeekRVA(rva);
            var body = new MethodBody(method, this, reader);
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
            Assembly.Modules.Sort();
        }
        #endregion

	    private ITypeMember GetMemberRef(int index, Context context)
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
                    if (index < 0) return SystemTypes.Object;
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
        bool _initDebugInfo = true;
        PdbReader _pdbReader;

        bool IsFrameworkLib
        {
            get 
            {
                if (Assembly.Location.ComparePath(GlobalSettings.GetCorlibPath(false)) == 0)
                    return true;
                return AssemblyResolver.IsFrameworkAssembly(Assembly);
            }
        }

        PdbReader CreatePdbReader()
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
    }
}