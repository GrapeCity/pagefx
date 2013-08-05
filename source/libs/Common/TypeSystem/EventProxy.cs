using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class EventProxy : IEvent
    {
        private readonly IType _instance;
        private readonly IEvent _event;
        private IMethod _adder;
        private IMethod _remover;
        private IMethod _raiser;
	    private bool _resolveAdder = true;
	    private bool _resolveRemover = true;
	    private bool _resolveRaiser = true;

	    public EventProxy(IType instance, IEvent e)
        {
		    if (instance == null) throw new ArgumentNullException("instance");
		    if (e == null) throw new ArgumentNullException("e");

		    _instance = instance;
            _event = e;
        }

		public IEvent ProxyOf
		{
			get { return _event; }
		}

	    public IMethod Adder
        {
            get
            {
				if (_resolveAdder)
				{
					_resolveAdder = false;
					_adder = ResolveMethod(x => x.ProxyOf == _event.Adder);
				}

	            return _adder;
            }
			set { _adder = value; }
        }

	    public IMethod Remover
        {
            get
            {
				if (_resolveRemover)
				{
					_resolveRemover = false;
					_remover = ResolveMethod(x => x.ProxyOf == _event.Remover);
				}
	            return _remover;
            }
			set { _remover = value; }
        }

        public IMethod Raiser
        {
            get
            {
				if (_resolveRaiser)
				{
					_resolveRaiser = false;
					_raiser = ResolveMethod(x => x.ProxyOf == _event.Raiser);
				}
	            return _raiser;
            }
			set { _raiser = value; }
        }

        public IAssembly Assembly
        {
            get { return _event.Assembly; }
        }

        public IModule Module
        {
            get { return _event.Module; }
        }

        public MemberType MemberType
        {
            get { return MemberType.Event; }
        }

        public string Name
        {
            get { return _event.Name; }
        }

        public string FullName
        {
            get { return _event.FullName; }
        }

        public string DisplayName
        {
            get { return _event.DisplayName; }
        }

        public IType DeclaringType
        {
            get { return _instance; }
            set { throw new NotSupportedException(); }
        }

        public IType Type { get; set; }

        public Visibility Visibility
        {
            get { return _event.Visibility; }
        }

	    public bool IsStatic
        {
            get { return _event.IsStatic; }
        }

        public bool IsSpecialName
        {
            get { return _event.IsSpecialName; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _event.IsRuntimeSpecialName; }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _event.MetadataToken; }
        }

	    public ICustomAttributeCollection CustomAttributes
        {
            get { return _event.CustomAttributes; }
        }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[0]; }
        }

        public object Data { get; set; }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

	    public string Documentation
        {
            get { return _event.Documentation; }
            set { throw new NotSupportedException(); }
        }

	    public override string ToString()
        {
            return ToString(null, null);
        }

		private IMethod ResolveMethod(Func<IMethod, bool> selector)
		{
			return _instance.Methods.FirstOrDefault(selector);
		}
    }
}