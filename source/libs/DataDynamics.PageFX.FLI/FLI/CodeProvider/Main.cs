using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AvmCodeProvider : ICodeProvider
    {
        #region Fields
        readonly AbcGenerator _generator;
        readonly AbcFile _abc;
        readonly IMethod _method;
        readonly IType _declType;
        readonly AbcMethodBody _body;
        readonly bool _asStatic;
        #endregion

        #region Constructors
        public AvmCodeProvider(AbcGenerator gen, AbcMethod abcMethod)
        {
            _generator = gen;
            _abc = gen._abc;
            _method = abcMethod.SourceMethod;
            _body = abcMethod.Body;
            _declType = _method.DeclaringType;

            _asStatic = MethodHelper.AsStaticCall(_method);

            //This code enshures initial capacity for local registers 
            _body.LocalCount = _method.Parameters.Count + 1;
            var body = _method.Body;
            if (body == null)
                throw new InvalidOperationException("method has no body");

            if (HasPseudoThis)
                _body.LocalCount++;

            int n = VarCount;
            if (n == 0)
            {
                //NOTE: AVM constraint for LocalCount 
                //if (local_count < info->param_count+1)
                //{
                // must have enough locals to hold all parameters including this
                //toplevel->throwVerifyError(kCorruptABCError);
                //}
                _body.LocalCount++;
            }
            else
            {
                _body.LocalCount += n;
            }
        }

        bool HasPseudoThis
        {
            get { return _asStatic && !_method.IsConstructor; }
        }

        bool IsCtorAsStaticCall
        {
            get { return _method.IsConstructor && _asStatic; }
        }
        #endregion

        #region Events
        public void BeforeTranslation()
        {
            InitActivation();
        }

        public void AfterTranslation()
        {
        }

        public IInstruction[] Begin()
        {
            var code = new AbcCode(_abc);

            //NOTE: We can not push this scope because of the following verifier error:
            //VerifyError: Error #1068: class System.String and class System.String cannot be reconciled.
            //code.PushThisScope();

            EmitLocalsDebugInfo(code);

            code.InitFields(_method);

            InitPointers(code);

            return code.ToArray();
        }

        public IInstruction[] End()
        {
            return null;
        }
        #endregion

        public IInstruction SourceInstruction { get; set; }

        public int CurrentMetadataToken
        {
            get 
            {
                return SourceInstruction.MetadataToken;
            }
        }

        void AddToken(TokenKind kind)
        {
            _body.AddToken(CurrentMetadataToken, kind);
        }

        public bool IsCcsRunning
        {
            get { return _generator.IsCcsRunning; }
        }

        public IInstruction Nop()
        {
            //NOTE: nop is really needed to build empty basic blocks.
            //Therefore we should not return null.
            return new Instruction(InstructionCode.Nop);
        }

        #region Stack Operations
        public IInstruction Dup()
        {
            return new Instruction(InstructionCode.Dup);
        }

        public IInstruction Swap()
        {
            return new Instruction(InstructionCode.Swap);
        }

        public IInstruction Pop()
        {
            return new Instruction(InstructionCode.Pop);
        }
        #endregion

        #region Delegates
        AbcMultiname GetMethodName(IMethod method)
        {
            var name = method.Tag as AbcMultiname;
            if (name != null)
                return _abc.ImportConst(name);

            var mn = method.Tag as AbcMemberName;
            if (mn != null)
                return _abc.ImportConst(mn.Name);

            var abcMethod = method.Tag as AbcMethod;
            if (abcMethod != null)
                return _abc.ImportConst(abcMethod.TraitName);

            return null;
        }

        public IInstruction[] LoadFunction(IMethod method)
        {
            EnsureMethod(method);
            var code = new AbcCode(_abc);
            var name = GetMethodName(method);
            code.GetProperty(name);
            code.CoerceFunction();
            return code.ToArray();
        }

        public IInstruction[] InvokeDelegate(IMethod method)
        {
            var code = new AbcCode(_abc);
            code.Add(InstructionCode.Call, method.Parameters.Count);
            if (TypeService.IsVoid(method))
            {
                code.Add(InstructionCode.Pop);
            }
            return code.ToArray();
        }
        #endregion

        #region IsDuplicate
        static bool CanBeDuplicated(InstructionCode code)
        {
            switch (code)
            {
                case InstructionCode.Coerce_u:
                case InstructionCode.Coerce:
                case InstructionCode.Coerce_a:
                case InstructionCode.Coerce_b:
                case InstructionCode.Coerce_d:
                case InstructionCode.Coerce_i:
                case InstructionCode.Coerce_o:
                case InstructionCode.Coerce_s:
                case InstructionCode.Convert_b:
                case InstructionCode.Convert_d:
                case InstructionCode.Convert_i:
                case InstructionCode.Convert_o:
                case InstructionCode.Convert_s:
                case InstructionCode.Convert_u:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether given instructions are the same and can be removed when they are subsequent.
        /// </summary>
        /// <param name="a">First instruction under test.</param>
        /// <param name="b">Second instruction under test.</param>
        /// <returns></returns>
        public bool IsDuplicate(IInstruction a, IInstruction b)
        {
            var x = a as Instruction;
            if (x == null)
                throw new ArgumentException();
            var y = b as Instruction;
            if (y == null)
                throw new ArgumentException();
            var ic = x.Code;
            if (ic != y.Code)
                return false;
            if (!CanBeDuplicated(ic))
                return false;
            if (Algorithms.Equals(x.Operands, y.Operands))
                return true;
            return false;
        }
        #endregion

        public bool MustPreventBoxing(IMethod method, IParameter arg)
        {
            if (arg == null) return false;
            if (!MethodHelper.IsAbcMethod(method)) return false;
            var ptype = TypeService.UnwrapRef(arg.Type);
            return ptype == SystemTypes.Object;
        }

        #region Utils
        int VarCount
        {
            get
            {
                var vars = _method.Body.LocalVariables;
                return vars != null ? vars.Count : 0;
            }
        }

        IVariable GetVar(int index)
        {
            return _method.Body.LocalVariables[index];
        }

        bool HasLocalVariables
        {
            get { return VarCount > 0; }
        }

        void EnsureType(IType type)
        {
            _generator.DefineType(type);
        }

        void EnsureMethod(IMethod method)
        {
            _generator.DefineMethod(method);
        }

        /// <summary>
        /// Returns true if given method is ctor of this or base type
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        bool IsThisOrBaseCtor(IMethod method)
        {
            if (!method.IsConstructor) return false;
            if (!_method.IsConstructor) return false;
            if (_method.IsStatic) return false;
            if (method.IsStatic) return false;
            var type = method.DeclaringType;
            var t = _method.DeclaringType;
            while (t != null)
            {
                if (type == t) return true;
                t = t.BaseType;
            }
            return false;
        }

        bool IsBaseCtor(IMethod method)
        {
            if (!method.IsConstructor) return false;
            if (!_method.IsConstructor) return false;
            if (_method.IsStatic) return false;
            if (method.IsStatic) return false;
            var type = method.DeclaringType;
            var t = _method.DeclaringType.BaseType;
            while (t != null)
            {
                if (type == t) return true;
                t = t.BaseType;
            }
            return false;
        }

        bool IsBaseMethod(IMethod method)
        {
            return TypeService.IsBaseMethod(_method, method);
        }
        #endregion

        public void Finish()
        {
            _resolver.Resolve();
            ResolveExceptionHandlers();
        }

        public void CompileMethod(IMethod method)
        {
            EnsureMethod(method);
        }

        AbcMethod DefineAbcMethod(IMethod m)
        {
            return _generator.DefineAbcMethod(m);
        }

        AbcInstance DefineAbcInstance(IType type)
        {
            return _generator.DefineAbcInstance(type);
        }

        object DefineType(IType type)
        {
            return _generator.DefineType(type);
        }
    }
}