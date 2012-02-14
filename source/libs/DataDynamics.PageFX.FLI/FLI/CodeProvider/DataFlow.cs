using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AvmCodeProvider
    {
        #region DeclareVariable
        static bool IsValueType(IType type)
        {
            var st = type.SystemType;
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Int64:
                    case SystemTypeCode.UInt64:
                    case SystemTypeCode.Decimal:
                    case SystemTypeCode.DateTime:
                        return true;
                }
                return false;
            }
            if (type.TypeKind == TypeKind.Struct)
                return true;
            return false;
        }

        static readonly IInstruction[] EmptyCode = new IInstruction[0];

        public IInstruction[] DeclareVariable(IVariable v)
        {
            var type = v.Type;
            EnsureType(type);

            switch (type.TypeKind)
            {
                case TypeKind.Pointer:
                case TypeKind.Reference: //only arguments can be references
                    throw new NotSupportedException();
            }

            var slot = v.Tag as AbcTrait;
            if (slot != null)
            {
                if (IsValueType(type))
                {
                    var code = new AbcCode(_abc);
                    GetActivation(code);
                    code.InitObject(type);
                    code.SetSlot(slot);
                    return code.ToArray();
                }
                return EmptyCode;
            }

            var st = type.SystemType;
            if (st != null)
                return DeclateSystemTypeVar(st, v);

            switch (type.TypeKind)
            {
                case TypeKind.Class:
                case TypeKind.Interface:
                case TypeKind.Delegate:
                case TypeKind.Array:
                    return SetNull(type, v);

                case TypeKind.Enum:
                    {
                        var vtype = type.ValueType;
                        st = vtype.SystemType;
                        return DeclateSystemTypeVar(st, v);
                    }

                case TypeKind.Struct:
                    return SetValueType(type, v);
            }
            return null;
        }

        IInstruction[] DeclateSystemTypeVar(SystemType sysType, IVariable var)
        {
            if (sysType == null)
                throw new ArgumentNullException("sysType");
            switch (sysType.Code)
            {
                case SystemTypeCode.Void:
                    throw new InvalidOperationException();

                case SystemTypeCode.Boolean:
                    return SetFalse(var);

                case SystemTypeCode.Int8:
                case SystemTypeCode.Int16:
                case SystemTypeCode.Int32:
                    return SetIntZero(var);

                case SystemTypeCode.UInt8:
                case SystemTypeCode.UInt16:
                case SystemTypeCode.UInt32:
                case SystemTypeCode.Char:
                    return SetUIntZero(var);

                case SystemTypeCode.Single:
                case SystemTypeCode.Double:
                    return SetDoubleZero(var);

                case SystemTypeCode.Int64:
                    if (AvmConfig.SupportNativeInt64)
                    {
                        throw new NotImplementedException();
                    }
                    return SetValueType(sysType.Value, var);
                //return SetIntZero(var);

                case SystemTypeCode.UInt64:
                    if (AvmConfig.SupportNativeInt64)
                    {
                        throw new NotImplementedException();
                    }
                    return SetValueType(sysType.Value, var);

                //return SetUIntZero(var);

                case SystemTypeCode.Decimal:
                    if (AvmConfig.SupportNativeDecimal)
                    {
                        throw new NotImplementedException();
                    }
                    return SetValueType(sysType.Value, var);

                case SystemTypeCode.DateTime:
                    return SetValueType(sysType.Value, var);

                default:
                    return SetNull(sysType.Value, var);
            }
        }

        IInstruction[] SetValueType(IType type, IVariable var)
        {
            var code = new AbcCode(_abc);
            code.InitObject(type);
            if (code.Count == 0)
                throw new InvalidOperationException("Unable to InitObject");
            code.AddRange(StoreVariable(var));
            return code.ToArray();
        }

        IInstruction[] SetFalse(IVariable var)
        {
            var code = new AbcCode(_abc);
            code.PushBool(false);
            code.AddRange(StoreVariable(var));
            return code.ToArray();
        }

        IInstruction[] SetIntZero(IVariable var)
        {
            var code = new AbcCode(_abc);
            code.PushInt(0);
            code.AddRange(StoreVariable(var));
            return code.ToArray();
        }

        IInstruction[] SetUIntZero(IVariable var)
        {
            var code = new AbcCode(_abc);
            code.PushUInt(0);
            code.AddRange(StoreVariable(var));
            return code.ToArray();
        }

        IInstruction[] SetDoubleZero(IVariable var)
        {
            var code = new AbcCode(_abc);
            code.PushDouble(0);
            code.AddRange(StoreVariable(var));
            return code.ToArray();
        }

        IInstruction[] SetNull(IType type, IVariable var)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.PushNull();
            code.Coerce(type, true);
            code.AddRange(StoreVariable(var));
            return code.ToArray();
        }
        #endregion

        #region LoadConstant
        public IInstruction[] PushSystemString(string s)
        {
            var code = new AbcCode(_abc);
            code.PushSystemString(s);
            return code.ToArray();
        }

        public IInstruction[] LoadConstant(object value)
        {
            var code = new AbcCode(_abc);
            code.LoadConstant(value);
            return code.ToArray();
        }
        #endregion

        #region LoadStaticInstance
        /// <summary>
        /// Pushes onto the stack static instance of the class to call static functions
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IInstruction[] LoadStaticInstance(IType type)
        {
            var code = new AbcCode(_abc);
            LoadStaticInstance(code, type);
            return code.ToArray();
        }

        bool UseThisForStaticReceiver(IType type)
        {
            return _method.IsStatic && _method.DeclaringType == type;
        }

        void LoadStaticInstance(AbcCode code, IType type)
        {
            EnsureType(type);
            var instance = type.Tag as AbcInstance;
            if (instance != null)
            {
                if (instance.IsNative)
                {
                    code.Getlex(instance.Name);
                    return;
                }
                if (UseThisForStaticReceiver(type))
                    code.LoadThis();
                else
                    code.Getlex(instance.Name);
                return;
            }
            throw new InvalidOperationException();
        }
        #endregion

        IInstruction[] LoadLocal(int index)
        {
            var code = new AbcCode(_abc);
            code.GetLocal(index);
            return code.ToArray();
        }

        #region LoadArgument, StoreArgument
        int GetArgIndex(int index)
        {
            return HasPseudoThis ? index + 2 : index + 1;
        }

        int GetArgIndex(IParameter p)
        {
            int index = p.Index - 1;
            Debug.Assert(index >= 0);
            return GetArgIndex(index);
        }

        public IInstruction[] LoadArgument(IParameter p)
        {
            if (p == null)
                throw new ArgumentNullException("p");
            var ptr = p.Tag as VarPtr;
            if (ptr != null)
                return GetSlot(ptr.Slot);
            return LoadLocal(GetArgIndex(p));
        }

        public IInstruction[] StoreArgument(IParameter p)
        {
            if (p == null)
                throw new ArgumentNullException("p");

            if (p.IsByRef)
            {
                throw new InvalidOperationException();
            }

            var ptr = p.Tag as VarPtr;
            if (ptr != null)
                return SetSlot(ptr.Slot);

            return new[]
                       {
                           (IInstruction)Instruction.SetLocal(GetArgIndex(p))
                       };
        }
        #endregion

        #region LoadThis, LoadBase
        int GetThisIndex()
        {
            return HasPseudoThis ? 1 : 0;
        }

        public IInstruction[] StoreThis()
        {
            if (_thisPtr != null)
            {
                return SetSlot(_thisPtr.Slot);
            }
            return new[] { (IInstruction)Instruction.SetLocal(GetThisIndex()) };
        }

        public IInstruction[] LoadThis()
        {
            if (_thisPtr != null)
            {
                return GetSlot(_thisPtr.Slot);
            }
            return LoadLocal(GetThisIndex());
        }

        public IInstruction[] LoadBase()
        {
            if (_thisPtr != null)
            {
                return GetSlot(_thisPtr.Slot);
            }
            return LoadLocal(GetThisIndex());
        }
        #endregion

        #region LoadVariable, StoreVariable
        int VarOrigin
        {
            get
            {
                int n = _method.Parameters.Count;
                if (HasActivationVar) ++n;
                return HasPseudoThis ? n + 2 : n + 1;
            }
        }

        public int GetVarIndex(int index, bool tobackend)
        {
            return tobackend ? index + VarOrigin : index - VarOrigin;
        }

        public int GetVarIndex(int index)
        {
            return GetVarIndex(index, true);
        }

        public IInstruction[] LoadVariable(IVariable v)
        {
            var ptr = v.Tag as VarPtr;
            if (ptr != null)
                return GetSlot(ptr.Slot);

            return LoadLocal(GetVarIndex(v.Index));
        }

        public IInstruction[] StoreVariable(IVariable v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            var ptr = v.Tag as VarPtr;
            if (ptr != null)
                return SetSlot(ptr.Slot);

            return new IInstruction[]
                       {
                           Instruction.SetLocal(GetVarIndex(v.Index))
                       };
        }
        #endregion

        #region LoadField, StoreField
        void EnsureField(IField field)
        {
            _generator.DefineField(field);
        }

        AbcMultiname GetFieldName(IField field)
        {
            EnsureField(field);
            var t = field.Tag as AbcTrait;
            AbcMultiname name;
            if (t != null)
            {
                name = t.Name;
            }
            else
            {
                name = field.Tag as AbcMultiname;
                if (name == null)
                    throw new ArgumentException("Invalid field tag");
            }
            name = _abc.ImportConst(name);
            //AbcNamespaceSet nss = GetLookupNamespaceSet(field.DeclaringType);
            //name = DefineLookupName(field.DeclaringType, name.Name);
            return name;
        }

        public bool LoadStaticFieldTarget = true;

        public IInstruction[] LoadField(IField field)
        {
            var prop = GetFieldName(field);
            var code = new AbcCode(_abc);
            CallStaticCtor(code, field);
            if (LoadStaticFieldTarget && field.IsStatic)
                LoadStaticInstance(code, field.DeclaringType);
            code.GetProperty(prop);
            return code.ToArray();
        }

        public IInstruction[] StoreField(IField field)
        {
            var code = new AbcCode(_abc);
            if (AvmHelper.HasEmbeddedAsset(field))
            {
                _generator.CheckEmbedAsset(field);
                code.Pop();
            }
            else
            {
                var prop = GetFieldName(field);
                CallStaticCtor(code, field);
                if (field.IsStatic)
                {
                    LoadStaticInstance(code, field.DeclaringType);
                    code.Swap();
                }
                code.SetProperty(prop);
            }
            return code.ToArray();
        }
        #endregion

        #region Temp Variables
        public bool SupportStaticTarget
        {
            get { return true; }
        }

        /// <summary>
        /// Saves value onto the stack in temporary variable (stack must not be changed)
        /// </summary>
        /// <param name="var"></param>
        /// <param name="keepStackState">true to keep stack state</param>
        /// <returns></returns>
        public IInstruction[] SetTempVar(out int var, bool keepStackState)
        {
            var code = new AbcCode(_abc);
            var = SetTempVar(code, keepStackState);
            return code.ToArray();
        }

        int SetTempVar(AbcCode code, bool keepStackState)
        {
            //NOTE: we duplicate value onto the stack to keep stack unchanged after this operation
            if (keepStackState)
                code.Add(InstructionCode.Dup);
            int var = NewTempVar(false);
            code.SetLocal(var);
            return var;
        }

        /// <summary>
        /// Loads value from temporary variable onto the stack
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public IInstruction[] GetTempVar(int var)
        {
            if (var >= 0)
            {
                var code = new AbcCode(_abc);
                code.GetLocal(var);
                return code.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Free temporary variable
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public IInstruction[] KillTempVar(int var)
        {
            if (var >= 0)
            {
                var code = new AbcCode(_abc);
                KillTempVar(code, var);
                return code.ToArray();
            }
            return null;
        }

        void KillTempVar(AbcCode code, int var)
        {
            var instr = KillTempVarCore(var);
            if (instr != null)
                code.Add(instr);
        }

        //true - var is free, false - var is busy
        readonly List<bool> _tempVars = new List<bool>();

        int TempVarsOrigin
        {
            get { return _body.LocalCount - _tempVars.Count; }
        }

        int NewTempVar(bool newreg)
        {
            int n = _tempVars.Count;
            if (!newreg && n > 0)
            {
                //try to find free local register
                for (int i = 0; i < n; ++i)
                {
                    //is local register is free?
                    if (_tempVars[i])
                    {
                        _tempVars[i] = false;
                        return TempVarsOrigin + i;
                    }
                }
            }
            n = _body.LocalCount;
            ++_body.LocalCount;
            _tempVars.Add(false);
            return n;
        }

        IInstruction KillTempVarCore(int index)
        {
            int i = index - TempVarsOrigin;
            if (i >= 0)
            {
                _tempVars[i] = true;
                return new Instruction(InstructionCode.Kill, index);
            }
            return null;
        }
        #endregion
    }
}