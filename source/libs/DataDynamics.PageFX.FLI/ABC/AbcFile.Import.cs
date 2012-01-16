using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.ABC
{
    using AbcString = AbcConst<string>;

    //ABC Import API
    public partial class AbcFile
    {
        #region Import ABC file
        bool Importing;
        bool Imported;

        /// <summary>
        /// Imports contents of given ABC file
        /// </summary>
        /// <param name="from"></param>
        public void Import(AbcFile from)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (from == this)
                throw new ArgumentException("from == this");
            if (from.IsCoreAPI)
                return;

            if (from.UseExternalLinking)
            {
                if (from.IsLinkedExternally)
                    return;

                if (AllowExternalLinking)
                {
                    from.IsLinkedExternally = true;
                    ImportAssets(from);
                    return;
                }
            }

            if (from.Imported) return;

            //ImportDeps(from, DepKind.Pre);
            ImportCore(from);
            //ImportDeps(from, DepKind.Post);
        }

        void ImportCore(AbcFile from)
        {
            from.Imported = true;
            from.Importing = true;

            ImportConstPools(from);

            foreach (var script in from.Scripts)
                ImportScript(script);

            //NOTE: Classes are imported with instances
            //NOTE: Methods are imported with instances, classes or scripts
            
            from.Importing = false;

            ImportTypes(from);
        }

        void ImportDeps(AbcFile from, DepKind kind)
        {
            var deps = from.Deps;
            if (deps == null) return;
            if (deps.Count <= 0) return;
            var ctx = new ImportContext
                          {
                              CurrentABC = from,
                              TargetABC = this
                          };
            deps.Import(ctx, kind);
        }

        void ImportTypes(AbcFile from)
        {
            if (from.HasDeps) return;

            switch (from.ImportStrategy)
            {
                    case ImportStrategy.Multinames:
                        ImportTypesUsingMultinames(from);
                        break;

                    case ImportStrategy.Refs:
                        ImportRefs(from);
                        break;
            }
        }

        void ImportRefs(AbcFile from)
        {
            foreach (var f in from.FileRefs)
                Import(f);
            foreach (var instance in from.InstanceRefs)
                ImportInstance(instance);
        }

        void ImportTypesUsingMultinames(AbcFile from)
        {
            int n = from.Multinames.Count;
            for (int i = 1; i < n; ++i)
            {
                var mn = from.Multinames[i];
                if (ExcludeTypeName(mn)) continue;

                AbcInstance type;
                ImportType(mn, out type);
            }
        }

        static bool ExcludeTypeName(AbcMultiname name)
        {
            if (name.IsRuntime) return true;
            return false;
        }
        #endregion

        #region GetConstPool
        /// <summary>
        /// Returns constant pool for given kind of constants.
        /// </summary>
        /// <param name="k">kind of constants.</param>
        /// <returns></returns>
        public IAbcConstPool GetConstPool(AbcConstKind k)
        {
            switch (k)
            {
                case AbcConstKind.String:
                    return _stringPool;
                case AbcConstKind.Int:
                    return _intPool;
                case AbcConstKind.UInt:
                    return _uintPool;
                case AbcConstKind.Double:
                    return _doublePool;

                case AbcConstKind.QName:
                case AbcConstKind.Multiname:
                case AbcConstKind.QNameA:
                case AbcConstKind.MultinameA:
                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                case AbcConstKind.RTQNameL:
                case AbcConstKind.RTQNameLA:
                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                case AbcConstKind.TypeName:
                    return _multinames;

                case AbcConstKind.PrivateNamespace:
                case AbcConstKind.PublicNamespace:
                case AbcConstKind.PackageNamespace:
                case AbcConstKind.InternalNamespace:
                case AbcConstKind.ProtectedNamespace:
                case AbcConstKind.ExplicitNamespace:
                case AbcConstKind.StaticProtectedNamespace:
                    return _nspool;

                case AbcConstKind.NamespaceSet:
                    return _nssets;

                default:
                    return null;
            }
        }
        #endregion

        #region ImportConst
        /// <summary>
        /// Determines whether given constant is defined in current ABC file.
        /// </summary>
        /// <param name="c">constant to test</param>
        /// <returns>true if the constant is defined, otherwise - false</returns>
        public bool IsDefined(IAbcConst c)
        {
            if (c == null) return false;
            var pool = GetConstPool(c.Kind);
            if (pool != null)
                return pool.IsDefined(c);
            return false;
        }

        /// <summary>
        /// Imports given constant.
        /// </summary>
        /// <typeparam name="T">type of constant. Can be int,uint,double,string</typeparam>
        /// <param name="c">constant to import.</param>
        /// <returns>imported constant.</returns>
        public AbcConst<T> ImportConst<T>(AbcConst<T> c)
        {
            if (c == null) return null;
            var pool = GetConstPool(c.Kind);
            if (pool != null)
                return ((AbcConstPool<T>)pool).Import(c);
            return c;
        }

        /// <summary>
        /// Imports <see cref="AbcNamespace"/> constant.
        /// </summary>
        /// <param name="ns">namespace to import.</param>
        /// <returns></returns>
        public AbcNamespace ImportConst(AbcNamespace ns)
        {
            return _nspool.Import(ns);
        }

        /// <summary>
        /// Imports <see cref="AbcNamespaceSet"/> constant.
        /// </summary>
        /// <param name="set">constant to import.</param>
        /// <returns></returns>
        public AbcNamespaceSet ImportConst(AbcNamespaceSet set)
        {
            return _nssets.Import(set);
        }

        public AbcMultiname ImportConst(AbcMultiname name)
        {
            return _multinames.Import(name);
        }

        public IAbcConst ImportConst(IAbcConst c)
        {
            var pool = GetConstPool(c.Kind);
            if (pool != null)
                return pool.Import(c);
            return null;
        }

        public void ImportConstPool(IAbcConstPool pool)
        {
            int n = pool.Count;
            for (int i = 1; i < n; ++i)
                ImportConst(pool[i]);
        }

        void ImportConstPools(AbcFile from)
        {
            ImportConstPool(from.IntPool);
            ImportConstPool(from.UIntPool);
            ImportConstPool(from.DoublePool);
            ImportConstPool(from.StringPool);
        }
        #endregion

        #region ImportValue
        public object ImportValue(object value)
        {
            if (value == null) return null;
            if (IsUndefined(value)) return Undefined;

            var c = value as IAbcConst;
            if (c != null) return ImportConst(c);

            var tc = Type.GetTypeCode(value.GetType());
            switch (tc)
            {
                case TypeCode.Object:
                    {
                        var instance = value as AbcInstance;
                        if (instance != null)
                            return ImportInstance(instance);

                        var klass = value as AbcClass;
                        if (klass != null)
                            return ImportInstance(klass.Instance);

                        var method = value as AbcMethod;
                        if (method != null)
                            return ImportMethod(method);

                        throw new NotImplementedException();
                    }

                case TypeCode.DBNull:
                    throw new NotSupportedException();

                case TypeCode.Boolean:
                    return value;

                case TypeCode.SByte:
                    return _intPool.Define((sbyte)value);

                case TypeCode.Int16:
                    return _intPool.Define((short)value);

                case TypeCode.Int32:
                    return _intPool.Define((int)value);

                case TypeCode.Byte:
                    return _uintPool.Define((byte)value);

                case TypeCode.Char:
                    return _uintPool.Define((char)value);

                case TypeCode.UInt16:
                    return _uintPool.Define((ushort)value);

                case TypeCode.UInt32:
                    return _uintPool.Define((uint)value);

                case TypeCode.Int64:
                    throw new NotImplementedException();

                case TypeCode.UInt64:
                    throw new NotImplementedException();

                case TypeCode.Single:
                    return DefineSingle((float)value);

                case TypeCode.Double:
                    return _doublePool.Define((double)value);

                case TypeCode.String:
                    return DefineString((string)value);

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region ImportMethod
        AbcMethod ImportMethod(AbcMethod method)
        {
            if (IsDefined(method))
                return method;

            if (method.ImportedMethod != null)
                return method.ImportedMethod;

            var m = new AbcMethod
                        {
                            Name = ImportConst(method.Name),
                            Flags = method.Flags,
                            IsInitializer = method.IsInitializer,
                            OriginalMethod = method,                            
                            SourceMethod = method.SourceMethod,
                            ReturnType = ImportType(method.ReturnType)
                        };

            method.ImportedMethod = m;
            method.IsImported = true;

            if (m.SourceMethod != null)
                m.SourceMethod.Tag = m;

            ImportParams(method, m);

            _methods.Add(m);

            var body = method.Body;
            if (body != null)
            {
                m.Body = ImportMethodBody(body, m);
            }

            return m;
        }

        void ImportParams(AbcMethod from, AbcMethod to)
        {
            foreach (var p in from.Parameters)
            {
                var p2 = new AbcParameter
                             {
                                 IsOptional = p.IsOptional,
                                 Name = ImportConst(p.Name),
                                 Type = ImportType(p.Type),
                                 Value = ImportValue(p.Value)
                             };
                to.Parameters.Add(p2);
            }
        }
        #endregion

        #region ImportMethodBody
        AbcMethodBody ImportMethodBody(AbcMethodBody from, AbcMethod method)
        {
            if (from.ImportedBody != null)
                return from.ImportedBody;

            var body = new AbcMethodBody(method);
            from.ImportedBody = body;
            body.MaxStackDepth = from.MaxStackDepth;
            body.LocalCount = from.LocalCount;
            body.MinScopeDepth = from.MinScopeDepth;
            body.MaxScopeDepth = from.MaxScopeDepth;
            _methodBodies.Add(body);

            ImportTraits(from, body);
            ImportExceptions(body, from);
            ImportIL(body, from);
            body.TranslateIndices();
            body.ResolveExceptionOffsets(this);

            return body;
        }

        static int GetOffsetIndex(IInstructionList code, int offset)
        {
            int i = code.GetOffsetIndex(offset);
            if (i < 0)
                throw Errors.ABC.InvalidBranchOffset.CreateException();
            return i;
        }

        void ImportExceptions(AbcMethodBody body, AbcMethodBody from)
        {
            var code = from.IL;
            foreach (var h in from.Exceptions)
            {
                var h2 = new AbcExceptionHandler
                             {
                                 From = GetOffsetIndex(code, h.From),
                                 To = GetOffsetIndex(code, h.To),
                                 Target = GetOffsetIndex(code, h.Target),
                                 Index = h.Index,
                                 Variable = ImportConst(h.Variable),
                                 Type = ImportType(h.Type)
                             };
                body.Exceptions.Add(h2);
            }
        }

        void ImportIL(AbcMethodBody body, AbcMethodBody from)
        {
            var code = from.IL;
            //NOTE: Size of instructions can be changed and therefore we should retranslate branch offsets.
            code.TranslateOffsets();
            int n = code.Count;
            bool hasBranches = false;
            for (int i = 0; i < n; ++i)
            {
                var instr = ImportInstruction(body, code[i]);
                body.IL.Add(instr);
                if (!hasBranches && (instr.IsBranch || instr.IsSwitch))
                    hasBranches = true;
            }
            //if (from.Exceptions.Count == 0 && !hasBranches)
            //    body.IL.Optimize();
        }

        Instruction ImportInstruction(AbcMethodBody body, Instruction i)
        {
            var i2 = i.Clone();
            if (!i2.HasOperands) return i2;
            foreach (var op in i2.Operands)
            {
                if (op.Value == null)
                    continue;

                switch (op.Type)
                {
                    case OperandType.MethodIndex:
                        {
                            var m = (AbcMethod)op.Value;
                            op.Value = ImportMethod(m);
                        }
                        break;

                    case OperandType.ClassIndex:
                        {
                            var klass = (AbcClass)op.Value;
                            var instance = ImportInstance(klass.Instance);
                            op.Value = instance.Class;
                        }
                        break;

                    case OperandType.ExceptionIndex:
                        {
                            var h = (AbcExceptionHandler)op.Value;
                            op.Value = body.Exceptions[h.Index];
                        }
                        break;

                    default:
                        {
                            var c = op.Value as IAbcConst;
                            if (c != null)
                            {
                                var nc = ImportConst(c);
                                if (nc == null)
                                    throw new InvalidOperationException(string.Format(
                                                                            "Unable to import const: {0}", c));
                                op.Value = nc;
                            }
                        }
                        break;
                }
            }
            return i2;
        }
        #endregion

        #region ImportInstance
        /// <summary>
        /// Imports given instance
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public AbcInstance ImportInstance(AbcInstance instance)
        {
            AbcMethod importMethod = null;
            return ImportInstance(instance, ref importMethod);
        }

        internal static bool AllowExternalLinking = true;

        public AbcInstance ImportInstance(AbcInstance instance, ref AbcMethod importMethod)
        {
            if (instance == null) return null;

            if (instance.IsNative)
                return instance;

            if (instance.UseExternalLinking)
            {
                if (instance.IsLinkedExternally)
                    return instance;

                if (AllowExternalLinking)
                {
                    instance.IsLinkedExternally = true;
                    if (instance.InSwc)
                    {
                        var abc = instance.ABC;
                        if (!abc.Importing)
                        {
                            Debug.Assert(abc != this);
                            Import(abc);
                        }
                    }
                    else
                    {
                        ImportAssets(instance);
                    }
                    return instance;
                }
            }

            if (IsDefined(instance))
                return instance;

            if (instance.ImportedInstance != null)
                return instance.ImportedInstance;

            if (instance.InSwc)
            {
                var abc = instance.ABC;
                if (!abc.Importing)
                {
                    Debug.Assert(abc != this);
                    Import(abc);
                }
            }

            if (instance.ImportedInstance != null)
                return instance.ImportedInstance;

            if (!IsImportTypeExternally)
            {
                var superType = instance.SuperType;
                if (superType != null)
                    ImportInstance(superType);
            }

            if (instance.ImportedInstance != null)
                return instance.ImportedInstance;

            ImportAbcFiles(instance);

            if (instance.ImportedInstance != null)
                return instance.ImportedInstance;

            var result = ImportInstanceCore(instance, ref importMethod);

            //ImportAbcFiles(instance);

            return result;
        }

        void ImportAbcFiles(AbcInstance instance)
        {
            if (instance.ImportAbcFiles.Count > 0)
            {
                foreach (var abc in instance.ImportAbcFiles)
                    Import(abc);
            }
        }

        internal bool IsSwcScript;

        AbcInstance ImportInstanceCore(AbcInstance from, ref AbcMethod importMethod)
        {
            var instance = new AbcInstance
                               {
                                   Name = ImportConst(from.Name),
                                   IsMixin = from.IsMixin,
                                   IsStyleMixin = from.IsStyleMixin,
                                   ImportedFrom = from
                               };
            from.ImportedInstance = instance;

            var klass = new AbcClass(instance);
            AddInstance(instance);
            
            AbcInstance superType;
            instance.SuperName = ImportType(from.SuperName, out superType);
            instance.SuperType = superType;
            instance.Flags = from.Flags;
            instance.ProtectedNamespace = ImportConst(from.ProtectedNamespace);
            instance.Type = from.Type;
            if (instance.Type != null)
                instance.Type.Tag = instance;

            foreach (var iname in from.Interfaces)
            {
                AbcInstance ifaceInstance;
                var mn = ImportType(iname, out ifaceInstance);
                if (mn == null)
                    throw new InvalidOperationException();
                //NOTE: Flex Compiler Bug!!!
                //I found that within SWC files interface names must be always declared as multinames (with namespace set)
                //This is true for flex 3.
                if (IsSwcScript)
                    mn = ToMultiname(mn);
                instance.Interfaces.Add(mn);
                if (ifaceInstance != null)
                {
                    ifaceInstance.Implementations.Add(instance);
                    instance.Implements.Add(ifaceInstance);
                }
            }

            instance.Initializer = ImportMethod(from.Initializer);
            klass.Initializer = ImportMethod(from.Class.Initializer);

            if (importMethod == from.Initializer)
                importMethod = instance.Initializer;

            if (importMethod == from.Class.Initializer)
                importMethod = klass.Initializer;

            ImportTraits(from, instance, ref importMethod);
            ImportTraits(from.Class, klass, ref importMethod);

            return instance;
        }

        AbcMultiname ToMultiname(AbcMultiname name)
        {
            if (name.IsMultiname) return name;
            if (!name.IsQName)
                throw new ArgumentException(string.Format("name {0} is not qname", name));
            var ns = name.Namespace;
            var nss = DefineNamespaceSet(ns);
            var kind = name.Kind == AbcConstKind.QName ? AbcConstKind.Multiname : AbcConstKind.MultinameA;
            return DefineMultiname(kind, nss, name.Name);
        }
        #endregion

        #region ImportTraits
        void ImportTraits(IAbcTraitProvider from, IAbcTraitProvider to, ref AbcMethod importMethod)
        {
            foreach (var trait in from.Traits)
            {
                var t2 = ImportTrait(trait);
                if (trait.Method == importMethod)
                    importMethod = t2.Method;
                to.Traits.Add(t2);
            }
        }

        void ImportTraits(IAbcTraitProvider from, IAbcTraitProvider to)
        {
            foreach (var trait in from.Traits)
            {
                var t2 = ImportTrait(trait);
                to.Traits.Add(t2);
            }
        }
        #endregion

        #region ImportTrait
        AbcTrait ImportTrait(AbcTrait from)
        {
            var trait = new AbcTrait
                            {
                                Kind = from.Kind,
                                Name = ImportConst(from.Name),
                                Attributes = from.Attributes,
                                Property = from.Property
                            };

            switch (from.Kind)
            {
                case AbcTraitKind.Slot:
                case AbcTraitKind.Const:
                    {
                        trait.SlotID = from.SlotID;
                        trait.HasValue = from.HasValue;
                        trait.SlotType = ImportType(from.SlotType);
                        if (from.HasValue)
                            trait.SlotValue = ImportValue(from.SlotValue);
                    }
                    break;

                case AbcTraitKind.Method:
                case AbcTraitKind.Getter:
                case AbcTraitKind.Setter:
                    {
                        trait.SlotID = from.SlotID;
                        trait.Method = ImportMethod(from.Method);
                    }
                    break;

                case AbcTraitKind.Class:
                    {
                        var instance = Instances.Find(from.Name);
                        if (instance == null)
                            instance = ImportInstance(from.Class.Instance);

                        trait.Class = instance.Class;
                        //trait.SlotID = trait.SlotID;
                    }
                    break;

                case AbcTraitKind.Function:
                    {
                        trait.SlotID = from.SlotID;
                        trait.Method = ImportMethod(from.Method);
                    }
                    break;
            }

            ImportEmbedAsset(trait, from);

            if (from.HasMetadata)
            {
                var md = from.Metadata;
                if (md != null)
                {
                    var abc = from.Name.ABC;
                    foreach (var e in md)
                    {
                        if (ProcessMetaEntry(abc, trait, e))
                            continue;

                        var e2 = ImportMetaEntry(abc, e);
                        if (e2 != null)
                        {
                            trait.HasMetadata = true;
                            trait.Metadata.Add(e2);
                        }
                    }
                }
            }

            return trait;
        }
        #endregion

        #region ImportType
        internal AbcInstance FindInstance(AbcMultiname name, bool seeExternal)
        {
            if (name == null) return null;
            if (name.IsRuntime) return null;

            AbcInstance instance;
            if (seeExternal)
            {
                var asm = ApplicationAssembly;
                if (asm != null)
                {
                    instance = AssemblyIndex.FindInstance(asm, name);
                    if (instance != null)
                        return instance;
                }
            }

            instance = _instances[name];
            if (instance != null)
                return instance;

            var pf = PrevFrame;
            if (pf != null)
            {
                Debug.Assert(pf != this);
                instance = pf.FindInstance(name, false);
                if (instance != null)
                    return instance;
            }

            return instance;
        }

        internal AbcInstance ImportInstance(AbcMultiname name)
        {
            var asm = ApplicationAssembly;
            if (asm != null)
            {
                var instance = AssemblyIndex.FindInstance(asm, name);
                if (instance != null)
                    return ImportInstance(instance);
            }
            return null;
        }

        internal AbcInstance ImportInstance(string fullname)
        {
            var asm = ApplicationAssembly;
            if (asm != null)
            {
                var instance = AssemblyIndex.FindInstance(asm, fullname);
                if (instance != null)
                    return ImportInstance(instance);
            }
            return null;
        }

        public ImportTypeStrategy ImportTypeStrategy { get; set; }

        bool IsImportTypeExternally
        {
            get { return ImportTypeStrategy == ImportTypeStrategy.External; }
        }

        AbcMultiname ImportType(AbcMultiname name, out AbcInstance type)
        {
            type = null;
            if (name == null) return null;

            if (IsImportTypeExternally)
                return ImportConst(name);

            if (name.IsRuntime) return ImportConst(name);

            type = ImportInstance(name);
            
            if (type != null)
                name = type.Name;

            return ImportConst(name);
        }

        AbcMultiname ImportType(AbcMultiname name)
        {
            AbcInstance type;
            name = ImportType(name, out type);
            return name;
        }
        #endregion

        #region ProcessMetaEntry
        bool ProcessMetaEntry(AbcFile abc, AbcTrait trait, AbcMetaEntry e)
        {
            if (ImportResourceBundle(abc, e))
                return true;

            string name = e.NameString;
            if (name == MDTags.Mixin)
            {
                var klass = trait.Class;
                if (klass != null)
                {
                    var instance = klass.Instance;
                    instance.IsMixin = true;

                    var sfc = SwfCompiler;
                    if (sfc != null)
                    {
                        sfc.RegisterMixin(instance);
                    }
                }
                return true;
            }

            if (name == MDTags.RemoteClass)
            {
                var sfc = SwfCompiler;
                if (sfc != null)
                {
                    var klass = trait.Class;
                    if (klass != null)
                    {
                        string alias = e["alias"];
                        sfc.RegisterRemoteClass(alias, klass.Instance);
                    }
                }
                return true;
            }

            if (name == MDTags.Effect)
            {
                var sfc = SwfCompiler;
                if (sfc != null)
                {
                    string effectName = e["name"];
                    string effectEvent = e["event"];
                    sfc.RegisterEffectTrigger(effectName, effectEvent);
                }
                return true;
            }

            if (name == MDTags.Style)
            {
                //TODO: Can be also used to collect inheriting styles
                var klass = trait.Class;
                if (klass != null)
                {
                    klass.Instance.HasStyles = true;

                    var sfc = SwfCompiler;
                    if (sfc != null)
                    {
                        sfc.AddStyleClient(klass.Instance);
                    }
                }
            }

            return false;
        }
        #endregion

        #region ImportEmbedAsset
        void ImportEmbedAsset(AbcTrait trait, AbcTrait from)
        {
            var newEmbed = ImportEmbedAsset(from);
            if (newEmbed == null) return;

            var instance = trait.Class.Instance;
            trait.Embed = newEmbed;
            trait.AssetInstance = instance;
            instance.Embed = newEmbed;
        }

        Embed ImportEmbedAsset(AbcTrait from)
        {
            if (from == null) return null;
            var embed = from.Embed;
            if (embed == null) return null;
            if (embed.Asset == null) return null;

            if (!IsSwf)
                throw Errors.NotSwf.CreateException();
            
            var sfc = SwfCompiler;
            var asset = sfc.ImportAsset(embed);

            var newEmbed = new Embed(embed);
            newEmbed.Asset = asset;
            newEmbed.Movie = sfc._swf;
            return newEmbed;
        }
        #endregion

        #region ImportMetaEntry
        static bool FilterMetaEntry(string name)
        {
            switch (name)
            {
                case "__go_to_definition_help":
                case "Embed":
                case "Style":
                case "Exclude":
                case "ExcludeClass":
                case "Event":
                case "Inspectable":
                //case "Bindable":
                case "Deprecated":
                case "DefaultProperty":
                case "ArrayElementType":
                case "PercentProxy":
                case "IconFile":
                case "DefaultTriggerEvent":
                case "DefaultBindingProperty":
                case "Frame":
                case "NonCommittingChangeEvent":
                case "CollapseWhiteSpace":
                case "DataBindingInfo":
                case "AccessibilityClass":
                case "MaxChildren":
                    return true;
            }
            return false;
        }

        internal static bool IgnoreMetadata;
        internal static bool FilterMetadata = true;

        AbcMetaEntry ImportMetaEntry(AbcFile abc, AbcMetaEntry from)
        {
            if (IgnoreMetadata)
                return null;

            if (from == null)
                return null;

            if (_metadata.IsDefined(from))
                return from;

            //NOTE: Ignore some unecessary metadata
            if (FilterMetadata)
            {
                string name = from.Name.Value;
                if (FilterMetaEntry(name))
                    return null;
            }

            var e = new AbcMetaEntry();
            e.Name = ImportConst(from.Name);
            foreach (var item in from.Items)
            {
                var key = ImportConst(item.Key);
                var val = ImportConst(item.Value);
                e.Items.Add(key, val);
            }

            _metadata.Add(e);

            return e;
        }
        #endregion

        #region ImportScript
        public void ImportScript(AbcScript from)
        {
            var script = new AbcScript();
            _scripts.Add(script);
            script.Initializer = ImportMethod(from.Initializer);
            ImportTraits(from, script);
        }
        #endregion

        #region ImportAssets
        void ImportAssets(AbcFile abc)
        {
            ImportAssets(abc.GetTraits(AbcTraitOwner.All));
        }

        void ImportAssets(AbcInstance instance)
        {
            ImportAssets(instance.GetAllTraits());
        }

        void ImportAssets(IEnumerable<AbcTrait> traits)
        {
            foreach (var trait in traits)
                ImportAssets(trait);
        }

        void ImportAssets(AbcTrait trait)
        {
            ImportEmbedAsset(trait);

            if (trait.HasMetadata)
            {
                foreach (var e in trait.Metadata)
                    ProcessMetaEntry(trait.Name.ABC, trait, e);
            }
        }
        #endregion

        #region Utils
        static bool IsSimilar(AbcTrait x, AbcTrait y)
        {
            if (x.Kind != y.Kind)
                return false;
            if (!Equals(x.Name, y.Name))
                return false;

            switch (x.Kind)
            {
                case AbcTraitKind.Class:
                    {
                        var cx = x.Class;
                        var cy = y.Class;
                        if (Equals(cx.Instance.Name, cy.Instance.Name))
                            return true;
                    }
                    break;

                case AbcTraitKind.Slot:
                case AbcTraitKind.Const:
                    {
                        //if (x.SlotID != y.SlotID)
                        //    return false;
                        if (!Equals(x.SlotType, y.SlotType))
                            return false;
                        return true;
                    }
            }

            return false;
        }

        static bool IsSimilar(AbcScript x, AbcScript y)
        {
            int n = x.Traits.Count;
            if (n != y.Traits.Count) return false;
            for (int i = 0; i < n; ++i)
            {
                var tx = x.Traits[i];
                var ty = y.Traits[i];
                if (!IsSimilar(tx, ty))
                    return false;
            }
            return true;
        }

        bool HasSimilarScript(AbcScript script)
        {
            foreach (var s in _scripts)
            {
                if (IsSimilar(s, script))
                    return true;
            }
            return false;
        }

        bool HasSimilarScripts(AbcFile abc)
        {
            foreach (var script in abc.Scripts)
            {
                if (!HasSimilarScript(script))
                    return false;
            }
            return true;
        }

        IEnumerable<AbcFile> GetAllFrames()
        {
            yield return this;
            foreach (var f in PrevFrames)
            {
                Debug.Assert(f != null);
                yield return f;
            }
        }
        #endregion
    }

    /// <summary>
    /// Specifies how to import type references.
    /// </summary>
    public enum ImportTypeStrategy
    {
        /// <summary>
        /// Type references are resolved and <see cref="AbcInstance"/>s imported into ABC file.
        /// </summary>
        Internal,

        /// <summary>
        /// Type references are imported externally
        /// </summary>
        External,
    }
}