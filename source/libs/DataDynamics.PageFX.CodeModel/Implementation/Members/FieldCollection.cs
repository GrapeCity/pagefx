using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class FieldCollection : IFieldCollection
    {
    	private readonly HashList<string, IField> _list = new HashList<string, IField>(x => x.Name);
		private readonly IType _owner;

        public FieldCollection(IType owner)
        {
            _owner = owner;
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public IField this[int index]
        {
            get { return _list[index]; }
        }

    	public void Add(IField field)
        {
    		if (field == null) throw new ArgumentNullException("field");

    		field.DeclaringType = _owner;
            _list.Add(field);
        }

        public IField this[string name]
        {
            get { return _list[name]; }
        }

    	public IEnumerator<IField> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    	public CodeNodeType NodeType
        {
            get { return CodeNodeType.Methods; }
        }

        public object Tag { get; set; }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _list.Cast<ICodeNode>(); }
        }

    	public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

    	public static readonly IFieldCollection Empty = new EmptyImpl();

		private sealed class EmptyImpl : IFieldCollection
		{
			public int Count
			{
				get { return 0; }
			}

			public IField this[int index]
			{
				get { return null; }
			}

			public void Add(IField field)
			{
			}

			public IField this[string name]
			{
				get { return null; }
			}

			public CodeNodeType NodeType
			{
				get { return CodeNodeType.Fields; }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return this.Cast<ICodeNode>(); }
			}

			public object Tag { get; set; }

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return "";
			}

			public IEnumerator<IField> GetEnumerator()
			{
				return Enumerable.Empty<IField>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
    }
}