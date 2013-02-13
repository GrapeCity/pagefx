using System;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents type property.
    /// </summary>
    public sealed class Property : TypeMember, IProperty
    {
		private IParameterCollection _parameters;

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return MemberType.Property; }
        }

        public override Visibility Visibility
        {
            get
            {
	            var getter = Getter;
	            var setter = Setter;
				
                if (getter != null)
                {
                    if (setter != null)
                    {
                        var gv = getter.Visibility;
                        var sv = setter.Visibility;
                        return gv > sv ? gv : sv;
                    }
                    return getter.Visibility;
                }

                return setter != null ? setter.Visibility : base.Visibility;
            }
            set
            {
                base.Visibility = value;
            }
        }

	    public bool HasDefault
        {
            get { return GetModifier(Modifiers.HasDefault); }
            set { SetModifier(value, Modifiers.HasDefault); }
        }

		private bool IsFlag(Func<IMethod, bool> get)
		{
			return new[] {Getter, Setter}
				.Where(x => x != null)
				.Any(get);
		}

        /// <summary>
        /// Gets or sets the flag indicating whether the property is abtract.
        /// </summary>
        public bool IsAbstract
        {
            get { return IsFlag(x => x.IsAbstract); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property is virtual (overridable)
        /// </summary>
        public bool IsVirtual
        {
            get { return IsFlag(x => x.IsVirtual); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property is final (can not be overriden)
        /// </summary>
        public bool IsFinal
        {
            get { return IsFlag(x => x.IsFinal); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether property overrides return type.
        /// </summary>
        public bool IsNewSlot
        {
            get { return IsFlag(x => x.IsNewSlot); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property overrides implementation of base type.
        /// </summary>
        public bool IsOverride
        {
            get { return IsFlag(x => x.IsOverride); }
        }

        public override bool IsStatic
        {
            get { return IsFlag(x => x.IsStatic); }
        }

        public IParameterCollection Parameters
        {
            get { return _parameters ?? (_parameters = new PropertyParameterCollection(this)); }
        }
        
        public IMethod Getter
        {
            get { return _getter ?? (_getter = ResolveGetter()); }
            set { _getter = value; }
        }
        private IMethod _getter;

        public IMethod Setter
        {
            get { return _setter ?? (_setter = ResolveSetter()); }
            set { _setter = value; }
        }
        private IMethod _setter;

	    public object Value { get; set; }

		private IMethod ResolveGetter()
		{
			var declType = DeclaringType;
			return declType != null ? declType.Methods.FirstOrDefault(x => x.Association == this && !x.IsVoid()) : null;
		}

		private IMethod ResolveSetter()
		{
			var declType = DeclaringType;
			return declType != null ? declType.Methods.FirstOrDefault(x => x.Association == this && x.IsVoid()) : null;
		}

		protected override IType ResolveType()
		{
			if (_getter != null)
			{
				return _getter.Type;
			}

			if (_setter != null)
			{
				return _setter.Parameters[_setter.Parameters.Count - 1].Type;
			}

			return null;
		}

		protected override IType ResolveDeclaringType()
		{
			return _getter != null
				       ? _getter.DeclaringType
				       : (_setter != null ? _setter.DeclaringType : null);
		}
    }
}