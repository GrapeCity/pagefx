using System;
using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    /// <summary>
    /// Represents type property.
    /// </summary>
    public sealed class Property : TypeMember, IProperty
    {
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
                if (_getter != null)
                {
                    if (_setter != null)
                    {
                        var gv = _getter.Visibility;
                        var sv = _setter.Visibility;
                        return gv > sv ? gv : sv;
                    }
                    return _getter.Visibility;
                }
                if (_setter != null) 
                    return _setter.Visibility;
                return base.Visibility;
            }
            set
            {
                base.Visibility = value;
            }
        }

        #region IProperty Members
        public bool HasDefault
        {
            get { return GetModifier(Modifiers.HasDefault); }
            set { SetModifier(value, Modifiers.HasDefault); }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property is abtract.
        /// </summary>
        public bool IsAbstract
        {
            get
            {
                if (_getter != null)
                {
                    if (_setter != null)
                        return _getter.IsAbstract || _setter.IsAbstract;
                    return _getter.IsAbstract;
                }
                if (_setter != null)
                    return _setter.IsAbstract;
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property is virtual (overridable)
        /// </summary>
        public bool IsVirtual
        {
            get
            {
                if (_getter != null)
                {
                    if (_setter != null)
                        return _getter.IsVirtual || _setter.IsVirtual;
                    return _getter.IsVirtual;
                }
                if (_setter != null)
                    return _setter.IsVirtual;
                return false;
                
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property is final (can not be overriden)
        /// </summary>
        public bool IsFinal
        {
            get
            {
                if (_getter != null)
                {
                    if (_setter != null)
                        return _getter.IsFinal || _setter.IsFinal;
                    return _getter.IsFinal;
                }
                if (_setter != null)
                    return _setter.IsFinal;
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether property overrides return type.
        /// </summary>
        public bool IsNewSlot
        {
            get
            {
                if (_getter != null)
                {
                    if (_setter != null)
                        return _getter.IsNewSlot || _setter.IsNewSlot;
                    return _getter.IsNewSlot;
                }
                if (_setter != null)
                    return _setter.IsNewSlot;
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the property overrides implementation of base type.
        /// </summary>
        public bool IsOverride
        {
            get
            {
                if (_getter != null)
                {
                    if (_setter != null)
                        return _getter.IsOverride || _setter.IsOverride;
                    return _getter.IsOverride;
                }
                if (_setter != null)
                    return _setter.IsOverride;
                return false;
            }
        }

        public override bool IsStatic
        {
            get
            {
                if (_getter != null)
                    return _getter.IsStatic;
                if (_setter != null)
                    return _setter.IsStatic;
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public IParameterCollection Parameters
        {
            get { return _parameters; }
        }
        readonly ParameterCollection _parameters = new ParameterCollection();

        public IMethod Getter
        {
            get { return _getter; }
            set
            {
                if (value != _getter)
                {
                    if (_getter != null)
                        _getter.Association = null;
                    _getter = value;
                    if (_getter != null)
                        _getter.Association = this;
                }
            }
        }
        private IMethod _getter;

        public IMethod Setter
        {
            get { return _setter; }
            set
            {
                if (value != _setter)
                {
                    if (_setter != null)
                        _setter.Association = null;
                    _setter = value;
                    if (_setter != null)
                        _setter.Association = this;
                }
            }
        }
        private IMethod _setter;

        public IExpression Initializer { get; set; }

        /// <summary>
        /// Returns true if the property is indexer.
        /// </summary>
        public bool IsIndexer
        {
            get { return _parameters.Count > 0; }
        }
        #endregion

	    public object Value { get; set; }

		protected override IType ResolveDeclaringType()
		{
			return _getter != null
				       ? _getter.DeclaringType
				       : (_setter != null ? _setter.DeclaringType : null);
		}
    }
}