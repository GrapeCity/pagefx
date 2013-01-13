using System;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
    /// Represents metadata table Column.
    /// </summary>
    internal sealed class MetadataColumn : ICloneable
    {
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
		/// Gets the simple row index to some table.
		/// </summary>
		public TableId SimpleIndex { get; private set; }

		/// <summary>
		/// Gets the coded row index (tableId, rowIndex).
		/// </summary>
		public CodedIndex CodedIndex { get; private set; }

		/// <summary>
		/// Gets the enum type for this column.
		/// </summary>
		public Type EnumType { get; private set; }

		/// <summary>
		/// Specifies size of column in bytes.
		/// </summary>
		public int Size { get; set; }

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
        	       		Name = Name,
        	       		Description = Description,
        	       		Type = Type,
        	       		EnumType = EnumType,
        	       		CodedIndex = CodedIndex,
        	       		SimpleIndex = SimpleIndex,
						Size = Size
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
}