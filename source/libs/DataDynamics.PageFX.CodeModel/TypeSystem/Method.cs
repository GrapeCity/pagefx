using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public sealed class Method : TypeMember, IMethod
    {
		private readonly string[] _sigNames = new string[2];
	    private bool _resolveBaseMethod = true;
	    private IMethod _baseMethod;
		private readonly GenericParameterCollection _genericParams = new GenericParameterCollection();
		private IParameterCollection _parameters;
		private readonly CustomAttributeCollection _returnAttrs = new CustomAttributeCollection();
		private ITypeMember _association;

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return IsConstructor ? MemberType.Constructor : MemberType.Method; }
        }

        #region IMethod Members

	    public string GetSigName(Runtime runtime)
	    {
		    return _sigNames[(int)runtime] ?? (_sigNames[(int)runtime] = this.BuildSigName(runtime));
	    }

	    public bool IsEntryPoint
        {
            get { return GetModifier(Modifiers.EntryPoint); }
            set { SetModifier(value, Modifiers.EntryPoint); }
        }

        public bool IsConstructor
        {
            get { return IsSpecialName && IsCtorName(Name); }
        }

        static bool IsCtorName(string name)
        {
            return name == CLRNames.Constructor || name == CLRNames.StaticConstructor || name == "<init>";
        }

        public bool IsAbstract
        {
            get { return GetModifier(Modifiers.Abstract); }
            set { SetModifier(value, Modifiers.Abstract); }
        }

        public bool IsFinal
        {
            get { return GetModifier(Modifiers.Sealed); }
            set { SetModifier(value, Modifiers.Sealed); }
        }

        public bool IsNewSlot
        {
            get { return GetModifier(Modifiers.New); }
            set { SetModifier(value, Modifiers.New); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the method overrides implementation of base type.
        /// </summary>
        public bool IsOverride
        {
            get { return IsVirtual && !IsNewSlot; }
        }

        public bool IsVirtual
        {
            get { return GetModifier(Modifiers.Virtual); }
            set { SetModifier(value, Modifiers.Virtual); }
        }

        /// <summary>
        /// Gets or sets flag indicating the method implementation is forwarded through PInvoke (Platform Invocation Services).
        /// </summary>
        public bool PInvoke
        {
            get { return GetModifier(Modifiers.PInvoke); }
            set { SetModifier(value, Modifiers.PInvoke); }
        }

        public MethodCallingConvention CallingConvention { get; set; }

        #region Impl Flags

	    public MethodImplAttributes ImplFlags { get; set; }

	    /// <summary>
        /// Gets or sets value indicating what kind of implementation is provided for this method.
        /// </summary>
        public MethodCodeType CodeType
        {
            get
            {
                switch (ImplFlags & MethodImplAttributes.CodeTypeMask)
                {
                    case MethodImplAttributes.Native:
                        return MethodCodeType.Native;
                    case MethodImplAttributes.OPTIL:
                        return MethodCodeType.OPTIL;
                    case MethodImplAttributes.Runtime:
                        return MethodCodeType.Runtime;
                }
                return MethodCodeType.IL;
            }
            set
            {
                if (value != CodeType)
                {
                    ImplFlags &= ~MethodImplAttributes.CodeTypeMask;
                    switch (value)
                    {
                        case MethodCodeType.IL:
                            ImplFlags |= MethodImplAttributes.IL;
                            break;

                        case MethodCodeType.Native:
                            ImplFlags |= MethodImplAttributes.Native;
                            break;

                        case MethodCodeType.OPTIL:
                            ImplFlags |= MethodImplAttributes.OPTIL;
                            break;

                        case MethodCodeType.Runtime:
                            ImplFlags |= MethodImplAttributes.Runtime;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("value");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets flag indicating whether the method is managed.
        /// </summary>
        public bool IsManaged
        {
            get
            {
                return (ImplFlags & MethodImplAttributes.Unmanaged) == 0;
            }
            set
            {
                if (value) ImplFlags &= ~MethodImplAttributes.Unmanaged;
                else ImplFlags |= MethodImplAttributes.Unmanaged;
            }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method is declared, but its implementation is provided elsewhere.
        /// </summary>
        public bool IsForwardRef
        {
            get { return (ImplFlags & MethodImplAttributes.ForwardRef) != 0; }
            set
            {
                if (value) ImplFlags |= MethodImplAttributes.ForwardRef;
                else ImplFlags &= ~MethodImplAttributes.ForwardRef;
            }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method signature is exported exactly as declared.
        /// </summary>
        public bool IsPreserveSig
        {
            get { return (ImplFlags & MethodImplAttributes.PreserveSig) != 0; }
            set
            {
                if (value) ImplFlags |= MethodImplAttributes.PreserveSig;
                else ImplFlags &= ~MethodImplAttributes.PreserveSig;
            }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method implemented within the common language runtime itself.
        /// </summary>
        public bool IsInternalCall
        {
            get { return (ImplFlags & MethodImplAttributes.InternalCall) != 0; }
            set
            {
                if (value) ImplFlags |= MethodImplAttributes.InternalCall;
                else ImplFlags &= ~MethodImplAttributes.InternalCall;
            }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method can be executed by only one thread at a time.
        /// </summary>
        public bool IsSynchronized
        {
            get { return (ImplFlags & MethodImplAttributes.Synchronized) != 0; }
            set
            {
                if (value) ImplFlags |= MethodImplAttributes.Synchronized;
                else ImplFlags &= ~MethodImplAttributes.Synchronized;
            }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method can not be inlined.
        /// </summary>
        public bool NoInlining
        {
            get { return (ImplFlags & MethodImplAttributes.NoInlining) != 0; }
            set
            {
                if (value) ImplFlags |= MethodImplAttributes.NoInlining;
                else ImplFlags &= ~MethodImplAttributes.NoInlining;
            }
        }
        #endregion

    	IGenericParameterCollection IMethod.GenericParameters
        {
            get { return _genericParams; }
        }
		public GenericParameterCollection GenericParameters
		{
			get { return _genericParams; }
		}

        public IType[] GenericArguments
        {
            get { return new IType[0]; }
        }

        /// <summary>
        /// Returns true if the method is generic.
        /// </summary>
        public bool IsGeneric
        {
            get { return _genericParams.Count > 0; }
        }

        public bool IsGenericInstance
        {
            get { return false; }
        }

        public IParameterCollection Parameters
        {
            get { return _parameters ?? (_parameters = new ParameterCollection()); }
			set { _parameters = value; }
        }

        /// <summary>
        /// Gets collection of custom attributes for return type.
        /// </summary>
        public ICustomAttributeCollection ReturnCustomAttributes
        {
            get { return _returnAttrs; }
        }
	    
	    public ITypeMember Association
	    {
			get { return _association ?? (_association = ResolveAssociation()); }
			set { _association = value; }
	    }

	    private ITypeMember ResolveAssociation()
	    {
		    return Meta != null ? Meta.Association : null;
	    }

	    public bool IsGetter
        {
             get
             {
                 var prop = Association as IProperty;
                 if (prop != null)
                 {
                     return prop.Getter == this;
                 }
                 return false;
             }
        }

        public bool IsSetter
        {
             get
             {
                 var prop = Association as IProperty;
                 if (prop != null)
                 {
                     return prop.Setter == this;
                 }
                 return false;
             }
        }

        /// <summary>
        /// Gets or sets boolean flag indicating whether the method is explicit implementation of some interface method.
        /// </summary>
        public bool IsExplicitImplementation
        {
            get { return GetModifier(Modifiers.ExplicitImplementation); }
            set { SetModifier(value, Modifiers.ExplicitImplementation); }
        }

        /// <summary>
        /// Gets or sets abstract methods implemented by this method
        /// </summary>
        public IMethod[] ImplementedMethods { get; set; }

        public IMethodBody Body { get; set; }

        /// <summary>
        /// Gets or sets documentation for return value.
        /// </summary>
        public string ReturnDocumentation { get; set; }

	    public IMethod BaseMethod
        {
            get
            {
                if (_baseMethod == null && _resolveBaseMethod)
                {
                    _resolveBaseMethod = false;
                    _baseMethod = ResolveBaseMethod(this);
                }
                return _baseMethod;
            }
        }

	    public IMethod ProxyOf
        {
            get { return null; }
        }

        public IMethod InstanceOf
        {
            get { return null; }
        }

        /// <summary>
        /// Returns true if signature was changed during resolving.
        /// </summary>
        public bool SignatureChanged
        {
            get { return false; }
        }

        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get
            {
                if (Body != null)
                    return new ICodeNode[] {Body.Statements};
                return null;
            }
        }
        #endregion

	    private static IMethod ResolveBaseMethod(IMethod method)
        {
            if (method.IsStatic) return null;
            if (method.IsConstructor) return null;
            if (method.IsAbstract) return null;

            var declType = method.DeclaringType;
            if (declType.IsInterface) return null;

            var baseType = declType.BaseType;
            while (baseType != null)
            {
                var bm = baseType.FindSameMethod(method, false);
                if (bm != null)
                    return bm;
                baseType = baseType.BaseType;
            }

            return null;
        }

        public static int GetSpecificity(IMethod method)
        {
        	return method.Parameters.Count(p => !p.HasResolvedType);
        }

		public IMetaMethod Meta { get; set; }

		protected override IType ResolveType()
		{
			return Meta != null ? Meta.Type : null;
		}

		protected override IType ResolveDeclaringType()
		{
			return Meta != null ? Meta.DeclaringType : null;
		}
    }

	public interface IMetaMethod
	{
		IType Type { get; }

		IType DeclaringType { get; }

		ITypeMember Association { get; }
	}
}