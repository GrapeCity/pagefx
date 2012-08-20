using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class MethodProxy : IMethod
    {
		private readonly bool _signatureChanged;
		private readonly string[] _sigNames = new string[2];
	    private readonly IGenericInstance _instance;
        private readonly IMethod _method;
        private readonly IType _type;
        private readonly ParameterCollection _params;
		private IMethod[] _implMethods;
		private IMethod _baseMethod;
		private bool _resolveBaseMethod = true;

	    #region ctor
        public MethodProxy(IGenericInstance instance, IMethod method)
        {
            _instance = instance;
            _method = method;
            _type = GenericType.Resolve(instance, method, method.Type);

            if (_type != method.Type)
                _signatureChanged = true;

            _params = new ParameterCollection();
            foreach (var p in method.Parameters)
            {
                var ptype = GenericType.Resolve(instance, method, p.Type);
                var p2 = new Parameter(ptype, p.Name, p.Index);
                _params.Add(p2);
                if (ptype != p.Type)
                {
                    _signatureChanged = true;
                    p2.HasResolvedType = true;
                }
            }
        }
        #endregion

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

        public bool PInvoke
        {
            get { return _method.PInvoke; }
            set { throw new NotSupportedException(); }
        }

        public MethodCallingConvention CallingConvention
        {
            get { return _method.CallingConvention; }
            set { throw new NotSupportedException(); }
        }

        public MethodCodeType CodeType
        {
            get { return _method.CodeType; }
            set { throw new NotSupportedException(); }
        }

        public bool IsManaged
        {
            get { return _method.IsManaged; }
            set { throw new NotSupportedException(); }
        }

        public bool IsForwardRef
        {
            get { return _method.IsForwardRef; }
            set { throw new NotSupportedException(); }
        }

        public bool IsPreserveSig
        {
            get { return _method.IsPreserveSig; }
            set { throw new NotSupportedException(); }
        }

        public bool IsInternalCall
        {
            get { return _method.IsInternalCall; }
            set { throw new NotSupportedException(); }
        }

        public bool IsSynchronized
        {
            get { return _method.IsSynchronized; }
            set { throw new NotSupportedException(); }
        }

        public bool NoInlining
        {
            get { return _method.NoInlining; }
            set { throw new NotSupportedException(); }
        }

        public IGenericParameterCollection GenericParameters
        {
            get { return _method.GenericParameters; }
        }

        public IType[] GenericArguments
        {
            get { return _method.GenericArguments; }
        }

        /// <summary>
        /// Returns true if the method is generic.
        /// </summary>
        public bool IsGeneric
        {
            get { return _method.IsGeneric; }
        }

        public bool IsGenericInstance
        {
            get { return _method.IsGenericInstance; }
        }

        public IParameterCollection Parameters
        {
            get { return _params; }
        }

        public ICustomAttributeCollection ReturnCustomAttributes
        {
            get { return _method.ReturnCustomAttributes; }
        }

        public ITypeMember Association { get; set; }

        public bool IsGetter
        {
            get { return _method.IsGetter; }
        }

        public bool IsSetter
        {
            get { return _method.IsSetter; }
        }

        public bool IsExplicitImplementation
        {
            get { return _method.IsExplicitImplementation; }
            set { throw new NotSupportedException(); }
        }

        private IMethod ResolveMethod(IMethod method)
        {
            if (method == null) return null;
            var declType = GenericType.Resolve(_instance, this, method.DeclaringType);
            if (declType is IGenericInstance)
            {
                var m = GenericType.FindMethodProxy(declType, method);
                if (m == null)
                    throw new InvalidOperationException();
                return m;
            }
            return method;
        }

        public IMethod[] ImplementedMethods
        {
            get
            {
                if (_method.IsGeneric)
                    return _method.ImplementedMethods;

                if (_implMethods == null)
                {
                    var impl = _method.ImplementedMethods;
                    int n;
                    if (impl != null && (n = impl.Length) > 0)
                    {
                        _implMethods = new IMethod[n];
                        for (int i = 0; i < n; ++i)
                        {
                            _implMethods[i] = ResolveMethod(impl[i]);
                        }
                    }
                    else
                    {
                        _implMethods = new IMethod[0];
                    }
                }
                return _implMethods;
            }
            set 
            { 
                if (value == null)
                    throw new ArgumentNullException("value");
                _implMethods = value;
            }
        }
        
        public IMethodBody Body
        {
            get { return _method.Body; }
            set { throw new NotSupportedException(); }
        }

        public string ReturnDocumentation
        {
            get { return _method.ReturnDocumentation; }
            set { throw new NotSupportedException(); }
        }

        public IMethod BaseMethod
        {
            get 
            {
                if (_resolveBaseMethod)
                {
                    _resolveBaseMethod = false;
                    _baseMethod = ResolveMethod(_method.BaseMethod);
                }
                return _baseMethod;
            }
        }
        
        public IMethod ProxyOf
        {
            get { return _method; }
        }

        public IMethod InstanceOf
        {
            get { return _method.InstanceOf; }
        }

        /// <summary>
        /// Returns true if signature was changed during resolving.
        /// </summary>
        public bool SignatureChanged
        {
            get { return _signatureChanged; }
        }
        
	    #endregion

        #region IOverridableMember Members
        public bool IsAbstract
        {
            get { return _method.IsAbstract; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVirtual
        {
            get { return _method.IsVirtual; }
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

        public bool IsOverride
        {
            get { return _method.IsOverride; }
        }
        #endregion

        #region ITypeMember Members
        public IAssembly Assembly
        {
            get { return _method.Assembly; }
        }

        public IModule Module
        {
            get { return _method.Module; }
            set { throw new NotSupportedException(); }
        }

        public MemberType MemberType
        {
            get { return MemberType.Method; }
        }

        public string Name
        {
            get { return _method.Name; }
            set { throw new NotSupportedException(); }
        }

        public string FullName
        {
            get { return _method.FullName; }
        }

        public string DisplayName
        {
            get { return _method.DisplayName; }
        }

        public IType DeclaringType
        {
            get { return _instance; }
            set 
            {
                if (value != _instance)
                    throw new InvalidOperationException();
            }
        }

        public IType Type
        {
            get { return _type; }
            set { throw new NotSupportedException(); }
        }

        public Visibility Visibility
        {
            get { return _method.Visibility; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVisible
        {
            get { return _method.IsVisible; }
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
        public int MetadataToken
        {
            get { return _method.MetadataToken; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region ICustomAttributeProvider Members
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _method.CustomAttributes; }
        }
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return _method.NodeType; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[0]; }
        }

    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IDocumentationProvider Members
        public string Documentation
        {
            get { return _method.Documentation; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}