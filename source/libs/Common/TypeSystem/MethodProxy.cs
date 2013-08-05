using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class MethodProxy : IMethod
    {
		private readonly string[] _sigNames = new string[2];
	    private readonly IType _instance;
        private readonly IMethod _method;
        private IType _type;
        private readonly IParameterCollection _parameters;
		private IReadOnlyList<IMethod> _impls;
		private IMethod _baseMethod;
		private bool _resolveBaseMethod = true;
	    private ITypeMember _association;
	    private bool _resolveAssociation = true;

	    public MethodProxy(IType instance, IMethod method)
        {
		    if (instance == null) throw new ArgumentNullException("instance");
		    if (method == null) throw new ArgumentNullException("method");

		    _instance = instance;
            _method = method;
			_parameters = new ParameterProxyCollection(_method.Parameters, _instance, _method);
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

        public bool PInvoke
        {
            get { return _method.PInvoke; }
        }

	    public MethodCodeType CodeType
        {
            get { return _method.CodeType; }
        }

        public bool IsManaged
        {
            get { return _method.IsManaged; }
        }

        public bool IsForwardRef
        {
            get { return _method.IsForwardRef; }
        }

        public bool IsPreserveSig
        {
            get { return _method.IsPreserveSig; }
        }

        public bool IsInternalCall
        {
            get { return _method.IsInternalCall; }
        }

        public bool IsSynchronized
        {
            get { return _method.IsSynchronized; }
        }

        public bool NoInlining
        {
            get { return _method.NoInlining; }
        }

        public ITypeCollection GenericParameters
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
            get { return _parameters; }
        }

	    public ICustomAttributeCollection ReturnCustomAttributes
        {
            get { return _method.ReturnCustomAttributes; }
        }

	    public ITypeMember Association
	    {
		    get
		    {
			    if (_resolveAssociation)
			    {
				    _resolveAssociation = false;
				    _association = ResolveAssociation();
			    }
			    return _association;
		    }
	    }

	    private ITypeMember ResolveAssociation()
	    {
		    var association = _method.Association;
		    if (association == null)
			    return null;

		    var property = association as IProperty;
			if (property != null)
			{
				var proxy =  _instance.Properties.OfType<PropertyProxy>().FirstOrDefault(x => ReferenceEquals(x.ProxyOf, association));

				if (property.Getter == _method)
					proxy.Getter = this;
				else if (property.Setter == _method)
					proxy.Setter = this;

				return proxy;
			}

		    var @event = association as IEvent;
			if (@event != null)
			{
				var proxy = _instance.Events.OfType<EventProxy>().FirstOrDefault(x => ReferenceEquals(x.ProxyOf, association));

				if (@event.Adder == _method)
					proxy.Adder = this;
				else if (@event.Remover == _method)
					proxy.Remover = this;
				else if (@event.Raiser == _method)
					proxy.Raiser = this;
				
				return proxy;
			}

			throw new InvalidOperationException();
	    }

	    public bool IsExplicitImplementation
        {
            get { return _method.IsExplicitImplementation; }
        }

        private IMethod ResolveMethod(IMethod method)
        {
            if (method == null) return null;
            var declType = GenericType.Resolve(_instance, this, method.DeclaringType);
            if (declType.IsGenericInstance())
            {
                var m = GenericType.FindMethodProxy(declType, method);
                if (m == null)
                    throw new InvalidOperationException();
                return m;
            }
            return method;
        }

        public IReadOnlyList<IMethod> Implements
        {
            get
            {
                if (_method.IsGeneric)
                    return _method.Implements;

	            return _impls ?? (_impls = PopulateImpls().Memoize());
            }
        }

		private IEnumerable<IMethod> PopulateImpls()
		{
			var methods = _method.Implements;
			return methods == null
				       ? Enumerable.Empty<IMethod>()
				       : methods.Select(x => ResolveMethod(x));
		}
        
        public IMethodBody Body
        {
            get { return _method.Body; }
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

        #endregion

        #region IOverridableMember Members

        public bool IsAbstract
        {
            get { return _method.IsAbstract; }
        }

        public bool IsVirtual
        {
            get { return _method.IsVirtual; }
        }

        public bool IsFinal
        {
            get { return _method.IsFinal; }
        }

        public bool IsNewSlot
        {
            get { return _method.IsNewSlot; }
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
        }

        public MemberType MemberType
        {
            get { return MemberType.Method; }
        }

        public string Name
        {
            get { return _method.Name; }
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
        }

        public IType Type
        {
            get { return _type ?? (_type = GenericType.Resolve(_instance, _method, _method.Type)); }
        }

        public Visibility Visibility
        {
            get { return _method.Visibility; }
        }

	    public bool IsStatic
        {
            get { return _method.IsStatic; }
        }

        public bool IsSpecialName
        {
            get { return _method.IsSpecialName; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _method.IsRuntimeSpecialName; }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _method.MetadataToken; }
        }

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
            get { return Enumerable.Empty<ICodeNode>(); }
        }

    	public object Data { get; set; }

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