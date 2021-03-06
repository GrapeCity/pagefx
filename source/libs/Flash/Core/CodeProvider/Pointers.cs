using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeProvider
{
    //contains implementation of pointer operations
    partial class CodeProviderImpl
    {
	    public bool IsThisAddressed
        {
            get { return _isThisAddressed; }
            set 
            { 
                _isThisAddressed = value; 
                if (value && _method.IsStatic)
                    throw new InvalidOperationException("Static method cannot have this pointer");
            }
        }
		private bool _isThisAddressed;

	    /// <summary>
	    /// Provides code to load address of this argument.
	    /// </summary>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> GetThisPtr()
        {
            var ptr = DefineThisPtr();
            return GetSlot(ptr.Ptr);
        }

	    /// <summary>
	    /// Provides code to load address of given variable onto the evaluation stack.
	    /// </summary>
	    /// <param name="v">local variable to load address.</param>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> GetVarPtr(IVariable v)
        {
            var ptr = v.Data as VarPtr;
            if (ptr == null)
                throw new InvalidOperationException();
            return GetSlot(ptr.Ptr);
        }

	    /// <summary>
	    /// Provides code to load address of given argument onto the evaluation stack.
	    /// </summary>
	    /// <param name="p">given argument</param>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> GetArgPtr(IParameter p)
        {
            var ptr = p.Data as VarPtr;
            if (ptr == null)
                throw new InvalidOperationException();
            return GetSlot(ptr.Ptr);
        }

	    /// <summary>
	    /// Provides code to load address of given field onto the evaluation stack.
	    /// </summary>
	    /// <param name="field">field to load address.</param>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> GetFieldPtr(IField field)
        {
            var code = new AbcCode(_abc);
            code.GetFieldPtr(field);
            _body.Flags |= AbcBodyFlags.HasFieldPointers;
            return code;
        }

	    /// <summary>
	    /// Provides code to load address of an element of an array onto the evaluation stack.
	    /// </summary>
	    /// <param name="elemType">type of an array element to load address.</param>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> GetElemPtr(IType elemType)
        {
            var code = new AbcCode(_abc);
            code.GetElemPtr(elemType);
            _body.Flags |= AbcBodyFlags.HasElemPointers;
            return code;
        }

	    /// <summary>
	    /// Provides code to load value indirect onto the stack.
	    /// </summary>
	    /// <param name="valueType">type of value to load.</param>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> LoadIndirect(IType valueType)
        {
            //stack transition: ..., addr -> ..., value
            var code = new AbcCode(_abc);
            if (valueType == null)
                code.ReadPtr();
            else
                code.ReadPtr(valueType);
            return code;
        }

	    /// <summary>
	    /// Provides code to store value indirect from stack.
	    /// </summary>
	    /// <param name="valueType">type of value to store by address onto the stack.</param>
	    /// <returns>Array of instructions (code) to perform the operation.</returns>
	    public IEnumerable<IInstruction> StoreIndirect(IType valueType)
        {
            //stack transition: ..., addr, value -> ...
            var code = new AbcCode(_abc);
            code.WritePtr();
            return code;
        }

	    #region InitActivation
		private AbcInstance _activation;
		private int _activationVar = -1;

		private bool HasActivationVar
        {
            get { return _activationVar >= 0; }
        }

		private int MethodIndex
        {
            get { return _body.Method.Index; }
        }

		private void InitActivation()
        {
            if (IsThisAddressed)
            {
                InitActivationVar();
                DefineThisPtr();
            }

            int n = _method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = _method.Parameters[i];
                if (p.IsAddressed)
                {
                    InitActivationVar();
                    DefineArgPtr(p);
                }
            }

            if (HasLocalVariables)
            {
                n = VarCount;
                for (int i = 0; i < n; ++i)
                {
                    var v = GetVar(i);
                    if (v.IsAddressed)
                    {
                        InitActivationVar();
                        DefineVarPtr(v);
                    }
                }
            }
        }

		private void InitActivationVar()
        {
            if (_activationVar >= 0) return;

            if (!AbcGenConfig.UseActivationTraits)
            {
                var name = _abc.DefineName(QName.PfxPublic("Activation" + MethodIndex));
                _activation = _abc.DefineEmptyInstance(name, true);
            }

            //reserve var for activation
            int n = _method.Parameters.Count;
            _activationVar = n + 1;
            if (HasPseudoThis)
                ++_activationVar;
            ++_body.LocalCount;
        }

		private void GetActivation(AbcCode code)
        {
            code.GetLocal(_activationVar);
        }

		private void GetSlot(AbcCode code, AbcTrait t)
        {
            GetActivation(code);
            code.GetSlot(t);
        }

		private IEnumerable<IInstruction> GetSlot(AbcTrait t)
        {
            var code = new AbcCode(_abc);
            GetSlot(code, t);
            _body.Flags |= AbcBodyFlags.HasSlotPointers;
            return code;
        }

		private IEnumerable<IInstruction> SetSlot(AbcTrait t)
        {
            var code = new AbcCode(_abc);
            GetActivation(code);
            code.Swap();
            code.SetSlot(t);
            return code;
        }

        private void DefineArgPtr(IParameter p)
        {
            DefineVarPtr(p, p.Type, p.Name);
        }

		private void DefineVarPtr(IVariable v)
        {
            DefineVarPtr(v, v.Type, v.Name);
        }

		private VarPtr DefineThisPtr()
        {
            if (_thisPtr != null) return _thisPtr;
            _thisPtr = CreateVarPtr(_body.Method.Instance.Name, "$this");
            return _thisPtr;
        }
		private VarPtr _thisPtr;

		private AbcTrait CreateSlot(AbcMultiname type, string name)
        {
            var traitName = DefineSlotName(name);
            AbcTrait t;
            if (AbcGenConfig.UseActivationTraits)
            {
                t = _body.Traits.AddSlot(type, traitName);
                t.SlotId = _body.Traits.Count;
            }
            else
            {
                t = _activation.CreateSlot(traitName, type);
                t.SlotId = _activation.Traits.Count;
            }
            return t;
        }

		private AbcTrait CreateSlotPtr(AbcTrait slot)
        {
            PointerKind kind;
            var instance = DefineSlotPtrClass(slot, out kind);
            var t = CreateSlot(instance.Name, slot.NameString + "$ptr");
            t.PtrSlot = slot;
            t.PtrKind = kind;
            return t;
        }

		private AbcInstance DefineSlotPtrClass(AbcTrait slot, out PointerKind kind)
        {
            if (AbcGenConfig.UseActivationTraits && AbcGenConfig.UseFuncPointers)
            {
                kind = PointerKind.FuncPtr;
                return _generator.Pointers.FuncPtr.Instance;
            }
            //kind = PointerKind.SlotPtr;
            //return _generator.DefineSlotPtr(slot);
            kind = PointerKind.PropertyPtr;
            return _generator.Pointers.PropertyPtr(slot.Name);
        }

		private string ThisTraitName
        {
            get 
            {
                var m = _body.Method;
                if (m.TraitName != null)
                    return m.TraitName.NameString;
                return "";
            }
        }

		private AbcMultiname DefineSlotName(string name)
        {
            if (AbcGenConfig.UseActivationTraits)
            {
                return _abc.DefineName(QName.BodyTrait(name));
            }
            return _abc.DefineName(QName.Global(name));
        }

		private AbcMultiname DefineMemberType(IType type)
        {
            type = type.UnwrapRef();
			return _abc.Generator.TypeBuilder.BuildMemberType(type);
        }

		private class VarPtr
        {
            public AbcTrait Slot;
            public AbcTrait Ptr;
            //NOTE: Creator field is needed to store actual creator of this ptr.
            //This fixes sharing error between multiple compilations of given method in generic context.
            public object Creator;
        }

		private void DefineVarPtr(ICodeNode node, IType type, string name)
        {
            var ptr = node.Data as VarPtr;
            if (ptr != null && ptr.Creator == this) return;
            ptr = CreateVarPtr(type, name);
            ptr.Creator = this;
            node.Data = ptr;
        }

		private VarPtr CreateVarPtr(IType type, string name)
        {
            return CreateVarPtr(DefineMemberType(type), name);
        }

		private VarPtr CreateVarPtr(AbcMultiname type, string name)
        {
            var ptr = new VarPtr {Slot = CreateSlot(type, name)};
            ptr.Ptr = CreateSlotPtr(ptr.Slot);
            return ptr;
        }
        #endregion

        #region InitPointers
		private void InitPointers(AbcCode code)
        {
            if (!HasActivationVar) return;

            if (AbcGenConfig.UseActivationTraits)
            {
                code.NewActivation();
                if (AbcGenConfig.UseFuncPointers)
                {
                    code.Dup();
                    code.PushThisScope();
                    code.PushScope();
                }
            }
            else
            {
                code.CreateInstance(_activation);
            }
            code.SetLocal(_activationVar);

            if (IsThisAddressed)
            {
                var ptr = DefineThisPtr();
                GetActivation(code);
                code.LoadThis();
                code.SetSlot(ptr.Slot);
                InitSlotPtr(code, ptr);
            }

            //store arguments in slots
            int n = _method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = _method.Parameters[i];
                var ptr = p.Data as VarPtr;
                if (ptr == null) continue;

                GetActivation(code);
                code.GetLocal(GetArgIndex(i));
                code.SetSlot(ptr.Slot);

                InitSlotPtr(code, ptr);
            }

            if (HasLocalVariables)
            {
                n = VarCount;
                for (int i = 0; i < n; ++i)
                {
                    var v = GetVar(i);
                    var ptr = v.Data as VarPtr;
                    if (ptr == null) continue;
                    InitSlotPtr(code, ptr);
                }
            }

            //NOTE: Because of VerifyError #1068
            if (AbcGenConfig.UseActivationTraits && AbcGenConfig.UseFuncPointers)
            {
                code.PopScope(); //activation
                code.PopScope(); //this
            }
        }

		private void InitSlotPtr(AbcCode code, VarPtr ptr)
        {
            GetActivation(code);
            if (AbcGenConfig.UseActivationTraits)
            {
                switch (ptr.Ptr.PtrKind)
                {
                    case PointerKind.SlotPtr:
                        code.CreateSlotPtr(ptr.Ptr, _activationVar);
                        break;

                    case PointerKind.PropertyPtr:
                        code.CreatePropertyPtr(ptr.Slot.Name, () => GetActivation(code));
                        break;

                    case PointerKind.FuncPtr:
                        code.CreateFuncPtr(ptr.Slot);
                        break;
                }
            }
            else
            {
                code.Getlex(ptr.Ptr.SlotType);
                GetActivation(code);
                code.Construct(1);
            }
            code.SetSlot(ptr.Ptr);
        }
        #endregion
    }
}