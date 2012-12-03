using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
    /// Represents metadata table Column.
    /// </summary>
    public sealed class MetadataColumn : ICloneable
    {
		/// <summary>
		/// Gets the table id of this column.
		/// </summary>
		public TableId TableId { get; internal set; }

		/// <summary>
		/// Gets the column name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the column index.
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Gets the column description.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// Gets the column type.
		/// </summary>
		public ColumnType Type { get; private set; }

		/// <summary>
		/// Gets the simple index.
		/// </summary>
		public TableId SimpleIndex { get; private set; }

		/// <summary>
		/// Gets the coded index.
		/// </summary>
		public CodedIndex CodedIndex { get; private set; }

		/// <summary>
		/// Gets the enum type for this column.
		/// </summary>
		public Type EnumType { get; private set; }

		internal MetadataColumn()
        {
        }

        internal MetadataColumn(int index, string name, ColumnType type, string desc)
        {
            Index = index;
            Name = name;
            Description = desc;
            Type = type;
        }

        internal MetadataColumn(int index, string name, ColumnType type, Type enumType, string desc)
        {
            Index = index;
            Name = name;
            Description = desc;
            Type = type;
            EnumType = enumType;
        }

        internal MetadataColumn(int index, string name, TableId simpleIndex, string desc)
        {
            Index = index;
            Name = name;
            Description = desc;
            Type = ColumnType.SimpleIndex;
            SimpleIndex = simpleIndex;
        }

        internal MetadataColumn(int index, string name, CodedIndex codedIndex, string desc)
        {
            Index = index;
            Name = name;
            Description = desc;
            CodedIndex = codedIndex;
            Type = ColumnType.CodedIndex;
        }

		object ICloneable.Clone()
        {
            return Clone();
        }

		public MetadataColumn Clone()
        {
        	return new MetadataColumn
        	       	{
        	       		TableId = TableId,
        	       		Name = Name,
        	       		Description = Description,
        	       		Type = Type,
        	       		EnumType = EnumType,
        	       		CodedIndex = CodedIndex,
        	       		SimpleIndex = SimpleIndex
        	       	};
        }

        /// <summary>
        /// Gets the column type name.
        /// </summary>
        public string TypeName
        {
            get
            {
                if (EnumType != null)
                    return EnumType.Name;
                switch (Type)
                {
                    case ColumnType.SimpleIndex:
                        return SimpleIndex.ToString();

                    case ColumnType.CodedIndex:
                        return CodedIndex.ToString();
                }
                return Type.ToString();
            }
        }

        public override string ToString()
        {
            return string.Format("Column({0} : {1})", Name, TypeName);
        }
    }

	/// <summary>
    /// List of <see cref="MetadataColumn"/>s.
    /// </summary>
    public sealed class MetadataColumnCollection : IReadOnlyList<MetadataColumn>
    {
        private readonly Hashtable _hash = new Hashtable();
        private readonly List<MetadataColumn> _list = new List<MetadataColumn>();

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        public MetadataColumn this[int index]
        {
            get { return _list[index]; }
        }

        public MetadataColumn this[string name]
        {
            get { return (MetadataColumn)_hash[name]; }
        }

		public IEnumerator<MetadataColumn> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

		internal void Add(MetadataColumn col)
        {
            if (this[col.Name] != null)
                throw new ArgumentException(string.Format("Column with name {0} already exists", col.Name));
            _list.Add(col);
            _hash[col.Name] = col;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Columns(");
            for (int i = 0; i < _list.Count; ++i)
            {
                if (i > 0) s.Append(", ");
                s.Append(_list[i].Name);
            }
            s.Append(")");
            return base.ToString();
        }
    }
}