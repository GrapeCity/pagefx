using System;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public abstract class TypeMember : CustomAttributeProvider, ITypeMember, IXmlSerializationFeedback
    {
        #region ITypeMember Members
        /// <summary>
        /// Gets the assembly in which the member is declared.
        /// </summary>
        public IAssembly Assembly
        {
            get
            {
                var mod = Module;
                if (mod != null)
                    return mod.Assembly;
                return null;
            }
        }

        /// <summary>
        /// Gets the module in which the member is defined. 
        /// </summary>
        public virtual IModule Module
        {
            get
            {
                if (DeclaringType != null)
                    return DeclaringType.Module;
                return null;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public abstract TypeMemberType MemberType { get; }

        /// <summary>
        /// Gets or sets member name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the fullname of the member.
        /// </summary>
        public virtual string FullName
        {
            get 
            {
                var dt = DeclaringType;
                if (dt != null)
                    return dt.FullName + "." + Name;
                return Name;
            }
        }

        public virtual string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets or sets the type that declares this member.
        /// </summary>
        public IType DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the type of this member (for methods it's return type)
        /// </summary>
        public IType Type { get; set; }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        public virtual Visibility Visibility { get; set; }

        public bool IsVisible
        {
            get
            {
                if (DeclaringType != null)
                {
                    if (!DeclaringType.IsVisible)
                        return false;
                }
                switch(Visibility)
                {
                    case Visibility.Public:
                    case Visibility.NestedPublic:
                        return true;
                }
                return false;
            }
        }

        internal Modifiers Modifiers
        {
            get { return _mods; }
            set { _mods = value; }
        }
        Modifiers _mods;

        internal bool GetModifier(Modifiers mod)
        {
            return (_mods & mod) != 0;
        }

        internal void SetModifier(bool value, Modifiers mod)
        {
            if (value) _mods |= mod;
            else _mods &= ~mod;
        }

        public virtual bool IsStatic
        {
            get { return GetModifier(Modifiers.Static); }
            set { SetModifier(value, Modifiers.Static); }
        }

        public bool IsSpecialName
        {
            get { return GetModifier(Modifiers.SpecialName); }
            set { SetModifier(value, Modifiers.SpecialName); }
        }

        public bool IsRuntimeSpecialName
        {
            get { return GetModifier(Modifiers.RuntimeSpecialName); }
            set { SetModifier(value, Modifiers.RuntimeSpecialName); }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken { get; set; }
        #endregion

        #region ICodeNode Members

        public virtual CodeNodeType NodeType
        {
            get { return CodeNodeType.TypeMember; }
        }

        public virtual IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region IXmlSerializationFeedback Members
        public virtual string XmlElementName
        {
            get { return MemberType.ToString(); }
        }

        public virtual void WriteProperties(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("Visibility", Visibility.ToString());
        }
        #endregion

        #region IDocumentationProvider Members

    	/// <summary>
    	/// Gets or sets documentation of this member
    	/// </summary>
    	public string Documentation { get; set; }

    	#endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}