using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    internal class ReadOnlyTypeCollection : ReadOnlyList<IType>, ITypeCollection
    {
        #region ITypeCollection Members
        public IType this[string fullname]
        {
            get 
            {
                return Algorithms.Find(list, t => t.FullName == fullname);
            }
        }

        public void Sort()
        {
            //list.Sort((x, y) => string.Compare(x.FullName, y.FullName));
            throw new NotSupportedException();
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Types; }
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

        public void AddInternal(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            list.Add(type);
        }
    }
}