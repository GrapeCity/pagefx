using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.Collections;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.IL
{
    /// <summary>
    /// This the main apparatus (API) to generate ABC code.
    /// </summary>
    class AbcCode : List<IInstruction>
    {
        #region ctor
        public AbcFile abc;

        public AbcCode(AbcFile abc)
        {
            this.abc = abc;
        }
        #endregion

        #region Add
        public Instruction Add(InstructionCode code)
        {
            var instr = new Instruction(code);
            Add(instr);
            return instr;
        }

        public Instruction Add(InstructionCode code, params object[] values)
        {
            var instr = new Instruction(code, values);
            Add(instr);
            return instr;
        }
        #endregion

        /// <summary>
        /// Gets currently used <see ref="AbcGenerator"/>.
        /// </summary>
        public AbcGenerator Generator
        {
            get 
            { 
                var g = abc.generator;
                if (g == null)
                    throw new InvalidOperationException("invalid context");
                return g;
            }
        }

        /// <summary>
        /// Returns last instruction.
        /// </summary>
        public Instruction Last
        {
            get
            {
                int n = Count - 1;
                if (n < 0) return null;
                return (Instruction)this[n];
            }
        }

        /// <summary>
        /// Gets name of builtin runtime type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AbcMultiname this[AvmTypeCode type]
        {
            get { return abc.BuiltinTypes[type]; }
        }

        #region QNames
        public AbcMultiname DefineGlobalQName(string name)
        {
            return abc.DefineGlobalQName(name);
        }

        public void PushQName(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (!name.IsQName)
                throw new ArgumentException("invalid qname");
            Getlex(AvmTypeCode.QName);
            PushNamespace(name.Namespace);
            PushString(name.NameString);
            Construct(2);
        }

        //stack transition: name -> QName(*, name)
        public void CreateQNameAny()
        {
            var qname = abc.BuiltinTypes[AvmTypeCode.QName];
            FindPropertyStrict(qname); //stack [name, global]
            Swap(); //stack [global, name]

            PushAnyNamespace(); //stack [global, name, ns]
            Swap(); //stack [global, ns, name]

            ConstructProperty(qname, 2);
        }
        #endregion

        #region Simple Instructions
        public Instruction Dup()
        {
            return Add(InstructionCode.Dup);
        }

        public Instruction Nop()
        {
            return Add(InstructionCode.Nop);
        }

        public Instruction Swap()
        {
            return Add(InstructionCode.Swap);
        }

        public Instruction Pop()
        {
            return Add(InstructionCode.Pop);
        }

        public void Pop(int n)
        {
            for (int i = 0; i < n; ++i)
                Add(InstructionCode.Pop);
        }

        public Instruction PushScope()
        {
            return Add(InstructionCode.Pushscope);
        }

        public Instruction PopScope()
        {
            return Add(InstructionCode.Popscope);
        }

        public void PushThisScope()
        {
            Add(InstructionCode.Getlocal0);
            Add(InstructionCode.Pushscope);
        }

        public Instruction GetScope(int index)
        {
            return Add(InstructionCode.Getscopeobject, index);
        }

        public Instruction ReturnValue()
        {
            return Add(InstructionCode.Returnvalue);
        }

        public Instruction ReturnVoid()
        {
            return Add(InstructionCode.Returnvoid);
        }

        public void ReturnNull()
        {
            PushNull();
            ReturnValue();
        }

        public void ReturnThis()
        {
            LoadThis();
            ReturnValue();
        }

        public void ReturnTypeOf(IType type)
        {
            TypeOf(type);
            ReturnValue();
        }

        public void ReturnTypeId(IType type)
        {
            PushTypeId(type);
            ReturnValue();
        }

        public void Return(IMethod method)
        {
            if (TypeService.IsVoid(method))
                ReturnVoid();
            else
                ReturnValue();
        }

        /// <summary>
        /// Calls base constructor.
        /// </summary>
        /// <param name="argc">number of arguments to pop from eval stack.</param>
        public void ConstructSuper(int argc)
        {
            Add(InstructionCode.Constructsuper, argc);
        }

        /// <summary>
        /// Loads this and calls base constructor.
        /// </summary>
        public void ConstructSuper()
        {
            LoadThis();
            ConstructSuper(0);
        }

        public void ConstructSuper(bool loadThis, int argc, Action args)
        {
            if (loadThis)
                LoadThis();
            if (args != null)
                args();
            ConstructSuper(argc);
        }

        public Instruction FindProperty(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Findproperty, name);
        }

        public Instruction FindProperty(string name)
        {
            var mn = abc.DefineGlobalQName(name);
            return FindProperty(mn);
        }

        public Instruction FindPropertyStrict(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Findpropstrict, name);
        }

        public Instruction FindPropertyStrict(AbcInstance instance)
        {
            return FindPropertyStrict(instance.Name);
        }

        public Instruction FindPropertyStrict(string name)
        {
            var mn = abc.DefineGlobalQName(name);
            return FindPropertyStrict(mn);
        }
        #endregion

        #region Debug Instructions 
        public Instruction DebugFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentException("Debug file name cannot be null or empty", "file");

            //NOTE: Operand of debugfile instruction is string
            var str = abc.DefineString(file);
            return Add(InstructionCode.Debugfile, str);
        }

        public Instruction DebugLine(int line)
        {
            return Add(InstructionCode.Debugline, line);
        }

        public Instruction DebugLocalInfo(int slot, string name, int line)
        {
            if (slot < 0)
                throw new ArgumentOutOfRangeException("slot");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty");
            if (line < 0)
                throw new ArgumentOutOfRangeException("line");
            var c = abc.DefineString(name);
            return Add(InstructionCode.Debug, 1, c, slot, line);
        }

        bool IsSwf
        {
            get { return Generator.IsSwf; }
        }

        public void DebuggerBreak()
        {
            if (GlobalSettings.EmitDebugInfo)
            {
                if (IsSwf)
                {
                    //HasDebugger();
                    //var noDebugger = IfFalse();

                    CallGlobalFunctionVoid("flash.debugger", "enterDebugger");

                    //noDebugger.BranchTarget = Label();
                }
            }
        }
        #endregion

        #region Branch Instructions
        public Instruction Goto()
        {
            var j = new Instruction(InstructionCode.Jump);
            Add(j);
            return j;
        }

        public Instruction If(BranchOperator op)
        {
            var br = Instruction.If(op);
            Add(br);
            return br;
        }

        public Instruction IfTrue()
        {
            var br = new Instruction(InstructionCode.Iftrue);
            Add(br);
            return br;
        }

        public Instruction IfFalse()
        {
            var br = new Instruction(InstructionCode.Iffalse);
            Add(br);
            return br;
        }

        public Instruction IfEquals()
        {
            var br = new Instruction(InstructionCode.Ifeq);
            Add(br);
            return br;
        }

        public Instruction IfNotEquals()
        {
            var br = new Instruction(InstructionCode.Ifne);
            Add(br);
            return br;
        }

        public Instruction IfNull(bool nullable)
        {
            if (nullable)
            {
                IsNull();
                return IfTrue();
            }
            PushNull();
            return IfEquals();
        }

        public Instruction IfNull()
        {
            return IfNull(false);
        }

        public Instruction IfNullable()
        {
            return IfNull(true);
        }

        public Instruction IfNotNull(bool nullable)
        {
            if (nullable)
            {
                IsNull();
                return IfFalse();
            }
            PushNull();
            return IfNotEquals();
        }

        public Instruction IfNotNull()
        {
            return IfNotNull(false);
        }

        public Instruction IfUndefined()
        {
            PushUndefined();
            return IfEquals();
        }

        public Instruction IfNotUndefined()
        {
            PushUndefined();
            return IfNotEquals();
        }

        public Instruction IfNotType(IType type, bool native)
        {
            //NOTE: null as type is always true
            As(type, native);
            return IfNull();
        }

        public Instruction IfNotType(AbcMultiname type)
        {
            //NOTE: null as type is always true
            As(type);
            return IfNull();
        }

        public Instruction IfNotType(AvmTypeCode type)
        {
            //NOTE: null as type is always true
            As(this[type]);
            return IfNull();
        }

        public Instruction IfType(IType type, bool native)
        {
            //NOTE: null as type is always true
            As(type, native);
            return IfNotNull();
        }

        public Instruction IfType(AbcMultiname type)
        {
            //NOTE: null as type is always true
            As(type);
            return IfNotNull();
        }

        public Instruction IfType(AvmTypeCode type)
        {
            return IfType(this[type]);
        }

        public Instruction IfArray()
        {
            //As(SystemTypes.Array);
            //return IfNotNullable();
            var instance = DefineAbcInstance(SystemTypes.Array);
            Is(instance);
            return IfTrue();
        }

        public Instruction IfNotArray()
        {
            //As(SystemTypes.Array);
            //return IfNullable();
            var instance = DefineAbcInstance(SystemTypes.Array);
            Is(instance);
            return IfFalse();
        }

        public void If(AbcCondition condition, Action trueCode, Action falseCode)
        {
            If(condition, trueCode, falseCode, false);
        }

        public void If(AbcCondition condition, Action trueCode, Action falseCode, bool skip)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (trueCode == null)
                throw new ArgumentNullException("trueCode");
            if (falseCode == null)
                throw new ArgumentNullException("falseCode");

            Instruction j = null;
            var br = condition();
            falseCode();
            if (skip)
                j = Goto();
            br.BranchTarget = Label();
            trueCode();
            if (skip)
                j.BranchTarget = Label();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">local register which stores value</param>
        public void IfUndefinedReturnNull(int value)
        {
            GetLocal(value);
            var ifUndef = IfNotUndefined();
            ReturnNull();
            ifUndef.BranchTarget = Label();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">local register which stores value</param>
        public void IfNullReturnNull(int value, bool nullable)
        {
            GetLocal(value);
            var notNull = IfNotNull(nullable);
            ReturnNull();
            notNull.BranchTarget = Label();
        }

        public void IfNullReturnNull(int value)
        {
            IfNullReturnNull(value, true);
        }

        public void BeginAsMethod()
        {
            IfNullReturnNull(1);
            //IfUndefinedReturnNull(1);
        }
        #endregion

        #region Loops
        public void While(AbcCondition condition, Action body)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (body == null)
                throw new ArgumentNullException("body");

            var begin = Label();

            var br = condition();
            if (!br.IsConditionalBranch)
                throw new InvalidOperationException();

            body();

            var gotoBegin = Goto();
            gotoBegin.BranchTarget = begin;

            var end = Label();
            br.BranchTarget = end;
        }

        public void DoWhile(AbcCondition condition, Action body)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (body == null)
                throw new ArgumentNullException("body");

            var begin = Label();

            body();

            var br = condition();
            if (!br.IsConditionalBranch)
                throw new InvalidOperationException();

            br.BranchTarget = begin;
        }
        #endregion

        #region Get/Set
        public Instruction LoadThis()
        {
            return Add(InstructionCode.Getlocal0);
        }

        public Instruction GetLocal(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "invalid local var index");
            var instr = Instruction.GetLocal(index);
            Add(instr);
            return instr;
        }

        public void GetLocals(int from, int to)
        {
            for (int i = from; i <= to; ++i)
                GetLocal(i);
        }

        public Instruction SetLocal(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "invalid local var index");
            var instr = Instruction.SetLocal(index);
            Add(instr);
            return instr;
        }

        public Instruction GetProperty(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Getproperty, name);
        }

        public Instruction GetProperty(AbcTrait trait)
        {
            if (trait == null)
                throw new ArgumentNullException("trait");
            return GetProperty(trait.Name);
        }

        public Instruction GetProperty(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", "name");
            var mn = abc.DefineGlobalQName(name);
            return GetProperty(mn);
        }

        public void GetStaticProperty(AbcTrait trait)
        {
            if (trait == null)
                throw new ArgumentNullException("trait");
            if (!trait.IsStatic)
                throw new ArgumentException("trait is not static");
            Getlex(trait.Instance);
            GetProperty(trait.Name);
        }

        public void SetStaticProperty(AbcTrait trait, Action getValue)
        {
            if (trait == null)
                throw new ArgumentNullException("trait");
            if (!trait.IsStatic)
                throw new ArgumentException("trait is not static");
            Getlex(trait.Instance);

            if (getValue != null)
                getValue();
            else
                Swap(); //value is onto the stack

            GetProperty(trait.Name);
        }

        public Instruction GetSuper(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Getsuper, name);
        }

        public Instruction GetSuper(string name)
        {
            var mn = abc.DefineGlobalQName(name);
            return GetSuper(mn);
        }

		public Instruction SetSuper(AbcMultiname name)
		{
			if (name == null) throw new ArgumentNullException("name");
			return Add(InstructionCode.Setsuper, name);
		}

		public Instruction SetSuper(string name)
		{
			var mn = abc.DefineGlobalQName(name);
			return SetSuper(mn);
		}

        public Instruction SetProperty(string name)
        {
            var mn = DefineGlobalQName(name);
            return SetProperty(mn);
        }

        public Instruction SetPublicProperty(string ns, string name)
        {
            var mn = abc.DefinePublicQName(ns, name);
            return SetProperty(mn);
        }

        public Instruction SetProperty(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Setproperty, name);
        }

        public Instruction SetProperty(AbcTrait trait)
        {
            if (trait == null)
                throw new ArgumentNullException("trait");
            return SetProperty(trait.Name);
        }

        public Instruction SetMxInternalProperty(string name)
        {
            var mn = abc.DefineMxInternalName(name);
            return SetProperty(mn);
        }

        public Instruction InitProperty(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Initproperty, name);
        }

        public Instruction InitProperty(string name)
        {
            var mn = abc.DefineGlobalQName(name);
            return InitProperty(mn);
        }

        public Instruction GetRuntimeProperty()
        {
            return GetProperty(abc.RuntimeQName);
        }

        public Instruction SetRuntimeProperty()
        {
            return SetProperty(abc.RuntimeQName);
        }

        public AbcMultiname GetFieldName(IField field)
        {
            return Generator.GetFieldName(field);
        }

        public Instruction GetField(IField field)
        {
            var name = GetFieldName(field);
            return GetProperty(name);
        }

        public Instruction GetField(FieldId id)
        {
            return GetField(Generator.GetField(id));
        }

        public Instruction SetField(IField field)
        {
            //No need to call DefineField
            var name = Generator.GetFieldName(field);
            return SetProperty(name);
        }

        public Instruction SetField(FieldId id)
        {
            return SetField(Generator.GetField(id));
        }

        static IField FindField(IType type, string name)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var field = TypeService.FindField(type, name, true);
            if (field == null)
                throw new InvalidOperationException(
                    string.Format("Unable to find field {0} in type {1}", name, type.FullName));
            return field;
        }

        public Instruction SetField(IType type, string name)
        {
            EnsureType(type);
            var field = FindField(type, name);
            return SetField(field);
        }

        public void SetField(int var, IType type, string name, Action value)
        {
            GetLocal(var);
            value();
            SetField(type, name);
        }

        public void SetProperty(IType type, string prop)
        {
            var p = TypeService.FindProperty(type, prop, true);
            if (p == null)
                throw new InvalidOperationException(
                    string.Format("Unable to find property {0} in type {1}", prop, type.FullName));
            if (p.Setter == null)
                throw new InvalidOperationException(
                    string.Format("Property {0} is readonly in type {1}", prop, type.FullName));
            var m = DefineAbcMethod(p.Setter);
            Call(m);
        }

        public void SetProperty(int var, IType type, string prop, Action value)
        {
            GetLocal(var);
            value();
            SetProperty(type, prop);
        }

        public void SetPropertyBool(int var, IType type, string prop, bool value)
        {
            SetProperty(var, type, prop, () => PushBool(value));
        }

        public void SetPropertyString(int var, IType type, string prop, string value)
        {
            SetProperty(var, type, prop, () => PushString(value));
        }

        public Instruction Getlex(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Getlex, name);
        }

        public Instruction Getlex(AbcInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            return Getlex(instance.Name);
        }

        /// <summary>
        /// Loads method class object onto the stack.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public Instruction Getlex(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            var instance = method.Instance;
            if (instance == null)
                throw new ArgumentException("method is not instance method");
            return Getlex(instance.Name);
        }

        public Instruction Getlex(string ns, string name)
        {
            var mn = abc.DefinePackageQName(ns, name);
            return Getlex(mn);
        }

        public Instruction Getlex(string name)
        {
            var mn = abc.DefineGlobalQName(name);
            return Getlex(mn);
        }

        public Instruction Getlex(IType type)
        {
            var name = abc.GetTypeName(type, false);
            return Getlex(name);
        }

        public Instruction Getlex(AvmTypeCode type)
        {
            return Getlex(this[type]);
        }

        public Instruction GetSlot(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id", id, "invalid slot id");
            return Add(InstructionCode.Getslot, id);
        }

        public Instruction SetSlot(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id", id, "invalid slot id");
            return Add(InstructionCode.Setslot, id);
        }

        public Instruction GetSlot(AbcTrait t)
        {
            if (t == null)
                throw new ArgumentNullException("t");
            if (!t.IsSlot)
                throw new ArgumentException("trait is not slot");
#if false //DEBUG
            return GetProperty(t.Name);
#else
            if (t.SlotID <= 0)
                return GetProperty(t.Name);
            return Add(InstructionCode.Getslot, t.SlotID);
#endif
        }

        public Instruction SetSlot(AbcTrait t)
        {
            if (t == null)
                throw new ArgumentNullException("t");
            if (!t.IsSlot)
                throw new ArgumentException("trait is not slot");
#if false //DEBUG
            return SetProperty(t.Name);
#else
            if (t.SlotID <= 0)
                return SetProperty(t.Name);
            return Add(InstructionCode.Setslot, t.SlotID);
#endif
        }

		public void GetClassRef(AbcMultiname name)
		{
			if (name == null) throw new ArgumentNullException("name");
			FindPropertyStrict(name);
			GetProperty(name);
			CoerceClass();
		}

		public void GetClassRef(AbcInstance instance)
		{
			GetClassRef(instance.Name);
		}
        #endregion

        #region Push Methods
        /// <summary>
        /// Pushes byte value onto the eval stack.
        /// </summary>
        /// <param name="value">value to push</param>
        public void PushByte(byte value)
        {
            Add(InstructionCode.Pushbyte, value);
        }

        /// <summary>
        /// Pushes short value onto the eval stack.
        /// </summary>
        /// <param name="value">value to push</param>
        public void PushShort(short value)
        {
            Add(InstructionCode.Pushshort, value);
        }

        /// <summary>
        /// Pushes any object value onto the eval stack.
        /// </summary>
        /// <param name="value">value to push</param>
        public void PushValue(AbcCode code, object value)
        {
            var type = value as IType;
            if (type != null)
            {
                code.TypeOf(type);
                return;
            }

            var arr = value as Array;
            if (arr != null)
            {
                var itype = GetArrayElementType(arr);

                int n = arr.Length;
                code.PushInt(n);
                code.NewArray(itype);
                
                for (int i = 0; i < n; i++)
                {
                    code.Dup();
                    code.PushInt(i);
                    code.PushValue(code, arr.GetValue(i));
                    code.SetArrayElem(false);
                }
                return;
            }
            code.LoadConstant(value);
        }

    	private static IType GetArrayElementType(Array arr)
        {
            var type = arr.GetType().GetElementType();
            return SystemTypes.GetType(type);
        }

        /// <summary>
        /// Pushes int value onto the eval stack.
        /// </summary>
        /// <param name="value"></param>
        public void PushInt(int value)
        {
            //NOTE: It seems that "pushbyte" instr in AVM loads signed integer. Therefore we should use sbyte range.
            if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            {
                PushByte((byte)value);
                return;
            }

            if (value >= short.MinValue && value <= short.MaxValue)
            {
                PushShort((short)value);
                return;
            }

            var c = abc.IntPool.Define(value);
            Add(InstructionCode.Pushint, c);
        }

        /// <summary>
        /// Pushes uint value onto the eval stack.
        /// </summary>
        /// <param name="value"></param>
        public void PushUInt(uint value)
        {
            if (value >= 0 && value <= sbyte.MaxValue)
            {
                PushByte((byte)value);
                CoerceUInt32();
                return;
            }

            if (value <= short.MaxValue)
            {
                PushShort((short)value);
                CoerceUInt32();
                return;
            }

            var c = abc.UIntPool.Define(value);
            Add(InstructionCode.Pushuint, c);
        }

        public void PushInt64(long value)
        {
            if (AvmConfig.SupportNativeInt64)
            {
                throw new NotImplementedException();
            }
            else
            {
                CreateInstance(SystemTypes.Int64, true);
                Add(InstructionCode.Dup);
                Add(InstructionCode.Dup);
                ulong v = (ulong)value;
                PushInt((int)((uint)(v & 0xFFFFFFFF)));
                SetProperty(Const.Int64.LoField);
                PushUInt((uint)((v >> 32) & 0xFFFFFFFF));
                SetProperty(Const.Int64.HiField);
            }
            //PushInt((int)value);
        }

        public void PushUInt64(ulong value)
        {
            if (AvmConfig.SupportNativeInt64)
            {
                throw new NotImplementedException();
            }
            else
            {
                CreateInstance(SystemTypes.UInt64, true);
                Add(InstructionCode.Dup);
                Add(InstructionCode.Dup);
                PushUInt((uint)(value & 0xFFFFFFFF));
                SetProperty(Const.Int64.LoField);
                PushUInt((uint)((value >> 32) & 0xFFFFFFFF));
                SetProperty(Const.Int64.HiField);
            }
            //PushUInt((uint)value);
        }

        public void PushString(AbcConst<string> s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            Add(InstructionCode.Pushstring, s);
        }

        public void PushString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            var c = abc.DefineString(value);
            Add(InstructionCode.Pushstring, c);
        }

        public void PushStrings(IEnumerable<string> set)
        {
            foreach (var s in set)
                PushString(s);
        }

        public void PushStringArray(IEnumerable<string> set)
        {
            int n = 0;
            foreach (var s in set)
            {
                PushString(s);
                ++n;
            }
            Add(InstructionCode.Newarray, n);
        }

        public void PushSystemString(string value)
        {
            if (AbcGenConfig.UseAvmString)
            {
                PushString(value);
            }
            else
            {
                CreateInstance(SystemTypes.String, true);
                Dup();
                PushString(value);
                SetProperty(Const.String.Value);
            }
        }

        public void PushBool(bool value)
        {
            if (AvmConfig.BooleanAsInt)
            {
                PushInt(value ? 1 : 0);
            }
            else
            {
            	Add(value ? InstructionCode.Pushtrue : InstructionCode.Pushfalse);
            }
        }

        public void PushTrue()
        {
            PushBool(true);
        }

        public void PushFalse()
        {
            PushBool(false);
        }

        public Instruction PushNativeBool(bool value)
        {
            if (value)
                return Add(InstructionCode.Pushtrue);
            return Add(InstructionCode.Pushfalse);
        }

        public Instruction PushNull()
        {
            return Add(InstructionCode.Pushnull);
        }

        public Instruction PushUndefined()
        {
            return Add(InstructionCode.Pushundefined);
        }

        public Instruction PushNaN()
        {
            return Add(InstructionCode.Pushnan);
        }

        public void PushDouble(double value)
        {
            if (double.IsNaN(value))
            {
                PushNaN();
            }
            else
            {
                var c = abc.DoublePool.Define(value);
                Add(InstructionCode.Pushdouble, c);
            }
        }

        public void PushFloat(float value)
        {
            PushDouble(AbcHelper.ToDouble(value));
        }

        public void PushNamespace(AbcNamespace ns)
        {
            if (ns == null) throw new ArgumentNullException();
            Add(InstructionCode.Pushnamespace, ns);
        }

        public void PushAnyNamespace()
        {
            PushNamespace(abc.Namespaces[0]);
        }

        public void PushGlobalPackage()
        {
            PushNamespace(abc.GlobalPackage);
        }

        public void LoadConstant(object value)
        {
            if (value == null)
            {
                PushNull();
                return;
            }

            var tc = Type.GetTypeCode(value.GetType());
            switch (tc)
            {
                case TypeCode.Boolean:
                    PushBool((bool)value);
                    break;

                case TypeCode.SByte:
                    PushInt((sbyte)value);
                    break;

                case TypeCode.Byte:
                    PushUInt((byte)value);
                    break;

                case TypeCode.Int16:
                    PushInt((short)value);
                    break;

                case TypeCode.UInt16:
                    PushUInt((ushort)value);
                    break;

                case TypeCode.Int32:
                    PushInt((int)value);
                    break;

                case TypeCode.UInt32:
                    PushUInt((uint)value);
                    break;

                case TypeCode.Int64:
                    PushInt64((long)value);
                    break;

                case TypeCode.UInt64:
                    PushUInt64((ulong)value);
                    break;

                case TypeCode.Char:
                    PushUInt((char)value);
                    break;

                case TypeCode.Single:
                    PushFloat((float)value);
                    break;

                case TypeCode.Double:
                    PushDouble((double)value);
                    break;

                case TypeCode.Decimal:
                    {
                        throw new NotImplementedException();
                    }

                case TypeCode.String:
                    {
                        PushSystemString((string)value);
                        //return Arr(PushString((string)value));
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }
        #endregion

        #region CreateInstance
		public void CreateInstance(AbcMultiname name, Func<int> pushArgs)
        {
            if (name == null) throw new ArgumentNullException();
            Getlex(name);

			int argCount = 0;
			if (pushArgs != null)
			{
				argCount = pushArgs();
			}

            Construct(argCount);
        }

		public void CreateInstance(AbcMultiname name)
		{
			CreateInstance(name, null);
		}

		public void CreateInstance(AbcInstance instance, Func<int> pushArgs)
        {
            if (instance == null) throw new ArgumentNullException();
            CreateInstance(instance.Name, pushArgs);
        }

		public void CreateInstance(AbcInstance instance)
		{
			CreateInstance(instance, null);
		}

		public void CreateInstance(IType type, bool mustBeInstance, Func<int> pushArgs)
        {
            var tag = Generator.DefineType(type);
            if (mustBeInstance && !(tag is AbcInstance))
                throw new InvalidOperationException("Type has invalid tag. Tag must be AbcInstance.");
            var mn = abc.GetTypeName(type, false);
            CreateInstance(mn, pushArgs);
        }

		public void CreateInstance(IType type, bool mustBeInstance)
		{
			CreateInstance(type, mustBeInstance, null);
		}

        public Instruction ConstructProperty(AbcMultiname name, int argc)
        {
            return Add(InstructionCode.Constructprop, name, argc);
        }

		public Instruction ConstructProperty(AbcInstance instance, int argc)
		{
			return Add(InstructionCode.Constructprop, instance.Name, argc);
		}

        public Instruction ConstructProperty(IType type, int argc)
        {
            var name = TypeHelper.GetTypeMultiname(type);
            return ConstructProperty(name, argc);
        }

        public Instruction Construct(int argc)
        {
            return Add(InstructionCode.Construct, argc);
        }

        public Instruction ApplyType(int argc)
        {
            return Add(InstructionCode.Applytype, argc);
        }

        public void LoadGenericClass(AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (!name.IsParameterizedType)
                throw new ArgumentException("type name is not parameterized", "name");
            Getlex(name.Type);

            int n = 1;
            var p = name.TypeParameter;
            while (p.IsParameterizedType)
            {
                Getlex(p.Type);
                p = p.TypeParameter;
                ++n;
            }
            Getlex(p);

            for (int i = 0; i < n; ++i)
                ApplyType(1);
        }
        #endregion

        #region Casting, Coercion
        public Instruction Coerce(IType type, bool native)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            EnsureType(type);
            var name = abc.GetTypeName(type, native);
            return Coerce(name);
        }

        public Instruction Coerce(AbcMultiname type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var c = abc.BuiltinTypes.GetCoercionInstructionCode(type);
            if (c == InstructionCode.Coerce)
                return Add(c, type);
            return Add(c);
        }

        public Instruction Coerce(AbcInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            return Coerce(instance.Name);
        }

        public Instruction Coerce(AvmTypeCode type)
        {
            return Coerce(this[type]);
        }

        public Instruction CoerceArray()
        {
            return Coerce(abc.BuiltinTypes.Array);
        }

        public Instruction CoerceFunction()
        {
            return Coerce(abc.BuiltinTypes.Function);
        }

        public Instruction CoerceClass()
        {
            return Coerce(abc.BuiltinTypes.Class);
        }

        public Instruction CoerceBool()
        {
            if (AvmConfig.BooleanAsInt)
                return Add(InstructionCode.Convert_i);
            return Add(InstructionCode.Convert_b);
        }

        public Instruction CoerceInt32()
        {
            return Add(InstructionCode.Coerce_i);
        }

        public Instruction CoerceUInt32()
        {
            return Add(InstructionCode.Coerce_u);
        }

        public Instruction CoerceChar()
        {
            return Add(InstructionCode.Coerce_u);
        }

        public Instruction CoerceDouble()
        {
            return Add(InstructionCode.Coerce_d);
        }

        public Instruction CoerceString()
        {
            return Add(InstructionCode.Coerce_s);
        }

        public Instruction CoerceObject()
        {
            return Add(InstructionCode.Coerce_o);
        }

        public Instruction CoerceAnyType()
        {
            return Add(InstructionCode.Coerce_a);
        }

        public Instruction CoerceXML()
        {
            return Coerce(abc.BuiltinTypes.XML);
        }

        public Instruction CoerceXMLList()
        {
            return Coerce(abc.BuiltinTypes.XMLList);
        }

        public void FixBool()
        {
            if (AvmConfig.BooleanAsInt)
                Add(InstructionCode.Convert_i);
        }

        AbcInstance DefineAbcInstance(IType type)
        {
            return Generator.DefineAbcInstance(type);
        }

        AbcMethod DefineAbcMethod(IMethod m)
        {
            return Generator.DefineAbcMethod(m);
        }

        AbcMethod DefineAbcMethod(IType type, string name, int argCount)
        {
            var m = MethodHelper.Find(type, name, argCount);
            if (m == null)
                throw new InvalidOperationException();
            return DefineAbcMethod(m);
        }

        void EnsureType(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            Generator.DefineType(type);
        }

        static bool AsNative(IType type)
        {
            if (type == SystemTypes.Object) return true;
            return false;
        }

        public void As(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            As(type, AsNative(type));
        }

        public void As(IType type, bool native)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            EnsureType(type);

            if (TypeHelper.MustUseCastToMethod(type, true))
            {
                var m = Generator.DefineCastToMethod(type, false);
                GetlexSwapCall(m);
                return;
            }

            As(abc.GetTypeName(type, native));
        }

        public Instruction As(AbcMultiname type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return Add(InstructionCode.Astype, type);
        }

        public Instruction As(AbcInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            return As(instance.Name);
        }

        public Instruction As(AvmTypeCode type)
        {
            return As(this[type]);
        }

        public Instruction Is(AbcMultiname typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");
            return Add(InstructionCode.Istype, typeName);
        }

        public Instruction Is(AbcInstance instance)
        {
            return Is(instance.Name);
        }

        public Instruction Is(AvmTypeCode type)
        {
            return Is(this[type]);
        }

        public Instruction Label()
        {
            var label = new Instruction(InstructionCode.Label);
            Add(label);
            return label;
        }

        private static IType ChangeType(IType type)
        {
            if (type == null) return null;
            return type.IsEnum ? type.ValueType : type;
        }

        static bool IsNative(IType type)
        {
            if (type == null) return false;

            while (type != null)
            {
                var instance = type.Tag as AbcInstance;
                if (instance != null)
                {
                    var name = instance.Name;
                    if (name.IsObject)
                        return true;
                }
                type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Performs type conversion without overflow checking.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Cast(IType source, IType target)
        {
            Cast(source, target, false);
        }

        /// <summary>
        /// Performs type conversion
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="checkOverflow"></param>
        public void Cast(IType source, IType target, bool checkOverflow)
        {
            source = ChangeType(source);
            target = ChangeType(target);

            if (source == target)
                return;

            if (checkOverflow)
            {
                CastOverflow(source, target);
                return;
            }

            if (source == null)
            {
                Coerce(target, true);
                return;
            }

            if (target == SystemTypes.Object)
            {
                if (IsNative(source)) return;

                //NOTE: Explicit conversion to System.Object. Do not use native Object type.
                Coerce(target, true);
                return;
            }

            var type = TypeHelper.SelectDecimalOrInt64(source, target);
            if (type != null && SystemTypes.IsNumeric(target))
            {
                CallCastOp(source, target, type);
                return;
            }

            if (AbcGenConfig.UseAvmString && TypeHelper.IsStringInterface(target))
            {
                CastToStringInterface(source, target);
                return;
            }

            if (IsUnbox(source, target))
            {
                Unbox(target);
                return;
            }

            if (TryCastToSystemType(source, target))
                return;

            CastTo(target);
        }

        public void CastTo(IType target)
        {
            if (TryCastToSystemType(null, target))
                return;

            if (TypeHelper.MustUseCastToMethod(target, false))
            {
                CallCastToMethod(target);
                return;
            }

            if (target == SystemTypes.String)
            {
                CastToString();
                return;
            }

            Coerce(target, true);
        }

        void CastToString()
        {
            var m = Generator.DefineCastToString();
            GetlexSwapCall(m);
        }

        static bool IsUnbox(IType source, IType target)
        {
            if (!TypeHelper.IsValueType(target)) return false;
            if (source == null) return true;
            if (source == SystemTypes.Object) return true;
            return false;
        }

        #region CastToStringInterface
        void CastToStringInterface(IType source, IType target)
        {
            if (source == SystemTypes.String)
            {
                CoerceObject();
                return;
            }

            var m = Generator.DefineCastToStringInterface(target, true);
            GetlexSwapCall(m);
        }
        #endregion

        #region CallConvert
        public IType GetType(CorlibTypeId id)
        {
            return Generator.GetType(CorlibTypeId.Convert);
        }
        
        void CallConvert(IType source, IType target)
        {
            //var type = GetType(CorlibTypeId.Convert);
            //EnsureType(type);

            //string mname = MethodHelper.GetConvertMethodName(target);
            //var method = MethodHelper.Find(type, mname, source);
            //if (method == null)
            //    throw new InvalidOperationException();

            //var m = DefineAbcMethod(method);

            var id = MethodHelper.GetConvertMethodId(source, target);
            if (id == ConvertMethodId.Unknown)
                throw new InvalidOperationException();
            var m = Generator.GetMethod(id);

            GetlexSwapCall(m);
        }

        public void GetlexSwapCall(AbcMethod method)
        {
            Getlex(method);
            Swap();
            Call(method);
        }
        #endregion

        void CastOverflow(IType source, IType target)
        {
            CallConvert(source, target);
        }

        void CallCastToMethod(IType target)
        {
            var typeName = abc.GetTypeName(target, true);
            var m = Generator.DefineCastToMethod(target, true);
            GetlexSwapCall(m);
            Coerce(typeName);
        }

        void CallCastOp(IType source, IType target, IType type)
        {
            var m = Generator.GetCastOperator(source, target);
            if (m == null)
            {
                var op = OpHelper.FindCastOperator(type, source, target);
                Debug.Assert(target == op.Type);

                m = DefineAbcMethod(op);
                Generator.CacheCastOperator(source, target, m);
            }
            GetlexSwapCall(m);
        }

        #region TryCastToSystemType
        public bool TryCastToSystemType(IType source, IType target)
        {
            if (target == null) return false;
            if (target.IsEnum)
                target = target.ValueType;
            var to = target.SystemType;
            if (to == null) return false;
            return TryCastToSystemType(source, to);
        }

        public bool TryCastToSystemType(IType source, SystemType to)
        {
            if (to == null) return false;

            SystemType from = null;
            if (source != null)
            {
                if (source.IsEnum)
                    source = source.ValueType;
                from = source.SystemType;
            }

            switch (to.TypeCode)
            {
                case TypeCode.Byte:
                    {
                        Add(InstructionCode.Coerce_u);
                        PushUInt(0xFF);
                        Add(InstructionCode.Bitand);
                        Add(InstructionCode.Coerce_u);
                        return true;
                    }

                case TypeCode.SByte:
                    {
                        //TODO: Optimize unsigned to signed conversion 
                        Add(InstructionCode.Coerce_u);
                        PushUInt(0xFF);
                        Add(InstructionCode.Bitand); //u8

                        Add(InstructionCode.Dup); //u8, u8
                        PushUInt(0x80);
                        Add(InstructionCode.Bitand); //0x80 or 0x00
                        PushInt(7);
                        Add(InstructionCode.Rshift); //0x01 or 0x00
                        PushUInt(0xFFFFFF00);
                        Add(InstructionCode.Multiply_i); //0xFFFFFF00 if 1

                        Add(InstructionCode.Bitor);

                        CoerceInt32();
                        return true;
                    }

                case TypeCode.UInt16:
                case TypeCode.Char:
                    {
                        Add(InstructionCode.Coerce_u);
                        PushUInt(0xFFFF);
                        Add(InstructionCode.Bitand);
                        Add(InstructionCode.Coerce_u);
                        return true;
                    }

                case TypeCode.Int16:
                    {
                        //TODO: Optimize unsigned to signed conversion 
                        Add(InstructionCode.Coerce_u);
                        PushUInt(0xFFFF);
                        Add(InstructionCode.Bitand); //u16

                        Add(InstructionCode.Dup); //u16, u16
                        PushUInt(0x8000);
                        Add(InstructionCode.Bitand); //0x8000 or 0x0000
                        PushInt(15);
                        Add(InstructionCode.Rshift); //0x01 or 0x00
                        PushUInt(0xFFFF0000);
                        Add(InstructionCode.Multiply_i);

                        Add(InstructionCode.Bitor);

                        CoerceInt32();
                        return true;
                    }

                case TypeCode.UInt32:
                    {
                        Add(InstructionCode.Coerce_u);
                        return true;
                    }

                case TypeCode.Int32:
                    {
                        if (from == null)
                        {
                            CoerceInt32();
                            return true;
                        }

                        switch (from.TypeCode)
                        {
                            case TypeCode.Byte:
                                PushUInt(0xFF);
                                Add(InstructionCode.Bitand);
                                CoerceInt32();
                                return true;

                            case TypeCode.Char:
                            case TypeCode.UInt16:
                                PushUInt(0xFFFF);
                                Add(InstructionCode.Bitand);
                                CoerceInt32();
                                return true;

                            default:
                                CoerceInt32();
                                return true;
                        }
                    }
            }
            return false;
        }
        #endregion
        #endregion

        #region LoadArguments
        public void LoadArguments(int n)
        {
            for (int i = 0; i < n; ++i)
                GetLocal(i + 1);
        }

        public void LoadArguments(IMethod method)
        {
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
                GetLocal(i + 1);
        }
        #endregion

        #region Call
        public void Call(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException();
            if (method.TraitName == null)
                throw new ArgumentException();
            if (method.IsInitializer)
                throw new ArgumentException();

            var name = method.TraitName;

            if (method.IsGetter)
            {
                Add(InstructionCode.Getproperty, name);
                return;
            }

            if (method.IsSetter)
            {
                Add(InstructionCode.Setproperty, name);
                return;
            }

            int n = method.Parameters.Count;
        	Add(method.IsVoid ? InstructionCode.Callpropvoid : InstructionCode.Callproperty, name, n);
        }

        public void Call(AbcMultiname name, int argc)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            Add(InstructionCode.Callproperty, name, argc);
        }

        /// <summary>
        /// Calls property with global qname (i.e. namespace == global package).
        /// </summary>
        /// <param name="name">name of function to call.</param>
        /// <param name="argc">number of arguments to take from the eval stack.</param>
        public void CallGlobal(string name, int argc)
        {
            Call(DefineGlobalQName(name), argc);
        }

        /// <summary>
        /// Calls global function
        /// </summary>
        /// <param name="ns">name of package where function is defined</param>
        /// <param name="name">name of function to call</param>
        /// <param name="argc">number of arguments</param>
        /// <param name="args">code that pushes arguments onto the stack</param>
        public void CallGlobalFunction(string ns, string name, int argc, Action args)
        {
            var mn = abc.DefineQName(ns, name);
            FindPropertyStrict(mn);
            if (args != null)
                args();
            Call(mn, argc);
        }

        /// <summary>
        /// Calls global function with no arguments
        /// </summary>
        /// <param name="ns">name of package where function is defined</param>
        /// <param name="name">name of function to call</param>
        public void CallGlobalFunction(string ns, string name)
        {
            CallGlobalFunction(ns, name, 0, null);
        }

        /// <summary>
        /// Calls global function
        /// </summary>
        /// <param name="ns">name of package where function is defined</param>
        /// <param name="name">name of function to call</param>
        /// <param name="argc">number of arguments</param>
        /// <param name="args">code that pushes arguments onto the stack</param>
        public void CallGlobalFunctionVoid(string ns, string name, int argc, Action args)
        {
            var mn = abc.DefineQName(ns, name);
            FindPropertyStrict(mn);
            if (args != null)
                args();
            CallVoid(mn, argc);
        }

        /// <summary>
        /// Calls global function with no arguments
        /// </summary>
        /// <param name="ns">name of package where function is defined</param>
        /// <param name="name">name of function to call</param>
        public void CallGlobalFunctionVoid(string ns, string name)
        {
            CallGlobalFunctionVoid(ns, name, 0, null);
        }

        public Instruction CallVoid(AbcMultiname name, int argc)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return Add(InstructionCode.Callpropvoid, name, argc);
        }

        public Instruction CallVoid(string name, int argc)
        {
            return CallVoid(DefineGlobalQName(name), argc);
        }

        public void CallClosure(int argc)
        {
            Add(InstructionCode.Call, argc);
        }

        public void CallAS3(string name, int argc)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty", "name");
            var mn = abc.DefineAS3Name(name);
            Call(mn, argc);
        }

        public void ApplyFunction(int argc)
        {
            CallAS3("apply", argc);
        }

        public void CallFunction(int argc)
        {
            CallAS3("call", argc);
        }

        public void CallStatic(AbcMethod method, Action args)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (method.IsInitializer)
            {
                var instance = method.Instance;
                Getlex(instance);
                if (args != null)
                    args();
                Construct(method.ParamCount);
            }
            else
            {
                Getlex(method);
                if (args != null)
                    args();
                Call(method);
            }
        }

        public void CallStatic(AbcMethod method)
        {
            CallStatic(method, null);
        }

        public void AddEventListener()
        {
            CallVoid(Const.Delegate.AddEventListener, 2);
        }

        public void RemoveEventListener()
        {
            CallVoid(Const.Delegate.RemoveEventListener, 2);
        }

        public void Call(ArrayMethodId id)
        {
            Call(Generator.GetMethod(id));
        }

        public void Call(ObjectMethodId id)
        {
            Call(Generator.GetMethod(id));
        }

        public void Call(TypeMethodId id)
        {
            Call(Generator.GetMethod(id));
        }

        public void Call(EnvironmentMethodId id)
        {
            Call(Generator.GetMethod(id));
        }

        public void Call(ConsoleMethodId id)
        {
            Call(Generator.GetMethod(id));
        }
        #endregion

        #region CopySlots, CopyValue, CopyFrom
        /// <summary>
        /// Copies slots from this to given object.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="from">local register of source object</param>
        /// <param name="to">local register of destination object</param>
        public void CopySlots(AbcInstance instance, int from, int to)
        {
            foreach (var t in instance.Traits)
            {
                if (t.Kind == AbcTraitKind.Slot)
                {
                    var prop = t.Name;
                    GetLocal(to);
                    GetLocal(from);
                    GetProperty(prop);
                    if (TypeHelper.HasCopy(t.Type))
                        CopyValue(t.Type);
                    SetProperty(prop);
                }
            }
        }

        public void CopyValue(IType type)
        {
            EnsureType(type);

            //NOTE:
            //No need to copy strings because string is immutable object,
            //but string must be copied in assign instructions.
            //if (type == SystemTypes.String)
            //{
            //    CopyString(code);
            //    return;
            //}

            if (TypeHelper.HasCopy(type))
            {
                var instance = type.Tag as AbcInstance;
                if (instance != null)
                {
                    var method = Generator.DefineCopyMethod(instance);
                    if (method != null)
                    {
                        Call(method);
                        //NOTE: This fixes verify error (cannot reconcily types) when copy method is called on '*' types.
                        Coerce(instance);
                    }
                }
                else
                {
                    //TODO: Warning or Error?
                }
            }
        }

        /// <summary>
        /// Copies slots from value onto the stack
        /// </summary>
        /// <param name="type"></param>
        public void CopyFrom(IType type)
        {
            EnsureType(type);

            if (TypeHelper.HasCopy(type))
            {
                var instance = type.Tag as AbcInstance;
                if (instance != null)
                {
                    var method = Generator.DefineCopyFromMethod(instance);
                    if (method != null)
                        Call(method);
                }
                else
                {
                    //TODO: Warning or Error?
                }
            }
        }
        #endregion

        #region Array API
        /// <summary>
        /// Creates native AVM array
        /// </summary>
        /// <param name="n">integer constant that specifies array size</param>
        public void CreateArray(int n)
        {
            var type = abc.BuiltinTypes.Array;
            Getlex(type);
            PushInt(n);
            Construct(1);
            CoerceArray();
        }

        public void CreateArrayVarSize(int varSize)
        {
            var type = abc.BuiltinTypes.Array;
            Getlex(type);
            GetLocal(varSize);
            Construct(1);
            CoerceArray();
        }

        /// <summary>
        /// Gets "length" property of native AVM array/vector
        /// </summary>
        public void GetArrayLength()
        {
            GetProperty("length");
        }

        /// <summary>
        /// Gets array length (unsigned integer) and coerce it to signed integer.
        /// </summary>
        public void GetArrayLengthInt()
        {
            GetArrayLength();
            CoerceInt32();
        }

        /// <summary>
        /// Calls native array indexer
        /// </summary>
        public void GetNativeArrayItem()
        {
            GetProperty(abc.NameArrayIndexer);
            CoerceObject();
        }

        public void GetNativeArrayItem(int arrayVar, int index)
        {
            GetLocal(arrayVar);
            PushInt(index);
            GetNativeArrayItem();
        }

        /// <summary>
        /// Sets value for item of native array
        /// </summary>
        public void SetNativeArrayItem()
        {
            SetProperty(abc.NameArrayIndexer);
        }

        /// <summary>
        /// Creates one-dimensional System.Array object.
        /// </summary>
        /// <param name="elemType">element type</param>
        public void NewArray(IType elemType)
        {
            var m = Generator.NewArray(elemType);

            //NOTE:
            //array size must be on stack [..., size]
            //stack transition: ..., size -> arr

            GetlexSwapCall(m);
        }

        #region NewArray
        /// <summary>
        /// Creates and initializes new System.Array.
        /// </summary>
        /// <typeparam name="T">type of array element</typeparam>
        /// <param name="arrVar">slot to store array</param>
        /// <param name="elemType">type of array element</param>
        /// <param name="elemNum">number of array elements</param>
        /// <param name="set">set of array elements</param>
        /// <param name="getItem">code to push element onto the eval stack.</param>
        public void NewArray<T>(int arrVar, IType elemType, int elemNum, IEnumerable<T> set, Action<T,int> getItem)
        {
            if (getItem == null)
                throw new ArgumentNullException("getItem");

            bool dup = arrVar < 1;
            PushInt(elemNum);
            NewArray(elemType);

            if (dup) Dup();
            else SetLocal(arrVar);

            int i = 0;
            foreach (var item in set)
            {
                if (dup) Dup();
                else GetLocal(arrVar);
                PushInt(i);
                getItem(item, i);
                SetArrayElem(false);
                ++i;
            }

            if (i != elemNum)
                throw new InvalidOperationException();

            if (!dup)
                GetLocal(arrVar);
        }

        public void NewArray<T>(int arrVar, IType elemType, IList<T> list, Action<T,int> getItem)
        {
            NewArray(arrVar, elemType, list.Count, list, getItem);
        }

        public void NewArray<T>(int arrVar, IType elemType, ISimpleList<T> list, Action<T, int> getItem)
        {
            NewArray(arrVar, elemType, list.Count, list, getItem);
        }

        public void NewArray<T>(int arrVar, IType elemType, T[] arr, Action<T,int> getItem)
        {
            NewArray(arrVar, elemType, arr.Length, arr, getItem);
        }

        public void NewArray<T>(int arrVar, IType elemType, int elemNum, IEnumerable<T> set, Action<T> getItem)
        {
            NewArray(arrVar, elemType, elemNum, set, (item, index) => getItem(item));
        }

        public void NewArray<T>(int arrVar, IType elemType, IList<T> list, Action<T> getItem)
        {
            NewArray(arrVar, elemType, list, (item, index) => getItem(item));
        }

        public void NewArray<T>(int arrVar, IType elemType, ISimpleList<T> list, Action<T> getItem)
        {
            NewArray(arrVar, elemType, list, (item, index) => getItem(item));
        }

        public void NewArray<T>(int arrVar, IType elemType, T[] arr, Action<T> getItem)
        {
            NewArray(arrVar, elemType, arr, (item, index) => getItem(item));
        }
        #endregion

        #region NewNativeArray
        public void NewNativeArray<T>(int arrVar, int elemNum, IEnumerable<T> set, Action<T, int> getItem)
        {
            if (getItem == null)
                throw new ArgumentNullException("getItem");

            bool dup = arrVar < 1;
            CreateArray(elemNum);

            if (dup) Dup();
            else SetLocal(arrVar);

            int i = 0;
            foreach (var item in set)
            {
                if (dup) Dup();
                else GetLocal(arrVar);
                PushInt(i);
                getItem(item, i);
                SetNativeArrayItem();
                ++i;
            }

            if (i != elemNum)
                throw new InvalidOperationException();

            if (!dup)
                GetLocal(arrVar);
        }

        public void NewNativeArray<T>(int arrVar, IList<T> list, Action<T, int> getItem)
        {
            NewNativeArray(arrVar, list.Count, list, getItem);
        }

        public void NewNativeArray<T>(int arrVar, T[] arr, Action<T, int> getItem)
        {
            NewNativeArray(arrVar, arr.Length, arr, getItem);
        }

        public void NewNativeArray<T>(int arrVar, int elemNum, IEnumerable<T> set, Action<T> getItem)
        {
            NewNativeArray(arrVar, elemNum, set, (item, index) => getItem(item));
        }

        public void NewNativeArray<T>(int arrVar, IList<T> list, Action<T> getItem)
        {
            NewNativeArray(arrVar, list.Count, list, (item, index) => getItem(item));
        }

        public void NewNativeArray<T>(int arrVar, T[] arr, Action<T> getItem)
        {
            NewNativeArray(arrVar, arr.Length, arr, (item, index) => getItem(item));
        }
        #endregion

        public void SetArrayElem(bool item)
        {
            //stack [arr, index, value]
            Call(item ? ArrayMethodId.SetItem : ArrayMethodId.SetElem);
        }

        public void GetArrayElem(bool item)
        {
            //stack [arr, index]
            Call(item ? ArrayMethodId.GetItem : ArrayMethodId.GetElem);
        }

        public void GetArrayElem(IType elemType, bool item)
        {
            if (TypeHelper.IsInt64Based(elemType))
            {
                var m = Generator.DefineSystemArray_GetElemInt64(elemType, item);
                Call(m);
            }
            else
            {
                GetArrayElem(item);
                CastTo(elemType);
                //if (TryCastToSystemType(null, elemType))
                //    return;
                //Coerce(elemType, true);
            }
        }

        public void HasElemType(IType elemType, Action getArr)
        {
            getArr();
            if (elemType == SystemTypes.Char)
            {
                Call(ArrayMethodId.IsCharArray);
            }
            else
            {
                PushTypeId(elemType);
                PushBool(true);
                Call(ArrayMethodId.HasElemType);
            }
            CoerceBool();
        }

        public void HasElemType(IType elemType, int local)
        {
            HasElemType(elemType, () => GetLocal(local));
        }

        //native AVM array must be onto the stack
        public void InitArray(IType elemType, Action getArr, int varSize)
        {
            var m = Generator.DefineInitArrayMethod(elemType);
            if (m == null) return;

            Getlex(m);
            if (getArr != null)
                getArr();
            else
                Swap(); //array is onto the stack!
            GetLocal(varSize);
            Call(m);
        }
        #endregion

        #region Boxing
        /// <summary>
        /// Gets "m_value"
        /// </summary>
        public Instruction GetBoxedValue()
        {
            return GetProperty(Const.Boxing.Value);
        }

        public void GetBoxedValue(int value)
        {
            GetLocal(value);
            GetBoxedValue();
        }

        public void SetBoxedValue()
        {
            SetProperty(Const.Boxing.Value);
        }

        public void Box(AbcInstance instance, Action getValue)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (getValue == null)
                throw new ArgumentNullException("getValue");

            CreateInstance(instance);
            Dup();
            getValue();
            SetProperty(Const.Boxing.Value);
        }

        public void BoxVariable(AbcInstance instance, int var)
        {
            Box(instance, () => GetLocal(var));
        }

        /// <summary>
        /// Performs boxing for given local register.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="var">local register that stores value to box.</param>
        public void BoxVariable(IType type, int var)
        {
            Box(type, () => GetLocal(var));
        }

        public void BoxPrimitive(IType type)
        {
            //TODO: Check primitive type.
            //NOTE: value to box must be on stack
            var m = Generator.DefineBoxMethod(type);
            if (m == null)
                throw new InvalidOperationException();
            GetlexSwapCall(m);
        }

        /// <summary>
        /// Performs boxing operation for evaluated value using getValue delegate
        /// </summary>
        /// <param name="type"></param>
        /// <param name="getValue">must push value onto the stack.</param>
        /// <remarks>
        /// NOTE: In generic context boxing operation can be applied for reference types also.
        /// </remarks>
        public void Box(IType type, Action getValue)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            
            var m = Generator.DefineBoxMethod(type);
            if (m != null)
            {
                Getlex(m);
                if (getValue != null) getValue();
                else Swap();
                Call(m);
                return;
            }

            if (getValue != null)
                getValue();

            CastTo(type);

            if (TypeHelper.IsValueType(type))
                CopyValue(type);
        }

        /// <summary>
        /// Performs boxing operation for value onto the evaluation stack.
        /// </summary>
        /// <param name="type"></param>
        public void Box(IType type)
        {
            Box(type, null);
        }

        /// <summary>
        /// Performs unbox operation for given type.
        /// </summary>
        /// <param name="type"></param>
        public void Unbox(IType type)
        {
            EnsureType(type);

            var m = Generator.SelectUnboxMethod(type, false);
            if (m != null)
            {
                GetlexSwapCall(m);
                return;
            }

            CastTo(type);
        }
        #endregion

        #region Exception Handling
        /// <summary>
        /// Throws exception onto the stack.
        /// </summary>
        public Instruction Throw()
        {
            return Add(InstructionCode.Throw);
        }

        /// <summary>
        /// Throws exception of given type.
        /// </summary>
        /// <param name="type"></param>
        public void ThrowException(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var instance = Generator.DefineAbcInstance(type);
            CreateInstance(instance);
            Throw();
        }

        public void ThrowException(CorlibTypeId exceptionType)
        {
            ThrowException(Generator.CorlibTypes[exceptionType]);
        }

        void GetStackTrace()
        {
            var m = Generator.GetMethod(EnvironmentMethodId.StackTrace);
            Getlex(m);
            Call(m);
        }

        /// <summary>
        /// Throws exception of given type with specified message
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void ThrowException(IType type, string message)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var instance = Generator.DefineAbcInstance(type);
            CreateInstance(instance);
            Dup();
            GetStackTrace();
            SetProperty("stack_trace");
            if (!string.IsNullOrEmpty(message))
            {
                Dup();
                PushString(message);
                SetProperty("message");
            }
            Throw();
        }

        public AbcTryBlockList TryBlocks
        {
            get { return _tryBlocks; }
        }
        readonly AbcTryBlockList _tryBlocks = new AbcTryBlockList();
        readonly Stack<AbcTryBlock> _tryStack = new Stack<AbcTryBlock>();

        /// <summary>
        /// Begins try block
        /// </summary>
        public void Try()
        {
            var tb = new AbcTryBlock();
            tb.From = tb.To = Count;
            _tryBlocks.Add(tb);
            _tryStack.Push(tb);
        }

        /// <summary>
        /// Begins SEH block
        /// </summary>
        /// <param name="exceptionType">Type of exception to catch. Can be null.</param>
        /// <param name="useSkipHandlers"></param>
        public void BeginCatch(object exceptionType, bool useSkipHandlers)
        {
            if (exceptionType == null)
                exceptionType = abc[AvmTypeCode.Object];

            var type = abc.DefineTypeName(exceptionType);

            var tb = _tryStack.Peek();

            if (tb.Handlers.Count == 0)
            {
                if (useSkipHandlers)
                    tb.SkipHandlers = Goto();
                tb.To = Count - 1;
            }

            var h = new AbcExceptionHandler
                        {
                            Type = type,
                            From = tb.From,
                            To = tb.To,
                            Target = tb.To + 1
                        };
            
            tb.CurrentHandler = h;
            tb.Handlers.Add(h);
        }

        public void BeginCatch()
        {
            BeginCatch(null, false);
        }

        /// <summary>
        /// Ends SEH block.
        /// </summary>
        /// <param name="last">true if block is last within protected region.</param>
        public void EndCatch(bool last)
        {
            var tb = last ? _tryStack.Pop() : _tryStack.Peek();

            if (tb.CurrentHandler == null)
                throw new InvalidOperationException("Current try block has no current handler");

            tb.CurrentHandler = null;

            if (last && tb.SkipHandlers != null)
            {
                tb.SkipHandlers.BranchTarget = Last;
                tb.SkipHandlers.BranchShift = 1;
            }
        }

        public void CatchReturnNull()
        {
            BeginCatch();
            Pop();
            ReturnNull();
            EndCatch(true);
        }
        #endregion

        #region Utils
        public void CloneValue(IType type, IMethod curmethod)
        {
            var clone = MethodHelper.Find(type, "Clone", 0);
            if (clone == curmethod) return;
            var m = Generator.DefineAbcMethod(clone);
            Call(m);
        }

        public void CallGetType()
        {
            Call(ObjectMethodId.GetType);
        }

        public void ThrowIfNull(IType exceptionType)
        {
            IsNull();
            var notNull = IfFalse();
            ThrowException(exceptionType);
            notNull.BranchTarget = Label();

            //var br = IfNotNull();
            //ThrowException(exceptionType);
            //br.BranchTarget = Label();
        }

        /// <summary>
        /// Throws NullReferenceException if top value onto the eval stack is null
        /// </summary>
        public void ThrowNullReferenceException()
        {
            ThrowIfNull(Corlib.Types.NullReferenceException);
        }
        
        public void ThrowInvalidCastException()
        {
            ThrowException(Corlib.Types.InvalidCastException);
        }

        public void ThrowNotSupportedException()
        {
            ThrowException(Corlib.Types.NotSupportedException);
        }

        public void ThrowInvalidCastException(string message)
        {
            ThrowException(Corlib.Types.InvalidCastException, message);
        }

        public void ThrowInvalidCastException(IType type)
        {
            ThrowInvalidCastException(string.Format("Unable to cast to type '{0}'", type.FullName));
        }

        /// <summary>
        /// If type throws specified exception
        /// </summary>
        /// <param name="type"></param>
        /// <param name="native"></param>
        /// <param name="exceptionType"></param>
        /// <param name="dup"></param>
        public void ThrowIfType(IType type, bool native, IType exceptionType, bool dup)
        {
            if (dup) Dup();

            var br = IfNotType(type, native);

            ThrowException(exceptionType);

            br.BranchTarget = Label();
        }

        /// <summary>
        /// If type throws specified exception
        /// </summary>
        /// <param name="type"></param>
        /// <param name="native"></param>
        /// <param name="dup"></param>
        public void ThrowInvalidCastExcpetionIfType(IType type, bool native, bool dup)
        {
            ThrowIfType(type, native, Corlib.Types.InvalidCastException, dup);
        }

        /// <summary>
        /// If type throws specified exception
        /// </summary>
        /// <param name="type"></param>
        /// <param name="native"></param>
        /// <param name="exceptionType"></param>
        /// <param name="dup"></param>
        public void ThrowIfNotType(IType type, bool native, IType exceptionType, bool dup)
        {
            if (dup) Dup();

            var br = IfType(type, native);

            ThrowException(exceptionType);

            br.BranchTarget = Label();
        }

        /// <summary>
        /// If type throws specified exception
        /// </summary>
        /// <param name="type"></param>
        /// <param name="native"></param>
        /// <param name="dup"></param>
        public void ThrowInvalidCastExcpetionIfNotType(IType type, bool native, bool dup)
        {
            ThrowIfNotType(type, native, Corlib.Types.InvalidCastException, dup);
        }

        public void GetStaticFunction(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            Getlex(method);
            GetProperty(method.TraitName);
            CoerceFunction();
        }

        //ns, name
        public void FixRuntimeQName()
        {
            Swap(); //name, ns
            Getlex(AvmTypeCode.Namespace);
            Swap();
            Construct(1); //now we have real namespace
            Swap();
        }
        #endregion

        #region Reflection
        public void PushTypeId(IType type)
        {
            if (type == null)
                throw new ArgumentNullException();
            int id = Generator.GetTypeId(type);
            PushInt(id);
        }

        public void TypeOf(IType type)
        {
            var instance = Generator.DefineType(type) as AbcInstance;
            if (instance == null)
            {
                PushNull();
                return;
            }

            CallAssemblyGetType(() => PushTypeId(type));
        }

        public void CallAssemblyGetType(Action getTypeId)
        {
            var m = Generator.GetMethod(AssemblyMethodId.GetTypeById);

            Getlex(m);

            getTypeId();

            //NOTE: We can not simply use Call because Parameters are defined later and GetType is also used TypeOf
            Call(m.TraitName, 1);
        }
        #endregion

        #region Initialization
        #region InitObject
        public void InitObject(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsEnum)
                type = type.ValueType;

            var st = type.SystemType;
            if (st != null)
            {
                switch (st.Code)
                {
                        case SystemTypeCode.Boolean:
                            PushBool(false);
                            return;

                        case SystemTypeCode.Char:
                            LoadConstant((char)0);
                            return;

                        case SystemTypeCode.Int8:
                            LoadConstant((sbyte)0);
                            return;

                        case SystemTypeCode.UInt8:
                            LoadConstant((byte)0);
                            return;

                        case SystemTypeCode.Int16:
                            LoadConstant((short)0);
                            return;

                        case SystemTypeCode.UInt16:
                            LoadConstant((ushort)0);
                            return;

                        case SystemTypeCode.Int32:
                            PushInt(0);
                            return;

                        case SystemTypeCode.UInt32:
                            PushUInt(0);
                            return;

                        case SystemTypeCode.Double:
                            PushDouble(0);
                            return;

                        case SystemTypeCode.Single:
                            PushFloat(0);
                            return;
                }
            }
            if (TypeHelper.IsInitRequired(type))
            {
                CreateInstance(type, true);
            }
            else
            {
                //SPEC: If typeTok is a reference type, the initobj instruction has the same effect as
                //ldnull followed by stind.ref.
                PushNull();
                Coerce(type, true);
            }
        }
        #endregion

        #region Class Properties
        public void InitClassProperties(AbcScript script)
        {
            foreach (var trait in script.Traits)
            {
                var klass = trait.Class;
                if (klass != null)
                {
                    var instance = klass.Instance;
                    InitClassProperty(script, instance);
                }
            }
        }

        public void InitClassProperty(AbcScript script, AbcInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException();
            if (instance.IsNative) return;
            if (instance.IsInterface) return;

            var klass = instance.Class;
            if (klass.Initialized) return;

            var superType = instance.SuperType;
            if (superType != null && superType.Script == script)
                InitClassProperty(script, instance.SuperType);

            var name = instance.Name;

            //FindPropertyStrict(name);
            //GetProperty(name);
            //var notNull = IfNotNull();

            //target for initproperty
            FindPropertyStrict(name);
            
            var scope = GetClassInitScope(instance);
            int n = scope.Length;
            for (int i = 0; i < n; ++i)
            {
                Getlex(scope[i]);
                PushScope();
            }

            //Push base type on the stack
            Getlex(instance.SuperName);
            Add(InstructionCode.Newclass, klass);

            for (int i = 0; i < n; ++i)
                PopScope();

            InitProperty(name);

            //notNull.BranchTarget = Label();

            klass.Initialized = true;
        }

        AbcMultiname[] GetClassInitScope(AbcInstance instance)
        {
            var list = new List<AbcMultiname>();

            var super = instance.SuperType;
            while (super != null)
            {
                if (super.IsObject) break;
                list.Insert(0, super.Name);
                if (super.IsError) break;
                super = super.SuperType;
            }

            //TODO: check type hierarchy

            list.Insert(0, abc.BuiltinTypes.Object);

            return list.ToArray();
        }
        #endregion

        #region InitFields
        public void InitFields(IMethod method)
        {
            if (!method.IsConstructor) return;
            var type = method.DeclaringType;
            if (!TypeHelper.MustInitValueTypeFields(type)) return;
            bool isStatic = method.IsStatic;
            InitFields(type, isStatic, false);
        }

        public void InitFields(IType type, bool isStatic, bool dup)
        {
            if (!TypeHelper.MustInitValueTypeFields(type)) return;
            foreach (var f in type.Fields)
            {
                if (f.IsConstant) continue;
                if (f.IsStatic == isStatic)
                {
                    if (!TypeHelper.IsInitRequiredField(f.Type)) continue;
                    if (dup) Dup();
                    else LoadThis();
                    InitObject(f.Type);
                    SetField(f);
                }
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Gets enum value (unwraps System.Enum object).
        /// Return value is boxed number or a copy of Int64/UInt64 object.
        /// </summary>
        public void GetEnumValue()
        {
            var name = abc.DefineGlobalQName(Const.Enum.GetValue);
            Call(name, 0);
        }

        #region Arithmetic
        /// <summary>
        /// Performs binary operation.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="result"></param>
        /// <param name="checkOverflow"></param>
        public void Op(BinaryOperator op, IType left, IType right, IType result, bool checkOverflow)
        {
            if (checkOverflow && OpOvf(op, left, right, result))
                return;

            if (OpHelper.IsRelation(op))
            {
                bool swap = false;
                if ((left != null && right == null)
                    || (swap = (left == null && right != null)))
                {
                    IsNull(left, right, op != BinaryOperator.Equality, true, swap, false);
                    return;
                }
            }

            if (TypeHelper.IsDecimalOrInt64(left, right))
            {
                CallBinOp(op, left, right, result);
                return;
            }

            switch (op)
            {
                #region Arithmetic Operations
                case BinaryOperator.Addition:
                    Add(result);
                    break;

                case BinaryOperator.Subtraction:
                    Sub(result);
                    break;

                case BinaryOperator.Multiply:
                    Mul(result);
                    break;

                case BinaryOperator.Division:
                    Add(InstructionCode.Divide);
                    Coerce(result, true);
                    break;

                case BinaryOperator.Modulus:
                    Add(InstructionCode.Modulo);
                    Coerce(result, true);
                    break;
                #endregion

                #region Bitwise Operations
                case BinaryOperator.LeftShift:
                    BitOp(result, InstructionCode.Lshift);
                    break;

                case BinaryOperator.RightShift:
            		BitOp(result, SystemTypes.IsUnsigned(result) ? InstructionCode.Urshift : InstructionCode.Rshift);
            		break;

                case BinaryOperator.BitwiseOr:
                    BitOp(result, InstructionCode.Bitor);
                    break;

                case BinaryOperator.BitwiseAnd:
                    BitOp(result, InstructionCode.Bitand);
                    break;

                case BinaryOperator.ExclusiveOr:
                    BitOp(result, InstructionCode.Bitxor);
                    break;
                #endregion

                #region Relation Operations
                case BinaryOperator.Equality:
                    Add(InstructionCode.Equals);
                    FixBool();
                    break;

                case BinaryOperator.Inequality:
                    Add(InstructionCode.Equals);
                    Add(InstructionCode.Not);
                    FixBool();
                    break;

                case BinaryOperator.LessThan:
                    Add(InstructionCode.Lessthan);
                    FixBool();
                    break;

                case BinaryOperator.LessThanOrEqual:
                    Add(InstructionCode.Lessequals);
                    FixBool();
                    break;

                case BinaryOperator.GreaterThan:
                    Add(InstructionCode.Greaterthan);
                    FixBool();
                    break;

                case BinaryOperator.GreaterThanOrEqual:
                    Add(InstructionCode.Greaterequals);
                    FixBool();
                    break;
                #endregion

                default:
                    throw new NotSupportedException();
            }
        }

        public void Nullable_HasValue(bool nativeBool)
        {
            GetProperty(Const.Nullable.HasValue);
            if (nativeBool)
                Coerce(AvmTypeCode.Boolean);
        }

        void IsNull(IType left, IType right, bool notEquals, bool pop, bool swap, bool nativeBool)
        {
            if (swap) Swap(); //usually when right is not null
            if (pop) Pop(); //pop right operand

            var type = left ?? right;
            bool call = AbcGenConfig.UseIsNull;
            if (AbcGenConfig.UseIsNull)
            {
                if (type != null && type != SystemTypes.Object)
                {
                    PushNull();
                    Add(InstructionCode.Equals);
                    call = false;
                }

                if (call)
                {
                    var m = Generator.DefineIsNullMethod();
                    GetlexSwapCall(m);
                }
            }
            else
            {
                if (type != null && TypeService.IsNullableInstance(type))
                {
                    //Nullable_HasValue(true);
                    var m = Generator.DefineIsNullableMethod();
                    GetlexSwapCall(m);
                }
                else
                {
                    PushNull();
                    Add(InstructionCode.Equals);
                }
            }

            if (notEquals)
                Add(InstructionCode.Not);
            if (!nativeBool)
                FixBool();
        }

        public void IsNull(IType left, IType right)
        {
            IsNull(left, right, false, false, false, true);
        }

        public void IsNull()
        {
            IsNull(null, null, false, false, false, true);
        }

        void CallBinOp(BinaryOperator op, IType left, IType right, IType result)
        {
            var stOp = Generator.FindOperator(op, left, right);
            if (stOp == null)
            {
                Cast(right, left);
                right = left;
            }
            var m = Generator.DefineOperator(op, left, right);
            Call(m);
            Coerce(result, true);
        }

        bool OpOvf(BinaryOperator op, IType left, IType right, IType result)
        {
            switch (op)
            {
                case BinaryOperator.Addition:
                    AddOvf(left, right, result);
                    return true;

                case BinaryOperator.Subtraction:
                    SubOvf(left, right, result);
                    return true;

                case BinaryOperator.Multiply:
                    MulOvf(left, right, result);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Performs unary operation.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="type"></param>
        /// <param name="checkOverflow"></param>
        public void Op(UnaryOperator op, IType type, bool checkOverflow)
        {
            if (TypeHelper.IsDecimalOrInt64(type))
            {
                var opm = Generator.DefineUnaryOperator(op, type);
                Call(opm);
                //Coerce(type, true);
                return;
            }

            switch (op)
            {
                case UnaryOperator.BooleanNot:
                    Add(InstructionCode.Not);
                    FixBool();
                    break;

                case UnaryOperator.BitwiseNot:
                    Add(InstructionCode.Bitnot);
                    if (NeedCoerce(type))
                        Coerce(type, true);
                    break;

                case UnaryOperator.Negate:
                    if (NeedCoerce(type))
                    {
                        Add(InstructionCode.Negate);
                        Coerce(type, true);
                    }
                    else
                    {
                        Add(InstructionCode.Negate_i);
                    }
                    break;

                case UnaryOperator.PreIncrement:
                case UnaryOperator.PostIncrement:
                    if (NeedCoerce(type))
                    {
                        Add(InstructionCode.Increment);
                        Coerce(type, true);
                    }
                    else
                    {
                        Add(InstructionCode.Increment_i);
                    }
                    break;

                case UnaryOperator.PreDecrement:
                case UnaryOperator.PostDecrement:
                    if (NeedCoerce(type))
                    {
                        Add(InstructionCode.Decrement);
                        Coerce(type, true);
                    }
                    else
                    {
                        Add(InstructionCode.Decrement_i);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        public void IntOp(IType type, InstructionCode c, InstructionCode ci)
        {
            if (SystemTypes.IsSigned(type))
            {
                AvmConfig.CheckInt64(type);
                Add(ci);
            }
            else
            {
                Add(c);
                Coerce(type, true);
            }
        }

        static bool NeedCoerce(IType type)
        {
            var nt = TypeHelper.GetNumberType(type);
            switch (nt)
            {
                case NumberType.Int32:
                case NumberType.Int64:
                    return false;
            }
            return true;
        }

        void BitOp(IType type, InstructionCode c)
        {
            Add(c);
            if (NeedCoerce(type))
                Coerce(type, true);
        }

        public void Add(IType type)
        {
            IntOp(type, InstructionCode.Add, InstructionCode.Add_i);
        }

        //stack transition: ..., left, right -> ..., result
        public void AddOvf(IType left, IType right, IType result)
        {
            if (SystemTypes.IsIntegral(result))
            {
                CallLeftOpTempOvf(result, "AddOvf");
            }
            else
            {
                Add(result);
            }
        }

        public void Sub(IType type)
        {
            IntOp(type, InstructionCode.Subtract, InstructionCode.Subtract_i);
        }

        public void SubOvf(IType left, IType right, IType result)
        {
            if (SystemTypes.IsSigned(result))
            {
                CallLeftOpTempOvf(result, "SubOvf");
            }
            else
            {
                Sub(result);
            }
        }

        public void Mul(IType type)
        {
            IntOp(type, InstructionCode.Multiply, InstructionCode.Multiply_i);
        }

        public void MulOvf(IType left, IType right, IType result)
        {
            if (SystemTypes.IsIntegral(result))
            {
                CallLeftOpTempOvf(result, "MulOvf");
            }
            else
            {
                Mul(result);
            }
        }

        #region Utils
        void CallLeftOpTempOvf(IType type, string methodName)
        {
            var m = DefineLeftOpTempOvf(type, methodName);
            SaveTempValue(type);
            GetlexSwapCall(m);
        }

        AbcTrait DefineTempValue(IType type)
        {
            var instance = DefineAbcInstance(type);
            var typeName = abc.GetTypeName(type, true);
            return instance.DefineStaticSlot("~temp", typeName);
        }

        void SaveTempValue(IType type)
        {
            //NOTE: value should be onto the stack
            var temp = DefineTempValue(type);
            Getlex(temp.Instance);
            Swap();
            SetProperty(temp);
        }

        void LoadTempValue(IType type, bool useThis)
        {
            var temp = DefineTempValue(type);
            if (useThis) LoadThis();
            else Getlex(temp.Instance);
            GetProperty(temp);
        }

        /// <summary>
        /// Defines binary operation with one parameter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        AbcMethod DefineLeftOpTempOvf(IType type, string methodName)
        {
            var instance = DefineAbcInstance(type);
            var retType = abc.GetTypeName(type, true);

            var ovf2 = DefineAbcMethod(type, methodName, 2);

            string ovf1Name = "$" + methodName + "1$";

            return instance.DefineStaticMethod(
                ovf1Name, retType,
                delegate(AbcCode code)
                    {
                        code.PushThisScope();

                        code.LoadThis();
                        code.GetLocal(1); //left

                        code.LoadTempValue(type, true); //right
                        code.Call(ovf2);

                        code.ReturnValue();
                    },
                retType, "left");
        }
        #endregion
        #endregion

        #region System.String
        public void UnwrapSystemString()
        {
            if (AbcGenConfig.UseAvmString)
                return;

            var instance = DefineAbcInstance(SystemTypes.String);
            var name = abc.DefinePfxName("unwrap");
            var m = instance.DefineStaticMethod(
                name, AvmTypeCode.String,
                delegate(AbcCode code)
                    {
                        int s = 1;
                        code.GetLocal(s);
                        var br = code.IfNotNull();

                        //null
                        code.PushNull();
                        code.CoerceString();
                        code.ReturnValue();

                        br.BranchTarget = code.Label();

                        code.GetLocal(s);
                        code.GetProperty(Const.String.Value);
                        code.ReturnValue();
                    },
                    instance.Name, "s");

            Getlex(instance);
            Swap();
            Call(m);
        }

        public void GetChar()
        {
            CallAS3("charCodeAt", 1);
            CoerceChar();
        }

        public void Substring(int argCount)
        {
            if (argCount <= 0 || argCount > 2)
                throw new ArgumentOutOfRangeException("argCount");
            CallAS3("substring", argCount);
        }

        public void Substr(int argCount)
        {
            if (argCount <= 0 || argCount > 2)
                throw new ArgumentOutOfRangeException("argCount");
            CallAS3("substr", argCount);
        }

        public void GetStringLength()
        {
            GetProperty("length");
        }
        #endregion

        #region NewObject
        /// <summary>
        /// Creates new native AVM object.
        /// </summary>
        /// <param name="n">number of properties (name/value pairs) to get from stack.</param>
        /// <returns></returns>
        public Instruction NewObject(int n)
        {
            return Add(InstructionCode.Newobject, n);
        }

        /// <summary>
        /// Creates new object using given ctor.
        /// </summary>
        /// <param name="ctor">ctor to call.</param>
        /// <param name="args"></param>
        public void NewObject(IMethod ctor, Action args)
        {
            if (ctor == null)
                throw new ArgumentNullException("ctor");
            if (!ctor.IsConstructor)
                throw new ArgumentException("ctor is not ctor");
            if (ctor.IsStatic)
                throw new ArgumentException("cannot call static ctor");

            var m = DefineAbcMethod(ctor);
            if (m.IsInitializer)
            {
                var instance = m.Instance;
                Getlex(instance);
                if (args != null)
                    args();
                Construct(ctor.Parameters.Count);
            }
            else
            {
                m = Generator.DefineCtorStaticCall(ctor);
                CallStatic(m, args);
            }
        }

        public void NewObject(IType type, System.Func<IMethod, bool> ctorPredicate, Action args)
        {
            var ctor = type.FindConstructor(ctorPredicate);
            if (ctor == null)
                throw new InvalidOperationException("Unable to find ctor");
            NewObject(ctor, args);
        }

        public void NewObject(IType type, int argc, Action args)
        {
            NewObject(type, ctor => ctor.Parameters.Count == argc, args);
        }

        public void NewNullable(IType type, int value)
        {
            if (!TypeService.IsNullableInstance(type))
                throw new ArgumentException("type is not nullable");

            var typeArg = ((IGenericInstance)type).GenericArguments[0];

            NewObject(
                type, 1,
                delegate
                    {
                        var unbox = Generator.SelectUnboxMethod(typeArg, true);
                        if (unbox != null)
                        {
                            CallStatic(unbox, () => GetLocal(value));
                        }
                        else
                        {
                            GetLocal(value);
                            Unbox(typeArg);
                        }
                    });
        }
        #endregion

        public void NewFunction(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            Add(InstructionCode.Newfunction, method);
        }

        public void NewProperty(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            Add(InstructionCode.Constructprop, method);
        }

        #region Prototype
        public void GetPrototype()
        {
            GetProperty("prototype");
        }

        public void SetProtoProperty(object type, object name, Action value, bool dontEnum)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var typeName = abc.DefineTypeNameStrict(type);
            var mn = abc.DefineName(name);

            Getlex(typeName);
            GetPrototype();

            if (dontEnum)
                Dup();

            value();
            SetProperty(mn);

            if (dontEnum)
            {
                PushString(mn.Name);
                PushNativeBool(false);
                CallVoid("setPropertyIsEnumerable", 2);
            }
        }

        public void SetProtoFunction(object type, object name, AbcMethod func, bool dontEnum)
        {
            SetProtoProperty(type, name, () => NewFunction(func), dontEnum);
        }

        public void SetProtoFunction(object type, object name, AbcMethod func)
        {
            SetProtoFunction(type, name, func, true);
        }

        public void SetProtoFunction(object type, AbcMethod sig, AbcCoder coder)
        {
            var f = abc.DefineMethod(sig, coder);
            SetProtoFunction(type, sig.TraitName, f, true); 
        }
        #endregion

        #region AVM/Flash API
        public void GetErrorStackTrace()
        {
            CallGlobal("getStackTrace", 0);
            CoerceString(); //just for sure
        }

        public void GetErrorMessage()
        {
            GetProperty("message"); //message has any (*) type
            CoerceString();
        }

        /// <summary>
        /// Loads flash.system.Capabilities onto the stack
        /// </summary>
        public void LoadCapabilities()
        {
            var mn = abc.DefinePackageQName("flash.system", "Capabilities");
            Getlex(mn);
        }

        public Instruction GetCapability(string name)
        {
            LoadCapabilities();

            var mn = abc.DefineGlobalQName(name);
            return GetProperty(mn);
        }

        /// <summary>
        /// Loads player type string onto the stack.
        /// </summary>
        public Instruction GetPlayerType()
        {
            return GetCapability("playerType");
        }

        public Instruction HasDebugger()
        {
            return GetCapability("isDebugger");
        }

        public void AvmplusSystemWrite()
        {
            var mn = abc.DefinePackageQName("avmplus", "System");
            Getlex(mn);
            mn = DefineGlobalQName("write");
            Swap(); //string
            CallVoid(mn, 1);
        }

        public void AvmplusSystemWriteLine()
        {
            AvmplusSystemWrite();
            PushString("\n");
            AvmplusSystemWrite();
        }

        public void ConsoleWriteLine(string str)
        {
            // TODO: (yurik) implement this function.

        }

        public void Trace(string msg)
        {
            var mn = DefineGlobalQName("trace");
            FindPropertyStrict(mn);
            PushString(msg);
            CallVoid(mn, 1);
        }
        #endregion

        #region Corlib API
        /// <summary>
        /// Starts redirection of System.Console output to temporary StringWriter.
        /// </summary>
        public void ConsoleOpenSW()
        {
            CallStatic(Generator.GetMethod(ConsoleMethodId.OpenSW));
        }

        /// <summary>
        /// Ends redirection of System.Console output to temporary StringWriter.
        /// </summary>
        public void ConsoleCloseSW(bool pop)
        {
            CallStatic(Generator.GetMethod(ConsoleMethodId.CloseSW));
            if (pop) Pop();
        }
        #endregion

        #region Pointers
        public Instruction NewActivation()
        {
            return Add(InstructionCode.Newactivation);
        }

        public void ReadPtr()
        {
            GetProperty(abc.PtrValueName);
        }

        public void ReadPtr(IType type)
        {
            type = TypeService.UnwrapRef(type);
            ReadPtr();
            Coerce(type, true);
        }

        public void WritePtr()
        {
            SetProperty(abc.PtrValueName);
        }

        public void CreateFuncPtr(AbcTrait trait)
        {
            if (trait == null)
                throw new ArgumentNullException("trait");
            if (trait.Kind != AbcTraitKind.Slot)
                throw new ArgumentException("trait is not slot");

            var instance = Generator.DefineFuncPtr();
            Getlex(instance);

            var m = Generator.DefineFunction_GetProperty(trait.Name);
            NewFunction(m);

            m = Generator.DefineFunction_SetProperty(trait.Name);
            NewFunction(m);

            Construct(2);
            CoerceObject();
        }

        public void CreateSlotPtr(AbcTrait ptr, int obj)
        {
            if (ptr == null)
                throw new ArgumentNullException("ptr");
            if (ptr.Kind != AbcTraitKind.Slot)
                throw new ArgumentException("trait is not slot");
            Getlex(ptr.SlotType);
            GetLocal(obj);
            Construct(1);
            CoerceObject();
        }

        public void GetFieldPtr(IField field)
        {
            GetFieldPtr(field, null);
        }

        public void GetFieldPtr(IField field, Action target)
        {
            if (field.IsStatic)
            {
                var instance = Generator.DefineStaticFieldPtr(field);
                Getlex(instance);
                GetProperty(Const.Instance);
            }
            else
            {
                var name = GetFieldName(field);
                CreatePropertyPtr(name, target);
            }
            CoerceObject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="target">if null object must be onto the eval stack</param>
        public void CreatePropertyPtr(AbcMultiname name, Action target)
        {
            if (name.Namespace.IsGlobalPackage)
            {
                var instance = Generator.DefineGlobalPropertyPtr();
                Getlex(instance);
                if (target != null) target();
                else Swap();
                PushString(name.Name);
                Construct(2);
            }
            else
            {
                var instance = Generator.DefinePropertyPtr();
                Getlex(instance);
                if (target != null) target();
                else Swap();
                PushNamespace(name.Namespace);
                PushString(name.Name);
                Construct(3);
            }
        }

        public void GetElemPtr(IType elemType)
        {
            //stack transition: arr, index -> addr
            var m = Generator.DefineGetElemPtr();
            Call(m);
        }
        #endregion
    }

    delegate void AbcCoder(AbcCode code);

    delegate void AbcCoder2(IMethod method, AbcCode code);

    delegate Instruction AbcCondition();
}