using System;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    internal partial class CodeProviderImpl : ICodeProvider
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
        public CodeProviderImpl(AbcGenerator gen, AbcMethod abcMethod)
        {
            _generator = gen;
            _abc = gen.Abc;
            _method = abcMethod.SourceMethod;
            _body = abcMethod.Body;
            _declType = _method.DeclaringType;

            _asStatic = _method.AsStaticCall();

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

        private bool HasPseudoThis
        {
            get { return _asStatic && !_method.IsConstructor; }
        }

		private bool IsCtorAsStaticCall
        {
            get { return _method.IsConstructor && _asStatic; }
        }

	    private IAssembly Assembly
	    {
			get { return _generator.AppAssembly; }
	    }

	    private SystemTypes SystemTypes
		{
			get { return _generator.SystemTypes; }
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

	        DeclareTempVars(code);

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
            var name = method.Data as AbcMultiname;
            if (name != null)
                return _abc.ImportConst(name);

            var mn = method.Data as AbcMemberName;
            if (mn != null)
                return _abc.ImportConst(mn.Name);

            var abcMethod = method.Data as AbcMethod;
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
            if (method.IsVoid())
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
            if (x.Operands.EqualsTo(y.Operands))
                return true;
            return false;
        }
        #endregion

        public bool MustPreventBoxing(IMethod method, IParameter arg)
        {
            if (arg == null) return false;
            if (!method.IsAbcMethod()) return false;
            var type = arg.Type.UnwrapRef();
            return type.Is(SystemTypeCode.Object);
        }

        #region Utils
        private int VarCount
        {
            get
            {
                var vars = _method.Body.LocalVariables;
                return vars != null ? vars.Count : 0;
            }
        }

        private IVariable GetVar(int index)
        {
            return _method.Body.LocalVariables[index];
        }

        private bool HasLocalVariables
        {
            get { return VarCount > 0; }
        }

        private void EnsureType(IType type)
        {
            _generator.DefineType(type);
        }

        private void EnsureMethod(IMethod method)
        {
            _generator.DefineMethod(method);
        }

	    private bool IsBaseCtor(IMethod method)
        {
			if (_method.IsStatic || method.IsStatic) return false;
            if (!_method.IsConstructor || !method.IsConstructor) return false;
            
            var type = method.DeclaringType;
            var t = _method.DeclaringType.BaseType;
            while (t != null)
            {
                if (ReferenceEquals(type, t)) return true;
                t = t.BaseType;
            }

            return false;
        }

        private bool IsBaseMethod(IMethod method)
        {
            return _method.IsBaseMethod(method);
        }
        #endregion

        public void Finish()
        {
            _resolver.Resolve();
        	FinishExceptionHandlers();
        }

        public void CompileMethod(IMethod method)
        {
            EnsureMethod(method);
        }

        private AbcMethod DefineAbcMethod(IMethod m)
        {
            return _generator.DefineAbcMethod(m);
        }

        private AbcInstance DefineAbcInstance(IType type)
        {
            return _generator.DefineAbcInstance(type);
        }
    }
}