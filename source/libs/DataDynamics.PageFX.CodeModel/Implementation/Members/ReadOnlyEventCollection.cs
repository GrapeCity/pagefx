using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    internal class ReadOnlyEventCollection : ReadOnlyList<IEvent>, IEventCollection
    {
        #region IEventCollection Members
        public IEvent this[string name]
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        #endregion

        #region IEnumerable Members
        public new IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Events; }
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

        internal void AddInternal(IEvent e)
        {
            list.Add(e);
        }
    }
}