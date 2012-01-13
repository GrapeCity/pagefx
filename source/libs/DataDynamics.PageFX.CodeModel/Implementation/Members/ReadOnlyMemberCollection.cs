using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    internal class ReadOnlyMemberCollection : ITypeMemberCollection
    {
        private readonly List<ITypeMember> list = new List<ITypeMember>();

        internal void AddInternal(ITypeMember member)
        {
            list.Add(member);
        }

        #region ITypeMemberCollection Members
        public int Count
        {
            get { return list.Count; }
        }

        public ITypeMember this[int index]
        {
            get { return list[index]; }
        }

        public void Add(ITypeMember m)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region IEnumerable<ITypeMember> Members
        public IEnumerator<ITypeMember> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.TypeMembers; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(list); }
        }

        public object Tag
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion
    }
}