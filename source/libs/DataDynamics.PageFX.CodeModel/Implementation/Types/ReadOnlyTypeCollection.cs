using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    internal class ReadOnlyTypeCollection : ReadOnlyList<IType>, ITypeCollection
    {
    	public IType this[string fullname]
        {
            get 
            {
                return List.FirstOrDefault(t => t.FullName == fullname);
            }
        }

    	public void Add(IType type)
    	{
    		throw new NotSupportedException("This collection is readonly.");
    	}

    	public void Sort()
        {
            //list.Sort((x, y) => string.Compare(x.FullName, y.FullName));
            throw new NotSupportedException();
        }

    	public CodeNodeType NodeType
        {
            get { return CodeNodeType.Types; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return List.Cast<ICodeNode>(); }
        }

        public object Tag
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

    	public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

    	public void AddInternal(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            List.Add(type);
        }
    }
}