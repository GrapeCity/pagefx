using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI
{
    class CodeCachingService : ICodeCachingService
    {
        #region Run
        public static void Run()
        {
            Run(GlobalSettings.GetCorlibPath(true));

            foreach (var name in GlobalSettings.CommonAssemblies)
            {
                string path = GlobalSettings.GetLibPath(name);
                Run(path);
            }
        }

        public static void Run(string path)
        {
            var cli = LanguageInfrastructure.CLI;
            if (cli == null)
                throw new InvalidOperationException("CLI does not exist");

            Console.WriteLine("--- Code Caching for '{0}'", path);
            int start = Environment.TickCount;

            cli.Init();
            var asm = cli.Deserialize(path, null);
            var g = new AbcGenerator {Mode = AbcGenMode.Full, IsCcsRunning = true};
            var abc = g.Generate(asm);

            var total = new List<AbcMethod>(
                abc.Methods.Where(m =>
                                  	{
                                  		var sm = m.SourceMethod;
                                  		if (sm == null) return false;
                                  		return sm.Assembly == asm;
                                  	}));

            var list = new List<AbcMethod>(
                total.Where(m =>
                            	{
                            		var sm = m.SourceMethod;
                            		if (sm == null) return false;
                            		if (sm.IsAbstract) return false;
                            		if (sm.IsInternalCall) return false;
                            		if (sm.CodeType != MethodCodeType.IL) return false;
                            		if (HasGenericContext(m)) return false;
                            		return true;
                            	}));

            Comparison<AbcMethod> c = (x, y) => GetIdx(x.SourceMethod) - GetIdx(y.SourceMethod);
            list.Sort(c);

            Console.WriteLine("ABC Stat:");
            Console.WriteLine("Total: {0}", abc.Methods.Count);

            var cgm = new List<AbcMethod>(abc.Methods.Where(m => m.SourceMethod == null));
            Console.WriteLine("Compiler Generated: {0}", cgm.Count);
            int n = cgm.Count(m => m.Trait != null);
            Console.WriteLine("  With Trait: {0}", n);
            Console.WriteLine("  Without Trait: {0}", cgm.Count - n);

            Console.WriteLine("{0} Methods Stat:", asm.Name);
            Console.WriteLine("Total Count: {0}", total.Count);
            Console.WriteLine("Abstract Count: {0}", total.Count(m => m.SourceMethod.IsAbstract));
            Console.WriteLine("InternalCall Count: {0}", total.Count(m => m.SourceMethod.IsInternalCall));
            Console.WriteLine("Runtime Count: {0}", total.Count(m => m.SourceMethod.CodeType == MethodCodeType.Runtime));
            Console.WriteLine("GenericContext Count: {0}", total.Count(HasGenericContext));
            Console.WriteLine("Cached Count: {0}", list.Count);

            var lib = new Lib
                          {
                              MVID = asm.MainModule.Version,
                              Methods = list,
                          };

            lib.Save(Path.ChangeExtension(path, ext));

            Console.WriteLine("Time: {0}", Environment.TickCount - start);
        }

        static bool HasGenericContext(AbcMethod m)
        {
            var sm = m.SourceMethod;
            if (sm.IsGeneric) return true;
            if (sm.IsGenericInstance) return true;
            var dt = sm.DeclaringType;
            if (dt is IGenericInstance) return true;
            if (dt is IGenericType) return true;
            return false;
        }
        #endregion

    	private readonly AbcGenerator _generator;

        public CodeCachingService(AbcGenerator generator)
        {
            _generator = generator;
        }

    	#region Import
        public AbcMethodBody Import(AbcMethod method, BodyReferences refs)
        {
            var lib = GetLib(method.SourceMethod);
            if (lib == null) return null;
            return lib.Import(method, _generator._abc, refs);
        }
        #endregion

        #region class Lib
        class Lib
        {
            public string Path;
            public Guid MVID;
            public List<AbcMethod> Methods;
            
            byte[] _libdata;
            SwfReader _reader;
            Body[] _methods;

            #region Body
            class Body
            {
                public int Index;
                public int Offset;

                bool _read;

                int _maxStackDepth;
                int _localCount;
                int _minScopeDepth;
                int _maxScopeDepth;
                AbcMethodFlags _methodFlags;
                AbcBodyFlags _flags;

                I[] _instructions;
                Handler[] _handlers;
                Slot[] _slots;
                int[] _tokens;
                int[] _fieldPtrs;

                #region Read
                //instructions
                //exceptions
                //slots
                //tokens
                //field ptrs

                void Read(SwfReader reader)
                {
                    if (_read) return;
                    _read = true;
                    reader.Position = Offset;

                    _maxStackDepth = reader.ReadInt32();
                    _localCount = reader.ReadInt32();
                    _minScopeDepth = reader.ReadInt32();
                    _maxScopeDepth = reader.ReadInt32();
                    _methodFlags = (AbcMethodFlags)reader.ReadByte();
                    _flags = (AbcBodyFlags)reader.ReadByte();

                    //writer.WriteInt32(body.MaxStackDepth);
                    //writer.WriteInt32(body.LocalCount);
                    //writer.WriteInt32(body.MinScopeDepth);
                    //writer.WriteInt32(body.MaxScopeDepth);
                    //writer.WriteByte((byte)method.Flags);
                    
                    int n = reader.ReadInt32();
                    _instructions = new I[n];
                    for (int i = 0; i < n; ++i)
                    {
                        var instr = new I();
                        instr.Read(reader);
                        _instructions[i] = instr;
                    }

                    if ((_flags & AbcBodyFlags.HasExceptions) != 0)
                    {
                        n = reader.ReadInt32();
                        _handlers = new Handler[n];
                        for (int i = 0; i < n; ++i)
                        {
                            var h = new Handler();
                            h.Read(reader);
                            _handlers[i] = h;
                        }
                    }

                    if ((_flags & AbcBodyFlags.HasSlots) != 0)
                    {
                        n = reader.ReadInt32();
                        _slots = new Slot[n];
                        for (int i = 0; i < n; ++i)
                        {
                            var slot = new Slot();
                            slot.Read(reader);
                            _slots[i] = slot;
                        }
                    }

                    if ((_flags & AbcBodyFlags.HasMetadataTokens) != 0)
                    {
                        n = reader.ReadInt32();
                        _tokens = reader.ReadInt32(n);
                    }

                    if ((_flags & AbcBodyFlags.HasFieldPointers) != 0)
                    {
                        n = reader.ReadInt32();
                        _fieldPtrs = reader.ReadInt32(n);
                    }
                }
                #endregion

                #region Import
                public AbcMethodBody Import(SwfReader reader, AbcMethod method, AbcFile abc, BodyReferences refs)
                {
                    try
                    {
                        Read(reader);
                    }
                    catch (Exception e)
                    {
                        //TODO: Warning
                        //Console.WriteLine(e);
                        return null;
                    }

                    var body = new AbcMethodBody
                                   {
                                       MaxStackDepth = _maxStackDepth,
                                       LocalCount = _localCount,
                                       MinScopeDepth = _minScopeDepth,
                                       MaxScopeDepth = _maxScopeDepth,
                                       Method = method,
                                       Flags = _flags,
                                   };

                    ImportHandlers(abc, body);

                    ImportIL(abc, body);

                    method.Flags |= _methodFlags;

                    ImportSlots(abc, body);

                    ResolveTokens(method, refs);

                    if ((_flags & AbcBodyFlags.HasElemPointers) != 0)
                        abc.generator.DefineGetElemPtr();

                    ResolveFieldPointers(method, refs);

                    body.SetupOffsets();
                    body.TranslateIndices();
                    body.ResolveExceptionOffsets(abc);

                    return body;
                }

                #region ImportHandlers
                void ImportHandlers(AbcFile abc, AbcMethodBody body)
                {
                    if ((_flags & AbcBodyFlags.HasExceptions) != 0)
                    {
                        int n = _handlers.Length;
                        for (int i = 0; i < n; ++i)
                        {
                            var h = _handlers[i];
                            var e = new AbcExceptionHandler
                            {
                                From = h.From,
                                To = h.To,
                                Target = h.Target,
                                Type = (h.Type != null ? h.Type.Import(abc) : null),
                                Variable = (h.Var != null ? h.Var.Import(abc) : null)
                            };
                            body.Exceptions.Add(e);
                        }
                    }
                }
                #endregion

                #region ImportIL
                void ImportIL(AbcFile abc, AbcMethodBody body)
                {
                    int n = _instructions.Length;
                    for (int i = 0; i < n; ++i)
                    {
                        var instr = _instructions[i].Import(body, abc);
                        instr.Index = i;
                        body.IL.Add(instr);

                        //if (instr.IsBranchOrSwitch)
                        //    instr.VerifyBranchTargets(n);
                    }
                }
                #endregion

                #region ImportSlots
                void ImportSlots(AbcFile abc, AbcMethodBody body)
                {
                    if ((_flags & AbcBodyFlags.HasSlots) != 0)
                    {
                        int n = _slots.Length;
                        for (int i = 0; i < n; ++i)
                        {
                            var slot = _slots[i];
                            var t = slot.Import(abc);
                            body.Traits.Add(t);
                            if (t.PtrKind != 0)
                            {
                                t.PtrSlot = body.Traits[slot.PtrSlot];
                                switch (t.PtrKind)
                                {
                                    case PointerKind.FuncPtr:
                                        abc.generator.DefineFuncPtr();
                                        break;

                                    case PointerKind.PropertyPtr:
                                        abc.generator.DefinePropertyPtr(t.PtrSlot.Name);
                                        break;

                                    case PointerKind.SlotPtr:
                                        abc.generator.DefineSlotPtr(t.PtrSlot);
                                        break;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region ResolveTokens
                void ResolveTokens(AbcMethod method, BodyReferences refs)
                {
                    if ((_flags & AbcBodyFlags.HasMetadataTokens) != 0)
                    {
                        var sm = method.SourceMethod;
                        var asm = sm.Assembly;
                        var mod = asm.MainModule;
                        int n = _tokens.Length;
                        for (int i = 0; i < n; ++i)
                        {
                            int token = _tokens[i];
                            var mt = mod.ResolveMetadataToken(sm, token);
                            if (mt == null)
                                throw new InvalidOperationException();
                            var member = mt as ITypeMember;
                            if (member == null)
                                throw new InvalidOperationException();
                            refs.Members.Add(member);
                        }
                    }
                }
                #endregion

                #region ResolveFieldPointers
                void ResolveFieldPointers(AbcMethod method, BodyReferences refs)
                {
                    if ((_flags & AbcBodyFlags.HasFieldPointers) != 0)
                    {
                        var sm = method.SourceMethod;
                        var asm = sm.Assembly;
                        var mod = asm.MainModule;
                        int n = _fieldPtrs.Length;
                        for (int i = 0; i < n; ++i)
                        {
                            int token = _fieldPtrs[i];
                            var mt = mod.ResolveMetadataToken(sm, token);
                            if (mt == null)
                                throw new InvalidOperationException();
                            var field = mt as IField;
                            if (field == null)
                                throw new InvalidOperationException();
                            refs.FieldPointers.Add(field);
                        }
                    }
                }
                #endregion
                #endregion
            }
            #endregion

            #region Namespace
            class NS
            {
                public AbcConstKind Kind;
                public string Name;

                public static NS Read(SwfReader reader)
                {
                    var ns = new NS
                                 {
                                     Kind = ((AbcConstKind)reader.ReadByte()),
                                     Name = reader.ReadString()
                                 };
                    return ns;
                }

                public static void Write(SwfWriter writer, AbcNamespace ns)
                {
                    writer.WriteByte((byte)ns.Kind);
                    writer.WriteString(ns.NameString);
                }

                public AbcNamespace Import(AbcFile abc)
                {
                    return abc.DefineNamespace(Kind, Name);
                }

                public override string ToString()
                {
                    return string.Format("{0} {1}", AbcNamespace.GetShortNsKind(Kind), Name);
                }
            }

            class NSSet : List<NS>
            {
                public static NSSet Read(SwfReader reader)
                {
                    var set = new NSSet();
                    int n = reader.ReadInt32();
                    for (int i = 0; i < n; ++i)
                    {
                        var ns = NS.Read(reader);
                        set.Add(ns);
                    }
                    return set;
                }

                public static void Write(SwfWriter writer, AbcNamespaceSet set)
                {
                    int n = set.Count;
                    writer.WriteInt32(n);
                    for (int i = 0; i < n; ++i)
                        NS.Write(writer, set[i]);
                }

                public AbcNamespaceSet Import(AbcFile abc)
                {
                    int n = Count;
                    var nss = new AbcNamespaceSet();
                    for (int i = 0; i < n; ++i)
                        nss.Add(this[i].Import(abc));
                    return abc.ImportConst(nss);
                }
            }
            #endregion

            #region Multiname
            class MName
            {
                public AbcConstKind Kind;
                public NS Namespace;
                public NSSet NSS;
                public string Name;
                public MName Type;
                public MName TypeParam;

                #region Read
                public void Read(SwfReader reader, byte kind)
                {
                    Kind = (AbcConstKind)kind;
                    ReadCore(reader);
                }

                public void Read(SwfReader reader)
                {
                    Kind = (AbcConstKind)reader.ReadByte();
                    ReadCore(reader);
                }

                void ReadCore(SwfReader reader)
                {
                    switch (Kind)
                    {
                        case AbcConstKind.QName:
                        case AbcConstKind.QNameA:
                            Namespace = NS.Read(reader);
                            Name = reader.ReadString();
                            break;

                        case AbcConstKind.RTQName:
                        case AbcConstKind.RTQNameA:
                            Name = reader.ReadString();
                            break;

                        case AbcConstKind.RTQNameL:
                        case AbcConstKind.RTQNameLA:
                            //nothing
                            break;

                        case AbcConstKind.Multiname:
                        case AbcConstKind.MultinameA:
                            NSS = NSSet.Read(reader);
                            Name = reader.ReadString();
                            break;

                        case AbcConstKind.MultinameL:
                        case AbcConstKind.MultinameLA:
                            NSS = NSSet.Read(reader);
                            break;

                        case AbcConstKind.TypeName:
                            Type = ReadMN(reader);
                            TypeParam = ReadMN(reader);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
                #endregion

                #region Write
                public static void Write(SwfWriter writer, AbcMultiname mn)
                {
                    var kind = mn.Kind;
                    writer.WriteByte((byte)kind);
                    switch (kind)
                    {
                        case AbcConstKind.QName:
                        case AbcConstKind.QNameA:
                            NS.Write(writer, mn.Namespace);
                            writer.WriteString(mn.NameString);
                            break;

                        case AbcConstKind.RTQName:
                        case AbcConstKind.RTQNameA:
                            writer.WriteString(mn.NameString);
                            break;

                        case AbcConstKind.RTQNameL:
                        case AbcConstKind.RTQNameLA:
                            //nothing
                            break;

                        case AbcConstKind.Multiname:
                        case AbcConstKind.MultinameA:
                            NSSet.Write(writer, mn.NamespaceSet);
                            writer.WriteString(mn.NameString);
                            break;

                        case AbcConstKind.MultinameL:
                        case AbcConstKind.MultinameLA:
                            NSSet.Write(writer, mn.NamespaceSet);
                            break;

                        case AbcConstKind.TypeName:
                            Write(writer, mn.Type);
                            Write(writer, mn.TypeParameter);
                            break;
                    }
                }
                #endregion

                #region Import
                public AbcMultiname Import(AbcFile abc)
                {
                    switch (Kind)
                    {
                        case AbcConstKind.QName:
                        case AbcConstKind.QNameA:
                            {
                                var ns = Namespace.Import(abc);
                                return abc.DefineQName(ns, Name);
                            }

                        case AbcConstKind.RTQName:
                        case AbcConstKind.RTQNameA:
                            return abc.ImportConst(new AbcMultiname(Kind) { Name = abc.DefineString(Name) });

                        case AbcConstKind.RTQNameL:
                        case AbcConstKind.RTQNameLA:
                            return abc.ImportConst(new AbcMultiname(Kind));

                        case AbcConstKind.Multiname:
                        case AbcConstKind.MultinameA:
                            {
                                var nss = NSS.Import(abc);
                                var name = abc.DefineString(Name);
                                return abc.ImportConst(new AbcMultiname(Kind) { NamespaceSet = nss, Name = name });
                            }

                        case AbcConstKind.MultinameL:
                        case AbcConstKind.MultinameLA:
                            {
                                var nss = NSS.Import(abc);
                                return abc.ImportConst(new AbcMultiname(Kind) { NamespaceSet = nss });
                            }

                        case AbcConstKind.TypeName:
                            {
                                var type = Type.Import(abc);
                                var param = TypeParam.Import(abc);
                                return abc.ImportConst(new AbcMultiname(type, param));
                            }

                        default:
                            throw new NotImplementedException();
                    }
                }
                #endregion
            }
            #endregion

            #region Handler
            class Handler
            {
                public int From, To, Target;
                public MName Type;
                public MName Var;

                public void Read(SwfReader reader)
                {
                    From = reader.ReadInt32();
                    To = reader.ReadInt32();
                    Target = reader.ReadInt32();
                    Type = ReadMN2(reader);
                    Var = ReadMN2(reader);
                }

                public static void Write(SwfWriter writer, AbcExceptionHandler handler)
                {
                    writer.WriteInt32(handler.From);
                    writer.WriteInt32(handler.To);
                    writer.WriteInt32(handler.Target);
                    if (handler.Type != null)
                        MName.Write(writer, handler.Type);
                    else
                        writer.WriteByte(0); //any
                    if (handler.Variable != null)
                        MName.Write(writer, handler.Variable);
                    else
                        writer.WriteByte(0);
                }
            }
            #endregion

            #region Slot
            class Slot
            {
                public MName Name;
                public MName Type;
                public int ID;
                public PointerKind PtrKind;
                public int PtrSlot;
                
                public void Read(SwfReader reader)
                {
                    Name = ReadMN(reader);
                    Type = ReadMN(reader);
                    ID = reader.ReadInt32();
                    PtrKind = (PointerKind)reader.ReadByte();
                    if (PtrKind != 0)
                        PtrSlot = reader.ReadInt32();
                }

                public static void Write(SwfWriter writer, AbcTrait trait)
                {
                    MName.Write(writer, trait.Name);
                    switch (trait.Kind)
                    {
                        case AbcTraitKind.Getter:
                        case AbcTraitKind.Setter:
                        case AbcTraitKind.Method:
                        case AbcTraitKind.Function:
                        case AbcTraitKind.Class:
                        case AbcTraitKind.Const:
                            throw new NotSupportedException();

                        case AbcTraitKind.Slot:
                            MName.Write(writer, trait.SlotType);
                            break;
                    }
                    writer.WriteInt32(trait.SlotID);
                    writer.WriteByte((byte)trait.PtrKind);
                    if (trait.PtrKind != 0)
                    {
                        writer.WriteInt32(trait.PtrSlot.SlotID - 1);
                    }
                }

                public AbcTrait Import(AbcFile abc)
                {
                    var name = Name.Import(abc);
                    var type = Type.Import(abc);
                    var t = AbcTrait.CreateSlot(type, name);
                    t.SlotID = ID;
                    t.PtrKind = PtrKind;
                    return t;
                }
            }
            #endregion

            #region ReadMN
            static MName ReadMN(SwfReader reader)
            {
                var mn = new MName();
                mn.Read(reader);
                return mn;
            }

            static MName ReadMN2(SwfReader reader)
            {
                byte kind = reader.ReadByte();
                if (kind != 0)
                {
                    var mn = new MName();
                    mn.Read(reader, kind);
                    return mn;
                }
                return null;
            }
            #endregion

            #region Instruction
            /// <summary>
            /// Fat instruction
            /// </summary>
            class I
            {
                InstructionCode Code;
                object[] _operands;

                #region Read
                public void Read(SwfReader reader)
                {
                    Code = (InstructionCode)reader.ReadByte();
                    var instr = Instructions.GetInstruction(Code);
                    if (instr.HasOperands)
                    {
                        int n = instr.Operands.Length;
                        _operands = new object[n];
                        for (int i = 0; i < n; ++i)
                        {
                            _operands[i] = ReadOperand(reader, instr.Operands[i].Type);
                        }
                    }
                }
                #endregion

                #region ReadOperand
                static object ReadOperand(SwfReader reader, OperandType type)
                {
                    switch (type)
                    {
                        case OperandType.U8:
                            return (int)reader.ReadByte();

                        case OperandType.U16:
                            return (int)reader.ReadUInt16();

                        case OperandType.S24:
                        case OperandType.BranchTarget:
                            return reader.ReadInt24();

                        case OperandType.U24:
                            return reader.ReadUInt24();

                        case OperandType.ConstInt:
                        case OperandType.MethodIndex:
                        case OperandType.ClassIndex:
                        case OperandType.ExceptionIndex:
                        case OperandType.U30:
                        case OperandType.S30:
                            return reader.ReadInt32();

                        case OperandType.ConstUInt:
                        case OperandType.U32:
                            return reader.ReadUInt32();

                        case OperandType.ConstDouble:
                            return reader.ReadDouble();

                        case OperandType.ConstString:
                            return reader.ReadString();

                        case OperandType.ConstMultiname:
                            return ReadMN(reader);

                        case OperandType.ConstNamespace:
                            return NS.Read(reader);

                        case OperandType.BranchTargets:
                            {
                                int n = reader.ReadInt32();
                                var sw = new int[n];
                                for (int i = 0; i < n; ++i)
                                    sw[i] = reader.ReadInt24();
                                return sw;
                            }

                        default:
                            throw new ArgumentOutOfRangeException("type");
                    }
                }
                #endregion

                #region Import
                public Instruction Import(AbcMethodBody body, AbcFile abc)
                {
                    var instr = new Instruction(Code);
                    if (instr.HasOperands)
                    {
                        int n = instr.Operands.Length;
                        for (int i = 0; i < n; ++i)
                        {
                            var op = instr.Operands[i];
                            op.Value = ImportOperand(body, _operands[i], op.Type, abc);
                        }
                    }
                    return instr;
                }

                static object ImportOperand(AbcMethodBody body, object value, OperandType type, AbcFile abc)
                {
                    var mn = value as MName;
                    if (mn != null)
                        return mn.Import(abc);

                    var ns = value as NS;
                    if (ns != null)
                        return ns.Import(abc);

                    //var nss = value as NSSet;
                    //if (nss != null)
                    //    return nss.Import(abc);

                    switch (type)
                    {
                            case OperandType.ConstInt:
                                return abc.DefineInt((int)value);

                            case OperandType.ConstUInt:
                                return abc.DefineUInt((uint)value);

                            case OperandType.ConstDouble:
                                return abc.DefineDouble((double)value);

                            case OperandType.ConstString:
                                return abc.DefineString((string)value);

                            case OperandType.ExceptionIndex:
                                return body.Exceptions[(int)value];
                    }

                    return value;
                }
                #endregion
            }
            #endregion

            #region Import
            public AbcMethodBody Import(AbcMethod method, AbcFile abc, BodyReferences refs)
            {
                if (method == null) return null;
                var sm = method.SourceMethod;
                if (sm == null) return null;
                var mvid = sm.Assembly.MainModule.Version;
                if (!Load(mvid)) return null;
                int index = GetIdx(method);
                if (index < 0 || index >= _methods.Length) return null;
                var m = _methods[index];
                if (m == null) return null;
                return m.Import(_reader, method, abc, refs);
            }
            #endregion

            #region Load
            bool Load(Guid mvid)
            {
                if (_methods != null) return true;
                _libdata = File.ReadAllBytes(Path);
                _reader = new SwfReader(_libdata);
                MVID = new Guid(_reader.ReadBlock(16)); //16
                if (mvid != MVID) return false;
                int max = _reader.ReadInt32(); //4
                int n = _reader.ReadInt32(); //4
                _methods = new Body[max + 1];
                for (int i = 0; i < n; ++i)
                {
                    int idx = _reader.ReadInt32();
                    int offset = _reader.ReadInt32();
                    _methods[idx] = new Body {Index = idx, Offset = offset};
                }
                return true;
            }
            #endregion

            #region Save
            public void Save(string path)
            {
                using (var writer = new SwfWriter(path))
                    Write(writer);
            }

            #region Write
            public void Write(SwfWriter writer)
            {
                writer.Write(MVID.ToByteArray()); //16
                int n = Methods.Count;
                writer.WriteInt32(GetIdx(Methods[n - 1])); //4
                writer.WriteInt32(n); //methodNum //4

                var md = new byte[n][];
                for (int i = 0; i < n; ++i)
                {
                    md[i] = GetMethodBytes(Methods[i]);
                }

                int offset = 24 + 8 * n;
                for (int i = 0; i < n; ++i)
                {
                    var m = Methods[i];
                    writer.WriteInt32(GetIdx(m));
                    writer.WriteInt32(offset);
                    offset += md[i].Length;
                }

                for (int i = 0; i < n; ++i)
                    writer.Write(md[i]);
            }
            #endregion

            #region GetMethodBytes
            static byte[] GetMethodBytes(AbcMethod method)
            {
                using (var writer = new SwfWriter())
                {
                    WriteMethod(writer, method);
                    return writer.ToByteArray();
                }
            }
            #endregion

            #region WriteMethod
            //instructions
            //exceptions
            //slots
            //tokens
            //field ptrs
            static void WriteMethod(SwfWriter writer, AbcMethod method)
            {
                var sm = method.SourceMethod;
                var declType = sm.DeclaringType;

                //int idx = GetIdx(sm);
                //if (!sm.IsStatic && sm.IsConstructor && sm.DeclaringType.Name == "Version"
                //    && sm.Parameters.Count == 4)
                //    Debugger.Break();

                var body = method.Body;

                //if (body.LocalCount < sm.Parameters.Count + 1)
                //    Debugger.Break();

                int[] tokens = sm.Body.GetReferencedMetadataTokens();

                writer.WriteInt32(body.MaxStackDepth);
                writer.WriteInt32(body.LocalCount);
                writer.WriteInt32(body.MinScopeDepth);
                writer.WriteInt32(body.MaxScopeDepth);
                writer.WriteByte((byte)method.Flags);

                var flags = body.Flags;
                if (body.Exceptions.Count > 0)
                    flags |= AbcBodyFlags.HasExceptions;
                if (body.Traits.Count > 0)
                    flags |= AbcBodyFlags.HasSlots;
                if (tokens != null && tokens.Length > 0)
                    flags |= AbcBodyFlags.HasMetadataTokens;

                writer.WriteByte((byte)flags);

                int n = body.IL.Count;
                writer.WriteInt32(n);
                for (int i = 0; i < n; ++i)
                {
                    var instr = body.IL[i];
                    //if (instr.IsBranchOrSwitch)
                    //    instr.VerifyBranchTargets(n);
                    WriteInstruction(writer, instr);
                }

                if ((flags & AbcBodyFlags.HasExceptions) != 0)
                {
                    n = body.Exceptions.Count;
                    writer.WriteInt32(n);
                    for (int i = 0; i < n; ++i)
                        Handler.Write(writer, body.Exceptions[i]);
                }

                if ((flags & AbcBodyFlags.HasSlots) != 0)
                {
                    n = body.Traits.Count;
                    writer.WriteInt32(n);
                    for (int i = 0; i < n; ++i)
                        Slot.Write(writer, body.Traits[i]);
                }

                if ((flags & AbcBodyFlags.HasMetadataTokens) != 0)
                {
                    WriteArray(writer, tokens);
                }

                if ((flags & AbcBodyFlags.HasFieldPointers) != 0)
                    WriteTokens(writer, body, TokenKind.FieldPtr);

                //TODO: Save refs on compiler generated methods
                //if ((flags & AbcBodyFlags.HasNewArrayInstructions) != 0)
                //    WriteTokens(writer, body, TokenKind.ArrayElementType);
            }

            static void WriteTokens(SwfWriter writer, AbcMethodBody body, TokenKind kind)
            {
                var arr = body.GetTokens(kind);
                WriteArray(writer, arr);
            }

            static void WriteArray(SwfWriter writer, int[] arr)
            {
                if (arr == null)
                {
                    writer.WriteByte(0);
                    return;
                }
                int n = arr.Length;
                writer.WriteInt32(n);
                for (int i = 0; i < n; ++i)
                    writer.WriteInt32(arr[i]);
            }
            #endregion

            #region WriteInstruction
            static void WriteInstruction(SwfWriter writer, Instruction instr)
            {
                writer.WriteByte((byte)instr.Code);
                if (!instr.HasOperands) return;
                foreach (var op in instr.Operands)
                    WriteOperand(writer, op);
            }
            #endregion

            #region WriteOperand
            static void WriteOperand(SwfWriter writer, Operand op)
            {
                var c = op.Value as IAbcConst;
                if (c != null)
                {
                    WriteConst(writer, c);
                    return;
                }

                switch (op.Type)
                {
                    case OperandType.U8:
                        writer.WriteByte((byte)op.ToInt32());
                        break;

                    case OperandType.U32:
                        writer.WriteUInt32((uint)op.ToInt32());
                        break;

                    case OperandType.S30:
                    case OperandType.U30:
                        writer.WriteInt32(op.ToInt32());
                        break;

                    case OperandType.U16:
                        writer.WriteUInt16((ushort)op.ToInt32());
                        break;

                    case OperandType.U24:
                        writer.WriteUInt24((uint)op.ToInt32());
                        break;

                    case OperandType.S24:
                    case OperandType.BranchTarget:
                        writer.WriteInt24(op.ToInt32());
                        break;

                    case OperandType.MethodIndex:
                    case OperandType.ClassIndex:
                        //We should not cache methods with such instructions
                        throw new NotImplementedException();

                    case OperandType.ExceptionIndex:
                		{
                			var h = op.Value as AbcExceptionHandler;
                			writer.WriteInt32(h != null ? h.Index : op.ToInt32());
                		}
                		break;

                    case OperandType.BranchTargets:
                        {
							var switchCases = op.Value as int[] ?? new int[0];
                            int n = switchCases.Length;
                            writer.WriteInt32(n);
                            for (int i = 0; i < n; ++i)
                                writer.WriteInt24(switchCases[i]);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            #endregion

            #region WriteConst
            static void WriteConst(SwfWriter writer, IAbcConst c)
            {
                var mn = c as AbcMultiname;
                if (mn != null)
                {
                    MName.Write(writer, mn);
                    return;
                }

                var ns = c as AbcNamespace;
                if (ns != null)
                {
                    NS.Write(writer, ns);
                    return;
                }

                var set = c as AbcNamespaceSet;
                if (set != null)
                {
                    NSSet.Write(writer, set);
                    return;
                }

                switch (c.Kind)
                {
                    case AbcConstKind.Int:
                        writer.WriteInt32(((AbcConst<int>)c).Value);
                        break;

                    case AbcConstKind.UInt:
                        writer.WriteUInt32(((AbcConst<uint>)c).Value);
                        break;

                    case AbcConstKind.Double:
                        writer.WriteDouble(((AbcConst<double>)c).Value);
                        break;

                    case AbcConstKind.String:
                        writer.WriteString(((AbcConst<string>)c).Value);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            #endregion          
            #endregion
        }
        #endregion

        #region LoadLibs, FindLib
        readonly List<Lib> _libs = new List<Lib>();
        readonly Hashtable _libcache = new Hashtable();

        const string ext = "abz";

        Lib LoadLib(string asmPath)
        {
            string libPath = Path.ChangeExtension(asmPath, "." + ext);
            if (!File.Exists(libPath)) return null;

            var lib = new Lib { Path = libPath };
            _libs.Add(lib);
            _libcache[asmPath] = lib;

            return lib;
        }

        Lib GetLib(IMethod method)
        {
            if (method == null) return null;
            string path = method.Assembly.Location.ToLower();
            var lib = _libcache[path] as Lib;
            if (lib != null) return lib;
            return LoadLib(path);
        }
        #endregion

        #region Utils
        static int GetIdx(AbcMethod m)
        {
            return GetIdx(m.SourceMethod);
        }

        static int GetIdx(IMetadataElement m)
        {
            return (m.MetadataToken & 0x00FFFFFF) - 1;
        }

        static bool HasDiplicates<T>(IList<T> list, Comparison<T> c)
        {
            int n = list.Count;
            for (int i = 0; i < n; ++i)
            {
                var x = list[i];
                for (int j = 0; j != i && j < n; ++j)
                {
                    var y = list[j];
                    if (c(x, y) == 0)
                    {
                        int k = c(x, y);
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }

    class BodyReferences
    {
        public readonly List<ITypeMember> Members = new List<ITypeMember>();
        public readonly List<IField> FieldPointers = new List<IField>();
    }
}