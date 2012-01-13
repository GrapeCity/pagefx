using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public class EventProxy : IEvent
    {
        private readonly IGenericInstance _instance;
        private readonly IEvent _event;
        private readonly IMethod _adder;
        private readonly IMethod _remover;
        private readonly IMethod _raiser;

        public EventProxy(IGenericInstance instance, IEvent e,
            IMethod adder, IMethod remover, IMethod raiser)
        {
            _instance = instance;
            _event = e;

            _adder = adder;
            _remover = remover;
            _raiser = raiser;

            if (adder != null)
                adder.Association = this;
            if (remover != null)
                remover.Association = this;
            if (raiser != null)
                raiser.Association = this;
        }

        #region IEvent Members
        public IMethod Adder
        {
            get { return _adder; }
            set { throw new NotSupportedException(); }
        }

        public IMethod Remover
        {
            get { return _remover; }
            set { throw new NotSupportedException();}
        }

        public IMethod Raiser
        {
            get { return _raiser; }
            set { throw new NotSupportedException(); }
        }

        public bool IsFlash
        {
            get;
            set;
        }
        #endregion

        #region ITypeMember Members
        public IAssembly Assembly
        {
            get { return _event.Assembly; }
        }

        public IModule Module
        {
            get { return _event.Module; }
            set { throw new NotSupportedException(); }
        }

        public TypeMemberType MemberType
        {
            get { return TypeMemberType.Event; }
        }

        public string Name
        {
            get { return _event.Name; }
            set { throw new NotSupportedException(); }
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

        public IType Type
        {
            get;
            set;
        }

        public Visibility Visibility
        {
            get { return _event.Visibility; }
            set { throw new NotSupportedException(); }
        }

        public bool IsVisible
        {
            get { return _event.IsVisible; }
        }

        public bool IsStatic
        {
            get { return _event.IsStatic; }
            set { throw new NotSupportedException(); }
        }

        public bool IsSpecialName
        {
            get { return _event.IsSpecialName; }
            set { throw new NotSupportedException(); }
        }

        public bool IsRuntimeSpecialName
        {
            get { return _event.IsRuntimeSpecialName; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return _event.MetadataToken; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region ICustomAttributeProvider Members
        public ICustomAttributeCollection CustomAttributes
        {
            get { return _event.CustomAttributes; }
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Event; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.EmptyCodeNodes; }
        }

        public object Tag
        {
            get;
            set;
        }
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
            get { return _event.Documentation; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}