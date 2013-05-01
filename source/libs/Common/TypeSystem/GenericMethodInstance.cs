using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class GenericMethodInstance : IMethod
    {
		private readonly string[] _sigNames = new string[2];
	    private IType _retType;
        private readonly IMethod _method;
        private readonly IType[] _args;
        private readonly IParameterCollection _params;
		private IReadOnlyList<IMethod> _impls;
		private IMethod _baseMethod;
		private bool _resolveBaseMethod = true;

        public GenericMethodInstance(IType declType, IMethod method, IType[] args)
        {
            if (declType == null)
                declType = method.DeclaringType;

            if (method == null)
                throw new ArgumentNullException("method");

            method = Unwrap(method);

            _method = method;
            _args = args;

            DeclaringType = declType;
            
			_params = new ParameterProxyCollection(method.Parameters, declType, this);
        }

        public static IMethod Unwrap(IMethod method)
        {
            while (method.ProxyOf != null)
                method = method.ProxyOf;

            while (method.IsGenericInstance)
            {
                method = method.InstanceOf;
                if (method == null)
                    throw new InvalidOperationException();
            }

            if (!method.IsGeneric)
                throw new InvalidOperationException(
                    string.Format("Method '{0}' has no generic parameters", method.FullName));

            return method;
        }

        #region IMethod Members

	    public string GetSigName(Runtime runtime)
	    {
			return _sigNames[(int)runtime] ?? (_sigNames[(int)runtime] = this.BuildSigName(runtime));
	    }

	    public bool IsEntryPoint
        {
            get { return _method.IsEntryPoint; }
        }

        public bool IsConstructor
        {
            get { return _method.IsConstructor; }
        }

        public bool IsAbstract
        {
            get { return _method.IsAbstract; }
            set { throw new NotSupportedException(); }
        }

        public bool IsFinal
        {
            get { return _method.IsFinal; }
            set { throw new NotSupportedException(); }
        }

        public bool IsNewSlot
        {
            get { return _method.IsNewSlot; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVirtual
        {
            get { return _method.IsVirtual; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the method overrides implementation of base type.
        /// </summary>
        public bool IsOverride
        {
            get { return _method.IsOverride; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating the method implementation is forwarded through PInvoke (Platform Invocation Services).
        /// </summary>
        public bool PInvoke
        {
            get { return _method.PInvoke; }
            set { throw new NotSupportedException(); }
        }

		#region Impl Flags
        /// <summary>
        /// Gets or sets value indicating what kind of implementation is provided for this method.
        /// </summary>
        public MethodCodeType CodeType
        {
            get { return _method.CodeType; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating whether the method is managed.
        /// </summary>
        public bool IsManaged
        {
            get { return _method.IsManaged; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method is declared, but its implementation is provided elsewhere.
        /// </summary>
        public bool IsForwardRef
        {
            get { return _method.IsForwardRef; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method signature is exported exactly as declared.
        /// </summary>
        public bool IsPreserveSig
        {
            get { return _method.IsPreserveSig; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method implemented within the common language runtime itself.
        /// </summary>
        public bool IsInternalCall
        {
            get { return _method.IsInternalCall; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method can be executed by only one thread at a time.
        /// </summary>
        public bool IsSynchronized
        {
            get { return _method.IsSynchronized; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets flag indicating that the method can not be inlined.
        /// </summary>
        public bool NoInlining
        {
            get { return _method.NoInlining; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        public ITypeCollection GenericParameters
        {
			get { return TypeCollection.Empty; }
        }
		
        public IType[] GenericArguments
        {
            get { return _args; }
        }

        /// <summary>
        /// Returns true if the method is generic.
        /// </summary>
        public bool IsGeneric
        {
            get { return false; }
        }

        public bool IsGenericInstance
        {
            get { return true; }
        }

        public IParameterCollection Parameters
        {
            get { return _params; }
        }

        /// <summary>
        /// Gets collection of custom attributes for return type.
        /// </summary>
        public ICustomAttributeCollection ReturnCustomAttributes
        {
            get { return _method.ReturnCustomAttributes; }
        }

        public ITypeMember Association
        {
            get { return _method.Association; }
            set { throw new NotSupportedException(); }
        }
        
		/// <summary>
        /// Gets or sets boolean flag indicating whether the method is explicit implementation of some interface method.
        /// </summary>
        public bool IsExplicitImplementation
        {
            get { return _method.IsExplicitImplementation; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets methods implemented by this method
        /// </summary>
        public IReadOnlyList<IMethod> Implements
        {
            get { return _impls ?? (_impls = PopulateImpls().Memoize()); }
            set { throw new NotSupportedException(); }
        }

		private IEnumerable<IMethod> PopulateImpls()
		{
			var methods = _method.Implements;
			return methods == null
				       ? Enumerable.Empty<IMethod>()
				       : methods.Select(x => ResolveInstance(x));
		}
        
        public IMethodBody Body
        {
            get { return _method.Body; }
            set { throw new NotSupportedException(); }
        }

        private IMethod ResolveInstance(IMethod method)
        {
            if (method.IsGeneric)
            {
                var declType = GenericType.Resolve(DeclaringType, this, method.DeclaringType);
                return GenericType.CreateMethodInstance(declType, method, GenericArguments);
            }

            throw new InvalidOperationException();
        }

        public IMethod BaseMethod
        {
            get
            {
                if (_resolveBaseMethod)
                {
                    _resolveBaseMethod = false;
                    var bm = _method.BaseMethod;
                    if (bm != null)
                        _baseMethod = ResolveInstance(bm);
                }
                return _baseMethod;
            }
            set { throw new NotSupportedException(); }
        }
        

        public IMethod ProxyOf
        {
            get { return null; }
        }

        public IMethod InstanceOf
        {
            get { return _method; }
        }
        #endregion

        #region ITypeMember Members
        /// <summary>
        /// Gets the assembly in which the member is declared.
        /// </summary>
        public IAssembly Assembly
        {
            get { return _method.Assembly; }
        }

        /// <summary>
        /// Gets the module in which the member is defined. 
        /// </summary>
        public IModule Module
        {
            get { return _method.Module; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public MemberType MemberType
        {
            get { return _method.MemberType; }
        }

        public string Name
        {
            get
            {
                //TODO:
                return _method.Name;
            }
            set { throw new NotSupportedException(); }
        }

        public string FullName
        {
            get { return _method.FullName; }
        }

        public string DisplayName
        {
            get { return Name; }
        }

        public IType DeclaringType { get; set; }

        public IType Type
        {
			get { return _retType ?? (_retType = GenericType.Resolve(DeclaringType, this, _method.Type)); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        public Visibility Visibility
        {
            get { return _method.Visibility; }
            set { throw new NotSupportedException(); }
        }

		public bool IsStatic
        {
            get { return _method.IsStatic; }
            set { throw new NotSupportedException(); }
        }

        public bool IsSpecialName
        {
            get { return _method.IsSpecialName; }
            set { throw new NotSupportedException(); }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _method.IsRuntimeSpecialName; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken { get; set; }
        #endregion

        #region ICustomAttributeProvider Members
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _method.CustomAttributes; }
        }
        #endregion

        #region ICodeNode Members

		public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        public object Data { get; set; }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IDocumentationProvider Members
        /// <summary>
        /// Gets or sets documentation of this member
        /// </summary>
        public string Documentation
        {
            get { return _method.Documentation; }
            set { }
        }

        /// <summary>
        /// Gets or sets documentation for return value.
        /// </summary>
        public string ReturnDocumentation
        {
            get { return _method.ReturnDocumentation; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}