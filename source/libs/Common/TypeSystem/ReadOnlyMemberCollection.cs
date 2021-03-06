using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    internal sealed class ReadOnlyMemberCollection : ITypeMemberCollection
    {
		private readonly IReadOnlyList<ITypeMember> _members;

        public ReadOnlyMemberCollection(IReadOnlyList<ITypeMember> members)
        {
        	_members = members;
        }

    	public int Count
        {
            get { return _members.Count; }
        }

        public ITypeMember this[int index]
        {
            get { return _members[index]; }
        }

        public void Add(ITypeMember member)
        {
            throw new NotSupportedException();
        }

    	public IEnumerator<ITypeMember> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

    	IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _members.Cast<ICodeNode>(); }
        }

    	public object Data { get; set; }
        
    	public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
    }
}